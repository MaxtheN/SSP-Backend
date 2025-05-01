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
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.DistrictServices
{
    public class DistrictDto : UpdateDistrictDlDto, ILinkToEntity<District>
    {
        public string State { get; internal set; }
        public string Region { get; set; }

        new public List<DistrictTranslateDto> Translates { get; set; } = new();
    }
}
