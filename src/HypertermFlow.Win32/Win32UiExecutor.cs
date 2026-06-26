using System;
using System.Windows.Forms;
using HypertermFlow.Core.Abstractions;
using HypertermFlow.Core.Geometry;

namespace HypertermFlow.Win32
{
    /// <summary>
    /// Implementacion real de IUiExecutor sobre Win32 (P/Invoke). Solo corre en
    /// Windows. El click se emite con SendInput (nivel sistema), que es lo mas
    /// fiable contra UI ajena como Phoenix.
    /// </summary>
    public sealed class Win32UiExecutor : IUiExecutor
    {
        public ScreenSize GetScreenSize()
        {
            int w = NativeMethods.GetSystemMetrics(NativeMethods.SM_CXSCREEN);
            int h = NativeMethods.GetSystemMetrics(NativeMethods.SM_CYSCREEN);
            return new ScreenSize(w, h);
        }

        public void MoveCursorTo(int x, int y)
        {
            NativeMethods.SetCursorPos(x, y);
        }

        public void IndicatePointerFocus(int x, int y)
        {
            // Apoyo visual estilo "Ctrl" de Win10: XP no lo trae nativo.
            // Para esta prueba reposicionamos el cursor; el overlay GDI concentrico
            // es una mejora futura (ver daemon-sequency-test.md).
            NativeMethods.SetCursorPos(x, y);
        }

        public void ClickLeft(int x, int y)
        {
            // Aseguramos posicion y emitimos DOWN + UP del boton izquierdo.
            NativeMethods.SetCursorPos(x, y);

            NativeMethods.INPUT[] inputs = new NativeMethods.INPUT[2];

            inputs[0].type = NativeMethods.INPUT_MOUSE;
            inputs[0].mi.dwFlags = NativeMethods.MOUSEEVENTF_LEFTDOWN;

            inputs[1].type = NativeMethods.INPUT_MOUSE;
            inputs[1].mi.dwFlags = NativeMethods.MOUSEEVENTF_LEFTUP;

            int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(NativeMethods.INPUT));
            NativeMethods.SendInput(2, inputs, size);
        }

        public void ShowMessage(string text)
        {
            MessageBox.Show(text, "HypertermFlow",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
