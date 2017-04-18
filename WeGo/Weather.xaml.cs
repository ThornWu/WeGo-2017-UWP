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
using Windows.Storage;
using Windows.UI.Xaml.Media;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace WeGo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>

    public sealed partial class Weather : Page
    {
        private double lat, lon;
        private string CityId { get; set; }
        private ObservableCollection<CityInfo> CitySuggestion { get; set; }
        private ObservableCollection<CityInfo> NoCitySuggestion { get; set; }
        private ObservableCollection<WeatherDaily> DailyCollection { get; set; }
        private ObservableCollection<WeatherSuggetInfo> SuggestionColletion { get; set; }
        public Weather()
        {
            this.InitializeComponent();
            WeatherWhole.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:///Background/Weather.png")) };
            Page_Loaded();
            CitySuggestion = new ObservableCollection<CityInfo>();
            DailyCollection = new ObservableCollection<WeatherDaily>();
            NoCitySuggestion = new ObservableCollection<CityInfo>();
            SuggestionColletion = new ObservableCollection<WeatherSuggetInfo>();
            CityId = "";
            StructCitySuggestion();
            ApplicationDataContainer localSettings =
            ApplicationData.Current.LocalSettings;
            Windows.Storage.StorageFolder localFolder =
                Windows.Storage.ApplicationData.Current.LocalFolder;
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
                            WeatherIcon.Source = new BitmapImage(new Uri("ms-appx:/WeatherIcons//" + Weather_Now.HeWeather5[0].now.cond.code + ".png"));
                            try {
                                foreach (var item in CitySuggestion)
                                {
                                    if (Weather_Now.HeWeather5[0].basic.id == item.id)
                                    {
                                        if (item.cityZh == item.leaderZh && item.leaderZh == item.provinceZh)
                                        {
                                            City.Text = item.cityZh;
                                        }
                                        else if (item.cityZh == item.leaderZh || item.leaderZh == item.provinceZh)
                                        {
                                            City.Text = item.provinceZh + " - " + item.cityZh;
                                        }
                                        else
                                        {
                                            City.Text = item.provinceZh + " - " + item.leaderZh + " - " + item.cityZh;
                                        }
                                        break;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                City.Text = Weather_Now.HeWeather5[0].basic.city;
                            }
                            foreach(var item in Weather_Now.HeWeather5[0].daily_forecast)
                            {
                                item.picaddress = "WeatherIcons/"+item.cond.code_d+".png";
                                DailyCollection.Add(item);
                            }

                            var SuggestionList = Weather_Now.HeWeather5[0].suggestion;
                            SuggestionColletion.Clear();
                            SuggestionList.trav.title = "旅游指数";
                            SuggestionList.trav.picaddress = "WeatherIcons/Suggestion1.png";
                            SuggestionColletion.Add(SuggestionList.trav);

                            SuggestionList.drsg.title = "穿衣指数";
                            SuggestionList.drsg.picaddress = "WeatherIcons/Suggestion2.png";
                            SuggestionColletion.Add(SuggestionList.drsg);

                            SuggestionList.sport.title = "运动指数";
                            SuggestionList.sport.picaddress = "WeatherIcons/Suggestion3.png";
                            SuggestionColletion.Add(SuggestionList.sport);

                            SuggestionList.uv.title = "紫外线指数";
                            SuggestionList.uv.picaddress = "WeatherIcons/Suggestion4.png";
                            SuggestionColletion.Add(SuggestionList.uv);

                            SuggestionList.flu.title = "感冒指数";
                            SuggestionList.flu.picaddress = "WeatherIcons/Suggestion5.png";
                            SuggestionColletion.Add(SuggestionList.flu);


                            Cond.Text = Weather_Now.HeWeather5[0].now.cond.txt;
                            Tem.Text =  Weather_Now.HeWeather5[0].now.tmp.ToString();
                            WindSpeed.Text = "风力："+Weather_Now.HeWeather5[0].now.wind.dir + " " + Weather_Now.HeWeather5[0].now.wind.sc + "级";
                            Humidity.Text = "湿度：" + Weather_Now.HeWeather5[0].now.hum + "%";
                            Celsius.Visibility = Windows.UI.Xaml.Visibility.Visible;
                            BeforeGetWeather.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                            AfterGetWeather.Visibility = Windows.UI.Xaml.Visibility.Visible;
                            
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
            if (autoSuggestBox.Text != "")
            {
                var filtered = CitySuggestion.Where(p => p.cityZh.StartsWith(autoSuggestBox.Text)).ToArray();
                autoSuggestBox.ItemsSource = filtered;
            }
            else
            {
                autoSuggestBox.ItemsSource = NoCitySuggestion;
            }

        }

        private async void WeatherCityList_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            WeatherCityList.Text = "";
            AfterGetWeather.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            BeforeGetWeather.Visibility = Windows.UI.Xaml.Visibility.Visible;
            Celsius.Visibility = Windows.UI.Xaml.Visibility.Visible;
            try
            {
                var Weather_Now = await GetInfoByCityNameOrId(CityId);
                if (Weather_Now.HeWeather5 != null)
                {
                    WeatherIcon.Source = new BitmapImage(new Uri("ms-appx:/WeatherIcons//" + Weather_Now.HeWeather5[0].now.cond.code + ".png"));
                    try
                    {

                        foreach (var item in CitySuggestion)
                        {
                            if (Weather_Now.HeWeather5[0].basic.id == item.id)
                            {
                                if (item.cityZh == item.leaderZh && item.leaderZh == item.provinceZh)
                                {
                                    City.Text = item.cityZh;
                                }
                                else if (item.cityZh == item.leaderZh || item.leaderZh == item.provinceZh)
                                {
                                    City.Text = item.provinceZh + " - " + item.cityZh;
                                }
                                else
                                {
                                    City.Text = item.provinceZh + " - " + item.leaderZh + " - " + item.cityZh;
                                }
                                break;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        City.Text = Weather_Now.HeWeather5[0].basic.city;
                    }
                    DailyCollection.Clear();
                    foreach (var item in Weather_Now.HeWeather5[0].daily_forecast)
                    {
                        item.picaddress = "WeatherIcons/" + item.cond.code_d + ".png";
                        DailyCollection.Add(item);
                    }

                    var SuggestionList = Weather_Now.HeWeather5[0].suggestion;
                    SuggestionColletion.Clear();
                    SuggestionList.trav.title = "旅游指数";
                    SuggestionList.trav.picaddress = "WeatherIcons/Suggestion1.png";
                    SuggestionColletion.Add(SuggestionList.trav);

                    SuggestionList.drsg.title = "穿衣指数";
                    SuggestionList.drsg.picaddress = "WeatherIcons/Suggestion2.png";
                    SuggestionColletion.Add(SuggestionList.drsg);

                    SuggestionList.sport.title = "运动指数";
                    SuggestionList.sport.picaddress = "WeatherIcons/Suggestion3.png";
                    SuggestionColletion.Add(SuggestionList.sport);

                    SuggestionList.uv.title = "紫外线指数";
                    SuggestionList.uv.picaddress = "WeatherIcons/Suggestion4.png";
                    SuggestionColletion.Add(SuggestionList.uv);

                    SuggestionList.flu.title = "感冒指数";
                    SuggestionList.flu.picaddress = "WeatherIcons/Suggestion5.png";
                    SuggestionColletion.Add(SuggestionList.flu);



                    Cond.Text = Weather_Now.HeWeather5[0].now.cond.txt;
                    Tem.Text = Weather_Now.HeWeather5[0].now.tmp.ToString();
                    WindSpeed.Text = "风力：" + Weather_Now.HeWeather5[0].now.wind.dir + " " + Weather_Now.HeWeather5[0].now.wind.sc + "级";
                    Humidity.Text = "湿度：" + Weather_Now.HeWeather5[0].now.hum + "%";
                    Celsius.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    BeforeGetWeather.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    AfterGetWeather.Visibility = Windows.UI.Xaml.Visibility.Visible;

                    WeatherCityList.ItemsSource = NoCitySuggestion;

                }
                else
                {
                    var dialog = new MessageDialog("获取天气信息失败，请检查网络服务是否开启", "消息提示");
                    dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                    await dialog.ShowAsync();
                }
            }
            catch (Exception)
            {
                var dialog = new MessageDialog("获取天气信息失败，请检查网络服务是否开启", "消息提示");
                dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                await dialog.ShowAsync();
            }
        }

        private void WeatherCityList_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var continents = args.SelectedItem as CityInfo;
            CityId = continents.id;
            foreach(var item in CitySuggestion)
            {
                if (CityId == item.id)
                {
                    sender.Text = item.cityZh;
                }
            }
            WeatherCityList.ItemsSource = NoCitySuggestion;
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



        private static async Task<WeatherRequest> GetInfoByCityNameOrId(string city)
        {
            var NoWeatherRequest = new WeatherRequest();
            try
            {
                var url = website + city + "&key=" + key;
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
