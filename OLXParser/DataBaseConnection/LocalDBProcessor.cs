using OLXParser.DBEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace OLXParser.DataBaseConnection
{
    public class LocalDBProcessor : IDBProcessor
    {
        private SqlConnection sqlConnection { set; get; }
        public LocalDBProcessor(string connectionString)
        {
            sqlConnection = new SqlConnection(connectionString);
        }
        public IDbConnection Connection
        {
            get { return sqlConnection; }
        }
        public void OpenConnection()
        {
            sqlConnection.Open();
        }
        public void CloseConnection()
        {
            sqlConnection.Close();
        }
    }
}
