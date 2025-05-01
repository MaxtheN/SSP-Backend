namespace SspUis.BizLogicLayer
{
    public class CorruptionApplicationDto 
    {
        public int? RegionId { get; set; }
        public string RegionOrderCode { get; set; }
        public string Region { get; set; }

        public int? DistrictId { get; set; }
        public string District { get; set; }

        public long? ContractorId { get; set; }
        public string Contractor { get; set; }
        public string ContractorInn { get; set; }

        //public long? TotalApplicationCount { get; set; }
        //public long? TotalApplicationSendCount { get; set; }
        //public long? TotalApplicationSendToOmbusmanCount { get; set; }
        //public long? TotalApplicationSendToAniCorruptionCount { get; set; }
        //public long? TotalApplicationAccepCount { get; set; }
        //public long? TotalApplicationCanceldCount { get; set; }
        public int TotalApplicationSendToRewiedCount { get; set; }
        public int? TotalCertificateCount { get; set; }
        public int? TotalCanceledFromResultCount { get; set; }
    }
}