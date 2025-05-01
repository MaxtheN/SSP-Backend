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

namespace SspUis.BizLogicLayer.CurrencyServices
{
    public class CurrencyService : StatusGenericHandler, ICurrencyService
    {
        private readonly ICurrencyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public CurrencyService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.CurrencyRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<CurrencyListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<CurrencyListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public CurrencyDto Get()
        {
            return new CurrencyDto();
        }

        public CurrencyDto Get(int id)
        {
            var dto = _repository.ById<CurrencyDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList(int? langId)
        {
            return _repository.AllAsQueryable.AsSelectList(langId);
        }

        public HaveId<int> Create(CreateCurrencyDlDto dto)
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

        public void Update(UpdateCurrencyDlDto dto)
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
