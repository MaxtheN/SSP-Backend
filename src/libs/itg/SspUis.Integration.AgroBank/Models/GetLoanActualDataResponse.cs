using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.AgroBank.Models;

public class GetLoanActualDataResponse
{
    public string Branch { get; set; }
    public string CbsId { get; set; }
    public string LoanId { get; set; }
    public string Currency { get; set;}
    public int BalanceRemainder { get; set; }
    public int OverdueRemainder { get; set; }
    public int PercentsRemainder { get; set; }
    public int PenaltiesRemainder { get; set; }

}

public class GetLoanActualDataResponse2
{
    public string Branch { get; set; }
    public string CbsId { get; set; }
    public string LoanId { get; set; }
    public string Currency { get; set; }
    public int OverdueMainDept { get; set; }
    public int OverduePercentDept { get; set; }
    public int CurrentMainDept { get; set; }
    public int PenaltyInterest { get; set; }
    public int OverdueDebtAccruedInterest { get; set; }
    public int AccruedInterestCurrentDebt { get; set; }

}


