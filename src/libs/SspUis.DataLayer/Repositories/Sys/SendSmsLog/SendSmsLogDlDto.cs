using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class SendSmsLogDlDto<TDto> : EntityDto<TDto, SendSmsLog>
        where TDto : SendSmsLogDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string PhoneNumer { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string SmsText { get; set; }
        public string? ErrorText { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int TableId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public long DocId { get; set; }
        public int? FromStatusId { get; set; }
        public int? ToStatusId { get; set; }

        public override SendSmsLog CreateEntity()
        {
            var entity = base.CreateEntity();
            return entity;
        }

        public override void UpdateEntity(SendSmsLog entity)
        {
            base.UpdateEntity(entity);
        }
    }
}