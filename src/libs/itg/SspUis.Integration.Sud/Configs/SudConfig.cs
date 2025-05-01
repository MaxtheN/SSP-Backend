namespace SspUis.Integration.Sud.Configs;

public class SudConfig
{
    public string api { get; set; }
    public string ClientId { get; set; } 
    public string ClientSecret { get; set; } 
    internal string BasicTokenClient { get { return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{ClientId}:{ClientSecret}")); } }
}

