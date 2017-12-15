using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WpfZaj_1
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Osoba> listaOsob;
        public MainWindow()
        {
            InitializeComponent();
            listaOsob = new List<Osoba>()
            {
                new Osoba("Jan","Kowalski",45),
                new Osoba("Mirex","Mik",34)
            };

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            ofd.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All file|*.*";
            bool? wybrano = ofd.ShowDialog();

            if (wybrano.HasValue && wybrano.Value)
            {
                LabPath.Content = ofd.FileName;
                Uri path = new Uri(ofd.FileName);
                Img.Source = new BitmapImage(path);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string sSath = TxtPath.Text;
            if (System.IO.File.Exists(sSath))
            {
                Uri path = new Uri(sSath);
                Img.Source = new BitmapImage(path);
            }
            else
                TxbPath2.Text = "Nie znaleziono pliku";

          
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LbDane.ItemsSource = listaOsob;
        }
    }
}
