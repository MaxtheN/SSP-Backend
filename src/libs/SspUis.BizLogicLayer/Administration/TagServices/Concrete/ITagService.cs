using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.TagServices
{
    public interface ITagService : IStatusGeneric
    {
        PagedResult<TagListDto> GetList(SortFilterPageOptions options);
        TagDto Get();
        TagDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateTagDlDto dto);
        void Update(UpdateTagDlDto dto);
        void Delete(int id);
    }
}
