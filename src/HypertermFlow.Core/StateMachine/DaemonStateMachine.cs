using System;
using HypertermFlow.Core.Abstractions;
using HypertermFlow.Core.Geometry;
using HypertermFlow.Core.Logging;

namespace HypertermFlow.Core.StateMachine
{
    /// <summary>
    /// Motor de estados determinista. Idle --F9--> Executing recorre los pasos
    /// (MoveCursorTo -> IndicatePointerFocus -> ClickLeft -> espera waitMs) y al
    /// terminar pasa a Done mostrando el mensaje. Antirebote: ignora F9 mientras
    /// no este en Idle. Tras Done vuelve a Idle para permitir re-ejecutar.
    /// </summary>
    public sealed class DaemonStateMachine
    {
        private readonly object _gate = new object();
        private readonly IUiExecutor _ui;
        private readonly IClock _clock;
        private readonly ITraceLog _log;
        private readonly SequenceDefinition _sequence;

        private DaemonState _state = DaemonState.Idle;
        private int _stepIndex;

        public DaemonStateMachine(IUiExecutor ui, IClock clock, ITraceLog log, SequenceDefinition sequence)
        {
            if (ui == null) throw new ArgumentNullException("ui");
            if (clock == null) throw new ArgumentNullException("clock");
            if (log == null) throw new ArgumentNullException("log");
            if (sequence == null) throw new ArgumentNullException("sequence");
            _ui = ui;
            _clock = clock;
            _log = log;
            _sequence = sequence;
        }

        public DaemonState State
        {
            get { lock (_gate) { return _state; } }
        }

        /// <summary>Handler para ISignalSource.HotkeyPressed.</summary>
        public void OnHotkey(object sender, EventArgs e)
        {
            lock (_gate)
            {
                if (_state != DaemonState.Idle)
                {
                    _log.Write("FSM", "F9 ignorado (estado=" + _state + ")");
                    return;
                }
                _state = DaemonState.Executing;
                _stepIndex = 0;
                _log.Write("FSM", "Idle -> Executing (F9)");
            }
            ExecuteCurrentStep();
        }

        private void ExecuteCurrentStep()
        {
            int index;
            lock (_gate)
            {
                if (_state != DaemonState.Executing) return;
                index = _stepIndex;
            }

            if (index >= _sequence.Steps.Count)
            {
                Complete();
                return;
            }

            SequenceStep step = _sequence.Steps[index];
            try
            {
                ScreenSize size = _ui.GetScreenSize();
                ScreenPoint p = CoordinateResolver.Resolve(step.Region, size);

                _ui.MoveCursorTo(p.X, p.Y);
                _ui.IndicatePointerFocus(p.X, p.Y);
                _ui.ClickLeft(p.X, p.Y);

                _log.Write("STEP", "[" + index + "] " + step.Region + " -> click " + p
                                   + " (espera " + step.WaitMs + "ms)");
            }
            catch (Exception ex)
            {
                Fault(ex);
                return;
            }

            _clock.Schedule(step.WaitMs, OnStepTimerElapsed);
        }

        private void OnStepTimerElapsed()
        {
            lock (_gate)
            {
                if (_state != DaemonState.Executing) return;
                _stepIndex++;
            }
            ExecuteCurrentStep();
        }

        private void Complete()
        {
            lock (_gate) { _state = DaemonState.Done; }
            _log.Write("FSM", "Executing -> Done");
            try
            {
                _ui.ShowMessage(_sequence.CompletionMessage);
            }
            catch (Exception ex)
            {
                Fault(ex);
                return;
            }
            lock (_gate) { _state = DaemonState.Idle; }
            _log.Write("FSM", "Done -> Idle (listo para re-ejecutar)");
        }

        private void Fault(Exception ex)
        {
            lock (_gate) { _state = DaemonState.Error; }
            _log.Write("ERROR", ex.GetType().Name + ": " + ex.Message);
        }
    }
}
