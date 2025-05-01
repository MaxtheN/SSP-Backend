using SspUis.DataLayer.EfClasses.Hrm;
using StatusGeneric;
using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm
{
    public interface IDocumentHeldForSignService : IStatusGeneric
    {
        PagedResult<HrmSignerVtView> GetData(DocumentHeldForSignFilterOption dto);
    }
}
