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
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class ScientificDegreeDto : UpdateScientificDegreeDlDto, ILinkToEntity<ScientificDegree>
{
    public string State { get; internal set; }
    new public List<ScientificDegreeTranslateDto> Translates { get; set; } = new();
}
