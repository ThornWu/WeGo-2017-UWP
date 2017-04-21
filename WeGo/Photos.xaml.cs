using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using WeGo.Models;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace WeGo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Photos : Page
    {
        public ObservableCollection<WeGo.Models.ImageSource> imageList;
        IReadOnlyCollection<StorageFile> fileList;
        System.Net.Http.HttpClient httpClient;
        Uri requestUri;
        string header = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
        System.Net.Http.HttpResponseMessage httpResponse;
        public bool IsUpload { set; get; }

        public Photos()
        {
            this.InitializeComponent();
            UploadButton.Opacity = 0;
            PhotoWhole.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:///Resources/Backgrounds/PhotoBackground.png")) };
            imageList = new ObservableCollection<WeGo.Models.ImageSource>();
            httpClient = new System.Net.Http.HttpClient();
            requestUri = new Uri("http://127.0.0.1:5000/upload_image");
            httpResponse = new System.Net.Http.HttpResponseMessage();
            IsUpload = false;
        }

        private async void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            // 文件选择器
            var folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;

            // 文件选择
            folderPicker.FileTypeFilter.Add(".jpg");
            folderPicker.FileTypeFilter.Add(".png");
            folderPicker.FileTypeFilter.Add(".jpeg");

            StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            if(folder != null)
            {
                Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);

                fileList = await folder.GetFilesAsync();
                imageList.Clear();
                Tips.Visibility = Visibility.Collapsed;
                foreach (StorageFile file in fileList)
                {
                    BitmapImage bitmap = new BitmapImage();
                    
                    using(var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        bitmap.SetSource(stream);
                    }

                    WeGo.Models.ImageSource image = new WeGo.Models.ImageSource();
                    image.name = file.Name;
                    image.path = file.Path;
                    image.bitmap = bitmap;
                    imageList.Add(image); 
                }
                    
                IsUpload = true;
                UploadButton.Opacity = 1;
            }
        }

        private async void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsUpload == true)
            {
                // 将当前文件夹中的图片全部上传
                if (fileList != null)
                {
                    // 上传文件的表单
                    MultipartFormDataContent form = new MultipartFormDataContent();

                    foreach (StorageFile file_ in fileList)
                    {
                        Stream fileStream = await file_.OpenStreamForReadAsync();
                        StreamContent streamContent = new StreamContent(fileStream);
                        form.Add(streamContent, "file", file_.Name);
                    }

                    var headers = httpClient.DefaultRequestHeaders;
                    if (!headers.UserAgent.TryParseAdd(header))
                    {
                        throw new Exception("Invalid header value: " + header);
                    }

                    try
                    {
                        httpResponse = await httpClient.PostAsync(requestUri, form);
                        httpResponse.EnsureSuccessStatusCode();
                        string httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                    }
                    catch (Exception ex)
                    {
                        string httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                    }
                }
                var dialog = new MessageDialog("照片上传成功", "上传提示");
                dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                await dialog.ShowAsync();
                IsUpload = false;
                UploadButton.Opacity = 0;
            }
            
           
        }
    }
}
