using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.OneId;

internal class ErrorResponseDto
{
    [JsonProperty("status")]
    public int Status { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    [JsonProperty("path")]
    public string Path { get; set; }

    [JsonProperty("error")]
    public string Error { get; set; }

    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; set; }
}
