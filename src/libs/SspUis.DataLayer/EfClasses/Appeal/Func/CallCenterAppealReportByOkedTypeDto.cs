using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SspUis.DataLayer;
[Keyless]
public class CallCenterAppealReportByOkedTypeDto
{
    [Column("id")]
    public int? Id { get; set; }

    [Column("full_name")]
    public string fullName { get; set; }

    [Column("order_code")]
    public string order_code { get; set; }

    [Column("document_count")]
    public long document_count { get; set; }

    [Column("document_percentage")]
    public string document_percentage { get; set; }

    [Column("document_count_region_6")]
    public long document_count_region_6 { get; set; }

    [Column("document_count_region_3")]
    public long document_count_region_3 { get; set; }

    [Column("document_count_region_4")]
    public long document_count_region_4 { get; set; }

    [Column("document_count_region_5")]
    public long document_count_region_5 { get; set; }

    [Column("document_count_region_7")]
    public long document_count_region_7 { get; set; }

    [Column("document_count_region_8")]
    public long document_count_region_8 { get; set; }

    [Column("document_count_region_9")]
    public long document_count_region_9 { get; set; }

    [Column("document_count_region_10")]
    public long document_count_region_10 { get; set; }

    [Column("document_count_region_11")]
    public long document_count_region_11 { get; set; }

    [Column("document_count_region_12")]
    public long document_count_region_12 { get; set; }

    [Column("document_count_region_2")]
    public long document_count_region_2 { get; set; }

    [Column("document_count_region_13")]
    public long document_count_region_13 { get; set; }

    [Column("document_count_region_14")]
    public long document_count_region_14 { get; set; }

    [Column("document_count_region_1")]
    public long document_count_region_1 { get; set; }

    [Column("total_document_count")]
    public decimal total_document_count { get; set; }
}