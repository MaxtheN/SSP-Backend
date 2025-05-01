using System;
using System.Collections.Generic;
using AutoMapper;
using GenericServices;
using SspUis.BizLogicLayer.ArbitrationCourtApplicationServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer;

public class ArbitrationResultDto :
    UpdateArbitrationResultDlDto,
    ILinkToEntity<ArbitrationResult>,
    IDocument
{
    public decimal Amount { get; set; }
    public string Status { get; set; }
    public int StatusId { get; set; }
    public long? ContractorId { get; set; }
    public string Contractor { get; set; }
    public long? ResponsibleContractorId { get; set; }
    public string ResponsibleContractor { get; set; }
    public ArbitrationCourtApplicationDto ArbitrationCourtApplication { get; set; }

    public new List<ArbitrationResultSignDto> Signs { get; set; } = new();
    public new List<ArbitrationResultFileDto> Files { get; set; } = new();

    #region Action
    public bool CanEdit { get; set; }
    public bool CanSign { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}
