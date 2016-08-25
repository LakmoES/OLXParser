using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OLXParser.DataBaseConnection;

namespace Core
{
    public static class Initializer
    {
        private static IDBProcessor dbProcessor;
        public static IDBProcessor Init()
        {

            dbProcessor =
                new LocalDBProcessor(
                    "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = |DataDirectory|\\DatabaseMain.mdf");

            try
            {
                dbProcessor.OpenConnection();
                return dbProcessor;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public static void Destroy()
        {
            dbProcessor?.CloseConnection();
            dbProcessor = null;
        }
    }
}
