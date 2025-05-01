using SspUis.DataLayer.EfClasses.Hrm;
using StatusGeneric;
using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm
{
    public interface IDocumentHeldForEmpService : IStatusGeneric
    {
        PagedResult<HrmEmpVtView> GetData(DocumentHeldForEmpFilterOption dto);
    }
}
