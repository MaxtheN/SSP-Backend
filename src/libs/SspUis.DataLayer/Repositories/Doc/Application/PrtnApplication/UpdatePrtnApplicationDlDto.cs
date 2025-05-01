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
    public class UpdatePrtnApplicationDlDto : UpdateApplicationDlDto, IPrtnApplicationDlDto
    {
        [LocalizedRequired]
        public bool ChooseLocation { get; set; }
        public int? ChoosedRegionId { get; set; }
        public int? ChoosedDistrictId { get; set; }
        [LocalizedRange(1, long.MaxValue)]
        public long MfyId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int PrtnContractTypeId { get; set; }
        [LocalizedRequired]
        public int NewVacanciesCount { get; set; }
        public List<PrtnApplicationGraphDlDto> Graphs { get; set; }

        public string ChoosedDistrict { get; set; }
        public new int ApplicationTypeId { get => base.ApplicationTypeId; internal set => base.ApplicationTypeId = value; }

        public override void UpdateEntity(Application entity)
        {
            base.UpdateEntity(entity);
            entity.ApplicationTypeId = ApplicationTypeIdConst.PARTNER;
            
            entity.PrtnApplication.PrtnContractTypeId = PrtnContractTypeId;
            entity.PrtnApplication.NewVacanciesCount = NewVacanciesCount;
            entity.PrtnApplication.MfyId = MfyId;
            entity.PrtnApplication.ChooseLocation = ChooseLocation;
            entity.PrtnApplication.ChoosedRegionId = ChoosedRegionId;
            entity.PrtnApplication.ChoosedDistrictId = ChoosedDistrictId;

            entity.StatusId = StatusIdConst.MODIFIED;
            entity.PrtnApplication.IsSent = false;
            Graphs.ApplyChangesTo<long, PrtnApplicationGraphDlDto, PrtnApplicationGraph>(entity.PrtnApplication.Graphs);

        }
    }

}
