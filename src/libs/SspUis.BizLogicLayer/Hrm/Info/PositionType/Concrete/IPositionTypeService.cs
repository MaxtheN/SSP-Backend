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

public interface IPositionTypeService : IStatusGeneric
{
    PagedResult<PositionTypeListDto> GetList(SortFilterPageOptions dto);
    PositionTypeDto Get();
    PositionTypeDto Get(int id);
    SelectList<int> AsSelectList();
    HaveId<int> Create(CreatePositionTypeDlDto dto);
    void Update(UpdatePositionTypeDlDto dto);
    void Delete(int id);
}
