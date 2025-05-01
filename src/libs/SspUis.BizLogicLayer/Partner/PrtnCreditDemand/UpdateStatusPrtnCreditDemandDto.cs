using System;
using SspUis.Core;
using SspUis.DataLayer.Repositories;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.Doc.PrtnContractServices
{
    public class CancelStatusPrtnCreditDemandDto : UpdateStatusPrtnCreditDemandDlDto
    {
        public CancelStatusPrtnCreditDemandDto()
        {
            base.StatusId = StatusIdConst.CANCELED;
        }
        public new int StatusId { get => base.StatusId; }
    }
    public class SignStatusPrtnCreditDemandDto : UpdateStatusPrtnCreditDemandDlDto
    {
        public SignStatusPrtnCreditDemandDto()
        {
            base.StatusId = StatusIdConst.SIGNING;
        }

        [LocalizedRequired]
        public string SignedData { get; set; }

        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
        internal long PrtnCreditDemandSignId { get; set; }
        internal Guid SignFile { get; set; }
        internal Guid DataFile { get; set; }
        internal string SignedUserInfo { get; set; }

        //public override void UpdateEntity(PrtnCreditDemand entity)
        //{
        //    base.UpdateEntity(entity);
        //    var sign = entity.FirstSign == PrtnCreditDemandSignId;
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
}
