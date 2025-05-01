using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.DataLayer.EfClasses;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ContractorDlDto<TDto> : EntityDto<TDto, Contractor>
        where TDto : ContractorDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string FullName { get; set; }
        [LocalizedStringLength(9)]
        public string? Inn { get; set; }
        [LocalizedStringLength(14)]
        public string Pinfl { get; set; }
        public int? OkedId { get; set; }
        public int? BankId { get; set; }
        public int CountryId { get; set; }
        public int RegionId { get; set; }
        public int DistrictId { get; set; }
        [LocalizedStringLength(500)]
        public string Address { get; set; }
        [LocalizedStringLength(250)]
        public string Accounter { get; set; }
        [LocalizedStringLength(250)]
        public string Director { get; set; }
        [LocalizedStringLength(15)]
        public string PhoneNumber { get; set; }
        [LocalizedStringLength(250)]
        public string Contact { get; set; }
        [LocalizedStringLength(30)]
        public string VatCode { get; set; }
        public int? OrganizationLegalFormId { get; set; }
        public DateOnly RegistrationDate { get; set; }
        public decimal? GovShare { get; set; }
        public int? OpfId { get; set; }
        public int? Kfs { get; set; }
        public string Soogu { get; set; } = null;
        public string SooguRegistrator { get; set; } = null;
        public string RegistrationNumber { get; set; } = null;
        public decimal? BusinessFund { get; set; }
        public int? VillageCode { get; set; }
        public string VillageName { get; set; }
        public string? OwnerName { get; set; }
        public decimal? TaxRate { get; set; }
        public int? AvgNumberEmployees { get; set; }
        public int? MonthlyNumberEmployees { get; set; }
        public List<ContractorSettlementAccountDlDto> SettlementAccounts { get; set; } = new();
        public List<ContractorContactDlDto> Contacts { get; set; } = new();

        protected override Action<IMappingExpression<TDto, Contractor>> AlterMapping => cfg => cfg
            .ForMember(x => x.SettlementAccounts, x => x.Ignore())
            .ForMember(x => x.Contacts, x => x.Ignore());

        public override Contractor CreateEntity()
        {
            ShortName = FullName;
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            if(!string.IsNullOrEmpty(PhoneNumber))
                entity.PhoneNumber = BusinessmanUser.GetCorrectUserName(PhoneNumber);
            SettlementAccounts.AddTo(entity.SettlementAccounts);
            Contacts.AddTo(entity.Contacts);
            return entity;
        }

        public override void UpdateEntity(Contractor entity)
        {
            ShortName = FullName;
            if(!string.IsNullOrEmpty(PhoneNumber))
                PhoneNumber = BusinessmanUser.GetCorrectUserName(PhoneNumber);
            base.UpdateEntity(entity);
            SettlementAccounts.ApplyChangesTo<long, ContractorSettlementAccountDlDto, ContractorSettlementAccount>(entity.SettlementAccounts);
            Contacts.ApplyChangesTo<long, ContractorContactDlDto, ContractorContact>(entity.Contacts);
        }
    }
}
