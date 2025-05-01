
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using System.Reactive;
using System;
using Ssp.DataLayer.EFClasses.Edoc;

namespace SspUis.DataLayer.EfClasses;

[Table("sys_integration_api_test_log", Schema = "intt")]
public class IntegrationApiTestLog
{
    [Column("id")]
    [Key]
    public long Id { get; set; }
    [Column("address_id")]
    public int AddressId { get; set; }
    
    [Column("date_at", TypeName = "timestamp without time zone")]
    public DateTime DateAt { get; set; }

    [Column("status")]
    public string Status { get; set; }

    [Column("message")]
    public string Message { get; set; }

    [Required]
    [Column("http_status")]
    public string HttpStatus { get; set; }

    [Column("exception")]
    public string Exception { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }

    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }

    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(AddressId))]
    public virtual IntegrationApiAddress Address { get; set; }
}
