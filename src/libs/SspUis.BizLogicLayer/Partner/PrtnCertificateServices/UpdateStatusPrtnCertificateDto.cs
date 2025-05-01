using System;
using SspUis.Core;
using SspUis.DataLayer.Repositories;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.Doc.PrtnContractServices
{
    public class CancelStatusPrtnCertificateDto : SignStatusPrtnCertificateDto
    {
        public CancelStatusPrtnCertificateDto()
        {
            base.StatusId = StatusIdConst.CANCELED;
        }
        public new int StatusId { get => base.StatusId; }
        [LocalizedRequired]
        public bool CancelApplication { get; set; } 
        [LocalizedRequired]
        public bool CancelContract { get; set; }
    }
    public class SignStatusPrtnCertificateDto : UpdateStatusPrtnCertificateDlDto
    {
        public SignStatusPrtnCertificateDto()
        {
            base.StatusId = StatusIdConst.SIGNING;
        }

        [LocalizedRequired]
        public string SignedData { get; set; }
        public bool IsPinfl { get; set; } = false;
        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
        internal long PrtnCertificateSignId { get; set; }
        internal Guid SignFile { get; set; }
        internal Guid DataFile { get; set; }
        internal string SignedUserInfo { get; set; }

        //public override void UpdateEntity(PrtnCertificate entity)
        //{
        //    base.UpdateEntity(entity);
        //    var sign = entity.FirstSign == PrtnCertificateSignId;
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
