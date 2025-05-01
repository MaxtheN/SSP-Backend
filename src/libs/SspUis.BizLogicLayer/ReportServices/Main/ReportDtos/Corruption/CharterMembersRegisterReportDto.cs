using GenericServices;
using SspUis.DataLayer.EfClasses.Corruption;
using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class CharterMembersRegisterReportDto : ILinkToEntity<JoinAntiCorruptionCertificate>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public Guid Id2 { get; set; }
        public DateOnly DocOn { get; set; }
        public string DocNumber { get; set; }
        public long ContractorId { get; set; }
        public string ContractorInn { get; set; }
        public int? ContractorRegionId { get; set; }
        public string ContractorRegion { get; set; }
        public int? ContractorDistrictId { get; set; }
        public string ContractorDistrict { get; set; }
        public string ContractorFullName { get; set; }
        public string ContractorPhoneNumber { get; set; }
        public string ContractorOpf { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DateOnly? ExpireOn { get; set; }
        public DateOnly? CancelOn { get; set; }
    }
    public class CharterMembersRegisterFilterDto
    {
        public string Inn { get; set; }
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
    }
}