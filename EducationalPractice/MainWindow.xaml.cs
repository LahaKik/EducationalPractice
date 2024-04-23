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


namespace EducationalPractice
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TButt_Click(object sender, RoutedEventArgs e)
        {
            string tStr = "test string;741416 to QR-Code";
            QRCodeWriter qrCodeWriter = new QRCodeWriter();
            Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>
            {
                { EncodeHintType.ERROR_CORRECTION, "Q" }
            };
            BitMatrix matrix = qrCodeWriter.encode(tStr, BarcodeFormat.QR_CODE, 300, 300, hints);

            BarcodeWriter barcode = new BarcodeWriter();
            Bitmap bitmap = barcode.Write(matrix);
            SaveFileDialog fileDialog = new SaveFileDialog
            {
                Title = "Выберите путь для сохранения",
                OverwritePrompt = true,
                Filter = "PNG-изображение|*.png",
                DefaultExt = ".png",
                
            };
            string filename = "";
            if(fileDialog.ShowDialog() == true)
            {
                filename = fileDialog.FileName;
                bitmap.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
            }
            else
            {
                MessageBox.Show("Файл не сохранен", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            

            
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
    }
}