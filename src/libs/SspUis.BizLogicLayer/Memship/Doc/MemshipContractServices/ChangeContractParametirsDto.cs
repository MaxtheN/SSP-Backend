using System;
using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Memship
{
    public class ChangeContractParametirsDto
    {
        public long Id { get; set; }
        public DateOnly DocOn { get; set; }
        public string DocNumber { get; set; }
        public string Details { get; set; }
    }
}
