using SspUis.Core;
using SspUis.DataLayer.Repositories;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer;

public class UpdateStatusCallCenterAppeal : UpdateStatusCallCenterAppealDlDto
{
}

public class AcceptStatusCallCenterAppealDto : UpdateStatusCallCenterAppealDlDto
{
    public AcceptStatusCallCenterAppealDto()
    {
        base.StatusId = StatusIdConst.ACCEPTED;
    }

    new public int StatusId { get => base.StatusId; }
} 

public class RejectStatusCallCenterAppealDto : UpdateStatusCallCenterAppealDlDto
{
    public RejectStatusCallCenterAppealDto()
    {
        base.StatusId = StatusIdConst.REJECTED;
    }
    new public int StatusId { get => base.StatusId; }
}

public class CancelStatusCallCenterAppealDto : UpdateStatusCallCenterAppealDlDto
{
    public CancelStatusCallCenterAppealDto()
    {
        base.StatusId = StatusIdConst.CANCELED;
    }
    new public int StatusId { get => base.StatusId; }
}

public class InExecutionStatusCallCenterAppealDto : UpdateStatusCallCenterAppealDlDto
{
    public InExecutionStatusCallCenterAppealDto()
    {
        base.StatusId = StatusIdConst.IN_EXECUTION;
    }
    new public int StatusId { get => base.StatusId; }
}

public class ExecutedStatusCallCenterAppealDto : UpdateStatusCallCenterAppealDlDto
{
    public ExecutedStatusCallCenterAppealDto()
    {
        base.StatusId = StatusIdConst.EXECUTED;
    }
    new public int StatusId { get => base.StatusId; }
}

public class HasEdocResponceStatusCallCenterAppealDto : UpdateStatusCallCenterAppealDlDto
{
    public HasEdocResponceStatusCallCenterAppealDto()
    {
        base.StatusId = StatusIdConst.HAS_EDOC_RESPONSE;
    }
    new public int StatusId { get => base.StatusId; }
}

public class SignStatusCallCenterAppealDto : UpdateStatusCallCenterAppealDlDto
{
    public SignStatusCallCenterAppealDto()
    {
        base.StatusId = StatusIdConst.SENT;
    }
    [LocalizedRequired]
    public string SignedData { get; set; }
    public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }

}
