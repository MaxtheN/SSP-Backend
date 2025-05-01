using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using GenericServices;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.OfficeTools.Models;

namespace SspUis.BizLogicLayer.Hrm;

public class ChastisementDto : UpdateChastisementDlDto, ILinkToEntity<Chastisement>, IDocument
{
	public Guid Id2 { get; set; }
    public string Region { get; set; }
    public string Organization { get; set; } = null!;
    public string OrgActivityType{ get; set; } = null!;
    public string Status { get; set; } = null!;
    public int StatusId { get; set; }
    public int TableId { get; set; }
    public string? Message { get; set; }
    public new List<PersonDto> Employees { get; set; } = new();
	public new string ConclusionForPrint { get; set; }
	public new List<ChastisementSignerDto> Signer { get; set; } = new();
    public new List<ChastisementTableDto> Tables { get; set; } = new();
    public List<ChastisementFileDto> Files { get; set; } = new();

    #region Actions
    public bool CanModify { get; set; }
    public bool CanSign { get; set; }
    public bool SignEmployee { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
	#endregion

	
}
