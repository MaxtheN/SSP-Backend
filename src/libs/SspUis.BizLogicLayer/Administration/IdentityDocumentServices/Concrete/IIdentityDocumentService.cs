using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.IdentityDocumentServices
{
    public interface IIdentityDocumentService : IStatusGeneric
    {
        PagedResult<IdentityDocumentListDto> GetList(SortFilterPageOptions dto);
        IdentityDocumentDto Get();
        IdentityDocumentDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateIdentityDocumentDlDto dto);
        void Update(UpdateIdentityDocumentDlDto dto);
        void Delete(int id);
    }
}
