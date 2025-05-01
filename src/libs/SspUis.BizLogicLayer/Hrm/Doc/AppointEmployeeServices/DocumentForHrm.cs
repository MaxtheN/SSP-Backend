using System;
using System.Collections.Generic;
using GenericServices;
using Newtonsoft.Json;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.Models;
using WEBASE.OfficeTools.Attributes;
using WEBASE.OfficeTools.Models;

namespace SspUis.BizLogicLayer.Hrm;
public class DocumentForHrm
{
  public long? DocId { get; set; } 
  public string? Details { get; set; } 
  public long? SignerTableId { get; set; } 
  public long? EmployeeManageId { get; set; } 
}
