using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class PrtnContractFileDlDto : EntityDto<PrtnContractFileDlDto, PrtnContractFile>, IHaveIdProp<Guid>, ILinkToEntity<PrtnContractFile>
    {
        [LocalizedRequired]
        public Guid Id { get; set; }
       
    }
}
