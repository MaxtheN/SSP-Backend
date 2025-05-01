using System;

namespace SspUis.BizLogicLayer.ApplicationServices
{

    /*
     public class MfyApplicationStateDto
    {
        public Guid Id { get; set; }
        public string Inn { get; set; }
        public bool IsAccepted { get; set; }
        public string Details { get; set; }
        public string FileUrl { get; set; }
        public string ConclusingPersonPosition { get; set; }
        public string ConclusingPersonFio { get; set; }
        public string ConclusingPersonInn { get; set; }
        public string ConclusingPersonPhone { get; set; }
    }
     */
    public class MfyApplicationStateDto
    {
        public Guid Id { get; set; }
        public string Inn { get; set; }
        public string Details { get; set; }
        public bool IsAccepted { get; set; }
        public string FileUrl { get; set; }
        public string ConclusingPersonPosition { get; set; }
        public string ConclusingPersonFio { get; set; }
        public string ConclusingPersonInn { get; set; }
        public string ConclusingPersonPhone { get; set; }
        public MfyApplicationAdditionalData Data { get; set; } = new();
    }

    public class MfyApplicationAdditionalData
    {
        public string TotalAmount { get; set; }
        public string PersonalAmount { get; set; }
        public string BankLoansAmount { get; set; }
        public string ForeignInvestmentAmount { get; set; }
        public int NewJobsCount { get; set; }
        public string ProjectStartDate { get; set; }
        public string ProjectAddress { get; set; }
        public int? IsProjectFinished { get; set; }
        public int? IsNewProjectDone { get; set; }
        public int? IsExistingProjectExpanded { get; set; }
        public int? IsInFurnishingProccess { get; set; }
        public int? IsConstructionStarted { get; set; }
        public int? ThereIsEmptySpaceButNotStarted { get; set; }
        public int? ThereIsNoEmptySpaceForProject { get; set; }
    }
}
