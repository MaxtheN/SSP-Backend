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

namespace SspUis.BizLogicLayer;

public class EducationItemDto : UpdateEducationItemDlDto, ILinkToEntity<EducationItem>
{
    public string State { get; internal set; }
    new public List<EducationItemTranslateDto> Translates { get; set; } = new();
}
