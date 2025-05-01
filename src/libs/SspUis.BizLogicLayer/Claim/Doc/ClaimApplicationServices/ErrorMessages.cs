using SspUis.BizLogicLayer.ClaimApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Claim.Doc.ClaimApplicationServices;

public class ErrorMessages : ClaimApplicationIntegrationResponseDto
{
    public string Message { get; set; }
    public ErrorMessages(string Message)
    {
        this.Message = Message;
    }
}
