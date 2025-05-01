using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeElectionMemberDto : EmployeeElectionMemberDlDto, ILinkToEntity<EmployeeElectionMember>
{
    public string ElectionMember { get; set; }
}
