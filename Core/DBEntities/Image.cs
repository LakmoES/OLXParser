using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLXParser.DBEntities
{
    public class Image
    {
        public int id { get; set; }
        public string sourceurl { get; set; }
        public int advertid { get; set; }
    }
}
