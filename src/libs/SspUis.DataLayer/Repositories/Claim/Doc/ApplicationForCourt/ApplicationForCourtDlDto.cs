using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Claim;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;
public class ApplicationForCourtDlDto<TDto> : EntityDto<TDto, ApplicationForCourt>
		where TDto : ApplicationForCourtDlDto<TDto>
{
	[LocalizedRequired]
	public DateOnly DocOn { get; set; }

	[LocalizedRequired]
	[LocalizedStringLength(50)]
	public string DocNumber { get; set; }
	public long? MediationId { get; set; }
	public long? ApplicationId { get; set; }
	[LocalizedRequired]
	[LocalizedRange(1, int.MaxValue)]
	public int ClaimOrganizationId { get; set; }

	[LocalizedRequired]
	[LocalizedRange(1, int.MaxValue)]
	public int PositionId { get; set; }

	[LocalizedRequired]
	[LocalizedRange(1, long.MaxValue)]
	public long EmployeeManageId { get; set; }

	[LocalizedRequired]
	[LocalizedRange(1, int.MaxValue)]
	public int DepartmentId { get; set; }
	public int? ClaimApplicationForCourtTypeId { get; set; }
	public string Message { get; set; }
	public List<ApplicationForCourtFileDlDto> Files { get; set; } = new();

	protected override Action<IMappingExpression<TDto, ApplicationForCourt>> AlterMapping =>
		cfg => cfg
			.ForMember(x => x.Files, opt => opt.Ignore());

	public override ApplicationForCourt CreateEntity()
	{
		var entity = base.CreateEntity();
		entity.StatusId = StatusIdConst.CREATED;
		entity.StepId = StepIdConst.APPLICATION_FOR_COURT_CREATE;
		entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_APPLICATION_FOR_COURT_FILE, Files.Select(a => a.Id).ToList());
		return entity;
	}

	public override void UpdateEntity(ApplicationForCourt entity)
	{
		base.UpdateEntity(entity);
		entity.StatusId = StatusIdConst.MODIFIED;
		entity.Files.UpdateFromFiles(
			DocumentStorageConst.DOC_APPLICATION_FOR_COURT_FILE,
			entity.Id.ToString(),
			Files.Select(a => a.Id).ToList());
	}
}

