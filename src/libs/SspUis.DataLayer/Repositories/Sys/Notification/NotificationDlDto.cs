using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class NotificationDlDto<TDto> : EntityDto<TDto, Notification>
        where TDto : NotificationDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string Title { get; set; }
        public string Content { get; set; }
        public int TypeId { get; set; }
        public int TableId { get; set; }
        public int DocStatusId { get; set; }
        public long DocId { get; set; }
        public List<int> NotificationUsers { get; set; } = new();

        protected override Action<IMappingExpression<TDto, Notification>> AlterMapping => cfg => cfg
            .ForMember(x => x.NotificationUsers, x => x.Ignore());

        public override Notification CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.NotificationUsers.AddFromForeignKeys(NotificationUsers);
            return entity;
        }

        public override void UpdateEntity(Notification entity)
        {
            base.UpdateEntity(entity);
            entity.NotificationUsers.UpdateFromForeignKeys(NotificationUsers);
        }

    }
}
