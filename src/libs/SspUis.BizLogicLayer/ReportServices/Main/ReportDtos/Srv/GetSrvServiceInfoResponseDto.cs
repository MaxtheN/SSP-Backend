using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class GetSrvServiceInfoResponseDto
    {
        public int? RegionId { get; set; }
        public string RegionOrderCode { get; set; }
        public string Region { get; set; }
        public int? DistrictId { get; set; }
        public string DistrictOrderCode { get; set; }
        public string District { get; set; }
        public long? ContractorId { get; set; }
        public string Contractor { get; set; }
        public string ContractorInn { get; set; }
        public decimal PlanPaidSum { get; set; }
        public int PlanPaidCount { get; set; }
        public decimal PlanFreeCount { get; set; }
        public decimal LegalAmount { get; set; }
        public decimal EconomomyAmount { get; set; }
        
       // public TotalSignApplication TotalSignedApplication { get; set; }
        public TotalMemshipPaymentOrder TotalMemshipPaymentOrderCount { get; set; }
        public decimal AcceptedPaidSumPercentage { get; set; }
        public decimal AcceptedFreePercentage { get; set; }
        public decimal PaidCoef { get; set; }
        public decimal FreeCoef { get; set; }
        public int AcceptedFreeCount { get; set; }
        public decimal AcceptedPaidCountPercentage { get; set; }
        public int TotalFreeApplicationCount { get; set; }
        public int TotalPaidApplicationCount { get; set; }
        public int RejectedApplicationCount { get; set; }
        public int TotalSignedCertificateCount { get; set; }
        public decimal TotalSignedCertificateSum { get; set; }
    }
   
    public class TotalMemshipPaymentOrder
    {
        public int TotalCount { get; set; }
        public decimal TotalSum { get; set; }
        public decimal EconomomyAmount { get; set; }
        public decimal LegalAmount { get; set; }
        public decimal BirjaAmount { get; set; }

        public int? TotalRegionCount { get; set; }
        public decimal? TotalRegionSum { get; set; }
        public decimal? EconomomyRegionAmount { get; set; }
        public decimal? LegalRegionAmount { get; set; }
        public decimal? BirjaRegionAmount { get; set; }
    }
}
