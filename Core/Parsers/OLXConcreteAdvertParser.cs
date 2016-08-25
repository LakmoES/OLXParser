using CsQuery;
using OLXParser.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;

namespace OLXParser.Parsers
{
    public class OLXConcreteAdvertParser
    {
        public OLXConcreteAdvertParser()
        {

        }
        public static Advert GetAdvert(string url, out List<string> images)
        {
            CQ cq = CQ.Create(PageDownloader.Download(url));
            string title = GetTitle(cq);
            string description = GetDescription(cq);
            var advertParams = GetParams(cq);
            string cost = GetCost(cq);
            string township = GetTownship(cq);
            images = GetImages(cq);
            //MessageBox.Show(String.Join(Environment.NewLine, images));

            Advert advert = new Advert
            { title = title, description = description, cost = cost, url = url,
                area = advertParams.area, floor = advertParams.floor, floors = advertParams.floors,
                isnew = advertParams.isnew, rooms = advertParams.rooms, township = township };
            return advert;
            //return String.Format("Название: {0}\r\nПараметры:\r\n{1}\r\nОписание: {2}\r\nСтоимость: {3}", title, advertParams, description, cost);
        }
        private static string GetTitle(CQ cq)
        {
            string title = String.Empty;
            foreach (IDomObject obj in cq.Find("title"))
                title = WebUtility.HtmlDecode(obj.InnerText);
            return title;
        }
        private static string GetDescription(CQ cq)
        {
            string description = String.Empty;
            //var c = cq.Find("div");
            //MessageBox.Show("" + c.Count());
            //foreach (IDomObject obj in c)
            //{
            //    if (obj.HasAttribute("id"))
            //    {
            //        MessageBox.Show("it has attribute");
            //        if (obj.GetAttribute("id").Equals("textContent"))
            //        {
            //            description = obj.InnerHTML;
            //            MessageBox.Show("something for description is found");
            //        }
            //    }
            //}
            //cq = GetElementByIdPattern(cq, "textContent");
            //description = cq.Text();
            description = cq["#textContent"].Text().Trim('\n', ' ', '\t');
            return description;
        }
        private static AdvertParameters GetParams(CQ cq)
        {
            string advertParams = String.Empty;

            //details fixed marginbott20 margintop5 full
            var c = cq.Find("table");
            //MessageBox.Show("Total tables: " + c.Count());
            foreach (IDomObject obj in c)
            {
                if (obj.HasClass("details")) //table found
                {
                    cq = CQ.Create(WebUtility.HtmlDecode(obj.InnerHTML));
                    cq = cq.Find(".item");
                    var items = ParseItem(cq);
                    return ParseDictToAdvertParams(items);
                    //advertParams = String.Join(Environment.NewLine, items.Select(x => x.Key + ": " + x.Value).ToList());
                    //System.Diagnostics.Debug.WriteLine(WebUtility.HtmlDecode(obj.InnerHTML));
                }
            }
            return null;
        }
        private static string GetCost(CQ cq)
        {
            var costVal = cq[".xxxx-large"].First();
            string cost = costVal.Text().Trim('\n', ' ', '\t');
            return cost;
        }
        private static string GetTownship(CQ cq)
        {
            //c2b small
            string township = cq[".c2b"].Text().Trim('\n', ' ', '\t');
            return township;
        }
        private static List<String> GetImages(CQ cq)
        {
            List<String> images = new List<string>();
            var mainImage = cq[".photo-handler"];//.Text().Trim('\n', ' ', '\t');
            var c = mainImage.Find("img");
            string mainImageUrl = String.Empty;
            foreach (IDomObject obj in c)
            {
                mainImageUrl = obj.GetAttribute("src");
            }
            if (mainImageUrl.Length > 0)
                images.Add(mainImageUrl);

            var image = cq[".photo-glow"];
            c = image.Find("img");
            string imageUrl = String.Empty;
            foreach (IDomObject obj in c)
            {
                imageUrl = obj.GetAttribute("src");
                if (imageUrl.Length > 0 && !images.Exists(x => x.Equals(imageUrl)))
                    images.Add(imageUrl);
            }

            return images;
        }
        private static Dictionary<string, string> ParseItem(CQ cq)
        {
            Dictionary<string, string> parsed = new Dictionary<string, string>();
            foreach (var element in cq)
            {
                CQ c = CQ.Create(element.InnerHTML);
                parsed.Add(c.Find("th").Text().Trim(' ', '\n', '\t'), c.Find("td").Text().Trim('\n', ' ', '\t'));
                //System.Diagnostics.Debug.WriteLine("element: " + WebUtility.HtmlDecode(element.InnerHTML));
            }

            return parsed;
        }
        private static CQ GetElementByIdPattern(CQ doc, string contains)
        {
            string select = string.Format("*[id*={0}]", contains);
            return doc.Select(select).First();
        }
        private static AdvertParameters ParseDictToAdvertParams(Dictionary<string, string> parameters)
        {
            AdvertParameters ap = new AdvertParameters();
            foreach(KeyValuePair<string, string> kvp in parameters)
            {
                if (kvp.Key.Equals("Объявление от"))
                    ap.from = kvp.Value;
                if (kvp.Key.Equals("Тип квартиры"))
                    ap.isnew = AdvertParameters.ParseIsNew(kvp.Value);
                if (kvp.Key.Equals("Количество комнат"))
                {
                    var rooms = Convert.ToInt32(kvp.Value);
                    if (rooms > 0)
                        ap.rooms = rooms;
                }
                if (kvp.Key.Equals("Общая площадь"))
                {
                    var area = AdvertParameters.ParseArea(kvp.Value);
                    if (area > 0)
                        ap.area = area;
                }
                if (kvp.Key.Equals("Тип"))
                    ap.buildType = kvp.Value;
                if (kvp.Key.Equals("Этаж"))
                    ap.floor = Convert.ToInt32(kvp.Value);
                if (kvp.Key.Equals("Этажность дома"))
                {
                    var floors = Convert.ToInt32(kvp.Value);
                    if (floors > 0)
                        ap.floors = floors;
                }
            }
            return ap;
        }
        private class AdvertParameters
        {
            public string from { get; set; }
            public int isnew { get; set; }
            public int? rooms { get; set; }
            public int? area { get; set; }
            public string buildType { get; set; }
            public int floor { get; set; }
            public int? floors { get; set; }

            public static int ParseIsNew(string s)
            {
                s = s.Trim(' ', '\n', '\t');
                if (s.Equals("Вторичный рынок"))
                    return 0;
                if (s.Equals("Новостройки"))
                    return 1;
                return -1;
            }
            public static int ParseArea(string s)
            {
                s = s.Replace("м2", "").Trim(' ', '\t');
                int num;
                bool result = Int32.TryParse(s, out num);
                if (!result)
                    return -1;
                return num;
            }
        }
    }
}
