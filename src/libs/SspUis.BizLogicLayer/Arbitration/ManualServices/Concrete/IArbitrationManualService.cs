using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Arbitration;

public interface IArbitrationManualService : IStatusGeneric
{
    SelectList<int> ArbitrationApplicationTypeSelectList();
    SelectList<int> ArbitrationCourtResultSelectList();
    SelectList<int> ArbitrationCourtSelectList();
}