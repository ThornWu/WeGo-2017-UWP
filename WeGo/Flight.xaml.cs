using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Navigation;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using Windows.UI.Popups;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace WeGo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Flight : Page
    {
        string[] Airport = { "北京","上海","广州","成都","深圳","昆明","西安","重庆","杭州",
                             "厦门","南京","武汉","长沙","乌鲁木齐","青岛","郑州", "三亚", "海口","天津",
                             "大连","哈尔滨","贵阳","沈阳","福州","南宁","济南","太原","长春","兰州",
                             "南昌", "呼和浩特","温州","宁波","合肥","桂林", "石家庄","丽江","银川",
                             "珠海","无锡","烟台","西双版纳","西宁","泉州","晋江","揭阳","汕头","拉萨","包头","呼伦贝尔",
                             "常州","喀什","绵阳","鄂尔多斯","榆林","延吉","九寨沟","张家界","德宏","芒市","威海",
                             "徐州","大理","宜昌","湛江","义乌","库尔勒","南通","北海","阿克苏","临沂",
                             "赤峰","伊宁市","赣州","扬州","通辽","柳州","盐城","泸州","遵义","运城",
                             "襄阳","洛阳","和田","宜宾","连云港","腾冲","牡丹江","舟山","普陀山","长治","黄山",
                             "台州","南阳","毕节","万州","南充","大庆","井冈山","淮安","香格里拉","长白山","迪庆",
                             "武夷山","佳木斯","景德镇","济宁","锡林浩特","大同","潍坊","乌兰浩特","阜阳","满洲里",
                             "西昌","宜春","恩施","敦煌","林芝","齐齐哈尔","常德","乌海","惠州","达州",
                             "嘉峪关","铜仁","普洱","思茅","临沧","东营","保山","兴义","佛山","九华山","库车",
                             "阿勒泰","唐山","邯郸","巴彦淖尔","安庆","庆阳","梅州","梅县","昌都","丹东",
                             "广元","昭通","衢州","延安","张家口","玉树","鸡西","怀化","芷江","攀枝花","秦皇岛","山海关",
                             "锦州","安顺","克拉玛依","鞍山","黔江","加格达奇","汉中","黑河","博乐",
                             "格尔木","二连浩特","朝阳","衡阳","塔城","康定","文山","连城","金昌","阿尔山",
                             "伊春","中卫","漠河","那拉提","张掖","百色","哈密","通化","荔波","且末",
                             "喀纳斯","永州","天水","阿里","固原","梧州","日喀则","吐鲁番","九江","安康","富蕴"
                            };
        public ObservableCollection<AirlinesTime> AirlineList { get; set; }

        public Flight()
        {
            this.InitializeComponent();
            AirlineList = new ObservableCollection<AirlinesTime>();
            BeforeSearch.Visibility = Visibility.Visible;
            MyCalendar.PlaceholderText =System.DateTime.Now.ToString("d").Replace("/","-");
            FlightStart.ItemsSource = Airport;
            FlightEnd.ItemsSource = Airport;
        }

        private static async Task<Air> GetInfo(string startcity,string lastcity,string date)
        {
            var NoAir = new Air();
            try {
                var url = "http://ws.webxml.com.cn/webservices/DomesticAirline.asmx/getDomesticAirlinesTime?startcity=" + startcity + "&lastcity=" + lastcity + "&theDate=" + date + "&userID=";
                HttpClient http = new HttpClient();
                var response = await http.GetAsync(url);
                var Message = await response.Content.ReadAsStringAsync();
                var Message_Slice1 = Message.Substring(0, (Message.Length - 34)).Substring(1506);
                var FinalMessage = "<Air> " + Message_Slice1.Replace("xmlns=\"\"", " ").Replace("diffgr:", "").Replace("msdata:", "") + " </Air>";
                var serializer = new XmlSerializer(typeof(Air));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(FinalMessage));
                var result = (Air)serializer.Deserialize(ms);
                return result;
            }
            catch (Exception) {
                return NoAir;
            }
        }

        
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            BeforeSearch.Visibility = Visibility.Collapsed;
            DuringSearch.Visibility = Visibility.Visible;
            var startcity = "";
            var lastcity ="";
            var date = "";
            if (FlightStart.Text != "")
            {
                startcity = FlightStart.Text;
            }
            else if(FlightStart.Text=="")
            {
                startcity = "北京";
            }
            if (FlightEnd.Text != "")
            {
                lastcity = FlightEnd.Text;
            }
            else if (FlightEnd.Text == "")
            {
                lastcity ="上海";
            }
            if (MyCalendar.Date != null)
            {
                date = MyCalendar.Date.Value.Year + "-" + MyCalendar.Date.Value.Month + "-" + MyCalendar.Date.Value.Day;
            }
            else if (MyCalendar.Date == null)
            {
                date = System.DateTime.Now.ToString("d").Replace("/","-");
            }
            var AirInfo = await GetInfo(startcity, lastcity, date);
            if (AirInfo.Airlines != null)
            {
                var AirlineArray = AirInfo.Airlines;
                AirlineList.Clear();
                DuringSearch.Visibility = Visibility.Collapsed;
                AfterSearch.Visibility = Visibility.Visible;
                foreach (var AirlineItem in AirlineArray)
                {
                    AirlineList.Add(AirlineItem);
                }
            }
            else if (AirInfo.Airlines == null) {
                var dialog = new MessageDialog("获取航班信息失败，请检查网络服务是否开启", "消息提示");
                dialog.Commands.Add(new UICommand("确定", cmd => { }, commandId: 0));
                await dialog.ShowAsync();
                AfterSearch.Visibility = Visibility.Collapsed;
                DuringSearch.Visibility = Visibility.Collapsed;
                BeforeSearch.Visibility = Visibility.Visible;
            }
           
        }

        private void FlightStart_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            var autoSuggestBox = (AutoSuggestBox)sender;
            var filtered = Airport.Where(p => p.StartsWith(autoSuggestBox.Text)).ToArray();
            autoSuggestBox.ItemsSource = filtered;
        }

        private void FlightEnd_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            var autoSuggestBox = (AutoSuggestBox)sender;
            var filtered = Airport.Where(p => p.StartsWith(autoSuggestBox.Text)).ToArray();
            autoSuggestBox.ItemsSource = filtered;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AfterSearch.Visibility = Visibility.Collapsed;
            BeforeSearch.Visibility = Visibility.Visible;
        }
    }
}
