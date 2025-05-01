using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.BizLogicLayer;

public class ExecutionApplicationCellDto
{
    public long PrtnCertificateId { get; set; }
    public long ContractorId { get; set; }
    public string Contractor { get; set; }
    public string ContractorInn { get; set; }
    public int PrtnNewVacanciesCount { get; set; }
    public int ProjectNewVacanciesCount { get; set; }
    public decimal Salary { get; set; }
    public decimal AverageSalary { get; set; }
}

