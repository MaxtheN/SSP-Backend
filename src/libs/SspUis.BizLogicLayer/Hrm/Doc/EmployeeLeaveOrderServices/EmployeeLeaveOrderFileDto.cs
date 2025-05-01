using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer;

public class EmployeeLeaveOrderFileDto :
    EmployeeLeaveOrderFileDlDto, ILinkToEntity<EmployeeLeaveOrderFile>
{
    public string FileName { get; set; }
    public DateTime CreatedAt { get; set; }
}