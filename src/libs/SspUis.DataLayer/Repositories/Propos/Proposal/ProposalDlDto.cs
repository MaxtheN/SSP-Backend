using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Proposal;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ProposalDlDto<TDto> : EntityDto<TDto, Proposal>
        where TDto : ProposalDlDto<TDto>
    {
        [LocalizedStringLength(20)]
        public string DocNumber { get; set; }
        public DateOnly DocOn { get; set; } = DateTime.Today.AsDateOnly();
        public int? ProposalTypeId { get; set; }
        public int? BusinessSectorId { get; set; }
        public int ExternalSourceTypeId { get; set; }
        [LocalizedStringLength(100)]
        public string? SurnameLatin { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyInn { get; set; }
        [LocalizedStringLength(100)]
        public string? NameLatin { get; set; }
        public string? PatronymLatin { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? GenderId { get; set; }
        [LocalizedStringLength(50)]
        public string? PhoneNumber { get; set; }
        [LocalizedStringLength(250)]
        public string? Email { get; set; }
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
        public long? MfyId { get; set; }
        [LocalizedStringLength(200)]
        public string? Address { get; set; }
        public int? EmployementTypeId { get; set; }
        public int? ProposalSubjectId { get; set; }
        public int? ToOrganizationId { get; set; }
        public int? ProposalDisclosureId { get; set; }
        public string? ProposalText { get; set; }
        public string? AppealText { get; set; }
        public int? CompanyTypeId { get; set; }

        public List<ProposalFileDlDto>? Files { get; set; } = new();

        protected override Action<IMappingExpression<TDto, Proposal>> AlterMapping =>
            cfg => cfg
                .ForMember(x => x.ProposalFiles, opt => opt.Ignore());

        public override Proposal CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StatusId = StatusIdConst.CREATED;
            entity.ProposalFiles.AddFromTempFiles(DocumentStorageConst.DOC_PROPOSAL_FILE, Files.Select(a => a.Id).ToList());
            return entity;
        }

        public override void UpdateEntity(Proposal entity)
        {
            base.UpdateEntity(entity);
        }

    }
}