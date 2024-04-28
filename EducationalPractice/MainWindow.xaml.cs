using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using System.Drawing;
using ZXing.Windows.Compatibility;
using Microsoft.Win32;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.IO;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;


namespace EducationalPractice
{
    public partial class MainWindow : Window
    {
        private static int _nqrcashe = 1;
        private static int NQRCashe 
        {
            get
            {
                return _nqrcashe++;
            }
        }

        delegate void QRHandler(string path);
        event QRHandler QRCreated;

        UsersDB db = new UsersDB();
        public MainWindow()
        {
            InitializeComponent();
            Directory.CreateDirectory(Environment.CurrentDirectory + "\\Cache");
            Loaded += MainWindow_Loaded;
            QRCreated += ApplyImage;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            db.Database.EnsureCreated();
            db.Users.Load();
            DataContext = db.Users.Local.ToObservableCollection();
            
        }

        private void TButt_Click(object sender, RoutedEventArgs e)
        {
            string tStr = "test string;741416 to QR-Code";
            Bitmap bitmap = CreateQRBitmap(tStr);
            
        }
        private void LoButt_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Title = "Выберите путь для сохранения",
                Filter = "PNG-изображение|*.png",
                
                DefaultExt = ".png"
            };
            string filename = "";
            if (fileDialog.ShowDialog() == true)
            {
                filename = fileDialog.FileName;
                img.Source = BitmapFrame.Create(new Uri(filename));
            }
            else
            {
                MessageBox.Show("Файл не загружен", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Add_CLK(object sender, RoutedEventArgs e)
        {

        }

        private void Edit_CLK(object sender, RoutedEventArgs e)
        {

        }

        private void CreateQR_DCLK(object sender, MouseButtonEventArgs e)
        {
            DataGrid? dataGrid = sender as DataGrid;
            if(dataGrid != null)
            {
                User? user = dataGrid.SelectedItem as User;
                if(user != null)
                {
                    if(user.QRPath == null)
                    {
                        string UserString = JsonSerializer.Serialize<User>(user);
                        Bitmap bitmap = CreateQRBitmap(UserString);
                        string path = Environment.CurrentDirectory + "\\Cache\\" + "QR" + NQRCashe.ToString() + ".png";
                        user.QRPath = path;
                        bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                        QRCreated?.Invoke(path);
                    }
                    else
                    {
                        QRCreated?.Invoke(user.QRPath);
                    }
                }
            }
        }

        private static Bitmap CreateQRBitmap(string str)
        {
            QRCodeWriter qrCodeWriter = new QRCodeWriter();
            Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>
            {
                { EncodeHintType.ERROR_CORRECTION, "Q" }
            };
            BitMatrix matrix = qrCodeWriter.encode(str, BarcodeFormat.QR_CODE, 300, 300, hints);

            BarcodeWriter barcode = new BarcodeWriter();
            Bitmap bitmap = barcode.Write(matrix);
            return bitmap;
        }
        private void ApplyImage(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = stream;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                img.Source = bmp;
            }
        }

        private void SaveButt_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog
            {
                Title = "Выберите путь для сохранения",
                OverwritePrompt = true,
                Filter = "PNG-изображение|*.png",
                DefaultExt = ".png",

            };

            string filename = "";
            if (fileDialog.ShowDialog() == true)
            {
                filename = fileDialog.FileName;
                //File.Copy()
            }
            else
            {
                MessageBox.Show("Файл не сохранен", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}