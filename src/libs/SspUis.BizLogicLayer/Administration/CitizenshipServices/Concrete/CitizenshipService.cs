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

namespace SspUis.BizLogicLayer.CitizenshipServices
{
    public class CitizenshipService : StatusGenericHandler, ICitizenshipService
    {
        private readonly ICitizenshipRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public CitizenshipService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.CitizenshipRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<CitizenshipListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<CitizenshipListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public CitizenshipDto Get()
        {
            return new CitizenshipDto();
        }

        public CitizenshipDto Get(int id)
        {
            var dto = _repository.ById<CitizenshipDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable.AsSelectList();
        }

        public HaveId<int> Create(CreateCitizenshipDlDto dto)
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

        public void Update(UpdateCitizenshipDlDto dto)
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
