using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace WeGo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Location : Page
    {
        public Location()
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
                    try
                    {
                        Geoposition pos = await geolocator.GetGeopositionAsync();
                        Geopoint myLocation = pos.Coordinate.Point;

                        // Set the map location.
                        map.Center = myLocation;
                        map.ZoomLevel = 16;
                        map.LandmarksVisible = true;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Specify a known location.
            BasicGeoposition cityPosition = new BasicGeoposition() { Latitude = 41.76, Longitude = 123.42 };
            Geopoint cityCenter = new Geopoint(cityPosition);

            // Set the map location.
            map.Center = cityCenter;
            map.ZoomLevel = 15;
            map.LandmarksVisible = true;
        }

    }
}
