using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class UpdateTaxBenefitDlDto : TaxBenefitDlDto<UpdateTaxBenefitDlDto>, IHaveIdProp<long>
{
    public long Id { get; set; }
}
