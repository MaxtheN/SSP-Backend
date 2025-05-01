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

namespace SspUis.BizLogicLayer.PrtnRejectReasonServices
{
    public class PrtnRejectReasonService : StatusGenericHandler, IPrtnRejectReasonService
    {
        private readonly IPrtnRejectReasonRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public PrtnRejectReasonService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _repository = unitOfWork.PrtnRejectReasonRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public PagedResult<PrtnRejectReasonListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<PrtnRejectReasonListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }

        public PrtnRejectReasonDto Get()
        {
            return new PrtnRejectReasonDto();
        }

        public PrtnRejectReasonDto Get(int id)
        {
            var dto = _repository.ById<PrtnRejectReasonDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public SelectList<int> AsSelectList(int? prtnContractTypeId = null)
        {
            var query = _repository.AllAsQueryable
                .Include(a=>a.PrtnContractType)
                .Include(a=>a.PrtnContractTypeTable).ThenInclude(x=>x.SignOrganizationType)
                .IsActive();

            if (prtnContractTypeId.HasValue)
                query = query.Where(a => a.PrtnContractTypeId == prtnContractTypeId || !a.PrtnContractTypeId.HasValue);

            if (_authService.Contractor != null)
                query = query.Where(a => a.PrtnContractTypeTable.SignOrganizationTypeId == SignOrganizationTypeIdConst.BUSINESSMAN);
            else
            {
                var orgSign = _unitOfWork.OrganizationRepository.ById(_authService.User.OrganizationId).Signs.FirstOrDefault(a => a.Pinfl == _authService.User.Pinfl);
                if (orgSign != null)
                    query = query.Where(a => a.PrtnContractTypeTableId == orgSign.PrtnContractTypeTableId);
            }
            return query.AsSelectList();
        }

        public HaveId<int> Create(CreatePrtnRejectReasonDlDto dto)
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

        public void Update(UpdatePrtnRejectReasonDlDto dto)
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

        private void Validation<TDto>(PrtnRejectReasonDlDto<TDto> dto, PrtnRejectReason entity)
            where TDto : PrtnRejectReasonDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

            if (dto.PrtnContractTypeId.HasValue && !dto.PrtnContractTypeTableId.HasValue
                || !dto.PrtnContractTypeId.HasValue && dto.PrtnContractTypeTableId.HasValue)
                AddError("Iltimos, imzo chekuvchini tanlang / Пожалуйста, выберите человека для подписи");
        }
    }
}
