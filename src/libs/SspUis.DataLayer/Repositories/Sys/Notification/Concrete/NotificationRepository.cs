using SspUis.DataLayer.EfClasses;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class NotificationRepository : BaseEntityRepository<long, Notification, CreateNotificationDlDto, UpdateNotificationDlDto>, INotificationRepository
    {
        public NotificationRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }

        protected override IQueryable<Notification> ByIdQuery()
        {
            return AllAsQueryable.Include(a => a.NotificationUsers);
        }

        protected override void CreateValidate(CreateNotificationDlDto dto)
        {
            Validate(null, dto);
        }

        protected override void UpdateValidate(Notification entity, UpdateNotificationDlDto dto)
        {
            Validate(entity, dto);
        }

        private void Validate<TDto>(Notification entity, NotificationDlDto<TDto> dto)
            where TDto : NotificationDlDto<TDto>
        {
            var query = DbSet.AsQueryable();

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);
        }
    }
}
