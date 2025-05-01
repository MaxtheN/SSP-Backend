using AutoMapper;
using GenericServices;
using Newtonsoft.Json;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class ServiceApplicationTableDlDto
        : EntityDto<ServiceApplicationTableDlDto, ServiceApplicationTable>,
        IHaveIdProp<long>,
        ILinkToEntity<ServiceApplicationTable>
    {
        public long Id { get; set; }

        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int NeedChamberServiceId { get; set; }

        [JsonIgnore]
        public long? ServicePriceId { get; set; }

        [JsonIgnore]
        public long? ServicePriceTableId { get; set; }

        public string OfferServiceText { get; set; }

        public List<ServiceApplicationTableFileDlDto> Files { get; set; } = new();

        protected override Action<IMappingExpression<ServiceApplicationTableDlDto, ServiceApplicationTable>> AlterMapping =>
            cfg => cfg.ForMember(x => x.Files, opt => opt.Ignore());

        public override ServiceApplicationTable CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_SERVICE_APPLICATION_TABLE_FILES, Files.Select(a => a.Id).ToList());
            return entity;
        }

        public override void UpdateEntity(ServiceApplicationTable entity)
        {
            base.UpdateEntity(entity);
            entity.Files.UpdateFromFiles(
                DocumentStorageConst.DOC_SERVICE_APPLICATION_TABLE_FILES,
                entity.Id.ToString(),
                Files.Select(a => a.Id).ToList());
        }
    }
}
