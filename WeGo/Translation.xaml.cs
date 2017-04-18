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
using Windows.UI.Xaml;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace WeGo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Translation : Page
    {
        public Translation()
        {
            this.InitializeComponent();
            //Tempbo.Background.Opacity = 0.1;
            TranslationWhole.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:///Background/GooglishFlat.png")) };
            PageLoad();
        }
        public async void PageLoad()
        {
            var iciba = await GetIciba();
            if (iciba.content != null)
            {
                EngSentence.Text = iciba.content;
                ChiSentence.Text = iciba.note;
            }
        }

        private async void Translation_Button_Click(object sender, RoutedEventArgs e)
        {
            var plaintext = Translation_Input.Text;
            if (plaintext != "") {
                var TranslationResult = await GetTranslation(plaintext);
                if (TranslationResult.translation != null)
                {
                    Translation_Output.Text = TranslationResult.translation[0];
                }
                else if (TranslationResult.translation == null) {
                    var dialog = new MessageDialog("操作失败，请检查网络服务是否开启和输入是否合法", "消息提示");
                    dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                    await dialog.ShowAsync();
                }
            }
        }

        private const string key = "324346101";
        private const string website = "http://fanyi.youdao.com/openapi.do?keyfrom=Thornwu&key=";
        private static async Task<TranslationRequest> GetTranslation(string plaintext)
        {
            try
            {
                var url = website + key + "&type=data&doctype=json&version=1.1&q=" + plaintext;
                HttpClient http = new HttpClient();
                var response = await http.GetAsync(url);
                var Message = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(TranslationRequest));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(Message));
                var result = (TranslationRequest)serializer.ReadObject(ms);
                return result;
            }
            catch (Exception){
                return new TranslationRequest();
            }
        }
        private static async Task<Iciba> GetIciba()
        {
            try
            {
                var url = "http://open.iciba.com/dsapi/";
                HttpClient http = new HttpClient();
                var response = await http.GetAsync(url);
                var Message = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(Iciba));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(Message));
                var result = (Iciba)serializer.ReadObject(ms);
                return result;
            }
            catch (Exception)
            {
                return new Iciba();
            }
        }

    }
}
