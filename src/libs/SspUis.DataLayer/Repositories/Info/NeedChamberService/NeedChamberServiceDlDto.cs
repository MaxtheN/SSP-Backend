using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class NeedChamberServiceDlDto<TDto> : EntityDto<TDto,NeedChamberService>
        where TDto : NeedChamberServiceDlDto<TDto>
    {
        [LocalizedRequired]
        [LocalizedStringLength(9)]
        public string Code { get; set; } = null!;

        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string OrderCode { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; } = null!;

        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; } = null!;

        [LocalizedRequired]
        public bool CanPayDivided { get; set; }

        [LocalizedRequired]
        public bool IsOffer { get; set; }

        [LocalizedRequired]
        public int ServicePriceTypeId { get; set; }
        public string Details { get; set; } = string.Empty;
        public int? MeetingTypeId { get; set; }
        public long? EmployeeManageId { get; set; }
        public int? NeedChamberServiceGroupId { get; set; }
        public List<NeedChamberServiceTranslateDlDto> Translates { get; set; } = new();
        public List<NeedChamberServiceFileDlDto> Files { get; set; } = new();
        protected override Action<IMappingExpression<TDto, NeedChamberService>> AlterMapping => 
            cfg => cfg
                .ForMember(x => x.Translates, x => x.Ignore())
                .ForMember(x => x.Files, x => x.Ignore());

        public override NeedChamberService CreateEntity()
        {
            var entity = base.CreateEntity();

            entity.Files.AddFromTempFiles(
                DocumentStorageConst.INFO_NEED_CHAMBER_SERVICE_FILES,
                Files.Select(a => a.Id).ToList());

            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(NeedChamberService entity)
        {
            base.UpdateEntity(entity);

            entity.Files.UpdateFromFiles(
                DocumentStorageConst.INFO_NEED_CHAMBER_SERVICE_FILES,
                entity.Id.ToString(),
                Files.Select(a => a.Id).ToList());

            Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        }
    }
}
