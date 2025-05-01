using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.CustomJobServices;
public class CustomJobDto : UpdateCustomJobDlDto, ILinkToEntity<CustomJob>, IDocument
{
    public DateOnly DocDate { get; set; }
    public string JobType { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; }
    public string Organization { get; set; }
    public string District { get; set; }
    public string Region { get; set; }
}
