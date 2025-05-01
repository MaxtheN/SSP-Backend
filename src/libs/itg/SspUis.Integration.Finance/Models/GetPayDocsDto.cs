

namespace SspUis.Integration.Finance.Models;

public class GetPayDocsDto
{
    public long Id { get; set; }
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
    public string DocDate { get; set; }
    public string BankDate { get; set; }
    public int FinYear { get; set; }
    public string DocNumb { get; set; }
    public string BankDocId { get; set; }
}
