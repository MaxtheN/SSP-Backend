using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.DigitizationCenter
{
    public class DigitizationCenterLoginResponseDto
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public string Scope { get; set; }

        public string TokenType { get; set; }

        public int ExpiresIn { get; set; }
    }
}
