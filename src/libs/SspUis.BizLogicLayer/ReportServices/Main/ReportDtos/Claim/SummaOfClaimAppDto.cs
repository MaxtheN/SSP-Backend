namespace SspUis.BizLogicLayer.ReportServices
{
    public class SummaOfClaimAppDto : BaseClaimApplicationReportDto
    {
        public decimal TotalSummaInArea { get; set; }
        public SummaOfClaimAppColumnsDto TreatedSum { get; set; }
        public SummaOfClaimAppColumnsDto UnidirectionalSum { get; set; }
    }
    public class SummaOfClaimAppColumnsDto
    {
        public SummaOfClaimAppColumnsDto() { }

        public SummaOfClaimAppColumnsDto(
            decimal totalSumma,
            decimal legalSumma,
            decimal yATTSumma,
            decimal individualsSumma,
            decimal stateOrganizationSumma,
            decimal foreignCitizenSumma)
        {
            this.TotalSumma = totalSumma;
            this.LegalSumma = legalSumma;
            this.YATTSumma = yATTSumma;
            this.IndividualsSumma = individualsSumma;
            this.StateOrganizationSumma = stateOrganizationSumma;
            this.ForeignCitizenSumma = foreignCitizenSumma;
        }

        public decimal TotalSumma { get; set; }
        public decimal LegalSumma { get; set; }
        public decimal YATTSumma { get; set; }
        public decimal IndividualsSumma { get; set; }
        public decimal StateOrganizationSumma { get; set; }
        public decimal ForeignCitizenSumma { get; set; }
    }
}