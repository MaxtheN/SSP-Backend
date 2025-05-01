using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.BusinessActivityTypeServices
{
    public class BusinessActivityTypeDto : UpdateBusinessActivityTypeDlDto, ILinkToEntity<BusinessActivityType>
    {
        public string Contractor { get; set; }
        public string ContractorInn { get; set; }
        public string Bank { get; set; }
        public string BankCode { get; set; }
        public string BusinessCtorName { get; set; }
    }
}
