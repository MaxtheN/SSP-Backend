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
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.PrtnContractServices
{
    public class PrtnContractDto : UpdatePrtnContractDlDto, ILinkToEntity<PrtnContract>, IDocument
    {
        public Guid Id2 { get; set; }
        public DateOnly ApplicationDocOn { get; set; }
        public string ApplicationDocNumber { get; set; }
        public string Status { get; set; }
        public string Contractor { get; set; }
        public string ContractorRegionSoato { get; set; }
        public string ContractorInn { get; set; }
        public string PrtnContractType { get; set; }
        public int OrganizationId { get; set; }
        public long? PrtnCertificateId { get; set; }
        public int? PrtnCertificateStatusId { get; set; }
        public int? PrtnApplicationId { get; set; }
        public string PrtnCertificateStatus { get; set; }
        public string Message { get; set; }
        public int TableId { get => TableIdConst.DOC_PRTN_CONTRACT; }
        [JsonIgnore]
        public long? CurrentPrtnContractSignId { get; set; }
        [JsonIgnore]
        public string CurrentPrtnContractSignPinfl { get; set; }
        public List<PrtnContractSignDto> Signs { get; set; } = new();
        public new List<PrtnContractFileDto> Files { get; set; } = new();

        public bool CanSign { get; set; }
        public bool CanReject { get; set; }
        public bool CanCancel { get; set; }
        public bool CanPassExpertise { get; set; }
        public bool CanNotPassExpertise { get; set; }
        public bool ReSendForExpertise { get; set; }
    }
}
