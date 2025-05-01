using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_mono_application", Schema = "my")]
    [Index(nameof(ApplicationId), Name = "uc_application_id", IsUnique = true)]
    public partial class MonoApplication : IHaveIdProp<long>, IBaseApplicationEntity, IJobDocumentEntity, IDocument<long>
    {
        public MonoApplication()
        {
            Files = new HashSet<MonoApplicationFile>();
            ItemTables = new HashSet<MonoApplicationItemTable>();
            StudentTables = new HashSet<MonoApplicationStudentTable>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("application_id")]
        public long ApplicationId { get; set; }
        [Column("mfy_id")]
        public long MfyId { get; set; }
        [Column("total_amount")]
        [Precision(18, 2)]
        public decimal TotalAmount { get; set; }
		[Column("total_cost")]
		[Precision(18, 2)]
		public decimal TotalCost { get; set; }
		[Column("currency_id")]
        public int CurrencyId { get; set; }
        [Column("spend_for_build")]
        [Precision(18, 2)]
        public decimal SpendForBuild { get; set; }
        [Column("mono_mfy_id")]
        public long MonoMfyId { get; set; }
        [Column("mono_region_id")]
        public int MonoRegionId { get; set; }
        [Column("mono_district_id")]
        public int MonoDistrictId { get; set; }
        [Required]
        [Column("mono_adress")]
        [StringLength(250)]
        public string MonoAdress { get; set; }
        [Column("building_count")]
        public int BuildingCount { get; set; }
        [Column("learning_area")]
        [Precision(18, 2)]
        public decimal LearningArea { get; set; }
        [Column("total_area")]
        [Precision(18, 2)]
        public decimal TotalArea { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("prev_status_id")]
        public int? PrevStatusId { get; set; }
        [Column("table_id")]
        public int TableId { get; set; }
        [Column("message")]
        public string Message { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(ApplicationId))]
        [InverseProperty(nameof(EfClasses.Application.MonoApplications))]
        public virtual Application Application { get; set; }
        [ForeignKey(nameof(CurrencyId))]
        public virtual Currency Currency { get; set; }
        [ForeignKey(nameof(MfyId))]
        public virtual Mfy Mfy { get; set; }
        [ForeignKey(nameof(MonoDistrictId))]
        public virtual District MonoDistrict { get; set; }



		[ForeignKey(nameof(MonoMfyId))]
        public virtual Mfy MonoMfy { get; set; }
        [ForeignKey(nameof(MonoRegionId))]
        public virtual Region MonoRegion { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [ForeignKey(nameof(TableId))]
        public virtual Table Table { get; set; }
        [ForeignKey(nameof(PrevStatusId))]
        public virtual Status PrevStatus { get; set; }
        [InverseProperty(nameof(MonoApplicationFile.Owner))]
        public virtual ICollection<MonoApplicationFile> Files { get; set; }
        [InverseProperty(nameof(MonoApplicationItemTable.Owner))]
        public virtual ICollection<MonoApplicationItemTable> ItemTables { get; set; }
        [InverseProperty(nameof(MonoApplicationStudentTable.Owner))]
        public virtual ICollection<MonoApplicationStudentTable> StudentTables { get; set; }
        public virtual ICollection<MonoApplicationBandlikResult> MonoApplicationBandlikResults { get; set; }
    }
}
