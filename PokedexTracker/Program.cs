using System;
using System.Windows.Forms;
using PokedexTracker.Forms;

namespace PokedexTracker
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Check if player name exists
            string savedName = Properties.Settings.Default.playerName;

            if (!string.IsNullOrWhiteSpace(savedName))
            {
                // If player name exists, skip IntroForm and go to MainForm
                using (var mainForm = new MainForm(savedName))
                {
                    Application.Run(mainForm);
                }
            }
            else
            {
                // If no name is saved, run IntroForm
                using (var introForm = new IntroForm())
                {
                    Application.Run(introForm);
                }
            }
        }
    }
}
