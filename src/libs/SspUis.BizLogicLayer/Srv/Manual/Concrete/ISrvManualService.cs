using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public interface ISrvManualService
    {
        SelectList<int> ServicePriceTypeSelectList();
    }
}
