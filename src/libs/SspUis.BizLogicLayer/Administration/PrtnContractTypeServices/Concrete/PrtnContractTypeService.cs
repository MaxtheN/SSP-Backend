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

namespace SspUis.BizLogicLayer.PrtnContractTypeServices
{
    public class PrtnContractTypeService : StatusGenericHandler, IPrtnContractTypeService
    {
        private readonly IPrtnContractTypeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public PrtnContractTypeService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.PrtnContractTypeRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<PrtnContractTypeListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<PrtnContractTypeListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public PrtnContractTypeDto Get()
        {
            return new PrtnContractTypeDto();
        }

        public PrtnContractTypeDto Get(int id)
        {
            var dto = _repository.ById<PrtnContractTypeDto>(id);
            CombineStatuses(_repository);
            if (IsValid)
                dto.Tables = dto.Tables.OrderBy(a => a.OrderNumber).ToList();
            return dto;
        }

        public List<PrtnContractTypeTableDto> GetTables(int id)
        {
            var dto = _repository.ById<PrtnContractTypeDto>(id);
            CombineStatuses(_repository);
            return dto.Tables.OrderBy(a => a.OrderNumber).ToList();
        }

        public SelectList<int> GetTablesBySignOrganizationType(TablesBySignOrganizationTypeDtoFilter filter)
        {
             var query = _unitOfWork.Context.Set<PrtnContractTypeTable>()
                .Where(a => a.SignOrganizationTypeId == filter.SignOrganizationTypeId);

            if (filter.IncludeBusinessman.HasValue && !filter.IncludeBusinessman.Value)
                query = query.Where(a => a.SignOrganizationTypeId != SignOrganizationTypeIdConst.BUSINESSMAN);

            if (filter.OrganizationId.HasValue)
                query = query.Where(a => a.OrganizationId == filter.OrganizationId);

            if (filter.RegionId.HasValue)
            {
                if (query.Where(a => a.RegionId == filter.RegionId).Count() > 0)
                    query = query.Where(a => a.RegionId == filter.RegionId);
                else
                    query = query.Where(a => !a.RegionId.HasValue);
            }

            return new SelectList<int>(
                query.Select(a => new SelectListItem<int>
                {
                    Value = a.Id,
                    OrderCode = a.OrderNumber.ToString(),
                    Text = a.Position.Translates.AsQueryable()
                        .FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Position.FullName
                }).OrderBy(a => a.OrderCode).ToList()
            );
        }

        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable.AsSelectList();
        }

        public HaveId<int> Create(CreatePrtnContractTypeDlDto dto)
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

        public void Update(UpdatePrtnContractTypeDlDto dto)
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

        private void Validation<TDto>(PrtnContractTypeDlDto<TDto> dto, PrtnContractType entity)
            where TDto : PrtnContractTypeDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
            {
                query = query.Where(a => a.Id != entity.Id);

            }

            if (dto.EmployeeRangeTo.HasValue && dto.EmployeeRangeFrom > dto.EmployeeRangeTo)
                AddError($"Hodimlar oralig'i xato kiritilgan. Boshlanish soni {dto.EmployeeRangeFrom}, tugash soni {dto.EmployeeRangeTo}");

            if (dto.Tables.Any(a => a.SignOrganizationTypeId == SignOrganizationTypeIdConst.MINISTRY && !a.OrganizationId.HasValue))
                AddError($"Iltimos vazirlikni tanlang / Пожалуйста выберите министерства");

            if (dto.Tables.Any(a => a.SignOrganizationTypeId != SignOrganizationTypeIdConst.BUSINESSMAN && !a.PositionId.HasValue))
                AddError($"Iltimos lavozimni tanlang / Пожалуйста выберите должность");
        }
    }
}
