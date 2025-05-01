using SspUis.DataLayer.Repositories.Corruption;
using System;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.Corruption
{
    public class UpdateStatusJoinAntiCorruptionResultDto : UpdateStatusJoinAntiCorruptionResultDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }

        [LocalizedRequired]
        public string SignedData { get; set; }
        internal Guid SignFile { get; set; }
        internal Guid DataFile { get; set; }
        public DateTime SignedAt { get; set; }
        internal string SignedUserInfo { get; set; }
    }
}
