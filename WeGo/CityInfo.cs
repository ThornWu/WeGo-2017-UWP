using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeGo
{
    public class CityInformationList
    {
        public List<CityInfo> root { get; set; }
    }

    public class CityInfo
    {
        public string id { get; set; }
        public string cityEn { get; set; }
        public string cityZh { get; set; }
        public string countryCode { get; set; }
        public string countryEn { get; set; }
        public string countryZh { get; set; }
        public string provinceEn { get; set; }
        public string provinceZh { get; set; }
        public string leaderEn { get; set; }
        public string leaderZh { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
    }
}
