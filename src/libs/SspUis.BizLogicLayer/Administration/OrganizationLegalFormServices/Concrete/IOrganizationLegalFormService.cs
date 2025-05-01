using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.OrganizationLegalFormServices
{
    public interface IOrganizationLegalFormService : IStatusGeneric
    {
        PagedResult<OrganizationLegalFormListDto> GetList(SortFilterPageOptions options);
        OrganizationLegalFormDto Get();
        OrganizationLegalFormDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateOrganizationLegalFormDlDto dto);
        void Update(UpdateOrganizationLegalFormDlDto dto);
        void Delete(int id);
    }
}
