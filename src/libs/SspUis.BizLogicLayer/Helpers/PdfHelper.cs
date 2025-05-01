using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.IO.Image;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer
{
    public class PdfText
    {
        public Paragraph Value { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public TextAlignment Alignment { get; set; } = TextAlignment.LEFT;
        public int Page { get; set; } = 1;
        public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.BOTTOM;
        public int RadAngle { get; set; }
    }

    public class PdfImage
    {
        public byte[] Bytes { get; set; }
        public int? Left { get; set; }
        public int? Bottom { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
    }

    public class PdfHelper
    {
        public static string HtmlToPdf(
        string htmlFilePath,
        bool isLandscapeOrientation = false
        //Sides margin)
        )
        {
            string tempPath = "appdata/tempfiles/";
            var fileInfo = new FileInfo(htmlFilePath);
            string pdfFileName = $"{tempPath}{Guid.NewGuid()}.pdf";

            using (var htmlStream = File.Open(htmlFilePath, FileMode.Open, FileAccess.Read))
            {
                var writerProperties = new WriterProperties();

                using (var pdfWriter = new PdfWriter(pdfFileName, writerProperties))
                {
                    var pdfDocument = new PdfDocument(pdfWriter);
                    var pageSize = PageSize.A4;

                    if (isLandscapeOrientation)
                        pageSize = pageSize.Rotate();

                    var documentConvertProperties = new ConverterProperties();
                    DefaultFontProvider fontProvider = new DefaultFontProvider(true, true, true, "Times New Roman");
                    string fontDir = System.IO.Path.Combine("appdata", "staticfiles", "fonts");
                    if (Directory.Exists(fontDir))
                        fontProvider.AddDirectory(fontDir);
                    documentConvertProperties.SetFontProvider(fontProvider);

                    pdfDocument.SetDefaultPageSize(pageSize);

                    #region Convert to html
                    IList<IElement> elements = HtmlConverter.ConvertToElements(htmlStream, documentConvertProperties);
                    pdfDocument.SetTagged();
                    Document doc = new Document(pdfDocument, pageSize);
                    //doc.SetMargins(
                    //    (float)margin.Top,
                    //    (float)margin.Right,
                    //    (float)margin.Bottom,
                    //    (float)margin.Left);

                    foreach (IElement element in elements)
                        doc.Add((IBlockElement)element);
                    doc.Close();
                    #endregion

                    //HtmlConverter.ConvertToPdf(htmlStream, pdfDocument, documentConvertProperties);
                }
            }

            return pdfFileName;
        }

        public static void AddTextAndImageToPdf(
            Stream source,
            Stream destination,
            PdfText text,
            PdfImage image)
        {
            AddTextAndImageToPdf(
                source,
                destination,
                text == null ? null : new PdfText[] { text },
                image == null ? null : new PdfImage[] { image });
        }

        public static void AddTextAndImageToPdf(
            Stream source, 
            Stream destination, 
            IEnumerable<PdfText> texts,
            IEnumerable<PdfImage> images)
        {
            using (var pdfReader = new PdfReader(source))
            {
                using (var ms = new MemoryStream())
                {
                    using (var pdfWriter = new PdfWriter(ms))
                    {
                        var pdfDocument = new PdfDocument(pdfReader, pdfWriter);
                        var pageSize = PageSize.A4;
                        pdfDocument.SetDefaultPageSize(pageSize);
                        pdfDocument.SetTagged();
                        Document doc = new Document(pdfDocument);

                        if (images != null)
                            foreach (var img in images)
                            {
                                // Load image from disk
                                ImageData imageData = ImageDataFactory.Create(img.Bytes, false);
                                // Create layout image object and provide parameters. Page number = 1
                                Image image = new Image(imageData);//.ScaleAbsolute(100, 200).SetFixedPosition(1, 25, 25);

                                if (img.Left.HasValue || img.Bottom.HasValue)
                                    image = image.SetFixedPosition(img.Left.GetValueOrDefault(), img.Bottom.GetValueOrDefault());

                                if (img.Width.HasValue)
                                    image = image.SetWidth(img.Width.Value);
                                if (img.Height.HasValue)
                                    image = image.SetHeight(img.Height.Value);

                                doc.Add(image);
                            }

                        if (texts != null)
                            foreach (var text in texts)
                                doc.ShowTextAligned(text.Value, text.X, text.Y, text.Page, text.Alignment, text.VerticalAlignment, text.RadAngle);
                        doc.Close();
                        var bytes = ms.ToArray();
                        destination.Write(bytes, 0, bytes.Length);
                    }
                }
            }
        }

    }

}
