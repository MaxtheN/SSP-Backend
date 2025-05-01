using GenericServices;
using SspUis.DataLayer.EfClasses.Proposal;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer.Propos
{
    public class ProposalDto : UpdateProposalDlDto, ILinkToEntity<Proposal>
    {
        public int StatusId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
