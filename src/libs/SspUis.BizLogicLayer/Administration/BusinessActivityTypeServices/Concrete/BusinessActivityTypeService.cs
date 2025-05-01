using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.Models;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.TadbirkorFund;
using SspUis.ServiceLayer.NumberServices;
using StatusGeneric;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.BusinessActivityTypeServices
{
    public class BusinessActivityTypeService : StatusGenericHandler, IBusinessActivityTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly ITadbirkorFundContractorService _tadbirkorFundContractorService;
        private readonly IBusinessActivityTypeRepository _repository;

        public BusinessActivityTypeService(
            IUnitOfWork unitOfWork,
            IAuthService authService,
            ITadbirkorFundContractorService tadbirkorFundContractorService,
            IBusinessActivityTypeRepository repository)
        {

            _unitOfWork = unitOfWork;
            _authService = authService;
            _tadbirkorFundContractorService = tadbirkorFundContractorService;
            _repository = repository;
        }
        public PagedResult<BusinessActivityTypeListDto> GetList(SortFilterPageOptions dto)
        {
            var result = _repository.ReadAsNoTracked<BusinessActivityTypeListDto>().SortFilter(dto).AsPagedResult(dto);
            return result;
        }
        /* public async Task<BusinessActivityTypeDto> SyncContractorInfo()
         {

             using (var transaction = _unitOfWork.BeginTransaction())
             {
                 var allContractor = _unitOfWork.ContractorRepository.AllAsQueryable.IsActive()
                 .Include(a => a.Bank)
                 .Include(a => a.BusinessmanUserInContractors)
                 .Include(a => a.District)
                 .Include(a => a.Contacts)
                 .Include(a => a.Region)
                 .Include(a => a.SettlementAccounts)
                 .ToList();

                 CombineStatuses(_unitOfWork.ContractorRepository);

                 if (HasErrors || !allContractor.Any())
                 {
                     transaction.Rollback();
                     return null;
                 }

                 foreach (var item in allContractor)
                 {
                     var data = await _tadbirkorFundContractorService.GetByInn(item.Inn);

                     CombineStatuses(_tadbirkorFundContractorService);

                     if (HasErrors)
                     {
                         transaction.Rollback();
                         return null;
                     }

                     if (data.Id != 0)
                     {
                         try
                         {
                             var hasOldBusinessActivityType = _unitOfWork.Context.Set<BusinessActivityType>().Any(c => c.Contractor.Inn == item.Inn);

                             if (!hasOldBusinessActivityType)
                             {
                                 var bank = _unitOfWork.BankRepository.ByCode(data.BankCode);

                                 if (bank == null)
                                 {
                                     AddError($"{data.BankCode} bank tizimda mavjud emas");
                                     transaction.Rollback();
                                     return null;
                                 }

                                 int bankId = bank.Id;

                                 var dlDto = new CreateBusinessActivityTypeDlDto
                                 {
                                     BankId = bankId,
                                     ContractorId = item.Id,
                                     BusinessCtorId = data.BusinessSectorId,
                                     BusinessCtorName = data.BusinessSectorName,
                                     FinancialHelpId = data.FinancialAssistanceId,
                                     FinancialHelp = data.FinancialaAsistanceName,
                                     HelpAmount = data.AidAmount,
                                     NewEmployeesCount = data.NewJobPosition,
                                     RealEmployeesCount = data.RealJobPosition
                                 };

                                 foreach (var credit in data.Credits)
                                 {
                                     dlDto.Tables.Add(new BusinessActivityTypeTableDlDto
                                     {
                                         Amount = credit.Amount,
                                         CurrencyId = credit.CurrencyId,
                                         OwnerId = dlDto.ContractorId
                                     });
                                 }

                                 var dto = _repository.Create(dlDto);
                                 CombineStatuses(_repository);

                                 if (HasErrors)
                                 {
                                     transaction.Rollback();
                                     return null;
                                 }

                                 _unitOfWork.Save();
                             }


                         }
                         catch (Exception ex)
                         {
                             AddError($"{ex.Message} : {ex.InnerException}");
                             transaction.Rollback();
                             return null;
                         }
                     }
                 }

                 if (IsValid)
                     transaction.Commit();
             }

             return null;
         }*/

        public async Task<BusinessActivityTypeDto> SyncContractorInfo()
        {
            int batchSize = 1000;
            int skip = 0;
            bool hasMoreData = true;
            var contractors = _unitOfWork.ContractorRepository.AllAsQueryable.IsActive().Select(a => new { a.Id, a.Inn }).ToArray();
            while (hasMoreData)
            {
                var allContractor = contractors
                    .Skip(skip)
                    .Take(batchSize)
                    .ToList();

                if (!allContractor.Any())
                    break;
                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in allContractor)
                        {
                            var data = await _tadbirkorFundContractorService.GetByInn(item.Inn);

                            CombineStatuses(_tadbirkorFundContractorService);

                            if (HasErrors)
                            {
                                transaction.Rollback();
                                return null;
                            }

                            if (data.Id != 0)
                            {
                                var hasOldBusinessActivityType = _unitOfWork.Context.Set<BusinessActivityType>().Any(c => c.Contractor.Inn == item.Inn);
                                if (!hasOldBusinessActivityType)
                                {
                                    var bank = _unitOfWork.BankRepository.ByCode(data.BankCode);

                                    if (bank == null)
                                    {
                                        AddError($"{data.BankCode} bank tizimda mavjud emas");
                                        transaction.Rollback();
                                        return null;
                                    }

                                    int bankId = bank.Id;
                                    var businessSectorCategoryId = _unitOfWork.Context.Set<BusinessSectorCategory>().FirstOrDefault(a => a.Id == data.BusinessSectorId)?.Id;

                                    if (businessSectorCategoryId == data.BusinessSectorId)
                                    {
                                        var dlDto = new CreateBusinessActivityTypeDlDto
                                        {
                                            BankId = bankId,
                                            ContractorId = item.Id,
                                            BusinessCtorId = data.BusinessSectorId,
                                            BusinessCtorName = data.BusinessSectorName,
                                            FinancialHelpId = data.FinancialAssistanceId,
                                            FinancialHelp = data.FinancialAsistanceName,
                                            HelpAmount = data.AidAmount,
                                            NewEmployeesCount = data.NewJobPosition,
                                            RealEmployeesCount = data.RealJobPosition
                                        };

                                        foreach (var credit in data.Credits)
                                        {
                                            dlDto.Tables.Add(new BusinessActivityTypeTableDlDto
                                            {
                                                Amount = credit.Amount,
                                                CurrencyId = credit.CurrencyId,
                                                OwnerId = dlDto.ContractorId
                                            });
                                        }

                                        var dto = _repository.Create(dlDto);
                                        CombineStatuses(_repository);

                                        if (HasErrors)
                                        {
                                            transaction.Rollback();
                                            return null;
                                        }
                                        _unitOfWork.Save();
                                    }
                                    else if (businessSectorCategoryId != data.BusinessSectorId)
                                    {
                                        var dlDto = new CreateBusinessActivityTypeDlDto
                                        {
                                            BankId = bankId,
                                            ContractorId = item.Id,
                                            BusinessCtorId = 1622,
                                            BusinessCtorName = data.BusinessSectorName,
                                            FinancialHelpId = data.FinancialAssistanceId,
                                            FinancialHelp = data.FinancialAsistanceName,
                                            HelpAmount = data.AidAmount,
                                            NewEmployeesCount = data.NewJobPosition,
                                            RealEmployeesCount = data.RealJobPosition
                                        };

                                        foreach (var credit in data.Credits)
                                        {
                                            dlDto.Tables.Add(new BusinessActivityTypeTableDlDto
                                            {
                                                Amount = credit.Amount,
                                                CurrencyId = credit.CurrencyId,
                                                OwnerId = dlDto.ContractorId
                                            });
                                        }

                                        var dto = _repository.Create(dlDto);
                                        CombineStatuses(_repository);

                                        if (HasErrors)
                                        {
                                            transaction.Rollback();
                                            return null;
                                        }
                                        _unitOfWork.Save();
                                    }
                                }
                            }
                        }
                        if (IsValid)
                            transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        AddError($"{ex.Message} : {ex.InnerException}");
                    }
                    skip += batchSize;
                }
            }
            return null;
        }
    }
}
