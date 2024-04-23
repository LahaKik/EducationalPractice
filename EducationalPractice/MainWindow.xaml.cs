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
            string tStr = "test string to QR-Code";
            QRCodeWriter qrCodeWriter = new QRCodeWriter();

            BitMatrix matrix = qrCodeWriter.encode(tStr, BarcodeFormat.QR_CODE, 300, 300);

            BarcodeWriter barcode = new BarcodeWriter();
            Bitmap bitmap = barcode.Write(matrix);

            bitmap.Save("QR.png", System.Drawing.Imaging.ImageFormat.Png);

            img.Source = new BitmapImage(new Uri("bin\\Debug\\net8.0-windows\\QR.png", UriKind.Relative));
        }
    }
}