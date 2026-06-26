using System.Drawing;
using System.Windows.Forms;
using HypertermFlow.Core;

namespace HypertermFlow.App
{
    /// <summary>
    /// Ventana minima que indica que el daemon esta activo y mantiene vivo el
    /// message loop (necesario para recibir F9). Cerrarla termina el programa.
    /// </summary>
    public sealed class StatusForm : Form
    {
        public StatusForm(RunMode mode)
        {
            Text = "HypertermFlow";
            Width = 440;
            Height = 170;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Label info = new Label();
            info.Dock = DockStyle.Fill;
            info.TextAlign = ContentAlignment.MiddleCenter;
            info.Text = "HypertermFlow  -  modo " + mode + "\n\n"
                      + "Pulsa F9 para ejecutar la secuencia de prueba.\n"
                      + "Cierra esta ventana para salir.";
            Controls.Add(info);
        }
    }
}
