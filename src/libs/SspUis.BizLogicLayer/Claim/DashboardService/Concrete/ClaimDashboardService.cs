using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SspUis.BizLogicLayer.Claim;

public class ClaimDashboardService : StatusGenericHandler, IClaimDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    public ClaimDashboardService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public ClaimDashTotalStatisticsDto GetClaimStatisticList(ClaimDashFilterOption options)
    {
        var claimApplication = this.ClaimApplications(options);
        var mediationPlan = this.MediationPlans(options);
        var mediation = this.Mediations(options);
        var totalClaimApplicationsCount = claimApplication.Count(c => ConstStatusParams.ClaimApplicationStatuses.Contains(c.Application.StatusId));
        var totalMediationPlanCount = mediationPlan.Count();
        var totalMediationsCount = mediation.Count();
        var totalNewApplicationsCount = claimApplication.Count(c => c.Application.StatusId == StatusIdConst.CREATED);

        return new ClaimDashTotalStatisticsDto()
        {
            TotalClaimApplicationsCount = totalClaimApplicationsCount,
            TotalMediationPlanCount = totalMediationPlanCount,
            TotalNewApplicationsCount = totalNewApplicationsCount,
            TotalMediationsCount = totalMediationsCount,
            TotalApplicationsCount = totalMediationsCount + totalMediationPlanCount + totalNewApplicationsCount + totalClaimApplicationsCount
        };
    }

    public List<ClaimApplicationTypeDto> GetClaimApplicationList(ClaimDashFilterOption options)
    {
        var types = _unitOfWork.Context.Set<ClaimApplicationType>()
            .Include(a => a.Translates).ToList();

        List<ClaimApplicationTypeDto> result = new();
        foreach (var type in types)
        {
            if (type.Id == ClaimApplicationTypeIdConst.APPLICATION_FOR_COURT)
                continue;

            var applications = ClaimApplications(new ClaimDashFilterOption
            {
                RegionId = options.RegionId,
                DistrictId = options.DistrictId,
                ClaimApplicationTypeId = type.Id
            });

            var dto = new ClaimApplicationTypeDto
            {
                ClaimApplicationTypeId = type.Id,
                ClaimApplicationType = type.Translates.AsQueryable().FirstOrDefault(ClaimApplicationTypeTranslate.GetExpr(
                    TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? type.FullName,
                ExpiredApplicationsCount = applications.Count(s => s.Application.ChangeLogs.Where(a => a.Application.Id == ApplicationTypeIdConst.CLAIM && a.DocId == s.Id
                    && a.StatusId == StatusIdConst.SENT).OrderBy(l => l.Id).LastOrDefault().DateAt.AddDays(7) <
                    s.Application.ChangeLogs.Where(c => c.Application.Id == ApplicationTypeIdConst.CLAIM && c.DocId == s.Id && c.StatusId == StatusIdConst.ACCEPTED).OrderBy(l => l.Id).LastOrDefault().DateAt),
                InTermApplicationsCount = applications.Count(s => s.Application.ChangeLogs.Where(a => a.Application.Id == ApplicationTypeIdConst.CLAIM && a.DocId == s.Id
                    && a.StatusId == StatusIdConst.SENT).OrderBy(l => l.Id).LastOrDefault().DateAt.AddDays(7) >=
                    s.Application.ChangeLogs.Where(c => c.Application.Id == ApplicationTypeIdConst.CLAIM && c.DocId == s.Id && c.StatusId == StatusIdConst.ACCEPTED)
                    .OrderBy(l => l.Id).LastOrDefault().DateAt),
                NonClosedApplicationsCount = applications.Count(s => s.Application.StatusId == StatusIdConst.SENT && s.Application.StatusId != StatusIdConst.ACCEPTED)
            };
            result.Add(dto); 
        }
        return result;
    }

    public List<ClaimDashDocsDto> GetApplicationForCourtList(ClaimDashFilterOption options)
    {
        var query = _unitOfWork.Context.Set<ApplicationForCourt>().Where(a => a.StatusId != StatusIdConst.DELETED);
        query = query.Where(a => ConstStatusParams.ApplicationForCourtStatuses.Contains(a.StatusId));

        var res = query
            .Select(a => new ClaimDashDocsDto { StatusId = a.StatusId, ApplicationsCount = 1 })
            .GroupBy(a => a.StatusId)
            .Select(a => new ClaimDashDocsDto
            {
                StatusId = a.Key,
                Status = query.FirstOrDefault(q => q.StatusId == a.Key).Status.Translates.AsQueryable()
                              .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                    ?? query.FirstOrDefault(q => q.StatusId == a.Key).Status.FullName,
                RegionId = options.RegionId,
                DistrictId = options.DistrictId,
                ApplicationsCount = a.Sum(b => b.ApplicationsCount)
            }).ToList();
        return AddNoValueSpecifStatus(ConstStatusParams.ApplicationForCourtStatuses, res, options).OrderBy(a => a.StatusId).ToList();
    }

    public List<MediatonResultTypeDto> GetMediationResultList(ClaimDashFilterOption options)
    {
        var query = this.Mediations(options);
        var result = query.Select(a => new MediatonResultTypeDto
        {
            MediationResultTypeId = a.MediationResultId,
            ApplicationsCount = 1
        })
        .GroupBy(a => a.MediationResultTypeId)
        .Select(a => new MediatonResultTypeDto
        {
            MediationResultTypeId = a.Key,
            MediationResultType = query.FirstOrDefault(q => q.MediationResultId == a.Key).MediationResult.Translates.AsQueryable()
                              .FirstOrDefault(MediationResultTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                    ?? query.FirstOrDefault(q => q.MediationResultId == a.Key).MediationResult.FullName,
            RegionId = options.RegionId,
            DistrictId = options.DistrictId,
            ApplicationsCount = a.Sum(b => b.ApplicationsCount)
        }).ToList();

        //return result;
        return AddNoValueSpecifResult(ConstStatusParams.ResultStatuses, result, options).ToList();
    }

    public List<ApplicationRate> GetApplicationRateList(ClaimDashRateFilterOption options)
    {
        var regions = _unitOfWork.Context.Set<Region>()
            .ToDictionary(ent => ent.Id, ent => ent);

        return options.TableId switch
        {
            TableIdConst.CLAIM__DOC_CLAIM_APPLICATION when options.IsNew => ByClaimApplication(options, regions, DistrictsByRegionId(options.RegionId)),
            TableIdConst.CLAIM__DOC_CLAIM_APPLICATION => ByClaimApplication(options, regions, DistrictsByRegionId(options.RegionId)),
            TableIdConst.CLAIM__DOC_MEDIATION_PLAN => ByMediationPlanApplication(options, regions, DistrictsByRegionId(options.RegionId)),
            TableIdConst.CLAIM__DOC_MEDIATION => ByMediationApplication(options, regions, DistrictsByRegionId(options.RegionId)),
            _ => null
        };
    }

    public List<ContractorTimeLineRate> GetContractorsRateList(ClaimContractorTimeLineOption options)
    {
        var query = ClaimApplications(new ClaimDashFilterOption
        {
            RegionId = options.RegionId,
            DistrictId = options.DistrictId,
            ClaimApplicationTypeId = options.ClaimApplicationTypeId
        });

        var result = query
            .Where(c => options.ByTimeLine ? 
                c.CreatedAt <= DateTime.Now.AddDays(-7) : c.CreatedAt <= DateTime.Now.AddDays(-30))
            .GroupBy(a => new 
            { 
                a.Application.ContractorId, 
                a.Application.Contractor.FullName, 
                a.Application.Contractor.RegionId, 
                a.Application.Contractor.DistrictId 
            })
            .Select(a => new ContractorTimeLineRate
            {
                ContractorId = a.Key.ContractorId.Value,
                Contractor = a.Key.FullName,
                RegionId = a.Key.RegionId,
                DistrictId = a.Key.DistrictId,
                ApplicationsCount = a.Count()
            })
            .OrderByDescending(a => a.ApplicationsCount)
            .Take(10)
            .ToList();

        return result;
    }

    private List<ApplicationRate> ByClaimApplication(
        ClaimDashRateFilterOption option,
        Dictionary<int, Region> regions,
        Dictionary<int, District> districts)
    {
        List<ApplicationRate> result = new();
        var query = _unitOfWork.Context.Set<ClaimApplication>()
            .Include(a => a.Application)
            .Include(a => a.Application.Region)
            .Include(a => a.Application.District)
            .Where(a => !option.IsNew || a.Application.StatusId == StatusIdConst.CREATED);

        if(option.RegionId.HasValue)
        {
            result = query.Where(a => a.Application.RegionId == option.RegionId)
                .Select(a => new ApplicationRate()
                {
                    DistrictId = a.Application.DistrictId,
                    DistrictOrderCode = a.Application.District.OrderCode,
                    Amount = 1
                })
                .GroupBy(a => new {a.DistrictId, a.DistrictOrderCode})
                .Select( a => new ApplicationRate()
                {
                    DistrictId = a.Key.DistrictId,
                    DistrictOrderCode = a.Key.DistrictOrderCode,
                    Name = districts[(int)a.Key.DistrictId].FullName,
                    Amount = a.Sum(b => b.Amount)
                })
                .OrderBy(a => a.DistrictOrderCode)
                .ToList();

            return AddNoValueSpecDistrictAndRegion(result, districts, null);
        }
        else
        {
            result = query
                .Select(a => new ApplicationRate()
                {
                    RegionId = a.Application.RegionId,
                    RegionOrderCode = a.Application.Region.OrderCode,
                    Amount = 1
                })
                .GroupBy(a => new {a.RegionId, a.RegionOrderCode})
                .Select(a => new ApplicationRate()
                {
                    RegionId = a.Key.RegionId,
                    RegionOrderCode = a.Key.RegionOrderCode,
                    Name = regions[(int)a.Key.RegionId].FullName,
                    Amount = a.Sum(b => b.Amount)
                })
                .OrderBy(a => a.RegionOrderCode)
                .ToList();

            return AddNoValueSpecDistrictAndRegion(result, null, regions);
        }
    }

    private List<ApplicationRate> ByMediationPlanApplication(
        ClaimDashRateFilterOption option,
        Dictionary<int, Region> regions,
        Dictionary<int, District> districts)
    {
        List<ApplicationRate> result = new();
        var query = _unitOfWork.Context.Set<MediationPlan>()
            .Include(a => a.Application)
            .Include(a => a.Application.District)
            .Include(a => a.Application.Region)
            .Include(a => a.Contractor)
            .Include(a => a.Contractor.District)
            .Include(a => a.Contractor.Region);

        if (option.RegionId.HasValue)
        {
            result = query.Where(a => a.ApplicationId != 0
                             ? (a.Application.RegionId == option.RegionId)
                             : a.Contractor.RegionId == option.RegionId)
                .Select(a => new ApplicationRate()
                {
                    DistrictId = a.Application != null ? a.Application.DistrictId : a.Contractor.DistrictId,
                    DistrictOrderCode = a.Application != null ? a.Application.District.OrderCode : a.Contractor.District.OrderCode,
                    Amount = 1
                })
                .GroupBy(a => new { a.DistrictId, a.DistrictOrderCode })
                .Select(a => new ApplicationRate()
                {
                    DistrictId = a.Key.DistrictId,
                    DistrictOrderCode = a.Key.DistrictOrderCode,
                    Name = districts[(int)a.Key.DistrictId].FullName,
                    Amount = a.Sum(b => b.Amount)
                })
                .OrderBy(a => a.DistrictOrderCode)
                .ToList();
            return AddNoValueSpecDistrictAndRegion(result, districts, null);
        }
        else
        {
            result = query.Select(a => new ApplicationRate()
            {
                RegionId = a.Application != null ? a.Application.RegionId : a.Contractor.RegionId,
                RegionOrderCode = a.Application != null ? a.Application.Region.OrderCode : a.Contractor.Region.OrderCode,
                Amount = 1
            })
            .GroupBy(a => new {a.RegionId, a.RegionOrderCode })
            .Select(a => new ApplicationRate()
            {
                RegionId = a.Key.RegionId,
                RegionOrderCode = a.Key.RegionOrderCode,
                Name = regions[(int)a.Key.RegionId].FullName,
                Amount = a.Sum(b => b.Amount)
            })
            .OrderBy(a => a.RegionOrderCode) 
            .ToList();

            return AddNoValueSpecDistrictAndRegion(result, null, regions);
        }
    }

    private List<ApplicationRate> ByMediationApplication(
        ClaimDashRateFilterOption option,
        Dictionary<int, Region> regions,
        Dictionary<int, District> districts)
    {
        List<ApplicationRate> result = new();
        var query = _unitOfWork.Context.Set<Mediation>()
            .Include(a => a.Contractor)
            .Include(a => a.Contractor.District)
            .Include(a => a.Contractor.Region);

        if (option.RegionId.HasValue)
        {
            result = query.Where(a => a.Contractor.RegionId == option.RegionId)
                .Select(a => new ApplicationRate()
                {
                    DistrictId = a.Contractor.DistrictId,
                    DistrictOrderCode = a.Contractor.District.OrderCode,
                    Amount = 1
                })
                .GroupBy(a => new { a.DistrictId, a.DistrictOrderCode })
                .Select(a => new ApplicationRate()
                {
                    DistrictId = a.Key.DistrictId,
                    DistrictOrderCode = a.Key.DistrictOrderCode,
                    Name = districts[(int)a.Key.DistrictId].FullName,
                    Amount = a.Sum(b => b.Amount)
                })
                .OrderBy(a => a.DistrictOrderCode)
                .ToList();

            return AddNoValueSpecDistrictAndRegion(result, districts, null);
        }
        else
        {
            result = query
                .Select(a => new ApplicationRate()
                {
                    RegionId = a.Contractor.RegionId,
                    RegionOrderCode = a.Contractor.Region.OrderCode,
                    Amount = 1
                })
                .GroupBy (a => new { a.RegionId, a.RegionOrderCode })
                .Select(a => new ApplicationRate()
                {
                    RegionId = a.Key.RegionId,
                    RegionOrderCode = a.Key.RegionOrderCode,
                    Name = regions[(int)a.Key.RegionId].FullName,
                    Amount = a.Sum(b => b.Amount)
                })
                .OrderBy(a => a.RegionOrderCode)
                .ToList();
            return AddNoValueSpecDistrictAndRegion(result, null, regions);
        }
    }

    private IQueryable<MediationPlan> MediationPlans(ClaimDashFilterOption options)
    {
        var query = _unitOfWork.Context.Set<MediationPlan>()
            .Include(m => m.Application)
            .Include(m => m.Contractor)
            .Include(m => m.Status)
            .Where(m => m.StatusId != StatusIdConst.DELETED);

        query = query.Where(a => (!options.RegionId.HasValue || a.Application != null ?
                                 (a.Application.RegionId == options.RegionId) : a.Contractor.RegionId == options.RegionId)
                                 && (!options.DistrictId.HasValue || a.Application != null ? 
                                 (a.Application.DistrictId == options.DistrictId) : a.Contractor.DistrictId == options.DistrictId));
        return query;
    }

    private IQueryable<ClaimApplication> ClaimApplications(ClaimDashFilterOption options)
    {
        var query = _unitOfWork.Context.Set<ClaimApplication>()
            .Include(a => a.Application)
            .Include(a => a.Application.Contractor)
            .Where(a => a.Application.StatusId != StatusIdConst.DELETED);

        query = query.Where(a => (!options.RegionId.HasValue || options.RegionId == a.Application.RegionId)
                                 && (!options.DistrictId.HasValue || options.DistrictId == a.Application.DistrictId));

        query = query.Where(a => !options.ClaimApplicationTypeId.HasValue || a.ClaimApplicationTypeId == options.ClaimApplicationTypeId);
        return query;
    }

    private IQueryable<Mediation> Mediations(ClaimDashFilterOption options)
    {
        var query = _unitOfWork.Context.Set<Mediation>()
            .Include(a => a.MediationResult)
            .Include(a => a.Contractor)
            .Include(a => a.Status)
            .Where(a => a.StatusId != StatusIdConst.DELETED);

        query = query.Where(a => (!options.RegionId.HasValue || options.RegionId == a.Contractor.RegionId)
                                 && (!options.DistrictId.HasValue || options.DistrictId == a.Contractor.DistrictId));
        return query;
    }

    private List<ClaimDashDocsDto> AddNoValueSpecifStatus(int[] statuses, List<ClaimDashDocsDto> res, ClaimDashFilterOption options)
    {
        var storageStatus = _unitOfWork.Context.Set<Status>()
            .Where(s => statuses.Contains(s.Id))
            .ToDictionary(s => s.Id);

        res.AddRange(storageStatus
            .Where(p => !res.GroupBy(r => r.StatusId)
                            .ToDictionary(g => g.Key)
                            .ContainsKey(p.Key))
            .Select(pair => new ClaimDashDocsDto
            {
                StatusId = pair.Key,
                Status = pair.Value.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    ?.TranslateText ?? pair.Value.FullName,
                RegionId = options.RegionId,
                DistrictId = options.DistrictId,
                ApplicationsCount = 0L
            }));

        return res;
    }

    private List<MediatonResultTypeDto> AddNoValueSpecifResult(int[] types, List<MediatonResultTypeDto> res, ClaimDashFilterOption options)
    {
        var storageStatus = _unitOfWork.Context.Set<MediationResult>()
            .Where(s => types.Contains(s.Id))
            .ToDictionary(s => s.Id);

        res.AddRange(storageStatus
            .Where(p => !res.GroupBy(r => r.MediationResultTypeId)
                            .ToDictionary(g => g.Key)
                            .ContainsKey(p.Key))
            .Select(pair => new MediatonResultTypeDto
            {
                MediationResultTypeId = pair.Key,
                MediationResultType = pair.Value.Translates.AsQueryable()
                    .FirstOrDefault(MediationResultTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    ?.TranslateText ?? pair.Value.FullName,
                RegionId = options.RegionId,
                DistrictId = options.DistrictId,
                ApplicationsCount = 0L
            }));

        return res;
    }

    private List<ApplicationRate> AddNoValueSpecDistrictAndRegion(List<ApplicationRate> res, 
        Dictionary<int, District> districts, Dictionary<int, 
        Region> regions)
    {
        if (districts is not null)
            res.AddRange(districts
                   .Where(d => !res.GroupBy(r => r.DistrictId)
                                   .ToDictionary(g => g.Key)
                                   .ContainsKey(d.Key))
                   .Select(d => new ApplicationRate
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
                   .Select(d => new ApplicationRate
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
            return _unitOfWork.Context.Set<District>()
                .Where(d => d.RegionId == regionId)
                .ToDictionary(ent => ent.Id, ent => ent);

        return null;
    }
}
