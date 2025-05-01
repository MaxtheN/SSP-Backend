using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class OrganizationFileDlDto : EntityDto<OrganizationFileDlDto, OrganizationFile>,
        IHaveIdProp<Guid>,
        ILinkToEntity<OrganizationFile>
    {
        [LocalizedRequired]
        public Guid Id { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(128)]
        public string ColumnName { get; set; }
    }
}
