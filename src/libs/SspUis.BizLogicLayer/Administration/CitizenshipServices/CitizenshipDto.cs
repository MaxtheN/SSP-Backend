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

namespace SspUis.BizLogicLayer.CitizenshipServices
{
    public class CitizenshipDto : UpdateCitizenshipDlDto, ILinkToEntity<Citizenship>
    {
        public string State { get; internal set; }
        new public List<CitizenshipTranslateDto> Translates { get; set; } = new();
    }
}
