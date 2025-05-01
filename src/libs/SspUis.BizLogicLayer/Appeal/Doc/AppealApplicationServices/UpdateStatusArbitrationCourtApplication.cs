using SspUis.Core;
using SspUis.DataLayer.Repositories.Appeal;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.Appeal;

public class UpdateStatusAppealApplication : UpdateStatusAppealApplicationDlDto
{
}

public class AcceptStatusAppealApplicationDto : UpdateStatusAppealApplicationDlDto
{
    public AcceptStatusAppealApplicationDto()
    {
        base.StatusId = StatusIdConst.ACCEPTED;
    }

    new public int StatusId { get => base.StatusId; }
}

public class RejectStatusAppealApplicationDto : UpdateStatusAppealApplicationDlDto
{
    public RejectStatusAppealApplicationDto()
    {
        base.StatusId = StatusIdConst.REJECTED;
    }
    new public int StatusId { get => base.StatusId; }
}

public class CancelStatusAppealApplicationDto : UpdateStatusAppealApplicationDlDto
{
    public CancelStatusAppealApplicationDto()
    {
        base.StatusId = StatusIdConst.CANCELED;
    }
    new public int StatusId { get => base.StatusId; }
}

public class InExecutionStatusAppealApplicationDto : UpdateStatusAppealApplicationDlDto
{
    public InExecutionStatusAppealApplicationDto()
    {
        base.StatusId = StatusIdConst.IN_EXECUTION;
    }
    new public int StatusId { get => base.StatusId; }
}

public class ExecutedStatusAppealApplicationDto : UpdateStatusAppealApplicationDlDto
{
    public ExecutedStatusAppealApplicationDto()
    {
        base.StatusId = StatusIdConst.EXECUTED;
    }
    new public int StatusId { get => base.StatusId; }
}
public class HasEdocResponceStatusAppealApplicationDto : UpdateStatusAppealApplicationDlDto
{
    public HasEdocResponceStatusAppealApplicationDto()
    {
        base.StatusId = StatusIdConst.HAS_EDOC_RESPONSE;
    }
    new public int StatusId { get => base.StatusId; }
}

public class SignStatusAppealApplicationDto : UpdateStatusAppealApplicationDlDto
{
    public SignStatusAppealApplicationDto()
    {
        base.StatusId = StatusIdConst.SENT;
    }
    [LocalizedRequired]
    public string SignedData { get; set; }
    public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }

}
