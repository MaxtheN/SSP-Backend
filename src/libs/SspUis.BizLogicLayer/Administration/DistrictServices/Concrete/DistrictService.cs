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

namespace SspUis.BizLogicLayer.DistrictServices
{
    public class DistrictService : StatusGenericHandler, IDistrictService
    {
        private readonly IDistrictRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public DistrictService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.DistrictRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<DistrictListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<DistrictListDto>()
                            .SortFilter(dto)
                            .AsPagedResult(dto);
            return result;
        }

        public DistrictDto Get()
        {
            return new DistrictDto();
        }

        public DistrictDto Get(int id)
        {
            var dto = _repository.ById<DistrictDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList(int? regionId)
        {
            return _repository.AllAsQueryable.Where(a => regionId == null || a.RegionId == regionId)
                                             .AsSelectList();
        }

        public HaveId<int> Create(CreateDistrictDlDto dto)
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

        public void Update(UpdateDistrictDlDto dto)
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
