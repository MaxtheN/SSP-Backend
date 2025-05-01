using SspUis.Core;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.DualEdu.Doc.SubsidyRequestServices
{
	public class UpdateStatusSubsidyRequest : UpdateSubsidyRequestDlDto
	{
	}
}

public class SendStatusSubsidyRequestDto : UpdateStatusSubsidyRequestDlDto
{
	public SendStatusSubsidyRequestDto()
	{
		base.StatusId = StatusIdConst.SENT;
	}
	[LocalizedRequired]
	public string SignedData { get; set; }
	public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
}
public class ExecutedStatusSubsidyRequestDto : UpdateStatusSubsidyRequestDlDto
{
	public ExecutedStatusSubsidyRequestDto()
	{
		base.StatusId = StatusIdConst.EXECUTED;
	}
	new public int StatusId { get => base.StatusId; }
}
public class InExecutionStatusSubsidyRequestDto : UpdateStatusSubsidyRequestDlDto
{
	public InExecutionStatusSubsidyRequestDto()
	{
		base.StatusId = StatusIdConst.IN_EXECUTION;
	}
	new public int StatusId { get => base.StatusId; }
}
public class CancelStatusSubsidyRequestDto : UpdateStatusSubsidyRequestDlDto
{
	public CancelStatusSubsidyRequestDto()
	{
		base.StatusId = StatusIdConst.CANCELED;
	}
	new public int StatusId { get => base.StatusId; }
}
public class RejectStatusSubsidyRequestDto : UpdateStatusSubsidyRequestDlDto
{
	public RejectStatusSubsidyRequestDto()
	{
		base.StatusId = StatusIdConst.REJECTED;
	}
	new public int StatusId { get => base.StatusId; }
}
public class AcceptStatusSubsidyRequestDto : UpdateStatusSubsidyRequestDlDto
{
	public AcceptStatusSubsidyRequestDto()
	{
		base.StatusId = StatusIdConst.ACCEPTED;
	}

	new public int StatusId { get => base.StatusId; }
}

