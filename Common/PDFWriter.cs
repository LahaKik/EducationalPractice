using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Fonts;
using PdfSharp.Snippets.Font;

namespace Common
{
    public static class PDFWriter
    {
        public static void GeneratePDF(string filename, string imagePath, string[] text )
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            GlobalFontSettings.FontResolver = new SegoeWpFontResolver();
            XFont font = new XFont("Segoe WP", 12);
            for (int i = 0; i < text.Length; i++)
            {
                gfx.DrawString(text[i], font, XBrushes.Black, 50, 50 + 25*i);
            }
            DrawImage(gfx, imagePath, 0, 300, 300, 300);
            document.Save(filename);
        }

        private static void DrawImage(XGraphics gfx, string imgPath, int x, int y, int width, int height)
        {
            XImage image = XImage.FromFile(imgPath);
            gfx.DrawImage(image, x, y, width, height);
        }
    }
}
