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

namespace SspUis.BizLogicLayer.CurrencyServices
{
    public class CurrencyDto : UpdateCurrencyDlDto, ILinkToEntity<Currency>
    {
        public string State { get; set; }
        new public List<CurrencyTranslateDto> Translates { get; set; } = new();
    }
}
