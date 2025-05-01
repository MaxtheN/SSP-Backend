using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer
{
    public class CandidatesConfirmationTableDto
        : CandidatesConfirmationTableDlDto, ILinkToEntity<CandidatesConfirmationTable>
    {
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string Employee { get; set; }
        public new List<CandidatesConfirmationTableFileDto> Files { get; set; } = new();
    }

    public class CandidatesConfirmationTableFileDto : CandidatesConfirmationTableFileDlDto,
        ILinkToEntity<CandidatesConfirmationTableFile>
    {
        public string FileName { get; internal set; }
        public DateTime CreatedAt { get; set; }
    }
}
