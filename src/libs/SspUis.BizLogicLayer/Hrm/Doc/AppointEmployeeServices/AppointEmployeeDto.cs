using System;
using System.Collections.Generic;
using GenericServices;
using Newtonsoft.Json;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.Models;
using WEBASE.OfficeTools.Attributes;
using WEBASE.OfficeTools.Models;

namespace SspUis.BizLogicLayer.Hrm;
[PrintableModel("Hodimni ishga olish", TableIdConst.DOC_APPOINT_EMPLOYEE)]
public class AppointEmployeeDto : UpdateAppointEmployeeDlDto, ILinkToEntity<AppointEmployee>, IDocument
{
    public Guid Id2 { get; set; }
    public string Status { get; set; }
    public string Organization { get; set; }
    public new string ConclusionForPrint { get; set; }
    public string Region { get; set; }
    public List<PersonDto> Employees { get; set; }
    public int TableId { get; set; }
    public int StatusId { get; set; }
    public new string Details { get; set; }
    public new List<AppointEmployeeTableDto> Tables { get; set; } = new();
    public new List<AppointEmployeeSignerDto> Signer { get; set; } = new();
    public new List<AppointEmployeeFileDto> Files { get; set; } = new();
    

    #region Actions
    public bool CanModify { get; set; }
    public bool CanSign { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    #endregion

    [JsonIgnore]
    public QrCodeModel QrCode { get; set; }
}
