using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using SspUis.BizLogicLayer.RegionServices;
using SspUis.BizLogicLayer.DistrictServices;
using Newtonsoft.Json;

namespace SspUis.BizLogicLayer.ApplicationServices
{
    public class ApplicationListDto : DocumentListDto<long>, ILinkToEntity<Application>, IHaveIdProp<long>
    {
        public Guid? Id2 { get; set; }
        public int? TableId { get; set; }
        public string DocNumber { get; set; }
        public string Status { get; set; }
        public long? ContractorId { get; set; }
        public string Contractor { get; set; }
        public int? ContractorRegionId { get; set; }
        public string ContractorInn { get; set; }
        public int? PrtnContractTypeId { get; set; }
        public string ContractorRegion { get; set; }
        public int? ContractorDistrictId { get; set; }
        public string ContractorDistrict { get; set; }
        public int? ApplicationTypeId { get; set; }
        public string ApplicationType { get; set; }
        //[JsonIgnore]
    
        [JsonIgnore]
        public DateTime CreatedAt { get; set; }
        public int? PrtnApplicationNewVacanciesCount { get; set; }
        public string PrtnContractType { get; set; }
        public int? PrtnContractStatusId { get; set; }
        public string PrtnContractStatus { get; set; }
        public string PrtnSertificateStatus { get; set; }
        public bool? HasBeenAnswered { get; set; }
        public bool ChooseLocation { get; set; }
        public int? ChoosedRegionId { get; set; }
        public string ChoosedRegion { get; set; }
        public int? ChoosedDistrictId { get; set; }
        public string ChoosedDistrict { get; set; }
        public long? MfyId { get; set; }
        public string Mfy { get; set; }
        public string ContractorPhoneNumber { get; set; }
        public int? OrganizationId { get; set; }
        public string Organization { get; set; }
        public string OrganizationInn { get; set; }
        public bool? ContractorHasGovShare { get; set; }
        public decimal? ContractorGovShare { get; set; }
        public string OkedCode { get; set; }
        public string Oked { get; set; }
        public string Director { get; set; }
        public bool IsLastOffer { get; set; }
        public RegionDto Region { get; set; }
        public DistrictDto District { get; set; }
        public  PrtnApplication PrtnApplication { get; set; }
        public PrtnContract PrtnContract { get; set; }

        #region Actions
        public bool CanViewMahallaResult { get; set; }
        #endregion
    }

}
