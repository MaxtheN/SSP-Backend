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

namespace SspUis.BizLogicLayer.Hrm.SettlementAccountSourceServices
{
    public class SettlementAccountSourceService : StatusGenericHandler, ISettlementAccountSourceService
    {
        private readonly ISettlementAccountSourceRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public SettlementAccountSourceService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.SettlementAccountSourceRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<SettlementAccountSourceListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<SettlementAccountSourceListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public SettlementAccountSourceDto Get()
        {
            return new SettlementAccountSourceDto();
        }

        public SettlementAccountSourceDto Get(int id)
        {
            var dto = _repository.ById<SettlementAccountSourceDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable
                .AsSelectList();
        }

        public HaveId<int> Create(CreateSettlementAccountSourceDlDto dto)
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

        public void Update(UpdateSettlementAccountSourceDlDto dto)
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

        private void Validation<TDto>(SettlementAccountSourceDlDto<TDto> dto, SettlementAccountSource entity)
            where TDto : SettlementAccountSourceDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

           var code = dto.Code1 + dto.Code2 + dto.Code3 + dto.Code4;
            if (query.Where(x => x.Code.Equals(code)).Any())
            {
                AddError($"Это информация с этим кодом {code} уже существует", nameof(code));
            }

        }
    }
}
