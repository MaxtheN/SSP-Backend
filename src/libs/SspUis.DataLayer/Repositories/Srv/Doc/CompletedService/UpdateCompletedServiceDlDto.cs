using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateCompletedServiceDlDto
        : CompletedServiceDlDto<UpdateCompletedServiceDlDto>,
        IHaveIdProp<long>
    {
        public long Id { get; set; }
    }
}
