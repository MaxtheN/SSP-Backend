using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.TariffScaleCoefServices
{
    public interface ITariffScaleCoefService : IStatusGeneric
    {
        PagedResult<TariffScaleCoefListDto> GetList(SortFilterPageOptions dto);
        TariffScaleCoefDto Get();
        TariffScaleCoefDto Get(int id);
        SelectList<int> AsSelectList();
        SelectList<int> GetTableAsSelectList(int? tariffScaleId = null, int? tariffScaleTableId = null);
        HaveId<int> Create(CreateTariffScaleCoefDlDto dto);
        void Update(UpdateTariffScaleCoefDlDto dto);
        void Delete(int id);
    }
}
