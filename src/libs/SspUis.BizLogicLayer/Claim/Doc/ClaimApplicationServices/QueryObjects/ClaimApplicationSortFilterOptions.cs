namespace SspUis.BizLogicLayer.ClaimApplicationServices
{
    public class ClaimApplicationSortFilterOptions : DocumentSortFilterOptions
    {
        public string ContractorInn { get; set; }
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
        public int? ContractorId { get; set; }
        public int? ClaimApplicationTypeId { get; set; }
        public bool? OneWeek { get; set; } = false;
		public bool? TwoWeek { get; set; } = false;
		public bool? ThreeWeek { get; set; } = false;
		public bool?  Month { get; set; } = false;
        public int? ClaimThemeId { get; set; }
        public int? StatusId { get; set; }
        public int? StepId { get; set;}
        public int? OrganizationId { get; set; }
        public bool IsEmployee { get; set; } = false;
        public bool CalimAppType { get; set; } = false;
        public bool ForSecondGetList { get; set; } = false;
        public bool IsIndividual { get; set; } = false;
    }
}
