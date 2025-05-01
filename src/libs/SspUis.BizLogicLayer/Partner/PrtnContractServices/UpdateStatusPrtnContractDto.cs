using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.EF;
using SkiaSharp;
using Microsoft.AspNetCore.Server.IIS.Core;
using AutoMapper;
using Newtonsoft.Json;

namespace SspUis.BizLogicLayer.Doc.PrtnContractServices
{
    public class SendStatusPrtnContractDto : SignStatusPrtnContractDto
    {
        public SendStatusPrtnContractDto()
        {
            base.StatusId = StatusIdConst.SENT;
        }

        public new int StatusId { get => base.StatusId; }
    }

    public class SignStatusPrtnContractDto : UpdateStatusPrtnContractDlDto
    {
        public SignStatusPrtnContractDto()
        {
            base.StatusId = StatusIdConst.SIGNING;
        }

        [LocalizedRequired]
        public string SignedData { get; set; }
        public bool IsPinfl { get; set; } = false;
        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
        internal long PrtnContractSignId { get; set; }
        internal Guid SignFile { get; set; }
        internal Guid DataFile { get; set; }
        internal string SignedUserInfo { get; set; }

        public override void UpdateEntity(PrtnContract entity)
        {
            base.UpdateEntity(entity);
            var sign = entity.Signs.FirstOrDefault(a => a.Id == PrtnContractSignId);
            if (sign != null)
            {
                sign.StatusId = StatusId;
                sign.SignFile = SignFile;
                sign.DataFile = DataFile;
                sign.SignedUserInfo = SignedUserInfo;
                sign.SignedAt = DateTime.Now;
                sign.IsSigned = true;
            }
            else throw new Exception("Imzolovchi topilmadi / Подписываемое лицо не было найдено");
        }
    }

    public class AgreeStatusPrtnContractDto : SignStatusPrtnContractDto
    {
        public AgreeStatusPrtnContractDto()
        {
            base.StatusId = StatusIdConst.AGREED;
        }
        public new int StatusId { get => base.StatusId; }

    }

    public class RevokeStatusPrtnContractDto : UpdateStatusPrtnContractDlDto
    {
        public RevokeStatusPrtnContractDto()
        {
            base.StatusId = StatusIdConst.REVOKED;
        }

        [LocalizedRequired]
        new public string Message { get; set; } = null!;
    }

    public class RejectStatusPrtnContractDto : UpdateStatusPrtnContractDlDto
    {
        public RejectStatusPrtnContractDto()
        {
            base.StatusId = StatusIdConst.REJECTED;
        }

        [LocalizedRequired]
        new public string Message { get; set; } = null!;
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int PrtnRejectReasonId { get; set; }
        //[LocalizedRequired]
        //public bool CancelApplication { get; set; }
    }

    public class PassExpertiseStatusPrtnContractDto : UpdateStatusPrtnContractDlDto
    {
        public PassExpertiseStatusPrtnContractDto()
        {
            base.StatusId = StatusIdConst.PASS_EXPERTISE;
        }
        public new int StatusId { get => base.StatusId; }

        [LocalizedRequired]
        new public string Message { get; set; } = null!;
        [LocalizedRequired]
        public List<PrtnContractFileDlDto> Files { get; set; } = new();

        protected override Action<IMappingExpression<UpdateStatusPrtnContractDlDto, PrtnContract>> AlterMapping => cfg => cfg
                .ForMember(x => x.Files, opt => opt.Ignore());

        public override void UpdateEntity(PrtnContract entity)
        {
            base.UpdateEntity(entity);
            entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_PRTN_CONTRACT_FILES, Files.Select(a => a.Id).ToList());
        }
    }

    public class ResendExpertiseStatusPrtnContractDto :
        UpdateStatusPrtnContractDlDto
    {
        public ResendExpertiseStatusPrtnContractDto()
        {
            base.StatusId = StatusIdConst.SENT_FOR_EXPERTISE;
        }
        public new int StatusId { get => base.StatusId; }

        [LocalizedRequired]
        new public string Message { get; set; } = null!;

        protected override Action<IMappingExpression<UpdateStatusPrtnContractDlDto, PrtnContract>> AlterMapping => cfg => cfg
                .ForMember(x => x.Files, opt => opt.Ignore());

        public override void UpdateEntity(PrtnContract entity)
        {
            base.UpdateEntity(entity);
        }
    }

    public class NotPassExpertiseStatusPrtnContractDto : UpdateStatusPrtnContractDlDto
    {
        public NotPassExpertiseStatusPrtnContractDto()
        {
            base.StatusId = StatusIdConst.NOT_PASS_EXPERTISE;
        }
        public new int StatusId { get => base.StatusId; }

        [LocalizedRequired]
        new public string Message { get; set; } = null!;
        [LocalizedRequired]
        public List<PrtnContractFileDlDto> Files { get; set; } = new();

        protected override Action<IMappingExpression<UpdateStatusPrtnContractDlDto, PrtnContract>> AlterMapping => cfg => cfg
                .ForMember(x => x.Files, opt => opt.Ignore());

        public override void UpdateEntity(PrtnContract entity)
        {
            base.UpdateEntity(entity);
            entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_PRTN_CONTRACT_FILES, Files.Select(a => a.Id).ToList());
 
            //entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_PRTN_CONTRACT_FILES, entity.Id.ToString(), Files.Select(a => a.Id).ToList());
        }
    }

    public class CancelStatusPrtnContractDto : UpdateStatusPrtnContractDlDto
    {
        public CancelStatusPrtnContractDto()
        {
            base.StatusId = StatusIdConst.CANCELED;
        }

        [LocalizedRequired]
        new public string Message { get; set; } = null!;
        [LocalizedRequired]
        public bool CancelApplication { get; set; }
    }
}
