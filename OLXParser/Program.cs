using OLXParser.DataBaseConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            IDBProcessor dbProcessor = new LocalDBProcessor("Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = |DataDirectory|\\DatabaseMain.mdf");
            //try
            //{
            dbProcessor.OpenConnection();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain(dbProcessor));
            //}
            //catch(Exception ex) { MessageBox.Show(ex.ToString()); }
            //finally
            //{
            //    dbProcessor.CloseConnection();
            //}
            dbProcessor.CloseConnection();
        }
    }
}
