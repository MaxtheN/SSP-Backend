using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SspUis.Integration.Sud.Models.AuthModels;

public class SudAuthCreateRequestDto
{
    [JsonPropertyName("client_id")]
    public string ClientId { get; set; }
    [JsonPropertyName("client_secret")]
    public string ClientSecret { get; set; }
}

