using System;

namespace HypertermFlow.Core.Abstractions
{
    // <summary>
    // Fuente de eventos externos que disparan el daemon.
    // Primero se prueba con F9
    // Mas a futuro tambien se inlcuye señal ON/OFF de controlador
    // </summary>
    public interface ISignalSource
    {
        event EventHandler HotkeyPressed;
        void Start();
        void Stop();
    }
}