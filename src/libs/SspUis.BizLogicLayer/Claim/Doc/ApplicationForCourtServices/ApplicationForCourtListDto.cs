using GenericServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Claim;
using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Claim;

public class ApplicationForCourtListDto : ILinkToEntity<ApplicationForCourt>, IHaveIdProp<long>
{
	public long Id { get; set; }
	public DateOnly DocOn { get; set; }
	public string DocNumber { get; set; }
	public long MediationId { get; set; }
	public int ClaimOrganizationId { get; set; }
	public string ClaimOrganization { get; set; }
	public long ContractorId { get; set; }
	public string ContractorInn { get; set; }
	public string Contractor { get; set; }
	public int StatusId { get; set; }
	public string Status { get; set; }
	public int? StepId { get; set; }
	public string Step { get; set; }
	public string ContractorDetails { get; set; }
	public string Details { get; set; }
	public long? ClaimApplicationEmployeeManageId { get; set; }
	public string ClaimApplicationEmployeeManage { get; set; }
	public int TableId { get; } = TableIdConst.CLAIM__DOC_APPLICATION_FOR_COURT;
	public int PositionId { get; set; }
	public string Position { get; set; }
	public long EmployeeManageId { get; set; }
	public string EmployeeManage { get; set; }
	public int DepartmentId { get; set; }
	public string Department { get; set; }

	public bool CanAccept { get; set; }
	public bool CanCancel { get; set; }
	public bool CanSend { get; set; }
	public bool CanEdit { get; set; }
	public bool CanReject { get; set; }
}