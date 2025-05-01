using StatusGeneric;
using System;
using System.IO;

namespace SspUis.BizLogicLayer.ReportServices
{
    public interface IHtmlReportService :
        IStatusGeneric
    {
        byte[] DownloadApplicationPdf(Guid id2, string lang = null);
        string DownloadApplicationAsHtml(long Id, Guid? id2 = null, string lang = null);
        byte[] DownloadCertificatePdf(Guid Id2, string lang = null);
        string DownloadCertificateAsHtml(Guid Id2, string lang = null);
        byte[] DownloadPrtnContractPdf(Guid Id2, string lang = null);
        string DownloadPrtnContractAsHtml(Guid Id2, string lang = null);
        byte[] TestPdf();
        Stream Print(PdfPrintDto dto);
    }
}
