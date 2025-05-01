using System;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class CallCenterAppealFileDlDto
        : EntityDto<CallCenterAppealFileDlDto, CallCenterAppealFile>,
        IHaveIdProp<Guid>
    {
        [LocalizedRequired]
        public Guid Id { get; set; }
    }
}
