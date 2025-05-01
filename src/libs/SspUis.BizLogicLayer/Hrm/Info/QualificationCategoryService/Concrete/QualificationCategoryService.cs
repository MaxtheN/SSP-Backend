using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.Hrm.QualificationCategoryServices;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.QualificationCategoryServices
{
    public class QualificationCategoryService : StatusGenericHandler, IQualificationCategoryService
    {
        private readonly IQualificationCategoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public QualificationCategoryService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.QualificationCategoryRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<QualificationCategoryListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<QualificationCategoryListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public QualificationCategoryDto Get()
        {
            return new QualificationCategoryDto();
        }

        public QualificationCategoryDto Get(int id)
        {
            var dto = _repository.ById<QualificationCategoryDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable
                            .AsSelectList();
        }

        public HaveId<int> Create(CreateQualificationCategoryDlDto dto)
        {
            var entity = _repository.Create(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            if (IsValid)
            {
                _unitOfWork.Save();
                return HaveId.Create(entity.Id);
            }
            return null;
        }

        public void Update(UpdateQualificationCategoryDlDto dto)
        {
            _repository.Update(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();
        }

        public void Delete(int id)
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
                AddError("������ �� ����� ���� ������");
            }
        }

        private void Validation<TDto>(QualificationCategoryDlDto<TDto> dto, QualificationCategory entity)
            where TDto : QualificationCategoryDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
