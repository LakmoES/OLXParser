using OLXParser.DataBaseConnection;
using System;
using System.Windows.Forms;
using Core;

namespace OLXParser
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var dbProcessor = Initializer.Init();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain(dbProcessor));

            Initializer.Destroy();
        }
    }
}
