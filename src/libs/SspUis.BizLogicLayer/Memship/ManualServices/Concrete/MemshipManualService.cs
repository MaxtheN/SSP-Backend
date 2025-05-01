using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Memship;

public class ClaimManualService : StatusGenericHandler, IMemshipManualService
{
    private readonly DbContext _context;

    public ClaimManualService(DbContext context)
    {
        this._context = context;
    }

    public SelectList<int> ContractorCategorySelectList()
    {
        return _context.Set<ContractorCategory>().Include(a => a.Translates).AsSelectList();
    }

    public SelectList<int> MemshipContractTypeSelectList()
    {
        return _context.Set<MemshipContractType>().Include(a => a.Translates).AsSelectList();
    }
    public SelectList<int> OpfSelectList()
    {
        return _context.Set<Opf>().Include(a => a.Translates).AsSelectList();
    }
}