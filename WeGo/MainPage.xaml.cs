using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace WeGo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Pageload();
            MyFrame.Navigate(typeof(Homepage));
        }
        private void Pageload() {
            if (!FunctionManager.IsFunctionAdded("Map"))
            {
                Location_Selection.Visibility = Visibility.Visible;
                MapSwitch.IsOn = true;
            }
            if (!FunctionManager.IsFunctionAdded("Flight"))
            {
                Flight_Selection.Visibility = Visibility.Visible;
                FlightSwitch.IsOn = true;
            }
            if (!FunctionManager.IsFunctionAdded("Translation"))
            {
                Translation_Selection.Visibility = Visibility.Visible;
                TranslationSwitch.IsOn = true;
            }
            if (!FunctionManager.IsFunctionAdded("Weather"))
            {
                Weather_Selection.Visibility = Visibility.Visible;
                WeatherSwitch.IsOn = true;
            }
            if (!FunctionManager.IsFunctionAdded("Photo"))
            {
                Photos_Selection.Visibility = Visibility.Visible;
                PhotoSwitch.IsOn = true;
            }
            if (!FunctionManager.IsFunctionAdded("News"))
            {
                NewsList_Selection.Visibility = Visibility.Visible;
                NewsSwitch.IsOn = true;
            }

        }
        private void Hamburger_Button_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Homepage_Selection.IsSelected)
            {
                MyFrame.Navigate(typeof(Homepage));
            }
            else if (Location_Selection.IsSelected)
            {
                MyFrame.Navigate(typeof(MapView));
            }
            else if (Flight_Selection.IsSelected)
            {
                MyFrame.Navigate(typeof(Flight));
            }
            else if (Translation_Selection.IsSelected)
            {
                MyFrame.Navigate(typeof(Translation));
            }
            else if (Weather_Selection.IsSelected)
            {
                MyFrame.Navigate(typeof(Weather));
            }
            else if (Photos_Selection.IsSelected)
            {
                MyFrame.Navigate(typeof(Photos));
            }
            else if (NewsList_Selection.IsSelected)
            {
                MyFrame.Navigate(typeof(NewsList));
            }
        }

        private void MapSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (MapSwitch.IsOn)
            {
                Location_Selection.Visibility = Visibility.Visible;
                if (FunctionManager.IsFunctionAdded("Map"))
                {
                    FunctionManager.FunctionRemove("Map");
                }
            }
            else if (!MapSwitch.IsOn)
            {
                Location_Selection.Visibility = Visibility.Collapsed;
                MyFrame.Navigate(typeof(Homepage));
                if (!FunctionManager.IsFunctionAdded("Map"))
                {
                    FunctionManager.FunctionAdd("Map");
                }
            }
        }

        private void TranslationSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (TranslationSwitch.IsOn)
            {
                Translation_Selection.Visibility = Visibility.Visible;
                if (FunctionManager.IsFunctionAdded("Translation"))
                {
                    FunctionManager.FunctionRemove("Translation");
                }
            }
            else if (!TranslationSwitch.IsOn)
            {
                Translation_Selection.Visibility = Visibility.Collapsed;
                MyFrame.Navigate(typeof(Homepage));
                if (!FunctionManager.IsFunctionAdded("Translation"))
                {
                    FunctionManager.FunctionAdd("Translation");
                }
            }
        }

        private void FlightSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (FlightSwitch.IsOn)
            {
                Flight_Selection.Visibility = Visibility.Visible;
                if (FunctionManager.IsFunctionAdded("Flight"))
                {
                    FunctionManager.FunctionRemove("Flight");
                }
            }
            else if (!FlightSwitch.IsOn)
            {
                Flight_Selection.Visibility = Visibility.Collapsed;
                MyFrame.Navigate(typeof(Homepage));
                if (!FunctionManager.IsFunctionAdded("Flight"))
                {
                    FunctionManager.FunctionAdd("Flight");
                }
            }
        }

        private void WeatherSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (WeatherSwitch.IsOn)
            {
                Weather_Selection.Visibility = Visibility.Visible;
                if (FunctionManager.IsFunctionAdded("Weather"))
                {
                    FunctionManager.FunctionRemove("Weather");
                }
            }
            else if (!WeatherSwitch.IsOn)
            {
                Weather_Selection.Visibility = Visibility.Collapsed;
                MyFrame.Navigate(typeof(Homepage));
                if (!FunctionManager.IsFunctionAdded("Weather"))
                {
                    FunctionManager.FunctionAdd("Weather");
                }
            }
        }

        private void PhotoSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (PhotoSwitch.IsOn)
            {
                Photos_Selection.Visibility = Visibility.Visible;
                if (FunctionManager.IsFunctionAdded("Photo"))
                {
                    FunctionManager.FunctionRemove("Photo");
                }
            }
            else if (!PhotoSwitch.IsOn)
            {
                Photos_Selection.Visibility = Visibility.Collapsed;
                MyFrame.Navigate(typeof(Homepage));
                if (!FunctionManager.IsFunctionAdded("Photo"))
                {
                    FunctionManager.FunctionAdd("Photo");
                }
            }
        }

        private void NewsSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (NewsSwitch.IsOn)
            {
                NewsList_Selection.Visibility = Visibility.Visible;
                if (FunctionManager.IsFunctionAdded("News"))
                {
                    FunctionManager.FunctionRemove("News");
                }
            }
            else if (!NewsSwitch.IsOn)
            {
                NewsList_Selection.Visibility = Visibility.Collapsed;
                MyFrame.Navigate(typeof(Homepage));
                if (!FunctionManager.IsFunctionAdded("News"))
                {
                    FunctionManager.FunctionAdd("News");
                }
            }
        }
    }
}
