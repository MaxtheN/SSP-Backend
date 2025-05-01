using System;
using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer
{
    public class MemshipCertificateProlongDto
    {
       public long Id { get; set; }
       public DateOnly NewExpireOn { get; set; }
       public string Details { get; set; }
        //public string NewDocNumber { get; set; }
    }
}
