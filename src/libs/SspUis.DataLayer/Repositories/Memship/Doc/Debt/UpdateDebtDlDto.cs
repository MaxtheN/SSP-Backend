using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateDebtDlDto : DebtDlDto<UpdateDebtDlDto>, IHaveIdProp<long>
    {
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int StatusId { get; set; }
    }
}
