using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;
using System.ComponentModel.DataAnnotations.Schema;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.TariffScaleServices;

public class TariffScaleListDto : ILinkToEntity<TariffScale>, IHaveIdProp<int>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string ShortName { get; set; }
    public string FullName { get; set; }
    public int StateId { get; set; }
    public string MinimumValueType { get; set; }
    public string State { get; set; }
    public int TariffScaleTypeId { get; set; }
    public string TariffScaleType { get; set; }
}