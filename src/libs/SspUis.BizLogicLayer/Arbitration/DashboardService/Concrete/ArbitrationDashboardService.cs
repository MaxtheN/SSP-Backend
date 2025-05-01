using Hangfire.Annotations;
using Microsoft.EntityFrameworkCore;
using OpenXmlPowerTools;
using SspUis.BizLogicLayer.Claim;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.Arbitration;

public class ArbitrationDashboardService : StatusGenericHandler, IArbitrationDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    public ArbitrationDashboardService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;


    private List<ArbitrationRateDto> AddNoValueSpecDistrictAndRegion(List<ArbitrationRateDto> res,
     ArbitrationRateFilterOptions filter)
    
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
                      .Select(d => new ArbitrationRateDto
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
                   .Select(d => new ArbitrationRateDto
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


    public List<ArbitrationRateDto> GetAllArbitrationRateDto(ArbitrationRateFilterOptions filter)
    {
        var regions = _unitOfWork.Context.Set<Region>()
                                  .ToDictionary(ent => ent.Id, ent => ent);

        var districts = _unitOfWork.Context.Set<District>()
                                         .ToDictionary(ent => ent.Id, ent => ent);

        var query = _unitOfWork.Context.Set<ArbitrationCourtApplication>()
                                       .Include(a => a.Application)
                                       .Where(a => a.Application.StatusId != StatusIdConst.DELETED);

        if (filter.RegionId.HasValue)
            query = query.Where(a => a.Application.RegionId == filter.RegionId);

        if (filter.ArbitrationTypeId == 2)
            query = query.Where(a => a.Application.StatusId == StatusIdConst.CREATED);
        else if (filter.ArbitrationTypeId == 3)
            query = query.Where(a => a.ArbitrationResult.StatusId == StatusIdConst.SIGNED && a.ArbitrationResult.CanByDivided);
        else if (filter.ArbitrationTypeId == 4)
            query = query.Where(a => a.ArbitrationResult.StatusId == StatusIdConst.SIGNED && !a.ArbitrationResult.CanByDivided);
        else if (filter.ArbitrationTypeId == 5)
            query = query.Where(a => a.Application.StatusId == StatusIdConst.CANCELED);

        var resultQuery = query.Select(a => new ArbitrationRateDto
        {
            RegionId = a.Application.RegionId,
            Region = a.Application.RegionName,
            RegionOrderCode = a.Application.Region.OrderCode,
            DistrictId = a.Application.DistrictId,
            District = a.Application.DistrictName,
            DistrictOrderCode = a.Application.District.OrderCode,
            Amount = 1
        })
                                .GroupBy(a => new
                                {
                                    Id = filter.RegionId.HasValue ? a.DistrictId : a.RegionId,
                                    OrderCode = filter.RegionId.HasValue ? a.DistrictOrderCode : a.RegionOrderCode
                                })
                                .Select(g => new ArbitrationRateDto
                                {
                                    RegionId = !filter.RegionId.HasValue ? g.Key.Id : null,
                                    DistrictId = filter.RegionId.HasValue ? g.Key.Id : null,
                                    Name = filter.RegionId.HasValue
                                                 ? (g.Key.Id.HasValue && districts.ContainsKey(g.Key.Id.Value) ? districts[g.Key.Id.Value].FullName : "Noma'lum tuman")
                                                 : (g.Key.Id.HasValue && regions.ContainsKey(g.Key.Id.Value) ? regions[g.Key.Id.Value].FullName : "Noma'lum hudud"),
                                    Amount = g.Sum(x => x.Amount)
                                }).ToList();

        return AddNoValueSpecDistrictAndRegion(resultQuery,filter);
    }

    public ArbitrationTypeCountDto GetAllArbitrationTypeAmount(ArbitrationDashFilterOptions options)
    {
        ArbitrationTypeCountDto res = new();

        var query = _unitOfWork.Context.Set<ArbitrationCourtApplication>()
                                       .Include(a => a.Application)
                                       .Where(a => a.Application.StatusId != StatusIdConst.DELETED);

        query = query.Where(a =>
                     (!options.RegionId.HasValue || options.RegionId == a.Application.RegionId) &&
                     (!options.DistrictId.HasValue || options.DistrictId == a.Application.DistrictId)

        );

        res.ArbitrationApplicationCount = query.Count();
        res.ArbitrationApplicationNewCount = query.Where(a => a.Application.StatusId == StatusIdConst.CREATED).Count();
        res.TotalArbitrationCanceledCount = query.Where(a => a.Application.StatusId == StatusIdConst.CANCELED).Count();
        res.TotalArbitrationAcceptedCount = query.Where(a => a.ArbitrationResult.StatusId == StatusIdConst.SIGNED && a.ArbitrationResult.CanByDivided).Count();
        res.TotalArbitrationPartiallyAcceptedCount = query.Where(a => a.ArbitrationResult.StatusId == StatusIdConst.SIGNED && (!a.ArbitrationResult.CanByDivided)).Count();

        return res;
    }
    public List<DeadlineNearArbitrationApplicationDto> GetAllDeadlineNearApplication(ArbitrationApplicationDtoFilter filter)
    {
        var query = _unitOfWork.Context.Set<ArbitrationCourtApplication>()
                                      .Include(a => a.Application)
                                      .Where(a => a.Application.StatusId != StatusIdConst.DELETED && a.Application.StatusId != StatusIdConst.SIGNED);
        query = query.Where(a =>
             (!filter.RegionId.HasValue || filter.RegionId == a.Application.RegionId)
         && (!filter.DistrictId.HasValue || filter.DistrictId == a.Application.DistrictId)
         && (!filter.ContractorId.HasValue || filter.ContractorId == a.Application.ContractorId)
        );

        query = query
                 .Where(c => c.CreatedAt <= DateTime.Now.AddDays(-30));

        List<DeadlineNearArbitrationApplicationDto> result = query.Select(a => new DeadlineNearArbitrationApplicationDto
        {
            ContractorId = a.Application.ContractorId,
            ContractorName = a.Application.Contractor.FullName,
            DistrictId = a.Application.Contractor.DistrictId,
            RegionId = a.Application.Contractor.RegionId,
        })
      .GroupBy(a => new { a.ContractorId, a.ContractorName, a.DistrictId, a.RegionId })
      .Select(a => new DeadlineNearArbitrationApplicationDto
      {
          ContractorId = a.Key.ContractorId ?? null,
          ContractorName = a.Key.ContractorName,
          RegionId = a.Key.RegionId,
          DistrictId = a.Key.DistrictId,
          ApplicationsCount = a.Count()
      })
      .Take(10).ToList();

        return result;
    }
}
