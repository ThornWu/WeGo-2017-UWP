using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeGo.BaiduGeoCoding
{
    public class Location
    {
        public double lng { get; set; }
        public double lat { get; set; }
    }

    public class AddressComponent
    {
        public string country { get; set; }
        public int country_code { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string adcode { get; set; }
        public string street { get; set; }
        public string street_number { get; set; }
        public string direction { get; set; }
        public string distance { get; set; }
    }

    public class Point
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class Pois
    {
        public string addr { get; set; }
        public string cp { get; set; }
        public string direction { get; set; }
        public string distance { get; set; }
        public string name { get; set; }
        public string poiType { get; set; }
        public Point point { get; set; }
        public string tag { get; set; }
        public string tel { get; set; }
        public string uid { get; set; }
        public string zip { get; set; }
    }

    public class Result
    {
        public Location location { get; set; }
        public string formatted_address { get; set; }
        public string business { get; set; }
        public AddressComponent addressComponent { get; set; }
        public List<Pois> pois { get; set; }
        public List<object> poiRegions { get; set; }
        public string sematic_description { get; set; }
        public int cityCode { get; set; }
    }

    public class BaiduGeo
    {
        public int status { get; set; }
        public Result result { get; set; }
    }
}
