using DocumentFormat.OpenXml.Bibliography;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.ClaimApplicationServices
{
    public static class ClaimApplicationListDtoSortFilter
    {
        public static IQueryable<ClaimApplicationListDto> SortFilter(this IQueryable<ClaimApplicationListDto> query, ClaimApplicationSortFilterOptions options)
        {
            if (options.OrganizationId.HasValue)
                 query = query.Where(w => w.OrganizationId == options.OrganizationId);
            
            if (options.StepId.HasValue && options.StepId != 0)
            {
                if(options.StepId == StepIdConst.CLAIM_APPLICATION_CANCEL)
                {
                    query = query.Where(x => x.Application.CurrentStepId == StepIdConst.CLAIM_APPLICATION_CANCEL 
                        || x.Application.CurrentStepId == StepIdConst.MEDIATION_PLAN_CANCEL
                        || x.Application.CurrentStepId == StepIdConst.MEDIATION_CANCEL);
                }
                else
                    query = query.Where(x => x.Application.CurrentStepId == options.StepId);
            }
            if (options.ForSecondGetList)
            {   
                if(options.ClaimApplicationTypeId is null)
                    query = query.Where(x => x.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APILATION_CASSATION
                          || x.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.CLAIM_REVISION
                          || x.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APPLICATION_FOR_COURT
                          || x.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.COUNTER_CLAIM);
                /*|| (x.ClaimResponsibleTypeId != null && x.ClaimResponsibleTypeId == ClaimResponsibleTypeIdConst.INDIVIDUAL_PERSON)*/
                else
                    query = query.Where(x => x.ClaimApplicationTypeId == options.ClaimApplicationTypeId);
            
            }
            else
            {
                query = query.Where(x => x.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.CLAIM_APPLICATION);
            }
            if (options.StatusId.HasValue && options.StatusId != 0)
                query = query.Where(x => x.Application.StatusId == options.StatusId);

            if (options.RegionId.HasValue)
                query = query.Where(a=>a.Application.RegionId == options.RegionId.Value);

            if (options.DistrictId.HasValue)
                query = query.Where(a => a.Application.DistrictId == options.DistrictId.Value);

            if(options.ContractorId.HasValue)
                query = query.Where(a => a.Application.ContractorId == options.ContractorId.Value);
            //search by application type removed
            //if (options.ClaimApplicationTypeId.HasValue)
            //    query = query.Where(a => a.ClaimApplicationTypeId == options.ClaimApplicationTypeId.Value);

            if (options.OneWeek.Value)
            {
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
                DateOnly day = DateOnly.FromDateTime(DateTime.Now.AddDays(-8));
                query = query.Where(a => a.Application.DocOn < currentDate );
                query = query.Where(a => a.Application.DocOn > day );
            }
            if (options.TwoWeek.Value)
            {
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
                DateOnly Day = DateOnly.FromDateTime(DateTime.Now.AddDays(-15));
                query = query.Where(a => a.Application.DocOn < currentDate && a.Application.DocOn > Day);
            }
            if (options.ThreeWeek.Value)
            {
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
                DateOnly day = DateOnly.FromDateTime(DateTime.Now.AddDays(-22));
                query = query.Where(a => a.Application.DocOn < currentDate );
				query = query.Where(a => a.Application.DocOn > day);
			}
            if (options.Month.Value)
            {
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
                DateOnly day = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1));
                query = query.Where(a => a.Application.DocOn < currentDate );
				query = query.Where(a => a.Application.DocOn > day);
			}
           
            if (options.ClaimThemeId.HasValue)
                query = query.Where(a => a.ClaimThemeId == options.ClaimThemeId.Value);

            if (!string.IsNullOrEmpty(options.ContractorInn))
                query = query.Where(a=>a.Application.ContractorInn == options.ContractorInn);

            if(options.IsIndividual)
                query = query.Where(a => a.InnOrPinfl.Length == 14);
            else
                query = query.Where(a => a.InnOrPinfl.Length != 14);

            if (options.HasSearch())
                query = query.Where(a => a.Application.DocNumber.ToLower().Contains(options.Search.ToLower())
                                    || a.Application.Contractor.ToLower().Contains(options.Search.ToLower())
                                    || a.Application.ContractorInn.ToLower().Contains(options.Search.ToLower())
                                    || a.Application.ContractorPhoneNumber.ToLower().Contains(options.Search.ToLower())
                                    );

            if (options.HasSort())
            {
                if (options.SortBy.ToLower() == nameof(Application.DocNumber).ToLower())
                {
                    query = options.OrderType.ToLower() == "asc" 
                        ? query.OrderBy(x => x.Application.DocNumber)
                        : query.OrderByDescending(x => x.Application.DocNumber);
                }
                else if (options.SortBy.ToLower() == nameof(Application.DocOn).ToLower())
                {
                    query = options.OrderType.ToLower() == "asc"
                        ? query.OrderBy(x => x.Application.DocOn)
                        : query.OrderByDescending(x => x.Application.DocOn);
                }
                else if (options.SortBy.ToLower() == nameof(Application.ClaimApplication.DurationGivenPerformer).ToLower())
                {
                    if (options.OrderType.ToLower() == "asc")
                    {
                        query = query.OrderBy(x => x.DurationGivenPerformer == null) // Null qiymatlarni oxirga
                                     .ThenBy(x => x.DurationGivenPerformer); // Mudatni yaqinlashuv bo‘yicha tartiblash
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.DurationGivenPerformer) // Mudatni uzoqlashuv bo‘yicha tartiblash
                                     .ThenBy(x => x.DurationGivenPerformer == null); // Null qiymatlarni oxirga
                    }
                }
                else if(options.SortBy.ToLower() == "step")
                {
                    query = options.OrderType.ToLower() == "asc"
                        ? query.OrderBy(x => x.Application.Step)
                        : query.OrderByDescending(x => x.Application.Step);
                }
                else if (options.SortBy.ToLower() == "status")
                {
                    query = options.OrderType.ToLower() == "asc"
                        ? query.OrderByDescending(x => x.Application.Status)
                        : query.OrderBy(x => x.Application.Status);
                }
                else
                    query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            }
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
