using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.BusinessActivityTypeServices
{
    public interface IBusinessActivityTypeService : IStatusGeneric
    {
        Task<BusinessActivityTypeDto> SyncContractorInfo();
        PagedResult<BusinessActivityTypeListDto> GetList(SortFilterPageOptions dto);
    }
}
