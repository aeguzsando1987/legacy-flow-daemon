using System;
using System.Collections.Generic;
using HypertermFlow.Core.Abstractions;

namespace HypertermFlow.Core.Tests
{
    /// <summary>
    /// Reloj falso y determinista para tests: no espera tiempo real. Guarda los
    /// callbacks programados y el test los dispara con DrainAll(), simulando que
    /// el tiempo avanza. Tambien registra los delays pedidos para verificarlos.
    /// </summary>
    public sealed class FakeClock : IClock
    {
        private readonly Queue<ScheduledCallback> _pending = new Queue<ScheduledCallback>();
        private readonly List<int> _scheduledDelays = new List<int>();
        private DateTime _now = new DateTime(2026, 1, 1, 0, 0, 0);

        public DateTime Now { get { return _now; } }

        public IList<int> ScheduledDelays { get { return _scheduledDelays; } }

        public int PendingCount { get { return _pending.Count; } }

        public void Schedule(int delayMs, ScheduledCallback callback)
        {
            _scheduledDelays.Add(delayMs);
            _pending.Enqueue(callback);
        }

        /// <summary>
        /// Dispara todos los callbacks pendientes en orden. Como cada callback
        /// puede programar el siguiente (la FSM encadena pasos), el bucle vacia
        /// la cola hasta que la secuencia completa termina.
        /// </summary>
        public void DrainAll()
        {
            while (_pending.Count > 0)
            {
                ScheduledCallback cb = _pending.Dequeue();
                _now = _now.AddMilliseconds(1);
                cb();
            }
        }
    }
}
