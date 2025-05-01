using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.TariffScaleServices
{
    public interface ITariffScaleService : IStatusGeneric
    {
        PagedResult<TariffScaleListDto> GetList(SortFilterPageOptions dto);
        TariffScaleDto Get();
        TariffScaleDto Get(int id);
        SelectList<int> AsSelectList();
        SelectList<int> AsSelectList(int typeId);
        SelectList<int> AsTableSelectList(int id);
        HaveId<int> Create(CreateTariffScaleDlDto dto);
        void Update(UpdateTariffScaleDlDto dto);
        void Delete(int id);
    }
}
