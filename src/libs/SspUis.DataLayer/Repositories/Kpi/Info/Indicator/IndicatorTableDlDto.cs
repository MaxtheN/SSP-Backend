using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class IndicatorTableDlDto :
EntityDto<IndicatorTableDlDto, IndicatorTable>,
IHaveIdProp<int>
{
    public int Id { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(9)]
    public string Code { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(500)]
    public string FullName { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(250)]
    public string ShortName { get; set; }
}

