using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;
[Table("doc_arbitration_discussion_file", Schema = "arbitration")]
public class ArbitrationDiscussionFile : FileEntity<long>, IHaveIdProp<Guid>
{
	[Required]
	[Column("can_sign")]
	public bool CanSign { get; set; } = false;
	[Required]
	[Column("arbitration_step_id")]
	public int ArbitrationStepId { get; set; } 	
	[ForeignKey(nameof(OwnerId))]
	[InverseProperty(nameof(ArbitrationDiscussion.Files))]
	public ArbitrationDiscussion Owner { get; set; }
}
