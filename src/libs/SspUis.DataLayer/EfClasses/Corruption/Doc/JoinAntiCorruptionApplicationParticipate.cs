using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_join_anti_corruption_application_participate", Schema = "corruption")]
    public partial class JoinAntiCorruptionApplicationParticipate : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("order_number")]
        public int OrderNumber { get; set; }
        [Column("year_in")]
        public int YearIn { get; set; }
        [Required]
        [Column("investigation_organization")]
        [StringLength(500)]
        public string InvestigationOrganization { get; set; }
        [Required]
        [Column("basis_for_investigation")]
        [StringLength(500)]
        public string BasisForInvestigation { get; set; }
        [Required]
        [Column("investigated_person_fio")]
        [StringLength(250)]
        public string InvestigatedPersonFio { get; set; }
        [Required]
        [Column("investigated_result")]
        public string InvestigatedResult { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(JoinAntiCorruptionApplication.Participates))]
        public virtual JoinAntiCorruptionApplication Owner { get; set; }
    }
}
