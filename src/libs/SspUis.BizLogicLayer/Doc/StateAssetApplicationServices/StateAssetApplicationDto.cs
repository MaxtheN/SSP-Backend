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
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.BizLogicLayer.ApplicationServices
{
    public class StateAssetApplicationDto : UpdateStateAssetApplicationDlDto, ILinkToEntity<Application>
    {
        public new long Id { get => base.Id; set => base.Id = value; }
        public Guid Id2 { get; set; }
        public int TableId { get; } = TableIdConst.DOC_STATE_ASSET_APPLICATION;
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string Contractor { get; set; }
        public string ContractorDirector { get; set; }
        public string ContractorInn { get; set; }
        public string ContractorAddress { get; set; }
        public string ContractorForm { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ApplicationType { get; set; }
        public string PrtnCertificateDocNumber { get; set; }
        public Guid PrtnCertificateId2 { get; set; }
        public DateOnly PrtnCertificateDocOn { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string RegionSoato { get; set; } = string.Empty;
        public string DistrictSoato { get; set; } = string.Empty;
        public int PrtnContractTypeId { get; set; }
        public string PrtnContractType { get; set; } = string.Empty;
        public int StateAssetStatusId { get; set; }

        public bool CanAccept { get; set; }
        public bool CanReject { get; set; }
        public bool CanSendForReview { get; set; }
        public bool CanEdit { get; set; }
    }
}
