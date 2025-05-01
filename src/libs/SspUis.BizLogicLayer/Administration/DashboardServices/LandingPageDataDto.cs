using SspUis.BizLogicLayer.LandingPageDatumServices;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.DashboardServices
{
    public class LandingPageDataDto
    {
        public int AllRequestCount { get; set; }
        public int AgreedRequestCount { get; set; }
        public int RejectedRequestCount { get; set; }
        public int InspectionOrganiztionCount { get; set; }
        public int InspectionOrganiztionFilialCount { get; set; }
        public List<LandingPageDatumListDto> OtherElements { get; set; } = new();
    }
}
