using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Memship;

public class UpdateMemshipContractDlDto : MemshipContractDlDto<UpdateMemshipContractDlDto>, IHaveIdProp<long>
{
    public long Id { get; set; }
}
