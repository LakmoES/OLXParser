using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLXParser.DataBaseConnection
{
    public interface IDBProcessor
    {
        IDbConnection Connection { get; }
        void OpenConnection();
        void CloseConnection();
    }
}
