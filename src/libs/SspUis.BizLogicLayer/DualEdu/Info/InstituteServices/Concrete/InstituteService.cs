using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.DualEdu;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.InstituteServices
{
    public class InstituteService : StatusGenericHandler, IInstituteService
    {
        private readonly IInstituteRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public InstituteService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.InstituteRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<InstituteListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<InstituteListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }
        public InstituteDto Get()
        {
            return new InstituteDto();
        }
        public InstituteDto Get(int id)
        {
            var dto = _repository.ById<InstituteDto>(id);
            CombineStatuses(_repository);
            return dto;
        }
        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable
                .AsSelectList();
        }
        public HaveId<int> Create(CreateInstituteDlDto dto)
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
        public void Update(UpdateInstituteDlDto dto)
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
                AddError("Запись не может быть удален");
            }
        }
        private void Validation<TDto>(InstituteDlDto<TDto> dto, Institute entity)
            where TDto : InstituteDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);
        }
    }
}