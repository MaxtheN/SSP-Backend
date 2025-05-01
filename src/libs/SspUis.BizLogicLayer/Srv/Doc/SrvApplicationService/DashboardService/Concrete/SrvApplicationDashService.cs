using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.Srv.Doc;
using SspUis.BizLogicLayer.Srv.Doc.SrvApplicationService.DashboardService;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SspUis.BizLogicLayer;

public class SrvApplicationDashService : StatusGenericHandler, ISrvApplicationDashService
{
    private readonly IUnitOfWork _unitOfWork;
    public SrvApplicationDashService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    private List<SrvApplicationTypeRateDto> AddNoValueSpecDistrictAndRegion(List<SrvApplicationTypeRateDto> res, SrvApplicationTypeCostFilter filter)
    {
        var regions = _unitOfWork.Context.Set<Region>()
                                          .ToDictionary(ent => ent.Id, ent => ent);

        if (filter.RegionId.HasValue)
        {
            var districts = _unitOfWork.Context.Set<District>().Where(a => a.RegionId == filter.RegionId)
                                                                                   .ToDictionary(ent => ent.Id, ent => ent);
            res.AddRange(districts
                      .Where(d => !res.GroupBy(r => r.DistrictId)
                                      .ToDictionary(g => g.Key)
                                      .ContainsKey(d.Key))
                      .Select(d => new SrvApplicationTypeRateDto
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
        }

        else
        {
            res.AddRange(regions
                   .Where(d => !res.GroupBy(r => r.RegionId)
                                   .ToDictionary(g => g.Key)
                                   .ContainsKey(d.Key))
                   .Select(d => new SrvApplicationTypeRateDto
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
        }
        return res;
    }

    public List<SrvApplicationTypeRateDto> GetSrvApplicationRate(SrvApplicationTypeCostFilter filter)
    {
        var regions = _unitOfWork.Context.Set<Region>().ToDictionary(ent => ent.Id, ent => ent);
        var districts = _unitOfWork.Context.Set<District>().ToDictionary(ent => ent.Id, ent => ent);

        IQueryable<Application> query;

        if (filter.StatusId.HasValue && filter.StatusId == 0)
        {
             query = _unitOfWork.Context.Set<ServiceContract>()
                                        .Where(a => a.Application.StatusId != StatusIdConst.DELETED &&
                                                   (!filter.RegionId.HasValue || a.Application.RegionId == filter.RegionId))
                                        .Select(a => a.Application);
        }
        else if (filter.StatusId.HasValue && filter.StatusId == 1)
        {
            query = _unitOfWork.Context.Set<ServiceDeed>()
                                        .Where(a => a.Application.StatusId != StatusIdConst.DELETED &&
                                                   (!filter.RegionId.HasValue || a.Application.RegionId == filter.RegionId))
                                        .Select(a => a.Application);
        }
        else
        {
            var query1 = _unitOfWork.Context.Set<ServiceApplication>()
                            .Where(a => a.Application.StatusId != StatusIdConst.DELETED &&
                                       (!filter.RegionId.HasValue || a.Application.RegionId == filter.RegionId));

             query1 = query1.Where(a =>
                            (!filter.IsFree.HasValue || a.IsFree) &&
                            (!filter.StatusId.HasValue || a.Application.StatusId == filter.StatusId) &&
                            (!filter.RegionId.HasValue || a.Application.RegionId == filter.RegionId));

            List<SrvApplicationTypeRateDto> result1 = query1.Select(a => new SrvApplicationTypeRateDto
            {
                RegionId = a.Application.RegionId,
                Region = a.Application.RegionName,
                RegionOrderCode = a.Application.Region.OrderCode,
                DistrictId = a.Application.DistrictId,
                District = a.Application.DistrictName,
                DistrictOrderCode = a.Application.District.OrderCode,
                Amount = 1
            }).GroupBy(a => new
            {
                Id = filter.RegionId.HasValue ? a.DistrictId : a.RegionId,
                OrderCode = filter.RegionId.HasValue ? a.DistrictOrderCode : a.RegionOrderCode
            })
                                    .Select(g => new SrvApplicationTypeRateDto
                                    {
                                        RegionId = !filter.RegionId.HasValue ? g.Key.Id : null,
                                        DistrictId = filter.RegionId.HasValue ? g.Key.Id : null,
                                        Name = filter.RegionId.HasValue
                                                     ? (g.Key.Id.HasValue && districts.ContainsKey(g.Key.Id.Value) ? districts[g.Key.Id.Value].FullName : "Noma'lum tuman")
                                                     : (g.Key.Id.HasValue && regions.ContainsKey(g.Key.Id.Value) ? regions[g.Key.Id.Value].FullName : "Noma'lum hudud"),
                                        Amount = g.Sum(x => x.Amount)
                                    }).ToList(); ;

            return AddNoValueSpecDistrictAndRegion(result1, filter);

        }

        var result = query.Select(a => new SrvApplicationTypeRateDto
        {
            RegionId = a.RegionId,
            Region = a.RegionName,
            RegionOrderCode = a.Region.OrderCode,
            DistrictId = a.DistrictId,
            District = a.DistrictName,
            DistrictOrderCode = a.District.OrderCode,
            Amount = 1
        })
        .GroupBy(a => new
        {
            Id = filter.RegionId.HasValue ? a.DistrictId : a.RegionId,
            OrderCode = filter.RegionId.HasValue ? a.DistrictOrderCode : a.RegionOrderCode
        })
        .Select(g => new SrvApplicationTypeRateDto
        {
            RegionId = !filter.RegionId.HasValue ? g.Key.Id : null,
            DistrictId = filter.RegionId.HasValue ? g.Key.Id : null,
            Name = filter.RegionId.HasValue
                       ? (g.Key.Id.HasValue && districts.ContainsKey(g.Key.Id.Value) ? districts[g.Key.Id.Value].FullName : "Noma'lum tuman")
                       : (g.Key.Id.HasValue && regions.ContainsKey(g.Key.Id.Value) ? regions[g.Key.Id.Value].FullName : "Noma'lum hudud"),
            Amount = g.Sum(x => x.Amount)
        }).ToList();

        return AddNoValueSpecDistrictAndRegion(result, filter);
    }

    public SrvApplicationTypeDashDto GetSrvApplicationTypeDash(SrvDashboardFilterOption filter)
    {
        var application = _unitOfWork.Context.Set<ServiceApplication>().Where(a => a.Application.StatusId == StatusIdConst.ACCEPTED);
        var contract = _unitOfWork.Context.Set<ServiceContract>().Where(a => a.StatusId == StatusIdConst.SIGNED);
        var deed = _unitOfWork.Context.Set<ServiceDeed>().Where(x => x.StatusId != StatusIdConst.DELETED);

        #region application filter
        application = application.Where(x => !filter.HasWeekly || x.Application.DocOn >= FilterTheDateTime.Weekly()[0]
                  && x.Application.DocOn <= FilterTheDateTime.Weekly()[1]);

        application = application.Where(x => !filter.HasMonthly || x.Application.DocOn >= FilterTheDateTime.Monthly()[0]
                    && x.Application.DocOn <= FilterTheDateTime.Monthly()[1]);

        application = application.Where(x => !filter.HasYearly || x.Application.DocOn >= FilterTheDateTime.Yearly()[0]
                    && x.Application.DocOn <= FilterTheDateTime.Yearly()[1]);

        application = application.Where(x => !filter.RegionId.HasValue || x.Application.RegionId == filter.RegionId.Value);
        application = application.Where(x => !filter.DistrictId.HasValue || x.Application.DistrictId == filter.DistrictId);
        #endregion

        #region contract filter
        contract = contract.Where(x => !filter.HasWeekly || x.DocOn >= FilterTheDateTime.Weekly()[0]
                    && x.DocOn <= FilterTheDateTime.Weekly()[1]);

        contract = contract.Where(x => !filter.HasMonthly || x.DocOn >= FilterTheDateTime.Monthly()[0]
                    && x.DocOn <= FilterTheDateTime.Monthly()[1]);

        contract = contract.Where(x => !filter.HasYearly || x.DocOn >= FilterTheDateTime.Yearly()[0]
                    && x.DocOn <= FilterTheDateTime.Yearly()[1]);

        contract = contract.Where(x => !filter.RegionId.HasValue || x.Application.RegionId == filter.RegionId);
        contract = contract.Where(x => !filter.DistrictId.HasValue || x.Application.DistrictId == filter.DistrictId);
        #endregion

        #region deed filter
        deed = deed.Where(x => !filter.HasWeekly || x.DocOn >= FilterTheDateTime.Weekly()[0]
            && x.DocOn <= FilterTheDateTime.Weekly()[1]);

        deed = deed.Where(x => !filter.HasMonthly || x.DocOn >= FilterTheDateTime.Monthly()[0]
                    && x.DocOn <= FilterTheDateTime.Monthly()[1]);

        deed = deed.Where(x => !filter.HasYearly || x.DocOn >= FilterTheDateTime.Yearly()[0]
                    && x.DocOn <= FilterTheDateTime.Yearly()[1]);

        deed = deed.Where(x => !filter.RegionId.HasValue || x.Application.RegionId == filter.RegionId);
        deed = deed.Where(x => !filter.DistrictId.HasValue || x.Application.DistrictId == filter.DistrictId);
        #endregion

        SrvApplicationTypeDashDto result = new();

        result.TotalAcceptApplications = application.Count();
        result.TotalAcceptPaidApplications = application.Where(x => x.IsFree == false).Count();
        result.TotalAcceptFreeApplications = application.Where(x => x.IsFree == true).Count();

        result.TotalSignedContracts = contract.Count();
        result.TotalDeeds = deed.Count();

        var totalDeedIncome = deed.SelectMany(x => x.Groups).SelectMany(g => g.Tables).Sum(t => t.Price);
        var totalContractIncome = contract.SelectMany(x => x.Groups).SelectMany(g => g.Tables).Sum(t => t.Price);

        result.TotalIncome = totalDeedIncome;
        result.TotalRemainsIncome = totalContractIncome - totalDeedIncome;
        return result;
    }

    public SrvContractTypeDashDto GetSrvContractTypeCount(SrvContractTypeDashFilter filter)
    {
        var res = _unitOfWork.Context.Set<ServiceContract>().Where(a => a.StatusId != StatusIdConst.DELETED && a.Contractor.RegionId == filter.RegionId);
        var application = _unitOfWork.Context.Set<ServiceApplication>().Where(a => a.RegionId == filter.RegionId);
        var deed = _unitOfWork.Context.Set<ServiceDeed>().Where(a => a.StatusId != StatusIdConst.DELETED && a.SrvContract.Contractor.RegionId == filter.RegionId);

        SrvContractTypeDashDto result = new();

        result.TotalNumberOfDeed = deed.Count();
        result.TotalNumberOfApplications = application.Count();
        result.TotalContracts = res.Count();
        result.SignedContracts = res.Where(a => a.StatusId == StatusIdConst.SIGNED).Count();
        result.SigningContracts = res.Where(a => a.StatusId == StatusIdConst.SIGNING).Count();
        result.RejectedContracts = res.Where(a => a.StatusId == StatusIdConst.REJECTED).Count();
        return result;
    }

    public List<SrvApplicationStatusDto> GetSrvApplicationStatusCount(SrvDashboardFilterOption filter)
    {
        var query = _unitOfWork.Context.Set<ServiceApplication>()
            .Include(x => x.Application)
                .ThenInclude(x => x.Status)
            .Where(a => new int[] { StatusIdConst.SENT, StatusIdConst.CANCELED, StatusIdConst.REJECTED, StatusIdConst.ACCEPTED }.Contains(a.Application.StatusId));

        query = query.Where(x => !filter.HasWeekly || x.Application.DocOn >= FilterTheDateTime.Weekly()[0]
                  && x.Application.DocOn <= FilterTheDateTime.Weekly()[1]);

        query = query.Where(x => !filter.HasMonthly || x.Application.DocOn >= FilterTheDateTime.Monthly()[0]
                    && x.Application.DocOn <= FilterTheDateTime.Monthly()[1]);

        query = query.Where(x => !filter.HasYearly || x.Application.DocOn >= FilterTheDateTime.Yearly()[0]
                    && x.Application.DocOn <= FilterTheDateTime.Yearly()[1]);

        query = query.Where(x => filter.IsFree == null || filter.IsFree.Value == x.IsFree);
        query = query.Where(x => !filter.RegionId.HasValue || filter.RegionId == x.Application.RegionId);
        query = query.Where(x => !filter.DistrictId.HasValue || filter.DistrictId == x.Application.DistrictId);

        string district = string.Empty, region = string.Empty;

        if (filter.RegionId.HasValue)
        {
            region = _unitOfWork.Context.Set<Region>()
                                        .Include(x => x.Translates)
                                        .FirstOrDefault(p => p.Id == filter.RegionId).Translates
                                        .AsQueryable()
                        .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                  ?? _unitOfWork.Context.Set<Region>().FirstOrDefault(p => p.Id == filter.RegionId).FullName;
        }

        if (filter.DistrictId.HasValue)
        {
            district = _unitOfWork.Context.Set<District>()
                                          .Include(x => x.Translates)
                                          .FirstOrDefault(p => p.Id == filter.DistrictId).Translates
                                          .AsQueryable()
                        .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                    ?? _unitOfWork.Context.Set<District>().FirstOrDefault(p => p.Id == filter.RegionId).FullName;
        }

        var allStatuses = _unitOfWork.Context.Set<Status>()
            .Where(x => new int[] { StatusIdConst.CREATED, StatusIdConst.CANCELED, StatusIdConst.REJECTED, StatusIdConst.ACCEPTED }.Contains(x.Id))
            .Select(x => new { x.Id, x.FullName })
            .ToList();

        var groupedResult = query
            .GroupBy(a => a.Application.StatusId)
            .Select(g => new { StatusId = g.Key, Count = g.Count() })
            .ToList();

        var result = allStatuses.Select(status =>
        {
            var match = groupedResult.FirstOrDefault(x => x.StatusId == status.Id);
            return new SrvApplicationStatusDto
            {
                StatusId = status.Id,
                Status = status.FullName,
                RegionId = filter.RegionId,
                DistrictId = filter.DistrictId,
                DocCount = match?.Count ?? 0
            };
        }).ToList();

        return result;
    }

    public List<SrvContractStatusDto> GetSrvContractStatusCount(SrvDashboardFilterOption filter)
    {
        var query = _unitOfWork.Context.Set<ServiceContract>()
            .Include(x => x.Application)
                .ThenInclude(x => x.ServiceApplication)
            .Where(x => !new int[] { StatusIdConst.CANCELED, StatusIdConst.DELETED, StatusIdConst.REJECTED }.Contains(x.StatusId))
            .Where(x => filter.IsFree == null || filter.IsFree.Value == x.Application.ServiceApplication.IsFree);

        query = query.Where(x => !filter.HasWeekly || x.DocOn >= FilterTheDateTime.Weekly()[0]
            && x.DocOn <= FilterTheDateTime.Weekly()[1]);

        query = query.Where(x => !filter.HasMonthly || x.DocOn >= FilterTheDateTime.Monthly()[0]
                    && x.DocOn <= FilterTheDateTime.Monthly()[1]);

        query = query.Where(x => !filter.HasYearly || x.DocOn >= FilterTheDateTime.Yearly()[0]
                    && x.DocOn <= FilterTheDateTime.Yearly()[1]);

        query = query.Where(x => !filter.RegionId.HasValue || filter.RegionId == x.Application.RegionId);
        query = query.Where(x => !filter.DistrictId.HasValue || filter.DistrictId == x.Application.DistrictId);

        string district = string.Empty, region = string.Empty;
        if (filter.RegionId.HasValue && filter.RegionId != 0)
        {
            region = _unitOfWork.Context.Set<Region>()
                                        .Include(x => x.Translates)
                                        .FirstOrDefault(p => p.Id == filter.RegionId).Translates
                                        .AsQueryable()
                        .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                  ?? _unitOfWork.Context.Set<Region>().FirstOrDefault(p => p.Id == filter.RegionId).FullName;
        }

        if (filter.DistrictId.HasValue && filter.DistrictId != 0)
        {
            district = _unitOfWork.Context.Set<District>()
                                          .Include(x => x.Translates)
                                          .FirstOrDefault(p => p.Id == filter.DistrictId).Translates
                                          .AsQueryable()
                        .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                    ?? _unitOfWork.Context.Set<District>().FirstOrDefault(p => p.Id == filter.RegionId).FullName;
        }

        var result = query
            .Select(a => new SrvContractStatusDto()
            {
                StatusId = a.StatusId,
                DocCount = 1
            })
            .GroupBy(a => a.StatusId)
            .Select(a => new SrvContractStatusDto()
            {
                StatusId = a.Key,
                Status = query.FirstOrDefault(q => q.StatusId == a.Key).Status.FullName ?? string.Empty,
                RegionId = filter.RegionId,
                DistrictId = filter.DistrictId,
                DocCount = a.Sum(x => x.DocCount)
            })
            .OrderBy(dto => dto.StatusId)
            .ToList();

        return result;
    }

    public SrvDeedSumDto GetSrvDeedSum(SrvDashboardFilterOption filter)
    {
        var query = _unitOfWork.Context.Set<ServiceDeed>()
            .Include(x => x.SrvContract)
                .ThenInclude(x => x.Application)
                    .ThenInclude(x => x.ServiceApplication)
            .Where(x => x.StatusId != StatusIdConst.DELETED || x.StatusId != StatusIdConst.REJECTED)
            .Where(x => filter.IsFree == null || filter.IsFree.Value == x.Application.ServiceApplication.IsFree);

        query = query.Where(x => !filter.HasWeekly || x.DocOn >= FilterTheDateTime.Weekly()[0]
            && x.DocOn <= FilterTheDateTime.Weekly()[1]);

        query = query.Where(x => !filter.HasMonthly || x.DocOn >= FilterTheDateTime.Monthly()[0]
                    && x.DocOn <= FilterTheDateTime.Monthly()[1]);

        query = query.Where(x => !filter.HasYearly || x.DocOn >= FilterTheDateTime.Yearly()[0]
                    && x.DocOn <= FilterTheDateTime.Yearly()[1]);

        query = query.Where(x => !filter.RegionId.HasValue || filter.RegionId == x.Application.RegionId);
        query = query.Where(x => !filter.DistrictId.HasValue || filter.DistrictId == x.Application.DistrictId);

        string district = string.Empty, region = string.Empty;
        if (filter.RegionId.HasValue && filter.RegionId != 0)
        {
            region = _unitOfWork.Context.Set<Region>()
                                        .Include(x => x.Translates)
                                        .FirstOrDefault(p => p.Id == filter.RegionId).Translates
                                        .AsQueryable()
                        .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                  ?? _unitOfWork.Context.Set<Region>().FirstOrDefault(p => p.Id == filter.RegionId).FullName;
        }

        if (filter.DistrictId.HasValue && filter.DistrictId != 0)
        {
            district = _unitOfWork.Context.Set<District>()
                                          .Include(x => x.Translates)
                                          .FirstOrDefault(p => p.Id == filter.DistrictId).Translates
                                          .AsQueryable()
                        .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                    ?? _unitOfWork.Context.Set<District>().FirstOrDefault(p => p.Id == filter.RegionId).FullName;
        }

        var contract = _unitOfWork.Context.Set<ServiceContract>().Where(a => a.StatusId == StatusIdConst.SIGNED);
        contract = contract.Where(x => !filter.RegionId.HasValue || filter.RegionId == x.Application.RegionId);
        contract = contract.Where(x => !filter.DistrictId.HasValue || filter.DistrictId == x.Application.DistrictId);

        contract = contract.Where(x => !filter.HasWeekly || x.DocOn >= FilterTheDateTime.Weekly()[0]
            && x.DocOn <= FilterTheDateTime.Weekly()[1]);

        contract = contract.Where(x => !filter.HasMonthly || x.DocOn >= FilterTheDateTime.Monthly()[0]
                    && x.DocOn <= FilterTheDateTime.Monthly()[1]);

        contract = contract.Where(x => !filter.HasYearly || x.DocOn >= FilterTheDateTime.Yearly()[0]
                    && x.DocOn <= FilterTheDateTime.Yearly()[1]);

		var totalDeedIncome = query.SelectMany(x => x.Groups).SelectMany(g => g.Tables).Sum(t => t.Price);
		var totalContractIncome = contract.SelectMany(x => x.Groups).SelectMany(g => g.Tables).Sum(t => t.Price);
		//var totalDeedIncome = query.Sum(x => x.Groups.SelectMany(a => a.Tables).Sum(b => b.Price));
        //var totalContractIncome = contract.Sum(x => x.Groups.SelectMany(a => a.Tables).Sum(b => b.Price));

        var result = new SrvDeedSumDto()
        {
            RegionId = filter.RegionId.HasValue ? filter.RegionId.Value : null,
            DistrictId = filter.DistrictId.HasValue ? filter.DistrictId.Value : null,
            TotalIncome = totalDeedIncome,
            TotalRemainsIncome = totalContractIncome - totalDeedIncome
        };

        return result;
    }

    public List<SrvDocumentRegionRate> GetSrvDocumentsRegionRate(SrvRegionRateFilterOptions filter)
    {
        Dictionary<int, string> regions = new();
        foreach (var ent in _unitOfWork.Context.Set<Region>().Include(r => r.Translates).ToList())
            regions.Add(ent.Id,
                        ent.Translates.AsQueryable().FirstOrDefault(RegionTranslate
                                .GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                        .TranslateText ?? ent.FullName);

        Dictionary<int, string> districts = new();
        foreach (var ent in _unitOfWork.Context.Set<District>().Include(d => d.Translates).ToList())
            districts.Add(ent.Id,
                        ent.Translates.AsQueryable().FirstOrDefault(DistrictTranslate
                                .GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                        .TranslateText ?? ent.FullName);

        return filter.TableId switch
        {
            TableIdConst.DOC_SERVICE_APPLICATION => GetSrvApplication(filter, regions, districts),
            TableIdConst.DOC_SERVICE_CONTRACT => GetSrvContract(filter, regions, districts),
            TableIdConst.DOC_SERVICE_DEED => GetSrvDeed(filter, regions, districts),
            1 => GetSrvTotalIncome(filter, regions, districts),
            2 => GetSrvTotalRemainsIncome(filter, regions, districts)
        };
    }

    private List<SrvDocumentRegionRate> GetSrvApplication(
        SrvRegionRateFilterOptions filter, 
        Dictionary<int, string> regions,
        Dictionary<int, string> districts)
    {
        var query = _unitOfWork.Context.Set<ServiceApplication>()
            .Include(x => x.Application)
            .Where(x => x.Application.StatusId == StatusIdConst.ACCEPTED)
            .Where(x => filter.IsFree == null || filter.IsFree.Value == x.IsFree);

        query = query.Where(x => !filter.HasWeekly || x.Application.DocOn >= FilterTheDateTime.Weekly()[0]
                    && x.Application.DocOn <= FilterTheDateTime.Weekly()[1]);

        query = query.Where(x => !filter.HasMonthly || x.Application.DocOn >= FilterTheDateTime.Monthly()[0]
                    && x.Application.DocOn <= FilterTheDateTime.Monthly()[1]);

        query = query.Where(x => !filter.HasYearly || x.Application.DocOn >= FilterTheDateTime.Yearly()[0]
                    && x.Application.DocOn <= FilterTheDateTime.Yearly()[1]);

        List<SrvDocumentRegionRate> result = new();
        if (filter.RegionId.HasValue) 
        {
            result = query
                .Where(x => x.Application.RegionId == filter.RegionId)
                .Select(x => new SrvDocumentRegionRate()
                {
                    DistrictId = x.Application.DistrictId,
                    DistrictOrderCode = x.Application.District.OrderCode,
                    DocCount = 1
                })
                .GroupBy(x => new { x.DistrictId, x.DistrictOrderCode })
                .Select(x => new SrvDocumentRegionRate()
                {
                    DistrictId = x.Key.DistrictId,
                    DistrictOrderCode = x.Key.DistrictOrderCode,
                    Region = districts[(int)x.Key.DistrictId],
                    DocCount = x.Sum(x => x.DocCount)
                })
                .OrderBy(x => x.DistrictOrderCode)
                .ToList();
        }
        else
        {
            result = query
                .Select(x => new SrvDocumentRegionRate
                {
                    RegionId = x.Application.RegionId,
                    RegionOrderCode = x.Application.Region.OrderCode,
                    DocCount = 1
                })
                .GroupBy(x => new { x.RegionId, x.RegionOrderCode })
                .Select(x => new SrvDocumentRegionRate()
                {
                    RegionId = x.Key.RegionId,
                    RegionOrderCode = x.Key.RegionOrderCode,
                    Region = regions[(int)x.Key.RegionId],
                    DocCount = x.Sum(x => x.DocCount)
                })
                .OrderBy(x => x.RegionOrderCode)
                .ToList();
        }

        return result;
    }

    private List<SrvDocumentRegionRate> GetSrvContract(
        SrvRegionRateFilterOptions filter,
        Dictionary<int, string> regions,
        Dictionary<int, string> districts)
    {
        var query = _unitOfWork.Context.Set<ServiceContract>()
            .Include(x => x.Application)
                .ThenInclude(x => x.ServiceApplication)
            .Where(x => x.StatusId == StatusIdConst.SIGNED)
            .Where(x => filter.IsFree == null || filter.IsFree.Value == x.Application.ServiceApplication.IsFree);

        query = query.Where(x => !filter.HasWeekly || x.DocOn >= FilterTheDateTime.Weekly()[0]
                    && x.DocOn <= FilterTheDateTime.Weekly()[1]);

        query = query.Where(x => !filter.HasMonthly || x.DocOn >= FilterTheDateTime.Monthly()[0]
                    && x.DocOn <= FilterTheDateTime.Monthly()[1]);

        query = query.Where(x => !filter.HasYearly || x.DocOn >= FilterTheDateTime.Yearly()[0]
                    && x.DocOn <= FilterTheDateTime.Yearly()[1]);

        List<SrvDocumentRegionRate> result = new();
        if (filter.RegionId.HasValue)
        {
            result = query
                .Where(x => x.Application.RegionId == filter.RegionId)
                .Select(x => new SrvDocumentRegionRate()
                {
                    DistrictId = x.Application.DistrictId,
                    DistrictOrderCode = x.Application.District.OrderCode,
                    DocCount = 1
                })
                .GroupBy(x => new { x.DistrictId, x.DistrictOrderCode })
                .Select(x => new SrvDocumentRegionRate()
                {
                    DistrictId = x.Key.DistrictId,
                    DistrictOrderCode = x.Key.DistrictOrderCode,
                    Region = districts[(int)x.Key.DistrictId],
                    DocCount = x.Sum(x => x.DocCount)
                })
                .OrderBy(x => x.DistrictOrderCode)
                .ToList();
        }
        else
        {
            result = query
                .Select(x => new SrvDocumentRegionRate
                {
                    RegionId = x.Application.RegionId,
                    RegionOrderCode = x.Application.Region.OrderCode,
                    DocCount = 1
                })
                .GroupBy(x => new { x.RegionId, x.RegionOrderCode })
                .Select(x => new SrvDocumentRegionRate()
                {
                    RegionId = x.Key.RegionId,
                    RegionOrderCode = x.Key.RegionOrderCode,
                    Region = regions[(int)x.Key.RegionId],
                    DocCount = x.Sum(x => x.DocCount)
                })
                .OrderBy(x => x.RegionOrderCode)
                .ToList();
        }

        return result;
    }

    private List<SrvDocumentRegionRate> GetSrvDeed(
        SrvRegionRateFilterOptions filter,
        Dictionary<int, string> regions,
        Dictionary<int, string> districts)
    {
        var query = _unitOfWork.Context.Set<ServiceDeed>()
            .Include(x => x.Application)
                .ThenInclude(x => x.ServiceApplication)
            .Where(x => x.StatusId != StatusIdConst.DELETED || x.StatusId != StatusIdConst.REJECTED)
            .Where(x => filter.IsFree == null || filter.IsFree.Value == x.Application.ServiceApplication.IsFree);

        query = query.Where(x => !filter.HasWeekly || x.DocOn >= FilterTheDateTime.Weekly()[0]
                    && x.DocOn <= FilterTheDateTime.Weekly()[1]);

        query = query.Where(x => !filter.HasMonthly || x.DocOn >= FilterTheDateTime.Monthly()[0]
                    && x.DocOn <= FilterTheDateTime.Monthly()[1]);
        
        query = query.Where(x => !filter.HasYearly || x.DocOn >= FilterTheDateTime.Yearly()[0]
                    && x.DocOn <= FilterTheDateTime.Yearly()[1]);

        List<SrvDocumentRegionRate> result = new();
        if (filter.RegionId.HasValue)
        {
            result = query
                .Where(x => x.Application.RegionId == filter.RegionId)
                .Select(x => new SrvDocumentRegionRate()
                {
                    DistrictId = x.Application.DistrictId,
                    DistrictOrderCode = x.Application.District.OrderCode,
                    DocCount = 1
                })
                .GroupBy(x => new { x.DistrictId, x.DistrictOrderCode })
                .Select(x => new SrvDocumentRegionRate()
                {
                    DistrictId = x.Key.DistrictId,
                    DistrictOrderCode = x.Key.DistrictOrderCode,
                    Region = districts[(int)x.Key.DistrictId],
                    DocCount = x.Sum(x => x.DocCount)
                })
                .OrderBy(x => x.DistrictOrderCode)
                .ToList();
        }
        else
        {
            result = query
                .Select(x => new SrvDocumentRegionRate
                {
                    RegionId = x.Application.RegionId,
                    RegionOrderCode = x.Application.Region.OrderCode,
                    DocCount = 1
                })
                .GroupBy(x => new { x.RegionId, x.RegionOrderCode })
                .Select(x => new SrvDocumentRegionRate()
                {
                    RegionId = x.Key.RegionId,
                    RegionOrderCode = x.Key.RegionOrderCode,
                    Region = regions[(int)x.Key.RegionId],
                    DocCount = x.Sum(x => x.DocCount)
                })
                .OrderBy(x => x.RegionOrderCode)
                .ToList();
        }

        return result;
    }

    private List<SrvDocumentRegionRate> GetSrvTotalIncome(
        SrvRegionRateFilterOptions filter,
        Dictionary<int, string> regions,
        Dictionary<int, string> districts)
    {
        var query = _unitOfWork.Context.Set<ServiceDeed>()
            .Include(x => x.Application)
                .ThenInclude(x => x.ServiceApplication)
            .Where(x => x.StatusId != StatusIdConst.DELETED || x.StatusId != StatusIdConst.REJECTED)
            .Where(x => filter.IsFree == null || filter.IsFree.Value == x.Application.ServiceApplication.IsFree);

        query = query.Where(x => !filter.HasWeekly || x.DocOn >= FilterTheDateTime.Weekly()[0]
                    && x.DocOn <= FilterTheDateTime.Weekly()[1]);

        query = query.Where(x => !filter.HasMonthly || x.DocOn >= FilterTheDateTime.Monthly()[0]
                    && x.DocOn <= FilterTheDateTime.Monthly()[1]);

        query = query.Where(x => !filter.HasYearly || x.DocOn >= FilterTheDateTime.Yearly()[0]
                    && x.DocOn <= FilterTheDateTime.Yearly()[1]);

        List<SrvDocumentRegionRate> result = new();
        if (filter.RegionId.HasValue)
        {
            result = query
                .Where(x => x.Application.RegionId == filter.RegionId)
                .Select(x => new SrvDocumentRegionRate()
                {
                    DistrictId = x.Application.DistrictId,
                    DistrictOrderCode = x.Application.District.OrderCode,
                    TotalIncome = x.Groups.SelectMany(a => a.Tables).Sum(b => b.Price),
                })
                .GroupBy(x => new { x.DistrictId, x.DistrictOrderCode })
                .Select(x => new SrvDocumentRegionRate()
                {
                    DistrictId = x.Key.DistrictId,
                    DistrictOrderCode = x.Key.DistrictOrderCode,
                    Region = districts[(int)x.Key.DistrictId],
                    TotalIncome = x.Sum(x => x.TotalIncome)
                })
                .OrderBy(x => x.DistrictOrderCode)
                .ToList();
        }
        else
        {
            result = query
                .Select(x => new SrvDocumentRegionRate
                {
                    RegionId = x.Application.RegionId,
                    RegionOrderCode = x.Application.Region.OrderCode,
                    TotalIncome = x.Groups.SelectMany(x => x.Tables).Sum(b => b.Price)
                })
                .GroupBy(x => new { x.RegionId, x.RegionOrderCode })
                .Select(x => new SrvDocumentRegionRate()
                {
                    RegionId = x.Key.RegionId,
                    RegionOrderCode = x.Key.RegionOrderCode,
                    Region = regions[(int)x.Key.RegionId],
                    TotalIncome = x.Sum(x => x.TotalIncome)
                })
                .OrderBy(x => x.RegionOrderCode)
                .ToList();
        }

        return result;
    }

    private List<SrvDocumentRegionRate> GetSrvTotalRemainsIncome(
        SrvRegionRateFilterOptions filter,
        Dictionary<int, string> regions,
        Dictionary<int, string> districts)
    {
        var serviceContracts = _unitOfWork.Context.Set<ServiceContract>()
            .Include(x => x.Application)
                .ThenInclude(x => x.ServiceApplication)
            .Where(x => x.StatusId == StatusIdConst.SIGNED)
            .Where(x => filter.IsFree == null || filter.IsFree.Value == x.Application.ServiceApplication.IsFree);

        serviceContracts = serviceContracts.Where(x => !filter.HasWeekly || x.DocOn >= FilterTheDateTime.Weekly()[0]
                    && x.DocOn <= FilterTheDateTime.Weekly()[1]);

        serviceContracts = serviceContracts.Where(x => !filter.HasMonthly || x.DocOn >= FilterTheDateTime.Monthly()[0]
                    && x.DocOn <= FilterTheDateTime.Monthly()[1]);

        serviceContracts = serviceContracts.Where(x => !filter.HasYearly || x.DocOn >= FilterTheDateTime.Yearly()[0]
                    && x.DocOn <= FilterTheDateTime.Yearly()[1]);

        var serviceDeeds = _unitOfWork.Context.Set<ServiceDeed>()
           .Include(x => x.SrvContract)
           .Include(x => x.Application)
               .ThenInclude(x => x.ServiceApplication)
           .Where(x => x.StatusId != StatusIdConst.DELETED || x.StatusId != StatusIdConst.DELETED)
           .Where(x => filter.IsFree == null || filter.IsFree.Value == x.Application.ServiceApplication.IsFree);

        serviceDeeds = serviceDeeds.Where(x => !filter.HasWeekly || x.DocOn >= FilterTheDateTime.Weekly()[0]
               && x.DocOn <= FilterTheDateTime.Weekly()[1]);

        serviceDeeds = serviceDeeds.Where(x => !filter.HasMonthly || x.DocOn >= FilterTheDateTime.Monthly()[0]
                    && x.DocOn <= FilterTheDateTime.Monthly()[1]);

        serviceDeeds = serviceDeeds.Where(x => !filter.HasYearly || x.DocOn >= FilterTheDateTime.Yearly()[0]
                    && x.DocOn <= FilterTheDateTime.Yearly()[1]);

        List<SrvDocumentRegionRate> result = new();
        if (filter.RegionId.HasValue)
        {
            result = serviceContracts
                .Where(x => x.Application.RegionId == filter.RegionId)
                .Select(x => new SrvDocumentRegionRate
                {
                    DistrictId = x.Application.DistrictId,
                    DistrictOrderCode = x.Application.District.OrderCode,
                    TotalIncome = x.Groups.SelectMany(a => a.Tables).Sum(b => b.Price)
                })
                .GroupBy(x => new { x.DistrictId, x.DistrictOrderCode })
                .Select(group => new SrvDocumentRegionRate
                {
                    DistrictId = group.Key.DistrictId,
                    DistrictOrderCode = group.Key.DistrictOrderCode,
                    Region = districts[(int)group.Key.DistrictId],
                    TotalIncome = group.Sum(x => x.TotalIncome),
                    TotalRemainsIncome = group.Sum(x => x.TotalIncome) -
                                         serviceDeeds
                                         .Where(sd => sd.Application.DistrictId == group.Key.DistrictId)
                                         .Sum(sd => sd.Groups.SelectMany(a => a.Tables).Sum(b => b.Price))
                })
                .OrderBy(x => x.DistrictOrderCode)
                .ToList();
        }
        else
        {
            result = serviceContracts
                .Select(x => new SrvDocumentRegionRate
                {
                    RegionId = x.Application.RegionId,
                    RegionOrderCode = x.Application.Region.OrderCode,
                    TotalIncome = x.Groups.SelectMany(a => a.Tables).Sum(b => b.Price)
                })
                .GroupBy(x => new { x.RegionId, x.RegionOrderCode })
                .Select(group => new SrvDocumentRegionRate
                {
                    RegionId = group.Key.RegionId,
                    RegionOrderCode = group.Key.RegionOrderCode,
                    Region = regions[(int)group.Key.RegionId],
                    TotalIncome = group.Sum(x => x.TotalIncome),
                    TotalRemainsIncome = group.Sum(x => x.TotalIncome) -
                                         serviceDeeds
                                         .Where(sd => sd.Application.RegionId == group.Key.RegionId)
                                         .Sum(sd => sd.Groups.SelectMany(a => a.Tables).Sum(b => b.Price))
                })
                .OrderBy(x => x.RegionOrderCode)
                .ToList();
        }

        return result;
    }

    public static class FilterTheDateTime
    {
        private static DateTime now { get; set; } = DateTime.Now;
        public static DateOnly[] Weekly()
        {
            var nowDayOfWeek = (int)now.DayOfWeek;

            return new DateOnly[]
            {
                DateOnly.FromDateTime(now.AddDays((int)DayOfWeek.Monday - nowDayOfWeek)),
                DateOnly.FromDateTime(now.AddDays(7 - nowDayOfWeek))
            };
        }
        public static DateOnly[] Monthly()
        {
            var begin = new DateTime(now.Year, now.Month, 1);
            return new DateOnly[]
            {
                DateOnly.FromDateTime(begin),
                DateOnly.FromDateTime(begin.AddMonths(1).AddDays(-1))
            };
        }
        public static DateOnly[] Yearly()
        {
            return new DateOnly[]
            {
                DateOnly.FromDateTime(new DateTime(now.Year, 1, 1)),
                DateOnly.FromDateTime(new DateTime(now.Year, 12, 31))
            };
        }
    }
}