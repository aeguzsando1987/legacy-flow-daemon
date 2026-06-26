using System;
using HypertermFlow.Core.Abstractions;
using HypertermFlow.Core.Geometry;
using HypertermFlow.Core.Simulation;
using HypertermFlow.Core.StateMachine;

namespace HypertermFlow.Core.Tests
{
    /// <summary>
    /// Runner de tests del Core (modo simulacion, en Ubuntu, sin Win32).
    /// Valida la FSM ejecutando la secuencia de prueba con FakeClock + Mock.
    /// </summary>
    public static class Program
    {
        // Pantalla virtual fija usada en simulacion.
        private static readonly ScreenSize Screen = new ScreenSize(1024, 768);

        public static int Main()
        {
            Console.WriteLine("=== HypertermFlow.Core.Tests ===");

            Test_FullSequence_FiveClicksAndMessage();
            Test_ScheduledDelays_MatchSpec();
            Test_Debounce_SecondHotkeyIgnored();
            Test_EndsInIdle_AndCanRerun();
            Test_Coordinates_For1024x768();

            Console.WriteLine();
            Console.WriteLine("RESUMEN: " + MiniAssert.Passed + " PASS, "
                              + MiniAssert.Failed + " FAIL");
            return MiniAssert.Failed > 0 ? 1 : 0;
        }

        private static DaemonStateMachine Build(out FakeClock clock, out InMemoryTraceLog log)
        {
            clock = new FakeClock();
            log = new InMemoryTraceLog();
            IUiExecutor ui = new MockUiExecutor(log, Screen);
            SequenceDefinition seq = SequenceDefinition.CreateDefaultTest();
            return new DaemonStateMachine(ui, clock, log, seq);
        }

        private static void Test_FullSequence_FiveClicksAndMessage()
        {
            Console.WriteLine();
            Console.WriteLine("- Test: secuencia completa = 5 clicks + mensaje");
            FakeClock clock; InMemoryTraceLog log;
            DaemonStateMachine fsm = Build(out clock, out log);

            fsm.OnHotkey(null, EventArgs.Empty);
            clock.DrainAll();

            MiniAssert.AreEqual(5, log.Count("ClickLeft ("), "se emiten 5 clicks");
            MiniAssert.AreEqual(1, log.Count("ShowMessage: fin de secuencia"),
                                "se muestra el mensaje final");
        }

        private static void Test_ScheduledDelays_MatchSpec()
        {
            Console.WriteLine();
            Console.WriteLine("- Test: tiempos = [1000,1000,1000,1000,100]");
            FakeClock clock; InMemoryTraceLog log;
            DaemonStateMachine fsm = Build(out clock, out log);

            fsm.OnHotkey(null, EventArgs.Empty);
            clock.DrainAll();

            MiniAssert.AreEqual(5, clock.ScheduledDelays.Count, "se programan 5 esperas");
            int[] expected = new int[] { 1000, 1000, 1000, 1000, 100 };
            bool ok = clock.ScheduledDelays.Count == 5;
            for (int i = 0; ok && i < 5; i++)
                ok = clock.ScheduledDelays[i] == expected[i];
            MiniAssert.IsTrue(ok, "los delays coinciden con la spec");
        }

        private static void Test_Debounce_SecondHotkeyIgnored()
        {
            Console.WriteLine();
            Console.WriteLine("- Test: antirebote (F9 durante la corrida se ignora)");
            FakeClock clock; InMemoryTraceLog log;
            DaemonStateMachine fsm = Build(out clock, out log);

            fsm.OnHotkey(null, EventArgs.Empty);   // arranca (estado Executing)
            fsm.OnHotkey(null, EventArgs.Empty);   // debe ignorarse
            clock.DrainAll();

            MiniAssert.AreEqual(1, log.Count("F9 ignorado"), "el segundo F9 se ignora");
            MiniAssert.AreEqual(5, log.Count("ClickLeft ("),
                                "sigue habiendo solo 5 clicks (no se duplico)");
        }

        private static void Test_EndsInIdle_AndCanRerun()
        {
            Console.WriteLine();
            Console.WriteLine("- Test: termina en Idle y permite re-ejecutar");
            FakeClock clock; InMemoryTraceLog log;
            DaemonStateMachine fsm = Build(out clock, out log);

            fsm.OnHotkey(null, EventArgs.Empty);
            clock.DrainAll();
            MiniAssert.AreEqual(DaemonState.Idle, fsm.State, "tras la corrida vuelve a Idle");

            fsm.OnHotkey(null, EventArgs.Empty);   // segunda corrida
            clock.DrainAll();
            MiniAssert.AreEqual(10, log.Count("ClickLeft ("),
                                "dos corridas producen 10 clicks en total");
        }

        private static void Test_Coordinates_For1024x768()
        {
            Console.WriteLine();
            Console.WriteLine("- Test: coordenadas correctas para 1024x768");
            FakeClock clock; InMemoryTraceLog log;
            DaemonStateMachine fsm = Build(out clock, out log);

            fsm.OnHotkey(null, EventArgs.Empty);
            clock.DrainAll();

            System.Collections.Generic.List<string> coords = log.ClickCoordinates();
            string[] expected = new string[]
            {
                "(255,191)", // TopLeft     25% / 25%
                "(767,191)", // TopRight    75% / 25%
                "(255,575)", // BottomLeft  25% / 75%
                "(767,575)", // BottomRight 75% / 75%
                "(511,383)"  // Center      50% / 50%
            };
            bool ok = coords.Count == 5;
            for (int i = 0; ok && i < 5; i++)
                ok = coords[i] == expected[i];
            MiniAssert.IsTrue(ok, "las 5 coordenadas coinciden con 1024x768");
        }
    }
}
