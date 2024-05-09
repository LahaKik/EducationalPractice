using System.Drawing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing;
using ZXing.Windows.Compatibility;
using System.Text.Json;
using System.Windows.Media.Imaging;

namespace Common
{
    public static class QRCoder
    {
        public static Bitmap CreateQRBitmap(string str)
        {
            QRCodeWriter qrCodeWriter = new QRCodeWriter();
            Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>
            {
                { EncodeHintType.ERROR_CORRECTION, "Q" },
                //{EncodeHintType.DISABLE_ECI, true }
            };
            BitMatrix matrix = qrCodeWriter.encode(str, BarcodeFormat.QR_CODE, 300, 300, hints);

            BarcodeWriter barcode = new BarcodeWriter();
            Bitmap bitmap = barcode.Write(matrix);
            return bitmap;
        }
        public static User? ReadQR(string path)
        {
            BarcodeReader barcode = new BarcodeReader();
            try
            {            
                Bitmap bitmap = new Bitmap(path);
                Result result = barcode.Decode(bitmap);
                if (result == null)
                    return null;
                string userJson = result.Text;
                User? user = JsonSerializer.Deserialize<User>(userJson);
                return user; 
            }
            catch { return null; }
        }
    }
}
