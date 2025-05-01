using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using SspUis.BizLogicLayer.BankServices;
using SspUis.BizLogicLayer.Memship;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.BizLogicLayer.ReportServices.Main;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Report;
using SspUis.DataLayer.EfClasses.Report.Func;
using SspUis.Integration.BankCredit;
using SspUis.Integration.BankCredit.Models;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Partner;
public class PrtnReportsDashboardService : StatusGenericHandler, IPrtnReportsDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBankCreditService _bankCreditService;
    private readonly IReportService _reportService;
    public PrtnReportsDashboardService(
        IUnitOfWork unitOfWork,
        IBankCreditService bankCreditService,
        IReportService reportService)
    {
        _unitOfWork = unitOfWork;
        _bankCreditService = bankCreditService;
        _reportService = reportService;
    }

    /// <summary>
    /// STATISTIKA
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public async Task<PrtnReportsDashTotalStatisticsDto> GetPrtnStatisticsList(DashboardFilterOption options)
    {
        return new PrtnReportsDashTotalStatisticsDto
        {
            FormedCertificateCount = this.PrtnCertificates(options).Count(),
            BankLoanSeparatorsCount = await this.BankLoanSeparators(options),
            TookAdvantegTaxDeductoinCount = this.TaxDeductoin(options).Sum(c => c.TotalContractApplicationCount),
            CustomsPrivilegeBeneficiariesCount = this.CustomsPrivilege(options).Sum(c => (c.DevContractorCount + c.GrChanContractorCount)),
            TookAdvantegPrivilegeLeasingAssetsCount = this.StateAssets(options).Where(s => s.Application.StatusId != StatusIdConst.REJECTED).Count(),
            ReceivedBailEnteepreneurialFundCount = _unitOfWork.Context.Set<BusinessActivityType>().Count(x => x.FinancialHelpId == 1)
        };
    }
    /// <summary>
    /// BOJXONA IMTIYOZI
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public CustomsPrivilegeDto GetPrtnCustomsPrivilegeList(DashboardFilterOption options)
    {
        var data = this.CustomsPrivilege(options);

        return new CustomsPrivilegeDto
        {
            RegionId = options.RegionId,
            DistrictId = options.DistrictId,
            PrtnContractTypeId = options.PrtnContractTypeId,
            MfyId = options.MfyId,
            GrnChanContractorCount = data.Sum(c => (long)c.GrChanContractorCount),
            AppContractorCount = data.Sum(c => (long)c.AppContractorCount),
            RejContractorCount = data.Sum(c => (long)c.RejContractorCount),
            DevContractorCount = data.Sum(c => (long)c.DevContractorCount),
            SummaInstallment = data.Sum(c => (long)c.Sum)
        };
    }
    /// <summary>
    /// TADBIRKORLIK JAMG'ARMASI
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public List<PrtnEntepreneurialDashDto> GetPrtnEntrepreneurFoundationList(DashboardFilterOption options)
    {
        var types = _unitOfWork.Context.Set<PrtnContractType>()
            .Include(x => x.Translates)
            .ToList();

        List<PrtnEntepreneurialDashDto> res = new();
        foreach (var type in types)
        {
            PrtnEntepreneurialDashDto temp = new();

            var bus = this.BusinessActivityType(new DashboardFilterOption
            {
                DistrictId = options.DistrictId,
                RegionId = options.RegionId,
                PrtnContractTypeId = type.Id
            }).ToList();

            temp.PrtnContractTypeId = type.Id;
            temp.PrtnContractType = type.Translates.AsQueryable()
                 .FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                 ?.TranslateText ?? type.FullName;
            temp.Amount = bus.Sum(x => decimal.Parse(x.RealEmployeesCount) + decimal.Parse(x.NewEmployeesCount));
            temp.Summa = bus.Sum(x => (decimal)x.HelpAmount);

            res.Add(temp);
        }

        return res;
    }
    /// <summary>
    /// KREDITLAR
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public CreditsDto GetReportsCreditsList(DashboardFilterOption options)
    {
        var data = BankCredit(options);

        return new CreditsDto
        {
            RegionId = options.RegionId,
            DistrictId = options.DistrictId,
            PrtnContractTypeId = options.PrtnContractTypeId,
            MfyId = options.MfyId,
            SubmittedCount = data.Sum(x => x.SubmittedContractorCount),
            SubmittedSumma = data.Sum(x => x.SubmittedSum),
            RejectedApplicationCount = data.Sum(x => x.RejectedContractorCount),
            RejectedApplicationSumma = data.Sum(x => x.RejectedSum),
            CanceledApplicationCount = data.Sum(x => x.CanceledContractorCount),
            CanceledApplicationSumma = data.Sum(x => x.CanceledSum),
            ApprovedApplicationCount = data.Sum(x => x.ApprovedContractorCount),
            ApprovedApplicationSumma = data.Sum(x => x.ApprovedSum),
            LoanAllocationApplicationCount = data.Sum(x => x.IssuanceContractorCount),
            LoanAllocationApplicationSumma = data.Sum(x => x.IssuanceSum)
        };
    }
    /// <summary>
    /// DAVLAT AKTIVLARI
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public List<StateAssetDashDto> GetReportsStateAssetsList(DashboardFilterOption options)
    {
        var query = this.StateAssets(options);

        var res = query
           .Select(a => new StateAssetDashDto
           {
               StatusId = a.Application.StatusId,
               Count = 1
           })
           .GroupBy(a => a.StatusId)
           .Select(a => new StateAssetDashDto
           {
               StatusId = a.Key,
               Status = query.FirstOrDefault(q => q.Application.StatusId == a.Key).Application.Status.Translates.AsQueryable()
                             .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                             .TranslateText ?? query.FirstOrDefault(q => q.Application.StatusId == a.Key).Application.Status.FullName,
               RegionId = options.RegionId,
               DistrictId = options.DistrictId,
               PrtnContractTypeId = options.PrtnContractTypeId,
               Count = a.Sum(c => c.Count)
           })
           .ToList();

        var storageStatus = _unitOfWork.Context.Set<DataLayer.EfClasses.Status>()
            .Where(s => new int[] { StatusIdConst.ACCEPTED, StatusIdConst.SENT_FOR_REVIEW, StatusIdConst.REJECTED }.Contains(s.Id))
            .ToDictionary(s => s.Id);

        res.AddRange(storageStatus
            .Where(p => !res.GroupBy(r => r.StatusId)
                            .ToDictionary(g => g.Key)
                            .ContainsKey(p.Key))
            .Select(pair => new StateAssetDashDto
            {
                StatusId = pair.Key,
                Status = pair.Value.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    ?.TranslateText ?? pair.Value.FullName,
                RegionId = options.RegionId,
                DistrictId = options.DistrictId,
                PrtnContractTypeId = options.PrtnContractTypeId,
                Count = 0
            }));

        return res;
    }
    /// <summary>
    /// SOLIQ IMTIYOZI
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public List<PrtnTaxPrivilegeRepotsDashDto> GetPrtnTaxPrivilegeList(DashboardFilterOption options)
    {
        var types = _unitOfWork.Context.Set<PrtnContractType>()
            .Include(x => x.Translates)
            .ToList();

        List<PrtnTaxPrivilegeRepotsDashDto> res = new();
        foreach (var type in types)
        {
            var query = this.TaxDeductoin(new DashboardFilterOption
            {
                RegionId = options.RegionId,
                DistrictId = options.DistrictId,
                PrtnContractTypeId = type.Id,
                MfyId = null,
            });

            PrtnTaxPrivilegeRepotsDashDto temp = new();
            temp.PrtnContractTypeId = type.Id;
            temp.PrtnContractType = type.Translates.AsQueryable()
                 .FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                 ?.TranslateText ?? type.FullName;

            temp.PrtnLandTax.Add(new PrtnTaxCreditClomn()
            {
                Count = query.Sum(x => x.LandTaxCount),
                Summa = query.Sum(x => x.LandTaxSum),
            });

            temp.PrtnPropertyTax.Add(new PrtnTaxCreditClomn()
            {
                Count = query.Sum(x => x.PropertyTaxCount),
                Summa = query.Sum(x => x.PropertyTaxSum),
            });

            temp.PrtnSocial.Add(new PrtnTaxCreditClomn()
            {
                Count = query.Sum(x => x.SocialTaxCount),
                Summa = query.Sum(x => x.SocialTaxSum),
            });

            temp.PrtnIncomeTax.Add(new PrtnTaxCreditClomn()
            {
                Count = query.Sum(x => x.IncomeTaxCount),
                Summa = query.Sum(x => x.IncomeTaxSum),
            });

            res.Add(temp);
        }

        return res;
    }
    /// <summary>
    /// R A T E
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public List<PrtnReportsDashRateDto> GetPrtnRateList(RegionRateFilterOption options)
    {
        var regions = _unitOfWork.Context.Set<Region>().Include(x => x.Translates)
            .ToDictionary(ent => ent.Id);

        foreach (var reg in regions.Values)
        {
            reg.FullName = reg.Translates.AsQueryable().FirstOrDefault(RegionTranslate
                            .GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    ?.TranslateText ?? reg.FullName;
        }

        return options.TableId switch
        {
            TableIdConst.DOC_PRTN_CERTIFICATE => ByCertificate(options, regions, DistrictsByRegionId(options.RegionId), MfysByDistrictId(options.DistrictId)),
            TableIdConst.BANK_CREDIT_APPLICATION => ByCredit(options),
            TableIdConst.BUSINESS_ACTIVITY_TYPE => ByBail(options, regions, DistrictsByRegionId(options.RegionId)),
            TableIdConst.IMTIYOZ_DATA => ByTax(options, regions, DistrictsByRegionId(options.RegionId)),
            TableIdConst.BOJ_BOJXONA_IMTIYOZ => ByCustoms(options, regions, DistrictsByRegionId(options.RegionId)),
            _ => new()
        };
    }
    private List<PrtnReportsDashRateDto> ByCertificate(RegionRateFilterOption option, Dictionary<int, Region> regions, Dictionary<int, District> districts, Dictionary<long, Mfy> mfys)
    {
        List<PrtnReportsDashRateDto> res = new();
        if (option.DistrictId.HasValue)
        {
            res = this.PrtnCertificates(new DashboardFilterOption
            {
                PrtnContractTypeId = option.PrtnContractTypeId,
                DistrictId = option.DistrictId,
                RegionId = null,
                MfyId = null,
            })
            .Select(a => new PrtnReportsDashRateDto()
            {
                MfyOrderCode = a.PrtnContract.Application.PrtnApplication.Mfy.OrderCode,
                MfyId = (int)a.PrtnContract.Application.PrtnApplication.MfyId,
                Amount = 1M,
            })
            .GroupBy(a => new { a.MfyId, a.MfyOrderCode })
            .Select(a => new PrtnReportsDashRateDto()
            {
                MfyId = a.Key.MfyId,
                MfyOrderCode = a.Key.MfyOrderCode,
                Name = mfys[(int)a.Key.MfyId].FullName,
                Amount = a.Sum(b => b.Amount)
            })
            .OrderBy(a => a.MfyOrderCode)
            .ToList();

            return AddNoValue(res, null, null, mfys);
        }
        else if (option.RegionId.HasValue)
        {
            res = this.PrtnCertificates(new DashboardFilterOption
            {
                PrtnContractTypeId = option.PrtnContractTypeId,
                DistrictId = null,
                RegionId = option.RegionId,
                MfyId = null,
            })
            .Select(a => new PrtnReportsDashRateDto()
            {
                DistrictOrderCode = a.PrtnContract.Application.PrtnApplication.ChooseLocation
                    ? a.PrtnContract.Application.PrtnApplication.ChoosedDistrict.OrderCode
                    : a.PrtnContract.Application.District.OrderCode,
                DistrictId = a.PrtnContract.Application.PrtnApplication.ChooseLocation
                    ? a.PrtnContract.Application.PrtnApplication.ChoosedDistrict.Id
                    : a.PrtnContract.Application.DistrictId,
                Amount = 1M
            })
            .GroupBy(a => new { a.DistrictId, a.DistrictOrderCode })
            .Select(a => new PrtnReportsDashRateDto()
            {
                DistrictId = a.Key.DistrictId,
                DistrictOrderCode = a.Key.DistrictOrderCode,
                Name = districts[(int)a.Key.DistrictId].FullName,
                Amount = a.Sum(b => b.Amount)
            })
            .OrderBy(a => a.DistrictOrderCode)
            .ToList();

            return AddNoValue(res, districts, null, null);
        }
        else
        {
            res = this.PrtnCertificates(new DashboardFilterOption
            {
                PrtnContractTypeId = option.PrtnContractTypeId,
                DistrictId = null,
                RegionId = null,
                MfyId = null,
            })
            .Select(a => new PrtnReportsDashRateDto()
            {
                RegionOrderCode = a.PrtnContract.Application.PrtnApplication.ChooseLocation
                   ? a.PrtnContract.Application.PrtnApplication.ChoosedRegion.OrderCode
                   : a.PrtnContract.Application.Region.OrderCode,
                RegionId = a.PrtnContract.Application.PrtnApplication.ChooseLocation
                   ? a.PrtnContract.Application.PrtnApplication.ChoosedRegion.Id
                   : a.PrtnContract.Application.RegionId,
                Amount = 1M
            })
            .GroupBy(a => new { a.RegionId, a.RegionOrderCode })
            .Select(a => new PrtnReportsDashRateDto()
            {
                RegionOrderCode = a.Key.RegionOrderCode,
                RegionId = a.Key.RegionId,
                Name = regions[(int)a.Key.RegionId].FullName,
                Amount = a.Sum(b => b.Amount)
            })
            .OrderBy(a => a.RegionOrderCode)
            .ToList();
        }
        return AddNoValue(res, null, regions, null);
    }
    private List<PrtnReportsDashRateDto> ByCredit(RegionRateFilterOption option)
    {
        var regions = _unitOfWork.Context.Set<Region>().Include(x => x.Translates)
            .ToDictionary(ent => ent.RoamingCode);

        foreach (var reg in regions.Values)
        {
            reg.FullName = reg.Translates.AsQueryable().FirstOrDefault(RegionTranslate
                            .GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    ?.TranslateText ?? reg.FullName;
        }

        List<PrtnReportsDashRateDto> res = new();
        if (option.RegionId.HasValue || option.DistrictId.HasValue)
        {
            var districts = _unitOfWork.Context.Set<District>().Include(x => x.Translates)
                .Where(d => d.RegionId == option.RegionId && d.RoamingCode != null)
                .ToDictionary(ent => ent.RoamingCode);

            foreach (var dis in districts.Values)
            {
                dis.FullName = dis.Translates.AsQueryable().FirstOrDefault(DistrictTranslate
                                .GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                        ?.TranslateText ?? dis.FullName;
            }

            option.HasDistrict = true;
            option.RegionId = RegionConstForCredit.Dict[(int)option.RegionId];
            res.AddRange(this.GetBankCreditReport(option)
                .Select(x => new PrtnReportsDashRateDto
                {
                    TableId = option.TableId,
                    DistrictId = option.DistrictId,
                    Name = districts.ContainsKey(x.DistrictId.ToString()) ? districts[x.DistrictId.ToString()].FullName : string.Empty,
                    Amount = x.Application.IssuanceCount,
                    Summa = (decimal)x.Application.IssuanceSum
                }));

            return res;
        }

        option.HasRegion = true;
        res.AddRange(this.GetBankCreditReport(option)
        .Select(x => new PrtnReportsDashRateDto
        {
            TableId = option.TableId,
            RegionId = option.RegionId,
            Name = regions[x.RegionId.ToString()].FullName,
            Amount = x.Application.IssuanceCount,
            Summa = (decimal)x.Application.IssuanceSum
        }));

        return res;
    }
    private List<PrtnReportsDashRateDto> ByBail(RegionRateFilterOption option, Dictionary<int, Region> regions, Dictionary<int, District> districts)
    {
        List<PrtnReportsDashRateDto> res = new();
        if (option.RegionId.HasValue || option.DistrictId.HasValue)
        {
            res = this.BusinessActivityType(new DashboardFilterOption
            {
                PrtnContractTypeId = option.PrtnContractTypeId,
                DistrictId = null,
                RegionId = option.RegionId,
                MfyId = null,
            })
            .Select(a => new PrtnReportsDashRateDto()
            {
                DistrictOrderCode = a.Contractor.District.OrderCode,
                DistrictId = a.Contractor.DistrictId,
                Amount = Convert.ToInt32(a.RealEmployeesCount) + Convert.ToInt32(a.NewEmployeesCount),
                Summa = (decimal)a.HelpAmount
            })
            .GroupBy(a => new { a.DistrictId, a.DistrictOrderCode })
            .Select(a => new PrtnReportsDashRateDto()
            {
                TableId = option.TableId,
                DistrictId = a.Key.DistrictId,
                DistrictOrderCode = a.Key.DistrictOrderCode,
                Name = districts[(int)a.Key.DistrictId].FullName,
                Amount = a.Sum(b => b.Amount),
                Summa = a.Sum(b => b.Summa)
            })
            .OrderBy(a => a.DistrictOrderCode)
            .ToList();

            return AddNoValue(res, districts, null, null);
        }
        else
        {
            res = this.BusinessActivityType(new DashboardFilterOption
            {
                PrtnContractTypeId = option.PrtnContractTypeId,
                DistrictId = null,
                RegionId = null,
                MfyId = null,
            })
            .Select(a => new PrtnReportsDashRateDto()
            {
                RegionOrderCode = a.Contractor.Region.OrderCode,
                RegionId = a.Contractor.RegionId,
                Amount = Convert.ToInt32(a.RealEmployeesCount) + Convert.ToInt32(a.NewEmployeesCount),
                Summa = (decimal)a.HelpAmount
            })
            .GroupBy(a => new { a.RegionId, a.RegionOrderCode })
            .Select(a => new PrtnReportsDashRateDto()
            {
                TableId = option.TableId,
                RegionOrderCode = a.Key.RegionOrderCode,
                RegionId = a.Key.RegionId,
                Name = regions[(int)a.Key.RegionId].FullName,
                Amount = a.Sum(b => b.Amount),
                Summa = a.Sum(b => b.Summa)
            })
            .OrderBy(a => a.RegionOrderCode)
            .ToList();
        }

        return AddNoValue(res, null, regions, null);
    }
    private List<PrtnReportsDashRateDto> ByTax(RegionRateFilterOption option, Dictionary<int, Region> regions, Dictionary<int, District> districts)
    {
        List<PrtnReportsDashRateDto> res = new();
        if (option.RegionId.HasValue || option.DistrictId.HasValue)
        {
            res.AddRange(this.TaxDeductoin(new DashboardFilterOption
            {
                RegionId = option.RegionId,
                HasDistrict = true,
                PrtnContractTypeId = option.PrtnContractTypeId
            })
            .Select(x => new PrtnReportsDashRateDto
            {
                TableId = option.TableId,
                DistrictId = option.DistrictId,
                Name = districts[(int)x.DistrictId].FullName,
                Amount = x.ContractorLandTaxCount + x.ContractorPropertyTaxCount + x.ContractorSocialTaxCount + x.ContractorIncomeTaxCount,
                Summa = x.LandTaxSum + x.PropertyTaxSum + x.SocialTaxSum + x.IncomeTaxSum
            }));

            return res;
        }

        res.AddRange(this.TaxDeductoin(new DashboardFilterOption
        {
            HasRegion = true,
            PrtnContractTypeId = option.PrtnContractTypeId
        })
        .Select(x => new PrtnReportsDashRateDto
        {
            TableId = option.TableId,
            RegionId = option.RegionId,
            Name = regions[(int)x.RegionId].FullName,
            Amount = x.ContractorLandTaxCount + x.ContractorPropertyTaxCount + x.ContractorSocialTaxCount + x.ContractorIncomeTaxCount,
            Summa = x.LandTaxSum + x.PropertyTaxSum + x.SocialTaxSum + x.IncomeTaxSum
        }));

        return res;

    }
    private List<PrtnReportsDashRateDto> ByCustoms(RegionRateFilterOption option, Dictionary<int, Region> regions, Dictionary<int, District> districts)
    {
        List<PrtnReportsDashRateDto> res = new();
        if (option.RegionId.HasValue || option.DistrictId.HasValue)
        {
            res.AddRange(this.CustomsPrivilege(new DashboardFilterOption
            {
                RegionId = option.RegionId,
                HasDistrict = true,
                PrtnContractTypeId = option.PrtnContractTypeId
            })
            .Select(x => new PrtnReportsDashRateDto
            {
                TableId = option.TableId,
                DistrictId = option.DistrictId,
                Name = districts[(int)x.DistrictId].FullName,
                Amount = x.CertificateCount,
                Summa = x.Sum
            }));

            return res;
        }

        res.AddRange(this.CustomsPrivilege(new DashboardFilterOption
        {
            HasRegion = true,
            PrtnContractTypeId = option.PrtnContractTypeId
        })
        .Select(x => new PrtnReportsDashRateDto
        {
            TableId = option.TableId,
            RegionId = option.RegionId,
            Name = regions[(int)x.RegionId].FullName,
            Amount = x.CertificateCount,
            Summa = x.Sum
        }));

        return res;
    }
    /// <summary>
    /// H E L P E R
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    private IQueryable<PrtnCertificate> PrtnCertificates(DashboardFilterOption options)
    {
        var query = _unitOfWork.Context.Set<PrtnCertificate>()
            .Where(a => new int[] { StatusIdConst.FORMED }.Contains(a.StatusId))
            .Where(a => !options.PrtnContractTypeId.HasValue || options.PrtnContractTypeId == a.PrtnContract.PrtnContractTypeId);

        query = query.Where(a =>
                (!options.RegionId.HasValue
                || options.RegionId == (a.PrtnContract.Application.PrtnApplication.ChooseLocation
                    ? a.PrtnContract.Application.PrtnApplication.ChoosedRegionId
                    : a.PrtnContract.Application.RegionId))

             && (!options.DistrictId.HasValue
                || options.DistrictId == (a.PrtnContract.Application.PrtnApplication.ChooseLocation
                    ? a.PrtnContract.Application.PrtnApplication.ChoosedDistrictId
                    : a.PrtnContract.Application.DistrictId))

             && (!options.MfyId.HasValue || options.MfyId == (a.PrtnContract.Application.PrtnApplication.MfyId.Value)));

        return query;
    }
    private async Task<int> BankLoanSeparators(DashboardFilterOption options)
    {
        var data = await _bankCreditService.GetReport();
        var res = data.SortFilter(new BankCreditReportDtoFilter()
        {
            Year = null,
            BankMfo = null,
            RegionId = options.RegionId.HasValue ? options.RegionId : null,
            DistrictId = options.DistrictId.HasValue ? options.DistrictId : null,
        });

        if(res is null || res.Count == 0) return 0;

        return options.PrtnContractTypeId.HasValue
            ? options.PrtnContractTypeId switch
            {
                PrtnContractTypeIdConst._50_100 => res.Sum(c => c.ContractType1.IssuanceCount),
                PrtnContractTypeIdConst._101_200 => res.Sum(c => c.ContractType2.IssuanceCount),
                PrtnContractTypeIdConst._201__ => res.Sum(c => c.ContractType3.IssuanceCount),
                _ => 0
            }
            : res.Sum(c => c.Application.IssuanceCount);
    }
    private IQueryable<TaxCreditreportDto> TaxDeductoin(DashboardFilterOption options)
    {
        return _unitOfWork.Context.GetTaxCreditReport(
            null, null, options.RegionId, options.DistrictId, options.HasRegion, options.HasDistrict, false, null, options.PrtnContractTypeId, false, true, null);
    }
    private IQueryable<BojxonaImtiyozReportByContractorDto> CustomsPrivilege(DashboardFilterOption options)
    {
        return _unitOfWork.Context.BojxonaImtiyozReportByContractor(
            null, null, options.RegionId, options.DistrictId, options.HasRegion, options.HasDistrict, false, null, options.PrtnContractTypeId, false, true, null);
    }
    private IQueryable<StateAssetApplication> StateAssets(DashboardFilterOption options)
    {
        var query = _unitOfWork.Context.Set<StateAssetApplication>()
            .Where(a => new int[] { StatusIdConst.ACCEPTED, StatusIdConst.SENT_FOR_REVIEW, StatusIdConst.REJECTED }.Contains(a.Application.StatusId))
            .Where(a => !options.PrtnContractTypeId.HasValue || options.PrtnContractTypeId == a.Application.PrtnContract.PrtnContractTypeId);

        query = query.Where(a =>
                (!options.RegionId.HasValue
                || options.RegionId == (a.Application.PrtnApplication.ChooseLocation
                    ? a.Application.PrtnApplication.ChoosedRegionId
                    : a.Application.RegionId))

             && (!options.DistrictId.HasValue
                || options.DistrictId == (a.Application.PrtnApplication.ChooseLocation
                    ? a.Application.PrtnApplication.ChoosedDistrictId
                    : a.Application.DistrictId))

             && (!options.MfyId.HasValue || options.MfyId == (a.Application.PrtnApplication.MfyId.Value)));

        return query;
    }
    private IQueryable<BankCreditApplicationReportByRegionAndDistrictDto> BankCredit(DashboardFilterOption options)
    {
        return _unitOfWork.Context.GetBankCreditApplicationReportByRegionAndDistrict(
            null, null, options.RegionId, options.DistrictId, options.HasRegion, options.HasDistrict, false, options.PrtnContractTypeId, false, true, null);
    }
    private List<ContractorBankCreditReportResponse> GetBankCreditReport(DashboardFilterOption options)
    {
        var data = _reportService.GetBankCreditReport(new ContractorBankCreditReportDtoFilter
        {
            RegionId = options.RegionId,
            DistrictId = options.DistrictId,
            ByRegion = options.HasRegion != null ? (bool)options.HasRegion : false,
            ByDistrict = options.HasDistrict != null ? (bool)options.HasDistrict : false,
        }).Result;

        return data;
    }
    private Dictionary<int, District> DistrictsByRegionId(int? regionId)
    {
        if (regionId.HasValue)
        {
            var districts = _unitOfWork.Context.Set<District>().Include(x => x.Translates)
                .Where(d => d.RegionId == regionId)
                .ToDictionary(ent => ent.Id);

            foreach (var dis in districts.Values)
            {
                dis.FullName = dis.Translates.AsQueryable().FirstOrDefault(DistrictTranslate
                                .GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                        ?.TranslateText ?? dis.FullName;
            }

            return districts;
        }

        return null;
    }
    private Dictionary<long, Mfy> MfysByDistrictId(int? dictrictId)
    {
        if (dictrictId.HasValue)
            return _unitOfWork.Context.Set<Mfy>()
                .Where(d => d.DistrictId == dictrictId)
                .ToDictionary(ent => ent.Id);

        return null;
    }
    private List<PrtnReportsDashRateDto> AddNoValue(List<PrtnReportsDashRateDto> res, Dictionary<int, District> districts, Dictionary<int, Region> regions, Dictionary<long, Mfy> mfys)
    {
        if (mfys is not null)
            res.AddRange(mfys
                   .Where(d => !res.GroupBy(r => r.MfyId)
                                   .ToDictionary(g => g.Key)
                                   .ContainsKey((int)d.Key))
                   .Select(d => new PrtnReportsDashRateDto
                   {
                       MfyId = (int)d.Key,
                       MfyOrderCode = d.Value.OrderCode,
                       Name = d.Value.FullName,
                       Amount = 0M,
                       Summa = 0M
                   })
                   .OrderBy(d => d.MfyOrderCode)
                   .AsEnumerable());

        else if (districts is not null)
            res.AddRange(districts
                   .Where(d => !res.GroupBy(r => r.DistrictId)
                                   .ToDictionary(g => g.Key)
                                   .ContainsKey(d.Key))
                   .Select(d => new PrtnReportsDashRateDto
                   {
                       DistrictId = d.Key,
                       DistrictOrderCode = d.Value.OrderCode,
                       Name = districts[d.Key].Translates.AsQueryable()
                               .FirstOrDefault(DistrictTranslate.GetExpr(
                                   TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
                               ?? d.Value.FullName,
                       Amount = 0M,
                       Summa = 0M
                   })
                   .OrderBy(d => d.DistrictOrderCode)
                   .AsEnumerable());

        else if (regions is not null)
            res.AddRange(regions
                   .Where(d => !res.GroupBy(r => r.RegionId)
                                   .ToDictionary(g => g.Key)
                                   .ContainsKey(d.Key))
                   .Select(d => new PrtnReportsDashRateDto
                   {
                       RegionId = d.Key,
                       RegionOrderCode = d.Value.OrderCode,
                       Name = regions[d.Key].Translates.AsQueryable()
                               .FirstOrDefault(RegionTranslate.GetExpr(
                                   TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
                               ?? d.Value.FullName,
                       Amount = 0M,
                       Summa = 0M
                   })
                   .OrderBy(d => d.RegionOrderCode)
                   .AsEnumerable());

        return res;
    }
    private IQueryable<BusinessActivityType> BusinessActivityType(DashboardFilterOption options)
    {
        var certificates = this.PrtnCertificates(new DashboardFilterOption
        { 
            DistrictId = options.DistrictId,
            RegionId = options.RegionId,
            PrtnContractTypeId = options.PrtnContractTypeId,
        }).Select(c => c.ContractorId).Distinct().ToList();

        var bus = _unitOfWork.Context.Set<BusinessActivityType>()
               .Where(x => certificates.Contains(x.ContractorId));

        return bus;
    }
}