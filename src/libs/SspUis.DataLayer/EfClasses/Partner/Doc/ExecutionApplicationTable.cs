using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_execution_application_table", Schema = "partner")]
    [Index(nameof(OwnerId), Name = "uc_owner_id", IsUnique = true)]
    public partial class ExecutionApplicationTable : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("prtn_certificate_id")]
        public long PrtnCertificateId { get; set; }
        [Column("contractor_id")]
        public long ContractorId { get; set; }
        [Column("prtn_new_vacancies_count")]
        public int PrtnNewVacanciesCount { get; set; }
        [Column("project_new_vacancies_count")]
        public int ProjectNewVacanciesCount { get; set; }
        [Column("salary")]
        [Precision(18, 2)]
        public decimal Salary { get; set; }
        [Column("average_salary")]
        [Precision(18, 2)]
        public decimal AverageSalary { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual ExecutionApplication Owner { get; set; }
        [ForeignKey(nameof(ContractorId))]
        public virtual Contractor Contractor { get; set; }
        [ForeignKey(nameof(PrtnCertificateId))]
        public virtual PrtnCertificate PrtnCertificate { get; set; }
    }
}
