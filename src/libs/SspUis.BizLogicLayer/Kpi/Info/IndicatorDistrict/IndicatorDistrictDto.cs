using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer
{
    public class IndicatorDistrictDto : UpdateIndicatorDistrictDlDto, ILinkToEntity<IndicatorDistrict>
    {
        public string State { get; set; } = null;
        public string Department { get; set; }
        public List<IndicatorDistrictTableDto> Tables { get; set; } = new();
        public List<IndicatorDistrictTranslateDto> Translates { get; set; } = new();
    }
}
