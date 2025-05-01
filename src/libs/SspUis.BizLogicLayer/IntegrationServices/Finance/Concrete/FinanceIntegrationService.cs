using Humanizer;
using Microsoft.AspNetCore.Mvc;
using OpenXmlPowerTools;
//using RestSharp.Extensions;
using SspUis.BizLogicLayer.BankServices;
using SspUis.BizLogicLayer.Hrm;
using SspUis.BizLogicLayer.NotificationServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Exapidata.Finance;
using SspUis.Integration.Finance.Models;
using SspUis.Integration.Finance.Services;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using WEBASE.OfficeTools.Extensions;
using WEBASE.Utility;

namespace SspUis.BizLogicLayer.IntegrationServices.Finance.Concrete;

public class FinanceIntegrationService : StatusGenericHandler, IFinanceIntegrationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFinanceService _financeService;

    public FinanceIntegrationService(IUnitOfWork unitOfWork,IFinanceService financeService)
    {
        _unitOfWork = unitOfWork;
        _financeService = financeService;
    }
    public HaveId<long> CreateFinancePymentData(FinPaymentDataDto dto)
    {

        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = new FinPaymentData()
                {
                    Acc = dto.Acc,
                    ClAcc = dto.ClAcc,
                    ClName = dto.ClName,
                    ClInn = dto.ClInn,
                    ClMfo = dto.ClMfo,
                    CoAcc = dto.CoAcc,
                    CoInn = dto.CoInn,
                    CoMfo = dto.CoMfo,
                    CoName = dto.CoName,
                    SumPay = dto.SumPay,
                    Purpose = dto.Purpose,
                    DocDate = dto.DocDate,
                    BankDate = dto.BankDate,
                    FinYear = dto.FinYear,
                    DocNumb = dto.DocNumb,
                    BankDocId = dto.BankDocId
                };
                _unitOfWork.Context.FinPaymentDatas.Add(entity);

                if (HasErrors)
                {
                    transaction.Rollback();
                    return null;
                }

                transaction.Commit();

                return HaveId.Create(entity.Id);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public PagedResult<FinancePayDocsByAcc> GetFinancePayDocsByAccList(FinancePayDocsByAccSortFilterPageOptions dto)
    {
        var query = _unitOfWork.Context.FinancePayDocsByAccs.AsQueryable();
        if(dto.FromDate.HasValue)
        {
            query = query.Where(s =>  s.BankDate >= dto.FromDate);
        }

        if (dto.ToDate.HasValue)
        {
            query = query.Where(s => s.BankDate <= dto.ToDate);
        }
        if (dto.AccountNumber is not null)
        {
            query = query.Where(s => s.Acc == dto.AccountNumber);
        }
        var result = query.AsPagedResult(dto);
        return result;
    }

    public PagedResult<FinancePaymentByInnDto> GetFinancePaymentByInn(FinancePaymentByInnDtoSortFilterPageOptions dto)
    {
        var query = _unitOfWork.Context.FinancePayDocsByAccs.AsQueryable();
        if(dto.Inn is not null)
        {
            query = query.Where(s => s.ClInn.Contains(dto.Inn.ToLower()));
        }
        return query.Select(s => new FinancePaymentByInnDto()
        {
            Id = s.Id,
            Inn = s.ClInn,
            SumPay = s.SumPay,
            Details = s.Purpose
        }).AsPagedResult(dto);
    }

    public PagedResult<FinPaymentData> GetFinancePaymentDataList(FinPaymentDataSortFilterPageOptions dto)
    {
        var result = _unitOfWork.Context.FinPaymentDatas.AsPagedResult(dto);
        return result;
    }

    public void SaveFinancePayDocsByAcc(List<GetPayDocsDto> data)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
             if(data is null)
            {
                transaction.Rollback();
            }
            else
            {
                foreach (var dto in data)
                {
                    try
                    {
                        if (_unitOfWork.Context.FinancePayDocsByAccs.Any(s => s.Id2 == dto.Id))
                        {
                            continue;
                        }
                        else
                        {
                            var entity = new FinancePayDocsByAcc()
                            {
                                Id2 = dto.Id,
                                Acc = dto.Acc,
                                ClAcc = dto.ClAcc,
                                ClName = dto.ClName,
                                ClInn = dto.ClInn,
                                ClMfo = dto.ClMfo,
                                CoAcc = dto.CoAcc,
                                CoInn = dto.CoInn,
                                CoMfo = dto.CoMfo,
                                CoName = dto.CoName,
                                SumPay = dto.SumPay,
                                Purpose = dto.Purpose,
                                DocDate = DateOnly.Parse(dto.DocDate.FormattedToString("yyyy-MM-dd")),
                                BankDate = DateOnly.Parse(dto.BankDate.FormattedToString("yyyy-MM-dd")),
                                FinYear = dto.FinYear,
                                DocNumb = dto.DocNumb,
                                BankDocId = dto.BankDocId
                            };
                            _unitOfWork.Context.FinancePayDocsByAccs.Add(entity);

                            if (HasErrors)
                            {
                                //transaction.Rollback();
                            }
                        }

                        _unitOfWork.Commit();
                    }
                    catch (Exception ex)
                    {
                        continue;
                        //transaction.Rollback();
                    }

                }



            }
                    
        }

    }
           
}
                
        
