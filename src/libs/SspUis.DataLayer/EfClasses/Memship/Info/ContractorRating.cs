using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("info_contractor_rating", Schema = "memship")]
public partial class ContractorRating : IHaveIdProp<int>, IHaveStateId
{
    public ContractorRating()
    {
        Translates = new HashSet<ContractorRatingTranslate>();
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("code")]
    [StringLength(50)]
    public string Code { get; set; }
    [Column("ordercode")]
    [StringLength(250)]
    public string Ordercode { get; set; }
    
    [Column("minimum_percentage")]
    public decimal MinimumPercentage { get; set; }
    [Column("maximum_percentage")]
    public decimal MaximumPercentage { get; set; }
    [Column("score")]
    public int Score { get; set; }
    [Column("state_id")]
    public int StateId { get; set; }
    [Column("contractor_type_id")]
    public int ContractorTypeId { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(ContractorTypeId))]
    public virtual ContractorType ContractorType { get; set; }
    [ForeignKey(nameof(StateId))]
    public virtual State State { get; set; }
    [InverseProperty(nameof(ContractorRatingTranslate.Owner))]
    public virtual ICollection<ContractorRatingTranslate> Translates { get; set; }
}