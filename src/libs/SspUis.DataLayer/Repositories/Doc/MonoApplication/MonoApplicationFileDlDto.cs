using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class MonoApplicationFileDlDto : EntityDto<MonoApplicationFileDlDto, MonoApplicationFile>,
        IHaveIdProp<Guid>,
        ILinkToEntity<MonoApplicationFile>
    {
        [LocalizedRequired]
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string ColumnName { get; set; }

    }
}
