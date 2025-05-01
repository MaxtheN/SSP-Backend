using SspUis.BizLogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class StateAssetDocumentSortFilterOptions : DocumentSortFilterOptions
    {
        public string ContractorInn { get; set; }
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
    }
}
