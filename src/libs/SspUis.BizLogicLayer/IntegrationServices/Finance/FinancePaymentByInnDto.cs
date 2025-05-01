using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class FinancePaymentByInnDto
{
    public long Id { get; set; }
    public string Inn {  get; set; }
    public decimal SumPay {  get; set; }
    public string Details {  get; set; }

}

public class FinancePaymentByInnDtoSortFilterPageOptions: SortFilterPageOptions
{
    public string? Inn { get; set; }

}