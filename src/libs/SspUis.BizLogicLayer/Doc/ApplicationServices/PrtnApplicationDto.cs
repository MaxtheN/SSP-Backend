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

namespace SspUis.BizLogicLayer.ApplicationServices
{
    public class PrtnApplicationDto : UpdatePrtnApplicationDlDto, ILinkToEntity<Application>
    {
        public Guid Id2 { get; set; }
        public int TableId { get; set; } = TableIdConst.DOC_PRTN_APPLICATION;
        public int StatusId { get; set; }
        public string Status { get; set; }
        public new long ContractorId { get; set; }
        public string Contractor { get; set; }
        public string ContractorDirector { get; set; }
        public string ContractorInn { get; set; }
        public string ApplicationType { get; set; }
        public bool IsRead { get; set; }
        public new int RegionId { get; set; }
        public new string RegionName { get; set; }
        public new int DistrictId { get; set; }
        public new string DistrictName { get; set; }
        public string MfyName { get; set; }
        public new int ApplicationTypeId { get; set; }
        public bool ChooseLocation { get; set; }
        public int? ChoosedRegionId { get; set; }
        public int? ChoosedDistrictId { get; set; }
        public string ChoosedRegion { get; set; }
        public string ChoosedDistrict { get; set; }

        public string PrtnContractType { get; set; }
        public string PrtnContractTypeFrom { get; set; }
        public string PrtnContractTypeTo { get; set; }
        public long? PrtnContractId { get; set; } 

        new public List<PrtnApplicationGraphDto> Graphs { get; set; } = new();

        public bool CanAccept { get; set; }
        public bool CanReject { get; set; }
        public bool CanSendForReview { get; set; }
        public bool CanEdit { get; set; }
    }
}
