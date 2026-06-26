using System;
using System.Runtime.InteropServices;

namespace HypertermFlow.Win32
{
    /// <summary>
    /// Declaraciones P/Invoke a Win32. Centralizadas aqui. El layout de las
    /// estructuras (INPUT/MOUSEINPUT) debe ser EXACTO o SendInput falla en silencio.
    /// </summary>
    internal static class NativeMethods
    {
        // ---- Cursor ----
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetCursorPos(int x, int y);

        // ---- Metricas de pantalla ----
        [DllImport("user32.dll")]
        internal static extern int GetSystemMetrics(int nIndex);

        internal const int SM_CXSCREEN = 0;
        internal const int SM_CYSCREEN = 1;

        // ---- Hotkey global ----
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        internal const int WM_HOTKEY = 0x0312;
        internal const uint MOD_NONE = 0x0000;
        internal const uint VK_F9 = 0x78;

        // ---- Inyeccion de input (click real) ----
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        internal const int INPUT_MOUSE = 0;
        internal const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        internal const uint MOUSEEVENTF_LEFTUP = 0x0004;

        [StructLayout(LayoutKind.Sequential)]
        internal struct INPUT
        {
            public int type;
            public MOUSEINPUT mi;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
    }
}
