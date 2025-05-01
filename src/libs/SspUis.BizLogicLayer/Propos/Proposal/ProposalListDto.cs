using GenericServices;
using SspUis.DataLayer.EfClasses.Proposal;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Propos
{
    public class ProposalListDto : ILinkToEntity<Proposal>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public string DocNumber { get; set; }
        public DateOnly? DocOn { get; set; }
        public int? ProposalTypeId { get; set; }
        public string ProposalTypeName { get; set; }
        public int? BusinessSectorId { get; set; }
        public string BusinessSectorName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyInn { get; set; }
        public int? ExternalSourceTypeId { get; set; }
        public string ExternalSourceTypeName { get; set; }
        public string NameLatin { get; set; }
        public string SurnameLatin { get; set; }
        public string PatronymLatin { get; set; }
        public string SurnameEng { get; set; }
        public string NameEng { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? GenderId { get; set; }
        public string GenderName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int? RegionId { get; set; }
        public string RegionName { get; set; }
        public int? DistrictId { get; set; }
        public string DistrictName { get; set; }
        public long? MfyId { get; set; }
        public string MfyName { get; set; }
        public string AddressName { get; set; }
        public int? EmployementTypeId { get; set; }
        public string EmployementTypeName { get; set; }
        public int? CompanyTypeId { get; set; }
        public string CompanyTypeName { get; set; }
        public int? ProposalSubjectId { get; set; }
        public string ProposalSubjectName { get; set; }
        public string ToOrganizationName { get; set; }
        public int? ToOrganizationId { get; set; }
        public int? ProposalDisclosureId { get; set; }
        public string ProposalDisclosureName { get; set; }
        public string ProposalText { get; set; }
        public string AppealText { get; set; }
        public int? StatusId { get; set; }
        public string Status { get; set; }
    }
}
