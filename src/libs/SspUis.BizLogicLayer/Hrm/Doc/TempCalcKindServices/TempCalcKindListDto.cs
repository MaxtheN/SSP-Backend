using System;
using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public class TempCalcKindListDto : ILinkToEntity<TempCalcKind>, IHaveIdProp<long>
{
    public long Id { get; set; }
    public string DocNumber { get; set; }
    public DateOnly DocOn { get; set; }
    public string Details { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; }
    public int CalculationKindId { get; set; }
    public string CalculationKind { get; set; }
    public int RoundingTypeId { get; set; }
    public string TempCalcKindType { get; set; }

    #region Actions
    public bool CanDelete { get; set; }
    public bool CanEdit { get; set; }
    public bool CanSign { get; set; }
    public bool CanCancel { get; set; }
    #endregion
}
