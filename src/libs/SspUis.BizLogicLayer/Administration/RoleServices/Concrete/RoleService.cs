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

namespace SspUis.BizLogicLayer.RoleServices
{
    public class RoleService : StatusGenericHandler, IRoleService
    {
        private readonly IRoleRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public RoleService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.RoleRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<RoleListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<RoleListDto>()
                            .SortFilter(dto)
                            .AsPagedResult(dto);
            return result;
        }

        public RoleDto Get()
        {
            return new RoleDto();
        }

        public RoleDto Get(int id)
        {
            var dto = _repository.ById<RoleDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable.AsSelectList();
        }

        public HaveId<int> Create(CreateRoleDlDto dto)
        {
            var entity = _repository.Create(dto);
            CombineStatuses(_repository);
            if (IsValid)
            {
                _unitOfWork.Save();
                return HaveId.Create(entity.Id);
            }
            return null;
        }

        public void Update(UpdateRoleDlDto dto)
        {
            _repository.Update(dto);
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

    }
}
