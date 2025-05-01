using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Memship;

public interface IMemshipManualService : IStatusGeneric
{
    SelectList<int> ContractorCategorySelectList();
    SelectList<int> MemshipContractTypeSelectList();
    SelectList<int> OpfSelectList();
}
