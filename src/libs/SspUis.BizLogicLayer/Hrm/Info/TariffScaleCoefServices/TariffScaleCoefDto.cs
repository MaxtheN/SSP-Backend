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

namespace SspUis.BizLogicLayer.Hrm.TariffScaleCoefServices;

public class TariffScaleCoefDto : UpdateTariffScaleCoefDlDto, ILinkToEntity<TariffScaleCoef>
{
    public string State { get; internal set; }
    public string TariffScale { get; internal set; }
    new public List<TariffScaleCoefTranslateDto> Translates { get; set; } = new();
    new public List<TariffScaleCoefTableDto> Tables { get; set; } = new();
}
