using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using WeGo.Models;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace WeGo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewsDetail : Page
    {
        private string ImgUri { set; get; }
        public NewsDetail()
        {
            this.InitializeComponent();
            NewsStackPanel.Background.Opacity = 0.6;
            NewsDetailWhole.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:///Resources/Backgrounds/NewsBackground.jpg")) };
            
        }


        private static async Task<NewsDetailRoot> GetNews(int id)
        {
            try
            {
                var url = "http://wangyi.butterfly.mopaasapp.com/detail/api?simpleId="+id;
                HttpClient http = new HttpClient();
                var response = await http.GetAsync(url);
                var Message = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(NewsDetailRoot));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(Message));
                var result = (NewsDetailRoot)serializer.ReadObject(ms);
                return result;
            }
            catch (Exception)
            {
                return new NewsDetailRoot();
            }
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            int id = (int)e.Parameter;
            var NewsReturn = await GetNews(id);
            if (NewsReturn.content != null)
            {
                NewsTitle.Text = NewsReturn.title;
                NewsTime.Text = NewsReturn.time;
                NewsSource.Text = NewsReturn.from;
                NewsContent.Text = System.Text.RegularExpressions.Regex.Replace(NewsReturn.content, "<[^>]*>", "").Replace("\n\n","\n").Replace("\n","\n\n       ");
            }

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewsList));
        }
    }
}
