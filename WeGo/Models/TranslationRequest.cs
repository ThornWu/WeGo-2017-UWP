using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeGo.Models
{
    public class TranslationRequest
    {
        public List<string> translation { get; set; }
        public TranslationBasic basic;
        public string query { get; set; }
        public int errorCode { get; set; }
        public List<WebTranslation> web { get; set; }
    }
    public class WebTranslation
    {
        public List<string> value { get; set; }
        public string key { get; set; }
    }
    public class TranslationBasic
    {
        public List<string> explains;
    }
}
