using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeGo
{
    public class WeatherRequest
    {
        public List<WeatherInfo> HeWeather5 { get; set; }
    }
    public class WeatherInfo
    {
        public List<WeatherAlarm> alarms { get; set; }
        public WeatherAqi aqi { get; set; }
        public WeatherBasic basic { get; set; }
        public List<WeatherDaily> daily_forecast { get; set; }
        public List<WeatherHour> hourly_forecast { get; set; }
        public WeatherNow now { get; set; }
        public string status { get; set; }
        public WeatherSuggestion suggestion { get; set; }
    }
    public class WeatherAlarm
    {
        public string level { get; set; }
        public string stat { get; set; }
        public string title { get; set; }
        public string txt { get; set; }
        public string type { get; set; }

    }
    public class WeatherAqi
    {
        public CityAqi city { get; set; }
    }
    public class CityAqi
    {
        public int aqi { get; set; }
        public int co { get; set; }
        public int no2 { get; set; }
        public int o3 { get; set; }
        public int pm10 { get; set; }
        public int pm25 { get; set; }
        public string qlty { get; set; }
        public int so2 { get; set; }
    }
    public class WeatherBasic
    {
        public string city { get; set; }
        public string cnty { get; set; }
        public string id { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string prov { get; set; }
        public UpdateTime update { get; set; }
    }
    public class UpdateTime
    {
        public string loc { get; set; }
        public string utc { get; set; }
    }
    public class WeatherDaily
    {
        public DailyAstro astro { get; set; }
        public DailyCond cond { get; set; }
        public string date { get; set; }
        public int hum { get; set; }
        public string pcpn { get; set; }
        public int pop { get; set; }
        public int pres { get; set; }
        public DailyTmp tmp { get; set; }
        public int vis { get; set; }
        public WindInfo wind { get; set; }
    }
    public class DailyAstro
    {
        public string mr { get; set; }
        public string ms { get; set; }
        public string sr { get; set; }
        public string ss { get; set; }
    }
    public class DailyCond
    {
        public string code_d { get; set; }
        public string code_n { get; set; }
        public string txt_d { get; set; }
        public string txt_n { get; set; }
    }
    public class DailyTmp
    {
        public int max { get; set; }
        public int min { get; set; }
    }

    public class WeatherHour
    {
        public CondInfo cond { get; set; }
        public string date { get; set; }
        public int hum { get; set; }
        public int pop { get; set; }
        public int pres { get; set; }
        public int tmp { get; set; }
        public WindInfo wind { get; set; }
    }


    public class WeatherNow
    {
        public CondInfo cond { get; set; }
        public int fl { get; set; }
        public int hum { get; set; }
        public int pcpn { get; set; }
        public int pres { get; set; }
        public int tmp { get; set; }
        public int vis { get; set; }
        public WindInfo win { get; set; }

    }
    public class CondInfo
    {
        public int code { get; set; }
        public string txt { get; set; }
    }
    public class WindInfo
    {
        public int deg { get; set; }
        public string dir { get; set; }
        public string sc { get; set; }
        public int spd { get; set; }
    }
    public class WeatherSuggestion
    {
        public WeatherCom comf { get; set; }
        public WeatherCw cw { get; set; }
        public WeatherDrsg drsg { get; set; }
        public WeatherFlu flu { get; set; }
        public WeatherSport sport { get; set; }
        public WeatherTrav trav { get; set; }
        public WeatherUv uv { get; set; }
    }
    public class WeatherCom
    {
        public string brf { get; set; }
        public string txt { get; set; }
    }
    public class WeatherCw
    {
        public string brf { get; set; }
        public string txt { get; set; }
    }
    public class WeatherDrsg
    {
        public string brf { get; set; }
        public string txt { get; set; }
    }
    public class WeatherFlu
    {
        public string brf { get; set; }
        public string txt { get; set; }
    }
    public class WeatherSport
    {
        public string brf { get; set; }
        public string txt { get; set; }
    }
    public class WeatherTrav
    {
        public string brf { get; set; }
        public string txt { get; set; }
    }
    public class WeatherUv
    {
        public string brf { get; set; }
        public string txt { get; set; }
    }
}
