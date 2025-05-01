using System;
using System.Collections.Generic;
using GenericServices;
using Newtonsoft.Json;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeLeaveOrderDto : UpdateEmployeeLeaveOrderDlDto, ILinkToEntity<EmployeeLeaveOrder>, IDocument
{
    public Guid Id2 { get; set; }
    public string Organization { get; set; } 
    public string Region { get; set; } 
    public string OrgActivityType { get; set; } 
    public string Status { get; set; }
    public int StatusId { get; set; }
    public int TableId { get; set; }
   // public int OrganizationId { get; set; }
    public string? EmployeeSickLeaveType { get; set; }
    public string? Message { get; set; }
    public  List<PersonDto> Employees { get; set; }
    public new List<EmployeeLeaveOrderSignerDto> Signer { get; set; } = new();
    public new List<EmployeeLeaveOrderTableDto> Tables { get; set; } = new();
    public new List<EmployeeLeaveOrderFileDto> Files { get; set; } = new();

    #region Actions
    public bool CanModify { get; set; }
    public bool CanSign { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    #endregion

}
