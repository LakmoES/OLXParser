using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLXParser.DBEntities
{
    public class Advert
    {
        public int id {get;set;}
        public string url { get; set; }
        public string title { get; set; }
        public int? rooms { get; set; }
        public int? floor { get; set; }
        public int? floors { get; set; }
        public int? area { get; set; }
        public int? isnew { get; set; }
        public string description { get; set; }
        public string cost { get; set; }
        public string township { get; set; }
        public override string ToString()
        {
            string toReturn = String.Format("{0}\r\n{1}\r\n", url, title);
            if (cost != null)
                toReturn += "Стоимость: " + cost + Environment.NewLine;
            if (township != null)
                toReturn += "Район: " + township + Environment.NewLine;
            if (rooms != null)
                toReturn += "Комнат: " + rooms.ToString() + Environment.NewLine;
            if (floor != null)
                toReturn += "Этаж: " + floor.ToString() + Environment.NewLine;
            if (floors != null)
                toReturn += "Этажность: " + floors.ToString() + Environment.NewLine;
            if (area != null)
                toReturn += "Площадь: " + area.ToString() + Environment.NewLine;
            if (isnew != null)
                toReturn += "Новострой: " + isnew.ToString() + Environment.NewLine;
            if (description != null)
                toReturn += "Описание: " + description.Trim(' ', '\t', '\n') + Environment.NewLine;

            return toReturn;
        }
        public string ToStringWithoutUrl()
        {
            string toReturn = String.Format("{0}\r\n\r\n", title);
            if (cost != null)
                toReturn += "Стоимость: " + cost + Environment.NewLine;
            if (township != null)
                toReturn += "Район: " + township + Environment.NewLine;
            if (rooms != null)
                toReturn += "Комнат: " + rooms.ToString() + Environment.NewLine;
            if (floor != null)
                toReturn += "Этаж: " + floor.ToString() + Environment.NewLine;
            if (floors != null)
                toReturn += "Этажность: " + floors.ToString() + Environment.NewLine;
            if (area != null)
                toReturn += "Площадь: " + area.ToString() + Environment.NewLine;
            if (isnew != null)
            {
                string isNewString = isnew == 0 ? "Вторичка" : "Новострой";
                toReturn += isNewString + Environment.NewLine;
            }
            if (description != null)
                toReturn += "\r\nОписание: " + description.Trim(' ', '\t', '\n') + Environment.NewLine;

            return toReturn;
        }
    }
}
