using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.TaxBenefitTypeServices
{
    public interface ITaxBenefitTypeService : IStatusGeneric
    {
        PagedResult<TaxBenefitTypeListDto> GetList(SortFilterPageOptions dto);
        TaxBenefitTypeDto Get();
        TaxBenefitTypeDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateTaxBenefitTypeDlDto dto);
        void Update(UpdateTaxBenefitTypeDlDto dto);
        void Delete(int id);
    }
}
