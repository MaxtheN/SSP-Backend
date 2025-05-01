using GenericServices;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using SspUis.DataLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.DualEdu;

namespace SspUis.BizLogicLayer.Hrm.SpecialtyServices
{
    public class SpecialtyService : StatusGenericHandler, ISpecialtyService
    {
        private readonly ISpecialtyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public SpecialtyService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.SpecialtyRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<SpecialtyListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<SpecialtyListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public SpecialtyDto Get()
        {
            return new SpecialtyDto();
        }

        public SpecialtyDto Get(int id)
        {
            var dto = _repository.ById<SpecialtyDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList(int? instituteId = null)
        {
            return _repository.AllAsQueryable
                .AsSelectList(instituteId);
        }

        public HaveId<int> Create(CreateSpecialtyDlDto dto)
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

        public void Update(UpdateSpecialtyDlDto dto)
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

        private void Validation<TDto>(SpecialtyDlDto<TDto> dto, Specialty entity)
            where TDto : SpecialtyDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
