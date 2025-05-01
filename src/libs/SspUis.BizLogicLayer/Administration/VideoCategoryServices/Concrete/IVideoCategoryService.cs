using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.VideoCategoryServices
{
    public interface IVideoCategoryService : IStatusGeneric
    {
        PagedResult<VideoCategoryListDto> GetList(SortFilterPageOptions options);
        VideoCategoryDto Get();
        VideoCategoryDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateVideoCategoryDlDto dto);
        void Update(UpdateVideoCategoryDlDto dto);
        void Delete(int id);
    }
}
