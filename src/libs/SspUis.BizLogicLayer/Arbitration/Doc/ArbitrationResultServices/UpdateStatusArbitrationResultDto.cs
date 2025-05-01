using SspUis.Core;
using SspUis.DataLayer.Repositories;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer;

public class UpdateStatusArbitrationResultDto : UpdateStatusArbitrationResultDlDto
{
    public new int StatusId { get => base.StatusId; set => base.StatusId = value; }
}

public class SignStatusArbitrationResultDto : UpdateStatusArbitrationResultDto
{
    public SignStatusArbitrationResultDto()
    {
        base.StatusId = StatusIdConst.SIGNING;
    }
    [LocalizedRequired]
    public string SignedData { get; set; }
}
public class ExecutedArbitrationResultDto : UpdateStatusArbitrationResultDto
{
	public ExecutedArbitrationResultDto()
	{
		base.StatusId = StatusIdConst.EXECUTED;
	}
}
public class InExecutionStatusArbitrationResultDto : UpdateStatusArbitrationResultDto
{
	public InExecutionStatusArbitrationResultDto()
	{
		base.StatusId = StatusIdConst.IN_EXECUTION;
	}
}
public class CancelStatusArbitrationResultDto : UpdateStatusArbitrationResultDto
{
	public CancelStatusArbitrationResultDto()
	{
		base.StatusId = StatusIdConst.CANCELED;
	}
	
}
public class RejectStatusArbitrationResultDto : UpdateStatusArbitrationResultDto
{
	public RejectStatusArbitrationResultDto()
	{
		base.StatusId = StatusIdConst.REJECTED;
	}
}



