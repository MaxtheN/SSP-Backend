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

namespace SspUis.BizLogicLayer.NationalityServices
{
    public class NationalityDto : UpdateNationalityDlDto, ILinkToEntity<Nationality>
    {
        public string State { get; set; }
        new public List<NationalityTranslateDto> Translates { get; set; } = new();
    }
}
