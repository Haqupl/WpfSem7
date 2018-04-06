using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace JsonToView
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string urlCoin = @"https://www.cryptopia.co.nz/api/GetCurrencies";
        private ObservableCollection<Data> ListData;

        public MainWindow()
        {
            InitializeComponent();

        }


        private async Task<WebResponse> GetCurrencies(string url)
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.Method = WebRequestMethods.Http.Get;
            request.ContentType = "application/json";
            return await request.GetResponseAsync();
        }

        private async Task<string> GetStreamFromResponse()
        {
            var response = await GetCurrencies(urlCoin);
            using (var responseReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return await responseReader.ReadToEndAsync();
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            var stringJson = await GetStreamFromResponse();
            RootJson deserializedProduct = JsonConvert.DeserializeObject<RootJson>(stringJson);

            pbWait.Visibility = System.Windows.Visibility.Collapsed;
            this.Cursor = Cursors.Arrow;
            ListData = new ObservableCollection<Data>(deserializedProduct.Data);
            lvData.ItemsSource = ListData;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string txt = txtSzukaj.Text.ToLower().Trim();
            if (txt.Length > 0)
            {
                lvData.ItemsSource = ListData.Where((d) => (d.Name.ToLower().StartsWith(txt)));
            }
            else
            {
                lvData.ItemsSource = ListData;
            }
        }
    }
}
