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
using Newtonsoft.Json;

namespace SspUis.BizLogicLayer.Doc.PrtnContractServices
{

    public class RejectStatusPrtnApplicationDto : UpdateStatusApplicationDlDto
    {
        public RejectStatusPrtnApplicationDto()
        {
            base.StatusId = StatusIdConst.REJECTED;
        }

        [LocalizedRequired]
        public new string Message { get => base.Message; set => base.Message = value; }

        new public int StatusId { get => base.StatusId; }
        public string Offer { get; set; }


        public bool IsRead { get; set; }

        public override void UpdateEntity(Application entity)
        {
            base.UpdateEntity(entity);
            entity.PrtnApplication.Offer = Offer;
        }
    }

    public class CancelStatusPrtnApplicationDto : UpdateStatusApplicationDlDto
    {
        public CancelStatusPrtnApplicationDto()
        {
            base.StatusId = StatusIdConst.CANCELED;
        }
        new public int StatusId { get => base.StatusId; }
    }

    public class AcceptStatusPrtnApplicationDto : UpdateStatusApplicationDlDto
    {
        public AcceptStatusPrtnApplicationDto()
        {
            base.StatusId = StatusIdConst.ACCEPTED;
        }

        [LocalizedRequired]
        public string Parameters { get; set; } = null!;

        [LocalizedRequired]
        public string Offer { get; set; } = null!;

        new public int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
        public override void UpdateEntity(Application entity)
        {
            base.UpdateEntity(entity);
            entity.PrtnApplication.Parameters = Parameters;
            entity.PrtnApplication.Offer = Offer;
        }
    }
}
