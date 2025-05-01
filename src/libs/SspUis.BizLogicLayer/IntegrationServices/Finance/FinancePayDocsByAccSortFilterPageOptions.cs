using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.IntegrationServices.Finance;

public class FinancePayDocsByAccSortFilterPageOptions:SortFilterPageOptions
{
    public DateOnly? FromDate {  get; set; }
    public DateOnly? ToDate {  get; set; }
    public string? AccountNumber {  get; set; }

}
