using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ExecutionApplicationServices;

public class ExecutionApplicationListDto : DocumentListDto<long>, ILinkToEntity<ExecutionApplication>, IHaveIdProp<long>
{
    public string DocNumber { get; set; }
    public string Region { get; set; }
    public string District { get; set; }
    public string Organization { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int TotalNewVacanciesCount { get; set; }
    public decimal TotalPaymentAmount { get; set; }
    public decimal TotalAverageSalary { get; set; }
    public string Status { get; set; }

    #region Actions
    public bool CanDelete { get; set; }
    public bool CanSign { get; set; }
    public bool CanEdit { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    #endregion
}

