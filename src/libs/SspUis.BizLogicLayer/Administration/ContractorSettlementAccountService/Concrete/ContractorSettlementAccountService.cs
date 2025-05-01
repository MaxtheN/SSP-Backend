using System;
using System.Collections.Generic;
using System.Linq;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Administration.ContractorSettlementAccountService
{
    public class ContractorSettlementAccountService : StatusGenericHandler, IContractorSettlementAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IContractorSettlementAccountRepository _repository;

        public ContractorSettlementAccountService(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _repository = unitOfWork.ContractorSettlementAccountRepository;
        }

        public HaveId<int> Create(CreateContractorSettlementAccountDlDto dto)
        {
            ContractorSettlementAccount entity = _repository.Create(dto, ent => Validation(dto, ent));
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

        public ContractorSettlementAccountDto Get(long id)
        {
            var dto = _repository.ById<ContractorSettlementAccountDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public ContractorSettlementAccountDto Get()
        {
            var dto = new ContractorSettlementAccountDto()
            {
                OwnerId = _authService.Contractor.Id
            };
            return dto;
        }

        public List<ContractorSettlementAccountListDto> GetList()
        {
            var contractorId = _authService.Contractor.Id;
            List<ContractorSettlementAccountListDto> result = _repository.ReadAsNoTracked<ContractorSettlementAccountListDto>().Where(a => a.OwnerId == contractorId).OrderBy(a=>a.Id).ToList();
            return result;
        }

        public bool SetMain(long id)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var contractor = _authService.Contractor;
                    var data = _unitOfWork.Context.Set<ContractorSettlementAccount>().Where(a => a.OwnerId == contractor.Id);
                    foreach (var item in data)
                    {
                        item.IsMain = false;
                    }
                  
                    if (IsValid)
                        _unitOfWork.Save();

                    var result = data.FirstOrDefault(a => a.Id == id);
                    result.IsMain = true;
                    if (IsValid)
                        _unitOfWork.Save();

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }


        }

        public void Update(UpdateContractorSettlementAccountDlDtoo dto)
        {
            var a = _repository.Update(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();
        }

        private void Validation<TDto>(ContractorSettlementAccountDlDtoo<TDto> dto, ContractorSettlementAccount entity)
        where TDto : ContractorSettlementAccountDlDtoo<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}
