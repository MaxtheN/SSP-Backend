using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System.Reflection;
using System;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class SendSmsConfigDlDto<TDto> : EntityDto<TDto, SendSmsConfig>
        where TDto : SendSmsConfigDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string Title { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string SmsText { get; set; }

        public int TableId { get; set; }

        public int? FromStatusId { get; set; }

        public int? ToStatusId { get; set; }

        public override SendSmsConfig CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            return entity;
        }

        public override void UpdateEntity(SendSmsConfig entity)
        {
            base.UpdateEntity(entity);
        }
    }
}
