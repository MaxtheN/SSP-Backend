using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_need_chamber_service", Schema = "public")]
    public partial class NeedChamberService : IHaveIdProp<int>, IHaveStateId
    {
        public NeedChamberService()
        {
            MemshipApplicationChamberServices = new HashSet<MemshipApplicationChamberService>();
            Files = new HashSet<NeedChamberServiceFile>();
            Translates = new HashSet<NeedChamberServiceTranslate>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("code")]
        [StringLength(9)]
        public string Code { get; set; }

        [Column("order_code")]
        [StringLength(50)]
        public string OrderCode { get; set; }

        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }

        [Required]
        [Column("full_name")]
        [StringLength(500)]
        public string FullName { get; set; }

        [Required]
        [Column("can_pay_divided")]
        public bool CanPayDivided { get; set; }

        [Required]
        [Column("is_offer")]
        public bool IsOffer { get; set; }

        [Column("state_id")]
        public int StateId { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }

        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [Column("details")]
        public string Details { get; set; }

        [Column("meeting_type_id")]
        public int? MeetingTypeId { get; set; }

        [Column("service_price_type_id")]
        public int ServicePriceTypeId { get; set; }

        [Column("employee_manage_id")]
        public long? EmployeeManageId { get; set; }

        [Column("need_chamber_service_group_id")]
        public int? NeedChamberServiceGroupId { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }

        [ForeignKey(nameof(EmployeeManageId))]
        [InverseProperty(nameof(EmployeeManage.NeedChamberServices))]
        public virtual EmployeeManage EmployeeManages { get; set; }

        [ForeignKey(nameof(NeedChamberServiceGroupId))]
        public virtual NeedChamberServiceGroup NeedChamberServiceGroup { get; set; }

        [ForeignKey(nameof(MeetingTypeId))]
        [InverseProperty(nameof(MeetingType.NeedChamberServices))]
        public virtual MeetingType MeetingTypes { get; set; }

        [ForeignKey(nameof(ServicePriceTypeId))]
        public virtual ServicePriceType ServicePriceType { get; set; }

        [InverseProperty(nameof(NeedChamberServiceFile.Owner))]
        public virtual ICollection<NeedChamberServiceFile> Files { get; set; }

        [InverseProperty(nameof(MemshipApplicationChamberService.NeedChamberService))]
        public virtual ICollection<MemshipApplicationChamberService> MemshipApplicationChamberServices { get; set; }

        [InverseProperty(nameof(NeedChamberServiceTranslate.Owner))]
        public virtual ICollection<NeedChamberServiceTranslate> Translates { get; set; }
    }
}
