using System;
using System.Collections.Generic;
using GenericServices;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class TempCalcKindDto : UpdateTempCalcKindDlDto, ILinkToEntity<TempCalcKind>, IDocument
{
    public Guid Id2 { get; set; }
    public string Status { get; set; }
    public string Organization { get; set; }
    public string Region { get; set; }
    public string ConclusionForPrint { get; set; }
    public string CalculationKind { get; set; }
    public new string Details { get; set; }
	public List<PersonDto> Employees { get; set; }
	public string DocDetails { get; set; }
    public string TempCalcKindType { get; set; }
    public int StatusId { get; set; }
    public string? Message { get; set; }
    public new List<TempCalcKindSignerDto> Signer { get; set; } = new();
    new public List<TempCalcKindTableDto> Tables { get; set; }
    public List<TempCalcKindFileDto> Files { get; set; } = new();
    #region Actions
    public bool CanModify { get; set; }
    public bool CanSign { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}
