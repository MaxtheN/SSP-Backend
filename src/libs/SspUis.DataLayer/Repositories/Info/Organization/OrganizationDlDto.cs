using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class OrganizationDlDto<TDto> : EntityDto<TDto, Organization>
        where TDto : OrganizationDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(300)]
        public string FullName { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int OrganizationGroupId { get; set; }
        public string Email { get; set; }
        [LocalizedStringLength(9)]
        public string Inn { get; set; }
        public string OrderCode { get; set; }
        public int? ParentId { get; set; }
        public int CountryId { get; set; }
        public int RegionId { get; set; }
        public int? DistrictId { get; set; }
        [LocalizedStringLength(500)]
        public string Address { get; set; }
        public int? OkedId { get; set; }
        [LocalizedStringLength(250)]
        public string Director { get; set; }
        [LocalizedStringLength(250)]
        public string Accounter { get; set; }
        [LocalizedStringLength(12)]
        public string VatCode { get; set; }
        [LocalizedStringLength(50)]
        public string ZipCode { get; set; }
        [LocalizedStringLength(250)]
        public string PhoneNumber { get; set; }
        public int? SignOrganizationTypeId { get; set; }
        public int? OrganizationLegalFormId { get; set; }
        public int? IncomingDocReceiverEmployeeId { get; set; }
        public List<OrganizationSignDlDto> Signs { get; set; } = new();
        public List<OrganizationSettlementAccountDlDto> SettlementAccounts { get; set; } = new();
        public List<OrganizationTranslateDlDto> Translates { get; set; } = new();
        public List<OrganizationFileDlDto> Files { get; set; } = new();

        protected override Action<IMappingExpression<TDto, Organization>> AlterMapping => cfg => cfg
             .ForMember(x => x.Signs, x => x.Ignore())
             .ForMember(x => x.SettlementAccounts, x => x.Ignore())
             .ForMember(x => x.Files, x => x.Ignore())
             .ForMember(x => x.Translates, x => x.Ignore());

        public override Organization CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            if (!string.IsNullOrEmpty(PhoneNumber))
                entity.PhoneNumber = Regex.Replace(PhoneNumber, @"\D", "");
            Signs.AddTo(entity.Signs, (e, d) =>
            {
                e.SetFIO();
            });
            SettlementAccounts.AddTo(entity.SettlementAccounts);
            entity.Files.AddFromTempFiles(
             DocumentStorageConst.INFO_ORGANIZATION_FILES,
              Files.Select(a => a.Id).ToList());
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(Organization entity)
        {
            if (!string.IsNullOrEmpty(PhoneNumber))
                PhoneNumber = Regex.Replace(PhoneNumber, @"\D", "");
            base.UpdateEntity(entity);
            Signs.ApplyChangesTo<int, OrganizationSignDlDto, OrganizationSign>(entity.Signs,
                (e, d) =>
                {
                    e.SetFIO();
                },
                (e, d) =>
                {
                    e.SetFIO();
                }
            );
            SettlementAccounts.ApplyChangesTo<long, OrganizationSettlementAccountDlDto, OrganizationSettlementAccount>(entity.SettlementAccounts);
            entity.Files.UpdateFromFiles(
                           DocumentStorageConst.INFO_ORGANIZATION_FILES,
                           entity.Id.ToString(),
                           Files.Select(a => a.Id).ToList());


            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }
    }
}
