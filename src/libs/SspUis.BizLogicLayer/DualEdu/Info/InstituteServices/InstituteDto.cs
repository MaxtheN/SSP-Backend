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

namespace SspUis.BizLogicLayer.Hrm.InstituteServices
{
    public class InstituteDto : UpdateInstituteDlDto, ILinkToEntity<Institute>
    {
        public string State { get; internal set; }
        public string Region { get; set; }
        public string District { get; set; }
        new public List<InstituteTranslateDto> Translates { get; set; } = new();
    }
}
