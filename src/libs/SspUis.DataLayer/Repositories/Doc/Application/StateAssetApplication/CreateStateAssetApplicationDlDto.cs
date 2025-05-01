using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.Models;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.EfClasses;
using SspUis.Core;
using AutoMapper;
using WEBASE.EF;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SspUis.DataLayer.Repositories
{
    public class CreateStateAssetApplicationDlDto : CreateApplicationDlDto
    {
        public CreateStateAssetApplicationDlDto()
        {
            base.ApplicationTypeId = ApplicationTypeIdConst.STATE_ASSET;
        }
        public long PrtnCertificateId { get; set; }
        [LocalizedRequired]
        public DateOnly? AuctionDocOn { get; set; }
        [LocalizedStringLength(50)]
        [LocalizedRequired]
        public string AuctionDocNumber { get; set; }
        [LocalizedStringLength(1024)]
        [LocalizedRequired]
        public string StateAssetName { get; set; }
        public new int ApplicationTypeId { get => base.ApplicationTypeId; }

        public override Application CreateEntity()
        {
            var entity = base.CreateEntity();

            entity.StateAssetApplication = new StateAssetApplication
            {
                PrtnCertificateId = PrtnCertificateId,
                AuctionDocOn = AuctionDocOn,
                AuctionDocNumber = AuctionDocNumber,
                StateAssetName = StateAssetName,
            };
            entity.StatusId = StatusIdConst.SENDING;
            return entity;
        }
    }
}
