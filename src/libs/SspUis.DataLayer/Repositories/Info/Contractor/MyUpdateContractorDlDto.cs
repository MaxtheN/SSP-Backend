using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using SspUis.Core.Security;
using AutoMapper;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;
using WEBASE.EF;
using System.Text.RegularExpressions;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public class MyUpdateContractorDlDto : EntityDto<MyUpdateContractorDlDto, Contractor>, IHaveIdProp<long>
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }

        public int? OkedId { get; set; }
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
        public List<ContractorSettlementAccountDlDto> SettlementAccounts { get; set; }
        public List<ContractorContactDlDto> Contacts { get; set; }

        protected override Action<IMappingExpression<MyUpdateContractorDlDto, Contractor>> AlterMapping => cfg => cfg
            .ForMember(x => x.Contacts, x => x.Ignore())
            .ForMember(x => x.SettlementAccounts, x => x.Ignore());

        public override void UpdateEntity(Contractor entity)
        {
            if (!string.IsNullOrEmpty(PhoneNumber))
                PhoneNumber = BusinessmanUser.GetCorrectUserName(PhoneNumber);
            base.UpdateEntity(entity);

            // `IsMain` bo'yicha saralangan asosiy `BankId`ni olish
            entity.BankId = SettlementAccounts?.OrderByDescending(a => a.IsMain).FirstOrDefault()?.BankId;

            // Asosiy hisob mavjud emasligini tekshirish
            if (SettlementAccounts != null && !SettlementAccounts.Any(a => a.IsMain) && SettlementAccounts.Count > 0)
                SettlementAccounts[0].IsMain = true;

            // `Contacts` kolleksiyasiga o'zgartirishlarni qo'llash
            if (Contacts != null)
                Contacts.ApplyChangesTo<long, ContractorContactDlDto, ContractorContact>(entity.Contacts);

            // `SettlementAccounts` kolleksiyasiga o'zgartirishlarni qo'llash
            if (SettlementAccounts != null)
                SettlementAccounts.ApplyChangesTo<long, ContractorSettlementAccountDlDto, ContractorSettlementAccount>(entity.SettlementAccounts);
        }
    }
}