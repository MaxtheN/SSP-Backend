using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Claim.ClaimThemeServices
{
    public class ClaimThemeService : StatusGenericHandler, IClaimThemeService
    {
        private readonly IClaimThemeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public ClaimThemeService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.ClaimThemeRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<ClaimThemeListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<ClaimThemeListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public ClaimThemeDto Get()
        {
            return new ClaimThemeDto();
        }

        public ClaimThemeDto Get(int id)
        {
            var dto = _repository.ById<ClaimThemeDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList(int? langId)
        {
            return _repository.AllAsQueryable
                            .AsSelectList(langId);
        }

        public HaveId<int> Create(CreateClaimThemeDlDto dto)
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

        public void Update(UpdateClaimThemeDlDto dto)
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

        private void Validation<TDto>(ClaimThemeDlDto<TDto> dto, ClaimTheme entity)
            where TDto : ClaimThemeDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
