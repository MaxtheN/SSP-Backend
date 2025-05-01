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
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.TaxBenefitTypeServices
{
    public class TaxBenefitTypeService : StatusGenericHandler, ITaxBenefitTypeService
    {
        private readonly ITaxBenefitTypeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public TaxBenefitTypeService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.TaxBenefitTypeRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<TaxBenefitTypeListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<TaxBenefitTypeListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public TaxBenefitTypeDto Get()
        {
            return new TaxBenefitTypeDto();
        }

        public TaxBenefitTypeDto Get(int id)
        {
            var dto = _repository.ById<TaxBenefitTypeDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable
                .AsSelectList();
        }

        public HaveId<int> Create(CreateTaxBenefitTypeDlDto dto)
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

        public void Update(UpdateTaxBenefitTypeDlDto dto)
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

        private void Validation<TDto>(TaxBenefitTypeDlDto<TDto> dto, TaxBenefitType entity)
            where TDto : TaxBenefitTypeDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
