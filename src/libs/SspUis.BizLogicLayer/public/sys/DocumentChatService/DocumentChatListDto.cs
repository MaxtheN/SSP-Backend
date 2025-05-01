using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class DocumentChatListDto : ILinkToEntity<DocumentChat>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public int? UserId { get; set; }
        public string MessageText { get; set; }
        public int TableId { get; set; }
        public string Table { get; set; } = string.Empty;
        public long DocumentId { get; set; }
        public int? OrganizationId { get; set; }
        public string Organization { get; set; } = string.Empty;
        public long? ContractorId { get; set; }
        public string Contractor { get; set; } = string.Empty;
        public int StateId { get; set; }
        public string State { get; set; } = string.Empty;
        public int AppId { get; set; }
        public string App { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
