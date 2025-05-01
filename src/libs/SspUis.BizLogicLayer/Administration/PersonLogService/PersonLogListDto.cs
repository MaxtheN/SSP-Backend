using GenericServices;
using SspUis.DataLayer.EfClasses.Public.Hl;
using System;

namespace SspUis.BizLogicLayer;

public class PersonLogListDto  :  ILinkToEntity<PersonLog>
{
    public int Id { get; set; }
    public string Pinfl { get; set; }
    public string PassportSeria { get; set; }
    public string PassportNumber { get; set; }
    public DateTime? PassportDate { get; set; }
    public DateTime? PassportExpiration { get; set; }
    public string PassportDivName { get; set; }
    public int PersonId { get; set; }
    public string? Person { get; set; }
    public int EmployeeId { get; set; }
}
    
