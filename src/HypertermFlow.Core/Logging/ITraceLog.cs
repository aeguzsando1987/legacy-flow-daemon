namespace HypertermFlow.Core.Logging
{
    /// <summary>Trazabilidad append-only: cada accion y transicion se registra.</summary>
    public interface ITraceLog
    {
        void Write(string category, string message);
    }
}
