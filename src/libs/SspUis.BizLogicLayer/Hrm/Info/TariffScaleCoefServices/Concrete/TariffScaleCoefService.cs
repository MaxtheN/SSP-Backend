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

namespace SspUis.BizLogicLayer.Hrm.TariffScaleCoefServices
{
    public class TariffScaleCoefService : StatusGenericHandler, ITariffScaleCoefService
    {
        private readonly ITariffScaleCoefRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public TariffScaleCoefService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.TariffScaleCoefRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<TariffScaleCoefListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<TariffScaleCoefListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public TariffScaleCoefDto Get()
        {
            return new TariffScaleCoefDto();
        }

        public TariffScaleCoefDto Get(int id)
        {
            var dto = _repository.ById<TariffScaleCoefDto>(id);
            CombineStatuses(_repository);
            dto.Tables = dto.Tables.OrderBy(a => a.OrderCode).ToList();
            return dto;
        }

        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable.Include(a => a.Translates)
                .AsSelectList();
        }
        public SelectList<int> GetTableAsSelectList(int? tariffScaleId = null, int? tariffScaleTableId = null)
        {
            return _repository.Context.Set<TariffScaleCoefTable>()
                            .AsSelectList(tariffScaleId, tariffScaleTableId);
        }
        public HaveId<int> Create(CreateTariffScaleCoefDlDto dto)
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

        public void Update(UpdateTariffScaleCoefDlDto dto)
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

        private void Validation<TDto>(TariffScaleCoefDlDto<TDto> dto, TariffScaleCoef entity)
            where TDto : TariffScaleCoefDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
