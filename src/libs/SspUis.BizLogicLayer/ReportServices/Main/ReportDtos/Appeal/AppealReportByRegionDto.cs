using System.Collections.Generic;
using System.Linq;

namespace SspUis.BizLogicLayer;

public class AppealReportByTypeReportDto
{
    public int? RegionId { get; set; }
    public string RegionName { get; set; }
    public string RegionCode { get; set; }
    public string DistrictName { get; set; }
    public int? DistrictId { get; set; }

    public (int? Application, int? Proposal, int? Complaint) TotalAppealApplicationType { get; set; }
    public (int? Application, int? Proposal, int? Complaint) TotalAppealApplicationTypeSent { get; set; }
    public (int? Application, int? Proposal, int? Complaint) TotalAppealApplicationTypeCreate { get; set; }
    public (int? Application, int? Proposal, int? Complaint) TotalAppealApplicationTypeInExecution { get; set; }
    public (int? Application, int? Proposal, int? Complaint) TotalAppealApplicationTypeExecuted { get; set; }
    public long TotalAppealApplicationCanceled { get; set; }
}
