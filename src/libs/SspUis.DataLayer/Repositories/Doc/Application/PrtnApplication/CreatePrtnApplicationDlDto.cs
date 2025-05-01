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
    public class CreatePrtnApplicationDlDto : CreateApplicationDlDto, IPrtnApplicationDlDto
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

        public new int ApplicationTypeId { get => base.ApplicationTypeId; internal set => base.ApplicationTypeId = value; }

        public override Application CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.ApplicationTypeId = ApplicationTypeIdConst.PARTNER;
            entity.PrtnApplication = new PrtnApplication
            {
                PrtnContractTypeId = PrtnContractTypeId,
                NewVacanciesCount = NewVacanciesCount,
                MfyId = MfyId,
                ChooseLocation = ChooseLocation,
                ChoosedRegionId = ChoosedRegionId,
                ChoosedDistrictId = ChoosedDistrictId
            };
            Graphs.AddTo(entity.PrtnApplication.Graphs);
            entity.StatusId = StatusIdConst.CREATED;
            entity.PrtnApplication.IsSent = false;
            return entity;
        }
    }
}
