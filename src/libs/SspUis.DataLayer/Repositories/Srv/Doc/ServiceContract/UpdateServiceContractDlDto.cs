using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateServiceContractDlDto
        : ServiceContractDlDto<UpdateServiceContractDlDto>,
        IHaveIdProp<long>
    {
        public long Id { get; set; }
    }
}
