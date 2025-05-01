using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.IntegrationServices;

public class FinPaymentDataDto
{
    public long Id {  get; set; }
    public long Id2 {  get; set; }
    public string Acc { get; set; }
    public string ClAcc { get; set; }
    public string ClName { get; set; }
    public string ClMfo { get; set; }
    public string ClInn { get; set; }
    public string CoAcc { get; set; }
    public string CoName { get; set; }
    public string CoMfo { get; set; }
    public string CoInn { get; set; }
    public decimal SumPay { get; set; }
    public string Purpose { get; set; }
    public DateOnly DocDate { get; set; }
    public DateOnly BankDate { get; set; }
    public int FinYear { get; set; }
    public string DocNumb { get; set; }
    public string BankDocId { get; set; }
    
}
