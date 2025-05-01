using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.PrtnRejectReasonServices
{
    public interface IPrtnRejectReasonService : IStatusGeneric
    {
        PagedResult<PrtnRejectReasonListDto> GetList(SortFilterPageOptions dto);
        PrtnRejectReasonDto Get();
        PrtnRejectReasonDto Get(int id);
        SelectList<int> AsSelectList(int? prtnContractTypeId = null);
        HaveId<int> Create(CreatePrtnRejectReasonDlDto dto);
        void Update(UpdatePrtnRejectReasonDlDto dto);
        void Delete(int id);
    }
}
