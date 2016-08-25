using OLXParser.DataBaseConnection;
using OLXParser.DBEntities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;

namespace OLXParser.TableRepositories
{
    public class AdvertRepository
    {
        private IDBProcessor dbp;
        public AdvertRepository(IDBProcessor dbp)
        {
            this.dbp = dbp;
        }

        public List<Advert> GetAdverts()
        {
            List<Advert> adverts = new List<Advert>();

            SqlCommand queryCommand = new SqlCommand("SELECT * FROM Advert", dbp.Connection as SqlConnection);
            var advertTableReader = queryCommand.ExecuteReader();

            if (advertTableReader.HasRows)
            {
                foreach (DbDataRecord dbDataRecord in advertTableReader)
                {
                    //Application.DoEvents();
                    var advert = new Advert
                    {
                        id = Convert.ToInt32(dbDataRecord["id"]),
                        url = dbDataRecord["url"].ToString(),
                        title = dbDataRecord["title"].ToString(),
                        rooms = FromObjectToNullableInt(FixDBValue(dbDataRecord["rooms"])),
                        floor = FromObjectToNullableInt(FixDBValue(dbDataRecord["floor"])),
                        floors = FromObjectToNullableInt(FixDBValue(dbDataRecord["floors"])),
                        area = FromObjectToNullableInt(FixDBValue(dbDataRecord["area"])),
                        isnew = FromObjectToNullableInt(FixDBValue(dbDataRecord["isnew"])),
                        description = dbDataRecord["description"].ToString(),
                        cost = dbDataRecord["cost"].ToString(),
                        township = dbDataRecord["township"].ToString()
                    };
                    adverts.Add(advert);
                }
            }
            advertTableReader.Close();
            return adverts;
        }
        public Advert GetConcreteAdvert(int id)
        {
            Advert advert = null;

            SqlCommand queryCommand = new SqlCommand("SELECT * FROM Advert WHERE \"id\" = @id", dbp.Connection as SqlConnection);
            queryCommand.Parameters.AddWithValue("@id", id);
            var advertTableReader = queryCommand.ExecuteReader();

            if (advertTableReader.HasRows)
                foreach (DbDataRecord dbDataRecord in advertTableReader)
                {
                    //Application.DoEvents();
                    advert = new Advert
                    {
                        id = Convert.ToInt32(dbDataRecord["id"]),
                        url = dbDataRecord["url"].ToString(),
                        title = dbDataRecord["title"].ToString(),
                        rooms = FromObjectToNullableInt(FixDBValue(dbDataRecord["rooms"])),
                        floor = FromObjectToNullableInt(FixDBValue(dbDataRecord["floor"])),
                        floors = FromObjectToNullableInt(FixDBValue(dbDataRecord["floors"])),
                        area = FromObjectToNullableInt(FixDBValue(dbDataRecord["area"])),
                        isnew = FromObjectToNullableInt(FixDBValue(dbDataRecord["isnew"])),
                        description = dbDataRecord["description"].ToString(),
                        cost = dbDataRecord["cost"].ToString(),
                        township = dbDataRecord["township"].ToString()
                    };
                    break;
                }
            advertTableReader.Close();

            return advert;
        }
        public int AddAdvert(Advert advert)
        {
            SqlCommand queryCommand = new SqlCommand("INSERT Advert (\"url\", \"title\", \"rooms\", \"floor\", \"floors\", \"area\", \"isnew\", \"description\", \"cost\", \"township\")" +
                " output INSERTED.ID VALUES(@url, @title, @rooms, @floor, @floors, @area, @isnew, @description, @cost, @township)", dbp.Connection as SqlConnection);
            queryCommand.Parameters.AddWithValue("@url", advert.url);
            queryCommand.Parameters.AddWithValue("@title", advert.title);
            queryCommand.Parameters.AddWithValue("@rooms", (advert.rooms ?? (object)DBNull.Value));
            queryCommand.Parameters.AddWithValue("@floor", (advert.floor ?? (object)DBNull.Value));
            queryCommand.Parameters.AddWithValue("@floors", (advert.floors ?? (object)DBNull.Value));
            queryCommand.Parameters.AddWithValue("@area", (advert.area ?? (object)DBNull.Value));
            queryCommand.Parameters.AddWithValue("@isnew", (advert.isnew ?? (object)DBNull.Value));
            queryCommand.Parameters.AddWithValue("@description", (advert.description ?? (object)DBNull.Value));
            queryCommand.Parameters.AddWithValue("@cost", (advert.cost ?? (object)DBNull.Value));
            queryCommand.Parameters.AddWithValue("@township", (advert.township ?? (object)DBNull.Value));
            //queryCommand.ExecuteNonQuery();
            int modified = (int)queryCommand.ExecuteScalar();
            return modified;
        }
        public int ClearAdvert()
        {
            SqlCommand queryCommand = new SqlCommand("DELETE FROM Advert", dbp.Connection as SqlConnection);
            int rows = queryCommand.ExecuteNonQuery();
            return rows;
        }
        public int NumberOfAdverts()
        {
            SqlCommand queryCommand = new SqlCommand("SELECT COUNT(*) FROM Advert", dbp.Connection as SqlConnection);
            int rows = (int)queryCommand.ExecuteScalar();
            return rows;
        }
        private object FixDBValue(object value)
        {
            if (value is DBNull)
                return null;
            return value;
        }
        private int? FromObjectToNullableInt(object value)
        {
            if (value == null)
                return null;
            return Convert.ToInt32(value);
        }
    }
}
