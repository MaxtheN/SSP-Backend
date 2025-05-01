using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ReportServices.Main;

public class GetTaxQqsAylanmaDtoFilter:SortFilterPageOptions
{
    public long? ContractorId { get; set; }
    public string ContractorInn { get; set; }
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
    public bool ByRegion { get; set; }
    public bool ByDistrict { get; set; }
    public bool ByContractor { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
}

    
   
