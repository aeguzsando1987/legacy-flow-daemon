using System;
using System.Threading;
using HypertermFlow.Core.Abstractions;

namespace HypertermFlow.App
{
    /// <summary>
    /// Reloj de produccion: implementa IClock con System.Threading.Timer (one-shot).
    /// Es el equivalente real del FakeClock de los tests; la FSM no nota la diferencia.
    /// </summary>
    public sealed class SystemClock : IClock
    {
        public DateTime Now { get { return DateTime.Now; } }

        public void Schedule(int delayMs, ScheduledCallback callback)
        {
            Timer timer = null;
            timer = new Timer(delegate(object state)
            {
                try { callback(); }
                finally { if (timer != null) timer.Dispose(); }
            }, null, delayMs, Timeout.Infinite);
        }
    }
}
