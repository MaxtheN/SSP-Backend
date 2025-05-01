using SspUis.Core;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer
{
    public class UpdateStatusMemshipCertificateDto : UpdateStatusMemshipCertificateDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
    }
    public class CancelStatusMemshipCertificateDto : UpdateStatusMemshipCertificateDlDto
    {
        public CancelStatusMemshipCertificateDto()
        {
            StatusId = StatusIdConst.CANCELED;
        }

        public string Message { get; set; }
        public DateOnly? CancelDay { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string CancelReasen { get; set; }
        public bool IsRejectContractAndApplication { get; set; } = false;
        public List<MemshipCertificateFileDlDto> Files { get; set; }
    }
}