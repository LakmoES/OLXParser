using OLXParser.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace OLXParser
{
    public class FilesProcessor
    {
        private string path { get; set; }
        public FilesProcessor(string path)
        {
            this.path = path;
        }
        public void Save(Advert advert, List<Image> images, out string error)
        {
            error = null;
            string newDirectoryPath = null;
            try
            {
                string validDirName = advert.title.Replace("\\", "").Replace("/","").Replace(":", "").Replace("*","").Replace("!", "").Replace("\"", "").Replace("<", "").Replace(">", "").Replace("|", "");
                newDirectoryPath = Path.Combine(path, validDirName);
                var newDirectory = Directory.CreateDirectory(newDirectoryPath);
                File.WriteAllText(Path.Combine(newDirectoryPath, "text.txt"), GenerateTXT(advert));

            }
            catch { error = "Произошла ошибка при записи файла/папки"; }
            if (newDirectoryPath == null)
                return;

            try
            {
                foreach (var image in images)
                    SaveImage(image, newDirectoryPath);
            }
            catch {
                if (error != null)
                    error += "\r\nОшибка при загрузке изображения";
                else
                    error = "Ошибка при загрузке изображения";
            }
        }
        private string GenerateTXT(Advert advert)
        {
            return advert.ToStringWithoutUrl();
        }
        private void SaveImage(Image image, string path)
        {
            WebClient client = new WebClient();
            Uri uri = new Uri(image.sourceurl);
            //if (uri.IsFile)
            //{
                string filename = System.IO.Path.GetFileName(uri.LocalPath);
            //}
            client.DownloadFile(uri, Path.Combine(path, filename));
        }
    }
}
