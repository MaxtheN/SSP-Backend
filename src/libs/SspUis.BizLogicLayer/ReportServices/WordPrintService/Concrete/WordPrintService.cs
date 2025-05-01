using Microsoft.AspNetCore.Hosting;
using Spire.Doc;
using StatusGeneric;
using System.IO;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class WordPrintService :
        StatusGenericHandler,
        IWordPrintService
    {
        private readonly IStorageService _storageService;
        private readonly IWebHostEnvironment _environment;

        public WordPrintService(
            IStorageService storageService,
            IWebHostEnvironment environment)
        {
            _storageService = storageService;
            _environment = environment;
        }
        public MemoryStream GetFileFromStorage(string fileName)
       => _storageService.GetStaticFile(StaticFileConst.WordTemplate.GetFileName("ru"
           , fileName));
        
        
        
        public byte[] ConvertWordStreamToPdf()
        {
            var fileName = Path.Combine(_environment.ContentRootPath ,"appdata\\staticfiles",StaticFileConst.WordTemplate.GetFileName("ru", "ownership_contract_district_AB.docx"));

            var wordTemplate = new WordprocessingWorker(fileName);
            wordTemplate.AddPropertyKeyValue("${DocNumber}:", "213423-23443");
            wordTemplate.Save();
            var wordStream = wordTemplate.GetStream();


            return ConvertWordToPdf(wordStream);
        }
        public byte[] ConvertWordToPdf(Stream wordStream)
        {
            byte[] pdfBytes;

            // Load the Word document from the stream
            Document document = new Document();
            document.LoadFromStream(wordStream, Spire.Doc.FileFormat.Docx);

            // Convert the Word document to PDF
            using (MemoryStream pdfStream = new MemoryStream())
            {
                document.SaveToStream(pdfStream, Spire.Doc.FileFormat.PDF);
                pdfBytes = pdfStream.ToArray();
            }

            return pdfBytes;
        }
    }
}
