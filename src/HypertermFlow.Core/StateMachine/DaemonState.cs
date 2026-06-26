namespace HypertermFlow.Core.StateMachine
{
    /// <summary>
    /// Estados del motor del daemon 
    /// </summary>
    public enum DaemonState
    {
        Idle,        // en reposo: esperando F9
        Executing,   // recorriendo la secuencia
        Done,        // secuencia completa (mostro mensaje)
        Error        // fallo en un evento
    }
}
