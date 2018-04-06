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

namespace WpfApplication1
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double liczba1, liczba2;
        private char operacja;
        private bool comma;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button przyslany_przycisk = sender as Button;
          
            if (przyslany_przycisk.Content.ToString() != ",")
            {
                var pobranaLiczba = Double.Parse(przyslany_przycisk.Content.ToString());
                if (operacja == '\0')
                {
                    if (!comma)
                    {
                        liczba1 = (liczba1 * 10) + pobranaLiczba;
                    }
                    else
                    {   //1,2
                        //1,23
                        var liczbaPoPrzecinku = Math.Round(liczba1 - Math.Truncate(liczba1),10);
                        var iloscZnakowPo = liczbaPoPrzecinku.ToString().Replace("0,","").Length;
                        liczba1 += pobranaLiczba * (Math.Pow(0.1, iloscZnakowPo));
                    }
                   
                    txbWyswietlacz.Text = liczba1.ToString();
                }
                else
                {
                    liczba2 = (liczba2 * 10) + pobranaLiczba;
                    txbWyswietlacz.Text += przyslany_przycisk.Content.ToString();
                }

            }
            else
            {
                comma = true;
            }

        }

        private void ButtonOperation_Click(object sender, RoutedEventArgs e)
        {
            Button przyslany_przycisk = sender as Button;
            var _operacja = Char.Parse(przyslany_przycisk.Content.ToString());

            switch (_operacja)
            {

                case 'C':
                    operacja = '\0';
                    liczba1 = 0;
                    liczba2 = 0;
                    txbWyswietlacz.Text = "";
                    comma = false;
                    break;
                case '=':
                    txbWyswietlacz.Text = pobierzWynik().ToString();
                    liczba1 = 0;
                    liczba2 = 0;
                    comma = false;
                    operacja = '\0';
                    break;

                default:
                    operacja = _operacja;
                    break;
            }
            if (operacja != '\0') txbWyswietlacz.Text += " " + operacja + " ";
        }

        private double pobierzWynik()
        {
            switch (operacja)
            {
                case '+':
                    return liczba1 + liczba2;
                case '-':
                    return liczba1 - liczba2;
                case '*':
                    return liczba1 * liczba2;
                case '/':
                    return liczba1 / liczba2;
                case '%':
                    return liczba1 % liczba2;
                default:
                    return -1;
            }
        }

    }
}
