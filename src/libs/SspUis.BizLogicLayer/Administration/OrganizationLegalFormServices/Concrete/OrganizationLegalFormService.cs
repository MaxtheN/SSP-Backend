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
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.OrganizationLegalFormServices
{
    public class OrganizationLegalFormService : StatusGenericHandler, IOrganizationLegalFormService
    {
        private readonly IOrganizationLegalFormRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        public OrganizationLegalFormService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.OrganizationLegalFormRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }
        public PagedResult<OrganizationLegalFormListDto> GetList(SortFilterPageOptions options)
        {
            var result = _repository.ReadAsNoTracked<OrganizationLegalFormListDto>()
                        .SortFilter(options)
                        .AsPagedResult(options);
            return result;
        }

        public OrganizationLegalFormDto Get()
        {
            return new OrganizationLegalFormDto();
        }

        public OrganizationLegalFormDto Get(int id)
        {
            var dto = _repository.ById<OrganizationLegalFormDto>(id);
            CombineStatuses(_repository);
            return dto;
        }
        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable.AsSelectList();
        }

        public HaveId<int> Create(CreateOrganizationLegalFormDlDto dto)
        {
            var entity = _repository.Create(dto);
            CombineStatuses(_repository);
            if(IsValid)
            {
                _unitOfWork.Save();
                return HaveId.Create(entity.Id);
            }
            return null;
        }

        public void Update(UpdateOrganizationLegalFormDlDto dto)
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
