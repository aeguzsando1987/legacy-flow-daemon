using System;
using System.Drawing;
using System.Windows.Forms;

namespace DummyTarget
{
    /// <summary>
    /// Blanco de clicks a pantalla completa. Coloca 5 zonas en las mismas
    /// coordenadas que calcula el daemon (25/75/50 %). Cada zona registra los
    /// clicks recibidos: evidencia objetiva de que el evento click real ocurrio.
    /// </summary>
    public sealed class DummyForm : Form
    {
        private int _total;
        private readonly Label _header;

        public DummyForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            Bounds = Screen.PrimaryScreen.Bounds;
            BackColor = Color.White;

            _header = new Label();
            _header.Height = 40;
            _header.Dock = DockStyle.Top;
            _header.TextAlign = ContentAlignment.MiddleCenter;
            _header.Font = new Font("Arial", 12, FontStyle.Bold);
            _header.Text = "DummyTarget - blanco de clicks. ESC para salir. Clicks: 0";
            Controls.Add(_header);

            CreateZone("TopLeft", 0.25, 0.25);
            CreateZone("TopRight", 0.75, 0.25);
            CreateZone("BottomLeft", 0.25, 0.75);
            CreateZone("BottomRight", 0.75, 0.75);
            CreateZone("Center", 0.50, 0.50);

            KeyPreview = true;
            KeyDown += delegate(object s, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Escape) Close();
            };
        }

        private void CreateZone(string name, double fx, double fy)
        {
            int w = Screen.PrimaryScreen.Bounds.Width;
            int h = Screen.PrimaryScreen.Bounds.Height;
            int cx = (int)((w - 1) * fx);
            int cy = (int)((h - 1) * fy);

            const int size = 140;
            int count = 0;

            Label zone = new Label();
            zone.Width = size;
            zone.Height = size;
            zone.Left = cx - size / 2;
            zone.Top = cy - size / 2;
            zone.BorderStyle = BorderStyle.FixedSingle;
            zone.BackColor = Color.LightGray;
            zone.TextAlign = ContentAlignment.MiddleCenter;
            zone.Text = name + "\n(" + cx + "," + cy + ")\nclicks: 0";

            zone.Click += delegate(object s, EventArgs e)
            {
                count++;
                _total++;
                zone.BackColor = Color.LightGreen;
                zone.Text = name + "\n(" + cx + "," + cy + ")\nclicks: " + count;
                _header.Text = "DummyTarget - blanco de clicks. ESC para salir. Clicks: " + _total;
            };

            Controls.Add(zone);
            zone.BringToFront();
        }
    }
}
