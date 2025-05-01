using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.ApplicationServices;
using SspUis.BizLogicLayer.PrtnCertificateServices;
using SspUis.BizLogicLayer.PrtnContractServices;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using StatusGeneric;

namespace SspUis.BizLogicLayer.Partner;

public class DashboardService : StatusGenericHandler, IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReportService _reportService;
    private readonly IApplicationService _applicationService;
    private readonly IPrtnContractService _prtnContractService;
    private readonly IPrtnCertificateService _prtnCertificateService;
    public DashboardService(IUnitOfWork unitOfWork,
        IReportService reportService,
        IApplicationService applicationService,
        IPrtnContractService prtnContractService,
        IPrtnCertificateService prtnCertificateService)
    {
        _unitOfWork = unitOfWork;
        _reportService = reportService;
        _applicationService = applicationService;
        _prtnContractService = prtnContractService;
        _prtnCertificateService = prtnCertificateService;
    }

    public List<PrtnApplicationCountDto> GetPrtnApplicationList(DashboardFilterOption options)
    {
        var query = _unitOfWork.Context.Set<PrtnApplication>()
           .Include(a => a.Application)
           .ThenInclude(a => a.Status)
           .Where(a => new int[] { StatusIdConst.REJECTED, StatusIdConst.SENT_FOR_REVIEW, StatusIdConst.ACCEPTED }.Contains(a.Application.StatusId))
           .Where(c => !options.PrtnContractTypeId.HasValue || options.PrtnContractTypeId == c.PrtnContractTypeId);

        query = query.Where(a => !options.RegionId.HasValue
                    || options.RegionId == (a.ChooseLocation ? a.ChoosedRegionId : a.Application.RegionId));

        query = query.Where(a => !options.DistrictId.HasValue
               || options.DistrictId == (a.ChooseLocation ? a.ChoosedDistrictId : a.Application.DistrictId));

        query = query.Where(a => !options.MfyId.HasValue || options.MfyId == a.MfyId);

        string mfy = string.Empty, district = string.Empty, region = string.Empty, type = string.Empty;

        if (options.PrtnContractTypeId.HasValue)
            type = _unitOfWork.Context.Set<PrtnContractType>().Include(x => x.Translates).FirstOrDefault(p => p.Id == options.PrtnContractTypeId).Translates.AsQueryable()
                    .FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                ?? _unitOfWork.Context.Set<PrtnContractType>().FirstOrDefault(p => p.Id == options.PrtnContractTypeId).FullName;

        if (options.RegionId.HasValue)
            region = _unitOfWork.Context.Set<Region>().Include(x => x.Translates).FirstOrDefault(p => p.Id == options.RegionId).Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                  ?? _unitOfWork.Context.Set<Region>().FirstOrDefault(p => p.Id == options.RegionId).FullName;

        if (options.DistrictId.HasValue)
            district = _unitOfWork.Context.Set<District>().Include(x => x.Translates).FirstOrDefault(p => p.Id == options.DistrictId).Translates.AsQueryable()
                        .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                    ?? _unitOfWork.Context.Set<District>().FirstOrDefault(p => p.Id == options.RegionId).FullName;

        if (options.MfyId.HasValue)
            mfy = _unitOfWork.Context.Set<Mfy>().FirstOrDefault(p => p.Id == options.MfyId).FullName;

        var result = query
            .Select(a => new PrtnApplicationCountDto()
            {
                StatusId = a.Application.StatusId,
                DocCount = 1
            })
            .GroupBy(a => a.StatusId)
            .Select(a => new PrtnApplicationCountDto()
            {
                StatusId = a.Key,
                Status = query.FirstOrDefault(q => q.Application.StatusId == a.Key).Application.Status.FullName ?? string.Empty,
                RegionId = options.RegionId,
                DistrictId = options.DistrictId,
                MfyId = options.MfyId,
                Region = region ?? "",
                District = district,
                Mfy = mfy,
                PrtnContractType = type,
                DocCount = a.Sum(b => b.DocCount)
            }).ToList();

        var contractedCount = query.Where(a => a.Application.PrtnContract.StatusId == StatusIdConst.SIGNED).Count();
        var certificatedCount = query.Where(a => a.Application.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED).Count();

        result.Add(new PrtnApplicationCountDto() { DocCount = contractedCount, StatusId = StatusIdConst.SIGNED, Status = "Контракт заключен" });
        result.Add(new PrtnApplicationCountDto() { DocCount = certificatedCount, StatusId = StatusIdConst.FORMED, Status = "Выдан сертификат" });

        return result;
    }
    public List<PrtnContractCountDto> GetPrtnContractList(DashboardFilterOption options)
    {
        var query = _unitOfWork.Context.Set<PrtnContract>()
            .Include(a => a.Application)
            .Where(a => !new int[] { StatusIdConst.CANCELED, StatusIdConst.REJECTED }.Contains(a.StatusId))
            .Where(c => !options.PrtnContractTypeId.HasValue || options.PrtnContractTypeId == c.PrtnContractTypeId);

        query = query.Where(a =>
                (!options.RegionId.HasValue
                    || options.RegionId ==
                    (a.Application.PrtnApplication.ChooseLocation
                        ? a.Application.PrtnApplication.ChoosedRegionId : a.Application.RegionId))

             && (!options.DistrictId.HasValue
                    || options.DistrictId ==
                    (a.Application.PrtnApplication.ChooseLocation
                        ? a.Application.PrtnApplication.ChoosedDistrictId : a.Application.DistrictId))

            && (!options.MfyId.HasValue
                    || options.MfyId == a.Application.PrtnApplication.MfyId)
                );

        string mfy = string.Empty, district = string.Empty, region = string.Empty, type = string.Empty;

        if (options.PrtnContractTypeId.HasValue)
            type = _unitOfWork.Context.Set<PrtnContractType>().Include(x => x.Translates).FirstOrDefault(p => p.Id == options.PrtnContractTypeId).Translates.AsQueryable()
                    .FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                ?? _unitOfWork.Context.Set<PrtnContractType>().FirstOrDefault(p => p.Id == options.PrtnContractTypeId).FullName;

        if (options.RegionId.HasValue)
            region = _unitOfWork.Context.Set<Region>().Include(x => x.Translates).FirstOrDefault(p => p.Id == options.RegionId).Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                  ?? _unitOfWork.Context.Set<Region>().FirstOrDefault(p => p.Id == options.RegionId).FullName;

        if (options.DistrictId.HasValue)
            district = _unitOfWork.Context.Set<District>().Include(x => x.Translates).FirstOrDefault(p => p.Id == options.DistrictId).Translates.AsQueryable()
                        .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                    ?? _unitOfWork.Context.Set<District>().FirstOrDefault(p => p.Id == options.RegionId).FullName;

        if (options.MfyId.HasValue)
            mfy = _unitOfWork.Context.Set<Mfy>().FirstOrDefault(p => p.Id == options.MfyId).FullName;

        var result = query
            .Select(a => new PrtnContractCountDto()
            {
                StatusId = a.StatusId,
                DocCount = 1
            })
            .GroupBy(a => a.StatusId)
            .Select(a => new PrtnContractCountDto()
            {
                StatusId = a.Key,
                Status = query.FirstOrDefault(q => q.StatusId == a.Key).Status.FullName ?? string.Empty,
                RegionId = options.RegionId,
                DistrictId = options.DistrictId,
                MfyId = options.MfyId,
                Region = region ?? "",
                District = district,
                Mfy = mfy,
                PrtnContractType = type,
                DocCount = a.Sum(b => b.DocCount)
            }).ToList();

        var certificatedCount = query.Where(a => a.PrtnCertificate.StatusId == StatusIdConst.FORMED).Count();
        result.Add(new PrtnApplicationCountDto() { DocCount = certificatedCount, StatusId = StatusIdConst.FORMED, Status = "Выдан сертификат" });

        return result;
    }
    public PrtnCertificateCountDto GetPrtnCertificateList(DashboardFilterOption options)
    {
        var query = _unitOfWork.Context.Set<PrtnCertificate>()
            .Include(a => a.PrtnContract)
            .ThenInclude(a => a.Application)
            .Where(a => new int[] { StatusIdConst.CANCELED, StatusIdConst.FORMED }.Contains(a.StatusId))
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

        string mfy = string.Empty, district = string.Empty, region = string.Empty, type = string.Empty;

        if (options.PrtnContractTypeId.HasValue && options.PrtnContractTypeId != 0)
            type = _unitOfWork.Context.Set<PrtnContractType>().Include(x => x.Translates).FirstOrDefault(p => p.Id == options.PrtnContractTypeId).Translates.AsQueryable()
                    .FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                ?? _unitOfWork.Context.Set<PrtnContractType>().FirstOrDefault(p => p.Id == options.PrtnContractTypeId).FullName;

        if (options.RegionId.HasValue && options.RegionId != 0)
            region = _unitOfWork.Context.Set<Region>().Include(x => x.Translates).FirstOrDefault(p => p.Id == options.RegionId).Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                  ?? _unitOfWork.Context.Set<Region>().FirstOrDefault(p => p.Id == options.RegionId).FullName;

        if (options.DistrictId.HasValue && options.DistrictId != 0)
            district = _unitOfWork.Context.Set<District>().Include(x => x.Translates).FirstOrDefault(p => p.Id == options.DistrictId).Translates.AsQueryable()
                        .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                    ?? _unitOfWork.Context.Set<District>().FirstOrDefault(p => p.Id == options.RegionId).FullName;

        if (options.MfyId.HasValue && options.MfyId != 0)
            mfy = _unitOfWork.Context.Set<Mfy>().FirstOrDefault(p => p.Id == options.MfyId).FullName;

        var result = new PrtnCertificateCountDto()
        {
            PrtnContractTypeId = options.PrtnContractTypeId.HasValue ? options.PrtnContractTypeId.Value : null,
            PrtnContractType = type,
            RegionId = options.RegionId.HasValue ? options.RegionId.Value : null,
            Region = region,
            DistrictId = options.DistrictId.HasValue ? options.DistrictId.Value : null,
            District = district,
            MfyId = options.MfyId.HasValue ? options.MfyId.Value : null,
            Mfy = mfy,
            FormedCount = query.Where(a => a.StatusId == StatusIdConst.FORMED).Count(),
            CancelCount = query.Where(a => a.StatusId == StatusIdConst.CANCELED).Count()
        };

        return result;
    }
    public PrtnStatisticsDto GetPrtnStatisticsList(DashboardFilterOption options)
    {
        var query = _unitOfWork.Context.Set<Application>()
            .Include(a => a.PrtnApplication)
            .Include(a => a.PrtnContract)
            .ThenInclude(a => a.PrtnCertificate)
            .Where(a => a.ApplicationTypeId == ApplicationTypeIdConst.PARTNER)
            .Where(c => !options.PrtnContractTypeId.HasValue || options.PrtnContractTypeId == c.PrtnContract.PrtnContractTypeId);

        query = query.Where(a =>
                (!options.RegionId.HasValue
                    || options.RegionId ==
                    (a.PrtnApplication.ChooseLocation
                        ? a.PrtnApplication.ChoosedRegionId : a.RegionId))

             && (!options.DistrictId.HasValue
                    || options.DistrictId ==
                    (a.PrtnApplication.ChooseLocation
                        ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId))

             && (!options.MfyId.HasValue || options.MfyId == (a.PrtnApplication.MfyId.Value)));

        var result = new PrtnStatisticsDto()
        {
            TotalApplicationsReceivedCount = query.Where(a => a.StatusId == StatusIdConst.ACCEPTED).Count(),
            TotalApplicationsReviewCount = query.Where(a => a.StatusId == StatusIdConst.SENT_FOR_REVIEW).Count(),
            TotalApplicationsRejectedCount = query.Where(a => a.StatusId == StatusIdConst.REJECTED).Count(),
            ContractsExpertiseCount = query.Where(c => c.PrtnContract.StatusId == StatusIdConst.SENT_FOR_EXPERTISE).Count(),
            ContractsSingingCount = query.Where(c => c.PrtnContract.StatusId == StatusIdConst.SIGNING).Count(),
            ContractsSignedCount = query.Where(c => c.PrtnContract.StatusId == StatusIdConst.SIGNED).Count()
            // CertificateFormedCount = query.Where(c => c.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED).Count(),
            // VacanciesCount = query.Sum(a => a.PrtnContract.NewVacanciesCount)
        };

        var CertificateCount = _reportService.GetPrtnApplicationByContractType(new PrtnApplicationByContractTypeDtoFilter
        {
            RegionId = options.RegionId,
            ByRegion = true
        });
        result.CertificateFormedCount = (int)CertificateCount.CertificateTotals.TotalCount;
        result.VacanciesCount = (int)CertificateCount.CertificateTotals.TotalNewVacanciesCount;
        return result;
    }
    public List<PrtnContractTypeDto> GetPrtnContractTypeList(DashboardFilterOption options)
    {
        //var query = _unitOfWork.Context.Set<PrtnApplication>()
        //   .Include(a => a.Application)
        //   .ThenInclude(a => a.PrtnContract)
        //   .ThenInclude(a => a.PrtnCertificate)
        //   .Where(a => (!options.MfyId.HasValue || options.MfyId == (a.MfyId.Value)));

        //query = query.Where(a =>
        //        (!options.RegionId.HasValue
        //            || options.RegionId ==
        //            (a.ChooseLocation
        //                ? a.ChoosedRegionId : a.Application.RegionId))

        //     && (!options.DistrictId.HasValue
        //            || options.DistrictId ==
        //            (a.ChooseLocation
        //                ? a.ChoosedDistrictId : a.Application.DistrictId)));


        var prtnApplication = _applicationService.GetListMethod(new PrtnDocumentSortFilterOptions
        {
            RegionId = options.RegionId,
            StatusId = 2,
            StatusIds = new int[] { 2 }
        });

        var prtnContract = _prtnContractService.GetPrtnContractListDto(new PrtnDocumentSortFilterOptions
        {
            RegionId = options.RegionId,
            StatusId = 21,
            StatusIds = new int[] { 21 }
        });

        var prtnCertificate = _prtnCertificateService.GetPrtnCertificateListDto(new PrtnDocumentSortFilterOptions
        {
            RegionId = options.RegionId,
            StatusId = 26,
            StatusIds = new int[] { 26 }
        });

        var types = _unitOfWork.Context.Set<PrtnContractType>()
            .Include(x => x.Translates).ToList();

        List<PrtnContractTypeDto> result = new();
        foreach (var type in types)
        {
            var _prtnApplication = prtnApplication.Where(a => a.PrtnContractTypeId == type.Id);
            var _prtnContract = prtnContract.Where(p => p.PrtnContractTypeId == type.Id);
            var _prtnCertificate = prtnCertificate.Where(p => p.PrtnContractTypeId == type.Id);

            PrtnContractTypeDto temp = new();
            temp.PrtnContractTypeId = type.Id;
            temp.PrtnContractType = type.Translates.AsQueryable()
                 .FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? type.FullName;



            temp.PrtnApplication.Add(new PrtnContractColumn()
            {
                VacancyCount = _prtnApplication.Sum(x => x.PrtnApplicationNewVacanciesCount) ?? 0,
                ContractorCount = _prtnApplication.Select(x => x.ContractorId).Distinct().Count()
            });


            temp.PrtnContract.Add(new PrtnContractColumn()
            {
                VacancyCount = _prtnContract.Sum(x => x.NewVacanciesCount),
                ContractorCount = _prtnContract.Select(x => x.ContractorId).Distinct().Count()
            });

            temp.PrtnCertificate.Add(new PrtnContractColumn()
            {
                VacancyCount = _prtnCertificate.Sum(x => x.NewVacanciesCount),
                ContractorCount = _prtnCertificate.Select(x => x.ContractorId).Distinct().Count()
            });

            result.Add(temp);
        }

        return result;
    }

    /// <summary>
    /// C O N T R A C T  R A T E
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public List<PrtnDocumentsRegionRate> GetPrtnDocumentsRegionRate(RegionRateFilterOption options)
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

        Dictionary<long, string> mfys = new();
        foreach (var ent in _unitOfWork.Context.Set<Mfy>().ToList())
            mfys.Add(ent.Id, ent.FullName);

        return options.TableId switch
        {
            TableIdConst.DOC_PRTN_APPLICATION => ByPrtnApplication(options, regions, districts, mfys),
            TableIdConst.DOC_PRTN_CONTRACT => ByPrtnContract(options, regions, districts, mfys),
            TableIdConst.DOC_PRTN_CERTIFICATE => ByPrtnCertificate(options, regions, districts, mfys),
            _ => null
        };
    }
    public List<PrtnDocumentsRegionRate> GetPrtnContractsRegionRate(RegionRateFilterOption options)
    {
        var prtnApplication = _applicationService.GetListMethod(new PrtnDocumentSortFilterOptions
        {
            RegionId = options.RegionId,
            StatusId = 2,
            StatusIds = new int[] { 2 }
        });

        var prtnContract = _prtnContractService.GetPrtnContractListDto(new PrtnDocumentSortFilterOptions
        {
            RegionId = options.RegionId,
            StatusId = 21,
            StatusIds = new int[] { 21 }
        });

        var prtnCertificate = _prtnCertificateService.GetPrtnCertificateListDto(new PrtnDocumentSortFilterOptions
        {
            RegionId = options.RegionId,
            StatusId = 26,
            StatusIds = new int[] { 26 }
        });


        var _prtnApplication = prtnApplication.Where(a => a.PrtnContractTypeId == options.PrtnContractTypeId);
        var _prtnContract = prtnContract.Where(p => p.PrtnContractTypeId == options.PrtnContractTypeId);
        var _prtnCertificate = prtnCertificate.Where(p => p.PrtnContractTypeId == options.PrtnContractTypeId);

        if (options.TableId == 82)
        {
            List<PrtnDocumentsRegionRate> PrtnApplications = new List<PrtnDocumentsRegionRate>();
            foreach (var ent in _unitOfWork.Context.Set<Region>().Include(r => r.Translates).ToList())
            {
                PrtnApplications.Add(new PrtnDocumentsRegionRate
                {
                    Region = ent.FullName,
                    RegionId = ent.Id,
                    RegionOrderCode = ent.OrderCode,
                    //DocCount = _prtnApplication.Where(a => a.ContractorRegionId == ent.Id).Select(x => x.ContractorId).Distinct().Count()
                    DocCount = options.PrtnContractTypeId == null 
                    ? prtnApplication.Where(a => a.ContractorRegionId == ent.Id).Sum(i => i.PrtnApplicationNewVacanciesCount).Value 
                    : _prtnApplication.Where(a => a.ContractorRegionId == ent.Id).Sum(i => i.PrtnApplicationNewVacanciesCount).Value,

                });
            }
            return PrtnApplications;
        }

        if (options.TableId == 83)
        {
            List<PrtnDocumentsRegionRate> PrtnContracts = new List<PrtnDocumentsRegionRate>();
            foreach (var ent in _unitOfWork.Context.Set<Region>().Include(r => r.Translates).ToList())
            {
                PrtnContracts.Add(new PrtnDocumentsRegionRate
                {
                    Region = ent.FullName,
                    RegionId = ent.Id,
                    RegionOrderCode = ent.OrderCode,
                    DocCount = options.PrtnContractTypeId == null 
                    ? prtnContract.Where(a => a.ContractorRegionId == ent.Id).Sum(i => i.NewVacanciesCount) 
                    : _prtnContract.Where(a => a.ContractorRegionId == ent.Id).Sum(i => i.NewVacanciesCount)

                });
            }
            return PrtnContracts;
        }
        if (options.TableId == 84)
        {

            List<PrtnDocumentsRegionRate> PrtnCertificates = new List<PrtnDocumentsRegionRate>();
            foreach (var ent in _unitOfWork.Context.Set<Region>().Include(r => r.Translates).ToList())
            {
                PrtnCertificates.Add(new PrtnDocumentsRegionRate
                {
                    Region = ent.FullName,
                    RegionId = ent.Id,
                    RegionOrderCode = ent.OrderCode,
                    DocCount = options.PrtnContractTypeId == null 
                    ? prtnCertificate.Where(a => a.ContractorRegionId == ent.Id).Sum(i => i.NewVacanciesCount) 
                    : _prtnCertificate.Where(a => a.ContractorRegionId == ent.Id).Sum(i => i.NewVacanciesCount)

                });
            }
            return PrtnCertificates;
        }

        return null;

    }
    private List<PrtnDocumentsRegionRate> ByPrtnApplication(
        RegionRateFilterOption options,
        Dictionary<int, string> regions,
        Dictionary<int, string> districts,
        Dictionary<long, string> mfys)
    {
        var query = _unitOfWork.Context.Set<PrtnApplication>()
                    .Include(a => a.Application)
                    .Where(c => !options.PrtnContractTypeId.HasValue || options.PrtnContractTypeId == c.PrtnContractTypeId);

        List<PrtnDocumentsRegionRate> res = new();
        if (options.DistrictId.HasValue)
            res = query
                .Where(a => a.ChooseLocation
                    ? a.ChoosedDistrictId == options.DistrictId
                    : a.Application.DistrictId == options.DistrictId)
                .Select(a => new PrtnDocumentsRegionRate()
                {
                    MfyId = a.MfyId,
                    MfyOrderCode = a.Mfy.OrderCode,
                    DocCount = 1
                })
                .GroupBy(a => new { a.MfyId, a.MfyOrderCode })
                .Select(a => new PrtnDocumentsRegionRate()
                {
                    MfyId = a.Key.MfyId,
                    MfyOrderCode = a.Key.MfyOrderCode,
                    Region = a.Key.MfyId.HasValue ? mfys[(long)a.Key.MfyId] : string.Empty,
                    DocCount = a.Sum(b => b.DocCount)
                })
                .OrderBy(a => a.MfyOrderCode)
                .ToList();

        else if (options.RegionId.HasValue)
            res = query
                .Where(a => a.ChooseLocation
                    ? a.ChoosedRegionId == options.RegionId
                    : a.Application.RegionId == options.RegionId)
                .Select(a => new PrtnDocumentsRegionRate()
                {
                    DistrictOrderCode = a.ChooseLocation ? a.ChoosedDistrict.OrderCode : a.Application.District.OrderCode,
                    DistrictId = a.ChooseLocation ? a.ChoosedDistrict.Id : a.Application.DistrictId,
                    DocCount = 1
                })
                .GroupBy(a => new { a.DistrictId, a.DistrictOrderCode })
                .Select(a => new PrtnDocumentsRegionRate()
                {
                    DistrictId = a.Key.DistrictId,
                    DistrictOrderCode = a.Key.DistrictOrderCode,
                    Region = districts[(int)a.Key.DistrictId],
                    DocCount = a.Sum(b => b.DocCount)
                })
                .OrderBy(a => a.DistrictOrderCode)
                .ToList();

        else
            res = query
                .Select(a => new PrtnDocumentsRegionRate()
                {
                    RegionOrderCode = a.ChooseLocation ? a.ChoosedRegion.OrderCode : a.Application.Region.OrderCode,
                    RegionId = a.ChooseLocation ? a.ChoosedRegion.Id : a.Application.RegionId,
                    DocCount = 1
                })
                .GroupBy(a => new { a.RegionId, a.RegionOrderCode })
                .Select(a => new PrtnDocumentsRegionRate
                {
                    RegionOrderCode = a.Key.RegionOrderCode,
                    RegionId = a.Key.RegionId,
                    Region = regions[(int)a.Key.RegionId],
                    DocCount = a.Sum(b => b.DocCount)
                })
                .OrderBy(a => a.RegionOrderCode)
                .ToList();

        return res;
    }
    private List<PrtnDocumentsRegionRate> ByPrtnContract(
        RegionRateFilterOption options,
        Dictionary<int, string> regions,
        Dictionary<int, string> districts,
        Dictionary<long, string> mfys)
    {
        var query = _unitOfWork.Context.Set<PrtnContract>()
                    .Include(a => a.Application)
                    .Where(c => !options.PrtnContractTypeId.HasValue || options.PrtnContractTypeId == c.PrtnContractTypeId);

        List<PrtnDocumentsRegionRate> res = new();
        if (options.DistrictId.HasValue)
            res = query
                .Where(a => a.Application.PrtnApplication.ChooseLocation
                    ? a.Application.PrtnApplication.ChoosedDistrictId == options.DistrictId
                    : a.Application.DistrictId == options.DistrictId)
                .Select(a => new PrtnDocumentsRegionRate()
                {
                    MfyId = a.Application.PrtnApplication.MfyId,
                    MfyOrderCode = a.Application.PrtnApplication.Mfy.OrderCode,
                    DocCount = 1
                })
                .GroupBy(a => new { a.MfyId, a.MfyOrderCode })
                .Select(a => new PrtnDocumentsRegionRate()
                {
                    Region = a.Key.MfyId.HasValue ? mfys[(long)a.Key.MfyId] : string.Empty,
                    MfyOrderCode = a.Key.MfyOrderCode,
                    MfyId = a.Key.MfyId,
                    DocCount = a.Sum(b => b.DocCount)
                })
                .OrderBy(a => a.MfyOrderCode)
                .ToList();

        else if (options.RegionId.HasValue)
            res = query
                .Where(a => a.Application.PrtnApplication.ChooseLocation
                    ? a.Application.PrtnApplication.ChoosedRegionId == options.RegionId
                    : a.Application.RegionId == options.RegionId)
                .Select(a => new PrtnDocumentsRegionRate()
                {
                    DistrictOrderCode = a.Application.PrtnApplication.ChooseLocation
                        ? a.Application.PrtnApplication.ChoosedDistrict.OrderCode
                        : a.Application.District.OrderCode,
                    DistrictId = a.Application.PrtnApplication.ChooseLocation
                        ? a.Application.PrtnApplication.ChoosedDistrict.Id
                        : a.Application.DistrictId,
                    DocCount = 1
                })
                .GroupBy(a => new { a.DistrictId, a.DistrictOrderCode })
                .Select(a => new PrtnDocumentsRegionRate()
                {
                    DistrictId = a.Key.DistrictId,
                    DistrictOrderCode = a.Key.DistrictOrderCode,
                    Region = districts[(int)a.Key.DistrictId],
                    DocCount = a.Sum(b => b.DocCount)
                })
                .OrderBy(a => a.DistrictOrderCode)
                .ToList();

        else
            res = query
            .Select(a => new PrtnDocumentsRegionRate()
            {
                RegionOrderCode = a.Application.PrtnApplication.ChooseLocation
                    ? a.Application.PrtnApplication.ChoosedRegion.OrderCode
                    : a.Application.Region.OrderCode,
                RegionId = a.Application.PrtnApplication.ChooseLocation
                    ? a.Application.PrtnApplication.ChoosedRegion.Id
                    : a.Application.RegionId,
                DocCount = 1
            })
            .GroupBy(a => new { a.RegionId, a.RegionOrderCode })
            .Select(a => new PrtnDocumentsRegionRate
            {
                RegionOrderCode = a.Key.RegionOrderCode,
                RegionId = a.Key.RegionId,
                Region = regions[(int)a.Key.RegionId],
                DocCount = a.Sum(b => b.DocCount)
            })
            .OrderBy(a => a.RegionOrderCode)
            .ToList();

        return res;
    }
    private List<PrtnDocumentsRegionRate> ByPrtnCertificate(
        RegionRateFilterOption options,
        Dictionary<int, string> regions,
        Dictionary<int, string> districts,
        Dictionary<long, string> mfys)
    {
        var query = _unitOfWork.Context.Set<PrtnCertificate>()
                    .Include(a => a.PrtnContract)
                    .ThenInclude(a => a.Application)
                    .ThenInclude(a => a.PrtnApplication)
                    .Where(c => !options.PrtnContractTypeId.HasValue || options.PrtnContractTypeId == c.PrtnContractTypeId);

        List<PrtnDocumentsRegionRate> res = new();
        if (options.DistrictId.HasValue)
            res = query
                .Where(a => a.PrtnContract.Application.PrtnApplication.ChooseLocation
                    ? a.PrtnContract.Application.PrtnApplication.ChoosedDistrictId == options.DistrictId
                    : a.PrtnContract.Application.DistrictId == options.DistrictId)
                .Select(a => new PrtnDocumentsRegionRate()
                {
                    MfyId = a.PrtnContract.Application.PrtnApplication.MfyId,
                    MfyOrderCode = a.PrtnContract.Application.PrtnApplication.Mfy.OrderCode,
                    DocCount = 1
                })
                .GroupBy(a => new { a.MfyId, a.MfyOrderCode })
                .Select(a => new PrtnDocumentsRegionRate()
                {
                    Region = a.Key.MfyId.HasValue ? mfys[(long)a.Key.MfyId] : string.Empty,
                    MfyOrderCode = a.Key.MfyOrderCode,
                    MfyId = a.Key.MfyId,
                    DocCount = a.Sum(b => b.DocCount)
                })
                .OrderBy(a => a.MfyOrderCode)
                .ToList();

        else if (options.RegionId.HasValue)
            res = query
                .Where(a => a.PrtnContract.Application.PrtnApplication.ChooseLocation
                    ? a.PrtnContract.Application.PrtnApplication.ChoosedRegionId == options.RegionId
                    : a.PrtnContract.Application.RegionId == options.RegionId)
                .Select(a => new PrtnDocumentsRegionRate()
                {
                    DistrictOrderCode = a.PrtnContract.Application.PrtnApplication.ChooseLocation
                        ? a.PrtnContract.Application.PrtnApplication.ChoosedDistrict.OrderCode
                        : a.PrtnContract.Application.District.OrderCode,
                    DistrictId = a.PrtnContract.Application.PrtnApplication.ChooseLocation
                        ? a.PrtnContract.Application.PrtnApplication.ChoosedDistrict.Id
                        : a.PrtnContract.Application.DistrictId,
                    DocCount = 1
                })
                .GroupBy(a => new { a.DistrictId, a.DistrictOrderCode })
                .Select(a => new PrtnDocumentsRegionRate()
                {
                    DistrictId = a.Key.DistrictId,
                    DistrictOrderCode = a.Key.DistrictOrderCode,
                    Region = districts[(int)a.Key.DistrictId],
                    DocCount = a.Sum(b => b.DocCount)
                })
                .OrderBy(a => a.DistrictOrderCode)
                .ToList();

        else
            res = query.Select(a => new PrtnDocumentsRegionRate()
            {
                RegionOrderCode = a.PrtnContract.Application.PrtnApplication.ChooseLocation
                       ? a.PrtnContract.Application.PrtnApplication.ChoosedRegion.OrderCode
                       : a.PrtnContract.Application.Region.OrderCode,
                RegionId = a.PrtnContract.Application.PrtnApplication.ChooseLocation
                       ? a.PrtnContract.Application.PrtnApplication.ChoosedRegion.Id
                       : a.PrtnContract.Application.RegionId,
                DocCount = 1
            })
            .GroupBy(a => new { a.RegionId, a.RegionOrderCode })
            .Select(a => new PrtnDocumentsRegionRate
            {
                RegionOrderCode = a.Key.RegionOrderCode,
                RegionId = a.Key.RegionId,
                Region = regions[(int)a.Key.RegionId],
                DocCount = a.Sum(b => b.DocCount)
            })
            .OrderBy(a => a.RegionOrderCode)
            .ToList();

        return res;
    }

}