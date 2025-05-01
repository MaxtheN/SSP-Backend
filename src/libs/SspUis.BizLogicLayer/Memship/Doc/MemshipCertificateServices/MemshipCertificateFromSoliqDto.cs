using System;
using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer
{
    public class MemshipCertificateFromSoliqDto
    {
        public decimal? Amount { get; set; }
        public string Contractor { get; set; }
        public string ContractorType { get; set; }
    }
}
