using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Report.Func;

[Keyless]
public class BankCreditReportFuncDto
{
    [Column("contractorinn")]
    public string contractorinn { get; set; }

    [Column("contractor")]
    public string contractor { get; set; }

    [Column("year")]
    public string year { get; set; }

    [Column("submittedcount")]
    public long submittedcount { get; set; }

    [Column("approvedcount")]
    public long approvedcount { get; set; }

    [Column("canceledcount")]
    public long canceledcount { get; set; }

    [Column("rejectedcount")]
    public long rejectedcount { get; set; }

    [Column("issuancecount")]
    public long issuancecount { get; set; }

    [Column("submittedsum")]
    public decimal? submittedsum { get; set; }

    [Column("rejectedsum")]
    public decimal? rejectedsum { get; set; }

    [Column("canceledsum")]
    public decimal? canceledsum { get; set; }

    [Column("approvedsum")]
    public decimal? approvedsum { get; set; }

    [Column("issuancesum")]
    public decimal? issuancesum { get; set; }

    [Column("submittedcount1")]
    public long submittedcount1 { get; set; }

    [Column("approvedcount1")]
    public long approvedcount1 { get; set; }

    [Column("canceledcount1")]
    public long canceledcount1 { get; set; }

    [Column("rejectedcount1")]
    public long rejectedcount1 { get; set; }

    [Column("issuancecount1")]
    public long issuancecount1 { get; set; }

    [Column("submittedsum1")]
    public decimal? submittedsum1 { get; set; }

    [Column("rejectedsum1")]
    public decimal? rejectedsum1 { get; set; }

    [Column("canceledsum1")]
    public decimal? canceledsum1 { get; set; }

    [Column("approvedsum1")]
    public decimal? approvedsum1 { get; set; }

    [Column("issuancesum1")]
    public decimal? issuancesum1 { get; set; }

    [Column("submittedcount2")]
    public long submittedcount2 { get; set; }

    [Column("approvedcount2")]
    public long approvedcount2 { get; set; }

    [Column("canceledcount2")]
    public long canceledcount2 { get; set; }

    [Column("rejectedcount2")]
    public long rejectedcount2 { get; set; }

    [Column("issuancecount2")]
    public long issuancecount2 { get; set; }

    [Column("submittedsum2")]
    public decimal? submittedsum2 { get; set; }

    [Column("rejectedsum2")]
    public decimal? rejectedsum2 { get; set; }

    [Column("canceledsum2")]
    public decimal? canceledsum2 { get; set; }

    [Column("approvedsum2")]
    public decimal? approvedsum2 { get; set; }

    [Column("issuancesum2")]
    public decimal? issuancesum2 { get; set; }

    [Column("submittedcount3")]
    public long submittedcount3 { get; set; }

    [Column("approvedcount3")]
    public long approvedcount3 { get; set; }

    [Column("canceledcount3")]
    public long canceledcount3 { get; set; }

    [Column("rejectedcount3")]
    public long rejectedcount3 { get; set; }

    [Column("issuancecount3")]
    public long issuancecount3 { get; set; }

    [Column("submittedsum3")]
    public decimal? submittedsum3 { get; set; }

    [Column("rejectedsum3")]
    public decimal? rejectedsum3 { get; set; }

    [Column("canceledsum3")]
    public decimal? canceledsum3 { get; set; }

    [Column("approvedsum3")]
    public decimal? approvedsum3 { get; set; }

    [Column("issuancesum3")]
    public decimal? issuancesum3 { get; set; }
}