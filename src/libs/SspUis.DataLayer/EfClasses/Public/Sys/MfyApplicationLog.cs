using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_mfy_application_log")]
    public class MfyApplicationLog
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("application_id2")]
        public Guid ApplicationId2 { get; set; }
        [Column("application_id")]
        public long ApplicationId { get; set; }
        [Column("is_accepted")]
        public bool IsAccepted { get; set; }
        [Column("details")]
        public string Details { get; set; }
        [Column("file_url")]
        public string FileUrl { get; set; }
        [Column("conclusing_person_position")]
        public string ConclusingPersonPosition { get; set; }
        [Column("conclusing_person_fio")]
        public string ConclusingPersonFio { get; set; }
        [Column("conclusing_person_inn")]
        public string ConclusingPersonInn { get; set; }
        [Column("conclusing_person_phone")]
        public string ConclusingPersonPhone { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("total_amount")]
        [Precision(18, 2)]
        public decimal? TotalAmount { get; set; }
        [Column("personal_amount")]
        [Precision(18, 2)]
        public decimal? PersonalAmount { get; set; }
        [Column("bank_loan_amount")]
        [Precision(18, 2)]
        public decimal? BankLoanAmount { get; set; }
        [Column("foreign_investment_amount")]
        [Precision(18, 2)]
        public decimal? ForeignInvestmentAmount { get; set; }
        [Column("new_job_count")]
        public int? NewJobCount { get; set; }
        [Column("project_start_date")]
        public DateOnly? ProjectStartDate { get; set; }
        [Column("project_address")]
        [StringLength(500)]
        public string ProjectAddress { get; set; }
        [Column("is_project_finished")]
        public bool? IsProjectFinished { get; set; }
        [Column("is_new_project_done")]
        public bool? IsNewProjectDone { get; set; }
        [Column("is_existing_project_expanded")]
        public bool? IsExistingProjectExpanded { get; set; }
        [Column("is_in_furnishing_proccess")]
        public bool? IsInFurnishingProccess { get; set; }
        [Column("is_construction_started")]
        public bool? IsConstructionStarted { get; set; }
        [Column("there_is_empty_space_but_not_started")]
        public bool? ThereIsEmptySpaceButNotStarted { get; set; }
        [Column("there_is_no_empty_space_for_project")]
        public bool? ThereIsNoEmptySpaceForProject { get; set; }
    }
}
