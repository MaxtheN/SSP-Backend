using System;
using SspUis.Core;
using SspUis.DataLayer.Repositories;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer;

public class UpdateStatusAdditionalAgreementDto : UpdateStatusAdditionalAgreementDlDto
{
    internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
}


public class SignStatusAdditionalAgreementDto : UpdateStatusAdditionalAgreementDto
{
    [LocalizedRequired]
    public string SignedData { get; set; }
    public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
    internal Guid SignFile { get; set; }
    internal Guid DataFile { get; set; }
    internal string SignedUserInfo { get; set; }
    public bool IsPinfl { get; set; } = false;
}

public class RejectStatusAdditionalAgreementDto : SignStatusAdditionalAgreementDto
{
    public RejectStatusAdditionalAgreementDto()
    {
        StatusId = StatusIdConst.REJECTED;
    }
}
