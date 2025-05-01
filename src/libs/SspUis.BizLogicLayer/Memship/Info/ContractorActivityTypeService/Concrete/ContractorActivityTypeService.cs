using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.Hrm.ContractorActivityTypeServices;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.ContractorActivityTypeServices
{
    public class ContractorActivityTypeService : StatusGenericHandler, IContractorActivityTypeService
    {
        private readonly IContractorActivityTypeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public ContractorActivityTypeService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.ContractorActivityTypeRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<ContractorActivityTypeListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<ContractorActivityTypeListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public ContractorActivityTypeDto Get()
        {
            return new ContractorActivityTypeDto();
        }

        public ContractorActivityTypeDto Get(int id)
        {
            var dto = _repository.ById<ContractorActivityTypeDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable
                            .AsSelectList();
        }

        public HaveId<int> Create(CreateContractorActivityTypeDlDto dto)
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

        public void Update(UpdateContractorActivityTypeDlDto dto)
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

        private void Validation<TDto>(ContractorActivityTypeDlDto<TDto> dto, ContractorActivityType entity)
            where TDto : ContractorActivityTypeDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
