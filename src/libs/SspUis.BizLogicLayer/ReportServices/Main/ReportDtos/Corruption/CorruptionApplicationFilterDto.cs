using GenericServices;
using SspUis.DataLayer.EfClasses.Corruption;
using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class CorruptionApplicationFilterDto
    {
        public bool ByRegion { get; set; }
        public int? RegionId { get; set; }
        public bool ByDistrict { get; set; }
        public int? DistrictId { get; set; }
        public bool ByContractor { get; set; }
        public int? ContractorId { get; set; }

        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}