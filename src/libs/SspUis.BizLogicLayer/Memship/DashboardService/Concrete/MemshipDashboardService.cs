using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using StatusGeneric;
namespace SspUis.BizLogicLayer.Memship;
public class MemshipDashboardService : StatusGenericHandler, IMemshipDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    public MemshipDashboardService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public MemshipDashTotalStatisticsDto GetMemshipStatisticList(MemshipDashFilterOption options)
    {
        var application = this.Applications(options);
        var contract = this.Contracts(options);
        var certificate = this.Certificates(options);

        var res = new MemshipDashTotalStatisticsDto();
        res.TotalApplicationsAcceptedCount = application.Count(c => c.Application.StatusId == StatusIdConst.ACCEPTED);

            res.TotalApplicationsReviewCount = application.Count(x => x.Application.StatusId == StatusIdConst.CREATED
                    || x.Application.StatusId == StatusIdConst.SIGNING);

            res.TotalApplicationsCanceledCount = application.Count(c => c.Application.StatusId == StatusIdConst.REJECTED || c.Application.StatusId == StatusIdConst.CANCELED);

            res.TotalContractReviewCount = contract.Count(x => x.StatusId == StatusIdConst.CREATED
                    || x.StatusId == StatusIdConst.SIGNING || x.StatusId == StatusIdConst.SENT_FOR_REVIEW);

            res.TotalContractSingingCount = contract.Count(c => c.StatusId == StatusIdConst.SIGNING);

            res.TotalContractSignedCount = contract.Count(c => c.StatusId == StatusIdConst.SIGNED);

            res.TotalContractCanceledCount = contract.Count(c => c.StatusId == StatusIdConst.REJECTED);

            res.TotalCertificateFormedCount = certificate.Count(c => c.StatusId == StatusIdConst.FORMED);

		    res.Income = contract.SelectMany(c => c.PaymentOrders).Sum(a => (long)a.Amount);

		    res.Debt = 0;

        return res;
    }
    public List<MemshipDashDocsDto> GetMemshipApplicationList(MemshipDashFilterOption options)
    {
        var query = this.Applications(options)
            .Where(a => ConstStatusParams.ApplicationStatuses.Contains(a.Application.StatusId));

        var res = query
           .Select(a => new MemshipDashDocsDto
           {
               StatusId = a.Application.StatusId,
               DocumentCount = 1
           })
           .GroupBy(a => a.StatusId)
           .Select(a => new MemshipDashDocsDto
           {
               StatusId = a.Key,
               Status = query.FirstOrDefault(q => q.Application.StatusId == a.Key).Application.Status.Translates.AsQueryable()
                             .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                   ?? query.FirstOrDefault(q => q.Application.StatusId == a.Key).Application.Status.FullName,
               RegionId = options.RegionId,
               DistrictId = options.DistrictId,
               ContractorCategoryId = options.ContractorCategoryId,
               DocumentCount = a.Sum(c => c.DocumentCount)
           })
           .ToList();

        res.AddRange(new[]
        {
            new MemshipDashDocsDto
            {
                StatusId = StatusIdConst.SIGNED,
                Status = ConstStatusParams.SIGNED_CONTRACT_NAME,
                ContractorCategoryId = options.ContractorCategoryId,
                RegionId = options.RegionId,
                DistrictId = options.DistrictId,
                DocumentCount = query.Count(a => a.Application.MemshipContract.StatusId == StatusIdConst.SIGNED)
            },
            new MemshipDashDocsDto
            {
                StatusId = StatusIdConst.SIGNING,
                Status = ConstStatusParams.SIGNING_CONTRACT_NAME,
                ContractorCategoryId = options.ContractorCategoryId,
                RegionId = options.RegionId,
                DistrictId = options.DistrictId,
                DocumentCount = query.Count(x => x.Application.StatusId == StatusIdConst.CREATED
                    || x.Application.StatusId == StatusIdConst.SIGNING)
            },
            new MemshipDashDocsDto
            {
                StatusId = StatusIdConst.FORMED,
                Status = ConstStatusParams.FORMED_CERTIFICATE_NAME,
                ContractorCategoryId = options.ContractorCategoryId,
                RegionId = options.RegionId,
                DistrictId = options.DistrictId,
                DocumentCount = query.Count(a => a.Application.MemshipContract.Certificates.Any(c => c.StatusId == StatusIdConst.FORMED))
            }
        });

        return AddNoValueSpecifStatus(ConstStatusParams.ApplicationStatuses, res, options).OrderBy(a => a.StatusId).ToList();
    }
    public List<MemshipDashDocsDto> GetMemshipContractList(MemshipDashFilterOption options)
    {
        var query = this.Contracts(options)
            .Where(a => ConstStatusParams.ContractStatuses.Contains(a.StatusId));

        var res = query
            .Select(a => new MemshipDashDocsDto
            {
                StatusId = a.StatusId,
                DocumentCount = 1
            })
            .GroupBy(a => a.StatusId)
            .Select(a => new MemshipDashDocsDto
            {
                StatusId = a.Key,
                Status = query.FirstOrDefault(q => q.StatusId == a.Key).Status.Translates.AsQueryable()
                             .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                   ?? query.FirstOrDefault(q => q.StatusId == a.Key).Status.FullName,
                RegionId = options.RegionId,
                DistrictId = options.DistrictId,
                ContractorCategoryId = options.ContractorCategoryId,
                DocumentCount = a.Sum(c => c.DocumentCount),
            })
            .ToList();

        /// bizda bunday statuslar yo'qligi uchun default qo'shildi
        res.AddRange(new[]
        {
            new MemshipDashDocsDto
            {
                StatusId =  StatusIdConst.FORMED,
                Status = ConstStatusParams.FORMED_CERTIFICATE_NAME,
                RegionId = options.RegionId,
                DistrictId = options.DistrictId,
                ContractorCategoryId = options.ContractorCategoryId,
                DocumentCount = query.Count(a => a.Certificates.Any(c => c.StatusId == StatusIdConst.FORMED))
            },
            new MemshipDashDocsDto
            {
                StatusId = StatusIdConst.SIGNING,
                Status = ConstStatusParams.SIGNING_CONTRACT_NAME,
                RegionId = options.RegionId,
                DistrictId = options.DistrictId,
                ContractorCategoryId = options.ContractorCategoryId,
                DocumentCount = query.Count(x => x.StatusId == StatusIdConst.CREATED
                    || x.StatusId == StatusIdConst.SIGNING || x.StatusId == StatusIdConst.SENT_FOR_REVIEW)
            },
        });

        return AddNoValueSpecifStatus(ConstStatusParams.ContractStatuses, res, options).OrderBy(a => a.StatusId).ToList();
    }
    public List<MemshipDashDocsDto> GetMemshipCertificateList(MemshipDashFilterOption options)
    {
        var query = this.Certificates(options)
            .Where(a => ConstStatusParams.CertificateStatuses.Contains(a.StatusId));

        var res = query
           .Select(a => new MemshipDashDocsDto
           {
               StatusId = a.StatusId,
               DocumentCount = 1
           })
           .GroupBy(a => a.StatusId)
           .Select(a => new MemshipDashDocsDto
           {
               StatusId = a.Key,
               Status = query.FirstOrDefault(q => q.StatusId == a.Key).Status.Translates.AsQueryable()
                              .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                    ?? query.FirstOrDefault(q => q.StatusId == a.Key).Status.FullName,
               RegionId = options.RegionId,
               DistrictId = options.DistrictId,
               ContractorCategoryId = options.ContractorCategoryId,
               DocumentCount = a.Sum(b => b.DocumentCount)
           }).ToList();

        return AddNoValueSpecifStatus(ConstStatusParams.CertificateStatuses, res, options).OrderBy(a => a.StatusId).ToList();
    }
    public List<MemshipContractTypeDto> GetMemshipContractTypeList(MemshipDashFilterOption options)
    {
        var types = _unitOfWork.Context.Set<ContractorCategory>()
            .Include(a => a.Translates).ToList();

        List<MemshipContractTypeDto> res = new();
        foreach (var type in types)
        {
            if (type.Id == ContractorCategoryIdConst.MIKROFIRMA)
                continue;

            MemshipContractTypeDto dto = new();
            dto.ContractCategoryTypeId = type.Id;
            dto.ContractCategoryType = type.Translates.AsQueryable()
                .FirstOrDefault(ContractorCategoryTranslate.GetExpr(
                    TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                ?.TranslateText
             ?? type.FullName;

            dto.ApplicationCount = this.Applications(new MemshipDashFilterOption
            {
                DistrictId = options.DistrictId,
                RegionId = options.RegionId,
                ContractorCategoryId = type.Id,
                HasWeekly = options.HasWeekly,
                HasMonthly = options.HasMonthly,
                HasYearly = options.HasYearly,
            }).Count();

            dto.ContractCount = this.Contracts(new MemshipDashFilterOption
            {
                DistrictId = options.DistrictId,
                RegionId = options.RegionId,
                ContractorCategoryId = type.Id,
                HasWeekly = options.HasWeekly,
                HasMonthly = options.HasMonthly,
                HasYearly = options.HasYearly,
            }).Count();

            dto.CertificateCount = this.Certificates(new MemshipDashFilterOption
            {
                DistrictId = options.DistrictId,
                RegionId = options.RegionId,
                ContractorCategoryId = type.Id,
                HasWeekly = options.HasWeekly,
                HasMonthly = options.HasMonthly,
                HasYearly = options.HasYearly,
            }).Count();

            res.Add(dto);
        }

        return res;
    }


    #region C O N T R A C T   R A T E
    public List<MemshipContractRate> GetMemshipContractRateList(MemshipDashRateFilterOption options)
    {
        var regions = _unitOfWork.Context.Set<Region>()
            .Include(x => x.Translates)
            .ToDictionary(ent => ent.Id, ent => ent);

        foreach (var reg in regions)
        {
            reg.Value.FullName = reg.Value.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                ?.TranslateText
            ?? reg.Value.FullName;
        }

        return options.TableId switch
        {
            TableIdConst.MEMSHIP__DOC_MEMSHIP_APPLICATION when options.IsDept => ByMemshipContractDept(options, regions, DistrictsByRegionId(options.RegionId)),
            TableIdConst.DOC_MEMSHIP_PAYMENT_ORDER => ByMemshipContractIncome(options, regions, DistrictsByRegionId(options.RegionId)),
            TableIdConst.MEMSHIP__DOC_MEMSHIP_APPLICATION => ByMemshipApplication(options, regions, DistrictsByRegionId(options.RegionId)),
            TableIdConst.MEMSHIP__DOC_MEMSHIP_CONTRACT => ByMemshipContract(options, regions, DistrictsByRegionId(options.RegionId)),
            TableIdConst.MEMSHIP__DOC_MEMSHIP_CERTIFICATE => ByMemshipCertificate(options, regions, DistrictsByRegionId(options.RegionId)),
            _ => null
        };
    }
    private List<MemshipContractRate> ByMemshipApplication(
        MemshipDashRateFilterOption option,
        Dictionary<int, Region> regions,
        Dictionary<int, District> districts)
    {
        List<MemshipContractRate> res = new();

        var query = _unitOfWork.Context.Set<MemshipApplication>()
                    .Where(c => !option.ContractorCategoryId.HasValue || option.ContractorCategoryId == c.ContractorCategoryId);

        query = query.Where(a => !option.HasWeekly || a.Application.DocOn >= FilterTheDateTime.Weekly()[0]
                && a.Application.DocOn <= FilterTheDateTime.Weekly()[1]);

        query = query.Where(a => !option.HasMonthly || a.Application.DocOn >= FilterTheDateTime.Monthly()[0]
               && a.Application.DocOn <= FilterTheDateTime.Monthly()[1]);

        query = query.Where(a => !option.HasYearly || a.Application.DocOn >= FilterTheDateTime.Yearly()[0]
               && a.Application.DocOn <= FilterTheDateTime.Yearly()[1]);

        if (option.RegionId.HasValue)
        {
            res = query
                .Where(a => a.ChooseLocation
                    ? a.ChoosedRegionId == option.RegionId
                    : a.Application.RegionId == option.RegionId)
                .Select(a => new MemshipContractRate()
                {
                    DistrictOrderCode = a.ChooseLocation ? a.ChoosedDistrict.OrderCode : a.Application.District.OrderCode,
                    DistrictId = a.ChooseLocation ? a.ChoosedDistrict.Id : a.Application.DistrictId,
                    Amount = 1
                })
                .GroupBy(a => new { a.DistrictId, a.DistrictOrderCode })
                .Select(a => new MemshipContractRate()
                {
                    DistrictId = a.Key.DistrictId,
                    DistrictOrderCode = a.Key.DistrictOrderCode,
                    Name = districts[(int)a.Key.DistrictId].FullName,
                    Amount = a.Sum(b => b.Amount)
                })
                .OrderBy(a => a.DistrictOrderCode)
                .ToList();

            return AddNoValueSpecDistrictAndRegion(res, districts, null);
        }
        else
        {
            res = query
                .Select(a => new MemshipContractRate()
                {
                    RegionOrderCode = a.ChooseLocation ? a.ChoosedRegion.OrderCode : a.Application.Region.OrderCode,
                    RegionId = a.ChooseLocation ? a.ChoosedRegion.Id : a.Application.RegionId,
                    Amount = 1
                })
                .GroupBy(a => new { a.RegionId, a.RegionOrderCode })
                .Select(a => new MemshipContractRate()
                {
                    RegionOrderCode = a.Key.RegionOrderCode,
                    RegionId = a.Key.RegionId,
                    Name = regions[(int)a.Key.RegionId].FullName,
                    Amount = a.Sum(b => b.Amount)
                })
                .OrderBy(a => a.RegionOrderCode)
                .ToList();

            return AddNoValueSpecDistrictAndRegion(res, null, regions);
        }
    }
    private List<MemshipContractRate> ByMemshipContract(
        MemshipDashRateFilterOption option,
        Dictionary<int, Region> regions,
        Dictionary<int, District> districts)
    {
        List<MemshipContractRate> res = new();

        var query = _unitOfWork.Context.Set<MemshipContract>()
                    .Where(c => !option.ContractorCategoryId.HasValue || option.ContractorCategoryId == c.ContractorCategoryId);

        query = query.Where(a => !option.HasWeekly || a.DocOn >= FilterTheDateTime.Weekly()[0]
                && a.DocOn <= FilterTheDateTime.Weekly()[1]);

        query = query.Where(a => !option.HasMonthly || a.DocOn >= FilterTheDateTime.Monthly()[0]
               && a.DocOn <= FilterTheDateTime.Monthly()[1]);

        query = query.Where(a => !option.HasYearly || a.DocOn >= FilterTheDateTime.Yearly()[0]
               && a.DocOn <= FilterTheDateTime.Yearly()[1]);

        if (option.RegionId.HasValue)
        {
            res = query
                .Where(a => a.ApplicationId != null
                    ? (a.Application.MemshipApplication.ChooseLocation
                           ? a.Application.MemshipApplication.ChoosedRegionId
                           : a.Application.RegionId) == option.RegionId
                    : a.Contractor.RegionId == option.RegionId)
                .Select(a => new MemshipContractRate()
                {
                    DistrictOrderCode = a.ApplicationId != null
                        ? a.Application.MemshipApplication.ChooseLocation
                            ? a.Application.MemshipApplication.ChoosedDistrict.OrderCode
                            : a.Application.District.OrderCode
                        : a.Contractor.District.OrderCode,

                    DistrictId = a.ApplicationId != null
                        ? a.Application.MemshipApplication.ChooseLocation
                            ? a.Application.MemshipApplication.ChoosedDistrictId
                            : a.Application.DistrictId
                        : a.Contractor.DistrictId,

                    Amount = 1
                })
                .GroupBy(a => new { a.DistrictId, a.DistrictOrderCode })
                .Select(a => new MemshipContractRate()
                {
                    DistrictId = a.Key.DistrictId,
                    DistrictOrderCode = a.Key.DistrictOrderCode,
                    Name = districts[(int)a.Key.DistrictId].FullName,
                    Amount = a.Sum(b => b.Amount)
                })
                .OrderBy(a => a.DistrictOrderCode)
                .ToList();

            return AddNoValueSpecDistrictAndRegion(res, districts, null);
        }
        else
        {
            res = query
                .Select(a => new MemshipContractRate()
                {
                    RegionOrderCode = a.ApplicationId != null
                        ? a.Application.MemshipApplication.ChooseLocation
                            ? a.Application.MemshipApplication.ChoosedRegion.OrderCode
                            : a.Application.Region.OrderCode
                        : a.Contractor.Region.OrderCode,

                    RegionId = a.ApplicationId != null
                        ? a.Application.MemshipApplication.ChooseLocation
                            ? a.Application.MemshipApplication.ChoosedRegionId
                            : a.Application.RegionId
                        : a.Contractor.RegionId,

                    Amount = 1
                })
                .GroupBy(a => new { a.RegionId, a.RegionOrderCode })
                .Select(a => new MemshipContractRate()
                {
                    RegionOrderCode = a.Key.RegionOrderCode,
                    RegionId = a.Key.RegionId,
                    Name = regions[(int)a.Key.RegionId].FullName,
                    Amount = a.Sum(b => b.Amount)
                })
                .OrderBy(a => a.RegionOrderCode)
                .ToList();

            return AddNoValueSpecDistrictAndRegion(res, null, regions);
        }
    }
    private List<MemshipContractRate> ByMemshipCertificate(
        MemshipDashRateFilterOption option,
        Dictionary<int, Region> regions,
        Dictionary<int, District> districts)
    {
        List<MemshipContractRate> res = new();

        var query = _unitOfWork.Context.Set<MemshipCertificate>()
                    .Where(c => !option.ContractorCategoryId.HasValue ||
                                 option.ContractorCategoryId == c.MemshipContract.ContractorCategoryId);

        query = query.Where(a => !option.HasWeekly || a.DocOn >= FilterTheDateTime.Weekly()[0]
                && a.DocOn <= FilterTheDateTime.Weekly()[1]);

        query = query.Where(a => !option.HasMonthly || a.DocOn >= FilterTheDateTime.Monthly()[0]
               && a.DocOn <= FilterTheDateTime.Monthly()[1]);

        query = query.Where(a => !option.HasYearly || a.DocOn >= FilterTheDateTime.Yearly()[0]
               && a.DocOn <= FilterTheDateTime.Yearly()[1]);

        if (option.RegionId.HasValue)
        {
            res = query
                .Where(a => a.MemshipContract.ApplicationId != null
                    ? (a.MemshipContract.Application.MemshipApplication.ChooseLocation
                           ? a.MemshipContract.Application.MemshipApplication.ChoosedRegionId
                           : a.MemshipContract.Application.RegionId) == option.RegionId
                    : a.Contractor.RegionId == option.RegionId)
                .Select(a => new MemshipContractRate()
                {
                    DistrictOrderCode = a.MemshipContract.ApplicationId != null
                        ? a.MemshipContract.Application.MemshipApplication.ChooseLocation
                            ? a.MemshipContract.Application.MemshipApplication.ChoosedDistrict.OrderCode
                            : a.MemshipContract.Application.District.OrderCode
                        : a.Contractor.District.OrderCode,

                    DistrictId = a.MemshipContract.ApplicationId != null
                        ? a.MemshipContract.Application.MemshipApplication.ChooseLocation
                            ? a.MemshipContract.Application.MemshipApplication.ChoosedDistrictId
                            : a.MemshipContract.Application.DistrictId
                        : a.Contractor.DistrictId,

                    Amount = 1
                })
                .GroupBy(a => new { a.DistrictId, a.DistrictOrderCode })
                .Select(a => new MemshipContractRate()
                {
                    DistrictId = a.Key.DistrictId,
                    DistrictOrderCode = a.Key.DistrictOrderCode,
                    Name = districts[(int)a.Key.DistrictId].FullName,
                    Amount = a.Sum(b => b.Amount)
                })
                .OrderBy(a => a.DistrictOrderCode)
                .ToList();

            return AddNoValueSpecDistrictAndRegion(res, districts, null);
        }
        else
            res = query
                .Select(a => new MemshipContractRate()
                {
                    RegionOrderCode = a.MemshipContract.ApplicationId != null
                        ? a.MemshipContract.Application.MemshipApplication.ChooseLocation
                            ? a.MemshipContract.Application.MemshipApplication.ChoosedRegion.OrderCode
                            : a.MemshipContract.Application.Region.OrderCode
                        : a.Contractor.Region.OrderCode,

                    RegionId = a.MemshipContract.ApplicationId != null
                        ? a.MemshipContract.Application.MemshipApplication.ChooseLocation
                            ? a.MemshipContract.Application.MemshipApplication.ChoosedRegionId
                            : a.MemshipContract.Application.RegionId
                        : a.Contractor.RegionId,

                    Amount = 1
                })
                .GroupBy(a => new { a.RegionId, a.RegionOrderCode })
                .Select(a => new MemshipContractRate()
                {
                    RegionOrderCode = a.Key.RegionOrderCode,
                    RegionId = a.Key.RegionId,
                    Name = regions[(int)a.Key.RegionId].FullName,
                    Amount = a.Sum(b => b.Amount)
                })
                .OrderBy(a => a.RegionOrderCode)
                .ToList();

        return AddNoValueSpecDistrictAndRegion(res, null, regions);
    }
    private List<MemshipContractRate> ByMemshipContractIncome(
        MemshipDashRateFilterOption option,
        Dictionary<int, Region> regions,
        Dictionary<int, District> districts)
    {
        List<MemshipContractRate> res = new();

        var query = _unitOfWork.Context.Set<MemshipContract>()
                    .Where(c => !option.ContractorCategoryId.HasValue || option.ContractorCategoryId == c.ContractorCategoryId);

        query = query.Where(a => !option.HasWeekly || a.DocOn >= FilterTheDateTime.Weekly()[0]
                && a.DocOn <= FilterTheDateTime.Weekly()[1]);

        query = query.Where(a => !option.HasMonthly || a.DocOn >= FilterTheDateTime.Monthly()[0]
               && a.DocOn <= FilterTheDateTime.Monthly()[1]);

        query = query.Where(a => !option.HasYearly || a.DocOn >= FilterTheDateTime.Yearly()[0]
               && a.DocOn <= FilterTheDateTime.Yearly()[1]);

        if (option.RegionId.HasValue)
        {
            res = query
                .Where(a => a.ApplicationId != null
                    ? (a.Application.MemshipApplication.ChooseLocation
                           ? a.Application.MemshipApplication.ChoosedRegionId
                           : a.Application.RegionId) == option.RegionId
                    : a.Contractor.RegionId == option.RegionId)
                .Select(a => new MemshipContractRate()
                {
                    DistrictOrderCode = a.ApplicationId != null
                        ? a.Application.MemshipApplication.ChooseLocation
                            ? a.Application.MemshipApplication.ChoosedDistrict.OrderCode
                            : a.Application.District.OrderCode
                        : a.Contractor.District.OrderCode,

                    DistrictId = a.ApplicationId != null
                        ? a.Application.MemshipApplication.ChooseLocation
                            ? a.Application.MemshipApplication.ChoosedDistrictId
                            : a.Application.DistrictId
                        : a.Contractor.DistrictId,

                    Amount = a.PaymentOrders.Where(p => p.MemshipContractId == a.Id).Sum(p => p.Amount),
                })
                .GroupBy(a => new { a.DistrictId, a.DistrictOrderCode })
                .Select(a => new MemshipContractRate()
                {
                    DistrictId = a.Key.DistrictId,
                    DistrictOrderCode = a.Key.DistrictOrderCode,
                    Name = districts[(int)a.Key.DistrictId].FullName,
                    Amount = a.Sum(b => b.Amount)
                })
                .OrderBy(a => a.DistrictOrderCode)
                .ToList();

            return AddNoValueSpecDistrictAndRegion(res, districts, null);
        }
        else
            res = query
                .Select(a => new MemshipContractRate()
                {
                    RegionOrderCode = a.ApplicationId != null
                        ? a.Application.MemshipApplication.ChooseLocation
                            ? a.Application.MemshipApplication.ChoosedRegion.OrderCode
                            : a.Application.Region.OrderCode
                        : a.Contractor.Region.OrderCode,

                    RegionId = a.ApplicationId != null
                        ? a.Application.MemshipApplication.ChooseLocation
                            ? a.Application.MemshipApplication.ChoosedRegionId
                            : a.Application.RegionId
                        : a.Contractor.RegionId,

                    Amount = a.PaymentOrders.Where(p => p.MemshipContractId == a.Id).Sum(p => p.Amount),
                })
                .GroupBy(a => new { a.RegionId, a.RegionOrderCode })
                .Select(a => new MemshipContractRate()
                {
                    RegionOrderCode = a.Key.RegionOrderCode,
                    RegionId = a.Key.RegionId,
                    Name = regions[(int)a.Key.RegionId].FullName,
                    Amount = a.Sum(b => b.Amount)
                })
                .OrderBy(a => a.RegionOrderCode)
                .ToList();

        return AddNoValueSpecDistrictAndRegion(res, null, regions);
    }
    private List<MemshipContractRate> ByMemshipContractDept(
        MemshipDashRateFilterOption option,
        Dictionary<int, Region> regions,
        Dictionary<int, District> districts)
    {
        List<MemshipContractRate> res = new();

        var fixedMinimumValue = _unitOfWork.Context.Set<FixedMinimumValue>()
                    .OrderByDescending(f => f.DateOn)
                    .ThenBy(f => f.Id)
                    .FirstOrDefault();

        var query = _unitOfWork.Context.Set<MemshipContract>()
                    .Where(c => !option.ContractorCategoryId.HasValue || option.ContractorCategoryId == c.ContractorCategoryId);

        query = query.Where(a => !option.HasWeekly || a.DocOn >= FilterTheDateTime.Weekly()[0]
                && a.DocOn <= FilterTheDateTime.Weekly()[1]);

        query = query.Where(a => !option.HasMonthly || a.DocOn >= FilterTheDateTime.Monthly()[0]
               && a.DocOn <= FilterTheDateTime.Monthly()[1]);

        query = query.Where(a => !option.HasYearly || a.DocOn >= FilterTheDateTime.Yearly()[0]
               && a.DocOn <= FilterTheDateTime.Yearly()[1]);

        if (option.RegionId.HasValue)
        {
            res = query
                .Select(a => new MemshipContractRate()
                {
                    DistrictOrderCode = a.ApplicationId != null
                        ? a.Application.MemshipApplication.ChooseLocation
                            ? a.Application.MemshipApplication.ChoosedDistrict.OrderCode
                            : a.Application.District.OrderCode
                        : a.Contractor.District.OrderCode,

                    DistrictId = a.ApplicationId != null
                        ? a.Application.MemshipApplication.ChooseLocation
                            ? a.Application.MemshipApplication.ChoosedDistrictId
                            : a.Application.DistrictId
                        : a.Contractor.DistrictId,

                    Amount = (a.BaseFixedMinimumValue * (fixedMinimumValue != null ? fixedMinimumValue.FixedValue : 1)) 
                        - a.PaymentOrders.Where(p => p.MemshipContractId == a.Id).Sum(p => p.Amount),
                })
                .GroupBy(a => new { a.DistrictId, a.DistrictOrderCode })
                .Select(a => new MemshipContractRate()
                {
                    DistrictId = a.Key.DistrictId,
                    DistrictOrderCode = a.Key.DistrictOrderCode,
                    Name = districts[(int)a.Key.DistrictId].FullName,
                    Amount = a.Sum(b => b.Amount)
                })
                .OrderBy(a => a.DistrictOrderCode)
                .ToList();

            return AddNoValueSpecDistrictAndRegion(res, districts, null);
        }
        else
            res = query
                .Select(a => new MemshipContractRate()
                {
                    RegionOrderCode = a.ApplicationId != null
                        ? a.Application.MemshipApplication.ChooseLocation
                            ? a.Application.MemshipApplication.ChoosedRegion.OrderCode
                            : a.Application.Region.OrderCode
                        : a.Contractor.Region.OrderCode,

                    RegionId = a.ApplicationId != null
                        ? a.Application.MemshipApplication.ChooseLocation
                            ? a.Application.MemshipApplication.ChoosedRegionId
                            : a.Application.RegionId
                        : a.Contractor.RegionId,

                    Amount = (a.BaseFixedMinimumValue * (fixedMinimumValue != null ? fixedMinimumValue.FixedValue : 1))
                        - a.PaymentOrders.Where(p => p.MemshipContractId == a.Id).Sum(p => p.Amount),
                })
                .GroupBy(a => new { a.RegionId, a.RegionOrderCode })
                .Select(a => new MemshipContractRate()
                {
                    RegionOrderCode = a.Key.RegionOrderCode,
                    RegionId = a.Key.RegionId,
                    Name = regions[(int)a.Key.RegionId].FullName,
                    Amount = a.Sum(b => b.Amount)
                })
                .OrderBy(a => a.RegionOrderCode)
                .ToList();

        return res;
    }
    #endregion

    #region H E L P E R
    private IQueryable<MemshipApplication> Applications(MemshipDashFilterOption options)
    {
        var query = _unitOfWork.Context.Set<MemshipApplication>()
           .Where(a => a.Application.StatusId != StatusIdConst.DELETED);

        query = query.Where(a =>
                (!options.ContractorCategoryId.HasValue
                    || (options.ContractorCategoryId == ContractorCategoryIdConst.KICHIK_KORXONA
                         ? (a.ContractorCategoryId == ContractorCategoryIdConst.KICHIK_KORXONA || a.ContractorCategoryId == ContractorCategoryIdConst.MIKROFIRMA)
                         : a.ContractorCategoryId == options.ContractorCategoryId)));

        query = query.Where(a => !options.RegionId.HasValue
                || options.RegionId == (a.ChooseLocation ? a.ChoosedRegionId : a.Application.RegionId));

        query = query.Where(a => !options.DistrictId.HasValue
                || options.DistrictId == (a.ChooseLocation ? a.ChoosedDistrictId : a.Application.DistrictId));

        query = query.Where(a => !options.HasWeekly || a.Application.DocOn >= FilterTheDateTime.Weekly()[0]
                && a.Application.DocOn <= FilterTheDateTime.Weekly()[1]);

         query = query.Where(a => !options.HasMonthly || a.Application.DocOn >= FilterTheDateTime.Monthly()[0]
                && a.Application.DocOn <= FilterTheDateTime.Monthly()[1]);

         query = query.Where(a => !options.HasYearly || a.Application.DocOn >= FilterTheDateTime.Yearly()[0]
                && a.Application.DocOn <= FilterTheDateTime.Yearly()[1]);

        return query;
    }
    private IQueryable<MemshipContract> Contracts(MemshipDashFilterOption options)
    {
        var query = _unitOfWork.Context.Set<MemshipContract>()
            .Include(c => c.Status)
            .Include(c => c.Contractor)
            .Include(c => c.PaymentOrders)
            .Where(a => a.StatusId != StatusIdConst.DELETED);

        query = query.Where(a =>
                (!options.ContractorCategoryId.HasValue
                    || (options.ContractorCategoryId == ContractorCategoryIdConst.KICHIK_KORXONA
                         ? (a.ContractorCategoryId == ContractorCategoryIdConst.KICHIK_KORXONA || a.ContractorCategoryId == ContractorCategoryIdConst.MIKROFIRMA)
                         : a.ContractorCategoryId == options.ContractorCategoryId))

             && (!options.RegionId.HasValue ||
                   (a.ApplicationId != null
                                     ? (a.Application.MemshipApplication.ChooseLocation
                                            ? a.Application.MemshipApplication.ChoosedRegionId
                                            : a.Application.RegionId) == options.RegionId
                                     : a.Contractor.RegionId == options.RegionId))

             && (!options.DistrictId.HasValue || (a.ApplicationId != null
                                     ? (a.Application.MemshipApplication.ChooseLocation
                                            ? a.Application.MemshipApplication.ChoosedDistrictId
                                            : a.Application.DistrictId) == options.DistrictId
                                     : a.Contractor.DistrictId == options.DistrictId)));

        query = query.Where(a => !options.HasWeekly || a.DocOn >= FilterTheDateTime.Weekly()[0]
                && a.DocOn <= FilterTheDateTime.Weekly()[1]);

        query = query.Where(a => !options.HasMonthly || a.DocOn >= FilterTheDateTime.Monthly()[0]
               && a.DocOn <= FilterTheDateTime.Monthly()[1]);

        query = query.Where(a => !options.HasYearly || a.DocOn >= FilterTheDateTime.Yearly()[0]
               && a.DocOn <= FilterTheDateTime.Yearly()[1]);

        return query;
    }
    private IQueryable<MemshipCertificate> Certificates(MemshipDashFilterOption options)
    {
        var query = _unitOfWork.Context.Set<MemshipCertificate>()
            .Include(c => c.Status)
            .Include(c => c.Contractor)
            .Include(c => c.MemshipContract)
            .Where(a => a.StatusId != StatusIdConst.DELETED);

        query = query.Where(a =>
                (!options.ContractorCategoryId.HasValue
                    || (options.ContractorCategoryId == ContractorCategoryIdConst.KICHIK_KORXONA
                         ? (a.MemshipContract.ContractorCategoryId == ContractorCategoryIdConst.KICHIK_KORXONA
                                || a.MemshipContract.ContractorCategoryId == ContractorCategoryIdConst.MIKROFIRMA)
                         : a.MemshipContract.ContractorCategoryId == options.ContractorCategoryId))

             && (!options.RegionId.HasValue ||
                   (a.MemshipContract.ApplicationId != null
                                     ? (a.MemshipContract.Application.MemshipApplication.ChooseLocation
                                            ? a.MemshipContract.Application.MemshipApplication.ChoosedRegionId
                                            : a.MemshipContract.Application.RegionId) == options.RegionId
                                     : a.Contractor.RegionId == options.RegionId))

             && (!options.DistrictId.HasValue || (a.MemshipContract.ApplicationId != null
                                     ? (a.MemshipContract.Application.MemshipApplication.ChooseLocation
                                            ? a.MemshipContract.Application.MemshipApplication.ChoosedDistrictId
                                            : a.MemshipContract.Application.DistrictId) == options.DistrictId
                                     : a.Contractor.DistrictId == options.DistrictId)));

        query = query.Where(a => !options.HasWeekly || a.DocOn >= FilterTheDateTime.Weekly()[0]
                && a.DocOn <= FilterTheDateTime.Weekly()[1]);

        query = query.Where(a => !options.HasMonthly || a.DocOn >= FilterTheDateTime.Monthly()[0]
               && a.DocOn <= FilterTheDateTime.Monthly()[1]);

        query = query.Where(a => !options.HasYearly || a.DocOn >= FilterTheDateTime.Yearly()[0]
               && a.DocOn <= FilterTheDateTime.Yearly()[1]);

        return query;
    }
    private List<MemshipDashDocsDto> AddNoValueSpecifStatus(int[] statuses, List<MemshipDashDocsDto> res, MemshipDashFilterOption options)
    {
        var storageStatus = _unitOfWork.Context.Set<Status>()
            .Where(s => statuses.Contains(s.Id))
            .ToDictionary(s => s.Id);

        res.AddRange(storageStatus
            .Where(p => !res.GroupBy(r => r.StatusId)
                            .ToDictionary(g => g.Key)
                            .ContainsKey(p.Key))
            .Select(pair => new MemshipDashDocsDto
            {
                StatusId = pair.Key,
                Status = pair.Value.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    ?.TranslateText ?? pair.Value.FullName,
                RegionId = options.RegionId,
                DistrictId = options.DistrictId,
                ContractorCategoryId = options.ContractorCategoryId,
                DocumentCount = 0L
            }));

        return res;
    }
    private List<MemshipContractRate> AddNoValueSpecDistrictAndRegion(List<MemshipContractRate> res, Dictionary<int, District> districts, Dictionary<int, Region> regions)
    {
        if (districts is not null)
            res.AddRange(districts
                   .Where(d => !res.GroupBy(r => r.DistrictId)
                                   .ToDictionary(g => g.Key)
                                   .ContainsKey(d.Key))
                   .Select(d => new MemshipContractRate
                   {
                       DistrictId = d.Key,
                       DistrictOrderCode = d.Value.OrderCode,
                       Name = districts[d.Key].Translates.AsQueryable()
                               .FirstOrDefault(DistrictTranslate.GetExpr(
                                   TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
                               ?? d.Value.FullName,
                       Amount = 0M
                   })
                   .OrderBy(d => d.DistrictOrderCode)
                   .AsEnumerable());

        else if (regions is not null)
            res.AddRange(regions
                   .Where(d => !res.GroupBy(r => r.RegionId)
                                   .ToDictionary(g => g.Key)
                                   .ContainsKey(d.Key))
                   .Select(d => new MemshipContractRate
                   {
                       RegionId = d.Key,
                       RegionOrderCode = d.Value.OrderCode,
                       Name = regions[d.Key].Translates.AsQueryable()
                               .FirstOrDefault(RegionTranslate.GetExpr(
                                   TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
                               ?? d.Value.FullName,
                       Amount = 0M
                   })
                   .OrderBy(d => d.RegionOrderCode)
                   .AsEnumerable());

        return res;
    }
    private Dictionary<int, District> DistrictsByRegionId(int? regionId)
    {
        if (regionId.HasValue)
        {
            var districts = _unitOfWork.Context.Set<District>()
                .Where(d => d.RegionId == regionId)
                .ToDictionary(ent => ent.Id, ent => ent);

            foreach (var dis in districts)
            {
                dis.Value.FullName = dis.Value.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    ?.TranslateText
                ?? dis.Value.FullName;
            }

            return districts;
        }

        return null;
    }
    #endregion
}