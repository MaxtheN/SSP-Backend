using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer;
public class EmployeeSendTrainFileDto :
    EmployeeSendTrainFileDlDto, ILinkToEntity<EmployeeSendTrainFile>
{
    public string FileName { get; set; }
    public DateTime CreatedAt { get; set; }
}