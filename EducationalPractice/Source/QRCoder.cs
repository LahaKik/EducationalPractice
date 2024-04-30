using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Common;
using ZXing.QrCode;
using ZXing;
using ZXing.Windows.Compatibility;

namespace EducationalPractice
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
    }
}
