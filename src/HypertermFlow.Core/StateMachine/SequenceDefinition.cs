using System.Collections.Generic;
using HypertermFlow.Core.Geometry;

namespace HypertermFlow.Core.StateMachine
{
    /// <summary>
    /// Secuencia de prueba:
    /// F9 -> 
    /// click sup.izq /
    /// click sup.der / 
    /// click inf.izq / 
    /// click inf.der /
    /// click centro  /
    /// mensaje "fin de secuencia".
    /// esperar (1000ms c/paso)
    /// </summary>
    public sealed class SequenceDefinition
    {
        public readonly IList<SequenceStep> Steps;
        public readonly string CompletionMessage;

        public SequenceDefinition(IList<SequenceStep> steps, string completionMessage)
        {
            Steps = steps;
            CompletionMessage = completionMessage;
        }

        public static SequenceDefinition CreateDefaultTest()
        {
            List<SequenceStep> steps = new List<SequenceStep>();
            steps.Add(new SequenceStep(ScreenRegion.TopLeft, 1000));
            steps.Add(new SequenceStep(ScreenRegion.TopRight, 1000));
            steps.Add(new SequenceStep(ScreenRegion.BottomLeft, 1000));
            steps.Add(new SequenceStep(ScreenRegion.BottomRight, 1000));
            steps.Add(new SequenceStep(ScreenRegion.Center, 100));
            return new SequenceDefinition(steps, "fin de secuencia");
        }
    }
}
