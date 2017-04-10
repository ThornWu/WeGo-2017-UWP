using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WeGo
{
   [XmlType(TypeName="Air")]
    public class Air {
        [XmlArray("Airlines")]
        public List<AirlinesTime> Airlines { get; set; }
    }
    [XmlType(TypeName="AirlinesTime")]
    public class AirlinesTime {
        [XmlElement]
        public string Company { get; set; }

        [XmlElement]
        public string AirlineCode { get; set; }

        [XmlElement]
        public string StartDrome { get; set; }

        [XmlElement]
        public string ArriveDrome { get; set; }

        [XmlElement]
        public string StartTime { get; set; }

        [XmlElement]
        public string ArriveTime { get; set; }

        [XmlElement]
        public string Mode { get; set; }

        [XmlElement]
        public int AirlineStop { get; set; }

        [XmlElement]
        public string Week { get; set; }

    }
}
