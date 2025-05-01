using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.Models;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.PrtnContractTypeServices
{
    public class TablesBySignOrganizationTypeDtoFilter
    {
        public int SignOrganizationTypeId { get; set; }
        public bool? IncludeBusinessman {get; set; }
        public int? OrganizationId { get; set; }
        public int? RegionId { get; set; }
    }
}
