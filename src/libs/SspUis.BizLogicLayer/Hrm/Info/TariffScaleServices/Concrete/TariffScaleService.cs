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
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.TariffScaleServices
{
    public class TariffScaleService : StatusGenericHandler, ITariffScaleService
    {
        private readonly ITariffScaleRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public TariffScaleService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.TariffScaleRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<TariffScaleListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<TariffScaleListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public TariffScaleDto Get()
        {
            return new TariffScaleDto();
        }

        public TariffScaleDto Get(int id)
        {
            var dto = _repository.ById<TariffScaleDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable.Include(a => a.Translates)
                .AsSelectList();
        }
        public SelectList<int> AsSelectList(int typeId)
        {
            return _repository.AllAsQueryable.Include(a => a.Translates).AsSelectList(typeId);
        }

        public SelectList<int> AsTableSelectList(int id)
        {
            return _repository.AllAsQueryable.AsTableSelectList(id);
        }
        public HaveId<int> Create(CreateTariffScaleDlDto dto)
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

        public void Update(UpdateTariffScaleDlDto dto)
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
                AddError("Запись не может быть удален");
            }
        }

        private void Validation<TDto>(TariffScaleDlDto<TDto> dto, TariffScale entity)
            where TDto : TariffScaleDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
