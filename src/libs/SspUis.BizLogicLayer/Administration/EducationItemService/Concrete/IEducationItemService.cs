using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public interface IEducationItemService : IStatusGeneric
{
    PagedResult<EducationItemListDto> GetList(SortFilterPageOptions dto);
    EducationItemDto Get();
    EducationItemDto Get(int id);
    SelectList<int> AsSelectList();
    HaveId<int> Create(CreateEducationItemDlDto dto);
    void Update(UpdateEducationItemDlDto dto);
    void Delete(int id);
}
