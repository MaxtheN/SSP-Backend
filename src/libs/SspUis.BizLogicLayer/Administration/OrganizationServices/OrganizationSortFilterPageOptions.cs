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
using SspUis.BizLogicLayer.Models;

namespace SspUis.BizLogicLayer.OrganizationServices
{
    public class OrganizationSortFilterPageOptions : WEBASE.TableSortFilterPageOptions
    {
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
        public int? SignOrganizationTypeId { get; set; }
        public int? OrganizationalStructureId { get; set; }
        public int? OrganizationGroupId { get; set; }
    }
}
