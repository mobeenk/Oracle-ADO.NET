using System;
using System.Windows.Forms;

namespace AlamiahDailycheckDashboard
{
    static class Program
    {
   //     static Label b;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //hjkjk
            //comment 2
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           // Application.Run(new Form1());
           Application.Run(new login());
        }
    }
}
