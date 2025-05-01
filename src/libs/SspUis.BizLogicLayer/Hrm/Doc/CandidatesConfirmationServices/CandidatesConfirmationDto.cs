using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer
{
    public class CandidatesConfirmationDto
        : UpdateCandidatesConfirmationDlDto,
        ILinkToEntity<CandidatesConfirmation>,
        IDocument
    {
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int OrganizationId { get; set; }
        public string Organization { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public new List<CandidatesConfirmationTableDto> Tables { get; set; } = new();

        #region Actions
        public bool CanModify { get; set; }
        public bool CanSend { get; set; }
        public bool CanAccept { get; set; }
        public bool CanCancel { get; set; }
        public bool CanDelete { get; set; }
        #endregion
    }
}
