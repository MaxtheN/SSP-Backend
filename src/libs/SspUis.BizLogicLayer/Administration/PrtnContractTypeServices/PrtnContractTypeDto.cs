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

namespace SspUis.BizLogicLayer.PrtnContractTypeServices
{
    public class PrtnContractTypeDto : UpdatePrtnContractTypeDlDto, ILinkToEntity<PrtnContractType>
    {
        public string State { get; internal set; }
        new public List<PrtnContractTypeTableDto> Tables { get; set; } = new();
        new public List<PrtnContractTypeTranslateDto> Translates { get; set; } = new();
    }
}
