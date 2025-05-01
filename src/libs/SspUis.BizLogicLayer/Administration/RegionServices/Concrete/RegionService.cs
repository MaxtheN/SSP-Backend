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

namespace SspUis.BizLogicLayer.RegionServices
{
    public class RegionService : StatusGenericHandler, IRegionService
    {
        private readonly IRegionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public RegionService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.RegionRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<RegionListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<RegionListDto>()
                            .SortFilter(dto)
                            .AsPagedResult(dto);
            return result;
        }

        public RegionDto Get()
        {
            return new RegionDto();
        }

        public RegionDto Get(int id)
        {
            var dto = _repository.ById<RegionDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable.AsSelectList();
        }

        public HaveId<int> Create(CreateRegionDlDto dto)
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

        public void Update(UpdateRegionDlDto dto)
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
