using AutoMapper;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using GenericServices;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.OrganizationServices
{
    public class OrganizationSettlementAccountDto : OrganizationSettlementAccountDlDto, ILinkToEntity<OrganizationSettlementAccount>
    {
        public string Bank { get; set; }
        public string BankCode { get; set; }
        public string State { get; set; }
    }
}
