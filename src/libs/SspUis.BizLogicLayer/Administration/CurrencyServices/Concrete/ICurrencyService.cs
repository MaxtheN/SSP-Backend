using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.CurrencyServices
{
    public interface ICurrencyService : IStatusGeneric
    {
        PagedResult<CurrencyListDto> GetList(SortFilterPageOptions dto);
        CurrencyDto Get();
        CurrencyDto Get(int id);
        SelectList<int> AsSelectList(int? langId);
        HaveId<int> Create(CreateCurrencyDlDto dto);
        void Update(UpdateCurrencyDlDto dto);
        void Delete(int id);
    }
}
