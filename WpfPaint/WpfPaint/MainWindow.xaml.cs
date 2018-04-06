using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpfSyntezator;

namespace WpfPaint
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<PaintColors> listColors;
        Serwer serwerTCP;
        IPAddress localIPs;

        public MainWindow()
        {
            InitializeComponent();
            listColors = new List<PaintColors>();

            Type colorType = typeof(System.Windows.Media.Brushes);
            // We take only static property to avoid properties like Name, IsSystemColor ...
            PropertyInfo[] propInfos = colorType.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);

            foreach (PropertyInfo propInfo in propInfos)
            {
                string name = propInfo.Name;
                SolidColorBrush brush = (SolidColorBrush)propInfo.GetValue(null, null);

                Color color = brush.Color;
                listColors.Add(new PaintColors() { ColorName = propInfo.Name, BrushColor = brush, ColorValue = brush.Color });
            }

            lvColors.ItemsSource = listColors;

            localIPs = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where((r) => !r.IsIPv6LinkLocal).FirstOrDefault();
            lblIp.Content += " " + localIPs;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            paint.EditingMode = InkCanvasEditingMode.None;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            paint.EditingMode = InkCanvasEditingMode.Select;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            paint.EditingMode = InkCanvasEditingMode.InkAndGesture;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            paint.EditingMode = InkCanvasEditingMode.Ink;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            paint.EditingMode = InkCanvasEditingMode.GestureOnly;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            paint.EditingMode = InkCanvasEditingMode.EraseByStroke;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            paint.EditingMode = InkCanvasEditingMode.EraseByPoint;
        }

        private void paint_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    //paint.DefaultDrawingAttributes.Height = paint.DefaultDrawingAttributes.Height + 1
                    paint.DefaultDrawingAttributes.Height++;
                }
                else
                    paint.DefaultDrawingAttributes.Width++;
            }
            else
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) && paint.DefaultDrawingAttributes.Height > 1)
                {
                    paint.DefaultDrawingAttributes.Height--;
                }
                else if (paint.DefaultDrawingAttributes.Width > 1)

                    paint.DefaultDrawingAttributes.Width--;
            }

            //infoHeight.Text = "Wysokość pędzla: " + Math.Round(paint.DefaultDrawingAttributes.Height, 0);
            //infoWidth.Text = "Szerokość pędzla: " + Math.Round(paint.DefaultDrawingAttributes.Width, 0);
        }

        private void lvColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = lvColors.SelectedItem as PaintColors;

            paint.DefaultDrawingAttributes.Color = selected.ColorValue;
        }


        private void paint_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {

        }

        private void paint_StrokeErased(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("paint_StrokeErased");
            var ddd = paint.EraserShape;
        }

        private void paint_StrokesReplaced(object sender, InkCanvasStrokesReplacedEventArgs e)
        {

        }

        private void paint_SelectionMoved(object sender, EventArgs e)
        {
            var ddd = paint.GetSelectedStrokes();
        }

        private void paint_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void paint_StrokeErasing(object sender, InkCanvasStrokeErasingEventArgs e)
        {
            Console.WriteLine("paint_StrokeErasing");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            paint.Strokes.Clear();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            sfd.Filter = "Bmp (*.bmp)|*.bmp|Jpge (*.jpge)|*.jpg|Png (*.png)|*.png|Storke (*.*)|*.*";

            bool? result = sfd.ShowDialog(this);
            if (result.HasValue && result.Value)
            {

                var extensio = System.IO.Path.GetExtension(sfd.FileName);
                if (String.IsNullOrEmpty(extensio))
                {
                    using (FileStream fs = File.Open(sfd.FileName, FileMode.Create))
                    {
                        paint.Strokes.Save(fs);
                    }
                }
                else
                {
                    RenderTargetBitmap rtb = new RenderTargetBitmap((int)paint.ActualWidth, (int)paint.ActualHeight, 96d, 96d, PixelFormats.Default);
                    rtb.Render(paint);

                    BmpBitmapEncoder enc = new BmpBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create(rtb));

                    using (FileStream fs = File.Open(sfd.FileName, FileMode.Create))
                    {
                        enc.Save(fs);
                    }
                }

            }

        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            bool? result = ofd.ShowDialog(this);

            if (result.HasValue && result.Value)
            {
                var extensio = System.IO.Path.GetExtension(ofd.FileName);
                if (String.IsNullOrEmpty(extensio))
                {
                    using (var fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read))
                    {
                        StrokeCollection strokes = new StrokeCollection(fs);
                        paint.Strokes = strokes;
                    }
                }
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (serwerTCP != null && serwerTCP.ServerActive)
            {
                try
                {
                     serwerTCP.StopSerwer();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                serwerTCP = new Serwer(localIPs.ToString(), Int32.Parse(txtPort.Text));
                serwerTCP.StartSerwer();
            }
        }

    }
}
