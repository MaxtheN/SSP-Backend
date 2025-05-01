using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;
using SspUis.Core;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.PrtnCertificateServices
{
    public class PrtnCertificateListDto : DocumentListDto<long>, ILinkToEntity<PrtnCertificate>, IHaveIdProp<long>
    {
        public int TableId { get { return TableIdConst.DOC_PRTN_CERTIFICATE; } }
        public Guid Id2 { get; set; }
        public string DocNumber { get; set; }
        public string Status { get; set; }


        public long PrtnContractId { get; set; }
        public int NewVacanciesCount { get; set; }
        public string PrtnContractDocNumber { get; set; }
        public DateOnly PrtnContractDocOn { get; set; }

        public int PrtnContractTypeId { get; set; }
        public string PrtnContractType { get; set; }

        public int TotalPostCount { get; set; }
        public int SuccessPostCount { get; set; }
        public bool IsTotalSuccessPost { get; set; }


        public bool IsExpired { get; set; }
        public DateOnly ExpireOn { get; set; }
        public DateOnly? CancelOn { get; set; }
        public long ContractorId { get; set; }
        public string Contractor { get; set; }
        public string ContractorInn { get; set; }
        public int ContractorRegionId { get; set; }
        public string ContractorRegion { get; set; }
        public int ContractorDistrictId { get; set; }
        public long PrtnApplicationId { get; set; }
        public string ContractorDistrict { get; set; }
        public long? MfyId { get; set; }
        public string Mfy { get; set; }
        public string OkedCode { get; set; }
        public string Oked { get; set; }

        public bool CanCancel { get => StatusIdConst.CanCertificateApplyStatus(StatusId, StatusIdConst.CANCELED); }
    }

}
