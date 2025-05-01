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

namespace SspUis.BizLogicLayer.PositionServices
{
    public class PositionDto : UpdatePositionDlDto, ILinkToEntity<Position>
    {
        public string PositionClassification { get; internal set; }
        public string PositionCategory { get; internal set; }
        public string TariffScaleType { get; internal set; }
        public string StaffTypeBasicTariff { get; internal set; }
        public string State { get; internal set; }
        new public List<PositionTranslateDto> Translates { get; set; } = new();
    }
}
