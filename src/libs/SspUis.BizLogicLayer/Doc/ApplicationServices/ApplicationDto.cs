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
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.ApplicationServices
{
    public class ApplicationDto : UpdateApplicationDlDto, ILinkToEntity<Application>, IDocument
    {
        public Guid Id2 { get; set; }
        public int TableId { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public long ContractorId { get; set; }
        public string Contractor { get; set; }
        public string ContractorInn { get; set; }
        public string ApplicationType { get; set; }
        //public int RegionId { get; set; }
        //public int DistrictId { get; set; }
        //public string Region { get; set; }
        //public string District { get; set; }
    }
}
