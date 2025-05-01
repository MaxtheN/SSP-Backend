using System.Collections.Generic;
using System;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using WEBASE.i18n;
using WEBASE.Storage;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using System.Web;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class BaseReportService :
        IBaseReportService
    {
        private readonly IStorageService _storageService;
        private readonly ICultureHelper _cultureHelper;

        public BaseReportService(
            IStorageService storageService,
            ICultureHelper cultureHelper)
        {
            _storageService = storageService;
            _cultureHelper = cultureHelper;
        }
        public MemoryStream GetFileFromStorage(string fileName, string lang = null)
        => _storageService.GetStaticFile(StaticFileConst.HtmlTemplate.GetFileName(
            lang ?? _cultureHelper.CurrentCulture.Code
            , fileName));


        private string FillTable(string shablon,
                                 Dictionary<string, List<string>> data)
        {
            int shablonCount = data.Values.FirstOrDefault().Count;
            List<string> allShablons = new List<string>();
            string finalShablon = "";

            for (int i = 0; i < shablonCount; i++)
            {
                allShablons.Add(shablon);
                //______________________________
                foreach (string key in data.Keys)
                {
                    if (allShablons[i].Contains(key))
                        allShablons[i] = allShablons[i].Replace(key, data[key][i]);
                    else
                        allShablons[i] = allShablons[i].Replace(key, "");
                }
                //______________________________
                finalShablon += allShablons[i];
            }
            return finalShablon;
        }

        public string ReturnReadyHtmlAsString(string filename,
                                       Dictionary<string, string> data,
                                       Dictionary<string, Dictionary<string, List<string>>> tableData,
                                       string lang = null)
        {
            var html = GetFileFromStorage(filename, lang);
            string htmlStream = html.ReadAsString();

            if (tableData != null)
                foreach (var oneShablon in tableData)
                {
                    int begin = htmlStream.IndexOf($"<!--$start-array-{oneShablon.Key}-->");
                    int end = htmlStream.IndexOf($"<!--$end-array-{oneShablon.Key}-->") + $"<!--$end-array-{oneShablon.Key}-->".Length;
                    string shablon = htmlStream.Substring(begin, end - begin);
                    htmlStream = htmlStream.Replace(shablon, FillTable(shablon, oneShablon.Value));
                }

            if (data != null)
                foreach (var (key, value) in data)
                    if (htmlStream.Contains(key))
                        htmlStream = htmlStream.Replace(key, value);
                    else
                        htmlStream = htmlStream.Replace(key, "");

            return htmlStream;
        }

        public Byte[] ReturnReadyPdf(string htmlString)
        {
            //string tempPath = "appdata/tempfiles/";
            //string tempHtmlFileName = $"{tempPath}{Guid.NewGuid()}.html";
            //File.WriteAllText(tempHtmlFileName, htmlString);
            //string pdfFileName = "";
            //pdfFileName = PdfHelper.HtmlToPdf(tempHtmlFileName, false);
            //byte[] bytes = File.ReadAllBytes(pdfFileName);
            //File.Delete(tempHtmlFileName);
            //File.Delete(pdfFileName);
            //return bytes;


            var workStream = new MemoryStream();

            using (var pdfWriter = new PdfWriter(workStream))
            {
                pdfWriter.SetCloseStream(false);

                var pdfDocument = new PdfDocument(pdfWriter);

                var documentConvertProperties = new ConverterProperties();
                using (var document = HtmlConverter.ConvertToDocument(htmlString, pdfDocument, documentConvertProperties))
                {
                    document.SetBottomMargin(0);
                    document.SetTopMargin(0);
                    document.Flush();
                }
            }

            workStream.Position = 0;
            return workStream.ToArray();
        }

        public string QRCodeImageAsBase64(string text)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                QRCoder.QRCodeGenerator QRgenerator = new QRCoder.QRCodeGenerator();

                QRCoder.QRCodeData QRdata
                    = QRgenerator.CreateQrCode(text, QRCoder.QRCodeGenerator.ECCLevel.Q);

                QRCoder.QRCode finalCode = new QRCoder.QRCode(QRdata);

                using (Bitmap drawQR = finalCode.GetGraphic(20,
                                                            "#063970",
                                                            "#FFFFFF",
                                                            true))
                {
                    drawQR.Save(ms,
                                ImageFormat.Png);

                    var imageCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());

                    return imageCode;
                }
            }
        }
    }
}
