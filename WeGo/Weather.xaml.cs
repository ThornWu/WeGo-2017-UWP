using System;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Geolocation;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Popups;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace WeGo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>

    public sealed partial class Weather : Page
    {
        double lat, lon;
        public string CityId { get; set; }
        public ObservableCollection<CityInfo> CitySuggestion { get; set; }
        public Weather()
        {
            this.InitializeComponent();
            Page_Loaded();
            CitySuggestion = new ObservableCollection<CityInfo>();
            WeatherCityList.ItemsSource = CitySuggestion;
            CityId = "";
            StructCitySuggestion();
        }

        private async void Page_Loaded()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:

                    // Get the current location.
                    Geolocator geolocator = new Geolocator();
                    try {
                        Geoposition pos = await geolocator.GetGeopositionAsync();
                        lat = pos.Coordinate.Point.Position.Latitude;
                        lon = pos.Coordinate.Point.Position.Longitude;
                        var Weather_Now = await GetInfo(lon, lat);
                        if (Weather_Now.HeWeather5 != null) {
                            WeatherIcon.Source = new BitmapImage(new Uri("ms-appx:/Icon//" + Weather_Now.HeWeather5[0].now.cond.code + ".png"));

                            City.Text = "城市：" + Weather_Now.HeWeather5[0].basic.city;
                            Cond.Text = "天气：" + Weather_Now.HeWeather5[0].now.cond.txt;
                            Tem.Text = "温度：" + Weather_Now.HeWeather5[0].now.tmp;
                            More.Text = "";
                        }
                        else
                        {
                            var dialog = new MessageDialog("获取天气信息失败，请检查网络服务是否开启", "消息提示");
                            dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                            await dialog.ShowAsync();
                        }
                    }
                    catch (Exception) {
                        var dialog = new MessageDialog("获取天气信息失败，请检查网络服务是否开启", "消息提示");
                        dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                        await dialog.ShowAsync();
                    }
                    break;

                case GeolocationAccessStatus.Denied:
                    // Handle the case  if access to location is denied.
                    break;

                case GeolocationAccessStatus.Unspecified:
                    // Handle the case if  an unspecified error occurs.
                    break;
            }

        }

        public async void StructCitySuggestion()
        {
            var SuggestionFromNet = await GetCityList();
            if (SuggestionFromNet.root!=null)
            {
                var SuggestionArray = SuggestionFromNet.root;
                CitySuggestion.Clear();
                foreach (var item in SuggestionArray)
                {
                    CitySuggestion.Add(item);
                }
            }

        }

        private const string key = "b094e6b8e4b84185bbed42275096fb49";
        private const string website = "https://free-api.heweather.com/v5/weather?city=";

        private void WeatherCityList_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            var autoSuggestBox = (AutoSuggestBox)sender;
            var filtered = CitySuggestion.Where(p => p.cityZh.StartsWith(autoSuggestBox.Text)).ToArray();
            autoSuggestBox.ItemsSource = filtered;
        }

        private void WeatherCityList_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            CityId = args.QueryText;
            WeatherCityList.Text = "";
        }

        private static async Task<WeatherRequest> GetInfo(double lon, double lat)
        {
            var NoWeatherRequest = new WeatherRequest();
            try
            {
                var url = website + lon + "," + lat + "&key=" + key;
                HttpClient http = new HttpClient();
                var response = await http.GetAsync(url);
                var Message = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(WeatherRequest));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(Message));
                var result = (WeatherRequest)serializer.ReadObject(ms);
                return result;
            }
            catch (Exception)
            {
                return NoWeatherRequest;
            }

        }

        private static async Task<CityInformationList> GetCityList()
        {
            var NoWeatherRequest = new CityInformationList();
            try
            {
                var url = "https://cdn.heweather.com/china-city-list.json";
                HttpClient http = new HttpClient();
                var response = await http.GetAsync(url);
                var Message = await response.Content.ReadAsStringAsync();
                var MessageProcess = "{\"root\":" + Message +"}";
                var serializer = new DataContractJsonSerializer(typeof(CityInformationList));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(MessageProcess));
                var result = (CityInformationList)serializer.ReadObject(ms);
                return result;
            }
            catch (Exception)
            {
                return NoWeatherRequest;
            }
        }

    }
}
