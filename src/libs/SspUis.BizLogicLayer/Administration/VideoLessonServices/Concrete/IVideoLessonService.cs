using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;
using WEBASE;

namespace SspUis.BizLogicLayer.VideoLessonServices
{
    public interface IVideoLessonService : IStatusGeneric
    {
        PagedResult<VideoLessonListDto> GetList(TableSortFilterPageOptions options);
        VideoLessonDto Get();
        VideoLessonDto Get(long id);
        SelectList<long> AsSelectList(int? categoryId = null);
        HaveId<long> Create(CreateVideoLessonDlDto dto);
        void Update(UpdateVideoLessonDlDto dto);
        void Delete(long id);
    }
}
