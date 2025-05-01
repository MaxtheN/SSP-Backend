using System;
using GenericServices;
using SspUis.DataLayer.EfClasses.DualEdu;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class SubsidyRequestFileDlDto : EntityDto<SubsidyRequestFileDlDto, SubsidyRequestFile>,
        IHaveIdProp<Guid>,
        ILinkToEntity<SubsidyRequestFile>
    {
        [LocalizedRequired]
        public Guid Id { get; set; }

        public long? SubsidyRequestTableId { get; set; }

    }
}
