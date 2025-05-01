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

public class PositionTypeDto : UpdatePositionTypeDlDto, ILinkToEntity<PositionType>
{
    public string State { get; internal set; }
    public string MinimumValueType { get; set; }
    new public List<PositionTypeTranslateDto> Translates { get; set; } = new();
}
