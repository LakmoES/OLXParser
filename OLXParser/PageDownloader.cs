using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OLXParser
{
    public static class PageDownloader
    {
        public static string Download(string url)
        {
            WebClient client = new WebClient();
            client.Encoding = ASCIIEncoding.UTF8;
            return client.DownloadString(new Uri(url));
        }
    }
}
