using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class UpdatingBirdMemshipCertificateDlDto : EntityDto<UpdatingBirdMemshipCertificateDlDto, MemshipCertificate>
{
    public long Id { get; set; }
    public string Detail { get; set; }
}
