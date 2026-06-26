using HypertermFlow.Core.Geometry;

namespace HypertermFlow.Core.Abstractions
{
    /// <summary>
    /// Interface para automatizacion de UI. Capa portable:
    /// si el CNC tuviera un SP < 3, solo se reescribe esta capa en C++.
    /// </summary>
    public interface IUiExecutor
    {
        ScreenSize GetScreenSize();
        void MoveCursorTo(int x, int y);
        void IndicatePointerFocus(int x, int y);
        void ClickLeft(int x, int y); // Evento de click de mouse en la coordenada de la region
        void ShowMessage(string text);
    }
}
