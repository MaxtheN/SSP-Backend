using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.Corruption.ContractorUnionActivityTypeServices;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Corruption;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Corruption.ContractorUnionActivityTypeServices
{
    public class ContractorUnionActivityTypeService : StatusGenericHandler, IContractorUnionActivityTypeService
    {
        private readonly IContractorUnionActivityTypeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public ContractorUnionActivityTypeService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.ContractorUnionActivityTypeRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<ContractorUnionActivityTypeListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<ContractorUnionActivityTypeListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public ContractorUnionActivityTypeDto Get()
        {
            return new ContractorUnionActivityTypeDto();
        }

        public ContractorUnionActivityTypeDto Get(int id)
        {
            var dto = _repository.ById<ContractorUnionActivityTypeDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable.Include(a => a.Translates)
                            .AsSelectList();
        }

        public HaveId<int> Create(CreateContractorUnionActivityTypeDlDto dto)
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

        public void Update(UpdateContractorUnionActivityTypeDlDto dto)
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

        private void Validation<TDto>(ContractorUnionActivityTypeDlDto<TDto> dto, ContractorUnionActivityType entity)
            where TDto : ContractorUnionActivityTypeDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
