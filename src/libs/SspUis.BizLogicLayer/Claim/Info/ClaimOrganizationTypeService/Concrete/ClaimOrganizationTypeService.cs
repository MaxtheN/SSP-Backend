using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Claim.ClaimOrganizationTypeServices
{
    public class ClaimOrganizationTypeService : StatusGenericHandler, IClaimOrganizationTypeService
    {
        private readonly IClaimOrganizationTypeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public ClaimOrganizationTypeService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.ClaimOrganizationTypeRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<ClaimOrganizationTypeListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<ClaimOrganizationTypeListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public ClaimOrganizationTypeDto Get()
        {
            return new ClaimOrganizationTypeDto();
        }

        public ClaimOrganizationTypeDto Get(int id)
        {
            var dto = _repository.ById<ClaimOrganizationTypeDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable
                            .AsSelectList();
        }

        public HaveId<int> Create(CreateClaimOrganizationTypeDlDto dto)
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

        public void Update(UpdateClaimOrganizationTypeDlDto dto)
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

        private void Validation<TDto>(ClaimOrganizationTypeDlDto<TDto> dto, ClaimOrganizationType entity)
            where TDto : ClaimOrganizationTypeDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
