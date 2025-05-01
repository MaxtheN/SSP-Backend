using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Info.OrganizationalStructureServices
{
    internal class UploadOrganizationStructureDto
    {
        
            public int OrganizationStructureId { get; set; }
            public int PositionClassificationId { get; set; }
            public string PositionName { get; set; }
            public int PositionId { get; set; }
            public int PositionTypeId { get; set; }
            public int PositionCategoryId { get; set; }
            public int TarifScaleTypeId { get; set; }
            public int? TarifScaleId { get; set; }
            public string RankCode { get; set; }
            public int? RankId { get; set; }
            public int SchoolGroupId{ get; set; }
            public decimal Sum  { get; set; }
        
    }
}
