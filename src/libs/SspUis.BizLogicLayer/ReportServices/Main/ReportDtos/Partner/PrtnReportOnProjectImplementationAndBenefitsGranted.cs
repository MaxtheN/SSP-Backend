using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Integration.MSPD.Models.GNK;

namespace SspUis.BizLogicLayer.ReportServices.Main.ReportDtos.Partner
{
    public class PrtnReportOnProjectImplementationAndBenefitsGranted
    {
        public int? DistrictId { get; set; }
        public string? District { get; set; }
        public int? RegionId { get; set; }
        public string? Region { get; set; }
        public long? ContractorId { get; set; }
        public string? Contractor { get; set; }
        public string? ContractorInn { get; set; }
        /// <summary>
        /// Sertifikati berilgan 
        /// </summary>
        public (long? ContractorCount, long? JobCount) CertificateHolders { get; set; }
        /// <summary>
        /// Ishga tushgan loyihalar (Reja bo'yicha ish orni yaratish muddati kelganlar) Tadbirkorlar soni
        /// </summary>
        public (int? ContractorCount, int? JobCount) RelationToLaunchedProjects { get; set; }
        /// <summary>
        /// Soliqdan olingan Tadbirkorlar 
        /// </summary>
        public (long? Count, long? JobCount,  decimal? Summ) DataFromTaxContractor { get; set; }
        /// <summary>
        /// Hokimyatdan olingan Tadbirkorlar 
        /// </summary>
        public (long? Count, long? JobCount, decimal? Summ) DataFromGovernmentContractorCount { get; set; }
        /// <summary>
        /// Ajratilagn Kreditlar 	
        /// </summary>
        public (double? Count, double? Summ) AprovedCredit { get; set; }
        /// <summary>
        /// Берилган кафиллик 	
        /// </summary>
        public (long? Count, decimal? Summ) SeparatePreferentialCredit { get; set; }
        /// <summary>
        /// Mol mulk va yer solig'i 
        /// </summary>
        public (long? Count, decimal? Summ) PropertyAndLandTax { get; set; }
        /// <summary>
        /// Daromad solig'i 
        /// </summary>
        public (long? Count, decimal? Summ) IncomeTax { get; set; }
        /// <summary>
        /// Soliq stavkasining 50 foiz miqdori 
        /// </summary>
        public (long? Count, decimal? Summ) FiftyPercentOfTheTaxRate { get; set; }
        /// <summary>
        /// QQS ni o'zaro hisobga olish	
        /// </summary>
        public (long? Count, decimal? Summ) CrossAccountingOfVAT { get; set; }
        /// <summary>
        /// Soliqlarni taminotsiz va foizsiz bolib tolash huquqini olganlar soni	
        /// </summary>
        public (long? Count, decimal? Summ) PeopleTaxesWithoutInsuranceAndWithoutInterest { get; set; }
        /// <summary>
        /// Bojxonadan
        /// </summary>
        public (int? GreenLand, long? Count, decimal? Summ ) FromCustoms { get; set; }
        /// <summary>
        /// Amaliy monomarkaz	
        /// </summary>
        public (int? AuctionBuildings, int? BuildingsCPC) PracticalMonocenter { get; set; }
    }
}
    public class PrtnFilterDto
    {
    public int? RegionId { get; set; }
    public bool ByRegion { get; set; } = false;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? DistrictId { get; set; }
    public bool ByDistrict { get; set; } = false;
    public bool HasDistrict { get; set; } = false;
    public bool HasRegion { get; set; } = false;

    public bool ByContractor { get; set; } = false;

    public bool Between51and100 { get; set; } = false;
    public bool Between101and200 { get; set; } = false;
    public bool MoreThan200 { get; set; } = false;
}

public class ContractorIdInnName
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Inn { get; set; }
}
