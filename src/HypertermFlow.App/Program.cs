using System;
using System.Windows.Forms;
using HypertermFlow.Core;
using HypertermFlow.Core.Abstractions;
using HypertermFlow.Core.Geometry;
using HypertermFlow.Core.Logging;
using HypertermFlow.Core.Simulation;
using HypertermFlow.Core.StateMachine;
using HypertermFlow.Win32;

namespace HypertermFlow.App
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            RunMode mode = ParseMode(args);
            Application.EnableVisualStyles();

            IClock clock = new SystemClock();
            ITraceLog log = new FileTraceLog("hyperterm_flow.log", clock);

            // Seleccion de executor segun el modo (el corazon de sim vs prod).
            IUiExecutor executor;
            if (mode == RunMode.Simulation)
                executor = new MockUiExecutor(log, new ScreenSize(1024, 768));
            else
                executor = new Win32UiExecutor();

            SequenceDefinition sequence = SequenceDefinition.CreateDefaultTest();
            DaemonStateMachine fsm = new DaemonStateMachine(executor, clock, log, sequence);

            using (HotkeySignalSource signal = new HotkeySignalSource())
            {
                signal.HotkeyPressed += fsm.OnHotkey;
                signal.Start();
                log.Write("APP", "Iniciado en modo " + mode + ". F9 para ejecutar.");

                // Message loop: necesario para recibir WM_HOTKEY (F9 global).
                Application.Run(new StatusForm(mode));

                signal.Stop();
            }
        }

        private static RunMode ParseMode(string[] args)
        {
            foreach (string a in args)
            {
                if (a == "--mode=prod" || a == "--mode=production") return RunMode.Production;
                if (a == "--mode=sim" || a == "--mode=simulation") return RunMode.Simulation;
            }
            return RunMode.Simulation; // por defecto: el modo seguro
        }
    }
}
