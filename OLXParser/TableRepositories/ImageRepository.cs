using OLXParser.DataBaseConnection;
using OLXParser.DBEntities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OLXParser.TableRepositories
{
    public class ImageRepository
    {
        private IDBProcessor dbp;
        public ImageRepository(IDBProcessor dbp)
        {
            this.dbp = dbp;
        }

        public List<Image> GetImages()
        {
            List<Image> images = new List<Image>();

            SqlCommand queryCommand = new SqlCommand("SELECT * FROM Image", dbp.Connection as SqlConnection);
            var imageTableReader = queryCommand.ExecuteReader();

            if (imageTableReader.HasRows)
            {
                foreach (DbDataRecord dbDataRecord in imageTableReader)
                {
                    Application.DoEvents();
                    var image = new Image
                    {
                        id = Convert.ToInt32(dbDataRecord["id"]),
                        sourceurl = dbDataRecord["sourceurl"].ToString(),
                        advertid = Convert.ToInt32(dbDataRecord["advertid"])
                    };
                    images.Add(image);
                }
            }
            imageTableReader.Close();
            return images;
        }
        public Image GetConcreteImage(int id)
        {
            Image image = null;

            SqlCommand queryCommand = new SqlCommand("SELECT * FROM Image WHERE \"id\" = @id", dbp.Connection as SqlConnection);
            queryCommand.Parameters.AddWithValue("@id", id);
            var imageTableReader = queryCommand.ExecuteReader();

            if (imageTableReader.HasRows)
                foreach (DbDataRecord dbDataRecord in imageTableReader)
                {
                    Application.DoEvents();
                    image = new Image
                    {
                        id = Convert.ToInt32(dbDataRecord["id"]),
                        sourceurl = dbDataRecord["sourceurl"].ToString(),
                        advertid = Convert.ToInt32(dbDataRecord["advertid"])
                    };
                    break;
                }
            imageTableReader.Close();

            return image;
        }
        public List<Image> GetImagesByAdvertID(int advertid)
        {
            Image image = null;
            List<Image> images = new List<Image>();

            SqlCommand queryCommand = new SqlCommand("SELECT * FROM Image WHERE \"advertid\" = @advertid", dbp.Connection as SqlConnection);
            queryCommand.Parameters.AddWithValue("@advertid", advertid);
            var imageTableReader = queryCommand.ExecuteReader();

            if (imageTableReader.HasRows)
                foreach (DbDataRecord dbDataRecord in imageTableReader)
                {
                    Application.DoEvents();
                    image = new Image
                    {
                        id = Convert.ToInt32(dbDataRecord["id"]),
                        sourceurl = dbDataRecord["sourceurl"].ToString(),
                        advertid = Convert.ToInt32(dbDataRecord["advertid"])
                    };
                    images.Add(image);
                }
            imageTableReader.Close();

            return images;
        }
        public void AddImage(Image image)
        {
            SqlCommand queryCommand = new SqlCommand("INSERT Image (\"sourceurl\", \"advertid\")" +
                " VALUES(@sourceurl, @advertid)", dbp.Connection as SqlConnection);
            queryCommand.Parameters.AddWithValue("@sourceurl", image.sourceurl);
            queryCommand.Parameters.AddWithValue("@advertid", image.advertid);
            
            queryCommand.ExecuteNonQuery();
        }
    }
}
