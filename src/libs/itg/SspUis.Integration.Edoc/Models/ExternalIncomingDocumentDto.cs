using Newtonsoft.Json;
using SspUis.Core;

namespace SspUis.Integration.Edoc.Models;

public class ExternalIncomingDocumentDto
{
    public Guid Id { get; set; }
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? TermExecution { get; set; }
    public string Assignment { get; set; }
    public string?  Organization { get; set; }
    public string RegNumber { get; set; }
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? RegDate { get; set; }
    public int ProcessId { get; set; }
    public List<DocumentAttachmentDto> Attachments { get; set; } = new();
}
