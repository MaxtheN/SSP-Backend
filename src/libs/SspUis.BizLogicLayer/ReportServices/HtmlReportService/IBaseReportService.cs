using StatusGeneric;
using System.Collections.Generic;
using System;
using System.IO;

namespace SspUis.BizLogicLayer.ReportServices
{
    public interface IBaseReportService
    {
        MemoryStream GetFileFromStorage(string fileName, string lang = null);

        string ReturnReadyHtmlAsString(string filename,
                                       Dictionary<string, string> data,
                                       Dictionary<string, Dictionary<string, List<string>>> tableData,
                                       string lang = null);

        Byte[] ReturnReadyPdf(string htmlString);
        string QRCodeImageAsBase64(string text);
    }
}
