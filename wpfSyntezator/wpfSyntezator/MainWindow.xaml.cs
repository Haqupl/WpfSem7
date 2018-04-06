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
using System.Speech.Synthesis;
using System.Net;

namespace wpfSyntezator
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SpeechSynthesizer synt;
        Serwer serw;

        public MainWindow()
        {
            InitializeComponent();

            synt = new SpeechSynthesizer();
            synt.SetOutputToDefaultAudioDevice();
            Console.WriteLine(synt.Voice.Gender);
            synt.SpeakProgress += synt_SpeakProgress;
            var localIPs = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where((r) => !r.IsIPv6LinkLocal).FirstOrDefault();
            txtInput.Text = "Ip " + localIPs + " Port: 13000";
            serw = new Serwer(localIPs.ToString(), 13000);
            serw.Refresh += serw_Refresh;


            lvWiadomosc.ItemsSource = serw.odebraneDane;

        }

        private void serw_Refresh(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() => lvWiadomosc.Items.Refresh());
        }

        void synt_SpeakProgress(object sender, SpeakProgressEventArgs e)
        {
            Console.WriteLine(e.AudioPosition + " " + e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (txtInput.Text.Length > 0)
            //{
            //    synt.Speak(txtInput.Text.Trim());
            //}
            //lvWiadomosc.Items.Refresh();
            Task.Run(() => czytajWiaomosc());

        }

        void czytajWiaomosc()
        {
            while (true)
            {
                if (synt.State == SynthesizerState.Ready)
                {
                    ClientResive wiersz;
                    lock (serw.odebraneDane)
                    {
                        wiersz = serw.odebraneDane.Where((r) => !r.IsReaded).FirstOrDefault();
                    }

                    if (wiersz != null)
                    {
                        string powiedz = String.Format("Do {0} : Wiadomość: {1}", wiersz.IpEndPoint, wiersz.sData);
                        wiersz.IsReaded = true;
                        synt.Speak(powiedz);
                        serw_Refresh(null, null);
                    }
                }
                else
                    System.Threading.Thread.Sleep(200);
            }
        }


        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            BtnRun.IsEnabled = false;
            BtnStop.IsEnabled = true;
            serw.StartSerwer();
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            BtnRun.IsEnabled = true;
            BtnStop.IsEnabled = false;
            serw.StopSerwer();
        }
    }
}
