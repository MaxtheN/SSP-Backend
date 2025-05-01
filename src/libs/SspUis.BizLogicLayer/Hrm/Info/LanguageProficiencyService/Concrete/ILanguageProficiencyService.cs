using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public interface ILanguageProficiencyService : IStatusGeneric
{
    PagedResult<LanguageProficiencyListDto> GetList(SortFilterPageOptions dto);
    LanguageProficiencyDto Get();
    LanguageProficiencyDto Get(int id);
    SelectList<int> AsSelectList();
    HaveId<int> Create(CreateLanguageProficiencyDlDto dto);
    void Update(UpdateLanguageProficiencyDlDto dto);
    void Delete(int id);
}
