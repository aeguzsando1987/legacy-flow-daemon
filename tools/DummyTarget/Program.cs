using System;
using System.Windows.Forms;

namespace DummyTarget
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new DummyForm());
        }
    }
}
