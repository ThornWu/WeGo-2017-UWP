using System;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
using Windows.UI;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using WeGo.BaiduGeoCoding;
using WeGo.BaiduSuggestion;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http;
using System.Runtime.Serialization.Json;
// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace WeGo
{
    public sealed partial class MapView : Page
    {
        public string DefaultCity { get; set; }
        public ObservableCollection<BaiduSuggestion.Result> suggestions { get; set; }
        public ObservableCollection<BaiduSuggestion.Result> routesuggestions { get; set; }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 设定默认地图中心点
            BasicGeoposition cityPosition = new BasicGeoposition() { Latitude = 41.76, Longitude = 123.42 };
            Geopoint cityCenter = new Geopoint(cityPosition);
            map.Center = cityCenter;
            map.ZoomLevel = 15;
        }


        public MapView()
        {
            this.InitializeComponent();

            //初始化
            AfterPageLoaded();
            suggestions = new ObservableCollection<BaiduSuggestion.Result>();
            LocationSearch.ItemsSource = suggestions;
            routesuggestions = new ObservableCollection<BaiduSuggestion.Result>();
            MapStartBox.ItemsSource = routesuggestions;
            MapEndBox.ItemsSource = routesuggestions;
            DefaultCity = "";

        }


        public async void GetDefaultCity(){
            //获取当前地图中心点所在城市
            var GeoFromBaidu = await GetGeo(map.Center.Position.Latitude, map.Center.Position.Longitude);
            if (GeoFromBaidu.result != null)
            {
                DefaultCity =GeoFromBaidu.result.addressComponent.city;
            }
            else if (GeoFromBaidu.result == null)
            {
                DefaultCity = "沈阳";//获取失败时默认城市沈阳
            }
        }


        //路况是否开启
        private void MapRoadCondition_Toggled(object sender, RoutedEventArgs e)
        {
            if (MapRoadCondition.IsOn)
            {
                map.TrafficFlowVisible = true;
            }
            else if (!MapRoadCondition.IsOn)
            {
                map.TrafficFlowVisible = false;
            }
        }


        private async void AfterPageLoaded()
        {

            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:

                    // Get the current location.
                    Geolocator geolocator = new Geolocator();
                    try
                    {
                        Geoposition pos = await geolocator.GetGeopositionAsync();
                        Geopoint myLocation = pos.Coordinate.Point;

                        // Create a MapIcon.
                        MapIcon mapIcon1 = new MapIcon();
                        mapIcon1.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/neddle.png"));
                        mapIcon1.Location = myLocation;
                        mapIcon1.NormalizedAnchorPoint = new Windows.Foundation.Point(0.5, 1.0);
                       
                        //mapIcon1.Title = "当前位置";
                        mapIcon1.ZIndex = 0;

                        // Add the MapIcon to the map.
                        map.MapElements.Clear();
                        map.MapElements.Add(mapIcon1);
                        // Set the map location.
                        map.Center = myLocation;
                        map.ZoomLevel = 16;

                        GetDefaultCity();
                    }
                    catch (Exception) {
                        var dialog = new MessageDialog("定位失败，请检查网络和定位服务是否开启", "消息提示");
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


        private async void ShowRouteOnMap()
        {
            // Start at Microsoft in Redmond, Washington.
            BasicGeoposition startLocation = new BasicGeoposition() { Latitude = 47.643, Longitude = -122.131 };

            // End at the city of Seattle, Washington.
            BasicGeoposition endLocation = new BasicGeoposition() { Latitude = 47.604, Longitude = -122.329 };


            // Get the route between the points.
            MapRouteFinderResult routeResult =
                  await MapRouteFinder.GetDrivingRouteAsync(
                  new Geopoint(startLocation),
                  new Geopoint(endLocation),
                  MapRouteOptimization.Time,
                  MapRouteRestrictions.None);

            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
                // Use the route to initialize a MapRouteView.
                MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                viewOfRoute.RouteColor = Colors.Yellow;
                viewOfRoute.OutlineColor = Colors.Black;

                // Add the new MapRouteView to the Routes collection
                // of the MapControl.
                map.Routes.Add(viewOfRoute);

                // Fit the MapControl to the route.
                await map.TrySetViewBoundsAsync(
                      routeResult.Route.BoundingBox,
                      null,
                      Windows.UI.Xaml.Controls.Maps.MapAnimationKind.None);
            }
        }

        private void MapPositionNow_Click(object sender, RoutedEventArgs e)
        {
            AfterPageLoaded();
        }



        private async void LocationSearch_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (LocationSearch.Text == "")
            {
                map.MapElements.Clear();
            }
            else
            {
                var PresentText = LocationSearch.Text;
                var SuggestionFromBaidu = await GetSuggestion(PresentText, DefaultCity);
                if (SuggestionFromBaidu.result != null)
                {
                    var SuggestionArray = SuggestionFromBaidu.result;
                    suggestions.Clear();
                    foreach (var item in SuggestionArray)
                    {
                        suggestions.Add(item);
                    }
                }
            }
        }

        private void LocationSearch_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            bool IsFind=false;
            double lat=0, lon=0;
            for (var i = 0; i < suggestions.Count; i++)
            {
                if (suggestions[i].name == args.QueryText)
                {
                    try
                    {
                        lat = suggestions[i].location.lat;
                        lon = suggestions[i].location.lng;
                        IsFind = true;
                    }
                    catch (Exception) { }
                    break;
                }
            }
            if (IsFind) {
                BasicGeoposition myPosition = new BasicGeoposition() { Latitude = lat-0.0065, Longitude = lon-0.0065 };
                Geopoint myLocation = new Geopoint(myPosition);
                // Create a MapIcon.
                MapIcon mapIcon1 = new MapIcon();
                mapIcon1.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/neddle.png"));
                mapIcon1.Location = myLocation;
                mapIcon1.NormalizedAnchorPoint = new Windows.Foundation.Point(0.5, 1.0);

                mapIcon1.Title = args.QueryText;
                mapIcon1.ZIndex = 0;

                // Add the MapIcon to the map.
                map.MapElements.Clear();
                map.MapElements.Add(mapIcon1);
                // Set the map location.
                map.Center = myLocation;
                map.ZoomLevel = 16;
                GetDefaultCity();
            }

        }

        private const string key = "Vpqm90vonTUb0LC1XV7467vlzZQvXDBe";
        private const string suggestwebsite = "http://api.map.baidu.com/place/v2/suggestion?query=";
        private static async Task<BaiduSuggest> GetSuggestion(string text,string city)
        {
            var NoBaiduSuggest = new BaiduSuggest();
            try
            {
                if (city == "") { city = "全国"; }
                var url = suggestwebsite+ text + "&region="+city+"&output=json&ak=" + key;
                HttpClient http = new HttpClient();
                var response = await http.GetAsync(url);
                var Message = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(BaiduSuggest));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(Message));
                var result = (BaiduSuggest)serializer.ReadObject(ms);
                return result;
            }
            catch (Exception) {
                return NoBaiduSuggest;
            }
        }

        private const string geowebsite = "http://api.map.baidu.com/geocoder/v2/?callback=renderReverse&location=";
        private static async Task<BaiduGeo> GetGeo(double lat,double lon)
        {
            var NoBaiduGeo = new BaiduGeo();
            try
            {
                var url = geowebsite + lat+","+lon+"&output=json&ak=" + key;
                HttpClient http = new HttpClient();
                var response = await http.GetAsync(url);
                var Message = await response.Content.ReadAsStringAsync();
                var MessageSlice = Message.ToString().Substring(0, Message.Length - 2).Substring(29);
                var serializer = new DataContractJsonSerializer(typeof(BaiduGeo));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(MessageSlice));
                var result = (BaiduGeo)serializer.ReadObject(ms);
                return result;
            }
            catch (Exception)
            {
                return NoBaiduGeo;
            }
        }

        private void MapStartBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

        }

        private void MapStartBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private void MapEndBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

        }

        private void MapEndBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private void MapRouteSearch_Click(object sender, RoutedEventArgs e)
        {
            //ShowRouteOnMap();
        }
    }
}
