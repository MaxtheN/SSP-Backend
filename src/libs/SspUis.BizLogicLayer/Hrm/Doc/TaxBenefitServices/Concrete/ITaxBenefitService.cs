using SspUis.BizLogicLayer.Models;
using SspUis.DataLayer.Repositories.Hrm;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm
{
    public interface ITaxBenefitService : IStatusGeneric
    {
        PagedResult<TaxBenefitListDto> GetList(TaxBenefitSortFilterOptions options);
        TaxBenefitDto Get();
        TaxBenefitDto Get(long id);
        SelectList<long> AsSelectList(TaxBenefitSortFilterOptions options);
        HaveId<long> Create(CreateTaxBenefitDlDto dto);
        void Accept(UpdateStatusTaxBenefitDto dTo);
        void Cancel(UpdateStatusTaxBenefitDto dTo);
        void Update(UpdateTaxBenefitDlDto dto);
        void Delete(long id);
    }
}


