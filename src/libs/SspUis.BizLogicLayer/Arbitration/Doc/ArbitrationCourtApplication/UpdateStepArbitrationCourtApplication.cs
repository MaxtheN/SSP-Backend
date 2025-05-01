using SspUis.Core;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.ArbitrationCourtApplicationServices;

public class UpdateStepArbitrationCourtApplication : UpdateStepArbitrationCourtApplicationDlDto
{
    internal new int CurrentStepId { get => base.CurrentStepId; set => base.CurrentStepId = value; }

}

public class NotifiedStepArbitrationCourtApplication : UpdateStepArbitrationCourtApplication
{
    public NotifiedStepArbitrationCourtApplication()
    {
        base.CurrentStepId = StepIdConst.NOTIFIED;
    }
    //public new int CurrentStepId { get => base.CurrentStepId; internal set => base.CurrentStepId = value; }
}

public class DiscussionStepArbitrationCourtApplication : UpdateStepArbitrationCourtApplication
{
    public DiscussionStepArbitrationCourtApplication()
    {
        base.CurrentStepId = StepIdConst.NEED_DISCUSSION;
    }
    //internal new int CurrentStepId { get => base.CurrentStepId; set => base.CurrentStepId = value; }
}

public class DelayedStepArbitrationCourtApplication : UpdateStepArbitrationCourtApplication
{
    public DelayedStepArbitrationCourtApplication()
    {
        base.CurrentStepId = StepIdConst.DELAYED;
    }
    public bool ApplyFilter { get; set; }
    //public new int CurrentStepId { get => base.CurrentStepId; internal set => base.CurrentStepId = value; }
}

public class CourtDecisionStepArbitrationCourtApplication : UpdateStepArbitrationCourtApplication
{
    public CourtDecisionStepArbitrationCourtApplication()
    {
        base.CurrentStepId = StepIdConst.COURT_DECISION;
    }
    public bool ApplyFilter { get; set; }
    //public new int CurrentStepId { get => base.CurrentStepId; internal set => base.CurrentStepId = value; }
}
