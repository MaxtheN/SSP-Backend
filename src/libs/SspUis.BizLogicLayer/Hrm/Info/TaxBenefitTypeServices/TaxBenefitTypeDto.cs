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

namespace SspUis.BizLogicLayer.Hrm.TaxBenefitTypeServices
{
    public class TaxBenefitTypeDto : UpdateTaxBenefitTypeDlDto, ILinkToEntity<TaxBenefitType>
    {
        public string State { get; internal set; }
        public string MinimumValueType { get; set; }
        new public List<TaxBenefitTypeTranslateDto> Translates { get; set; } = new();
    }
}
