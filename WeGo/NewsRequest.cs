using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeGo
{
    public class NewsLists
    {
        public string imgurl { get; set; }
        public bool has_content { get; set; }
        public string docurl { get; set; }
        public int id { get; set; }
        public string time { get; set; }
        public string title { get; set; }
        public string channelname { get; set; }
    }

    public class NewsListRoot
    {
        public int size { get; set; }
        public List<NewsLists> list { get; set; }
    }

    public class NewsDetailRoot
    {
        public int simple_id { get; set; }
        public string from { get; set; }
        public string old_title { get; set; }
        public int id { get; set; }
        public string time { get; set; }
        public string title { get; set; }
        public string content { get; set; }
    }
}
