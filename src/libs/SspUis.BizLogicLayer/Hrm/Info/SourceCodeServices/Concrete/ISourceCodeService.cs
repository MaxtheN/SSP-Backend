using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.SourceCodeServices
{
    public interface ISourceCodeService : IStatusGeneric
    {
        PagedResult<SourceCodeListDto> GetList(SortFilterPageOptions dto);
        SourceCodeDto Get();
        SourceCodeDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateSourceCodeDlDto dto);
        void Update(UpdateSourceCodeDlDto dto);
        void Delete(int id);
    }
}
