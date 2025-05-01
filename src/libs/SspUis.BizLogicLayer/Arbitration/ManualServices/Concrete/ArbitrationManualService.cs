using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.ArbitrationCourtApplicationServices;
using SspUis.DataLayer.EfClasses;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Arbitration;

public class ArbitrationManualService : StatusGenericHandler, IArbitrationManualService
{
    private readonly DbContext _context;

    public ArbitrationManualService(DbContext context)
    {
        this._context = context;
    }

    public SelectList<int> ArbitrationApplicationTypeSelectList()
    {
        return _context.Set<ArbitrationApplicationType>().Include(a => a.Translates).AsSelectList();
    }

    public SelectList<int> ArbitrationCourtSelectList()
    {
        return _context.Set<ArbitrationCourt>().Include(a => a.Translates).AsSelectList();
    }

    public SelectList<int> ArbitrationCourtResultSelectList()
    {
        return _context.Set<ArbitrationCourtResult>().Include(a => a.Translates).AsSelectList();
    }
}