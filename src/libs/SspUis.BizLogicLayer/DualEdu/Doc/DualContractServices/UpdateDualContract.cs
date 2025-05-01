using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Linq;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.DualContractServices;

public class UpdateDualContract : UpdateStatusDualContractDlDto
{
    public UpdateDualContract()
    {
        base.StatusId = StatusIdConst.SIGNED;
    }
    [LocalizedRequired]
    public string SignedData { get; set; }
    internal Guid SignFile { get; set; }
    internal Guid DataFile { get; set; }
    internal string SignedUserInfo { get; set; }
    //internal long DualContractSignId { get; set; }
    //public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
    public bool IsPinfl { get; set; } = false;
    //public override void UpdateEntity(DualContract entity)
    //{
    //    base.UpdateEntity(entity);
    //    var sign = entity.Signs.FirstOrDefault(x => x.Id == DualContractSignId);
    //    if (sign != null) 
    //    {
    //        sign.StatusId = StatusId;
    //        sign.SignFile = SignFile;
    //        sign.DataFile = DataFile;
    //        sign.SignedUserInfo = SignedUserInfo;
    //        sign.SignedAt = DateTime.Now;
    //        sign.IsSigned = true;
    //    }
    //    else throw new Exception("Imzolovchi topilmadi / Подписываемое лицо не было найдено");
    //}
}

public class RejectStatusDualContractDto : UpdateStatusDualContractDlDto
{
    public RejectStatusDualContractDto()
    {
        base.StatusId = StatusIdConst.REJECTED;
    }
    [LocalizedRequired]
    public string SignedData { get; set; }
    internal Guid SignFile { get; set; }
    internal Guid DataFile { get; set; }
    internal string SignedUserInfo { get; set; }
    public bool IsPinfl { get; set; } = false;
}