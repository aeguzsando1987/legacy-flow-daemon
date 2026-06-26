using System;

namespace HypertermFlow.Core.Abstractions
{
    /// <summary>
    /// Callback
    /// Pausa para eventos
    /// </summary>
    public delegate void ScheduledCallback();

    /// <summary>
    /// temporizador
    /// para pruebas el reloj es emulado
    /// </summary>
    public interface IClock
    {
        DateTime Now { get; }
        void Schedule(int delayMs, ScheduledCallback callback);
    }
}