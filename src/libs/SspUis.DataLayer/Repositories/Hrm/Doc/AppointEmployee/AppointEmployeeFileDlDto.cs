using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class AppointEmployeeFileDlDto :
    EntityDto<AppointEmployeeFileDlDto, AppointEmployeeFile>
    , IHaveIdProp<Guid>
{
    [LocalizedRequired]
    public Guid Id { get; set; }
    public bool? IsReject { get; set; }
}