using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Corruption;
using SspUis.DataLayer.Repositories.Corruption;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Corruption
{
    public class JoinAntiCorruptionResultDto : UpdateJoinAntiCorruptionResultDlDto, ILinkToEntity<JoinAntiCorruptionResult>, IDocument
    {
        public string Status { get; set; }
        public string Organization { get; set; }
        public int TableId { get; set; }
        public int StatusId { get; set; }
        public int OrganizationId { get; set; }
        public string CorruptionCertificateNumber { get; set; }
        public string Message { get; set; }
        new public List<JoinAntiCorruptionResultTableDto> Tables { get; set; } = new();

        #region Actions
        public bool CanModify { get; set; }
        public bool CanAccept { get; set; }
        public bool CanCancel { get; set; }
        public bool CanDelete { get; set; }
        #endregion
    }
}
