using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Claim.ClaimOrganizationServices
{
    public class ClaimOrganizationService : StatusGenericHandler, IClaimOrganizationService
    {
        private readonly IClaimOrganizationRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public ClaimOrganizationService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.ClaimOrganizationRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<ClaimOrganizationListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<ClaimOrganizationListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public ClaimOrganizationDto Get()
        {
            return new ClaimOrganizationDto();
        }

        public ClaimOrganizationDto Get(int id)
        {
            var dto = _repository.ById<ClaimOrganizationDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable
                            .AsSelectList();
        }

        public HaveId<int> Create(CreateClaimOrganizationDlDto dto)
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

        public void Update(UpdateClaimOrganizationDlDto dto)
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

        private void Validation<TDto>(ClaimOrganizationDlDto<TDto> dto, ClaimOrganization entity)
            where TDto : ClaimOrganizationDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
