namespace SspUis.BizLogicLayer.ReportServices.Main
{
    public class StateAssetApplicationReportDto
    {
        public int? RegionId { get; set; }
        public string RegionName { get; set; }
        public int? DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string StateAssetName { get; set; }
        public int AssetApplicationSum { get; set; }
        public StateAssetType StateAssetType { get; set; }
        public StateAssetType StateAssetType1 { get; set; }
        public StateAssetType StateAssetType2 { get; set; }
        public StateAssetType StateAssetType3 { get; set; }

    }
    public class StateAssetType
    {
        public int TotalApplicationCount { get; set; }
        public int CertificateCount { get; set; }
        public int NewVacanciesCount { get; set; }
        public int StateAssetApplicationCount { get; set; }
    }
}