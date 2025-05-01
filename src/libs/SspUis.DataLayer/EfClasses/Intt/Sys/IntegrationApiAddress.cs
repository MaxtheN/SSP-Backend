
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace SspUis.DataLayer.EfClasses;

[Table("sys_integration_api_address", Schema = "intt")]
public class IntegrationApiAddress
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Required]
    [Column("name")]
    public string Name { get; set; }
    [Required]
    [Column("url")]
    public string Url { get; set; }
    [Required]
    [Column("http_method_code")]
    public string HttpMethodCode { get; set; }

    [Column("post_data")]
    public string PostData { get; set; }

    [Column("headers")]
    public string[] Headers { get; set; }   
    
    [Required]
    [Column("for_test_server")]
    public bool ForTestServer { get; set; }

    [Required]
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


    [ForeignKey(nameof(StateId))]
    public virtual State State { get; set; }
}
