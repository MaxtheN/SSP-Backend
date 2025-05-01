using GenericServices;
using SspUis.DataLayer.EfClasses.Claim;
using System;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ApplicationForCourtFileDlDto :
        EntityDto<ApplicationForCourtFileDlDto, 
            ApplicationForCourtFile>, 
        ILinkToEntity<ApplicationForCourtFile>
    {
        [LocalizedRequired]
        public Guid Id { get; set; }
    }
}
