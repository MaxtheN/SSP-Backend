using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_prtn_application", Schema = "partner")]
    [Index(nameof(ApplicationId), Name = "uc_application_id", IsUnique = true)]
    public partial class PrtnApplication: IHaveIdProp<long>
    {
        public PrtnApplication()
        {
            Graphs = new HashSet<PrtnApplicationGraph>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("application_id")]
        public long ApplicationId { get; set; }
        [Column("prtn_contract_type_id")]
        public int PrtnContractTypeId { get; set; } 
        [Column("new_vacancies_count")]
        public int NewVacanciesCount { get; set; }
        [Column("message")]
        [StringLength(1024)]
        public string Message { get; set; }
        [Column("mfy_id")]
        public long? MfyId { get; set; }
        [Column("mfy")]
        [StringLength(500)]
        public string MfyName { get; set; }
        [Column("mahalla_external_id")]
        public long? MahallaExternalId { get; set; }
        [Column("choose_location")]
        public bool ChooseLocation { get; set; }
        [Column("choosed_region_id")]
        public int? ChoosedRegionId { get; set; }
        [Column("choosed_district_id")]
        public int? ChoosedDistrictId { get; set; }
        [Column("has_been_answered")]
        public bool HasBeenAnswered { get; set; }
        [Column("parameters")]
        public string Parameters { get; set; }
        [Column("offer")]
        public string Offer { get; set; }
        [Column("is_sent")]
        public bool IsSent { get; set; }
        [Column("status_change_expire_on", TypeName = "timestamp without time zone")]
        public DateTime? StatusChangeExpireOn { get; set; }
        [Column("pass_expertise_expire_on", TypeName = "timestamp without time zone")]
        public DateTime? PassExpertiseExpireOn { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [Column("is_read")]
        public bool IsRead { get; set; }

        [ForeignKey(nameof(ApplicationId))]
        [InverseProperty(nameof(EfClasses.Application.PrtnApplication))]
        public virtual Application Application { get; set; }
        [ForeignKey(nameof(CreatedUserId))]
        public virtual BusinessmanUser CreatedUser { get; set; }
        [ForeignKey(nameof(PrtnContractTypeId))]
        public virtual PrtnContractType PrtnContractType { get; set; }
        [ForeignKey(nameof(ChoosedRegionId))]
        public virtual Region ChoosedRegion { get; set; }
        [ForeignKey(nameof(ChoosedDistrictId))]
        public virtual District ChoosedDistrict { get; set; }
        [ForeignKey(nameof(MfyId))]
        public virtual Mfy Mfy { get; set; }

        [InverseProperty(nameof(PrtnApplicationGraph.Owner))]
        public virtual ICollection<PrtnApplicationGraph> Graphs { get; set; }
    }
}
