using HypertermFlow.Core.Geometry;

namespace HypertermFlow.Core.StateMachine
{
    /// <summary>
    /// Paso: click una region y luego esperar waitMs.
    /// </summary>
    public sealed class SequenceStep
    {
        public readonly ScreenRegion Region;
        public readonly int WaitMs;

        public SequenceStep(ScreenRegion region, int waitMs)
        {
            Region = region;
            WaitMs = waitMs;
        }
    }
}
