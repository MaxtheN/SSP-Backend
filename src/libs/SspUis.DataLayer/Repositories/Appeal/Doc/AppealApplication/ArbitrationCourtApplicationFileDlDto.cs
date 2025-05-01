using System;
using SspUis.DataLayer.EfClasses.Appeal;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class AppealApplicationFileDlDto
        : EntityDto<AppealApplicationFileDlDto, AppealApplicationFile>,
        IHaveIdProp<Guid>
    {
        [LocalizedRequired]
        public Guid Id { get; set; }
    }
}
