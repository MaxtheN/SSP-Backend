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

namespace SspUis.BizLogicLayer.PrtnContractTypeServices
{
    public class PrtnContractTypeTableDto : PrtnContractTypeTableDlDto, ILinkToEntity<PrtnContractTypeTable>
    {
        public string SignOrganizationType { get; set; }
        public string Position { get; set; }
        public string State { get; set; }
        public string Organization { get; set; }
        public string Region { get; set; }
    }
}
