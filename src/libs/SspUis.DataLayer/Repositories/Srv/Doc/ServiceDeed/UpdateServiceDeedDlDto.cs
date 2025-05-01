using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateServiceDeedDlDto
        : ServiceDeedDlDto<UpdateServiceDeedDlDto>,
        IHaveIdProp<long>
    {
        public long Id { get; set; }
    }
}
