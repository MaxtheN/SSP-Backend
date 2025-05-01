using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeMilitaryRankDto : EmployeeMilitaryRankDlDto, ILinkToEntity<EmployeeMilitaryRank>
{
    public string MilitaryRanks { get; set; }
}
