

using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Memship;
using SspUis.DataLayer.Repositories.Memship;
using System;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class MemshipNewContractorFileDlDto : EntityDto<MemshipNewContractorFileDlDto, MemshipNewContractorsFile>, IHaveIdProp<Guid>, ILinkToEntity<MemshipNewContractorsFile>
{
    [LocalizedRequired]
    public Guid Id { get; set; }

}
