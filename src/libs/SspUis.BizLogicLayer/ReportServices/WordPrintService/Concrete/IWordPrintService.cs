using StatusGeneric;
using System.IO;

namespace SspUis.BizLogicLayer.ReportServices
{
    public interface IWordPrintService :
        IStatusGeneric
    {
        byte[] ConvertWordStreamToPdf();
    }
}
