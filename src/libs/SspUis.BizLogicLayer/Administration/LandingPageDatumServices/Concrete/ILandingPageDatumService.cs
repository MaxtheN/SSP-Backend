using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.LandingPageDatumServices
{
    public interface ILandingPageDatumService : IStatusGeneric
    {
        PagedResult<LandingPageDatumListDto> GetList(SortFilterPageOptions options);
        LandingPageDatumDto Get();
        LandingPageDatumDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateLandingPageDatumDlDto dto);
        void Update(UpdateLandingPageDatumDlDto dto);
        void Delete(int id);
    }
}
