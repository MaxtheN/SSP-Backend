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

namespace SspUis.BizLogicLayer.LandingPageDatumServices
{
    public class LandingPageDatumService : StatusGenericHandler, ILandingPageDatumService
    {
        private readonly ILandingPageDatumRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        public LandingPageDatumService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.LandingPageDatumRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }
        public PagedResult<LandingPageDatumListDto> GetList(SortFilterPageOptions options)
        {
            var result = _repository.ReadAsNoTracked<LandingPageDatumListDto>()
                        .SortFilter(options)
                        .AsPagedResult(options);
            return result;
        }

        public LandingPageDatumDto Get()
        {
            return new LandingPageDatumDto();
        }

        public LandingPageDatumDto Get(int id)
        {
            var dto = _repository.ById<LandingPageDatumDto>(id);
            CombineStatuses(_repository);
            return dto;
        }
        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable.AsSelectList();
        }

        public HaveId<int> Create(CreateLandingPageDatumDlDto dto)
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

        public void Update(UpdateLandingPageDatumDlDto dto)
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
