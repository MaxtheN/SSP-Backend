using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.DataLayer.EfClasses
{

    public class RequestExcelDto
    {
        public long Id { get; set; }
        public string ContractorInn { get; set; }
        public string Contractor { get; set; }
        public string DocNumber { get; set; }
        public DateTime DocDate { get; set; }
        public int OrderedOrganizationId { get; set; }
        public int? InspectionOrganizationId { get; set; }
        public int AuthorizedOrganizationId { get; set; }
        public string OrderedOrganization { get; set; }
        public string InspectionOrganization { get; set; }
        public string AuthorizedOrganization { get; set; }
        public int CheckTypeId { get; set; }
        public string CheckType { get; set; }
        public string CheckBasis { get; set; }
        public string InspectorStatus { get; set; }
        public string ModeratorStatus { get; set; }
        public string CeoStatus { get; set; }
        public int? InspectorStatusId { get; set; }
        public int? ModeratorStatusId { get; set; }
        public int? CeoStatusId { get; set; }
        public DateTime? CheckStartDate { get; set; }
        public DateTime? CheckEndDate { get; set; }
        public DateTime? CheckCoverageStartDate { get; set; }
        public DateTime? CheckCoverageEndDate { get; set; }
        public int CheckDaysNumber { get; set; }
        public int StatusId { get; set; }
        public int RegionId { get; set; }
        public string Region { get; set; }
        public int DistrictId { get; set; }
        public string District { get; set; }
        public string CheckSubjects { get; set; }
        public string CanViolatedLegalDocuments { get; set; }
        public string ViolatedLegalDocuments { get; set; }
        public bool HasBasicFile { get; set; }
        public bool HasOrderFile { get; set; }
        public DateTime? PostponementStartDate { get; set; }
        public DateTime? PostponementEndDate { get; set; }
        public string PostponementReason { get; set; }
        public int? PostponementStatusId { get; set; }
        public string PostponementInspectorStatus { get; set; }
        public string PostponementModeratorStatus { get; set; }
        public string PostponementCeoStatus { get; set; }
        public bool? HasResultActFile { get; set; }
        public bool? HasMeasuresOfInfluenceFile { get; set; }
        public bool? HasMeasuresResultFile { get; set; }
        public bool? HasCancelledMeasuresFile { get; set; }
        public bool? HasPostponementFile { get; set; }
        public bool? HasPostponement { get; set; }
        public int? PostponementRequestStatusId { get; set; }
        public DateTime? FactStartDate { get; set; }
        public DateTime? FactEndDate { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public string MeasuresOfInfluence { get; set; }
        public string MeasuresResult { get; set; }
        public string CancelledMeasures { get; set; }
        public string Oked { get; set; }
        public int? AgreedCheckDaysNumber { get; set; }
        public string Conclusion { get; set; }
        public string ConclusionComment { get; set; }
        public string OrganizationInspectionType { get; set; }
        public bool? HasNotificationFile { get; set; }
        public string ControlFunctionName { get; set; }
    }
}
