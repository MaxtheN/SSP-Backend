using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Interfaces;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.BusinessmanAccountServices
{
    public class ContractorDocumentInfo
    {
        public bool HasPrtnApplication { get; set; }
        public bool HasPrtnContract { get; set; }
        public bool HasPrtnCertificate { get; set; }
        public bool HasMemshipApplication { get; set; }
        public bool HasMemshipContract { get; set; }
        public bool HasMemshipCertificate { get; set; }

        public bool HasClaimApplication { get; set; }
        public bool HasClaimMediationPlan { get; set; }
        public bool HasClaimMediation { get; set; }
        public bool HasPrtnCreditDemand { get; set; }
        public bool HasAdditionalAgreement { get; set; }
        public int? PrntContractTypeId { get; set; }
    }
}
