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
using SspUis.DataLayer.EfClasses.DualEdu;

namespace SspUis.BizLogicLayer.Hrm.SpecialtyServices
{
    public class SpecialtyDto : UpdateSpecialtyDlDto, ILinkToEntity<Specialty>
    {
        public string State { get; internal set; }
        new public List<SpecialtyTranslateDto> Translates { get; set; } = new();
    }
}
