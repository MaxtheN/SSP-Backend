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
    public class DocumentChangeLogDlDto<TDto> : EntityDto<TDto, DocumentChangeLog>
        where TDto : DocumentChangeLogDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int UserId { get; set; }
        [LocalizedRequired]
        public DateTime DateAt { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(2000)]
        public string UserInfo { get; set; } = null!;
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string IpAddress { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string UserAgent { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string Message { get; set; }


        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int TableId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long DocId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int StatusId { get; set; }

        public override DocumentChangeLog CreateEntity()
        {
            var entity = base.CreateEntity();
            return entity;
        }

        public override void UpdateEntity(DocumentChangeLog entity)
        {
            base.UpdateEntity(entity);
        }

    }
}
