using System;
using System.Collections.Generic;
using System.Linq;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.ContractorServices
{
    public class ContractorDto : CreateContractorDlDto, ILinkToEntity<Contractor>
    {
        public long Id { get; set; }
        public string State { get; set; }
        public int StateId { get; set; }
        public string Oked { get; set; }
        public string Bank { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public bool IsLastOffer { get; set; }
        public string DirectorSeria{ get; set; }
        public string DirectorNumber { get; set; }
        public string DirectorPinfl { get; set; }
        public string DirectorBirthDate { get; set; }
        public int? BusinessType { get; set; }
        public new List<ContractorSettlementAccountDto> SettlementAccounts { get; set; } = new();

        public List<ContractorContactDto> Contacts { get; set; } = new();

        public void LoadSettlementAccountsToBase()
        {
            base.SettlementAccounts = SettlementAccounts.Select(a => a as ContractorSettlementAccountDlDto).ToList();
        }
    }
}
