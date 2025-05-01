using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses.DualEdu;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer;

public class SubsidyRequestTableDto : SubsidyRequestTableDlDto, ILinkToEntity<SubsidyRequestTable>
{
    public string Pinfl { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronym { get; set; }
    //public string PassportSeria { get; set; }
    //public string PassportNumber { get; set; }
    //public DateOnly BirthDate { get; set; }
    public new List<SubsidyRequestFileDto> Files { get; set; } = new();
}
