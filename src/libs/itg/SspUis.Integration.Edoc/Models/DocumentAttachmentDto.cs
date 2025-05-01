using Newtonsoft.Json;
using SspUis.Core;

namespace SspUis.Integration.Edoc.Models;

public class DocumentAttachmentDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public int CreatedUserId { get; set; }
    public string CreatedUser { get; set; }
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? DateOfCreated { get; set; }
    public string FileExtension { get; set; }
}
