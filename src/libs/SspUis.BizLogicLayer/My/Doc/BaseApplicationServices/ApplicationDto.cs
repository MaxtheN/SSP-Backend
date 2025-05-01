using System;
using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Doc.BaseApplication;

namespace SspUis.BizLogicLayer;

public class ApplicationDto : ApplicationDlDto, IDocument, ILinkToEntity<Application>
{
    public long Id { get; set; }
    public Guid Id2 { get; set; }
    public int TableId { get; } = TableIdConst.DOC_APPLICATION;
    public int StatusId { get; set; }
    public string Status { get; set; }
    public string Contractor { get; set; }
    public long? ContractorId { get; set; }
    public string ContractorDirector { get; set; }
    public string ContractorInn { get; set; }
    public int? ContractorOpfId { get; set; }
    public string Message { get; set; }
    public string ContractorAddress { get; set; }
    public string ContractorForm { get; set; }
    public string ApplicationType { get; set; }
    //public new int ApplicationTypeId { get; set; }
    public int RegionId { get; set; }
    public ApplicationTypeStepDto CurrentStep { get; set; }
    public int DistrictId { get; set; }
    public string Region { get; set; }
    public string District { get; set; }
    public new List<ApplicationStepDto> Steps { get; set; } = new();
}
