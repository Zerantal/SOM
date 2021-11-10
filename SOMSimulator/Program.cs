using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SOMSimulator
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.7
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);         
            Application.Run(new MainForm());
        }
    }
}