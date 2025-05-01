using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Claim;

public class ClaimManualService : StatusGenericHandler, IClaimManualService
{
    private readonly DbContext _context;

    public ClaimManualService(DbContext context)
    {
        this._context = context;
    }

    public SelectList<int> ClaimApplicationTypeSelectList(int? langId)
    {
        return _context.Set<ClaimApplicationType>().Include(a => a.Translates).AsSelectList(langId);
    }

    public SelectList<int> MediationTypeSelectList()
    {
        return _context.Set<MediationType>().Include(a => a.Translates).AsSelectList();
    }

    public SelectList<int> ClaimResponsibleTypeSelectList(int? langId)
    {
        return _context.Set<ClaimResponsibleType>().Include(a => a.Translates).AsSelectList(langId);
    }

    public SelectList<int> MediationResultSelectList()
    {
        return _context.Set<MediationResult>().Include(a => a.Translates).AsSelectList();
    }

    public SelectList<int> ClaimNeedCourtSelectList()
    {
        return _context.Set<ClaimNeedCourt>().Include(a => a.Translates).AsSelectList();
    }
}