using System;
using System.Windows.Forms;

namespace PokedexTracker
{
    static class Program
    {
        // The main entry point for the application
        [STAThread]  // This attribute is important for Windows Forms applications
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());  // Make sure to create an instance of your MainForm
        }
    }
}
