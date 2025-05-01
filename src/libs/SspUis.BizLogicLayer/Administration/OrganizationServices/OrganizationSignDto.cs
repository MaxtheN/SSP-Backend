using AutoMapper;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using GenericServices;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.OrganizationServices
{
    public class OrganizationSignDto : OrganizationSignDlDto, ILinkToEntity<OrganizationSign>
    {
        public string PrtnContractTypeTablePosition { get; set; }
        public string PhoneNumber { get; set; }
    }
}
