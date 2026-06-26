using HypertermFlow.Core.Abstractions;
using HypertermFlow.Core.Geometry;
using HypertermFlow.Core.Logging;

namespace HypertermFlow.Core.Simulation
{
    /// <summary>
    /// Modo simulacion: implementa IUiExecutor logueando lo que HARIA, sin tocar
    /// la pantalla real. Permite validar la FSM en Ubuntu y en la maquina real
    /// sin riesgo. Usa una pantalla virtual fija para resolver coordenadas.
    /// </summary>
    public sealed class MockUiExecutor : IUiExecutor
    {
        private readonly ITraceLog _log;
        private readonly ScreenSize _virtualScreen;

        public MockUiExecutor(ITraceLog log, ScreenSize virtualScreen)
        {
            _log = log;
            _virtualScreen = virtualScreen;
        }

        public ScreenSize GetScreenSize() { return _virtualScreen; }

        public void MoveCursorTo(int x, int y)
        {
            _log.Write("SIM", "MoveCursorTo (" + x + "," + y + ")");
        }

        public void IndicatePointerFocus(int x, int y)
        {
            _log.Write("SIM", "IndicatePointerFocus (" + x + "," + y + ")");
        }

        public void ClickLeft(int x, int y)
        {
            _log.Write("SIM", "ClickLeft (" + x + "," + y + ")");
        }

        public void ShowMessage(string text)
        {
            _log.Write("SIM", "ShowMessage: " + text);
        }
    }
}
