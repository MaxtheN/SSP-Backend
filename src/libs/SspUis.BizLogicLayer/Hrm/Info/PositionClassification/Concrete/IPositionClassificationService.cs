using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.PositionClassificationServices
{
    public interface IPositionClassificationService : IStatusGeneric
    {
        PagedResult<PositionClassificationListDto> GetList(SortFilterPageOptions dto);
        PositionClassificationDto Get();
        PositionClassificationDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreatePositionClassificationDlDto dto);
        void Update(UpdatePositionClassificationDlDto dto);
        void Delete(int id);
    }
}
