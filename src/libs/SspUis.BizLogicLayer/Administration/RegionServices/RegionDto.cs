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

namespace SspUis.BizLogicLayer.RegionServices
{
    public class RegionDto : UpdateRegionDlDto, ILinkToEntity<Region>
    {
        public string State { get; internal set; }
        public string Country { get; set; }

        new public List<RegionTranslateDto> Translates { get; set; } = new();
    }
}
