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
        public Weather()
        {
            this.InitializeComponent();
            Page_Loaded();

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
                        //经度
                        lon = pos.Coordinate.Point.Position.Longitude;
                        var Weather_Now = await GetInfo(lon, lat);
                        WeatherIcon.Source = new BitmapImage(new Uri("ms-appx:/Icon//" + Weather_Now.HeWeather5[0].now.cond.code + ".png"));

                        City.Text = "城市：" + Weather_Now.HeWeather5[0].basic.city;
                        Cond.Text = "天气：" + Weather_Now.HeWeather5[0].now.cond.txt;
                        Tem.Text = "温度：" + Weather_Now.HeWeather5[0].now.tmp;
                        More.Text = "";
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
        private const string key = "b094e6b8e4b84185bbed42275096fb49";
        private const string website = "https://free-api.heweather.com/v5/weather?city=";
        private static async Task<WeatherRequest> GetInfo(double lon, double lat)
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


    }
}
