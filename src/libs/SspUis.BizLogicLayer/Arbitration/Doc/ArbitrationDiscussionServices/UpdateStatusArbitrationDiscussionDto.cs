using SspUis.Core;
using SspUis.DataLayer.Repositories;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer;

public class UpdateStatusArbitrationDiscussionDto : UpdateStatusArbitrationDiscussionDlDto
{
    public new int StatusId { get => base.StatusId; set => base.StatusId = value; }
}

public class SignStatusArbitrationDiscussionDto : UpdateStatusArbitrationDiscussionDto
{
    public SignStatusArbitrationDiscussionDto()
    {
        base.StatusId = StatusIdConst.SIGNED;
    }
    [LocalizedRequired]
    public string SignedData { get; set; }

    public bool IsWithoutDelay { get; set; }
}

public class DeleteStatusArbitrationDiscussionDto : UpdateStatusArbitrationDiscussionDto
{
    public DeleteStatusArbitrationDiscussionDto()
    {
        base.StatusId = StatusIdConst.DELETED;
    }

}



