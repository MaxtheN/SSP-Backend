using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateAdditionalAgreementDlDto : AdditionalAgreementDlDto<UpdateAdditionalAgreementDlDto>, IHaveIdProp<long>
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }
        //[LocalizedRequired]
        //[LocalizedRange(1, int.MaxValue)]
        //public int StatusId { get; set; }
    }
}
