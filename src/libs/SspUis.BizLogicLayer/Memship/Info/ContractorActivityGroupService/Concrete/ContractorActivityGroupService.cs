using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.Hrm.ContractorActivityGroupServices;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.ContractorActivityGroupServices
{
    public class ContractorActivityGroupService : StatusGenericHandler, IContractorActivityGroupService
    {
        private readonly IContractorActivityGroupRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public ContractorActivityGroupService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.ContractorActivityGroupRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<ContractorActivityGroupListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<ContractorActivityGroupListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public ContractorActivityGroupDto Get()
        {
            return new ContractorActivityGroupDto();
        }

        public ContractorActivityGroupDto Get(int id)
        {
            var dto = _repository.ById<ContractorActivityGroupDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable
                            .AsSelectList();
        }

        public HaveId<int> Create(CreateContractorActivityGroupDlDto dto)
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

        public void Update(UpdateContractorActivityGroupDlDto dto)
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

        private void Validation<TDto>(ContractorActivityGroupDlDto<TDto> dto, ContractorActivityGroup entity)
            where TDto : ContractorActivityGroupDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
