using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateStatusServiceApplicationDlDto 
        : Doc.BaseApplication.UpdateStatusApplicationDlDto<UpdateStatusServiceApplicationDlDto, ServiceApplication>
    {
        [LocalizedRequired]
        public new string Message { get => base.Message; set => base.Message = value; }
        public override void UpdateEntity(ServiceApplication entity)
        {
            base.UpdateEntity(entity);
            entity.Application.StatusId = StatusId;
            entity.Application.Message = Message;
        }
    }
}
