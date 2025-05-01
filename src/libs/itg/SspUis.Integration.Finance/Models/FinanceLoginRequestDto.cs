using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SspUis.Integration.Finance.Models;

public class FinanceLoginRequestDto
{
    [JsonProperty("login")]
    public string Login { get; set; }
    [JsonProperty("password")]
    public string Password { get; set; } 
}

