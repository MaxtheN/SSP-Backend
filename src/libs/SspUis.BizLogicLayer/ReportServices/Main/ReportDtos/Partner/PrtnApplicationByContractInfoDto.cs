using System;

namespace SspUis.BizLogicLayer.ReportServices.Main.ReportDtos.Partner
{
    public class PrtnApplicationByContractInfoDto
    {
        public string? Region { get; set; }
        public int? RegionId { get; set; }
        public string? District { get; set; }
        public int? DistrictId { get; set; }
        public string? Organization { get; set; }
        public long? OrganizationId { get; set; }
        public string? MFY { get; set; }
        public long? MFYId { get; set; }
        public string? INN { get; set; }
        public DateTime? DateSendedOfApplication { get; set; }
        public DateTime? DateBusinessmanApplicationSigned { get; set; }
        public int? DaysBusinessmanApplicationSigned { get; set; }
        public string? BusinessmanStatus { get; set; }
        public DateTime? DateEmploymentApplicationSigned { get; set; }
        public int? DaysEmploymentApplicationSigned { get; set; }
        public string? EmploymentStatus { get; set; }
        public DateTime? DateEconomyApplicationSigned { get; set; }
        public int? DaysEconomyApplicationSigned { get; set; }
        public string? EconomyStatus { get; set; }
        public DateOnly? DateOfCertificate { get; set; }
        public string? PrtnContractTypeName { get; set; }
        public int? PrtnContractTypeId { get; set; }
        public int? StatusId { get; set; }

    }
}