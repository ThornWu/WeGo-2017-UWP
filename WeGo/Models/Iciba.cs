using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeGo.Models
{
    public class Tag
    {
        public object id { get; set; }
        public object name { get; set; }
    }

    public class Iciba
    {
        public string sid { get; set; }
        public string tts { get; set; }
        public string content { get; set; }
        public string note { get; set; }
        public string love { get; set; }
        public string translation { get; set; }
        public string picture { get; set; }
        public string picture2 { get; set; }
        public string caption { get; set; }
        public string dateline { get; set; }
        public string s_pv { get; set; }
        public string sp_pv { get; set; }
        public List<Tag> tags { get; set; }
        public string fenxiang_img { get; set; }
    }
}
