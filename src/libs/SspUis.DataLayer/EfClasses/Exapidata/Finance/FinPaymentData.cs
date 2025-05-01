using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.DataLayer.EfClasses.Exapidata.Finance;

[Table("fin_payment_data",Schema ="exapidata")] 
public class FinPaymentData
{
    [Column("id")]
    public long Id { get; set; }
    [Column("acc")]
    public string Acc { get; set; }
    [Column("cl_acc")]
    public string ClAcc { get; set; }
    [Column("cl_name")]
    public string ClName { get; set; }
    [Column("cl_mfo")]
    public string ClMfo { get; set; }
    [Column("cl_inn")]
    public string ClInn { get; set; }
    [Column("co_acc")]
    public string CoAcc { get; set; }
    [Column("co_name")]
    public string CoName { get; set; }
    [Column("co_mfo")]
    public string CoMfo { get; set; }
    [Column("co_inn")]
    public string CoInn { get; set; }
    [Column("sum_pay")]
    public decimal SumPay { get; set; }
    [Column("purpose")]
    public string Purpose { get; set; }
    [Column("doc_date")]
    public DateOnly DocDate { get; set; }
    [Column("bank_date")]
    public DateOnly BankDate { get; set; }
    [Column("fin_year")]
    public int FinYear { get; set; }
    [Column("doc_numb")]
    public string DocNumb { get; set; }
    [Column("bank_doc_id")]
    public string BankDocId { get; set; }
}

