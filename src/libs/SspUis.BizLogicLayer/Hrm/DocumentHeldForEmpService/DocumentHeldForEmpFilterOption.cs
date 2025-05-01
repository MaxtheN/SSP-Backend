using SspUis.Core;
using System;
using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;
public class DocumentHeldForEmpFilterOption : SortFilterPageOptions
{
    public long Id { get; set; }
    public string DocNumber { get; set; } = null!;
    public string Status { get; set; }
    public int? StatusId { get; set; }
    public string Organization { get; set; }
    public string? Details { get; set; }
    public int? TableId { get; set; }
    public string Table { get; set; }
    public string Employee { get; set; }
    public int? PersonId { get; set; }
}