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

namespace SspUis.BizLogicLayer.RelativeDegreeServices
{
    public class RelativeDegreeDto : UpdateRelativeDegreeDlDto, ILinkToEntity<RelativeDegree>
    {
        public string State { get; internal set; }
        public string Gender { get; internal set; }
        new public List<RelativeDegreeTranslateDto> Translates { get; set; } = new();
    }
}
