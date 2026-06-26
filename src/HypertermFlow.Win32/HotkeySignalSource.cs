using System;
using System.Windows.Forms;
using HypertermFlow.Core.Abstractions;

namespace HypertermFlow.Win32
{
    /// <summary>
    /// Fuente de senal real: registra el hotkey global F9 (RegisterHotKey) y, al
    /// recibir WM_HOTKEY, dispara HotkeyPressed. Usa una ventana oculta
    /// (NativeWindow) para captar el mensaje sin UI visible.
    /// </summary>
    public sealed class HotkeySignalSource : ISignalSource, IDisposable
    {
        private const int HotkeyId = 1;
        private readonly MessageWindow _window;
        private bool _registered;

        public event EventHandler HotkeyPressed;

        public HotkeySignalSource()
        {
            _window = new MessageWindow(this);
        }

        public void Start()
        {
            if (_registered) return;
            bool ok = NativeMethods.RegisterHotKey(_window.Handle, HotkeyId,
                                                   NativeMethods.MOD_NONE, NativeMethods.VK_F9);
            if (!ok)
                throw new InvalidOperationException(
                    "No se pudo registrar el hotkey F9 (RegisterHotKey fallo).");
            _registered = true;
        }

        public void Stop()
        {
            if (!_registered) return;
            NativeMethods.UnregisterHotKey(_window.Handle, HotkeyId);
            _registered = false;
        }

        private void RaiseHotkey()
        {
            EventHandler h = HotkeyPressed;
            if (h != null) h(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            Stop();
            _window.DestroyHandle();
        }

        /// <summary>Ventana oculta que solo intercepta WM_HOTKEY.</summary>
        private sealed class MessageWindow : NativeWindow
        {
            private readonly HotkeySignalSource _owner;

            public MessageWindow(HotkeySignalSource owner)
            {
                _owner = owner;
                CreateHandle(new CreateParams());
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == NativeMethods.WM_HOTKEY
                    && m.WParam.ToInt32() == HotkeyId)
                {
                    _owner.RaiseHotkey();
                }
                base.WndProc(ref m);
            }
        }
    }
}
