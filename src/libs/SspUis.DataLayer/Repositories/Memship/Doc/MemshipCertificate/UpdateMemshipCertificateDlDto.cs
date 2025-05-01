using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class UpdateMemshipCertificateDlDto : MemshipCertificateDlDto<UpdateMemshipCertificateDlDto>, IHaveIdProp<long>
{
    public long Id { get; set; }
    public bool IsFree { get; set; } 
}
