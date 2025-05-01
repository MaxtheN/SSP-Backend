using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;

namespace SspUis.BizLogicLayer
{
    public class RestrictionSendingAppListDto : ILinkToEntity<RestrictionOfSendingApplication>
    {
        public long Id { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Details { get; set; }
        public string MessageText { get; set; }
        public int TableId { get; set; }
        public string Table { get; set; }
        public int AppId { get; set; }
        public string ApplicatonModel { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
