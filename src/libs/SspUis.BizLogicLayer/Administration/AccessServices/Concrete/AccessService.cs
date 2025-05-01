using SspUis.DataLayer.Repositories;
using SspUis.DataLayer;
using StatusGeneric;
using SspUis.Core.Security;
using SspUis.BizLogicLayer.RoleServices;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;
using WEBASE.Utility;
using WEBASE;

namespace SspUis.BizLogicLayer.AccessServices
{
    public class AccessService : StatusGenericHandler, IAccessService
    {
        private readonly IAccessibilityRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public AccessService(
            IAuthService authService, 
            IAccessibilityRepository repository, 
            IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public PagedResult<AccessListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<AccessListDto>()
                .SortFilter(dto)
                .AsPagedResult(dto);
                            
            return result;
        }

        public AccessDto Get(int id)
        {
            var dto = _repository.ById<AccessDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable.AsSelectList();
        }

        public void Update(UpdateAccessibilityDlDto dto)
        {
            _repository.Update(dto);
            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();
        }
    }
}
