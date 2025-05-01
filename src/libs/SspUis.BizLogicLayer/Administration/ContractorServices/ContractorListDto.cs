using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ContractorServices
{
    public class ContractorListDto : ILinkToEntity<Contractor>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string InnOrPinfl { get; set; }
        public string Inn { get; set; }
        public string Pinfl { get; set; }
        public int? OkedId { get; set; }
        public int? BankId { get; set; }
        public int CountryId { get; set; }
        public int RegionId { get; set; }
        public int DistrictId { get; set; }
        public string Address { get; set; }
        public string Accounter { get; set; }
        public string Director { get; set; }
        public string? WorkPhoneNumber { get; set; }
        public string? MobilePhoneNumber { get; set; }
        public string? AdditionalPhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Faks { get; set; }
        public string? Skype { get; set; }
        public string? Facebook { get; set; }
        public string? Telegram { get; set; }
        public string? WebSite { get; set; }
        public string VatCode { get; set; }
        public int? OpfId { get; set; }
        public string State { get; set; }
        public string Oked { get; set; }
        public string OkedCode { get; set; }
        public string Bank { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public DateOnly RegistrationDate { get; set; }

        public string? OwnerName { get; set; }

      //  public List<ApplicationsOfContractorDto> Applications { get; set; } = new();
    }
    public class ApplicationsOfContractorDto : ILinkToEntity<Application>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public long ContractorId { get; set; }
        public DateOnly DocOn { get; set; }
        public string DocNumber { get; set; }
        public int ApplicationTypeId { get; set; }
        public string ApplicationType { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
