using System;
using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public class AppointEmployeeListDto : DocumentListDto<long>, ILinkToEntity<AppointEmployee>, IHaveIdProp<long>
{
    public string DocNumber { get; set; } = null!;
    public string Status { get; set; }
    public string Organization { get; set; }
    public int OrganizationId { get; set; }
    public IEnumerable<string> Employees { get; set; }
    //public IEnumerable<string> EmpAppointOrderTypes { get; set; }
    public int[] EmployeesId { get; set; }
    public int[] EmpAppointOrderTypeId { get; set; }
    public string? Details { get; set; }

    #region Actions
    public bool CanDelete { get; set; }
    public bool CanSign { get; set; }
    public bool CanWithOutSigner { get; set; }
    public bool CanSignAnyway { get; set; }
    public bool CanEdit { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    #endregion
}
