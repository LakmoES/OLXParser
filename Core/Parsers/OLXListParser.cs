using CsQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLXParser.Parsers
{
    public class OLXListParser
    {
        public OLXListParser()
        {

        }
        public static List<string> GetHrefs(string url)
        {
            List<string> result = new List<string>();
            CQ cq = CQ.Create(PageDownloader.Download(url));
            foreach (IDomObject obj in cq.Find("a"))
            {
                string foundHref = obj.GetAttribute("href");

                if (foundHref != null && foundHref.IndexOf("olx.ua/obyavlenie") != -1)
                {
                    foundHref = foundHref.Replace(";promoted", String.Empty);
                    if (!result.Exists(x => x.Equals(foundHref)))
                        result.Add(foundHref);
                }
            }
            return result;
        }
    }
}
