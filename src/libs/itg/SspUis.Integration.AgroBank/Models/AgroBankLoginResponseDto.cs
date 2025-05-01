using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.AgroBank.Models;

public class AgroBankLoginResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string TokenType { get; set; }
    public int RefreshExpiresIn { get; set; }
    public int ExpiresIn { get; set; }
    public int NotBeforePolicy { get; set; }
    public string Scope { get; set; }
    public string SessionState { get; set; }
}

public class AgroBankLoginRequestDto
{
    public string username { get; set; }
    public string password { get; set; }
}
