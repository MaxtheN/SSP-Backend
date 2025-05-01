using SspUis.DataLayer.EfClasses;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class DocumentChatDlDto<TDto> : EntityDto<TDto, DocumentChat>
        where TDto : DocumentChatDlDto<TDto>
    {
        public string MessageText { get; set; }

        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int TableId { get; set; }

        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long DocumentId { get; set; }

        public override DocumentChat CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            return entity;
        }
    }
}