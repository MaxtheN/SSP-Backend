using GenericServices;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.BizLogicLayer.ExecutionApplicationServices;

public class ExecutionApplicationTableDto : ExecutionApplicationTableDlDto, ILinkToEntity<ExecutionApplicationTable>
{
    public string Contractor { get; set; }
    public string ContractorInn { get; set; }
}
