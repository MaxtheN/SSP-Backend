using System;
using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public class ChastisementListDto : DocumentListDto<long>, ILinkToEntity<Chastisement>, IHaveIdProp<long>, IHaveStatusId
{
    public string DocNumber { get; set; }
    public string Organization { get; set; }
    public string Status { get; set; } 
    public string Details { get; set; }
    public string Employee { get; set; }

    #region Actions
    public bool CanDelete { get; set; }
    public bool CanSign { get; set; }
    public bool CanEdit { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    #endregion
}
