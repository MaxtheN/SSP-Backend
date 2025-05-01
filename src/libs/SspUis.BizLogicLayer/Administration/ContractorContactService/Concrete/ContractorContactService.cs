using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Administration.ContractorContactService
{
    public class ContractorContactService : StatusGenericHandler, IContractorContactService
    {
        private readonly IContractorContactRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public ContractorContactService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _repository = unitOfWork.ContractorContactRepository;
        }

        public HaveId<int> Create(CreateContractorContactDlDto dto)
        {
            var entity = _repository.Create(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            if (IsValid)
            {
                _unitOfWork.Save();
                return HaveId.Create((int)entity.Id);
            }
            return null;
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
            catch (Exception)
            {
                AddError("Ошибка при удаление");
            }
        }

        public ContractorContactDto Get(long id)
        {
            var dto = _repository.ById<ContractorContactDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public ContractorContactDto Get()
        {
            ContractorContactDto dto = new ContractorContactDto()
            {
                OwnerId = _authService.Contractor.Id
            };
            return dto;
        }

        public List<ContractorContactListDto> GetList()
        {
            var contractorId = _authService.Contractor.Id;
            List<ContractorContactListDto> result = _repository.ReadAsNoTracked<ContractorContactListDto>().Where(a=>a.OwnerId == contractorId).ToList();
            return result;
        }

        public void Update(UpdateContractorContactDlDto dto)
        {
           var a = _repository.Update(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();
        }


        private void Validation<TDto>(ContractorContactDlDtoo<TDto> dto, ContractorContact entity)
          where TDto : ContractorContactDlDtoo<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
