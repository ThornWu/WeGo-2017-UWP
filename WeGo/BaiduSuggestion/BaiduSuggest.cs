using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeGo.BaiduSuggestion
{
    public class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Result
    {
        public string name { get; set; }
        public Location location { get; set; }
        public string uid { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string business { get; set; }
        public string cityid { get; set; }
    }

    public class BaiduSuggest
    {
        public int status { get; set; }
        public string message { get; set; }
        public List<Result> result { get; set; }
    }
}
