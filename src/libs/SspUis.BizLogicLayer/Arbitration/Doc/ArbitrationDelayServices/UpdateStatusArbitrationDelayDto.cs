using SspUis.Core;
using SspUis.DataLayer.Repositories;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer;

public class UpdateStatusArbitrationDelayDto : UpdateStatusArbitrationDelayDlDto
{
    public new int StatusId { get => base.StatusId; set => base.StatusId = value; }
}

public class SignStatusArbitrationDelayDto : UpdateStatusArbitrationDelayDto
{
    public SignStatusArbitrationDelayDto()
    {
        base.StatusId = StatusIdConst.SIGNED;
    }
    [LocalizedRequired]
    public string SignedData { get; set; }

    public bool IsWithoutDelay { get; set; }
}

public class DeleteStatusArbitrationDelayDto : UpdateStatusArbitrationDelayDto
{
    public DeleteStatusArbitrationDelayDto()
    {
        base.StatusId = StatusIdConst.DELETED;
    }

}



