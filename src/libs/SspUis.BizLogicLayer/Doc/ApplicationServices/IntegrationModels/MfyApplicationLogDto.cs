using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;

namespace SspUis.BizLogicLayer.ApplicationServices
{
    public class MfyApplicationLogDto : 
        ILinkToEntity<MfyApplicationLog>
    {
        public long Id { get; set; }
        public Guid ApplicationId2 { get; set; }
        public bool IsAccepted { get; set; }
        public string Details { get; set; }
        public string FileUrl { get; set; }
        public string ConclusingPersonPosition { get; set; }
        public string ConclusingPersonFio { get; set; }
        public string ConclusingPersonInn { get; set; }
        public string ConclusingPersonPhone { get; set; }

        public decimal? TotalAmount { get; set; }
        public decimal? PersonalAmount { get; set; }
        public decimal? BankLoansAmount { get; set; }
        public decimal? ForeignInvestmentAmount { get; set; }
        public int? NewJobsCount { get; set; }
        public DateOnly? ProjectStartDate { get; set; }
        public string ProjectAddress { get; set; }
        public bool? IsProjectFinished { get; set; }
        public bool? IsNewProjectDone { get; set; }
        public bool? IsExistingProjectExpanded { get; set; }
        public bool? IsInFurnishingProccess { get; set; }
        public bool? IsConstructionStarted { get; set; }
        public bool? ThereIsEmptySpaceButNotStarted { get; set; }
        public bool? ThereIsNoEmptySpaceForProject { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}