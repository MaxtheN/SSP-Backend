/*using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class WordResult : FileResult
    {
        private const int BufferSize = 0x1000;
        
        public WordResult(WordprocessingWorker worker)
            : base(WordConstants.WORDPROCESSING_CONTENT_TYPE)
        {
            Worker = worker;
            Stream = worker.GetStream();
        }

        *//*public WordResult(WordprocessingWorker worker, string fileDownloadName)
            : this(worker)
        {
            FileDownloadName = string.Format("{0}.{1}.docx", fileDownloadName, CultureHelper.CurrentCultureName);
        }*//*

        public WordResult(Stream stream)
            : base (WordConstants.WORDPROCESSING_CONTENT_TYPE)
        {
            Stream = stream;
        }

        public WordResult(Stream stream, string fileDownloadName)
            : this(stream)
        {
            FileDownloadName = string.Concat(fileDownloadName, ".docx");
        }
               
        public Stream Stream { get; private set; }

        public WordprocessingWorker Worker { get; private set; }
*//*
        protected override void WriteFile(HttpResponseBase response)
        {
            Stream outputStream = response.OutputStream;
            byte[] buffer = new byte[BufferSize];

            while (true)
            {
                int bytesRead = Stream.Read(buffer, 0, BufferSize);
                
                if (bytesRead == 0)
                    break;

                outputStream.Write(buffer, 0, bytesRead);
            }

            if (Worker == null)
                Stream.Dispose();
            else
                Worker.Dispose();
            
        }*//*

    }
}*/