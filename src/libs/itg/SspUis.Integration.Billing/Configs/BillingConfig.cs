namespace SspUis.Integration.Billing.Configs;

public class BillingConfig
{
    public string api { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    internal string BasicTokenClient 
    { 
        get 
        { 
            return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{Username}:{Password}")); 
        } 
    }
}
