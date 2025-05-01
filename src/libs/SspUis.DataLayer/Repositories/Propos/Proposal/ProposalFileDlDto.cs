using GenericServices;
using SspUis.DataLayer.EfClasses.Proposal;
using System;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ProposalFileDlDto : EntityDto<ProposalFileDlDto, ProposalFile>, ILinkToEntity<ProposalFile>
    {
        [LocalizedRequired]
        public Guid Id { get; set; }
    }
}
