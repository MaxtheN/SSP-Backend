namespace SspUis.BizLogicLayer.ReportServices
{
    public class ContractorCategoryDto
    {
        public int? RegionId { get; set; }
        public string Region{ get; set; }
        public string RegionOrderCode { get; set; }

        public int? DistrictId { get; set; }
        public string District { get; set; }
        public string DistrictOrderCode { get; set; }

        public long? TotalNewCreatedContractorLegalCount { get; set; }
        public long? TotalNewCreatedContractorPhysicalCount { get; set; }

        public long? TotalFreeAddedMemshipLegalCount { get; set; }
        public long? TotalFreeAddedMemshipPhysicalCount { get; set; }

        public long? Total { get; set; }

        public decimal TotalEvaluationRatingLegalCount { get; set; }
        public decimal TotalEvaluationRatingPhysicalCount { get; set; }

        public int? TotalRatingFromNormaLegalCount { get; set; }
        public int? TotalRatingFromNormaPhysicalCount { get; set; }

        public long? AverageRating { get; set; } // TotalRatingFromNorma ichidagi legalCount va physicalCount qoshib 2 bolish kere
        public string Evaluation { get; set; } // AverageRating qaysi ballga tori keladi enum bor enum_rating dan olinadi
    }
}
