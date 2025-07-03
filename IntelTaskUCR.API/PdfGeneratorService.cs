using DinkToPdf;
using DinkToPdf.Contracts;

namespace IntelTaskUCR.API.Services
{
    public static class PdfGeneratorService
    {
        public static byte[] GenerarPdfDesdeHtml(string html)
        {
            var converter = new SynchronizedConverter(new PdfTools());

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = {
                    new ObjectSettings() {
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            return converter.Convert(doc);
        }
    }
}
