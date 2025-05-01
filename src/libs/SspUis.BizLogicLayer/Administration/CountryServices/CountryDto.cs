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

namespace SspUis.BizLogicLayer.CountryServices
{
    public class CountryDto : UpdateCountryDlDto, ILinkToEntity<Country>
    {
        public string State { get; internal set; }
        new public List<CountryTranslateDto> Translates { get; set; } = new();
    }
}
