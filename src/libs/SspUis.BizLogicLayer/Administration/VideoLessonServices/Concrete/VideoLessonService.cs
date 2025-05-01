using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.VideoLessonServices
{
    public class VideoLessonService : StatusGenericHandler, IVideoLessonService
    {
        private readonly IVideoLessonRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        public VideoLessonService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.VideoLessonRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }
        public PagedResult<VideoLessonListDto> GetList(TableSortFilterPageOptions options)
        {
            var result = _repository.ReadAsNoTracked<VideoLessonListDto>()
                        .SortFilter(options)
                        .ToTableData(options);
            return result;
        }
        public VideoLessonDto Get()
        {
            return new VideoLessonDto();
        }
        public VideoLessonDto Get(long id)
        {
            var dto = _repository.ById<VideoLessonDto>(id);
            CombineStatuses(_repository);
            return dto;
        }
        public SelectList<long> AsSelectList(int? categoryId = null)
        {
            return _repository.AllAsQueryable.AsSelectList(categoryId);
        }
        public HaveId<long> Create(CreateVideoLessonDlDto dto)
        {
            var entity = _repository.Create(dto);
            CombineStatuses(_repository);
            if (IsValid)
            {
                _unitOfWork.Save();
                return HaveId.Create(entity.Id);
            }
            return null;
        }
        public void Update(UpdateVideoLessonDlDto dto)
        {
            _repository.Update(dto);
            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();
        }
        public void Delete(long id)
        {
            try
            {
                _repository.Delete(id);
                CombineStatuses(_repository);
                if (IsValid)
                    _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                AddError("Запись не может быть удален");
            }
        }
    }
}
