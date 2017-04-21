using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
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
    public sealed partial class NewsList : Page
    {
        private ObservableCollection<NewsLists> NewsTitleList { get; set; }
        public NewsList()
        {
            this.InitializeComponent();
            NewsListWhole.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:///Resources/Backgrounds/NewsBackground.jpg")) };
            NewsTitleList = new ObservableCollection<NewsLists>();
            PageLoad();

        }

        private async void PageLoad()
        {
            var NewsListReturn = await GetNewsList("tech");
            if (NewsListReturn.list != null)
            {
                NewsTitleList.Clear();
                foreach(var item in NewsListReturn.list)
                {
                    NewsTitleList.Add(item);
                }
            }
        }
        private static async Task<NewsListRoot> GetNewsList(string s)
        {
            try
            {
                var url = "http://wangyi.butterfly.mopaasapp.com/news/api?type="+s+"&page=1&limit=24";
                HttpClient http = new HttpClient();
                var response = await http.GetAsync(url);
                var Message = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(NewsListRoot));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(Message));
                var result = (NewsListRoot)serializer.ReadObject(ms);
                return result;
            }
            catch (Exception)
            {
                return new NewsListRoot();
            }
        }

        private void NewsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedNews = (NewsLists)e.ClickedItem;
            Frame.Navigate(typeof(NewsDetail), selectedNews.id);
        }
    }
}
