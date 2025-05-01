using SspUis.BizLogicLayer.Models;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.NewsServices
{
    public interface INewsService : IStatusGeneric
    {
        PagedResult<NewsListDto> GetList(WEBASE.TableSortFilterPageOptions options);
        NewsDto Get();
        NewsDto Get(int id);
        SelectList<int> AsSelectList();
        HaveId<int> Create(CreateNewsDlDto dto);
        void Update(UpdateNewsDlDto dto);
        void Delete(int id);
        IStorageFileInfo UploadNewsImage(StorageFile file);
        (byte[], string)? GetNewsImage(Guid newsImageId);
        PagedResult<NewsListDto> GetListByTag(string tag, SortFilterPageOptions options);
        NewsViewDto GetForView(int id);
        List<TagDto> GetTags();
    }
}
