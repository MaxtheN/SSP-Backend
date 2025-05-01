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

namespace SspUis.DataLayer.Repositories
{
    public class UpdateStateAssetApplicationDlDto : UpdateApplicationDlDto
    {
        public UpdateStateAssetApplicationDlDto()
        {
            base.ApplicationTypeId = ApplicationTypeIdConst.STATE_ASSET;
        }
        public long PrtnCertificateId { get; set; }
        public DateOnly? AuctionDocOn { get; set; }
        [LocalizedStringLength(50)]
        public string AuctionDocNumber { get; set; }
        [LocalizedStringLength(1024)]
        public string StateAssetName { get; set; }
        public new int ApplicationTypeId { get => base.ApplicationTypeId; }
        public int RegionId { get; set; }
        public int DistrictId { get; set; }

        public override void UpdateEntity(Application entity)
        {
            base.UpdateEntity(entity);
            entity.StateAssetApplication.PrtnCertificateId = PrtnCertificateId;
            entity.StateAssetApplication.AuctionDocOn = AuctionDocOn;
            entity.StateAssetApplication.AuctionDocNumber = AuctionDocNumber;
            entity.StateAssetApplication.StateAssetName = StateAssetName;
            entity.StatusId = StatusIdConst.MODIFIED;
            entity.RegionId = RegionId;
            entity.DistrictId = DistrictId;
        }
    }

}
