using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateCourtntegrationDlDto : CourtntegrationDlDto<UpdateCourtntegrationDlDto>, IHaveIdProp<int>
    {
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
