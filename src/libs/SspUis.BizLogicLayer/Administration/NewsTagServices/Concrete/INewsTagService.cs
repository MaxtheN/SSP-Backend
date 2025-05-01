using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.NewsTagServices
{
    public interface INewsTagService : IStatusGeneric
    {
        PagedResult<NewsTagListDto> GetList(SortFilterPageOptions options);
        NewsTagDto Get();
        NewsTagDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateNewsTagDlDto dto);
        void Update(UpdateNewsTagDlDto dto);
        void Delete(int id);
    }
}
