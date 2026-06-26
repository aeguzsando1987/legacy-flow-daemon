namespace HypertermFlow.Core
{
    /// <summary>
    /// Modos de ejecucion del programa
    /// Simulation = MockUiExecutor (solo loguea, no toca pantalla).
    /// Production = Win32UiExecutor (click real). Se elige al arranque.
    /// </summary>
    public enum RunMode
    {
        Simulation,
        Production
    }
}
