using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OpenXmlPowerTools;
using SspUis.BizLogicLayer.ApplicationServices;
using SspUis.BizLogicLayer.Claim;
using SspUis.BizLogicLayer.Doc;
using SspUis.BizLogicLayer.Doc.MonoApplicationServices;
using SspUis.BizLogicLayer.ExecutionApplicationServices;
using SspUis.BizLogicLayer.Hrm;
using SspUis.BizLogicLayer.ManualServices;
using SspUis.BizLogicLayer.MonoApplicationServices;
using SspUis.BizLogicLayer.PrtnContractServices;
using SspUis.BizLogicLayer.ReportServices.Main;
using SspUis.BizLogicLayer.ReportServices.Main.ReportDtos.Memship;
using SspUis.BizLogicLayer.ReportServices.Main.ReportDtos.Partner;
using SspUis.BizLogicLayer.Srv.Doc.SrvDeedService.QueryObject;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Appeal;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.EfClasses.Corruption;
using SspUis.DataLayer.EfClasses.Exapidata;
using SspUis.DataLayer.EfClasses.Exapidata.Tax;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.EfClasses.Memship;
using SspUis.DataLayer.EfClasses.Report;
using SspUis.DataLayer.EfClasses.Report.Func;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.Bandlik.Models;
using SspUis.Integration.Bandlik.Services;
using SspUis.Integration.BankCredit;
using SspUis.Integration.BankCredit.Models;
using StatusGeneric;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Models;
using WEBASE.Storage;
using WEBASE.Utility;

namespace SspUis.BizLogicLayer.ReportServices
{
	public partial class ReportService : StatusGenericHandler, IReportService
	{
		#region INJECT
		private readonly IUnitOfWork _unitOfWork;
		private readonly IBandlikService _bandlikService;
		private readonly IAuthService _authService;
		private readonly SystemConf _systemConf;
		private readonly IStorageService _storageService;
		private readonly ICultureHelper _cultureHelper;
		private readonly IBankCreditService _bankCreditService;
		private readonly IApplicationRepository _applicationRepository;
		private readonly IManualService _manualService;
		private readonly IPrtnContractService _prtnContractService;
		private readonly IExecutionApplicationService _executionApplication;
		private readonly IApplicationService _applicationService;
		private readonly IServiceDeedRepository _serviceDeedRepository;
		private readonly IServiceApplicationRepository _serviceApplicationRepository;
		private readonly IMonoApplicationResultService _monoApplicationResultService;

		private readonly DbContext _context;
		public ReportService(IUnitOfWork unitOfWork,
			IBandlikService bandlikService,
			SystemConf systemConf,
			IAuthService authService,
			IStorageService storageService,
			IBankCreditService bankCreditService,
			ICultureHelper cultureHelper,
			IManualService manualService,
			IExecutionApplicationService executionApplication,
			IPrtnContractService prtnContractService,
			DbContext context,
			IApplicationRepository applicationRepository,
			IApplicationService applicationService,
			IServiceDeedRepository serviceDeedRepository,
			IServiceApplicationRepository serviceApplicationRepository,
			IMonoApplicationResultService monoApplicationResultService)
		{
			_unitOfWork = unitOfWork;
			_bandlikService = bandlikService;
			_systemConf = systemConf;
			_authService = authService;
			_storageService = storageService;
			_cultureHelper = cultureHelper;
			_bankCreditService = bankCreditService;
			_manualService = manualService;
			_executionApplication = executionApplication;
			_prtnContractService = prtnContractService;
			_context = context;
			_applicationRepository = applicationRepository;
			_applicationService = applicationService;
			_serviceDeedRepository = serviceDeedRepository;
			_serviceApplicationRepository = serviceApplicationRepository;
			_monoApplicationResultService = monoApplicationResultService;
		}
		#endregion

		#region MEMSHIP
		public List<MemshipNewContractorReportDto> GetMemshipNewContractorReportList(MemshipNewContractorReportDtoFilter option)
		{
			var ratingLists = _unitOfWork.Context.Set<ContractorRating>().Include(s => s.Translates);
			var regions = _unitOfWork.Context.Set<Region>().Include(x => x.Translates);
			// var newContractors = new MemshipNewContractorHelper[] {};
			var result = new List<MemshipNewContractorReportDto>();
			var lang = ServiceProvider.CultureHelper.CurrentCulture.Id;


			if (_authService.Organization.Id != OrganizationIdConst.SSP && option.ByRegion)
			{
				option.RegionId = _authService.Organization.RegionId;
				option.ByRegion = false;
				option.ByDistrict = true;
			}


			var memshipCertificates = _unitOfWork.Context.MemshipCertificates.Include(x => x.MemshipContract).Where(x => x.StatusId == StatusIdConst.FORMED).Select(x => new MemshipCertificateHelper()
			{
				DocOn = x.DocOn,
				ApplicationRegId = x.Contractor.RegionId,
				ApplicationDisId = x.Contractor.DistrictId,
				ContractorPinfl = x.Contractor.Pinfl,
				ContractorInn = x.Contractor.Inn
			}).ToArray();
			if (option.ByDistrict)
			{
				var newContractors = _unitOfWork.Context.MemshipNewContractors.Where(s => s.RegionId == option.RegionId).Include(s => s.Tables).Where(s => s.StatusId == StatusIdConst.ACCEPTED).Select(s => new MemshipNewContractorHelper
				{

				}).ToArray();
			}


			if (option.FromDocDate.HasValue)
			{
				memshipCertificates = memshipCertificates.Where(a => a.DocOn >= option.FromDocDate.Value).ToArray();
			}

			if (option.ToDocDate.HasValue)
			{
				memshipCertificates = memshipCertificates.Where(a => a.DocOn < option.ToDocDate.Value).ToArray();
			}
			//var ratingLists = _unitOfWork.Context.Set<ContractorRating>().Include(s => s.Translates);
			//         var regions = _unitOfWork.Context.Set<Region>().Include(x => x.Translates);
			//var newContractors = _unitOfWork.Context.Set<MemshipNewContractor>().Include(s => s.Tables).AsQueryable();
			//var result = new List<MemshipNewContractorReportDto>();
			//         var lang = ServiceProvider.CultureHelper.CurrentCulture.Id;

			////if (option.FromDate is not null)
			////{
			////	newContractors = newContractors.Where(s => s.FromDate >= option.FromDate);
			////}

			////if (option.RegionId.HasValue)
			////{
			//         //             var districts = _unitOfWork.Context.Set<District>().Where(s => s.RegionId == option.RegionId).Include(x => x.Translates);

			////}

			//if(option.RegionId is not null)
			//{

			//	foreach(var item in newContractors)
			//	{

			//	}
			//}




			return new List<MemshipNewContractorReportDto>();

			//          string GetRating(decimal score, IQueryable<ContractorRating> ratings,int contractorTypeId)
			//          {
			//             var rate =  ratings.FirstOrDefault(s => s.MinimumPercentage <= score && s.MaximumPercentage >= score && s.ContractorTypeId == contractorTypeId);
			//	return rate.Translates.AsQueryable().FirstOrDefault(ContractorRatingTranslate.GetExpr(TranslateColumn.full_name,
			//				lang))?.TranslateText ?? rate.FullName;

			//          }
			// Dictionary<int,int> GetLegalMemships(int? regionId = null)
			// {
			//	var result = new Dictionary<int, int>();
			//	var data = _unitOfWork.MemshipCertificateRepository.ReadAsNoTracked<MemshipCertificateListDto>().Where(s => s.ContractorPinfl.IsNullOrEmpty());
			//	if(regionId is not null)
			//	{
			//		data = data.Where(s => s.RegionId == regionId).OrderBy(s => s.DistrictId);

			//		foreach(var item in data)
			//		{
			//			result.Add(item.DistrictId,data.Where(s => s.DistrictId== item.DistrictId).Count());
			//		}

			//             }
			//	else
			//	{

			//                 foreach (var item in data)
			//                 {
			//                     result.Add(item.RegionId, data.Where(s => s.RegionId == item.RegionId).Count());
			//                 }
			//             }

			//	return result;

			// }

		}
		public List<MemshipReportDto> GetMemshipReports1(MemshipReportDtoFilter option)
		{
			if (_authService.Organization.Id != OrganizationIdConst.SSP)
			{
				option.RegionId = _authService.Organization.RegionId;
				option.ByRegion = true;
			}
			new NotImplementedException();
			var userOrg = _unitOfWork.Context.Set<Organization>()
				.FirstOrDefault(x => x.Id == _authService.User.OrganizationId);

			if ((userOrg.OrganizationGroupId == OrganizationGroupIdConst.SSP || userOrg.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)
				&& userOrg.Id != OrganizationIdConst.SSP)
				option.RegionId = userOrg.RegionId;

			var lang = ServiceProvider.CultureHelper.CurrentCulture.Id;
			var date = DateTime.Now;

			var regions = _unitOfWork.Context.Set<Region>()
				.Include(x => x.Translates);

			var result = new List<MemshipReportDto>();

			var memshipYearlyPlans = _unitOfWork.Context.Set<MemshipYearlyPlanTable>().Where(x => x.Owner.Year == date.Year).ToList();

			if (option.RegionId.HasValue)
			{
				var districts = _unitOfWork.Context.Set<District>()
				.Include(x => x.Translates)
				.Where(x => x.RegionId == option.RegionId);
				var region = regions.FirstOrDefault(x => x.Id == option.RegionId);
				foreach (var district in districts)
				{
					result.Add(new()
					{
						Region = region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name,
							lang))?.TranslateText ?? region.FullName,
						RegionId = region.Id,
						RegionOrderCode = region.OrderCode,
						District = district.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name,
							lang))?.TranslateText ?? district.FullName,
						DistrictId = district.Id,
						DistrictOrderCode = district.OrderCode,
						ByDistrict = true,
						MemshipGeneralPlan = memshipYearlyPlans.Where(x => x.MonthOn == date.Month && x.DistrictId == district.Id).Sum(x => x.MembersCount),
						MemshipGeneralPlanByYear = memshipYearlyPlans.Where(x => x.DistrictId == district.Id).Sum(x => x.MembersCount),
					});
				}
				result = result.OrderBy(x => x.DistrictOrderCode).ToList();
			}
			else
			{

				foreach (var region in regions)
				{
					result.Add(new()
					{
						RegionId = region.Id,
						Region = region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name,
							lang))?.TranslateText ?? region.FullName,
						RegionOrderCode = region.OrderCode,
						ByRegion = true,
						MemshipGeneralPlan = memshipYearlyPlans.Where(x => x.MonthOn == date.Month && x.RegionId == region.Id).Sum(x => x.MembersCount),
						MemshipGeneralPlanByYear = memshipYearlyPlans.Where(x => x.RegionId == region.Id).Sum(x => x.MembersCount),

					});

				}
				result = result.OrderBy(x => x.RegionOrderCode).ToList();
			}

			foreach (var item in result)
			{
				option.RegionId = item.RegionId;
				option.DistrictId = item.DistrictId;
				item.MemshipFactByMonth = Certificates(option).Where(x => x.DocOn.Month == date.Month && x.StatusId == StatusIdConst.FORMED).Count();
				item.MemshipFactByMonthPercentage = item.MemshipGeneralPlan == 0 ? 0 : (int)(item.MemshipFactByMonth / (item.MemshipGeneralPlan) * 100);

				item.MemshipApplicationLegal = Applications(option).Where(x => x.Application.Contractor.Pinfl == null && x.Application.DocOn.Month == date.Month).Count();
				item.MemshipApplicationYtt = Applications(option).Where(x => x.Application.Contractor.Pinfl != null && x.Application.DocOn.Month == date.Month).Count();
				item.MemshipCertificateLegal = Certificates(option).Where(x => x.Contractor.Pinfl == null && x.DocOn.Month == date.Month).Count();
				item.MemshipCertificateYtt = Certificates(option).Where(x => x.Contractor.Pinfl != null && x.DocOn.Month == date.Month).Count();
				item.MemshipFactByYear = Contracts(option).Count(x => x.StatusId == StatusIdConst.SIGNING);
				item.MemshipFactByYearPercentage = item.MemshipGeneralPlanByYear == 0 ? 0 : (int)((item.MemshipFactByYear / item.MemshipGeneralPlanByYear) * 100);

				item.MemshipApplicationCountByYear = Applications(option).Count();
				item.MemshipContractCountByYear = Contracts(option).Count(x => x.StatusId == StatusIdConst.FORMED);
				item.MemshipCertificateAcceptedCountByear = Certificates(option).Count(x => x.StatusId == StatusIdConst.FORMED);
				item.MemshipCertificateProgressCountByear = Certificates(option).Count(x => x.StatusId == StatusIdConst.SIGNED);
				item.MemshipCertificateNotIncludedCountByear = Certificates(option).Count(x => x.StatusId == StatusIdConst.REJECTED || x.StatusId == StatusIdConst.CANCELED);

				item.MemshipGeneralIndebtednessByYear = (item.MemshipGeneralPlanByYear - item.MemshipCertificateAcceptedCountByear) < 0 ? 0 : (item.MemshipGeneralPlanByYear - item.MemshipCertificateAcceptedCountByear);
				item.MemshipGeneralIndebtednessCoefficientByYear = item.MemshipGeneralPlanByYear == 0 ? 0 : (int)(item.MemshipGeneralPlanByYear / (item.MemshipContractCountByYear > 0 ? item.MemshipContractCountByYear : 1));
				item.MemshipGeneralIndebtednessPercentageByYear = item.MemshipGeneralPlanByYear == 0 ? 0 : (int)((item.MemshipGeneralIndebtednessByYear / item.MemshipGeneralPlanByYear) * 100);
			}

			return result;

			IQueryable<MemshipApplication> Applications(MemshipReportDtoFilter options)
			{
				//if (options.IsOld.Value)
				//{
				//    return new List<MemshipApplication>().AsQueryable();
				//}
				var query = _unitOfWork.Context.Set<MemshipApplication>()
					.Where(a => a.Application.StatusId != StatusIdConst.DELETED);

				//query = query.Where(a => option.IsOld.Value ? a.Application.DocOn <= new DateOnly(2023, 10, 1) : a.Application.DocOn >= new DateOnly(2023, 10, 1));
				query = query.Where(a => !options.RegionId.HasValue
							|| options.RegionId == (a.ChooseLocation ? a.ChoosedRegionId : a.Application.RegionId));

				query = query.Where(a => !options.DistrictId.HasValue
							|| options.DistrictId == (a.ChooseLocation ? a.ChoosedDistrictId : a.Application.DistrictId));

				query = query.Where(x => x.Application.DocOn.Year == date.Year);

				query = query.Where(a => !options.ContractorCategoryId.HasValue || a.ContractorCategoryId == options.ContractorCategoryId);
				return query;
			}

			IQueryable<MemshipCertificate> Certificates(MemshipReportDtoFilter options)
			{
				var query = _unitOfWork.Context.Set<MemshipCertificate>()
					.Include(c => c.Status)
					.Include(c => c.Contractor)
					.Include(c => c.MemshipContract)
					.Where(a => a.StatusId != StatusIdConst.DELETED);

				//query = query.Where(a => option.IsOld.Value ? a.DocOn <= new DateOnly(2023, 10, 1) : a.DocOn >= new DateOnly(2023, 10, 1));
				query = query.Where(a =>
					  (!options.RegionId.HasValue ||
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

				query = query.Where(x => x.DocOn.Year == date.Year);

				query = query.Where(a => !options.ContractorCategoryId.HasValue || a.MemshipContract.ContractorCategoryId == options.ContractorCategoryId);
				return query;
			}
			IQueryable<MemshipContract> Contracts(MemshipReportDtoFilter options)
			{
				var query = _unitOfWork.Context.Set<MemshipContract>()
					.Where(a => a.StatusId != StatusIdConst.DELETED);

				//query = query.Where(a => option.IsOld.Value ? a.DocOn <= new DateOnly(2023, 10, 1) : a.DocOn >= new DateOnly(2023, 10, 1));

				query = query.Where(a =>

					 (!options.RegionId.HasValue ||
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

				query = query.Where(x => x.DocOn.Year == date.Year);

				query = query.Where(a => !options.ContractorCategoryId.HasValue || a.ContractorCategoryId == options.ContractorCategoryId);
				return query;
			}
		}
		public List<MemshipReportFuncDto> GetMemshipReports(MemshipReportDtoFilter options)
		{
			if (_authService.Organization.Id != OrganizationIdConst.SSP)
			{
				options.RegionId = _authService.Organization.RegionId;
				options.ByRegion = true;
			}
			var userOrg = _unitOfWork.Context.Set<Organization>()
				.FirstOrDefault(x => x.Id == _authService.User.OrganizationId);

			if ((userOrg.OrganizationGroupId == OrganizationGroupIdConst.SSP || userOrg.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)
				&& userOrg.Id != OrganizationIdConst.SSP)
			{
				options.RegionId = userOrg.RegionId;
			}

			var languageId = _cultureHelper.CurrentCulture.Id;
			var result = _unitOfWork.Context.GetMemshipMainReport(
				options.RegionId,
				options.DistrictId,
				options.ByRegion,
				options.ByDistrict,
				options.ContractorCategoryId,
				languageId
				);
			return result.ToList();
		}
		public List<MemshipReportByOrganizationDto> GetMemshipReportByOrganization(MemshipReportByOrganizationDtoFilter dto)
		{
			if (_authService.Organization.Id != OrganizationIdConst.SSP && dto.ByRegion)
			{
				dto.RegionId = _authService.Organization.RegionId;
				dto.ByRegion = false;
				dto.ByDistrict = true;
			}
			var applications = _unitOfWork.Context.MemshipApplications.Include(x => x.Application).Where(x => x.Application.StatusId == StatusIdConst.ACCEPTED).ToArray();
			var memshipCertificates = _unitOfWork.Context.MemshipCertificates.Include(x => x.Contractor).Include(x => x.MemshipContract).Where(x => x.StatusId == StatusIdConst.FORMED).ToList();
			Console.WriteLine(applications.Length);
			Console.WriteLine(memshipCertificates.Count);


			var res = _unitOfWork.Context.MemshipContracts.Include(x => x.Application).ThenInclude(x => x.MemshipApplication).Where(x => x.StatusId == StatusIdConst.SIGNED).Select(x => new Helpme()
			{
				DocOn = x.DocOn,
				ApplicationRegId = x.RegionId,
				ApplicationDisId = x.DistrictId,
				CategoryId = x.Application.MemshipApplication.ContractorCategoryId,
				ContractorId = x.ContractorId
			}).ToArray();

			if (dto.FromDocDate.HasValue)
			{
				applications = applications.Where(a => a.Application.DocOn >= dto.FromDocDate.Value).ToArray();
				memshipCertificates = memshipCertificates.Where(a => a.DocOn >= dto.FromDocDate.Value).ToList();
				res = res.Where(x => x.DocOn >= dto.FromDocDate.Value).ToArray();
			}

			if (dto.ToDocDate.HasValue)
			{
				applications = applications.Where(a => a.Application.DocOn < dto.ToDocDate.Value).ToArray();
				memshipCertificates = memshipCertificates.Where(a => a.DocOn < dto.ToDocDate.Value).ToList();
				res = res.Where(x => x.DocOn < dto.ToDocDate.Value).ToArray();
			}
			if (dto.IsOld.HasValue)
			{
				var dateForNew = new DateOnly(2023, 10, 1);
				applications = applications.Where(a => dto.IsOld.Value ? a.Application.DocOn < dateForNew : a.Application.DocOn >= dateForNew).ToArray();
				memshipCertificates = memshipCertificates.Where(a => dto.IsOld.Value ? a.DocOn < dateForNew : a.DocOn >= dateForNew).ToList();
				res = res.Where(a => dto.IsOld.Value ? a.DocOn < dateForNew : a.DocOn >= dateForNew).ToArray();
			}

			List<MemshipReportByOrganizationDto> data = new List<MemshipReportByOrganizationDto>();
			if (dto.ByRegion)
			{
				var regions = _unitOfWork.Context.Regions.ToArray();
				foreach (var item in regions)
				{
					data.Add(new MemshipReportByOrganizationDto()
					{
						RegionId = item.Id,
						RegionOrderCode = item.OrderCode,
						Region = item.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? item.FullName,
						DistrictId = null,
						District = null,
						ApprovedApplication = new ApprovedApplication()
						{
							TotalCount = applications.Where(x => (x.ChoosedRegionId.HasValue && x.ChoosedRegionId == item.Id) || (!x.ChoosedRegionId.HasValue && x.Application.RegionId == item.Id)).Count(),
							BigOrgCount = applications.Where(x => (x.ChoosedRegionId.HasValue && x.ChoosedRegionId == item.Id && x.ContractorCategoryId == 4) ||
							(!x.ChoosedRegionId.HasValue && x.Application.RegionId == item.Id && x.ContractorCategoryId == 4)).Count(),
							SmallOrgCount = applications.Where(x => (x.ChoosedRegionId.HasValue && x.ChoosedRegionId == item.Id && x.ContractorCategoryId == 3) ||
							(!x.ChoosedRegionId.HasValue && x.Application.RegionId == item.Id && x.ContractorCategoryId == 3)).Count(),
							MiddleOrgCount = applications.Where(x => (x.ChoosedRegionId.HasValue && x.ChoosedRegionId == item.Id && x.ContractorCategoryId == 1) ||
							(!x.ChoosedRegionId.HasValue && x.Application.RegionId == item.Id && x.ContractorCategoryId == 1)).Count(),
							MicroOrgCount = applications.Where(x => (x.ChoosedRegionId.HasValue && x.ChoosedRegionId == item.Id && x.ContractorCategoryId == 2) ||
							(!x.ChoosedRegionId.HasValue && x.Application.RegionId == item.Id && x.ContractorCategoryId == 2)).Count()
						},
						SignedApplication = new SignedApplication()
						{
							TotalCount = res.Where(x => x.ApplicationRegId == item.Id).Count(),
							BigOrgCount = res.Where(x => x.ApplicationRegId == item.Id && x.CategoryId == 4).Count(),
							SmallOrgCount = res.Where(x => x.ApplicationRegId == item.Id && x.CategoryId == 3).Count(),
							MiddleOrgCount = res.Where(x => x.ApplicationRegId == item.Id && x.CategoryId == 1).Count(),
							MicroOrgCount = res.Where(x => x.ApplicationRegId == item.Id && x.CategoryId == 2).Count()
						},
						CertificateGivenApplication = new CertificateGivenApplication()
						{
							TotalCount = memshipCertificates.Where(x => x.Contractor.RegionId == item.Id).Count(),
							BigOrgCount = memshipCertificates.Where(x => x.Contractor.RegionId == item.Id && x.MemshipContract.ContractorCategoryId == 4).Count(),
							SmallOrgCount = memshipCertificates.Where(x => x.Contractor.RegionId == item.Id && x.MemshipContract.ContractorCategoryId == 3).Count(),
							MiddleOrgCount = memshipCertificates.Where(x => x.Contractor.RegionId == item.Id && x.MemshipContract.ContractorCategoryId == 1).Count(),
							MicroOrgCount = memshipCertificates.Where(x => x.Contractor.RegionId == item.Id && x.MemshipContract.ContractorCategoryId == 2).Count(),
						}
					});
				}
				return data.OrderBy(x => x.RegionOrderCode).ToList();
			}

			if (dto.ByDistrict)
			{
				var districts = _unitOfWork.Context.Districts.Include(x => x.Region).Where(x => x.RegionId == dto.RegionId).ToArray();
				foreach (var item in districts)
				{
					data.Add(new MemshipReportByOrganizationDto()
					{
						RegionId = item.RegionId,
						Region = item.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? item.FullName,
						DistrictId = item.Id,
						District = item.FullName,
						DistrictOrderCode = item.OrderCode,
						ApprovedApplication = new ApprovedApplication()
						{
							TotalCount = applications.Where(x => x.Application.DistrictId == item.Id).Count(),
							BigOrgCount = applications.Where(x => x.Application.DistrictId == item.Id && x.ContractorCategoryId == 4).Count(),
							SmallOrgCount = applications.Where(x => x.Application.DistrictId == item.Id && x.ContractorCategoryId == 3).Count(),
							MiddleOrgCount = applications.Where(x => x.Application.DistrictId == item.Id && x.ContractorCategoryId == 1).Count(),
							MicroOrgCount = applications.Where(x => x.Application.DistrictId == item.Id && x.ContractorCategoryId == 2).Count(),
						},
						SignedApplication = new SignedApplication()
						{
							TotalCount = res.Where(x => x.ApplicationDisId == item.Id).Count(),
							BigOrgCount = res.Where(x => x.ApplicationDisId == item.Id && x.CategoryId == 4).Count(),
							SmallOrgCount = res.Where(x => x.ApplicationDisId == item.Id && x.CategoryId == 3).Count(),
							MiddleOrgCount = res.Where(x => x.ApplicationDisId == item.Id && x.CategoryId == 1).Count(),
							MicroOrgCount = res.Where(x => x.ApplicationDisId == item.Id && x.CategoryId == 2).Count()
						},
						CertificateGivenApplication = new CertificateGivenApplication()
						{
							TotalCount = memshipCertificates.Where(x => x.Contractor.DistrictId == item.Id).Count(),
							BigOrgCount = memshipCertificates.Where(x => x.Contractor.DistrictId == item.Id && x.MemshipContract.ContractorCategoryId == 4).Count(),
							SmallOrgCount = memshipCertificates.Where(x => x.Contractor.DistrictId == item.Id && x.MemshipContract.ContractorCategoryId == 3).Count(),
							MiddleOrgCount = memshipCertificates.Where(x => x.Contractor.DistrictId == item.Id && x.MemshipContract.ContractorCategoryId == 1).Count(),
							MicroOrgCount = memshipCertificates.Where(x => x.Contractor.DistrictId == item.Id && x.MemshipContract.ContractorCategoryId == 2).Count(),
						}
					});
				}
				return data.OrderBy(x => x.DistrictOrderCode).ToList();
			}

			if (dto.ByContractor)
			{
				var contractors = _unitOfWork.Context.Contractors.Where(x => x.DistrictId == dto.DistrictId).ToArray();
				foreach (var item in contractors)
				{
					var t = new MemshipReportByOrganizationDto()
					{
						ContractorInn = item.Inn,
						Contractor = item.FullName,

						ApprovedApplication = new ApprovedApplication()
						{
							TotalCount = applications.Where(x => x.Application.ContractorId == item.Id).Count(),
							BigOrgCount = applications.Where(x => x.Application.ContractorId == item.Id && x.ContractorCategoryId == 4).Count(),
							SmallOrgCount = applications.Where(x => x.Application.ContractorId == item.Id && x.ContractorCategoryId == 3).Count(),
							MiddleOrgCount = applications.Where(x => x.Application.ContractorId == item.Id && x.ContractorCategoryId == 1).Count(),
							MicroOrgCount = applications.Where(x => x.Application.ContractorId == item.Id && x.ContractorCategoryId == 2).Count(),
						},
						SignedApplication = new SignedApplication()
						{
							TotalCount = res.Where(x => x.ContractorId == item.Id).Count(),
							BigOrgCount = res.Where(x => x.ContractorId == item.Id && x.CategoryId == 4).Count(),
							SmallOrgCount = res.Where(x => x.ContractorId == item.Id && x.CategoryId == 3).Count(),
							MiddleOrgCount = res.Where(x => x.ContractorId == item.Id && x.CategoryId == 1).Count(),
							MicroOrgCount = res.Where(x => x.ContractorId == item.Id && x.CategoryId == 2).Count()
						},
						CertificateGivenApplication = new CertificateGivenApplication()
						{
							TotalCount = memshipCertificates.Where(x => x.ContractorId == item.Id).Count(),
							BigOrgCount = memshipCertificates.Where(x => x.ContractorId == item.Id && x.MemshipContract.ContractorCategoryId == 4).Count(),
							SmallOrgCount = memshipCertificates.Where(x => x.ContractorId == item.Id && x.MemshipContract.ContractorCategoryId == 3).Count(),
							MiddleOrgCount = memshipCertificates.Where(x => x.ContractorId == item.Id && x.MemshipContract.ContractorCategoryId == 1).Count(),
							MicroOrgCount = memshipCertificates.Where(x => x.ContractorId == item.Id && x.MemshipContract.ContractorCategoryId == 2).Count(),
						}
					};
					if (t.ApprovedApplication.TotalCount > 0 || t.SignedApplication.TotalCount > 0 || t.CertificateGivenApplication.TotalCount > 0)
						data.Add(t);
				}
			}

			return data;

		}
		public List<MemshipReportByPersonType> GetMemshipReportByPersonTypes(MemshipReportByPersonTypeFilter dto)
		{
			if (_authService.Organization.Id != OrganizationIdConst.SSP && dto.ByRegion)
			{
				dto.RegionId = _authService.Organization.RegionId;
				dto.ByRegion = false;
				dto.ByDistrict = true;
			}
			var memshipContract = _unitOfWork.Context.MemshipContracts.Where(x => x.StatusId == StatusIdConst.SIGNED).Select(x => new MemshipContractHelper()
			{
				DocOn = x.DocOn,
				ApplicationRegId = x.RegionId,
				ApplicationDisId = x.DistrictId,
				ContractorInn = x.Contractor.Inn,
				ContractorPinfl = x.Contractor.Pinfl
			}).ToArray();

			var memshipApplication = _unitOfWork.Context.MemshipApplications.Include(x => x.Application).Where(x => x.Application.StatusId == StatusIdConst.ACCEPTED).Select(x => new MemshipApplicationHelper()
			{
				DocOn = x.Application.DocOn,
				ApplicationRegId = x.ChoosedRegionId != null ? x.ChoosedRegionId : x.Application.RegionId,
				ApplicationDisId = x.ChoosedDistrictId != null ? x.ChoosedDistrictId : x.Application.DistrictId,
				ContractorInn = x.Application.Contractor.Inn,
				ContractorPinfl = x.Application.Contractor.Pinfl
			}).ToArray();

			var memshipCertificates = _unitOfWork.Context.MemshipCertificates.Include(x => x.MemshipContract).Where(x => x.StatusId == StatusIdConst.FORMED).Select(x => new MemshipCertificateHelper()
			{
				DocOn = x.DocOn,
				ApplicationRegId = x.Contractor.RegionId,
				ApplicationDisId = x.Contractor.DistrictId,
				ContractorPinfl = x.Contractor.Pinfl,
				ContractorInn = x.Contractor.Inn
			}).ToArray();


			if (dto.FromDocDate.HasValue)
			{
				memshipApplication = memshipApplication.Where(a => a.DocOn >= dto.FromDocDate.Value).ToArray();
				memshipCertificates = memshipCertificates.Where(a => a.DocOn >= dto.FromDocDate.Value).ToArray();
				memshipContract = memshipContract.Where(x => x.DocOn >= dto.FromDocDate.Value).ToArray();
			}

			if (dto.ToDocDate.HasValue)
			{
				memshipApplication = memshipApplication.Where(a => a.DocOn < dto.ToDocDate.Value).ToArray();
				memshipCertificates = memshipCertificates.Where(a => a.DocOn < dto.ToDocDate.Value).ToArray();
				memshipContract = memshipContract.Where(x => x.DocOn < dto.ToDocDate.Value).ToArray();
			}
			if (dto.IsOld.HasValue && (bool)dto.IsOld)
			{
				var dateForNew = new DateOnly(2023, 10, 1);
				memshipApplication = memshipApplication.Where(a => a.DocOn < dateForNew).ToArray();
				memshipCertificates = memshipCertificates.Where(a => a.DocOn < dateForNew).ToArray();
				memshipContract = memshipContract.Where(a => a.DocOn < dateForNew).ToArray();
			}
			if (dto.IsOld.HasValue && (bool)!dto.IsOld)
			{
				var dateForNew = new DateOnly(2023, 10, 1);
				memshipApplication = memshipApplication.Where(a => a.DocOn > dateForNew).ToArray();
				memshipCertificates = memshipCertificates.Where(a => a.DocOn > dateForNew).ToArray();
				memshipContract = memshipContract.Where(a => a.DocOn > dateForNew).ToArray();
			}

			List<MemshipReportByPersonType> data = new();
			if (dto.ByRegion)
			{
				var regions = _unitOfWork.Context.Regions.ToArray();
				foreach (var item in regions)
				{
					data.Add(new MemshipReportByPersonType()
					{
						RegionId = item.Id,
						RegionOrderCode = item.OrderCode,
						Region = item.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? item.FullName,
						DistrictId = null,
						District = null,
						TotalAcceptedApplication = new TotalAcceptedApplication()
						{
							LegalPersonCount = memshipApplication.Where(x => x.ContractorPinfl == null && x.ApplicationRegId == item.Id).Count(),
							PhysicalPersonCount = memshipApplication.Where(x => x.ContractorPinfl != null && x.ApplicationRegId == item.Id).Count()
						},
						TotalSignedApplication = new TotalSignedApplication()
						{
							LegalPersonCount = memshipContract.Where(x => x.ContractorPinfl == null && x.ApplicationRegId == item.Id).Count(),
							PhysicalPersonCount = memshipContract.Where(x => x.ContractorPinfl != null && x.ApplicationRegId == item.Id).Count()
						},
						TotalGivenCertificateCount = new TotalGivenCertificateCount()
						{
							LegalPersonCount = memshipCertificates.Where(x => x.ContractorPinfl == null && x.ApplicationRegId == item.Id).Count(),
							PhysicalPersonCount = memshipCertificates.Where(x => x.ContractorPinfl != null && x.ApplicationRegId == item.Id).Count()
						}
					});
				}
				return data.OrderBy(x => x.RegionOrderCode).ToList();
			}
			if (dto.ByDistrict)
			{
				var districts = _unitOfWork.Context.Districts.Include(x => x.Region).Where(x => x.RegionId == dto.RegionId && x.StateId != StateIdConst.PASSIVE).ToArray();
				foreach (var item in districts)
				{
					data.Add(new MemshipReportByPersonType()
					{
						//RegionId = item.RegionId,
						//Region = item.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? item.FullName,
						DistrictId = item.Id,
						DistrictOrderCode = item.OrderCode,
						District = item.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? item.FullName,
						TotalAcceptedApplication = new TotalAcceptedApplication()
						{
							LegalPersonCount = memshipApplication.Where(x => x.ContractorPinfl == null && x.ApplicationDisId == item.Id).Count(),
							PhysicalPersonCount = memshipApplication.Where(x => x.ContractorPinfl != null && x.ApplicationDisId == item.Id).Count()
						},
						TotalSignedApplication = new TotalSignedApplication()
						{
							LegalPersonCount = memshipContract.Where(x => x.ContractorPinfl == null && x.ApplicationDisId == item.Id).Count(),
							PhysicalPersonCount = memshipContract.Where(x => x.ContractorPinfl != null && x.ApplicationDisId == item.Id).Count()
						},
						TotalGivenCertificateCount = new TotalGivenCertificateCount()
						{
							LegalPersonCount = memshipCertificates.Where(x => x.ContractorPinfl == null && x.ApplicationDisId == item.Id).Count(),
							PhysicalPersonCount = memshipCertificates.Where(x => x.ContractorPinfl != null && x.ApplicationDisId == item.Id).Count()
						}
					});
				}
				return data.OrderBy(x => x.DistrictOrderCode).ToList();
			}
			if (dto.ByContractor)
			{
				var contractors = _unitOfWork.Context.Contractors.Where(x => x.DistrictId == dto.DistrictId).ToArray();
				foreach (var item in contractors)
				{
					data.Add(new MemshipReportByPersonType()
					{
						ContractorInn = item.Inn,
						Contractor = item.FullName,
						TotalAcceptedApplication = new TotalAcceptedApplication()
						{
							LegalPersonCount = memshipApplication.Where(x => x.ContractorPinfl == null && x.ContractorId == item.Id).Count(),
							PhysicalPersonCount = memshipApplication.Where(x => x.ContractorPinfl != null && x.ContractorId == item.Id).Count()
						},
						TotalSignedApplication = new TotalSignedApplication()
						{
							LegalPersonCount = memshipContract.Where(x => x.ContractorPinfl == null && x.ContractorId == item.Id).Count(),
							PhysicalPersonCount = memshipContract.Where(x => x.ContractorPinfl != null && x.ContractorId == item.Id).Count()
						},
						TotalGivenCertificateCount = new TotalGivenCertificateCount()
						{
							LegalPersonCount = memshipCertificates.Where(x => x.ContractorPinfl == null && x.ContractorId == item.Id).Count(),
							PhysicalPersonCount = memshipCertificates.Where(x => x.ContractorPinfl != null && x.ContractorId == item.Id).Count()
						}
					});
				}
			}
			return data;
		}
		public MemshipDocsInfoReestrDto GetMemshipDocsInfoReestr(MemshipDocsInfoReestrDtoFilter options)
		{

			if (_authService.Organization.Id != OrganizationIdConst.SSP)
				options.RegionId = _authService.Organization.RegionId;

			var languageId = _cultureHelper.CurrentCulture.Id;
			MemshipDocsInfoReestrDto result = new();
			result.Rows = _unitOfWork.Context.GetMemshipDocsInfoReestr(
				pr_region_id: options.RegionId,
				pr_district_id: options.DistrictId,
				pr_memship_contract_type_id: options.MemshipContractTypeId,
				pr_contractor_category_id: options.ContractorCategoryId,
				pr_from_doc_date: options.FromDocDate,
				pr_to_doc_date: options.ToDocDate,
				pr_is_old: options.IsOld,
				pr_is_pinfl: options.IsPinfl,
				pr_language_id: languageId,
				pr_search: options.Search,
				pr_offset: options.Page * options.PageSize,
				pr_limit: options.PageSize
				).ToList();

			result.Count = _unitOfWork.Context.GetMemshipDocsInfoReestrCount(
				pr_region_id: options.RegionId,
				pr_district_id: options.DistrictId,
				pr_memship_contract_type_id: options.MemshipContractTypeId,
				pr_contractor_category_id: options.ContractorCategoryId,
				pr_from_doc_date: options.FromDocDate,
				pr_to_doc_date: options.ToDocDate,
				pr_is_old: options.IsOld,
				pr_is_pinfl: options.IsPinfl,
				pr_language_id: languageId,
				pr_search: options.Search).FirstOrDefault().TotalCount;
			return result;
		}
		public List<MemshipDocsInfoDto> GetMemshipDocsInfo(MemshipDocsInfoDtoFilter options)
		{
			if (_authService.Organization.Id != OrganizationIdConst.SSP)
				options.RegionId = _authService.Organization.RegionId;

			var languageId = _cultureHelper.CurrentCulture.Id;
			var result = _unitOfWork.Context.GetMemshipDocsInfo(
			   pr_region_id: options.RegionId,
			   pr_memship_contract_type_id: options.MemshipContractTypeId,
			   pr_contractor_category_id: options.ContractorCategoryId,
			   pr_from_doc_date: options.FromDocDate,
			   pr_to_doc_date: options.ToDocDate,
			   pr_is_old: options.IsOld,
			   pr_is_pinfl: options.IsPinfl,
			   pr_language_id: languageId).ToList();
			return result;
		}
		public List<MemshipPaidReportDto> GetPaidMemshipReport(MemshipPaidReportDtoFilter options)
		{
			if (_authService.Organization.Id != OrganizationIdConst.SSP)
				options.RegionId = _authService.Organization.RegionId;

			options.IsOld = options.IsOld ?? false;

			var userOrg = _unitOfWork.Context.Set<Organization>()
				.FirstOrDefault(x => x.Id == _authService.User.OrganizationId);
			if ((userOrg.OrganizationGroupId == OrganizationGroupIdConst.SSP
				|| userOrg.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)
				&& userOrg.Id != OrganizationIdConst.SSP)
				options.RegionId = userOrg.RegionId;

			var lang = ServiceProvider.CultureHelper.CurrentCulture.Id;
			var result = new List<MemshipPaidReportDto>();

			var regions = _unitOfWork.Context.Set<Region>()
				.Include(x => x.Translates);

			if (options.RegionId.HasValue)
			{
				var districts = _unitOfWork.Context.Set<District>()
				.Include(x => x.Translates)
				.Where(x => x.RegionId == options.RegionId);
				var region = regions.FirstOrDefault(x => x.Id == options.RegionId);
				foreach (var district in districts)
				{
					result.Add(new()
					{
						Region = region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name,
							lang))?.TranslateText ?? region.FullName,
						RegionId = region.Id,
						RegionOrderCode = region.OrderCode,
						District = district.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name,
							lang))?.TranslateText ?? district.FullName,
						DistrictId = district.Id,
						DistrictOrderCode = district.OrderCode
					});
				}
			}
			else
			{
				foreach (var region in regions)
				{
					result.Add(new()
					{
						RegionId = region.Id,
						Region = region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name,
							lang))?.TranslateText ?? region.FullName,
						RegionOrderCode = region.OrderCode
					});
				}
			}

			var BXM = _unitOfWork.Context.Set<FixedMinimumValue>().FirstOrDefault(x => x.MinimumValueTypeId == MinimumValueTypeIdConst.BRV);

			foreach (var item in result)
			{
				options.RegionId = item.RegionId;
				options.DistrictId = item.DistrictId;
				item.SentMemshipApplicationCount = Applications(options).Count(x => x.Application.StatusId == StatusIdConst.SENT_FOR_REVIEW);
				item.AcceptedMemshipApplicationCount = Applications(options).Count(x => x.Application.StatusId == StatusIdConst.ACCEPTED);
				item.CreatedMemshipContractCount = Contracts(options).Count(x => x.StatusId == StatusIdConst.CREATED);
				item.SigningMemshipContractCount = Contracts(options).Count(x => x.StatusId == StatusIdConst.SIGNING);
				item.SignedMemshipContractCount = Contracts(options).Count(x => x.StatusId == StatusIdConst.SIGNED);
				item.MemshipCertificateCount = Certificates(options).Count(x => x.StatusId == StatusIdConst.FORMED);
				item.MemshipCertificateContributionBXM = Contracts(options).Sum(x => x.BaseFixedMinimumValue);
				item.MemshipCertificateContributionAmount = item.MemshipCertificateContributionBXM * BXM.FixedValue;
				item.MemshipCertificateRevenueAmount = Contracts(options).Sum(x => x.PaymentOrders.Sum(x => x.Amount));

				var sum = Contracts(options)
					.Where(x => x.StatusId == StatusIdConst.SIGNING || x.StatusId == StatusIdConst.SIGNED)
						.Sum(x => x.PaymentOrders.Sum(x => x.Amount));

				item.MemshipCertificateIndebtednessAmount = sum > 0 ? item.MemshipCertificateContributionAmount - sum : 0;
			}

			result = result.OrderBy(x => x.RegionOrderCode).ThenBy(x => x.DistrictOrderCode).ToList();
			return result;

			IQueryable<MemshipApplication> Applications(MemshipPaidReportDtoFilter options)
			{
				var query = _unitOfWork.Context.Set<MemshipApplication>()
					.Include(x => x.Application)
					.Where(a => a.Application.StatusId != StatusIdConst.DELETED);

				query = query.Where(a => !options.RegionId.HasValue
						|| options.RegionId == (a.ChooseLocation ? a.ChoosedRegionId : a.Application.RegionId));

				if (options.IsOld.HasValue)
					query = query.Where(a => options.IsOld.Value
					? a.Application.DocOn <= new DateOnly(2023, 10, 1)
					: a.Application.DocOn >= new DateOnly(2023, 10, 1));

				query = query.Where(a => !options.FromDocDate.HasValue
						|| options.FromDocDate >= a.Application.DocOn);

				query = query.Where(a => !options.ToDocDate.HasValue
						|| options.ToDocDate <= a.Application.DocOn);

				query = query.Where(a => !options.DistrictId.HasValue
					   || options.DistrictId == (a.ChooseLocation ? a.ChoosedDistrictId : a.Application.DistrictId));

				query = query.Where(a => a.ContractorCategoryId == ContractorCategoryIdConst.YIRIK_KORXONA);

				return query;
			}
			IQueryable<MemshipContract> Contracts(MemshipPaidReportDtoFilter options)
			{
				var query = _unitOfWork.Context.Set<MemshipContract>()
					.Include(c => c.PaymentOrders)
					.Where(a => a.StatusId != StatusIdConst.DELETED);

				if (options.IsOld.HasValue)
					query = query.Where(a => options.IsOld.Value
					? a.DocOn <= new DateOnly(2023, 10, 1)
					: a.DocOn >= new DateOnly(2023, 10, 1));
				query = query.Where(a => !options.FromDocDate.HasValue
						|| options.FromDocDate >= a.DocOn);

				query = query.Where(a => !options.ToDocDate.HasValue
						|| options.ToDocDate <= a.DocOn);

				query = query.Where(a =>
					(!options.RegionId.HasValue ||
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

				query = query.Where(a => a.MemshipContractTypeId == MemshipContractTypeIdConst.PAID);
				return query;
			}
			IQueryable<MemshipCertificate> Certificates(MemshipPaidReportDtoFilter options)
			{
				var query = _unitOfWork.Context.Set<MemshipCertificate>()
					.Include(c => c.Status)
					.Include(c => c.Contractor)
					.Include(c => c.MemshipContract)
					.Where(a => a.StatusId != StatusIdConst.DELETED);

				if (options.IsOld.HasValue)
					query = query.Where(a => options.IsOld.Value
					? a.DocOn <= new DateOnly(2023, 10, 1)
					: a.DocOn >= new DateOnly(2023, 10, 1));
				query = query.Where(a => !options.FromDocDate.HasValue
						|| options.FromDocDate >= a.DocOn);

				query = query.Where(a => !options.ToDocDate.HasValue
						|| options.ToDocDate <= a.DocOn);

				query = query.Where(a =>
					  (!options.RegionId.HasValue ||
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

				query = query.Where(a => a.MemshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.PAID);
				return query;
			}

		}
		public List<PrtnApplicationAndContractInfoDto> GetMemshipCeritiface(PrtnApplicationAndContractInfoDtoFilter filter)
		{
			var result = new List<PrtnApplicationAndContractInfoDto>();

			var query = _unitOfWork.Context.Set<Application>()
				.Include(a => a.PrtnApplication)
				.Include(a => a.PrtnContract).ThenInclude(a => a.PrtnCertificate)
				.Where(a => new int[]
					{
						StatusIdConst.SENT_FOR_REVIEW, StatusIdConst.SENT,
						StatusIdConst.ACCEPTED, StatusIdConst.EXECUTING,
						StatusIdConst.REJECTED, StatusIdConst.PASS_EXPERTISE,
						StatusIdConst.NOT_PASS_EXPERTISE, StatusIdConst.SIGNED,
						StatusIdConst.SIGNING, StatusIdConst.SENT_FOR_EXPERTISE
					}.Contains(a.StatusId)
					&& a.ApplicationTypeId == ApplicationTypeIdConst.PARTNER
				)
				.Where(a => !filter.PrtnContractTypeId.HasValue || filter.PrtnContractTypeId == a.PrtnApplication.PrtnContractTypeId);

			query = query.Where(a => (filter.StartDate.HasValue ? a.DocOn >= filter.StartDate.Value : true)
				  && (filter.EndDate.HasValue ? a.DocOn <= filter.EndDate.Value : true));

			query = query.Where(a => (!filter.RegionId.HasValue || filter.RegionId == (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId : a.RegionId))
				&& (!filter.DistrictId.HasValue || filter.DistrictId == (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId))
				&& (!filter.ContractorId.HasValue || filter.ContractorId == a.ContractorId));

			result = query
				.Select(a => new PrtnApplicationAndContractInfoDto
				{
					PrtnContractTypeId = filter.PrtnContractTypeId.HasValue ? a.PrtnApplication.PrtnContractTypeId : 0,
					PrtnContractType = filter.PrtnContractTypeId.HasValue ? (a.PrtnApplication.PrtnContractType.Translates.AsQueryable()
						.FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.PrtnContractType.FullName) : "",

					RegionId = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId : a.RegionId) : null,
					RegionOrderCode = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegion.OrderCode : a.Region.OrderCode) : null,
					Region = filter.ByRegion
						? (a.PrtnApplication.ChooseLocation
							? (a.PrtnApplication.ChoosedRegion.Translates.AsQueryable()
								.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.ChoosedRegion.FullName)
							: (a.Region.Translates.AsQueryable()
								.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Region.FullName))
						: null,

					DistrictId = filter.ByDistrict ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId) : null,
					District = filter.ByDistrict
						? (a.PrtnApplication.ChooseLocation
							? (a.PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
								.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.ChoosedDistrict.FullName)
							: (a.District.Translates.AsQueryable()
								.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.District.FullName))
						: null,

					ContractorId = filter.ByContractor ? a.ContractorId : null,
					Contractor = filter.ByContractor ? a.Contractor.FullName : null,
					ContractorInn = filter.ByContractor ? a.Contractor.Inn : null,
					ContractorPhoneNumber = filter.ByContractor ? a.Contractor.PhoneNumber : null,

					TotalPrtnApplicationSentCount = a.StatusId == StatusIdConst.SENT ? 1 : 0,
					TotalPrtnApplicationPassExpertisesCount = a.PrtnContract.StatusId == StatusIdConst.PASS_EXPERTISE ? 1 : 0,
					TotalPrtnApplicationSentForExpertisesCount = a.PrtnContract.StatusId == StatusIdConst.SENT_FOR_EXPERTISE ? 1 : 0,
					TotalPrtnApplicationNotPassExpertisesCount = a.PrtnContract.StatusId == StatusIdConst.NOT_PASS_EXPERTISE ? 1 : 0,
					TotalPrtnApplicationSignedCount = a.PrtnContract.StatusId == StatusIdConst.SIGNED ? 1 : 0,
					TotalPrtnApplicationSignningCount = a.PrtnContract.StatusId == StatusIdConst.SIGNING ? 1 : 0,
					TotalPrtnApplicationSentForReviewCount = a.StatusId == StatusIdConst.SENT_FOR_REVIEW ? 1 : 0,
					TotalPrtnApplicationSentAcceptedCount = a.StatusId == StatusIdConst.ACCEPTED ? 1 : 0,
					TotalPrtnApplicationSentRejectedCount = a.StatusId == StatusIdConst.REJECTED ? 1 : 0,
					TotalPrtnApplicationCanceledCount = a.StatusId == StatusIdConst.CANCELED ? 1 : 0,
					TotalPrtnContractCount = a.PrtnContract != null && a.PrtnContract.StatusId != StatusIdConst.REJECTED ? 1 : 0,
					TotalPrtnContractCancelCount = a.PrtnContract != null && a.PrtnContract.StatusId == StatusIdConst.CANCELED ? 1 : 0,
					TotalPrtnContractRejectedCount = a.PrtnContract != null && a.PrtnContract.StatusId == StatusIdConst.REJECTED ? 1 : 0,
					TotalPrtnCertificateCount = a.PrtnContract.PrtnCertificate != null && a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED ? 1 : 0,
					TotalNewVacanciesCount = a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED ? a.PrtnApplication.NewVacanciesCount : 0,
					TotalPrtnApplicationIsOffersCount = a.Contractor.IsLastOffer ? 1 : 0,
				})
				.AsEnumerable()
				.GroupBy(a => new
				{
					a.PrtnContractTypeId,
					a.PrtnContractType,
					a.DistrictId,
					a.District,
					a.RegionId,
					a.RegionOrderCode,
					a.Region,
					a.ContractorId,
					a.Contractor,
					a.ContractorInn,
					a.ContractorPhoneNumber,
				})
				.Select(a => new PrtnApplicationAndContractInfoDto
				{
					PrtnContractTypeId = a.Key.PrtnContractTypeId,
					PrtnContractType = a.Key.PrtnContractType,
					DistrictId = a.Key.DistrictId,
					District = a.Key.District,
					RegionId = a.Key.RegionId,
					RegionOrderCode = a.Key.RegionOrderCode,
					Region = a.Key.Region,
					ContractorId = a.Key.ContractorId,
					Contractor = a.Key.Contractor,
					ContractorInn = a.Key.ContractorInn,
					ContractorPhoneNumber = a.Key.ContractorPhoneNumber,
					TotalPrtnApplicationSentCount = a.Sum(b => b.TotalPrtnApplicationSentCount),
					TotalPrtnApplicationSentForReviewCount = a.Sum(b => b.TotalPrtnApplicationSentForReviewCount),
					TotalPrtnApplicationSentAcceptedCount = a.Sum(b => b.TotalPrtnApplicationSentAcceptedCount),
					TotalPrtnApplicationPassExpertisesCount = a.Sum(b => b.TotalPrtnApplicationPassExpertisesCount),
					TotalPrtnApplicationSentForExpertisesCount = a.Sum(b => b.TotalPrtnApplicationSentForExpertisesCount),
					TotalPrtnApplicationNotPassExpertisesCount = a.Sum(b => b.TotalPrtnApplicationNotPassExpertisesCount),
					TotalPrtnApplicationSignedCount = a.Sum(b => b.TotalPrtnApplicationSignedCount),
					TotalPrtnApplicationSignningCount = a.Sum(b => b.TotalPrtnApplicationSignningCount),
					TotalPrtnApplicationCanceledCount = a.Sum(b => b.TotalPrtnApplicationCanceledCount),
					TotalPrtnApplicationSentRejectedCount = a.Sum(b => b.TotalPrtnApplicationSentRejectedCount) + a.Sum(b => b.TotalPrtnApplicationCanceledCount),
					TotalPrtnContractCount = a.Sum(b => b.TotalPrtnContractCount),
					TotalPrtnContractCancelCount = a.Sum(b => b.TotalPrtnContractCancelCount),
					TotalPrtnContractRejectedCount = a.Sum(b => b.TotalPrtnContractRejectedCount),
					TotalPrtnCertificateCount = a.Sum(b => b.TotalPrtnCertificateCount),
					TotalNewVacanciesCount = a.Sum(b => b.TotalNewVacanciesCount),
					TotalPrtnApplicationCount = a.Sum(b => b.TotalPrtnApplicationSentCount) + a.Sum(b => b.TotalPrtnApplicationSentForReviewCount) + a.Sum(b => b.TotalPrtnApplicationSentAcceptedCount),
					TotalPrtnApplicationIsOffersCount = a.Sum(b => b.TotalPrtnApplicationIsOffersCount),
				})
				.ToList();

			// Hamma viloyatlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
			if (filter.ByRegion)
			{
				var regions = _unitOfWork.RegionRepository.AllAsQueryable
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.Id)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							OrderCode = a.OrderCode,
							FullName = a.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.FullName
						}
					);

				foreach (var region in regions)
				{
					if (result.Select(a => a.RegionId).Contains(region.Key))
						continue;

					result.Add(new PrtnApplicationAndContractInfoDto
					{
						Region = region.Value.FullName,
						RegionOrderCode = region.Value.OrderCode,
						RegionId = region.Key
					});
				}

				result = result.OrderBy(a => a.RegionOrderCode).ToList();
			}

			// Viloyatdagi hamma tumanlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
			if (filter.ByDistrict)
			{
				var districts = _unitOfWork.DistrictRepository.AllAsQueryable
					.Include(a => a.Region).ThenInclude(a => a.Translates)
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.RegionId
						&& !filter.DistrictId.HasValue || filter.DistrictId == a.Id
					)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							RegionId = a.RegionId,
							Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
								?.TranslateText
							?? a.Region.FullName,
							District = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
								?.TranslateText
							?? a.FullName,
							DistrictOrderCode = a.OrderCode,
						}
					);

				foreach (var district in districts)
				{
					if (result.Select(a => a.DistrictId).Contains(district.Key))
						continue;

					result.Add(new PrtnApplicationAndContractInfoDto
					{
						Region = district.Value.Region,
						RegionId = district.Value.RegionId,
						District = district.Value.District,
						RegionOrderCode = district.Value.DistrictOrderCode,
						DistrictId = district.Key
					});
				}

				result = result.OrderBy(a => a.RegionOrderCode).ToList();
			}

			return result;
		}
		public List<MemshipContractPaidDto> GetMemshipContract(MemshipContractPaidDtoFilter options)
		{
			var fixedMinimum = _unitOfWork.Context.Set<FixedMinimumValue>()
								.OrderByDescending(a => a.DateOn)
								.FirstOrDefault(x => x.MinimumValueTypeId == MinimumValueTypeIdConst.BRV)?.FixedValue ?? 0;

			IQueryable<MemshipContract> baseQuery = _unitOfWork.Context.Set<MemshipContract>()
				.Where(a => a.StatusId == StatusIdConst.SIGNED && a.MemshipContractTypeId == MemshipContractTypeIdConst.PAID
				&& a.Certificates.Any(c => c.StatusId != StatusIdConst.CANCELED));

			if (options.IsOld)
			{
				baseQuery = baseQuery.Where(a => a.DocOn <= new DateOnly(2023, 10, 1));
			}
			else
			{
				baseQuery = baseQuery.Where(a => a.DocOn > new DateOnly(2023, 10, 1));
			}

			if (options.ByOrganization)
			{
				if (options.RegionalOrganizationId.HasValue)
				{
					baseQuery = baseQuery.Where(a => a.RegionalOrganizationId == options.RegionalOrganizationId.Value);
				}

				var today = DateOnly.FromDateTime(DateTime.Today).AddDays(30);

				var organizationData = baseQuery
								  .Select(x => new
								  {
									  x.RegionalOrganizationId,
									  FullName = x.RegionalOrganization.FullName,
									  BaseFixedMinimumValue = x.BaseFixedMinimumValue,
									  DocOnContract = x.DocOn,
									  StatusId = x.StatusId,
									  RegionalOrganizationOrderCode = x.RegionalOrganization.OrderCode,
									  PaymentOrdersAccepted = x.PaymentOrders.Where(b => b.StatusId == StatusIdConst.ACCEPTED && b.ApplicationTypeId == ApplicationTypeIdConst.MEMSHIP),
									  PaymentOrdersNotAccepted = x.PaymentOrders.Where(b => b.StatusId != StatusIdConst.ACCEPTED && b.ApplicationTypeId == ApplicationTypeIdConst.MEMSHIP),
									  PaymentOrdersAcceptedCount = x.PaymentOrders
									 .Where(b => b.StatusId == StatusIdConst.ACCEPTED && b.ApplicationTypeId == ApplicationTypeIdConst.MEMSHIP)
									 .GroupBy(p => new { p.ContractorId })
									 .Count(),
									  AdditionalAgreementsCanPayDivided = x.AdditionalAgreements.Any(b => b.CanPayDivided),
									  IsCertificateExpired = x.Certificates.Any(c => c.ExpireOn <= today),
								  })
								  .ToList()
								  .GroupBy(x => new { x.RegionalOrganizationId, x.FullName, x.RegionalOrganizationOrderCode })
								  .Select(g => new
								  {
									  RegionalOrganizationId = g.Key.RegionalOrganizationId,
									  FullName = g.Key.FullName,
									  RegionalOrganizationOrderCode = g.Key.RegionalOrganizationOrderCode,
									  TotalMemshipContractCount = g.Count(),
									  TotalAmount = g.Sum(x => x.BaseFixedMinimumValue * fixedMinimum),
									  TotalPaidContractCount = g.Sum(x => x.PaymentOrdersAcceptedCount),
									  TotalPaidFromMemshimpContractAmount = g.Where(x => x.PaymentOrdersAccepted.Any())
									   .Sum(x => x.BaseFixedMinimumValue * fixedMinimum),
									  TotalPaymentAmount = g.Sum(x => x.PaymentOrdersAccepted.Sum(p => p.Amount)),
									  TotalNotPaidAmount = g.Sum(x => x.PaymentOrdersNotAccepted
																		   .Where(p => p.DocOn > x.DocOnContract.AddDays(14))
																		   .Sum(p => p.Amount)),
									  TotalTermPaymentAmount = g.Sum(x => x.AdditionalAgreementsCanPayDivided ? Math.Max(x.BaseFixedMinimumValue * fixedMinimum - x.PaymentOrdersAccepted.Sum(p => p.Amount), 0) : 0),
									  TotalUnpaidOnTimeAmount = g.Sum(x => x.PaymentOrdersNotAccepted
																		   .Where(p => p.DocOn == x.DocOnContract.AddDays(14))
																		   .Sum(p => p.Amount)),
									  TotalPaymentInDue = g.Sum(x =>
													x.IsCertificateExpired && x.AdditionalAgreementsCanPayDivided ?
													Math.Max(x.BaseFixedMinimumValue * fixedMinimum - x.PaymentOrdersAccepted.Sum(p => p.Amount), 0) : 0)
								  });

				var result = organizationData.Select(x => new MemshipContractPaidDto
				{
					RegionalOrganizationId = x.RegionalOrganizationId,
					RegionalOrganization = x.FullName,
					RegionalOrganizationOrderCode = x.RegionalOrganizationOrderCode,
					TotalMemshipContractCount = x.TotalMemshipContractCount,
					TotalAmount = x.TotalAmount,
					TotalPaidContractCount = x.TotalPaidContractCount,
					TotalPaidFromMemshimpContractAmount = x.TotalPaidFromMemshimpContractAmount,
					TotalPaymentAmount = x.TotalPaymentAmount,
					TotalTermPaymentAmount = x.TotalTermPaymentAmount,
					TotalNotPaidAmount = x.TotalNotPaidAmount,
					TotalUnpaidOnTimeAmount = x.TotalUnpaidOnTimeAmount,
					TotalPaymentInDue = x.TotalPaymentInDue,
				}).OrderBy(a => a.RegionalOrganizationOrderCode).ToList();

				return result;
			}
			else if (options.ByContractor)
			{

				baseQuery = baseQuery.Where(a => !options.RegionalOrganizationId.HasValue || options.RegionalOrganizationId == a.RegionalOrganizationId);

				if (options.ContractorId.HasValue)
				{
					baseQuery = baseQuery.Where(a => a.Contractor.Id == options.ContractorId.Value);
				}

				var today = DateOnly.FromDateTime(DateTime.Today);

				var contractorData = baseQuery
								  .Select(x => new
								  {
									  x.RegionalOrganizationId,
									  x.ContractorId,
									  ContractorInn = x.Contractor.Inn,
									  FullName = x.Contractor.FullName,
									  BaseFixedMinimumValue = x.BaseFixedMinimumValue,
									  DocOnContract = x.DocOn,
									  StatusId = x.StatusId,
									  RegionalOrganizationOrderCode = x.RegionalOrganization.OrderCode,
									  PaymentOrdersAccepted = x.PaymentOrders.Where(b => b.StatusId == StatusIdConst.ACCEPTED && b.ApplicationTypeId == ApplicationTypeIdConst.MEMSHIP),
									  PaymentOrdersNotAccepted = x.PaymentOrders.Where(b => b.StatusId != StatusIdConst.ACCEPTED && b.ApplicationTypeId == ApplicationTypeIdConst.MEMSHIP),
									  PaymentOrdersAcceptedCount = x.PaymentOrders
									 .Where(b => b.StatusId == StatusIdConst.ACCEPTED && b.ApplicationTypeId == ApplicationTypeIdConst.MEMSHIP)
									 .GroupBy(p => new { p.ContractorId })
									 .Count(),
									  AdditionalAgreementsCanPayDivided = x.AdditionalAgreements.Any(b => b.CanPayDivided),
									  IsCertificateExpired = x.Certificates.Any(c => c.ExpireOn <= today),
								  })
								  .ToList()
								  .GroupBy(x => new { x.RegionalOrganizationId, x.ContractorId, x.FullName, x.ContractorInn })
								  .Select(g => new
								  {
									  RegionalOrganizationId = g.Key.RegionalOrganizationId,
									  FullName = g.Key.FullName,
									  ContractorId = g.Key.ContractorId,
									  ContractorInn = g.Key.ContractorInn,
									  TotalMemshipContractCount = g.Count(),
									  TotalAmount = g.Sum(x => x.BaseFixedMinimumValue * fixedMinimum),
									  TotalPaidContractCount = g.Sum(x => x.PaymentOrdersAcceptedCount),
									  TotalPaidFromMemshimpContractAmount = g.Where(x => x.PaymentOrdersAccepted.Any())
									   .Sum(x => x.BaseFixedMinimumValue * fixedMinimum),
									  TotalPaymentAmount = g.Sum(x => x.PaymentOrdersAccepted.Sum(p => p.Amount)),
									  TotalNotPaidAmount = g.Sum(x => x.PaymentOrdersNotAccepted
																		   .Where(p => p.DocOn > x.DocOnContract.AddDays(14))
																		   .Sum(p => p.Amount)),
									  TotalUnpaidOnTimeAmount = g.Sum(x => x.PaymentOrdersNotAccepted
																		   .Where(p => p.DocOn == x.DocOnContract.AddDays(14))
																		   .Sum(p => p.Amount)),
									  TotalTermPaymentAmount = g.Sum(x => x.AdditionalAgreementsCanPayDivided ? Math.Max(x.BaseFixedMinimumValue * fixedMinimum - x.PaymentOrdersAccepted.Sum(p => p.Amount), 0) : 0),
									  TotalPaymentInDue = g.Sum(x =>
													x.IsCertificateExpired && x.AdditionalAgreementsCanPayDivided ?
													Math.Max(x.BaseFixedMinimumValue * fixedMinimum - x.PaymentOrdersAccepted.Sum(p => p.Amount), 0) : 0)
								  });

				var result = contractorData.Select(x => new MemshipContractPaidDto
				{
					RegionalOrganizationId = x.RegionalOrganizationId,
					ContractorId = x.ContractorId,
					Contractor = x.FullName,
					ContractorInn = x.ContractorInn,
					TotalMemshipContractCount = x.TotalMemshipContractCount,
					TotalAmount = x.TotalAmount,
					TotalPaidContractCount = x.TotalPaidContractCount,
					TotalPaidFromMemshimpContractAmount = x.TotalPaidFromMemshimpContractAmount,
					TotalPaymentAmount = x.TotalPaymentAmount,
					TotalTermPaymentAmount = x.TotalTermPaymentAmount,
					TotalNotPaidAmount = x.TotalNotPaidAmount,
					TotalUnpaidOnTimeAmount = x.TotalUnpaidOnTimeAmount,
					TotalPaymentInDue = x.TotalPaymentInDue,
				}).ToList();

				return result;
			}


			return new List<MemshipContractPaidDto>();
		}
		public List<ContractorCategoryDto> GetContractorCategoryType(ContractorCategoryDtoFilter options)
		{
			return GetContractorCategoryTypeMethod(options);
		}
		private decimal RoundToTwoDecimalPlaces(decimal value)
		{
			return Math.Round(value, 0, MidpointRounding.AwayFromZero);
		}
		private List<ContractorCategoryDto> GetContractorCategoryTypeMethod(ContractorCategoryDtoFilter options)
		{
			var contractorRating = _unitOfWork.Context.Set<ContractorRating>().Where(a => a.StateId != StateIdConst.PASSIVE);

			IQueryable<MemshipNewContractorsTable> memshipNewContractors = _unitOfWork.Context.Set<MemshipNewContractorsTable>().Include(a => a.Owner).ThenInclude(r => r.Region).ThenInclude(tr => tr.Translates).Include(d => d.District).ThenInclude(dt => dt.Translates)
				.Where(a => a.Owner.StatusId == StatusIdConst.ACCEPTED);

			memshipNewContractors = memshipNewContractors.Where(a => (options.StartDate.HasValue ? a.Owner.FromDate >= options.StartDate.Value : true)
			 && (options.EndDate.HasValue ? a.Owner.ToDate <= options.EndDate.Value : true));

			IQueryable<MemshipCertificate> memshipCertificates =
											   from cert in _unitOfWork.Context.Set<MemshipCertificate>()
											   join contractor in _unitOfWork.Context.Set<MemshipNewContractor>()
												   on cert.RegionId equals contractor.RegionId
											   where
													 cert.StatusId == StatusIdConst.FORMED &&
													 contractor.StatusId == StatusIdConst.ACCEPTED &&
													 cert.MemshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.FREE &&
													 contractor.FromDate <= cert.DocOn &&
													 contractor.ToDate >= cert.DocOn
											   select new MemshipCertificate
											   {
												   Contractor = cert.Contractor,
												   RegionId = cert.RegionId,
												   Region = cert.Region,
												   District = cert.District,
												   DistrictId = cert.DistrictId,
												   DocOn = cert.DocOn,
												   MemshipContract = cert.MemshipContract
											   };

			memshipCertificates = memshipCertificates.Where(a => (options.StartDate.HasValue ? a.DocOn >= options.StartDate.Value : true)
				  && (options.EndDate.HasValue ? a.DocOn <= options.EndDate.Value : true));

			if (options.ByRegion)
			{
				var result = memshipNewContractors
				   .GroupBy(c => new { c.Owner.RegionId, c.Owner.Region.FullName, c.Owner.Region.OrderCode })
				   .Select(g => new ContractorCategoryDto
				   {
					   RegionId = g.Key.RegionId,
					   Region = g.Key.FullName,
					   RegionOrderCode = g.Key.OrderCode,
					   TotalNewCreatedContractorLegalCount = g.Sum(l => l.LegalCount),
					   TotalNewCreatedContractorPhysicalCount = g.Sum(l => l.PhysicalCount)
				   }).OrderBy(a => a.RegionOrderCode)
				   .ToList();

				var memshipCertificatesList = memshipCertificates.ToList();

				var memshipCertificateCounts = memshipCertificatesList
					.Where(c => result.Any(r => r.RegionId == c.RegionId))
					.GroupBy(c => new { c.RegionId, c.Region.OrderCode })
					.Select(g => new
					{
						RegionId = g.Key.RegionId,
						RegionOrderCode = g.Key.OrderCode,
						TotalFreeAddedMemshipLegalCount = g.Count(l => l.Contractor.Pinfl == null),
						TotalFreeAddedMemshipPhysicalCount = g.Count(l => l.Contractor.Pinfl != null)
					}).OrderBy(a => a.RegionOrderCode)
					.ToList();


				foreach (var item in memshipCertificateCounts)
				{
					var dto = result.FirstOrDefault(r => r.RegionId == item.RegionId);
					if (dto != null)
					{
						dto.TotalFreeAddedMemshipLegalCount = item.TotalFreeAddedMemshipLegalCount;

						dto.TotalFreeAddedMemshipPhysicalCount = item.TotalFreeAddedMemshipPhysicalCount;

						dto.Total = item.TotalFreeAddedMemshipLegalCount + item.TotalFreeAddedMemshipPhysicalCount;

						var sumLegalCount = memshipNewContractors.Where(a => a.Owner.RegionId == item.RegionId)?.Sum(l => l.LegalCount);
						var sumPhysicalCount = memshipNewContractors.Where(a => a.Owner.RegionId == item.RegionId)?.Sum(l => l.PhysicalCount);

						dto.TotalEvaluationRatingLegalCount = sumLegalCount != 0
							? RoundToTwoDecimalPlaces((item.TotalFreeAddedMemshipLegalCount * 100m) / sumLegalCount.Value)
							: 0;

						dto.TotalEvaluationRatingPhysicalCount = sumPhysicalCount != 0
							? RoundToTwoDecimalPlaces((item.TotalFreeAddedMemshipPhysicalCount * 100m) / sumPhysicalCount.Value)
							: 0;

						dto.TotalRatingFromNormaLegalCount = contractorRating
													.Where(a => a.MinimumPercentage <= dto.TotalEvaluationRatingLegalCount && a.MaximumPercentage >= dto.TotalEvaluationRatingLegalCount && a.ContractorTypeId == 1)
													.Select(a => a.Score)
													.FirstOrDefault();

						dto.TotalRatingFromNormaPhysicalCount = contractorRating
												.Where(a => a.MinimumPercentage <= dto.TotalEvaluationRatingPhysicalCount && a.MaximumPercentage >= dto.TotalEvaluationRatingPhysicalCount && a.ContractorTypeId == 2)
												.Select(a => a.Score)
												.FirstOrDefault();

						dto.AverageRating = (dto.TotalRatingFromNormaLegalCount + dto.TotalRatingFromNormaPhysicalCount) / 2;

						dto.Evaluation = null;
					}
				}
				return result;
			}
			else if (options.ByDistrict)
			{
				if (options.RegionId.HasValue)
				{
					memshipNewContractors = memshipNewContractors.Where(a => a.Owner.RegionId == options.RegionId.Value);
					memshipCertificates = memshipCertificates.Where(a => a.RegionId == options.RegionId.Value);
				}

				var result = memshipNewContractors
				   .GroupBy(c => new { c.Owner.RegionId, RegionName = c.Owner.Region.FullName, c.DistrictId, DistrictName = c.District.FullName, c.District.OrderCode })
				   .Select(g => new ContractorCategoryDto
				   {
					   RegionId = g.Key.RegionId,
					   Region = g.Key.RegionName,
					   DistrictId = g.Key.DistrictId,
					   DistrictOrderCode = g.Key.OrderCode,
					   District = g.Key.DistrictName,
					   TotalNewCreatedContractorLegalCount = g.Sum(l => l.LegalCount),
					   TotalNewCreatedContractorPhysicalCount = g.Sum(l => l.PhysicalCount)
				   }).OrderBy(a => a.DistrictOrderCode)
				   .ToList();

				var memshipCertificatesList = memshipCertificates.ToList();

				var memshipCertificateCounts = memshipCertificatesList
					//.Where(c => result.Any(r => r.DistrictId == c.DistrictId))
					.GroupBy(c => new { c.RegionId, RegionName = c.Region.FullName, c.DistrictId, DistrictName = c.District.FullName, c.District.OrderCode })
					.Select(g => new
					{
						RegionId = g.Key.RegionId,
						Region = g.Key.RegionName,
						DistrictId = g.Key.DistrictId,
						DistrictOrderCode = g.Key.OrderCode,
						District = g.Key.DistrictName,
						TotalFreeAddedMemshipLegalCount = g.Count(l => l.Contractor.Pinfl == null),
						TotalFreeAddedMemshipPhysicalCount = g.Count(l => l.Contractor.Pinfl != null)
					}).OrderBy(a => a.DistrictOrderCode)
					.ToList();


				foreach (var item in memshipCertificateCounts)
				{
					var dto = result.FirstOrDefault(r => r.RegionId == item.RegionId && r.DistrictId == item.DistrictId);
					if (dto != null)
					{
						dto.TotalFreeAddedMemshipLegalCount = item.TotalFreeAddedMemshipLegalCount;
						dto.TotalFreeAddedMemshipPhysicalCount = item.TotalFreeAddedMemshipPhysicalCount;
						dto.Total = item.TotalFreeAddedMemshipLegalCount + item.TotalFreeAddedMemshipPhysicalCount;

						var sumLegalCount = memshipNewContractors.Where(a => a.DistrictId == item.DistrictId)?.Sum(l => l.LegalCount);
						var sumPhysicalCount = memshipNewContractors.Where(a => a.DistrictId == item.DistrictId)?.Sum(l => l.PhysicalCount);

						dto.TotalEvaluationRatingLegalCount = sumLegalCount != 0
						   ? RoundToTwoDecimalPlaces((item.TotalFreeAddedMemshipLegalCount * 100m) / sumLegalCount.Value)
						   : 0;

						dto.TotalEvaluationRatingPhysicalCount = sumPhysicalCount != 0
							? RoundToTwoDecimalPlaces((item.TotalFreeAddedMemshipPhysicalCount * 100m) / sumPhysicalCount.Value)
							: 0;

						dto.TotalRatingFromNormaLegalCount = contractorRating
													.Where(a => a.MinimumPercentage <= dto.TotalEvaluationRatingLegalCount && a.MaximumPercentage >= dto.TotalEvaluationRatingLegalCount && a.ContractorTypeId == 1)
													.Select(a => a.Score)
													.FirstOrDefault();

						dto.TotalRatingFromNormaPhysicalCount = contractorRating
												.Where(a => a.MinimumPercentage <= dto.TotalEvaluationRatingPhysicalCount && a.MaximumPercentage >= dto.TotalEvaluationRatingPhysicalCount && a.ContractorTypeId == 2)
												.Select(a => a.Score)
												.FirstOrDefault();

						dto.AverageRating = (dto.TotalRatingFromNormaLegalCount + dto.TotalRatingFromNormaPhysicalCount) / 2;

						dto.Evaluation = null;
					}
				}
				return result;

			}

			return new List<ContractorCategoryDto>();
		}

		#endregion

		#region Corruption
		//public List<CorruptionApplicationDto> GetCorruptionByRegion(CorruptionApplicationFilterDto options)
		//{
		//    var result = new List<CorruptionApplicationDto>();

		//    if (options.ByRegion)
		//    {
		//        var regions = _unitOfWork.Context.Set<Region>().Include(a => a.Translates).Where(a => a.StateId != StateIdConst.PASSIVE).OrderBy(a => a.OrderCode).ToList();

		//        foreach (var region in regions)
		//        {
		//            var item = new CorruptionApplicationDto();

		//            item.RegionId = region.Id;
		//            item.Region = region.Translates.AsQueryable()
		//                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
		//                    .TranslateText ?? region.FullName;

		//            item.TotalApplicationCount = Contracts(region.Id).Count();
		//            item.TotalApplicationSendCount = Contracts(region.Id).Count(x => x.Application.StatusId == StatusIdConst.SENT);
		//            item.TotalApplicationSendToOmbusmanCount = Contracts(region.Id).Count(x => x.Application.CurrentStepId == StepIdConst.SENT_TO_OMBUDSMAN);
		//            item.TotalApplicationSendToAniCorruptionCount = Contracts(region.Id).Count(x => x.Application.CurrentStepId == StepIdConst.SENT_TO_ANTI_CORRUPTION_AGENCY);
		//            item.TotalApplicationAccepCount = Contracts(region.Id).Count(x => x.Application.StatusId == StatusIdConst.ACCEPTED);
		//            item.TotalApplicationCanceldCount = Contracts(region.Id).Count(x => x.Application.StatusId == StatusIdConst.CANCELED);
		//            item.TotalApplicationSendToRewiedCount = CorruptionResult2(region.Id).Count();
		//            item.TotalCertificateCount = CorruptionResult1(region.Id).Count();
		//            item.TotalCanceledFromResultCount = CorruptionResult3(region.Id).Count();

		//            result.Add(item);
		//        }
		//    }
		//    else if (options.ByDistrict)
		//    {
		//        var districts = _unitOfWork.Context.Set<District>().Include(a => a.Translates).Where(a => a.StateId != StateIdConst.PASSIVE).OrderBy(a => a.OrderCode).ToList();

		//        foreach (var district in districts)
		//        {
		//            var item = new CorruptionApplicationDto();

		//            //item.RegionId = district.RegionId;
		//            //item.Region = district.Region.Translates.AsQueryable()
		//            //        .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
		//            //        .TranslateText ?? district.Region.FullName;

		//            item.DistrictId = district.Id;
		//            item.District = district.Translates.AsQueryable()
		//                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
		//                    .TranslateText ?? district.FullName;

		//            item.TotalApplicationCount = Contracts(null, district.Id).Count();
		//            item.TotalApplicationSendCount = Contracts(null, district.Id).Count(x => x.Application.StatusId == StatusIdConst.SENT);
		//            item.TotalApplicationSendToOmbusmanCount = Contracts(null, district.Id).Count(x => x.Application.CurrentStepId == StepIdConst.SENT_TO_OMBUDSMAN);
		//            item.TotalApplicationSendToAniCorruptionCount = Contracts(null, district.Id).Count(x => x.Application.CurrentStepId == StepIdConst.SENT_TO_ANTI_CORRUPTION_AGENCY);
		//            item.TotalApplicationAccepCount = Contracts(null, district.Id).Count(x => x.Application.StatusId == StatusIdConst.ACCEPTED);
		//            item.TotalApplicationCanceldCount = Contracts(null, district.Id).Count(x => x.Application.StatusId == StatusIdConst.CANCELED);
		//            item.TotalApplicationSendToRewiedCount = CorruptionResult2(null, district.Id).Count();
		//            item.TotalCertificateCount = CorruptionResult1(null, district.Id).Count();
		//            item.TotalCanceledFromResultCount = CorruptionResult3(null, district.Id).Count();

		//            result.Add(item);
		//        }

		//    }

		//    result = result.OrderBy(x => x.RegionOrderCode).ToList();

		//    return result;

		//    #region Action
		//    IQueryable<JoinAntiCorruptionApplication> Contracts(int? regId = null, int? disId = null)
		//    {
		//        var query = _unitOfWork.Context.Set<JoinAntiCorruptionApplication>().Include(a => a.Application).ThenInclude(b => b.Region).ThenInclude(s => s.Translates).Include(a => a.Application).ThenInclude(b => b.District).ThenInclude(s => s.Translates).Where(a => a.Application.StatusId != StatusIdConst.DELETED && a.Application.ApplicationTypeId == ApplicationTypeIdConst.CORRUPTION && (regId.HasValue && a.Application.RegionId == regId.Value || disId.HasValue && a.Application.DistrictId == disId.Value));

		//        query = query.Where(a => !options.StartDate.HasValue
		//                || options.StartDate >= a.Application.DocOn);

		//        query = query.Where(a => !options.EndDate.HasValue
		//                || options.EndDate <= a.Application.DocOn);

		//        query = query.Where(a => a.Application.StatusId != StatusIdConst.DELETED);
		//        return query;
		//    }
		//    IQueryable<JoinAntiCorruptionResultTable> CorruptionResult1(int? regId = null, int? disId = null)
		//    {
		//        var query = _unitOfWork.Context.Set<JoinAntiCorruptionResultTable>().Include(a => a.Application).ThenInclude(a => a.Region).Include(a => a.Application).ThenInclude(a => a.District).Where(m => m.Application.StatusId == StatusIdConst.ACCEPTED && m.ApplicationId != null && m.Owner.StatusId == StatusIdConst.ACCEPTED && (regId.HasValue && m.Application.RegionId == regId.Value || disId.HasValue && m.Application.DistrictId == disId.Value));

		//        query = query.Where(a => !options.StartDate.HasValue
		//                || options.StartDate >= a.Application.DocOn);

		//        query = query.Where(a => !options.EndDate.HasValue
		//                || options.EndDate <= a.Application.DocOn);

		//        return query;
		//    }
		//    IQueryable<JoinAntiCorruptionResultTable> CorruptionResult2(int? regId = null, int? disId = null)
		//    {
		//        var query = _unitOfWork.Context.Set<JoinAntiCorruptionResultTable>()
		//                        .Include(a => a.Application)
		//                            .ThenInclude(a => a.Region)
		//                        .Include(a => a.Application)
		//                            .ThenInclude(a => a.District)
		//                        .Where(m => m.ApplicationId == null
		//                                    && m.Owner.StatusId == StatusIdConst.ACCEPTED
		//                                    && (regId == null || m.Application.RegionId == regId.Value
		//                                        || (disId.HasValue && m.Application.DistrictId == disId.Value)));

		//        query = query.Where(a => !options.StartDate.HasValue || options.StartDate >= a.Application.DocOn);
		//        query = query.Where(a => !options.EndDate.HasValue || options.EndDate <= a.Application.DocOn);

		//        return query;
		//    }
		//    IQueryable<JoinAntiCorruptionResultTable> CorruptionResult3(int? regId = null, int? disId = null)
		//    {
		//        var query = _unitOfWork.Context.Set<JoinAntiCorruptionResultTable>().Include(a => a.Application).ThenInclude(a => a.Region).Include(a => a.Application).ThenInclude(a => a.District).Where(m => m.Application.StatusId == StatusIdConst.ACCEPTED && m.ApplicationId != null && m.Owner.StatusId == StatusIdConst.ACCEPTED && m.JoinAntiCorruptionResultTypeId == 2 && (regId.HasValue && m.Application.RegionId == regId.Value || disId.HasValue && m.Application.DistrictId == disId.Value));

		//        query = query.Where(a => !options.StartDate.HasValue
		//                || options.StartDate >= a.Application.DocOn);

		//        query = query.Where(a => !options.EndDate.HasValue
		//                || options.EndDate <= a.Application.DocOn);

		//        return query;
		//    }
		//    #endregion
		//}
		public List<CorruptionApplicationDto> GetAntiCorruptionByRegion(CorruptionApplicationFilterDto options)
		{
			var result = new List<CorruptionApplicationDto>();

			if (options.ByRegion)
			{
                //var regionQuery = _unitOfWork.Context.Set<JoinAntiCorruptionApplication>()
                //						.Include(a => a.Application)
                //							.ThenInclude(b => b.Region)
                //								.ThenInclude(s => s.Translates)
                //						.Include(a => a.Application)
                //							.ThenInclude(b => b.District)
                //								.ThenInclude(s => s.Translates)
                //						.Where(a => a.Application.StatusId != StatusIdConst.DELETED
                //									&& a.Application.ApplicationTypeId == ApplicationTypeIdConst.CORRUPTION);

                var regions = _unitOfWork.Context.Set<Region>()
												 .Where(r => r.StateId == 1)
												 .Include(r => r.Translates)
												 .OrderBy(r => r.OrderCode)
												 .ToList();

                if (options.RegionId.HasValue)
				{
                    regions = regions.Where(a => a.Id == options.RegionId.Value).ToList();
				}

				//var regions = regionQuery.Select(a => a.Id).Distinct().ToList();

                foreach (var region in regions)
				{
					var item = new CorruptionApplicationDto();

					item.RegionId = region.Id;
					item.Region = region.Translates.AsQueryable()
										.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
										?.TranslateText ?? region.FullName;
					
					// Bu narsa kerak emas ekan
					//var contracts = Contracts(region.Id, null, null);
					//FillDtoWithContractCounts(item, contracts);

					item.TotalCertificateCount = CorruptionResult1(region.Id, null, null).Count();
					item.TotalApplicationSendToRewiedCount = CorruptionResult2(region.Id, null, null).Count();
					item.TotalCanceledFromResultCount = CorruptionResult3(region.Id, null, null).Count();

					result.Add(item);
				}
			}
			else if (options.ByDistrict)
			{
				//var districtQuery = _unitOfWork.Context.Set<JoinAntiCorruptionApplication>()
				//						.Include(a => a.Application)
				//							.ThenInclude(b => b.Region)
				//								.ThenInclude(s => s.Translates)
				//						.Include(a => a.Application)
				//							.ThenInclude(b => b.District)
				//								.ThenInclude(s => s.Translates)
				//						.Where(a => a.Application.StatusId != StatusIdConst.DELETED
				//									&& a.Application.ApplicationTypeId == ApplicationTypeIdConst.CORRUPTION);

				var districtQuery = _unitOfWork.Context.Set<District>()
													   .Include(x => x.Translates)
													   .Where(x => x.StateId == 1 && x.RegionId == options.RegionId.Value)
													   .OrderBy(x => x.OrderCode)
													   .ToList();

                if (options.DistrictId.HasValue)
					districtQuery = districtQuery.Where(a => a.Id == options.DistrictId.Value).ToList();

				//var districts = districtQuery.Select(a => a.Application.District).Where(b => b.RegionId == options.RegionId).Distinct().OrderBy(a => a.OrderCode).ToList();

				foreach (var district in districtQuery)
				{
					var item = new CorruptionApplicationDto();

					item.DistrictId = district.Id;
					item.District = district.Translates.AsQueryable()
										.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
										?.TranslateText ?? district.FullName;

					//var contracts = Contracts(null, district.Id, null);
					//FillDtoWithContractCounts(item, contracts);

					item.TotalCertificateCount = CorruptionResult1(null, district.Id, null).Count();
					item.TotalApplicationSendToRewiedCount = CorruptionResult2(null, district.Id, null).Count();
					item.TotalCanceledFromResultCount = CorruptionResult3(null, district.Id, null).Count();

					result.Add(item);
				}

				result = result.OrderBy(x => x.RegionOrderCode).ToList();
			}
			else if (options.ByContractor)
			{
				//var contractorQuery = _unitOfWork.Context.Set<JoinAntiCorruptionApplication>()
				//						.Include(a => a.Application)
				//							.ThenInclude(b => b.Region)
				//								.ThenInclude(s => s.Translates)
				//						.Include(a => a.Application)
				//							.ThenInclude(b => b.District)
				//								.ThenInclude(s => s.Translates)
				//								.Include(c => c.Application)
				//								.ThenInclude(c => c.Contractor)
				//						.Where(a => a.Application.StatusId != StatusIdConst.DELETED
				//									&& a.Application.ApplicationTypeId == ApplicationTypeIdConst.CORRUPTION);

				var contractorQuery = _unitOfWork.Context.Set<JoinAntiCorruptionCertificate>()
														 .Include(x => x.Contractor)
														 .ToList();


                if (options.ContractorId.HasValue)
					contractorQuery = contractorQuery.Where(a => a.ContractorId == options.ContractorId.Value).ToList();

				var contractors = contractorQuery.Select(a => a.Contractor).Where(b => b.RegionId == options.RegionId && b.DistrictId == options.DistrictId).Distinct().OrderBy(a => a.Id).ToList();

				foreach (var contractor in contractors)
				{
					var item = new CorruptionApplicationDto();

					item.ContractorId = contractor.Id;
					item.Contractor = contractor.FullName;
					item.ContractorInn = contractor.Inn;

					// Bu narsa kerak emas ekan
					//var contractss = Contracts(null, null, contractor.Id);
					//FillDtoWithContractCounts(item, contractss);

					item.TotalCertificateCount = CorruptionResult1(null, null, contractor.Id).Count();
					item.TotalApplicationSendToRewiedCount = CorruptionResult2(null, null, contractor.Id).Count();
					item.TotalCanceledFromResultCount = CorruptionResult3(null, null, contractor.Id).Count();

					result.Add(item);
				}


				result = result.OrderBy(x => x.ContractorId).ToList();
			}

			return result;

			#region Action
			IQueryable<JoinAntiCorruptionApplication> Contracts(int? regId = null, int? disId = null, long? contId = null)
			{
				var query = _unitOfWork.Context.Set<JoinAntiCorruptionApplication>().Include(a => a.Application).ThenInclude(b => b.Region).ThenInclude(s => s.Translates).Include(a => a.Application).ThenInclude(b => b.District).ThenInclude(s => s.Translates).Include(c => c.Application).ThenInclude(c => c.Contractor).Where(a => a.Application.StatusId != StatusIdConst.DELETED && a.Application.ApplicationTypeId == ApplicationTypeIdConst.CORRUPTION && (regId.HasValue && a.Application.RegionId == regId.Value || disId.HasValue && a.Application.DistrictId == disId.Value || contId.HasValue && a.Application.ContractorId == contId.Value));

				query = query.Where(a => !options.StartDate.HasValue
						|| options.StartDate >= a.Application.DocOn);

				query = query.Where(a => !options.EndDate.HasValue
						|| options.EndDate <= a.Application.DocOn);

				query = query.Where(a => a.Application.StatusId != StatusIdConst.DELETED);
				return query;
			}
			IQueryable<JoinAntiCorruptionCertificate> CorruptionResult1(int? regId = null, int? disId = null, long? contId = null)
			{
				var query = _unitOfWork.Context.Set<JoinAntiCorruptionCertificate>()
					.Include(a => a.JoinAntiCorruptionResultTable)
						.ThenInclude(a => a.Application)
							.ThenInclude(a => a.Region)
					.Include(a => a.JoinAntiCorruptionResultTable)
						.ThenInclude(a => a.Application)
							.ThenInclude(a => a.District)
					.Where(a => a.StatusId != StatusIdConst.DELETED);

                var resultQuery = query.Where(m =>
					(m.JoinAntiCorruptionResultTable != null &&
					m.JoinAntiCorruptionResultTable.Application.StatusId == StatusIdConst.ACCEPTED &&
					m.JoinAntiCorruptionResultTable.ApplicationId != null &&
					m.JoinAntiCorruptionResultTable.Owner.StatusId == StatusIdConst.ACCEPTED &&
					(regId.HasValue && m.JoinAntiCorruptionResultTable.Application.RegionId == regId.Value ||
					disId.HasValue && m.JoinAntiCorruptionResultTable.Application.DistrictId == disId.Value ||
					contId.HasValue && m.JoinAntiCorruptionResultTable.Application.ContractorId == contId.Value)));

                var certificateQuery = query.Where(m =>
					m.JoinAntiCorruptionResultTable == null &&
					(regId == null || m.Contractor.RegionId == regId.Value) &&
					(disId == null || m.Contractor.DistrictId == disId.Value) &&
					(contId == null || m.ContractorId == contId.Value));

                var finalQuery = resultQuery.Union(certificateQuery);

                if (options.StartDate.HasValue)
                {
                    finalQuery = finalQuery.Where(a =>
                        a.JoinAntiCorruptionResultTable != null &&
                        options.StartDate <= a.JoinAntiCorruptionResultTable.Application.DocOn
                        || a.JoinAntiCorruptionResultTable == null &&
                        options.StartDate <= a.DocOn);
                }

                if (options.EndDate.HasValue)
                {
                    finalQuery = finalQuery.Where(a =>
                        a.JoinAntiCorruptionResultTable != null &&
                        options.EndDate >= a.JoinAntiCorruptionResultTable.Application.DocOn
                        || a.JoinAntiCorruptionResultTable == null &&
                        options.EndDate >= a.DocOn);
                }

                return finalQuery;
			}
			IQueryable<JoinAntiCorruptionResultTable> CorruptionResult2(int? regId = null, int? disId = null, long? contId = null)
			{
				var query = _unitOfWork.Context.Set<JoinAntiCorruptionResultTable>()
								.Include(a => a.Application)
									.ThenInclude(a => a.Region)
								.Include(a => a.Application)
									.ThenInclude(a => a.District)
								.Where(m => m.ApplicationId == null
											&& m.Owner.StatusId == StatusIdConst.ACCEPTED
											&& (regId == null || m.Application.RegionId == regId.Value
												|| disId.HasValue && m.Application.DistrictId == disId.Value || contId.HasValue && m.Application.ContractorId == contId.Value));

				query = query.Where(a => !options.StartDate.HasValue || options.StartDate >= a.Application.DocOn);
				query = query.Where(a => !options.EndDate.HasValue || options.EndDate <= a.Application.DocOn);

				return query;
			}
			IQueryable<JoinAntiCorruptionResultTable> CorruptionResult3(int? regId = null, int? disId = null, long? contId = null)
			{
				var query = _unitOfWork.Context.Set<JoinAntiCorruptionResultTable>().Include(a => a.Application).ThenInclude(a => a.Region).Include(a => a.Application).ThenInclude(a => a.District).Where(m => m.Application.StatusId == StatusIdConst.ACCEPTED && m.ApplicationId != null && m.Owner.StatusId == StatusIdConst.ACCEPTED && m.JoinAntiCorruptionResultTypeId == 2 && (regId.HasValue && m.Application.RegionId == regId.Value || disId.HasValue && m.Application.DistrictId == disId.Value || contId.HasValue && m.Application.ContractorId == contId.Value));

				query = query.Where(a => !options.StartDate.HasValue
						|| options.StartDate >= a.Application.DocOn);

				query = query.Where(a => !options.EndDate.HasValue
						|| options.EndDate <= a.Application.DocOn);

				return query;
			}
			#endregion

		}
		//private void FillDtoWithContractCounts(CorruptionApplicationDto item, IQueryable<JoinAntiCorruptionApplication> contracts)
		//{
		//	item.TotalApplicationCount = contracts.Count();
		//	item.TotalApplicationSendCount = contracts.Count(x => x.Application.StatusId == StatusIdConst.SENT);
		//	item.TotalApplicationSendToOmbusmanCount = contracts.Count(x => x.Application.CurrentStepId == StepIdConst.SENT_TO_OMBUDSMAN);
		//	item.TotalApplicationSendToAniCorruptionCount = contracts.Count(x => x.Application.CurrentStepId == StepIdConst.SENT_TO_ANTI_CORRUPTION_AGENCY);
		//	item.TotalApplicationAccepCount = contracts.Count(x => x.Application.StatusId == StatusIdConst.ACCEPTED);
		//	item.TotalApplicationCanceldCount = contracts.Count(x => x.Application.StatusId == StatusIdConst.CANCELED);
		//}
		#endregion

		#region PARTNER
		public List<PrtnApplicationAndContractInfoDto> GetPrtnApplicationAndContractInfo(PrtnApplicationAndContractInfoDtoFilter filter)
		{
			var result = new List<PrtnApplicationAndContractInfoDto>();

			var query = _unitOfWork.Context.Set<Application>()
				.Include(a => a.PrtnApplication)
				.Include(a => a.PrtnContract).ThenInclude(a => a.PrtnCertificate)
				.Include(a => a.Contractor).ThenInclude(c => c.Oked).ThenInclude(a => a.OkedType)
				.Where(a => new int[]
					{
						StatusIdConst.SENT_FOR_REVIEW, StatusIdConst.SENT,
						StatusIdConst.ACCEPTED, StatusIdConst.EXECUTING,
						StatusIdConst.REJECTED, StatusIdConst.PASS_EXPERTISE,
						StatusIdConst.NOT_PASS_EXPERTISE, StatusIdConst.SIGNED,
						StatusIdConst.SIGNING, StatusIdConst.SENT_FOR_EXPERTISE,
						StatusIdConst.CANCELED, StatusIdConst.REVOKED
					}.Contains(a.StatusId)
					&& a.ApplicationTypeId == ApplicationTypeIdConst.PARTNER
				)
				.Where(a => !filter.PrtnContractTypeId.HasValue || filter.PrtnContractTypeId == a.PrtnApplication.PrtnContractTypeId);

			query = query.Where(a => (filter.StartDate.HasValue ? a.DocOn >= filter.StartDate.Value : true)
				  && (filter.EndDate.HasValue ? a.DocOn <= filter.EndDate.Value : true));

			query = query.Where(a => (!filter.RegionId.HasValue || filter.RegionId == (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId : a.RegionId))
				&& (!filter.DistrictId.HasValue || filter.DistrictId == (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId))
				&& (!filter.ContractorId.HasValue || filter.ContractorId == a.ContractorId) && (!filter.OkedTypeId.HasValue || filter.OkedTypeId == a.Contractor.Oked.OkedTypeId));

			var query2 = query.ToList();

			result = query
				.Select(a => new PrtnApplicationAndContractInfoDto
				{
					PrtnContractTypeId = filter.PrtnContractTypeId.HasValue ? a.PrtnApplication.PrtnContractTypeId : 0,
					PrtnContractType = filter.PrtnContractTypeId.HasValue ? (a.PrtnApplication.PrtnContractType.Translates.AsQueryable()
						.FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.PrtnContractType.FullName) : "",

					RegionId = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId : a.RegionId) : null,
					RegionOrderCode = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegion.OrderCode : a.Region.OrderCode) : null,
					Region = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? (a.PrtnApplication.ChoosedRegion.Translates.AsQueryable()
								.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.ChoosedRegion.FullName)
							: (a.Region.Translates.AsQueryable()
								.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Region.FullName))
						: null,

					DistrictId = filter.ByDistrict ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId) : null,
					District = filter.ByDistrict
						? (a.PrtnApplication.ChooseLocation
							? (a.PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
								.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.ChoosedDistrict.FullName)
							: (a.District.Translates.AsQueryable()
								.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.District.FullName))
						: null,

					ContractorId = filter.ByContractor ? a.ContractorId : null,
					Contractor = filter.ByContractor ? a.Contractor.FullName : null,
					ContractorInn = filter.ByContractor ? a.Contractor.Inn : null,
					ContractorPhoneNumber = filter.ByContractor ? a.Contractor.PhoneNumber : null,
					NewVacanciesCount = a.PrtnApplication.NewVacanciesCount,

					TotalPrtnApplicationSentCount = a.StatusId == StatusIdConst.SENT ? 1 : 0,
					TotalPrtnApplicationPassExpertisesCount = a.PrtnContract.StatusId == StatusIdConst.PASS_EXPERTISE ? 1 : 0,
					TotalPrtnApplicationSentForExpertisesCount = a.PrtnContract.StatusId == StatusIdConst.SENT_FOR_EXPERTISE ? 1 : 0,
					TotalPrtnApplicationNotPassExpertisesCount = a.PrtnContract.StatusId == StatusIdConst.NOT_PASS_EXPERTISE ? 1 : 0,
					TotalPrtnApplicationSignedCount = a.PrtnContract.StatusId == StatusIdConst.SIGNED ? 1 : 0,
					TotalPrtnApplicationSignningCount = a.PrtnContract.StatusId == StatusIdConst.SIGNING ? 1 : 0,
					TotalPrtnApplicationSentForReviewCount = a.StatusId == StatusIdConst.SENT_FOR_REVIEW ? 1 : 0,
					TotalPrtnApplicationSentAcceptedCount = a.StatusId == StatusIdConst.ACCEPTED ? 1 : 0,
					TotalPrtnApplicationSentRejectedCount = a.StatusId == StatusIdConst.REJECTED ? 1 : 0,
					TotalPrtnApplicationSentRevokedCount = a.StatusId == StatusIdConst.REVOKED ? 1 : 0,
					TotalPrtnApplicationCanceledCount = a.StatusId == StatusIdConst.CANCELED ? 1 : 0,

					TotalPrtnApplicationCanceledWhithOutContractCount = a.PrtnContract == null && a.StatusId == StatusIdConst.CANCELED ? 1 : 0,
					TotalPrtnApplicationCanceledWhithOutRejectCount = a.PrtnContract == null && a.StatusId == StatusIdConst.REJECTED ? 1 : 0,

					TotalPrtnContractCanceledWhithOutCertificateCount = a.PrtnContract.PrtnCertificate == null && a.PrtnContract.StatusId == StatusIdConst.CANCELED ? 1 : 0,
					TotalPrtnContractRejectWhithOutCertificateCount = a.PrtnContract.PrtnCertificate == null && a.PrtnContract.StatusId == StatusIdConst.REJECTED ? 1 : 0,

					TotalPrtnCertificateCanceledApplicationCount = (a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.CANCELED) ? 1 : 0,
					TotalPrtnCertificateRejectApplicationCount = (a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.REJECTED) ? 1 : 0,

					TotalPrtnContractCount = a.PrtnContract != null && a.PrtnContract.StatusId != StatusIdConst.REJECTED ? 1 : 0,
					TotalPrtnContractCancelCount = a.PrtnContract != null && a.PrtnContract.StatusId == StatusIdConst.CANCELED ? 1 : 0,
					TotalPrtnContractRejectedCount = a.PrtnContract != null && a.PrtnContract.StatusId == StatusIdConst.REJECTED ? 1 : 0,
					TotalPrtnCertificateCount = a.PrtnContract.PrtnCertificate != null && a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED ? 1 : 0,
					TotalPrtnCertificateCanceledCount = a.PrtnContract.PrtnCertificate != null && a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.CANCELED ? 1 : 0,
					TotalNewVacanciesCount = a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED ? a.PrtnApplication.NewVacanciesCount : 0,
					TotalPrtnApplicationIsOffersCount = a.Contractor.IsLastOffer ? 1 : 0,
				})
				.AsEnumerable()
				.GroupBy(a => new
				{
					a.PrtnContractTypeId,
					a.PrtnContractType,
					a.DistrictId,
					a.District,
					a.RegionId,
					a.RegionOrderCode,
					a.Region,
					a.ContractorId,
					a.Contractor,
					a.ContractorInn,
					a.ContractorPhoneNumber,
				})
				.Select(a => new PrtnApplicationAndContractInfoDto
				{
					PrtnContractTypeId = a.Key.PrtnContractTypeId,
					PrtnContractType = a.Key.PrtnContractType,
					DistrictId = a.Key.DistrictId,
					District = a.Key.District,
					RegionId = a.Key.RegionId,
					RegionOrderCode = a.Key.RegionOrderCode,
					Region = a.Key.Region,
					ContractorId = a.Key.ContractorId,
					Contractor = a.Key.Contractor,
					ContractorInn = a.Key.ContractorInn,
					ContractorPhoneNumber = a.Key.ContractorPhoneNumber,
					TotalPrtnApplicationSentCount = a.Sum(b => b.TotalPrtnApplicationSentCount),

					TotalApplication = ((long)a.Count(), a.Sum(c => c.NewVacanciesCount)),

					TotalPrtnApplicationCanceledWhithOutContractCount = a.Sum(b => b.TotalPrtnApplicationCanceledWhithOutContractCount),
					TotalPrtnApplicationCanceledWhithOutRejectCount = a.Sum(b => b.TotalPrtnApplicationCanceledWhithOutRejectCount),


					TotalPrtnContractCanceledWhithOutCertificateCount = a.Sum(b => b.TotalPrtnContractCanceledWhithOutCertificateCount),
					TotalPrtnContractRejectWhithOutCertificateCount = a.Sum(b => b.TotalPrtnContractRejectWhithOutCertificateCount),

					TotalPrtnCertificateCanceledApplicationCount = a.Sum(b => b.TotalPrtnCertificateCanceledApplicationCount),
					TotalPrtnCertificateRejectApplicationCount = a.Sum(b => b.TotalPrtnCertificateRejectApplicationCount),

					TotalPrtnApplicationSentForReviewCount = a.Sum(b => b.TotalPrtnApplicationSentForReviewCount),
					TotalPrtnApplicationSentAcceptedCount = a.Sum(b => b.TotalPrtnApplicationSentAcceptedCount),
					TotalPrtnApplicationPassExpertisesCount = a.Sum(b => b.TotalPrtnApplicationPassExpertisesCount),
					TotalPrtnApplicationSentForExpertisesCount = a.Sum(b => b.TotalPrtnApplicationSentForExpertisesCount),
					TotalPrtnApplicationNotPassExpertisesCount = a.Sum(b => b.TotalPrtnApplicationNotPassExpertisesCount),
					TotalPrtnApplicationSignedCount = a.Sum(b => b.TotalPrtnApplicationSignedCount),
					TotalPrtnApplicationSignningCount = a.Sum(b => b.TotalPrtnApplicationSignningCount),
					TotalPrtnApplicationCanceledCount = a.Sum(b => b.TotalPrtnApplicationCanceledCount),
					TotalPrtnApplicationSentRejectedCount = a.Sum(b => b.TotalPrtnApplicationSentRejectedCount),
					TotalPrtnApplicationSentRevokedCount = a.Sum(b => b.TotalPrtnApplicationSentRevokedCount),
					TotalPrtnContractCount = a.Sum(b => b.TotalPrtnContractCount),
					TotalPrtnContractCancelCount = a.Sum(b => b.TotalPrtnContractCancelCount),
					TotalPrtnContractRejectedCount = a.Sum(b => b.TotalPrtnContractRejectedCount),
					TotalPrtnCertificateCount = a.Sum(b => b.TotalPrtnCertificateCount),
					TotalPrtnCertificateCanceledCount = a.Sum(b => b.TotalPrtnCertificateCanceledCount),
					TotalNewVacanciesCount = a.Sum(b => b.TotalNewVacanciesCount),
					TotalPrtnApplicationCount = a.Sum(b => b.TotalPrtnApplicationSentCount) + a.Sum(b => b.TotalPrtnApplicationSentForReviewCount) + a.Sum(b => b.TotalPrtnApplicationSentAcceptedCount),
					TotalPrtnApplicationIsOffersCount = a.Sum(b => b.TotalPrtnApplicationIsOffersCount),
				})
				.ToList();

			// Hamma viloyatlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
			if (filter.ByRegion)
			{
				var regions = _unitOfWork.RegionRepository.AllAsQueryable
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.Id)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							OrderCode = a.OrderCode,
							FullName = a.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.FullName
						}
					);

				foreach (var region in regions)
				{
					if (result.Select(a => a.RegionId).Contains(region.Key))
						continue;

					result.Add(new PrtnApplicationAndContractInfoDto
					{
						Region = region.Value.FullName,
						RegionOrderCode = region.Value.OrderCode,
						RegionId = region.Key
					});
				}

				result = result.OrderBy(a => a.RegionOrderCode).ToList();
			}

			// Viloyatdagi hamma tumanlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
			if (filter.ByDistrict)
			{
				var districts = _unitOfWork.DistrictRepository.AllAsQueryable
					.Include(a => a.Region).ThenInclude(a => a.Translates)
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.RegionId
						&& !filter.DistrictId.HasValue || filter.DistrictId == a.Id
					)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							RegionId = a.RegionId,
							Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
								?.TranslateText
							?? a.Region.FullName,
							District = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
								?.TranslateText
							?? a.FullName,
							DistrictOrderCode = a.OrderCode,
						}
					);

				foreach (var district in districts)
				{
					if (result.Select(a => a.DistrictId).Contains(district.Key))
						continue;

					result.Add(new PrtnApplicationAndContractInfoDto
					{
						Region = district.Value.Region,
						RegionId = district.Value.RegionId,
						District = district.Value.District,
						RegionOrderCode = district.Value.DistrictOrderCode,
						DistrictId = district.Key
					});
				}

				result = result.OrderBy(a => a.RegionOrderCode).ToList();
			}

			return result;
		}
		//public IQueryable<ExecutationApplicationDto> GetExecutionApplication(ExecutationApplicationDtoFilter filter)
		//{
		//    var query = _unitOfWork.Context.Set<ExecutionApplication>()
		//        .Include(e => e.Region)
		//        .Include(e => e.District)
		//        .Include(e => e.Organization)
		//        .Include(t => t.Tables)
		//        .ThenInclude(prtn => prtn.PrtnCertificate)
		//        .Where(a => a.StatusId == StatusIdConst.ACCEPTED);

		//    var prtnCertificate = _unitOfWork.Context.Set<PrtnCertificate>().Where(s => s.StatusId == StatusIdConst.FORMED);


		//    query = query.Where(e => (!filter.RegionId.HasValue || e.RegionId == filter.RegionId)
		//            && (!filter.DistrictId.HasValue || e.DistrictId == filter.DistrictId)
		//            //&& (!filter.ByContractor || e.OrganizationId == filter.ContractorId)
		//            && (!filter.StartDate.HasValue || e.DocOn >= filter.StartDate.Value)
		//            && (!filter.EndDate.HasValue || e.DocOn <= filter.EndDate.Value));

		//    var result = query.Select(x => new ExecutationApplicationDto
		//    {
		//        RegionId = x.RegionId,
		//        RegionOrderCode = x.Region.OrderCode,
		//        Region = x.Region.FullName,
		//        DistrictId = x.DistrictId,
		//        District = x.District.FullName,
		//        ContractorId = x.Tables.Where(x => x.ContractorId != null).Select(x => x.PrtnCertificate.ContractorId).FirstOrDefault(),
		//        Contractor = x.Tables.Where(x => x.Contractor.FullName != null).Select(x => x.Contractor.FullName).FirstOrDefault(),
		//        ContractorInn = x.Tables.Where(x => x.Contractor.Inn != null).Select(x => x.Contractor.Inn).FirstOrDefault(),
		//        TotalContractorCount = x.Tables.Where(x => x.Contractor.FullName != null).Select(x => x.Contractor).Count(),
		//        TotalPrtnNewVacanseCount = x.Tables.Where(x => x.Contractor.FullName != null).Select(x => x.ProjectNewVacanciesCount).FirstOrDefault(),
		//    });

		//    return result;
		//}
		public List<PrtnCreditDemandInfoDto> GetPrtnCreditDemandInfo(PrtnCreditDemandInfoDtoFilter filter)
		{
			var result = new List<PrtnCreditDemandInfoDto>();

			var query = _unitOfWork.Context.Set<PrtnCreditDemand>()
				.Include(a => a.PrtnApplication)
				.Include(a => a.Contractor)
				.Include(a => a.PrtnCertificate.PrtnContract)
				.Where(a => new int[] { StatusIdConst.FORMED }.Contains(a.StatusId))
				.Where(a => !filter.ContractTypeId.HasValue || filter.ContractTypeId == a.PrtnApplication.PrtnContractTypeId);

			query = query.Where(a => (!filter.RegionId.HasValue || filter.RegionId == (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId : a.RegionId))
				&& (!filter.DistrictId.HasValue || filter.DistrictId == (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId))
				&& (!filter.ContractorId.HasValue || filter.ContractorId == a.ContractorId) && (filter.ContractorInn != null || filter.ContractorInn == a.Contractor.Inn));

			result = query
				.Select(a => new PrtnCreditDemandInfoDto
				{
					ContractTypeId = filter.ContractTypeId.HasValue ? a.PrtnApplication.PrtnContractTypeId : 0,
					ContractType = filter.ContractTypeId.HasValue ? (a.PrtnApplication.PrtnContractType.Translates.AsQueryable()
						.FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.PrtnContractType.FullName) : "",

					RegionId = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId : a.RegionId) : null,
					RegionOrderCode = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegion.OrderCode : a.Region.OrderCode) : null,
					Region = filter.ByRegion
						? (a.PrtnApplication.ChooseLocation
							? (a.PrtnApplication.ChoosedRegion.Translates.AsQueryable()
								.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.ChoosedRegion.FullName)
							: (a.Region.Translates.AsQueryable()
								.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Region.FullName))
						: null,

					DistrictId = filter.ByDistrict ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId) : null,
					District = filter.ByDistrict
						? (a.PrtnApplication.ChooseLocation
							? (a.PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
								.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.ChoosedDistrict.FullName)
							: (a.District.Translates.AsQueryable()
								.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.District.FullName))
						: null,

					ContractorId = filter.ByContractor ? a.Contractor.Id : null,
					Contractor = filter.ByContractor ? a.Contractor.FullName : null,
					ContractorInn = filter.ByContractor ? a.Contractor.Inn : null,
					ContractorPhoneNumber = filter.ByContractor ? a.BusinessmanUser.UserName : null,

					TotalDocCount = a.StatusId == StatusIdConst.FORMED ? 1 : 0,
					TotalProjectCost = a.ProjectCost,
					TotalOwnInvestment = a.OwnInvestment,
					TotalForeignInvestment = a.ForeignInvestment,
					TotalPrivillageBankCredit = a.PrivillageBankCredit,
				})
				.AsEnumerable()
				.GroupBy(a => new
				{
					a.ContractTypeId,
					a.ContractType,
					a.DistrictId,
					a.District,
					a.RegionId,
					a.RegionOrderCode,
					a.Region,
					a.ContractorId,
					a.Contractor,
					a.ContractorInn,
					a.ContractorPhoneNumber,
				})
				.Select(a => new PrtnCreditDemandInfoDto
				{
					ContractTypeId = a.Key.ContractTypeId,
					ContractType = a.Key.ContractType,
					DistrictId = a.Key.DistrictId,
					District = a.Key.District,
					RegionId = a.Key.RegionId,
					RegionOrderCode = a.Key.RegionOrderCode,
					Region = a.Key.Region,
					ContractorId = a.Key.ContractorId,
					Contractor = a.Key.Contractor,
					ContractorInn = a.Key.ContractorInn,
					ContractorPhoneNumber = a.Key.ContractorPhoneNumber,
					TotalDocCount = a.Sum(b => b.TotalDocCount),
					TotalProjectCost = a.Sum(b => b.TotalProjectCost),
					TotalOwnInvestment = a.Sum(b => b.TotalOwnInvestment),
					TotalForeignInvestment = a.Sum(b => b.TotalForeignInvestment),
					TotalPrivillageBankCredit = a.Sum(b => b.TotalPrivillageBankCredit),
				})
				.ToList();

			// Hamma viloyatlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
			if (filter.ByRegion)
			{
				var regions = _unitOfWork.RegionRepository.AllAsQueryable
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.Id)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							OrderCode = a.OrderCode,
							FullName = a.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.FullName
						}
					);

				foreach (var region in regions)
				{
					if (result.Select(a => a.RegionId).Contains(region.Key))
						continue;

					result.Add(new PrtnCreditDemandInfoDto
					{
						Region = region.Value.FullName,
						RegionOrderCode = region.Value.OrderCode,
						RegionId = region.Key
					});
				}

				result = result.OrderBy(a => a.RegionOrderCode).ToList();
			}

			// Viloyatdagi hamma tumanlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
			if (filter.ByDistrict)
			{
				var districts = _unitOfWork.DistrictRepository.AllAsQueryable
					.Include(a => a.Region).ThenInclude(a => a.Translates)
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.RegionId
						&& !filter.DistrictId.HasValue || filter.DistrictId == a.Id
					)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							DistrictOrderCode = a.OrderCode,
							RegionId = a.RegionId,
							Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.Region.FullName,
							District = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.FullName
						}
					);

				foreach (var district in districts)
				{
					if (result.Select(a => a.DistrictId).Contains(district.Key))
						continue;

					result.Add(new PrtnCreditDemandInfoDto
					{
						RegionOrderCode = district.Value.DistrictOrderCode,
						Region = district.Value.Region,
						RegionId = district.Value.RegionId,
						District = district.Value.District,
						DistrictId = district.Key
					});
				}

				result = result.OrderBy(a => a.RegionOrderCode).ToList();
			}

			return result;
		}
		public WEBASE.Models.PagedResult<PrtnCreditDemandInfoDto> GetPrtnCreditDemandInfoPaged(PrtnCreditDemandInfoDtoFilterPaged filter)
		{
			var data = GetPrtnCreditDemandInfoMethod(filter);
			return data.AsPagedResult(filter);
		}
		private IQueryable<PrtnCreditDemandInfoDto> GetPrtnCreditDemandInfoMethod(PrtnCreditDemandInfoDtoFilterPaged filter)
		{
			var result = new List<PrtnCreditDemandInfoDto>();

			var query = _unitOfWork.Context.Set<PrtnCreditDemand>()
				.Include(a => a.PrtnApplication)
				.Include(a => a.Contractor)
				.Include(a => a.PrtnCertificate.PrtnContract)
				.Where(a => new int[] { StatusIdConst.FORMED }.Contains(a.StatusId))
				.Where(a => !filter.ContractTypeId.HasValue || filter.ContractTypeId == a.PrtnApplication.PrtnContractTypeId);

			query = query.Where(a => (!filter.RegionId.HasValue || filter.RegionId == (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId : a.RegionId))
				&& (!filter.DistrictId.HasValue || filter.DistrictId == (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId))
				&& (!filter.ContractorId.HasValue || filter.ContractorId == a.ContractorId) && (filter.ContractorInn == null || filter.ContractorInn == a.Contractor.Inn)
			);

			result = query
				.Select(a => new PrtnCreditDemandInfoDto
				{
					ContractTypeId = filter.ContractTypeId.HasValue ? a.PrtnApplication.PrtnContractTypeId : 0,
					ContractType = filter.ContractTypeId.HasValue ? (a.PrtnApplication.PrtnContractType.Translates.AsQueryable()
						.FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.PrtnContractType.FullName) : "",

					RegionId = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId : a.RegionId) : null,
					RegionOrderCode = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegion.OrderCode : a.Region.OrderCode) : null,
					Region = filter.ByRegion
						? (a.PrtnApplication.ChooseLocation
							? (a.PrtnApplication.ChoosedRegion.Translates.AsQueryable()
								.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.ChoosedRegion.FullName)
							: (a.Region.Translates.AsQueryable()
								.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Region.FullName))
						: null,

					DistrictId = filter.ByDistrict ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId) : null,
					District = filter.ByDistrict
						? (a.PrtnApplication.ChooseLocation
							? (a.PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
								.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.ChoosedDistrict.FullName)
							: (a.District.Translates.AsQueryable()
								.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.District.FullName))
						: null,

					ContractorId = a.Contractor.Id,
					Contractor = a.Contractor.FullName,
					ContractorInn = a.Contractor.Inn,
					ContractorPhoneNumber = a.BusinessmanUser.UserName,

					TotalDocCount = a.StatusId == StatusIdConst.FORMED ? 1 : 0,
					TotalProjectCost = a.ProjectCost,
					TotalOwnInvestment = a.OwnInvestment,
					TotalForeignInvestment = a.ForeignInvestment,
					TotalPrivillageBankCredit = a.PrivillageBankCredit,
				}).AsEnumerable()
				.GroupBy(a => new
				{
					a.ContractTypeId,
					a.ContractType,
					a.DistrictId,
					a.District,
					a.RegionId,
					a.RegionOrderCode,
					a.Region,
					a.ContractorId,
					a.Contractor,
					a.ContractorInn,
					a.ContractorPhoneNumber,
				})
				.Select(a => new PrtnCreditDemandInfoDto
				{
					ContractTypeId = a.Key.ContractTypeId,
					ContractType = a.Key.ContractType,
					DistrictId = a.Key.DistrictId,
					District = a.Key.District,
					RegionId = a.Key.RegionId,
					RegionOrderCode = a.Key.RegionOrderCode,
					Region = a.Key.Region,
					ContractorId = a.Key.ContractorId,
					Contractor = a.Key.Contractor,
					ContractorInn = a.Key.ContractorInn,
					ContractorPhoneNumber = a.Key.ContractorPhoneNumber,
					TotalDocCount = a.Sum(b => b.TotalDocCount),
					TotalProjectCost = a.Sum(b => b.TotalProjectCost),
					TotalOwnInvestment = a.Sum(b => b.TotalOwnInvestment),
					TotalForeignInvestment = a.Sum(b => b.TotalForeignInvestment),
					TotalPrivillageBankCredit = a.Sum(b => b.TotalPrivillageBankCredit),
				})
				.ToList();

			return result.AsQueryable();
		}
		public PrtnApplicationByContractTypeDto GetPrtnApplicationByContractType(PrtnApplicationByContractTypeDtoFilter filter)
		{
			return GetPrtnApplicationByContractTypeMethod(filter);
		}
		private PrtnApplicationByContractTypeDto GetPrtnApplicationByContractTypeMethod(PrtnApplicationByContractTypeDtoFilter filter)
		{
			var result = new PrtnApplicationByContractTypeDto();

			result.Columns = _unitOfWork.PrtnContractTypeRepository.AllAsQueryable
				.Include(a => a.Translates)
				.Where(p => !filter.PrtnContractTypeId.HasValue || p.Id == filter.PrtnContractTypeId.Value)
				.IsActive().Select(a => new
				{
					Id = a.Id,
					FullName = a.Translates.AsQueryable().FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
				}).ToList()
				.ToDictionary(a => a.Id, a => a.FullName);

			var query = _unitOfWork.Context.Set<Application>()
				.Include(app => app.PrtnApplication)
				.Include(app => app.Contractor)
				.ThenInclude(con => con.Oked)
				.ThenInclude(oked => oked.OkedType)
				.Include(app => app.PrtnContract)
				.ThenInclude(prcapp => prcapp.PrtnCertificate)
				.Where(a => a.ApplicationTypeId == ApplicationTypeIdConst.PARTNER
					&& (a.StatusId == StatusIdConst.ACCEPTED
						|| a.StatusId == StatusIdConst.SENT_FOR_REVIEW
						|| a.StatusId == StatusIdConst.EXECUTING)
					&& (filter.StartDate.HasValue ? a.DocOn >= filter.StartDate.Value : true)
					&& (filter.EndDate.HasValue ? a.DocOn <= filter.EndDate.Value : true)
				)
				.Where(a => (!filter.PrtnContractTypeId.HasValue || filter.PrtnContractTypeId == a.PrtnApplication.PrtnContractTypeId) && (!filter.OkedTypeId.HasValue || filter.OkedTypeId == a.Contractor.Oked.OkedTypeId));

			query = query.Where(a => (!filter.RegionId.HasValue || filter.RegionId == (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId : a.RegionId))
				&& (!filter.DistrictId.HasValue || filter.DistrictId == (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId))
				&& (!filter.MfyId.HasValue || filter.MfyId == a.PrtnApplication.MfyId && (!filter.ContractorId.HasValue || filter.ContractorId == a.ContractorId))
			);

			result.Rows = query
					.Select(a => new PrtnApplicationByContractTypeItemDto
					{
						RegionId = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId : a.RegionId) : null,
						RegionOrderCode = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegion.OrderCode : a.Region.OrderCode) : null,
						Region = filter.ByRegion
							? (a.PrtnApplication.ChooseLocation
								? (a.PrtnApplication.ChoosedRegion.Translates.AsQueryable()
									.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.ChoosedRegion.FullName)
								: (a.Region.Translates.AsQueryable()
									.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Region.FullName))
							: null,

						DistrictId = filter.ByDistrict ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId) : null,
						District = filter.ByDistrict
							? (a.PrtnApplication.ChooseLocation
								? (a.PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
									.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.ChoosedDistrict.FullName)
								: (a.District.Translates.AsQueryable()
									.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.District.FullName))
							: null,

						Contractor = filter.ByContractor ? a.Contractor.FullName : null,
						ContractorId = filter.ByContractor ? a.ContractorId : null,
						ContractorInn = filter.ByContractor ? a.Contractor.Inn : null,
						MfyId = filter.ByMfy ? a.PrtnApplication.MfyId : null,
						Mfy = filter.ByMfy ? a.PrtnApplication.Mfy.FullName : null,
						PrtnContractTypeId = a.PrtnApplication.PrtnContractTypeId,
						NewVacanciesCount = a.PrtnApplication.NewVacanciesCount,

						CertificateCount = a.PrtnContract.PrtnCertificate != null && a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED ? 1 : 0,
						CertificateNewVacanciesCount = a.PrtnContract.PrtnCertificate != null && a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED ? a.PrtnApplication.NewVacanciesCount : 0,

						ContractCount = new int[] { StatusIdConst.PASS_EXPERTISE, StatusIdConst.SIGNED, StatusIdConst.SIGNING, StatusIdConst.SENT_FOR_EXPERTISE, StatusIdConst.NOT_PASS_EXPERTISE }.Contains(a.PrtnContract.StatusId) ? 1 : 0,
						ContractNewVacanciesCount = new int[] { StatusIdConst.PASS_EXPERTISE, StatusIdConst.SIGNED, StatusIdConst.SIGNING, StatusIdConst.SENT_FOR_EXPERTISE, StatusIdConst.NOT_PASS_EXPERTISE }.Contains(a.PrtnContract.StatusId) ? a.PrtnApplication.NewVacanciesCount : 0,

						PassExpertiseApplicationCount = a.PrtnContract.StatusId == StatusIdConst.PASS_EXPERTISE ? 1 : 0,
						PassExpertiseApplicationNewVacanciesCount = a.PrtnContract.StatusId == StatusIdConst.PASS_EXPERTISE ? a.PrtnApplication.NewVacanciesCount : 0,

						CreatedContractCount = a.PrtnContract != null && a.PrtnContract.StatusId != StatusIdConst.DELETED ? 1 : 0,
						CreatedContractNewVacanciesCount = a.PrtnContract != null && a.PrtnContract.StatusId != StatusIdConst.DELETED ? a.PrtnContract.NewVacanciesCount : 0,

						SignedContractCount = a.PrtnContract != null && a.PrtnContract.StatusId == StatusIdConst.SIGNED ? 1 : 0,
						SignedContractNewVacanciesCount = a.PrtnContract != null && a.PrtnContract.StatusId == StatusIdConst.SIGNED ? a.PrtnContract.NewVacanciesCount : 0,

						CountApplication = new Dictionary<int, (long? Count, long? NewVacanciesCount)>(),
						CountContracts = new Dictionary<int, (long? Count, long? NewVacanciesCount)>(),
						CountContractsCreated = new Dictionary<int, (long? Count, long? NewVacanciesCount)>(),
						CountContractsSigned = new Dictionary<int, (long? Count, long? NewVacanciesCount)>(),
						CountCertificates = new Dictionary<int, (long? Count, long? NewVacanciesCount)>()
					})
					.AsEnumerable()
					.GroupBy(a => new
					{
						a.RegionId,
						a.Region,
						a.RegionOrderCode,
						a.DistrictId,
						a.District,
						a.MfyId,
						a.Mfy,
						a.ContractorId,
						a.Contractor,
						a.ContractorInn,
					})
					.Select(a => new PrtnApplicationByContractTypeItemDto
					{
						RegionId = a.Key.RegionId,
						Region = a.Key.Region,
						RegionOrderCode = a.Key.RegionOrderCode,
						DistrictId = a.Key.DistrictId,
						District = a.Key.District,
						MfyId = a.Key.MfyId,
						Mfy = a.Key.Mfy,
						ContractorId = a.Key.ContractorId,
						Contractor = a.Key.Contractor,
						ContractorInn = a.Key.ContractorInn,
						TotalApplication = ((long)a.Count(), a.Sum(c => c.NewVacanciesCount)),
						TotalContract = (a.Sum(c => c.ContractCount), a.Sum(c => c.ContractNewVacanciesCount)),
						TotalPassExContract = (a.Sum(c => c.PassExpertiseApplicationCount), a.Sum(c => c.PassExpertiseApplicationNewVacanciesCount)),
						TotalContractCreated = (a.Sum(c => c.CreatedContractCount), a.Sum(c => c.CreatedContractNewVacanciesCount)),
						TotalContractSigned = (a.Sum(c => c.SignedContractCount), a.Sum(c => c.SignedContractNewVacanciesCount)),
						TotalCertificate = (a.Sum(c => c.CertificateCount), a.Sum(c => c.CertificateNewVacanciesCount)),
						CountApplication = a.GroupBy(b => b.PrtnContractTypeId.GetValueOrDefault()).ToDictionary(
							b => b.Key,
							b => ((long?)b.Count(), b.Sum(c => c.NewVacanciesCount))
						),
						CountContracts = a.GroupBy(b => b.PrtnContractTypeId.GetValueOrDefault()).ToDictionary(
							b => b.Key,
							b => (b.Sum(c => c.ContractCount), b.Sum(c => c.ContractNewVacanciesCount))
						),
						CountContractsCreated = a.GroupBy(b => b.PrtnContractTypeId.GetValueOrDefault()).ToDictionary(
							b => b.Key,
							b => (b.Sum(c => c.CreatedContractCount), b.Sum(c => c.CreatedContractNewVacanciesCount))
							),
						CountContractsSigned = a.GroupBy(b => b.PrtnContractTypeId.GetValueOrDefault()).ToDictionary(
							b => b.Key,
							b => (b.Sum(c => c.SignedContractCount), b.Sum(c => c.SignedContractNewVacanciesCount))
							),
						CountCertificates = a.GroupBy(b => b.PrtnContractTypeId.GetValueOrDefault()).ToDictionary(
							b => b.Key,
							b => (b.Sum(c => c.CertificateCount), b.Sum(c => c.CertificateNewVacanciesCount))
						),
					}).ToList();

			foreach (var row in result.Rows)
			{
				if (row.CountApplication.Count() != result.Columns.Count)
				{
					foreach (var column in result.Columns)
					{
						if (!row.CountApplication.ContainsKey(column.Key))
							row.CountApplication.Add(column.Key, (0, 0));
					}
				}
				if (row.CountContracts.Count() != result.Columns.Count)
				{
					foreach (var column in result.Columns)
					{
						if (!row.CountContracts.ContainsKey(column.Key))
							row.CountContracts.Add(column.Key, (0, 0));
					}
				}
				if (row.CountContractsCreated.Count() != result.Columns.Count)
				{
					foreach (var column in result.Columns)
					{
						if (!row.CountContractsCreated.ContainsKey(column.Key))
							row.CountContractsCreated.Add(column.Key, (0, 0));
					}
				}
				if (row.CountContractsSigned.Count() != result.Columns.Count)
				{
					foreach (var column in result.Columns)
					{
						if (!row.CountContractsSigned.ContainsKey(column.Key))
							row.CountContractsSigned.Add(column.Key, (0, 0));
					}
				}
				if (row.CountCertificates.Count() != result.Columns.Count)
				{
					foreach (var column in result.Columns)
					{
						if (!row.CountCertificates.ContainsKey(column.Key))
							row.CountCertificates.Add(column.Key, (0, 0));
					}
				}
			}

			// Hamma viloyatlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
			if (filter.ByRegion)
			{
				var regions = _unitOfWork.RegionRepository.AllAsQueryable
						.Include(a => a.Translates)
						.IsActive()
						.Where(a => (!filter.RegionId.HasValue || filter.RegionId == a.Id) && (result.Rows.Count() == 0 || !result.Rows.Select(a => a.RegionId).Contains(a.Id))
						)
						.ToDictionary(
						a => a.Id,
						a => new
						{
							OrderCode = a.OrderCode,
							FullName = a.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.FullName,
							RegionSoato = a.Soato
						}
					);
				foreach (var region in regions)
				{
					result.Rows.Add(new PrtnApplicationByContractTypeItemDto
					{
						Region = region.Value.FullName,
						RegionId = region.Key,
						RegionOrderCode = region.Value.OrderCode,
						CountApplication = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountContracts = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountContractsCreated = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountContractsSigned = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountCertificates = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
					});
				}
				result.Rows = result.Rows.OrderBy(a => a.RegionOrderCode).ThenBy(a => a.District).ToList();
			}

			// Viloyatdagi hamma tumanlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
			if (filter.ByDistrict)
			{
				var districts = _unitOfWork.DistrictRepository.AllAsQueryable
						.Include(a => a.Region).ThenInclude(a => a.Translates)
						.Include(a => a.Translates)
						.IsActive()
						.Where(a => (!filter.RegionId.HasValue || filter.RegionId == a.RegionId) && (!filter.DistrictId.HasValue || filter.DistrictId == a.Id)
						)
						.ToDictionary(
						a => a.Id,
						a => new
						{
							OrderCode = a.OrderCode,
							RegionId = a.RegionId,
							Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.Region.FullName,
							District = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.FullName,
						});
				foreach (var district in districts)
				{
					if (result.Rows.Select(a => a.DistrictId).Contains(district.Key))
					{
						result.Rows.Where(a => a.DistrictId == district.Key).ToList().ForEach(a =>
						{
							a.RegionId = district.Value.RegionId;
							a.Region = district.Value.Region;
							a.District = district.Value.District;
						});
						continue;
					}
					result.Rows.Add(new PrtnApplicationByContractTypeItemDto
					{
						RegionOrderCode = district.Value.OrderCode,
						Region = district.Value.Region,
						RegionId = district.Value.RegionId,
						District = district.Value.District,
						DistrictId = district.Key,
						CountApplication = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountContracts = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountContractsCreated = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountContractsSigned = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountCertificates = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
					});
				}
				result.Rows = result.Rows.OrderBy(a => a.RegionOrderCode)
					.ThenBy(a => a.RegionOrderCode).ToList();
			}

			// MFY boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz

			if (filter.ByMfy)
			{
				var mfys = _unitOfWork.Context.Set<Mfy>()
					.Include(a => a.Region).ThenInclude(a => a.Translates)
					.Include(a => a.District).ThenInclude(a => a.Translates)
					.IsActive()
					.Where(a => (!filter.RegionId.HasValue || filter.RegionId == a.RegionId)
					&& (!filter.DistrictId.HasValue || filter.DistrictId == a.DistrictId)
					&& (!filter.MfyId.HasValue || filter.MfyId == a.Id))
					.ToList()
					.ToDictionary(
						a => a.Id,
						a => new
						{
							RegionId = a.RegionId,
							Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.Region.FullName,
							DistrictId = a.DistrictId,
							District = a.District.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.District.FullName,
							MfyId = a.Id,
							Mfy = a.FullName,
						}
					);
				foreach (var mfy in mfys)
				{
					if (result.Rows.Select(a => a.MfyId).Contains(mfy.Key))
					{
						result.Rows.Where(a => a.MfyId == mfy.Key).ToList().ForEach(a =>
						{
							a.Region = mfy.Value.Region;
							a.District = mfy.Value.District;
							a.Mfy = mfy.Value.Mfy;
						});
						continue;
					}
					result.Rows.Add(new PrtnApplicationByContractTypeItemDto
					{
						Region = mfy.Value.Region,
						RegionId = mfy.Value.RegionId,
						District = mfy.Value.District,
						DistrictId = mfy.Value.DistrictId,
						Mfy = mfy.Value.Mfy,
						MfyId = mfy.Key,
						CountApplication = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountContracts = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountContractsCreated = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountContractsSigned = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountCertificates = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
					});
				}

				result.Rows = result.Rows.OrderBy(a => a.RegionOrderCode)
					.ThenBy(a => a.District).ThenBy(a => a.Mfy).ToList();
			}

			foreach (var column in result.Columns.Keys)
			{
				long? totalCount = result.Rows.Sum(a => GetDicValue(a.CountApplication, column).Count);
				long? totalNewVacanciesCount = result.Rows.Sum(a => GetDicValue(a.CountApplication, column).NewVacanciesCount);
				result.ApplicationColumnTotals.Add(column, (totalCount, totalNewVacanciesCount));

				totalCount = result.Rows.Sum(a => GetDicValue(a.CountContracts, column).Count);
				totalNewVacanciesCount = result.Rows.Sum(a => GetDicValue(a.CountContracts, column).NewVacanciesCount);
				result.ContractColumnTotals.Add(column, (totalCount, totalNewVacanciesCount));

				totalCount = result.Rows.Sum(a => GetDicValue(a.CountContractsCreated, column).Count);
				totalNewVacanciesCount = result.Rows.Sum(a => GetDicValue(a.CountContractsCreated, column).NewVacanciesCount);
				result.ContractColumnThatCreatedTotals.Add(column, (totalCount, totalNewVacanciesCount));

				totalCount = result.Rows.Sum(a => GetDicValue(a.CountContractsSigned, column).Count);
				totalNewVacanciesCount = result.Rows.Sum(a => GetDicValue(a.CountContractsSigned, column).NewVacanciesCount);
				result.ContractColumnThatSignedTotals.Add(column, (totalCount, totalNewVacanciesCount));

				totalCount = result.Rows.Sum(a => GetDicValue(a.CountCertificates, column).Count);
				totalNewVacanciesCount = result.Rows.Sum(a => GetDicValue(a.CountCertificates, column).NewVacanciesCount);
				result.CertificateColumnTotals.Add(column, (totalCount, totalNewVacanciesCount));
			}

			result.ApplicationTotals = (result.ApplicationColumnTotals.Sum(a => a.Value.TotalCount), result.ApplicationColumnTotals.Sum(a => a.Value.TotalNewVacanciesCount));
			result.ContractTotals = (result.ContractColumnTotals.Sum(a => a.Value.TotalCount), result.ContractColumnTotals.Sum(a => a.Value.TotalNewVacanciesCount));
			result.ContractCreatedTotals = (result.ContractColumnThatCreatedTotals.Sum(a => a.Value.TotalCount), result.ContractColumnThatCreatedTotals.Sum(a => a.Value.TotalNewVacanciesCount));
			result.ContractSignedTotals = (result.ContractColumnThatSignedTotals.Sum(a => a.Value.TotalCount), result.ContractColumnThatSignedTotals.Sum(a => a.Value.TotalNewVacanciesCount));
			result.CertificateTotals = (result.CertificateColumnTotals.Sum(a => a.Value.TotalCount), result.CertificateColumnTotals.Sum(a => a.Value.TotalNewVacanciesCount));

			return result;

			TValue GetDicValue<TKey, TValue>(Dictionary<TKey, TValue> dic, TKey key)
			{
				if (dic.ContainsKey(key))
					return dic[key];
				return default(TValue);
			}
		}
		//public PrtnApplicationByRegionDto GetPrtnApplicationByRegion(PrtnApplicationByRegionDtoFilter filter)
		//{
		//    var result = new PrtnApplicationByRegionDto();

		//    result.Columns = _unitOfWork.PrtnContractTypeRepository.AllAsQueryable
		//        .Include(a => a.Translates)
		//        .Where(p => !filter.PrtnContractTypeId.HasValue || p.Id == filter.PrtnContractTypeId.Value)
		//        .IsActive().Select(a => new
		//        {
		//            Id = a.Id,
		//            FullName = a.Translates.AsQueryable().FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
		//        }).ToList()
		//        .ToDictionary(a => a.Id, a => a.FullName);

		//    var query = _unitOfWork.Context.Set<Application>()
		//        .Include(app => app.PrtnApplication)
		//        .ThenInclude(prtn => prtn.Graphs)
		//        .Include(app => app.Contractor)
		//        .Include(app => app.PrtnContract)
		//        .ThenInclude(prcapp => prcapp.PrtnCertificate)
		//        .Where(a => a.ApplicationTypeId == ApplicationTypeIdConst.PARTNER
		//            && (a.StatusId == StatusIdConst.ACCEPTED
		//                || a.StatusId == StatusIdConst.SENT_FOR_REVIEW
		//                || a.StatusId == StatusIdConst.EXECUTING)
		//        )
		//        .Where(a => !filter.PrtnContractTypeId.HasValue || filter.PrtnContractTypeId == a.PrtnApplication.PrtnContractTypeId);

		//    query = query.Where(a => (!filter.RegionId.HasValue || filter.RegionId == (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId : a.RegionId))
		//        && (!filter.DistrictId.HasValue || filter.DistrictId == (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId))
		//        && (!filter.MfyId.HasValue || filter.MfyId == a.PrtnApplication.MfyId && (!filter.ContractorId.HasValue || filter.ContractorId == a.ContractorId) && (!filter.YearIn.HasValue || a.PrtnApplication.Graphs.Any(c => c.YearIn == filter.YearIn)))
		//    );

		//    result.Rows = query
		//            .Select(a => new PrtnApplicationByRegionItemDto
		//            {
		//                RegionId = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId : a.RegionId) : null,
		//                RegionOrderCode = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegion.OrderCode : a.Region.OrderCode) : null,
		//                Region = filter.ByRegion
		//                    ? (a.PrtnApplication.ChooseLocation
		//                        ? (a.PrtnApplication.ChoosedRegion.Translates.AsQueryable()
		//                            .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.ChoosedRegion.FullName)
		//                        : (a.Region.Translates.AsQueryable()
		//                            .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Region.FullName))
		//                    : null,

		//                DistrictId = filter.ByDistrict ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId) : null,
		//                District = filter.ByDistrict
		//                    ? (a.PrtnApplication.ChooseLocation
		//                        ? (a.PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
		//                            .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.ChoosedDistrict.FullName)
		//                        : (a.District.Translates.AsQueryable()
		//                            .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.District.FullName))
		//                    : null,

		//                Contractor = filter.ByContractor ? a.Contractor.FullName : null,
		//                ContractorId = filter.ByContractor ? a.ContractorId : null,
		//                ContractorInn = filter.ByContractor ? a.Contractor.Inn : null,
		//                MfyId = filter.ByMfy ? a.PrtnApplication.MfyId : null,
		//                Mfy = filter.ByMfy ? a.PrtnApplication.Mfy.FullName : null,
		//                PrtnContractTypeId = a.PrtnApplication.PrtnContractTypeId,
		//                NewVacanciesCount = a.PrtnApplication.NewVacanciesCount,
		//                YearIn = a.PrtnApplication.Graphs.Select(a => a.YearIn).ToList(),

		//                CertificateCount = a.PrtnContract.PrtnCertificate != null && a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED ? 1 : 0,
		//                CertificateNewVacanciesCount = a.PrtnContract.PrtnCertificate != null && a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED ? a.PrtnApplication.NewVacanciesCount : 0,

		//                CountApplication = new Dictionary<int, (long? Count, long? NewVacanciesCount)>(),
		//                CountCertificates = new Dictionary<int, (long? Count, long? NewVacanciesCount)>()
		//            })
		//            .AsEnumerable()
		//            .GroupBy(a => new
		//            {
		//                a.RegionId,
		//                a.Region,
		//                a.RegionOrderCode,
		//                a.DistrictId,
		//                a.District,
		//                a.MfyId,
		//                a.Mfy,
		//                a.ContractorId,
		//                a.Contractor,
		//                a.ContractorInn,
		//                a.YearIn
		//            })
		//            .Select(a => new PrtnApplicationByRegionItemDto
		//            {
		//                RegionId = a.Key.RegionId,
		//                Region = a.Key.Region,
		//                RegionOrderCode = a.Key.RegionOrderCode,
		//                DistrictId = a.Key.DistrictId,
		//                District = a.Key.District,
		//                MfyId = a.Key.MfyId,
		//                Mfy = a.Key.Mfy,
		//                ContractorId = a.Key.ContractorId,
		//                Contractor = a.Key.Contractor,
		//                ContractorInn = a.Key.ContractorInn,
		//                YearIn = a.Key.YearIn.SelectMany(year => year).Distinct().ToList(),
		//                TotalApplication = ((long)a.Count(), a.Sum(c => c.NewVacanciesCount)),
		//                TotalCertificate = (a.Sum(c => c.CertificateCount), a.Sum(c => c.CertificateNewVacanciesCount)),
		//                CountApplication = a.GroupBy(b => b.PrtnContractTypeId.GetValueOrDefault()).ToDictionary(
		//                    b => b.Key,
		//                    b => ((long?)b.Count(), b.Sum(c => c.NewVacanciesCount))
		//                ),
		//                CountCertificates = a.GroupBy(b => b.PrtnContractTypeId.GetValueOrDefault()).ToDictionary(
		//                    b => b.Key,
		//                    b => (b.Sum(c => c.CertificateCount), b.Sum(c => c.CertificateNewVacanciesCount))
		//                ),
		//            }).ToList();

		//    foreach (var row in result.Rows)
		//    {
		//        if (row.CountApplication.Count() != result.Columns.Count)
		//        {
		//            foreach (var column in result.Columns)
		//            {
		//                if (!row.CountApplication.ContainsKey(column.Key))
		//                    row.CountApplication.Add(column.Key, (0, 0));
		//            }
		//        }
		//        if (row.CountCertificates.Count() != result.Columns.Count)
		//        {
		//            foreach (var column in result.Columns)
		//            {
		//                if (!row.CountCertificates.ContainsKey(column.Key))
		//                    row.CountCertificates.Add(column.Key, (0, 0));
		//            }
		//        }
		//    }

		//    // Hamma viloyatlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
		//    if (filter.ByRegion)
		//    {
		//        var regions = _unitOfWork.RegionRepository.AllAsQueryable
		//                .Include(a => a.Translates)
		//                .IsActive()
		//                .Where(a => (!filter.RegionId.HasValue || filter.RegionId == a.Id) && (result.Rows.Count() == 0 || !result.Rows.Select(a => a.RegionId).Contains(a.Id))
		//                )
		//                .ToDictionary(
		//                a => a.Id,
		//                a => new
		//                {
		//                    OrderCode = a.OrderCode,
		//                    FullName = a.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.FullName,
		//                    RegionSoato = a.Soato
		//                }
		//            );
		//        foreach (var region in regions)
		//        {
		//            result.Rows.Add(new PrtnApplicationByRegionItemDto
		//            {
		//                Region = region.Value.FullName,
		//                RegionId = region.Key,
		//                RegionOrderCode = region.Value.OrderCode,
		//                CountApplication = result.Columns.ToDictionary(
		//                    a => a.Key,
		//                    a => ((long?)0, (long?)0)
		//                ),
		//                CountCertificates = result.Columns.ToDictionary(
		//                    a => a.Key,
		//                    a => ((long?)0, (long?)0)
		//                ),
		//            });
		//        }
		//        result.Rows = result.Rows.OrderBy(a => a.RegionOrderCode).ThenBy(a => a.District).ToList();
		//    }

		//    // Viloyatdagi hamma tumanlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
		//    if (filter.ByDistrict)
		//    {
		//        var districts = _unitOfWork.DistrictRepository.AllAsQueryable
		//                .Include(a => a.Region).ThenInclude(a => a.Translates)
		//                .Include(a => a.Translates)
		//                .IsActive()
		//                .Where(a => (!filter.RegionId.HasValue || filter.RegionId == a.RegionId) && (!filter.DistrictId.HasValue || filter.DistrictId == a.Id)
		//                )
		//                .ToDictionary(
		//                a => a.Id,
		//                a => new
		//                {
		//                    OrderCode = a.OrderCode,
		//                    RegionId = a.RegionId,
		//                    Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.Region.FullName,
		//                    District = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.FullName,
		//                });
		//        foreach (var district in districts)
		//        {
		//            if (result.Rows.Select(a => a.DistrictId).Contains(district.Key))
		//            {
		//                result.Rows.Where(a => a.DistrictId == district.Key).ToList().ForEach(a =>
		//                {
		//                    a.Region = district.Value.Region;
		//                    a.District = district.Value.District;
		//                });
		//                continue;
		//            }
		//            result.Rows.Add(new PrtnApplicationByRegionItemDto
		//            {
		//                RegionOrderCode = district.Value.OrderCode,
		//                Region = district.Value.Region,
		//                RegionId = district.Value.RegionId,
		//                District = district.Value.District,
		//                DistrictId = district.Key,
		//                CountApplication = result.Columns.ToDictionary(
		//                    a => a.Key,
		//                    a => ((long?)0, (long?)0)
		//                ),
		//                CountCertificates = result.Columns.ToDictionary(
		//                    a => a.Key,
		//                    a => ((long?)0, (long?)0)
		//                ),
		//            });
		//        }
		//        result.Rows = result.Rows.OrderBy(a => a.RegionOrderCode)
		//            .ThenBy(a => a.RegionOrderCode).ToList();
		//    }

		//    // MFY boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz

		//    if (filter.ByMfy)
		//    {
		//        var mfys = _unitOfWork.Context.Set<Mfy>()
		//            .Include(a => a.Region).ThenInclude(a => a.Translates)
		//            .Include(a => a.District).ThenInclude(a => a.Translates)
		//            .IsActive()
		//            .Where(a => (!filter.RegionId.HasValue || filter.RegionId == a.RegionId)
		//            && (!filter.DistrictId.HasValue || filter.DistrictId == a.DistrictId)
		//            && (!filter.MfyId.HasValue || filter.MfyId == a.Id))
		//            .ToList()
		//            .ToDictionary(
		//                a => a.Id,
		//                a => new
		//                {
		//                    RegionId = a.RegionId,
		//                    Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.Region.FullName,
		//                    DistrictId = a.DistrictId,
		//                    District = a.District.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.District.FullName,
		//                    MfyId = a.Id,
		//                    Mfy = a.FullName,
		//                }
		//            );
		//        foreach (var mfy in mfys)
		//        {
		//            if (result.Rows.Select(a => a.MfyId).Contains(mfy.Key))
		//            {
		//                result.Rows.Where(a => a.MfyId == mfy.Key).ToList().ForEach(a =>
		//                {
		//                    a.Region = mfy.Value.Region;
		//                    a.District = mfy.Value.District;
		//                    a.Mfy = mfy.Value.Mfy;
		//                });
		//                continue;
		//            }
		//            result.Rows.Add(new PrtnApplicationByRegionItemDto
		//            {
		//                Region = mfy.Value.Region,
		//                RegionId = mfy.Value.RegionId,
		//                District = mfy.Value.District,
		//                DistrictId = mfy.Value.DistrictId,
		//                Mfy = mfy.Value.Mfy,
		//                MfyId = mfy.Key,
		//                CountApplication = result.Columns.ToDictionary(
		//                    a => a.Key,
		//                    a => ((long?)0, (long?)0)
		//                ),
		//                CountCertificates = result.Columns.ToDictionary(
		//                    a => a.Key,
		//                    a => ((long?)0, (long?)0)
		//                ),
		//            });
		//        }

		//        result.Rows = result.Rows.OrderBy(a => a.RegionOrderCode)
		//            .ThenBy(a => a.District).ThenBy(a => a.Mfy).ToList();
		//    }

		//    foreach (var column in result.Columns.Keys)
		//    {
		//        long? totalCount = result.Rows.Sum(a => GetDicValue(a.CountApplication, column).Count);
		//        long? totalNewVacanciesCount = result.Rows.Sum(a => GetDicValue(a.CountApplication, column).NewVacanciesCount);
		//        result.ApplicationColumnTotals.Add(column, (totalCount, totalNewVacanciesCount));

		//        totalCount = result.Rows.Sum(a => GetDicValue(a.CountCertificates, column).Count);
		//        totalNewVacanciesCount = result.Rows.Sum(a => GetDicValue(a.CountCertificates, column).NewVacanciesCount);
		//        result.CertificateColumnTotals.Add(column, (totalCount, totalNewVacanciesCount));
		//    }

		//    result.ApplicationTotals = (result.ApplicationColumnTotals.Sum(a => a.Value.TotalCount), result.ApplicationColumnTotals.Sum(a => a.Value.TotalNewVacanciesCount));
		//    result.CertificateTotals = (result.CertificateColumnTotals.Sum(a => a.Value.TotalCount), result.CertificateColumnTotals.Sum(a => a.Value.TotalNewVacanciesCount));

		//    return result;

		//    TValue GetDicValue<TKey, TValue>(Dictionary<TKey, TValue> dic, TKey key)
		//    {
		//        if (dic.ContainsKey(key))
		//            return dic[key];
		//        return default(TValue);
		//    }
		//}
		public PrtnApplicationByContractTypeDto GetPrtnApplicationByContractTypePaged(PrtnApplicationByContractTypeDtoFiler2 filter)
		{
			var result = new PrtnApplicationByContractTypeDto();


			result.Columns = _unitOfWork.PrtnContractTypeRepository.AllAsQueryable
					.Include(a => a.Translates)
					.Where(p => !filter.PrtnContractTypeId.HasValue || p.Id == filter.PrtnContractTypeId.Value)
					.IsActive().Select(a => new
					{
						Id = a.Id,
						FullName = a.Translates.AsQueryable().FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
					}).ToList()
					.ToDictionary(a => a.Id, a => a.FullName);

			var query = _unitOfWork.Context.Set<Application>()
				.Include(app => app.PrtnApplication)
				.Include(app => app.Contractor)
				.Include(app => app.PrtnContract)
				.ThenInclude(prcapp => prcapp.PrtnCertificate)
				.Where(a => a.ApplicationTypeId == ApplicationTypeIdConst.PARTNER
					&& (a.StatusId == StatusIdConst.ACCEPTED
						|| a.StatusId == StatusIdConst.SENT_FOR_REVIEW
						|| a.StatusId == StatusIdConst.EXECUTING)
					&& (filter.StartDate.HasValue ? a.DocOn >= filter.StartDate.Value : true)
					&& (filter.EndDate.HasValue ? a.DocOn <= filter.EndDate.Value : true)
				)
				.Where(a => !filter.PrtnContractTypeId.HasValue || filter.PrtnContractTypeId == a.PrtnApplication.PrtnContractTypeId);

			query = query.Where(a => (!filter.RegionId.HasValue || filter.RegionId == (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId : a.RegionId))
				&& (!filter.DistrictId.HasValue || filter.DistrictId == (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId))
				&& (!filter.MfyId.HasValue || filter.MfyId == a.PrtnApplication.MfyId && (!filter.ContractorId.HasValue || filter.ContractorId == a.ContractorId)) && (filter.ContractorInn == null || filter.ContractorInn == a.Contractor.Inn)
			);

			result.Rows = query
					.Select(a => new PrtnApplicationByContractTypeItemDto
					{
						RegionId = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId : a.RegionId) : null,
						RegionOrderCode = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegion.OrderCode : a.Region.OrderCode) : null,
						Region = filter.ByRegion
							? (a.PrtnApplication.ChooseLocation
								? (a.PrtnApplication.ChoosedRegion.Translates.AsQueryable()
									.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.ChoosedRegion.FullName)
								: (a.Region.Translates.AsQueryable()
									.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Region.FullName))
							: null,

						DistrictId = filter.ByDistrict ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId) : null,
						District = filter.ByDistrict
							? (a.PrtnApplication.ChooseLocation
								? (a.PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
									.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.ChoosedDistrict.FullName)
								: (a.District.Translates.AsQueryable()
									.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.District.FullName))
							: null,

						Contractor = filter.ByContractor ? a.Contractor.FullName : null,
						ContractorId = filter.ByContractor ? a.ContractorId : null,
						ContractorInn = filter.ByContractor ? a.Contractor.Inn : null,
						MfyId = filter.ByMfy ? a.PrtnApplication.MfyId : null,
						Mfy = filter.ByMfy ? a.PrtnApplication.Mfy.FullName : null,
						PrtnContractTypeId = a.PrtnApplication.PrtnContractTypeId,
						NewVacanciesCount = a.PrtnApplication.NewVacanciesCount,

						CertificateCount = a.PrtnContract.PrtnCertificate != null && a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED ? 1 : 0,
						CertificateNewVacanciesCount = a.PrtnContract.PrtnCertificate != null && a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED ? a.PrtnApplication.NewVacanciesCount : 0,

						ContractCount = new int[] { StatusIdConst.PASS_EXPERTISE, StatusIdConst.SIGNED, StatusIdConst.SIGNING, StatusIdConst.SENT_FOR_EXPERTISE, StatusIdConst.NOT_PASS_EXPERTISE }.Contains(a.PrtnContract.StatusId) ? 1 : 0,
						ContractNewVacanciesCount = new int[] { StatusIdConst.PASS_EXPERTISE, StatusIdConst.SIGNED, StatusIdConst.SIGNING, StatusIdConst.SENT_FOR_EXPERTISE, StatusIdConst.NOT_PASS_EXPERTISE }.Contains(a.PrtnContract.StatusId) ? a.PrtnApplication.NewVacanciesCount : 0,

						CreatedContractCount = a.PrtnContract != null && a.PrtnContract.StatusId != StatusIdConst.DELETED ? 1 : 0,
						CreatedContractNewVacanciesCount = a.PrtnContract != null && a.PrtnContract.StatusId != StatusIdConst.DELETED ? a.PrtnContract.NewVacanciesCount : 0,

						SignedContractCount = a.PrtnContract != null && a.PrtnContract.StatusId == StatusIdConst.SIGNED ? 1 : 0,
						SignedContractNewVacanciesCount = a.PrtnContract != null && a.PrtnContract.StatusId == StatusIdConst.SIGNED ? a.PrtnContract.NewVacanciesCount : 0,

						CountApplication = new Dictionary<int, (long? Count, long? NewVacanciesCount)>(),
						CountContracts = new Dictionary<int, (long? Count, long? NewVacanciesCount)>(),
						CountContractsCreated = new Dictionary<int, (long? Count, long? NewVacanciesCount)>(),
						CountContractsSigned = new Dictionary<int, (long? Count, long? NewVacanciesCount)>(),
						CountCertificates = new Dictionary<int, (long? Count, long? NewVacanciesCount)>()
					})
					.Skip((filter.Page - 1) * filter.PageSize)
					.Take(filter.PageSize)
					.AsEnumerable()
					.GroupBy(a => new
					{
						a.RegionId,
						a.Region,
						a.RegionOrderCode,
						a.DistrictId,
						a.District,
						a.MfyId,
						a.Mfy,
						a.ContractorId,
						a.Contractor,
						a.ContractorInn,
					})
					.Select(a => new PrtnApplicationByContractTypeItemDto
					{
						RegionId = a.Key.RegionId,
						Region = a.Key.Region,
						RegionOrderCode = a.Key.RegionOrderCode,
						DistrictId = a.Key.DistrictId,
						District = a.Key.District,
						MfyId = a.Key.MfyId,
						Mfy = a.Key.Mfy,
						ContractorId = a.Key.ContractorId,
						Contractor = a.Key.Contractor,
						ContractorInn = a.Key.ContractorInn,
						TotalApplication = ((long)a.Count(), a.Sum(c => c.NewVacanciesCount)),
						TotalContract = (a.Sum(c => c.ContractCount), a.Sum(c => c.ContractNewVacanciesCount)),
						TotalContractCreated = (a.Sum(c => c.CreatedContractCount), a.Sum(c => c.CreatedContractNewVacanciesCount)),
						TotalContractSigned = (a.Sum(c => c.SignedContractCount), a.Sum(c => c.SignedContractNewVacanciesCount)),
						TotalCertificate = (a.Sum(c => c.CertificateCount), a.Sum(c => c.CertificateNewVacanciesCount)),
						CountApplication = a.GroupBy(b => b.PrtnContractTypeId.GetValueOrDefault()).ToDictionary(
							b => b.Key,
							b => ((long?)b.Count(), b.Sum(c => c.NewVacanciesCount))
						),
						CountContracts = a.GroupBy(b => b.PrtnContractTypeId.GetValueOrDefault()).ToDictionary(
							b => b.Key,
							b => (b.Sum(c => c.ContractCount), b.Sum(c => c.ContractNewVacanciesCount))
						),
						CountContractsCreated = a.GroupBy(b => b.PrtnContractTypeId.GetValueOrDefault()).ToDictionary(
							b => b.Key,
							b => (b.Sum(c => c.CreatedContractCount), b.Sum(c => c.CreatedContractNewVacanciesCount))
							),
						CountContractsSigned = a.GroupBy(b => b.PrtnContractTypeId.GetValueOrDefault()).ToDictionary(
							b => b.Key,
							b => (b.Sum(c => c.SignedContractCount), b.Sum(c => c.SignedContractNewVacanciesCount))
							),
						CountCertificates = a.GroupBy(b => b.PrtnContractTypeId.GetValueOrDefault()).ToDictionary(
							b => b.Key,
							b => (b.Sum(c => c.CertificateCount), b.Sum(c => c.CertificateNewVacanciesCount))
						),
					}).ToList();

			foreach (var row in result.Rows)
			{
				if (row.CountApplication.Count() != result.Columns.Count)
				{
					foreach (var column in result.Columns)
					{
						if (!row.CountApplication.ContainsKey(column.Key))
							row.CountApplication.Add(column.Key, (0, 0));
					}
				}
				if (row.CountContracts.Count() != result.Columns.Count)
				{
					foreach (var column in result.Columns)
					{
						if (!row.CountContracts.ContainsKey(column.Key))
							row.CountContracts.Add(column.Key, (0, 0));
					}
				}
				if (row.CountContractsCreated.Count() != result.Columns.Count)
				{
					foreach (var column in result.Columns)
					{
						if (!row.CountContractsCreated.ContainsKey(column.Key))
							row.CountContractsCreated.Add(column.Key, (0, 0));
					}
				}
				if (row.CountContractsSigned.Count() != result.Columns.Count)
				{
					foreach (var column in result.Columns)
					{
						if (!row.CountContractsSigned.ContainsKey(column.Key))
							row.CountContractsSigned.Add(column.Key, (0, 0));
					}
				}
				if (row.CountCertificates.Count() != result.Columns.Count)
				{
					foreach (var column in result.Columns)
					{
						if (!row.CountCertificates.ContainsKey(column.Key))
							row.CountCertificates.Add(column.Key, (0, 0));
					}
				}
			}

			// Hamma viloyatlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
			if (filter.ByRegion)
			{
				var regions = _unitOfWork.RegionRepository.AllAsQueryable
						.Include(a => a.Translates)
						.IsActive()
						.Where(a => (!filter.RegionId.HasValue || filter.RegionId == a.Id) && (result.Rows.Count() == 0 || !result.Rows.Select(a => a.RegionId).Contains(a.Id))
						)
						.ToDictionary(
						a => a.Id,
						a => new
						{
							OrderCode = a.OrderCode,
							FullName = a.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.FullName,
							RegionSoato = a.Soato
						}
					);
				foreach (var region in regions)
				{
					result.Rows.Add(new PrtnApplicationByContractTypeItemDto
					{
						Region = region.Value.FullName,
						RegionId = region.Key,
						RegionOrderCode = region.Value.OrderCode,
						CountApplication = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountContracts = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountContractsCreated = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountContractsSigned = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountCertificates = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
					});
				}
				result.Rows = result.Rows.OrderBy(a => a.RegionOrderCode).ThenBy(a => a.District).ToList();
			}

			// Viloyatdagi hamma tumanlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
			if (filter.ByDistrict)
			{
				var districts = _unitOfWork.DistrictRepository.AllAsQueryable
						.Include(a => a.Region).ThenInclude(a => a.Translates)
						.Include(a => a.Translates)
						.IsActive()
						.Where(a => (!filter.RegionId.HasValue || filter.RegionId == a.RegionId) && (!filter.DistrictId.HasValue || filter.DistrictId == a.Id)
						)
						.ToDictionary(
						a => a.Id,
						a => new
						{
							OrderCode = a.OrderCode,
							RegionId = a.RegionId,
							Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.Region.FullName,
							District = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.FullName,
						});
				foreach (var district in districts)
				{
					if (result.Rows.Select(a => a.DistrictId).Contains(district.Key))
					{
						result.Rows.Where(a => a.DistrictId == district.Key).ToList().ForEach(a =>
						{
							a.Region = district.Value.Region;
							a.District = district.Value.District;
						});
						continue;
					}
					result.Rows.Add(new PrtnApplicationByContractTypeItemDto
					{
						RegionOrderCode = district.Value.OrderCode,
						Region = district.Value.Region,
						RegionId = district.Value.RegionId,
						District = district.Value.District,
						DistrictId = district.Key,
						CountApplication = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountContracts = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountContractsCreated = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountContractsSigned = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountCertificates = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
					});
				}
				result.Rows = result.Rows.OrderBy(a => a.RegionOrderCode)
					.ThenBy(a => a.RegionOrderCode).ToList();
			}

			// MFY boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz

			if (filter.ByMfy)
			{
				var mfys = _unitOfWork.Context.Set<Mfy>()
					.Include(a => a.Region).ThenInclude(a => a.Translates)
					.Include(a => a.District).ThenInclude(a => a.Translates)
					.IsActive()
					.Where(a => (!filter.RegionId.HasValue || filter.RegionId == a.RegionId)
					&& (!filter.DistrictId.HasValue || filter.DistrictId == a.DistrictId)
					&& (!filter.MfyId.HasValue || filter.MfyId == a.Id))
					.ToList()
					.ToDictionary(
						a => a.Id,
						a => new
						{
							RegionId = a.RegionId,
							Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.Region.FullName,
							DistrictId = a.DistrictId,
							District = a.District.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.District.FullName,
							MfyId = a.Id,
							Mfy = a.FullName,
						}
					);
				foreach (var mfy in mfys)
				{
					if (result.Rows.Select(a => a.MfyId).Contains(mfy.Key))
					{
						result.Rows.Where(a => a.MfyId == mfy.Key).ToList().ForEach(a =>
						{
							a.Region = mfy.Value.Region;
							a.District = mfy.Value.District;
							a.Mfy = mfy.Value.Mfy;
						});
						continue;
					}
					result.Rows.Add(new PrtnApplicationByContractTypeItemDto
					{
						Region = mfy.Value.Region,
						RegionId = mfy.Value.RegionId,
						District = mfy.Value.District,
						DistrictId = mfy.Value.DistrictId,
						Mfy = mfy.Value.Mfy,
						MfyId = mfy.Key,
						CountApplication = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountContracts = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountContractsCreated = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountContractsSigned = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
						CountCertificates = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long?)0, (long?)0)
						),
					});
				}

				result.Rows = result.Rows.OrderBy(a => a.RegionOrderCode)
					.ThenBy(a => a.District).ThenBy(a => a.Mfy).ToList();
			}

			foreach (var column in result.Columns.Keys)
			{
				long? totalCount = result.Rows.Sum(a => GetDicValue(a.CountApplication, column).Count);
				long? totalNewVacanciesCount = result.Rows.Sum(a => GetDicValue(a.CountApplication, column).NewVacanciesCount);
				result.ApplicationColumnTotals.Add(column, (totalCount, totalNewVacanciesCount));

				totalCount = result.Rows.Sum(a => GetDicValue(a.CountContracts, column).Count);
				totalNewVacanciesCount = result.Rows.Sum(a => GetDicValue(a.CountContracts, column).NewVacanciesCount);
				result.ContractColumnTotals.Add(column, (totalCount, totalNewVacanciesCount));

				totalCount = result.Rows.Sum(a => GetDicValue(a.CountContractsCreated, column).Count);
				totalNewVacanciesCount = result.Rows.Sum(a => GetDicValue(a.CountContractsCreated, column).NewVacanciesCount);
				result.ContractColumnThatCreatedTotals.Add(column, (totalCount, totalNewVacanciesCount));

				totalCount = result.Rows.Sum(a => GetDicValue(a.CountContractsSigned, column).Count);
				totalNewVacanciesCount = result.Rows.Sum(a => GetDicValue(a.CountContractsSigned, column).NewVacanciesCount);
				result.ContractColumnThatSignedTotals.Add(column, (totalCount, totalNewVacanciesCount));

				totalCount = result.Rows.Sum(a => GetDicValue(a.CountCertificates, column).Count);
				totalNewVacanciesCount = result.Rows.Sum(a => GetDicValue(a.CountCertificates, column).NewVacanciesCount);
				result.CertificateColumnTotals.Add(column, (totalCount, totalNewVacanciesCount));
			}

			result.ApplicationTotals = (result.ApplicationColumnTotals.Sum(a => a.Value.TotalCount), result.ApplicationColumnTotals.Sum(a => a.Value.TotalNewVacanciesCount));
			result.ContractTotals = (result.ContractColumnTotals.Sum(a => a.Value.TotalCount), result.ContractColumnTotals.Sum(a => a.Value.TotalNewVacanciesCount));
			result.ContractCreatedTotals = (result.ContractColumnThatCreatedTotals.Sum(a => a.Value.TotalCount), result.ContractColumnThatCreatedTotals.Sum(a => a.Value.TotalNewVacanciesCount));
			result.ContractSignedTotals = (result.ContractColumnThatSignedTotals.Sum(a => a.Value.TotalCount), result.ContractColumnThatSignedTotals.Sum(a => a.Value.TotalNewVacanciesCount));
			result.CertificateTotals = (result.CertificateColumnTotals.Sum(a => a.Value.TotalCount), result.CertificateColumnTotals.Sum(a => a.Value.TotalNewVacanciesCount));

			TValue GetDicValue<TKey, TValue>(Dictionary<TKey, TValue> dic, TKey key)
			{
				if (dic.ContainsKey(key))
					return dic[key];
				return default(TValue);
			};


			return result;
		}
		public PrtnEmploymentGraphReportDto GetPrtnEmploymentGraphReport(PrtnEmploymentGraphPageOption filter, bool isPrint = false)
		{
			var res = new PrtnEmploymentGraphReportDto();

			res.Columns = (!filter.YearIn.HasValue || filter.YearIn.Value == CommonConst._2023)
				? this._manualService.GetMonthSelectList().Where(a => a.Value >= CommonConst.IYUN).ToDictionary(month => month.Value, month => month.Text)
				: this._manualService.GetMonthSelectList().ToDictionary(month => month.Value, month => month.Text);

			#region For Tax Employee Count
			var untilFounded = _unitOfWork.Context.Set<EmployeeCount>()
				.Where(emp => ((emp.Year == CommonConst._2023 && emp.Month == CommonConst.MAY)))
				.GroupBy(emp => emp.Tin)
				.Select(emp => new
				{
					Inn = emp.Key,
					EmployeeCount = emp.Sum(a => a.MonthlyNumberEmployees)
				})
				.ToDictionary(emp => emp.Inn, emp => emp.EmployeeCount);

			var monthly = _unitOfWork.Context.Set<EmployeeCount>()
						.Where(tax => (!filter.YearIn.HasValue || tax.Year == filter.YearIn.Value));
			#endregion

			var contractor = _unitOfWork.Context.Set<Contractor>()
				.Where(a => a.Applications.Any());

			contractor = contractor
				.Where(con => con.StateId != StateIdConst.PASSIVE
						 && (!filter.ContractorId.HasValue || con.Id == filter.ContractorId.Value)
						 && (string.IsNullOrEmpty(filter.ContractorInn) || con.Inn == filter.ContractorInn)
						 && (!filter.RegionId.HasValue || con.RegionId == filter.RegionId.Value)
						 && (!filter.DistrictId.HasValue || con.DistrictId == filter.DistrictId.Value));

			contractor = contractor
				.Where(con => con.Applications.Any(app =>
					app.ApplicationTypeId == ApplicationTypeIdConst.PARTNER
					&& app.StatusId == StatusIdConst.ACCEPTED
					&& app.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED
					&& (!filter.MfyId.HasValue || app.PrtnApplication.MfyId == filter.MfyId.Value)));

			res.TotalCount = contractor.Count();

			contractor = contractor.Where(a =>
				   (!filter.RegionId.HasValue ||
						(a.Applications.FirstOrDefault() != null && a.Applications.FirstOrDefault().PrtnApplication.ChooseLocation
							? a.Applications.FirstOrDefault().PrtnApplication.ChoosedRegionId
							: a.RegionId) == filter.RegionId.Value)
				&& (!filter.DistrictId.HasValue ||
						(a.Applications.FirstOrDefault() != null && a.Applications.FirstOrDefault().PrtnApplication.ChooseLocation
							? a.Applications.FirstOrDefault().PrtnApplication.ChoosedDistrictId
							: a.DistrictId) == filter.DistrictId.Value));

			if (isPrint)
			{
				res.Rows = contractor
				.Select(a => new PrtnEmploymentGraphReportRowsDto
				{
					RegionId = a.Applications.FirstOrDefault() != null && a.Applications.FirstOrDefault().PrtnApplication.ChooseLocation
										? a.Applications.FirstOrDefault().PrtnApplication.ChoosedRegionId
										: a.RegionId,
					RegionOrderCode = a.Applications.FirstOrDefault() != null && a.Applications.FirstOrDefault().PrtnApplication.ChooseLocation
										? a.Applications.FirstOrDefault().PrtnApplication.ChoosedRegion.OrderCode
										: a.Region.OrderCode,
					Region = a.Applications.FirstOrDefault() != null && a.Applications.FirstOrDefault().PrtnApplication.ChooseLocation
										? (a.Applications.FirstOrDefault().PrtnApplication.ChoosedRegion.Translates.AsQueryable()
												.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
												?? a.Applications.FirstOrDefault().PrtnApplication.ChoosedRegion.FullName)
										: (a.Region.Translates.AsQueryable()
												.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
												?? a.Region.FullName),

					DistrictId = a.Applications.FirstOrDefault() != null && a.Applications.FirstOrDefault().PrtnApplication.ChooseLocation
										? a.Applications.FirstOrDefault().PrtnApplication.ChoosedDistrictId
										: a.DistrictId,
					District = a.Applications.FirstOrDefault() != null && a.Applications.FirstOrDefault().PrtnApplication.ChooseLocation
										? (a.Applications.FirstOrDefault().PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
												.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
												?? a.Applications.FirstOrDefault().PrtnApplication.ChoosedDistrict.FullName)
										: (a.District.Translates.AsQueryable()
												.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
												?? a.District.FullName),


					ContractorName = a.FullName,
					ContractorId = a.Id,
					ContractorInn = a.Inn,

					MfyId = a.Applications.FirstOrDefault() != null ? a.Applications.FirstOrDefault().PrtnApplication.MfyId : 0,
					Mfy = a.Applications.FirstOrDefault() != null ? a.Applications.FirstOrDefault().PrtnApplication.Mfy.FullName : "",

					EmployeesCountUntilFounded = untilFounded.ContainsKey(a.Inn)
						? (long)untilFounded[a.Inn]
						: 0,

					GraphYears = a.Applications.SelectMany(app => app.PrtnApplication.Graphs)
						.Where(b => b.YearIn == CommonConst._2023 || b.YearIn == CommonConst._2024 ||
									b.YearIn == CommonConst._2025 || b.YearIn == CommonConst._2026)
						.Sum(b => b.NewVacanciesCount),

					PlanGrap = a.Applications.SelectMany(app => app.PrtnApplication.Graphs)
						.Where(graph => !filter.YearIn.HasValue || graph.YearIn == filter.YearIn.Value)
						.GroupBy(graph => new { graph.YearIn, graph.MonthIn })
						.Select(graph => new PrtnEmploymentGraphReportItemsDto()
						{
							YearIn = graph.Key.YearIn,
							MonthIn = graph.Key.MonthIn,
							ParamSumm = graph.Sum(b => b.NewVacanciesCount)
						}),

					ByReport = monthly
						.Where(tax => tax.Tin == a.Inn)
						.AsEnumerable()
						.GroupBy(tax => new { tax.Year, tax.Month })
						.Select(tax => new PrtnEmploymentGraphReportItemsDto()
						{
							YearIn = tax.Key.Year,
							MonthIn = tax.Key.Month,
							ParamSumm = (long)tax.Sum(b => b.MonthlyNumberEmployees)
						}),

					RowsMonthly = new Dictionary<int, (long PlanGraphCount, long DifferenceGraphAndReportCount, long DifferenceCount)>(),
				})
				.AsEnumerable()
				.GroupBy(a => new
				{
					a.RegionId,
					a.Region,
					a.RegionOrderCode,
					a.DistrictId,
					a.District,
					a.MfyId,
					a.Mfy,
					a.ContractorId,
					a.ContractorName,
					a.ContractorInn,
				})
				.OrderByDescending(a => a.Key.ContractorInn)
				.Select(a => new PrtnEmploymentGraphReportRowsDto
				{
					RegionId = a.Key.RegionId,
					Region = a.Key.Region,
					RegionOrderCode = a.Key.RegionOrderCode,
					DistrictId = a.Key.DistrictId,
					District = a.Key.District,
					MfyId = a.Key.MfyId,
					Mfy = a.Key.Mfy,
					ContractorId = a.Key.ContractorId,
					ContractorName = a.Key.ContractorName,
					ContractorInn = a.Key.ContractorInn,
					EmployeesCountUntilFounded = a.Sum(b => b.EmployeesCountUntilFounded),
					GraphYears = a.Sum(b => b.GraphYears),
					PlanGrap = a.FirstOrDefault(c => c.ContractorId == a.Key.ContractorId)?.PlanGrap ?? Enumerable.Empty<PrtnEmploymentGraphReportItemsDto>(),
					ByReport = a.FirstOrDefault(c => c.ContractorId == a.Key.ContractorId)?.ByReport ?? Enumerable.Empty<PrtnEmploymentGraphReportItemsDto>(),
				})
				.ToList();
			}
			else
			{
				res.Rows = contractor
				.Select(a => new PrtnEmploymentGraphReportRowsDto
				{
					RegionId = a.Applications.FirstOrDefault() != null && a.Applications.FirstOrDefault().PrtnApplication.ChooseLocation
										? a.Applications.FirstOrDefault().PrtnApplication.ChoosedRegionId
										: a.RegionId,
					RegionOrderCode = a.Applications.FirstOrDefault() != null && a.Applications.FirstOrDefault().PrtnApplication.ChooseLocation
										? a.Applications.FirstOrDefault().PrtnApplication.ChoosedRegion.OrderCode
										: a.Region.OrderCode,
					Region = a.Applications.FirstOrDefault() != null && a.Applications.FirstOrDefault().PrtnApplication.ChooseLocation
										? (a.Applications.FirstOrDefault().PrtnApplication.ChoosedRegion.Translates.AsQueryable()
												.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
												?? a.Applications.FirstOrDefault().PrtnApplication.ChoosedRegion.FullName)
										: (a.Region.Translates.AsQueryable()
												.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
												?? a.Region.FullName),

					DistrictId = a.Applications.FirstOrDefault() != null && a.Applications.FirstOrDefault().PrtnApplication.ChooseLocation
										? a.Applications.FirstOrDefault().PrtnApplication.ChoosedDistrictId
										: a.DistrictId,
					District = a.Applications.FirstOrDefault() != null && a.Applications.FirstOrDefault().PrtnApplication.ChooseLocation
										? (a.Applications.FirstOrDefault().PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
												.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
												?? a.Applications.FirstOrDefault().PrtnApplication.ChoosedDistrict.FullName)
										: (a.District.Translates.AsQueryable()
												.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
												?? a.District.FullName),

					ContractorName = a.FullName,
					ContractorId = a.Id,
					ContractorInn = a.Inn,

					MfyId = a.Applications.FirstOrDefault() != null ? a.Applications.FirstOrDefault().PrtnApplication.MfyId : 0,
					Mfy = a.Applications.FirstOrDefault() != null ? a.Applications.FirstOrDefault().PrtnApplication.Mfy.FullName : "",

					EmployeesCountUntilFounded = untilFounded.ContainsKey(a.Inn)
						? (long)untilFounded[a.Inn]
						: 0,

					GraphYears = a.Applications.SelectMany(app => app.PrtnApplication.Graphs)
						.Where(b => b.YearIn == CommonConst._2023 || b.YearIn == CommonConst._2024 ||
									b.YearIn == CommonConst._2025 || b.YearIn == CommonConst._2026)
						.Sum(b => b.NewVacanciesCount),

					PlanGrap = a.Applications.SelectMany(app => app.PrtnApplication.Graphs)
						.Where(graph => !filter.YearIn.HasValue || graph.YearIn == filter.YearIn.Value)
						.GroupBy(graph => new { graph.YearIn, graph.MonthIn })
						.Select(graph => new PrtnEmploymentGraphReportItemsDto()
						{
							YearIn = graph.Key.YearIn,
							MonthIn = graph.Key.MonthIn,
							ParamSumm = graph.Sum(b => b.NewVacanciesCount)
						}),

					ByReport = monthly
						.Where(tax => tax.Tin == a.Inn)
						.AsEnumerable()
						.GroupBy(tax => new { tax.Year, tax.Month })
						.Select(tax => new PrtnEmploymentGraphReportItemsDto()
						{
							YearIn = tax.Key.Year,
							MonthIn = tax.Key.Month,
							ParamSumm = (long)tax.Sum(b => b.MonthlyNumberEmployees)
						}),

					RowsMonthly = new Dictionary<int, (long PlanGraphCount, long DifferenceGraphAndReportCount, long DifferenceCount)>(),
				})
				.Skip((filter.Page - 1) * filter.PageSize)
				.Take(filter.PageSize)
				.AsEnumerable()
				.GroupBy(a => new
				{
					a.RegionId,
					a.Region,
					a.RegionOrderCode,
					a.DistrictId,
					a.District,
					a.MfyId,
					a.Mfy,
					a.ContractorId,
					a.ContractorName,
					a.ContractorInn,
				})
				.OrderByDescending(a => a.Key.ContractorInn)
				.Select(a => new PrtnEmploymentGraphReportRowsDto
				{
					RegionId = a.Key.RegionId,
					Region = a.Key.Region,
					RegionOrderCode = a.Key.RegionOrderCode,
					DistrictId = a.Key.DistrictId,
					District = a.Key.District,
					MfyId = a.Key.MfyId,
					Mfy = a.Key.Mfy,
					ContractorId = a.Key.ContractorId,
					ContractorName = a.Key.ContractorName,
					ContractorInn = a.Key.ContractorInn,
					EmployeesCountUntilFounded = a.Sum(b => b.EmployeesCountUntilFounded),
					GraphYears = a.Sum(b => b.GraphYears),
					PlanGrap = a.FirstOrDefault(c => c.ContractorId == a.Key.ContractorId)?.PlanGrap ?? Enumerable.Empty<PrtnEmploymentGraphReportItemsDto>(),
					ByReport = a.FirstOrDefault(c => c.ContractorId == a.Key.ContractorId)?.ByReport ?? Enumerable.Empty<PrtnEmploymentGraphReportItemsDto>(),
				})
				.ToList();
			}


			foreach (var rows in res.Rows)
			{
				var taxDict = rows.ByReport
					.GroupBy(a => a.MonthIn)
					.ToDictionary(g => g.Key, g => g.Sum(a => a.ParamSumm));

				var planDict = rows.PlanGrap
					.GroupBy(a => a.MonthIn)
					.ToDictionary(g => g.Key, g => g.Sum(a => a.ParamSumm));

				long totalTax = 0, totalDifference = 0;
				for (int iterator = (filter.YearIn.HasValue && filter.YearIn.Value == CommonConst._2023)
						? CommonConst.IYUN
						: CommonConst._1;
					iterator <= CommonConst._12; iterator++)
				{
					long diffReport = 0, diff = 0;
					long planSumma = planDict.TryGetValue(iterator, out var plan) ? plan : 0;
					long before1month = taxDict.TryGetValue(iterator - 1, out var before1m) ? before1m : 0;
					if (taxDict.TryGetValue(iterator, out var item))
					{
						long taxSum = taxDict[iterator];
						if (taxSum != 0)
						{
							diffReport = (taxSum - before1month);
							diff = (taxSum - before1month - planSumma);
						}
					}

					// Replace negative values with 0
					if (planSumma < 0)
						planSumma = 0;

					if (diffReport < 0)
						diffReport = 0;

					if (diff < 0)
						diff = 0;

					rows.RowsMonthly.Add(iterator, (planSumma, diffReport, diff));

					totalTax += diffReport;
					totalDifference += diff;
				}
				rows.TaxYears += totalTax;

				rows.Years = (rows.GraphYears, rows.TaxYears, rows.GraphYears - rows.TaxYears);
				rows.RowsTotal = (rows.PlanGrap.Sum(a => a.ParamSumm), totalTax, totalDifference);
			}

			for (int iterator = (filter.YearIn.HasValue && filter.YearIn.Value == CommonConst._2023)
					? CommonConst.IYUN
					: CommonConst._1;
				iterator <= CommonConst._12; iterator++)
			{
				res.TotalMonthly.Add(iterator,
				(
					res.Rows.Sum(a => a.RowsMonthly[iterator].PlanGraphCount),
					res.Rows.Sum(a => a.RowsMonthly[iterator].DifferenceGraphAndReportCount),
					res.Rows.Sum(a => a.RowsMonthly[iterator].DifferenceCount)
				));
			}

			res.TotalYears =
			(
				res.Rows.Sum(a => a.GraphYears),
				res.Rows.Sum(a => a.TaxYears),
				(res.Rows.Sum(a => a.GraphYears) - res.Rows.Sum(a => a.TaxYears))
			);

			res.Total =
			(
				res.TotalMonthly.Sum(a => a.Value.TotalPlanGraphCount),
				res.TotalMonthly.Sum(a => a.Value.TotalDifferenceGraphAndReportCount),
				res.TotalMonthly.Sum(a => a.Value.TotalDifferenceCount)
			);

			res.TotalEmployeesCountUntilFounded = res.Rows.Sum(a => a.EmployeesCountUntilFounded);

			return res;
		}
		public PrtnCertificateByContractDto GetPrtnCertificateByContract(PrtnCertificateByContractDtoFilter filter)
		{
			var result = new PrtnCertificateByContractDto();

			var certificateBank = _unitOfWork.Context.Set<PrtnCertificate>()
						.Include(a => a.Contractor)
						.ThenInclude(b => b.Bank)
						.Select(c => (int?)c.Contractor.BankId)
						.ToList();

			result.Columns = _unitOfWork.PrtnContractTypeRepository.AllAsQueryable
						.Include(a => a.Translates)
						.Where(p => !filter.PrtnContractTypeId.HasValue || p.Id == filter.PrtnContractTypeId.Value)
						.IsActive()
						.Select(a => new
						{
							Id = a.Id,
							FullName = a.Translates.AsQueryable().FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
						})
						.ToList()
						.ToDictionary(
							a => a.Id,
							a => a.FullName
						);

			var query = _unitOfWork.Context.Set<PrtnCertificate>()
						.Include(a => a.Contractor)
						.Include(a => a.PrtnContract).ThenInclude(b => b.Application).ThenInclude(c => c.PrtnApplication)
						.Where(a => a.StatusId == StatusIdConst.FORMED
						&& (filter.StartDate.HasValue ? a.DocOn >= filter.StartDate.Value : true)
						&& (filter.EndDate.HasValue ? a.DocOn >= filter.EndDate.Value : true))
						.Where(a => !filter.PrtnContractTypeId.HasValue || filter.PrtnContractTypeId == a.PrtnContract.Application.PrtnApplication.PrtnContractTypeId);

			query = query.Where(a => (!filter.RegionId.HasValue || filter.RegionId == (a.PrtnContract.Application.PrtnApplication.ChooseLocation
																							? a.PrtnContract.Application.PrtnApplication.ChoosedRegionId : a.Contractor.RegionId))
						&& (!filter.DistrictId.HasValue || filter.DistrictId == (a.PrtnContract.Application.PrtnApplication.ChooseLocation
																					? a.PrtnContract.Application.PrtnApplication.ChoosedDistrictId : a.Contractor.DistrictId))
					   && (!filter.MfyId.HasValue || filter.MfyId == a.PrtnContract.Application.PrtnApplication.MfyId)
					   && (!filter.ContractorId.HasValue || filter.ContractorId == a.ContractorId));

			result.Rows = query
					.Select(a => new PrtnCertificateByContractItemDto
					{
						RegionId = filter.ByRegion ? (a.PrtnContract.Application.PrtnApplication.ChooseLocation ? a.PrtnContract.Application.PrtnApplication.ChoosedRegionId : a.Contractor.RegionId) : null,
						RegionOrderCode = filter.ByRegion ? (a.PrtnContract.Application.PrtnApplication.ChooseLocation ? a.PrtnContract.Application.PrtnApplication.ChoosedRegion.OrderCode : a.Contractor.Region.OrderCode) : null,
						Region = filter.ByRegion
									? (a.PrtnContract.Application.PrtnApplication.ChooseLocation ? (a.PrtnContract.Application.PrtnApplication.ChoosedRegion.Translates.AsQueryable()
										.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnContract.Application.PrtnApplication.ChoosedRegion.FullName)
									: (a.Contractor.Region.Translates.AsQueryable()
										.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Contractor.Region.FullName))
									: null,
						DistrictId = filter.ByDistrict ? (a.PrtnContract.Application.PrtnApplication.ChooseLocation ? a.PrtnContract.Application.PrtnApplication.ChoosedDistrictId : a.Contractor.DistrictId) : null,
						District = filter.ByDistrict
										? (a.PrtnContract.Application.PrtnApplication.ChooseLocation ? (a.PrtnContract.Application.PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
											.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnContract.Application.PrtnApplication.ChoosedDistrict.FullName)
										: (a.Contractor.District.Translates.AsQueryable()
											.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Contractor.District.FullName))
										: null,
						Contractor = filter.ByContractor ? a.Contractor.FullName : null,
						ContractorId = filter.ByContractor ? a.Contractor.Id : null,
						ContractorInn = filter.ByContractor ? a.ContractorInn : null,
						MfyId = filter.ByMfy ? a.PrtnContract.Application.PrtnApplication.MfyId : null,
						Mfy = filter.ByMfy ? a.PrtnContract.Application.PrtnApplication.Mfy.FullName : null,
						BankId = a.Contractor.BankId,
						PrtnContractTypeId = a.PrtnContractTypeId,
						CertificateCount = a.StatusId == StatusIdConst.FORMED ? 1 : 0,
						CertificateNewVacanciesCount = a.StatusId == StatusIdConst.FORMED ? a.PrtnContract.Application.PrtnApplication.NewVacanciesCount : 0,
						CountCertificatesWithVacancies = new Dictionary<int, (long Count, long NewVacanciesCount)>()
					})
					.AsEnumerable()
					.ToList()
					.GroupBy(a => new
					{
						a.RegionId,
						a.Region,
						a.RegionOrderCode,
						a.DistrictId,
						a.District,
						a.MfyId,
						a.Mfy,
						a.ContractorId,
						a.Contractor,
						a.ContractorInn
					})
					.Select(a => new PrtnCertificateByContractItemDto
					{
						RegionId = a.Key.RegionId,
						Region = a.Key.Region,
						RegionOrderCode = a.Key.RegionOrderCode,
						DistrictId = a.Key.DistrictId,
						District = a.Key.District,
						MfyId = a.Key.MfyId,
						Mfy = a.Key.Mfy,
						ContractorId = a.Key.ContractorId,
						Contractor = a.Key.Contractor,
						ContractorInn = a.Key.ContractorInn,
						TotalCertificateWithVacancies = (a.Sum(c => c.CertificateCount), a.Sum(c => c.CertificateNewVacanciesCount)),
						CountCertificatesWithVacancies = a.GroupBy(b => b.PrtnContractTypeId).ToDictionary(
							b => b.Key,
							b => (b.Sum(c => c.CertificateCount), b.Sum(c => c.CertificateNewVacanciesCount))
							),
					}).ToList();


			foreach (var row in result.Rows)
			{
				if (row.CountCertificatesWithVacancies.Count() != result.Columns.Count)
				{
					foreach (var column in result.Columns)
					{
						if (!row.CountCertificatesWithVacancies.ContainsKey(column.Key))
							row.CountCertificatesWithVacancies.Add(column.Key, (0, 0));
					}
				}
			}

			//Hamma viloyatlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
			if (filter.ByRegion)
			{
				var regions = _unitOfWork.RegionRepository.AllAsQueryable
							.Include(a => a.Translates)
							.IsActive()
							.Where(a => (!filter.RegionId.HasValue || filter.RegionId == a.Id) && (result.Rows.Count() == 0 || !result.Rows.Select(a => a.RegionId).Contains(a.Id)))
							.ToDictionary
							(
								a => a.Id,
								a => new
								{
									OrderCode = a.OrderCode,
									FullName = a.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.FullName,
									RegionSoato = a.Soato
								}
							);
				foreach (var region in regions)
				{
					result.Rows.Add(new PrtnCertificateByContractItemDto
					{
						Region = region.Value.FullName,
						RegionId = region.Key,
						RegionOrderCode = region.Value.OrderCode,
						CountCertificatesWithVacancies = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long)0, (long)0)
							)
					});
				}
				result.Rows = result.Rows.OrderBy(a => a.RegionOrderCode).ThenBy(a => a.District).ToList();
			}

			// Viloyatdagi hamma tumanlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
			if (filter.ByDistrict)
			{
				var districts = _unitOfWork.DistrictRepository.AllAsQueryable
					   .Include(a => a.Region).ThenInclude(a => a.Translates)
					   .Include(a => a.Translates)
					   .IsActive()
					   .Where(a => (!filter.RegionId.HasValue || filter.RegionId == a.RegionId) && (!filter.DistrictId.HasValue || filter.DistrictId == a.Id)
					   )
					   .ToDictionary(
					   a => a.Id,
					   a => new
					   {
						   OrderCode = a.OrderCode,
						   RegionId = a.RegionId,
						   Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.Region.FullName,
						   District = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.FullName,
					   });

				foreach (var district in districts)
				{
					if (result.Rows.Select(a => a.DistrictId).Contains(district.Key))
					{
						result.Rows.Where(a => a.DistrictId == district.Key).ToList().ForEach(a =>
						{
							a.Region = district.Value.Region;
							a.District = district.Value.District;
						});
						continue;
					}
					result.Rows.Add(new PrtnCertificateByContractItemDto
					{
						RegionOrderCode = district.Value.OrderCode,
						Region = district.Value.Region,
						RegionId = district.Value.RegionId,
						District = district.Value.District,
						DistrictId = district.Key,
						CountCertificatesWithVacancies = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long)0, (long)0)
							)
					});
				}
				result.Rows = result.Rows.OrderBy(a => a.RegionOrderCode).ToList();
			}

			// MFY boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
			if (filter.ByMfy)
			{
				var mfys = _unitOfWork.Context.Set<Mfy>()
				 .Include(a => a.Region).ThenInclude(a => a.Translates)
				 .Include(a => a.District).ThenInclude(a => a.Translates)
				 .IsActive()
				 .Where(a => (!filter.RegionId.HasValue || filter.RegionId == a.RegionId)
				 && (!filter.DistrictId.HasValue || filter.DistrictId == a.DistrictId)
				 && (!filter.MfyId.HasValue || filter.MfyId == a.Id))
				 .ToList()
				 .ToDictionary(
					 a => a.Id,
					 a => new
					 {
						 RegionId = a.RegionId,
						 Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.Region.FullName,
						 DistrictId = a.DistrictId,
						 District = a.District.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.District.FullName,
						 MfyId = a.Id,
						 Mfy = a.FullName,
					 }
				 );
				foreach (var mfy in mfys)
				{
					if (result.Rows.Select(a => a.MfyId).Contains(mfy.Key))
					{
						result.Rows.Where(a => a.MfyId == mfy.Key).ToList().ForEach(a =>
						{
							a.Region = mfy.Value.Region;
							a.District = mfy.Value.District;
							a.Mfy = mfy.Value.Mfy;
						});
						continue;
					}
					result.Rows.Add(new PrtnCertificateByContractItemDto
					{
						Region = mfy.Value.Region,
						RegionId = mfy.Value.RegionId,
						District = mfy.Value.District,
						DistrictId = mfy.Value.DistrictId,
						Mfy = mfy.Value.Mfy,
						MfyId = mfy.Key,
						CountCertificatesWithVacancies = result.Columns.ToDictionary(
							a => a.Key,
							a => ((long)0, (long)0)
							)
					});
				}
				result.Rows = result.Rows.OrderBy(a => a.RegionOrderCode).ThenBy(a => a.District).ThenBy(a => a.Mfy).ToList();
			}

			foreach (var column in result.Columns.Keys)
			{
				long totalCount = result.Rows.Sum(a => a.CountCertificatesWithVacancies[column].Count);
				long totalNewVacanciesCount = result.Rows.Sum(a => a.CountCertificatesWithVacancies[column].NewVacanciesCount);
				result.CertificateColumnTotals.Add(column, (totalCount, totalNewVacanciesCount));
			}
			result.CertificateTotalsWithVacancies = (result.CertificateColumnTotals.Sum(a => a.Value.TotalCount), result.CertificateColumnTotals.Sum(a => a.Value.TotalNewVacanciesCount));
			return result;
		}
		public List<PrtnCreditDemandInfoByBankDto> GetPrtnCreditDemandInfoByBank(PrtnCreditDemandInfoByBankDtoFilter options)
		{
			var languageId = _cultureHelper.CurrentCulture.Id;
			var result = _unitOfWork.Context.GetPrtnCreditDemandInfoByBank(
				options.ContractorId,
				options.ContractorInn,
				options.RegionId,
				options.DistrictId,
				options.ByRegion,
				options.ByDistrict,
				options.ByContractor,
				options.BankId,
				options.ByBank,
				options.MainBankId,
				options.ByMainBank,
				options.ContractTypeId,
				options.ByContractType,
				languageId
				).ToList();

			return result;
		}
		public Stream PrtnApplicationByContractTypeExcel(PrtnApplicationByContractTypeDtoFilter dto)
		{
			var data = GetPrtnApplicationByContractType(dto);

			MemoryStream result = new MemoryStream();
			MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.PRNT_APPLICATION_BY_CONTRACT_TYPE));

			if (IsValid && data != null)
			{
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				ExcelPackage excelPackage = new ExcelPackage(template);

				var namerange = excelPackage.Workbook.Names["OnDate"];
				if (_cultureHelper.CurrentCulture.Id == LanguageIdConst.UZ_CYRL)
					namerange.Value = DateTime.Now.ToString("dd.MM.yyyy") + " й. " + DateTime.Now.ToString("HH:mm") + " ҳолатига ";
				else if (_cultureHelper.CurrentCulture.Id == LanguageIdConst.UZ_LATN)
					namerange.Value = DateTime.Now.ToString("dd.MM.yyyy") + " y. " + DateTime.Now.ToString("HH:mm") + " holatiga ";
				else
					namerange.Value = DateTime.Now.ToString("dd.MM.yyyy") + " г. " + DateTime.Now.ToString("HH: mm");
				var ws = namerange.Worksheet;

				#region Initialize columns
				var importApplicationColumns = excelPackage.Workbook.Names["ApplicationColumns"];
				foreach (var applicationColumn in data.Columns.OrderByDescending(a => a.Key))
				{
					int currentColumn = importApplicationColumns.Start.Column + 2;
					ws.InsertColumn(currentColumn, 2);
					ws.Cells[importApplicationColumns.Start.Row + 1, currentColumn, importApplicationColumns.Start.Row + 1, currentColumn + 1].Merge = true;
					ws.Cells[importApplicationColumns.Start.Row + 1, currentColumn].Value = applicationColumn.Value;
					ws.Cells[importApplicationColumns.Start.Row + 1, currentColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[importApplicationColumns.Start.Row + 1, currentColumn].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[importApplicationColumns.Start.Row + 1, currentColumn, importApplicationColumns.Start.Row + 1, currentColumn + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[importApplicationColumns.Start.Row + 1, currentColumn, importApplicationColumns.Start.Row + 1, currentColumn + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[importApplicationColumns.Start.Row + 1, currentColumn, importApplicationColumns.Start.Row + 1, currentColumn + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[importApplicationColumns.Start.Row + 1, currentColumn, importApplicationColumns.Start.Row + 1, currentColumn + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[importApplicationColumns.Start.Row + 1, currentColumn].Style.Font.Bold = true;
					ws.Cells[importApplicationColumns.Start.Row + 1, currentColumn].Style.Font.Size = 10;
					ws.Cells[importApplicationColumns.Start.Row + 1, currentColumn].Style.WrapText = true;
					//soni
					ws.Cells[importApplicationColumns.Start.Row + 2, currentColumn].Value = ws.Cells[importApplicationColumns.Start.Row + 2, importApplicationColumns.Start.Column].Value;
					ws.Cells[importApplicationColumns.Start.Row + 2, currentColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[importApplicationColumns.Start.Row + 2, currentColumn].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[importApplicationColumns.Start.Row + 2, currentColumn, importApplicationColumns.Start.Row + 2, currentColumn + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[importApplicationColumns.Start.Row + 2, currentColumn, importApplicationColumns.Start.Row + 2, currentColumn + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[importApplicationColumns.Start.Row + 2, currentColumn, importApplicationColumns.Start.Row + 2, currentColumn + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[importApplicationColumns.Start.Row + 2, currentColumn, importApplicationColumns.Start.Row + 2, currentColumn + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[importApplicationColumns.Start.Row + 2, currentColumn].Style.Font.Bold = true;
					ws.Cells[importApplicationColumns.Start.Row + 2, currentColumn].Style.Font.Size = 10;
					// ish o'rni
					ws.Cells[importApplicationColumns.Start.Row + 2, currentColumn + 1].Value = ws.Cells[importApplicationColumns.Start.Row + 2, importApplicationColumns.Start.Column + 1].Value;
					ws.Cells[importApplicationColumns.Start.Row + 2, currentColumn + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[importApplicationColumns.Start.Row + 2, currentColumn + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[importApplicationColumns.Start.Row + 2, currentColumn, importApplicationColumns.Start.Row + 2, currentColumn + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[importApplicationColumns.Start.Row + 2, currentColumn, importApplicationColumns.Start.Row + 2, currentColumn + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[importApplicationColumns.Start.Row + 2, currentColumn, importApplicationColumns.Start.Row + 2, currentColumn + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[importApplicationColumns.Start.Row + 2, currentColumn, importApplicationColumns.Start.Row + 2, currentColumn + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[importApplicationColumns.Start.Row + 2, currentColumn + 1].Style.Font.Bold = true;
					ws.Cells[importApplicationColumns.Start.Row + 2, currentColumn + 1].Style.Font.Size = 10;
				}

				if (_cultureHelper.CurrentCulture.Id == LanguageIdConst.UZ_CYRL)
					ws.Cells[importApplicationColumns.Start.Row, importApplicationColumns.Start.Column + 2].Value = "шундан";
				else if (_cultureHelper.CurrentCulture.Id == LanguageIdConst.UZ_LATN)
					ws.Cells[importApplicationColumns.Start.Row, importApplicationColumns.Start.Column + 2].Value = "shundan";
				else
					ws.Cells[importApplicationColumns.Start.Row, importApplicationColumns.Start.Column + 2].Value = "от этого";
				ws.Cells[importApplicationColumns.Start.Row, importApplicationColumns.Start.Column + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				ws.Cells[importApplicationColumns.Start.Row, importApplicationColumns.Start.Column + 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				ws.Cells[importApplicationColumns.Start.Row, importApplicationColumns.Start.Column + 2].Style.Font.Bold = true;
				ws.Cells[importApplicationColumns.Start.Row, importApplicationColumns.Start.Column + 2].Style.Font.Size = 10;
				ws.Cells[importApplicationColumns.Start.Row, importApplicationColumns.Start.Column + 2, importApplicationColumns.Start.Row, importApplicationColumns.Start.Column + 1 + (data.Columns.Count * 2)].Merge = true;
				ws.Cells[importApplicationColumns.Start.Row, importApplicationColumns.Start.Column + 2, importApplicationColumns.Start.Row, importApplicationColumns.Start.Column + 1 + (data.Columns.Count * 2)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells[importApplicationColumns.Start.Row, importApplicationColumns.Start.Column + 2, importApplicationColumns.Start.Row, importApplicationColumns.Start.Column + 1 + (data.Columns.Count * 2)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				ws.Cells[importApplicationColumns.Start.Row, importApplicationColumns.Start.Column + 2, importApplicationColumns.Start.Row, importApplicationColumns.Start.Column + 1 + (data.Columns.Count * 2)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				ws.Cells[importApplicationColumns.Start.Row, importApplicationColumns.Start.Column + 2, importApplicationColumns.Start.Row, importApplicationColumns.Start.Column + 1 + (data.Columns.Count * 2)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

				var importContractorCreateColumns = excelPackage.Workbook.Names["ContractorCreateColumns"];
				foreach (var contractorCreateColumn in data.Columns.OrderByDescending(a => a.Key))
				{
					int currentColumn = importContractorCreateColumns.Start.Column + 2;
					ws.InsertColumn(currentColumn, 2);
					ws.Cells[importContractorCreateColumns.Start.Row + 1, currentColumn, importContractorCreateColumns.Start.Row + 1, currentColumn + 1].Merge = true;
					ws.Cells[importContractorCreateColumns.Start.Row + 1, currentColumn].Value = contractorCreateColumn.Value;
					ws.Cells[importContractorCreateColumns.Start.Row + 1, currentColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[importContractorCreateColumns.Start.Row + 1, currentColumn].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[importContractorCreateColumns.Start.Row + 1, currentColumn, importContractorCreateColumns.Start.Row + 1, currentColumn + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorCreateColumns.Start.Row + 1, currentColumn, importContractorCreateColumns.Start.Row + 1, currentColumn + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorCreateColumns.Start.Row + 1, currentColumn, importContractorCreateColumns.Start.Row + 1, currentColumn + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorCreateColumns.Start.Row + 1, currentColumn, importContractorCreateColumns.Start.Row + 1, currentColumn + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorCreateColumns.Start.Row + 1, currentColumn].Style.Font.Bold = true;
					ws.Cells[importContractorCreateColumns.Start.Row + 1, currentColumn].Style.Font.Size = 10;
					ws.Cells[importContractorCreateColumns.Start.Row + 1, currentColumn].Style.WrapText = true;
					//soni
					ws.Cells[importContractorCreateColumns.Start.Row + 2, currentColumn].Value = ws.Cells[importContractorCreateColumns.Start.Row + 2, importContractorCreateColumns.Start.Column].Value;
					ws.Cells[importContractorCreateColumns.Start.Row + 2, currentColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[importContractorCreateColumns.Start.Row + 2, currentColumn].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[importContractorCreateColumns.Start.Row + 2, currentColumn, importContractorCreateColumns.Start.Row + 2, currentColumn + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorCreateColumns.Start.Row + 2, currentColumn, importContractorCreateColumns.Start.Row + 2, currentColumn + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorCreateColumns.Start.Row + 2, currentColumn, importContractorCreateColumns.Start.Row + 2, currentColumn + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorCreateColumns.Start.Row + 2, currentColumn, importContractorCreateColumns.Start.Row + 2, currentColumn + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorCreateColumns.Start.Row + 2, currentColumn].Style.Font.Bold = true;
					ws.Cells[importContractorCreateColumns.Start.Row + 2, currentColumn].Style.Font.Size = 10;
					// ish o'rni
					ws.Cells[importContractorCreateColumns.Start.Row + 2, currentColumn + 1].Value = ws.Cells[importContractorCreateColumns.Start.Row + 2, importContractorCreateColumns.Start.Column + 1].Value;
					ws.Cells[importContractorCreateColumns.Start.Row + 2, currentColumn + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[importContractorCreateColumns.Start.Row + 2, currentColumn + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[importContractorCreateColumns.Start.Row + 2, currentColumn, importContractorCreateColumns.Start.Row + 2, currentColumn + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorCreateColumns.Start.Row + 2, currentColumn, importContractorCreateColumns.Start.Row + 2, currentColumn + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorCreateColumns.Start.Row + 2, currentColumn, importContractorCreateColumns.Start.Row + 2, currentColumn + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorCreateColumns.Start.Row + 2, currentColumn, importContractorCreateColumns.Start.Row + 2, currentColumn + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorCreateColumns.Start.Row + 2, currentColumn + 1].Style.Font.Bold = true;
					ws.Cells[importContractorCreateColumns.Start.Row + 2, currentColumn + 1].Style.Font.Size = 10;
				}

				if (_cultureHelper.CurrentCulture.Id == LanguageIdConst.UZ_CYRL)
					ws.Cells[importContractorCreateColumns.Start.Row, importContractorCreateColumns.Start.Column + 2].Value = "шундан";
				else if (_cultureHelper.CurrentCulture.Id == LanguageIdConst.UZ_LATN)
					ws.Cells[importContractorCreateColumns.Start.Row, importContractorCreateColumns.Start.Column + 2].Value = "shundan";
				else
					ws.Cells[importContractorCreateColumns.Start.Row, importContractorCreateColumns.Start.Column + 2].Value = "от этого";
				ws.Cells[importContractorCreateColumns.Start.Row, importContractorCreateColumns.Start.Column + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				ws.Cells[importContractorCreateColumns.Start.Row, importContractorCreateColumns.Start.Column + 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				ws.Cells[importContractorCreateColumns.Start.Row, importContractorCreateColumns.Start.Column + 2].Style.Font.Bold = true;
				ws.Cells[importContractorCreateColumns.Start.Row, importContractorCreateColumns.Start.Column + 2].Style.Font.Size = 10;
				ws.Cells[importContractorCreateColumns.Start.Row, importContractorCreateColumns.Start.Column + 2, importContractorCreateColumns.Start.Row, importContractorCreateColumns.Start.Column + 1 + (data.Columns.Count * 2)].Merge = true;
				ws.Cells[importContractorCreateColumns.Start.Row, importContractorCreateColumns.Start.Column + 2, importContractorCreateColumns.Start.Row, importContractorCreateColumns.Start.Column + 1 + (data.Columns.Count * 2)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells[importContractorCreateColumns.Start.Row, importContractorCreateColumns.Start.Column + 2, importContractorCreateColumns.Start.Row, importContractorCreateColumns.Start.Column + 1 + (data.Columns.Count * 2)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				ws.Cells[importContractorCreateColumns.Start.Row, importContractorCreateColumns.Start.Column + 2, importContractorCreateColumns.Start.Row, importContractorCreateColumns.Start.Column + 1 + (data.Columns.Count * 2)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				ws.Cells[importContractorCreateColumns.Start.Row, importContractorCreateColumns.Start.Column + 2, importContractorCreateColumns.Start.Row, importContractorCreateColumns.Start.Column + 1 + (data.Columns.Count * 2)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

				var importContractColumns = excelPackage.Workbook.Names["ContractColumns"];

				foreach (var contractColumns in data.Columns.OrderByDescending(a => a.Key))
				{
					int currentColumn = importContractColumns.Start.Column + 2;
					ws.InsertColumn(currentColumn, 2);
					ws.Cells[importContractColumns.Start.Row + 1, currentColumn, importContractColumns.Start.Row + 1, currentColumn + 1].Merge = true;
					ws.Cells[importContractColumns.Start.Row + 1, currentColumn].Value = contractColumns.Value;
					ws.Cells[importContractColumns.Start.Row + 1, currentColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[importContractColumns.Start.Row + 1, currentColumn].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[importContractColumns.Start.Row + 1, currentColumn, importContractColumns.Start.Row + 1, currentColumn + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractColumns.Start.Row + 1, currentColumn, importContractColumns.Start.Row + 1, currentColumn + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractColumns.Start.Row + 1, currentColumn, importContractColumns.Start.Row + 1, currentColumn + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractColumns.Start.Row + 1, currentColumn, importContractColumns.Start.Row + 1, currentColumn + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractColumns.Start.Row + 1, currentColumn].Style.Font.Bold = true;
					ws.Cells[importContractColumns.Start.Row + 1, currentColumn].Style.Font.Size = 10;
					ws.Cells[importContractColumns.Start.Row + 1, currentColumn].Style.WrapText = true;

					//soni
					ws.Cells[importContractColumns.Start.Row + 2, currentColumn].Value = ws.Cells[importContractColumns.Start.Row + 2, importContractColumns.Start.Column].Value;
					ws.Cells[importContractColumns.Start.Row + 2, currentColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[importContractColumns.Start.Row + 2, currentColumn].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[importContractColumns.Start.Row + 2, currentColumn, importContractColumns.Start.Row + 2, currentColumn + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractColumns.Start.Row + 2, currentColumn, importContractColumns.Start.Row + 2, currentColumn + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractColumns.Start.Row + 2, currentColumn, importContractColumns.Start.Row + 2, currentColumn + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractColumns.Start.Row + 2, currentColumn, importContractColumns.Start.Row + 2, currentColumn + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractColumns.Start.Row + 2, currentColumn].Style.Font.Bold = true;
					ws.Cells[importContractColumns.Start.Row + 2, currentColumn].Style.Font.Size = 10;
					// ish o'rni
					ws.Cells[importContractColumns.Start.Row + 2, currentColumn + 1].Value = ws.Cells[importContractColumns.Start.Row + 2, importContractColumns.Start.Column + 1].Value;
					ws.Cells[importContractColumns.Start.Row + 2, currentColumn + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[importContractColumns.Start.Row + 2, currentColumn + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[importContractColumns.Start.Row + 2, currentColumn, importContractColumns.Start.Row + 2, currentColumn + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractColumns.Start.Row + 2, currentColumn, importContractColumns.Start.Row + 2, currentColumn + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractColumns.Start.Row + 2, currentColumn, importContractColumns.Start.Row + 2, currentColumn + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractColumns.Start.Row + 2, currentColumn, importContractColumns.Start.Row + 2, currentColumn + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractColumns.Start.Row + 2, currentColumn + 1].Style.Font.Bold = true;
					ws.Cells[importContractColumns.Start.Row + 2, currentColumn + 1].Style.Font.Size = 10;
				}

				if (_cultureHelper.CurrentCulture.Id == LanguageIdConst.UZ_CYRL)
					ws.Cells[importContractColumns.Start.Row, importContractColumns.Start.Column + 2].Value = "шундан";
				else if (_cultureHelper.CurrentCulture.Id == LanguageIdConst.UZ_LATN)
					ws.Cells[importContractColumns.Start.Row, importContractColumns.Start.Column + 2].Value = "shundan";
				else
					ws.Cells[importContractColumns.Start.Row, importContractColumns.Start.Column + 2].Value = "от этого";

				ws.Cells[importContractColumns.Start.Row, importContractColumns.Start.Column + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				ws.Cells[importContractColumns.Start.Row, importContractColumns.Start.Column + 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				ws.Cells[importContractColumns.Start.Row, importContractColumns.Start.Column + 2].Style.Font.Bold = true;
				ws.Cells[importContractColumns.Start.Row, importContractColumns.Start.Column + 2].Style.Font.Size = 10;
				ws.Cells[importContractColumns.Start.Row, importContractColumns.Start.Column + 2, importContractColumns.Start.Row, importContractColumns.Start.Column + 1 + (data.Columns.Count * 2)].Merge = true;
				ws.Cells[importContractColumns.Start.Row, importContractColumns.Start.Column + 2, importContractColumns.Start.Row, importContractColumns.Start.Column + 1 + (data.Columns.Count * 2)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells[importContractColumns.Start.Row, importContractColumns.Start.Column + 2, importContractColumns.Start.Row, importContractColumns.Start.Column + 1 + (data.Columns.Count * 2)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				ws.Cells[importContractColumns.Start.Row, importContractColumns.Start.Column + 2, importContractColumns.Start.Row, importContractColumns.Start.Column + 1 + (data.Columns.Count * 2)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				ws.Cells[importContractColumns.Start.Row, importContractColumns.Start.Column + 2, importContractColumns.Start.Row, importContractColumns.Start.Column + 1 + (data.Columns.Count * 2)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

				var importContractorSignedColumns = excelPackage.Workbook.Names["ContractorSignedColumns"];
				foreach (var contractorSignedColumn in data.Columns.OrderByDescending(a => a.Key))
				{
					int currentColumn = importContractorSignedColumns.Start.Column + 2;
					ws.InsertColumn(currentColumn, 2);
					ws.Cells[importContractorSignedColumns.Start.Row + 1, currentColumn, importContractorSignedColumns.Start.Row + 1, currentColumn + 1].Merge = true;
					ws.Cells[importContractorSignedColumns.Start.Row + 1, currentColumn].Value = contractorSignedColumn.Value;
					ws.Cells[importContractorSignedColumns.Start.Row + 1, currentColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[importContractorSignedColumns.Start.Row + 1, currentColumn].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[importContractorSignedColumns.Start.Row + 1, currentColumn, importContractorSignedColumns.Start.Row + 1, currentColumn + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorSignedColumns.Start.Row + 1, currentColumn, importContractorSignedColumns.Start.Row + 1, currentColumn + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorSignedColumns.Start.Row + 1, currentColumn, importContractorSignedColumns.Start.Row + 1, currentColumn + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorSignedColumns.Start.Row + 1, currentColumn, importContractorSignedColumns.Start.Row + 1, currentColumn + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorSignedColumns.Start.Row + 1, currentColumn].Style.Font.Bold = true;
					ws.Cells[importContractorSignedColumns.Start.Row + 1, currentColumn].Style.Font.Size = 10;
					ws.Cells[importContractorSignedColumns.Start.Row + 1, currentColumn].Style.WrapText = true;
					//soni
					ws.Cells[importContractorSignedColumns.Start.Row + 2, currentColumn].Value = ws.Cells[importContractorSignedColumns.Start.Row + 2, importContractorSignedColumns.Start.Column].Value;
					ws.Cells[importContractorSignedColumns.Start.Row + 2, currentColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[importContractorSignedColumns.Start.Row + 2, currentColumn].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[importContractorSignedColumns.Start.Row + 2, currentColumn, importContractorSignedColumns.Start.Row + 2, currentColumn + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorSignedColumns.Start.Row + 2, currentColumn, importContractorSignedColumns.Start.Row + 2, currentColumn + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorSignedColumns.Start.Row + 2, currentColumn, importContractorSignedColumns.Start.Row + 2, currentColumn + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorSignedColumns.Start.Row + 2, currentColumn, importContractorSignedColumns.Start.Row + 2, currentColumn + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorSignedColumns.Start.Row + 2, currentColumn].Style.Font.Bold = true;
					ws.Cells[importContractorSignedColumns.Start.Row + 2, currentColumn].Style.Font.Size = 10;
					// ish o'rni
					ws.Cells[importContractorSignedColumns.Start.Row + 2, currentColumn + 1].Value = ws.Cells[importContractorSignedColumns.Start.Row + 2, importContractorSignedColumns.Start.Column + 1].Value;
					ws.Cells[importContractorSignedColumns.Start.Row + 2, currentColumn + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[importContractorSignedColumns.Start.Row + 2, currentColumn + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[importContractorSignedColumns.Start.Row + 2, currentColumn, importContractorSignedColumns.Start.Row + 2, currentColumn + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorSignedColumns.Start.Row + 2, currentColumn, importContractorSignedColumns.Start.Row + 2, currentColumn + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorSignedColumns.Start.Row + 2, currentColumn, importContractorSignedColumns.Start.Row + 2, currentColumn + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorSignedColumns.Start.Row + 2, currentColumn, importContractorSignedColumns.Start.Row + 2, currentColumn + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[importContractorSignedColumns.Start.Row + 2, currentColumn + 1].Style.Font.Bold = true;
					ws.Cells[importContractorSignedColumns.Start.Row + 2, currentColumn + 1].Style.Font.Size = 10;
				}

				if (_cultureHelper.CurrentCulture.Id == LanguageIdConst.UZ_CYRL)
					ws.Cells[importContractorSignedColumns.Start.Row, importContractorSignedColumns.Start.Column + 2].Value = "шундан";
				else if (_cultureHelper.CurrentCulture.Id == LanguageIdConst.UZ_LATN)
					ws.Cells[importContractorSignedColumns.Start.Row, importContractorSignedColumns.Start.Column + 2].Value = "shundan";
				else
					ws.Cells[importContractorSignedColumns.Start.Row, importContractorSignedColumns.Start.Column + 2].Value = "от этого";
				ws.Cells[importContractorSignedColumns.Start.Row, importContractorSignedColumns.Start.Column + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				ws.Cells[importContractorSignedColumns.Start.Row, importContractorSignedColumns.Start.Column + 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				ws.Cells[importContractorSignedColumns.Start.Row, importContractorSignedColumns.Start.Column + 2].Style.Font.Bold = true;
				ws.Cells[importContractorSignedColumns.Start.Row, importContractorSignedColumns.Start.Column + 2].Style.Font.Size = 10;
				ws.Cells[importContractorSignedColumns.Start.Row, importContractorSignedColumns.Start.Column + 2, importContractorSignedColumns.Start.Row, importContractorSignedColumns.Start.Column + 1 + (data.Columns.Count * 2)].Merge = true;
				ws.Cells[importContractorSignedColumns.Start.Row, importContractorSignedColumns.Start.Column + 2, importContractorSignedColumns.Start.Row, importContractorSignedColumns.Start.Column + 1 + (data.Columns.Count * 2)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells[importContractorSignedColumns.Start.Row, importContractorSignedColumns.Start.Column + 2, importContractorSignedColumns.Start.Row, importContractorSignedColumns.Start.Column + 1 + (data.Columns.Count * 2)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				ws.Cells[importContractorSignedColumns.Start.Row, importContractorSignedColumns.Start.Column + 2, importContractorSignedColumns.Start.Row, importContractorSignedColumns.Start.Column + 1 + (data.Columns.Count * 2)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				ws.Cells[importContractorSignedColumns.Start.Row, importContractorSignedColumns.Start.Column + 2, importContractorSignedColumns.Start.Row, importContractorSignedColumns.Start.Column + 1 + (data.Columns.Count * 2)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


				var importCertificateColumns = excelPackage.Workbook.Names["CertificateColumns"];

				foreach (var certificateColumns in data.Columns.OrderByDescending(a => a.Key))
				{
					int currentColumn = importCertificateColumns.Start.Column + 2;
					ws.InsertColumn(currentColumn, 2);
					ws.Cells[importCertificateColumns.Start.Row + 1, currentColumn, importCertificateColumns.Start.Row + 1, currentColumn + 1].Merge = true;
					ws.Cells[importCertificateColumns.Start.Row + 1, currentColumn].Value = certificateColumns.Value;
					ws.Cells[importCertificateColumns.Start.Row + 1, currentColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[importCertificateColumns.Start.Row + 1, currentColumn].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[importCertificateColumns.Start.Row + 1, currentColumn, importCertificateColumns.Start.Row + 1, currentColumn + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[importCertificateColumns.Start.Row + 1, currentColumn, importCertificateColumns.Start.Row + 1, currentColumn + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[importCertificateColumns.Start.Row + 1, currentColumn, importCertificateColumns.Start.Row + 1, currentColumn + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[importCertificateColumns.Start.Row + 1, currentColumn, importCertificateColumns.Start.Row + 1, currentColumn + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[importCertificateColumns.Start.Row + 1, currentColumn].Style.Font.Bold = true;
					ws.Cells[importCertificateColumns.Start.Row + 1, currentColumn].Style.Font.Size = 10;
					ws.Cells[importCertificateColumns.Start.Row + 1, currentColumn].Style.WrapText = true;

					//soni
					ws.Cells[importCertificateColumns.Start.Row + 2, currentColumn].Value = ws.Cells[importCertificateColumns.Start.Row + 2, importCertificateColumns.Start.Column].Value;
					ws.Cells[importCertificateColumns.Start.Row + 2, currentColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[importCertificateColumns.Start.Row + 2, currentColumn].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[importCertificateColumns.Start.Row + 2, currentColumn, importCertificateColumns.Start.Row + 2, currentColumn + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[importCertificateColumns.Start.Row + 2, currentColumn, importCertificateColumns.Start.Row + 2, currentColumn + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[importCertificateColumns.Start.Row + 2, currentColumn, importCertificateColumns.Start.Row + 2, currentColumn + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[importCertificateColumns.Start.Row + 2, currentColumn, importCertificateColumns.Start.Row + 2, currentColumn + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[importCertificateColumns.Start.Row + 2, currentColumn].Style.Font.Bold = true;
					ws.Cells[importCertificateColumns.Start.Row + 2, currentColumn].Style.Font.Size = 10;
					// ish o'rni
					ws.Cells[importCertificateColumns.Start.Row + 2, currentColumn + 1].Value = ws.Cells[importCertificateColumns.Start.Row + 2, importCertificateColumns.Start.Column + 1].Value;
					ws.Cells[importCertificateColumns.Start.Row + 2, currentColumn + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[importCertificateColumns.Start.Row + 2, currentColumn + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[importCertificateColumns.Start.Row + 2, currentColumn, importCertificateColumns.Start.Row + 2, currentColumn + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[importCertificateColumns.Start.Row + 2, currentColumn, importCertificateColumns.Start.Row + 2, currentColumn + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[importCertificateColumns.Start.Row + 2, currentColumn, importCertificateColumns.Start.Row + 2, currentColumn + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[importCertificateColumns.Start.Row + 2, currentColumn, importCertificateColumns.Start.Row + 2, currentColumn + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[importCertificateColumns.Start.Row + 2, currentColumn + 1].Style.Font.Bold = true;
					ws.Cells[importCertificateColumns.Start.Row + 2, currentColumn + 1].Style.Font.Size = 10;
				}

				ws.Cells[importCertificateColumns.Start.Row, importCertificateColumns.Start.Column + 2].Value = "шундан";

				if (_cultureHelper.CurrentCulture.Id == LanguageIdConst.UZ_CYRL)
					ws.Cells[importCertificateColumns.Start.Row, importCertificateColumns.Start.Column + 2].Value = "шундан";
				else if (_cultureHelper.CurrentCulture.Id == LanguageIdConst.UZ_LATN)
					ws.Cells[importCertificateColumns.Start.Row, importCertificateColumns.Start.Column + 2].Value = "shundan";
				else
					ws.Cells[importCertificateColumns.Start.Row, importCertificateColumns.Start.Column + 2].Value = "от этого";

				ws.Cells[importCertificateColumns.Start.Row, importCertificateColumns.Start.Column + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				ws.Cells[importCertificateColumns.Start.Row, importCertificateColumns.Start.Column + 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				ws.Cells[importCertificateColumns.Start.Row, importCertificateColumns.Start.Column + 2].Style.Font.Bold = true;
				ws.Cells[importCertificateColumns.Start.Row, importCertificateColumns.Start.Column + 2].Style.Font.Size = 10;
				ws.Cells[importCertificateColumns.Start.Row, importCertificateColumns.Start.Column + 2, importCertificateColumns.Start.Row, importCertificateColumns.Start.Column + 1 + (data.Columns.Count * 2)].Merge = true;
				ws.Cells[importCertificateColumns.Start.Row, importCertificateColumns.Start.Column + 2, importCertificateColumns.Start.Row, importCertificateColumns.Start.Column + 1 + (data.Columns.Count * 2)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells[importCertificateColumns.Start.Row, importCertificateColumns.Start.Column + 2, importCertificateColumns.Start.Row, importCertificateColumns.Start.Column + 1 + (data.Columns.Count * 2)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				ws.Cells[importCertificateColumns.Start.Row, importCertificateColumns.Start.Column + 2, importCertificateColumns.Start.Row, importCertificateColumns.Start.Column + 1 + (data.Columns.Count * 2)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				ws.Cells[importCertificateColumns.Start.Row, importCertificateColumns.Start.Column + 2, importCertificateColumns.Start.Row, importCertificateColumns.Start.Column + 1 + (data.Columns.Count * 2)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				#endregion


				var importRow = excelPackage.Workbook.Names["ImportRow"];
				int currentRow = importRow.Start.Row;
				int index = 1;

				foreach (var item in data.Rows)
				{
					var column = 1;
					ws.InsertRow(currentRow, 1, importRow.Start.Row);
					ws.Cells[currentRow, column++].Value = index++;
					ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

					if (dto.ByRegion == true)
					{
						ws.Cells[currentRow, column++].Value = item.Region;
						ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.WrapText = true;
					}
					else if (dto.ByDistrict == true)
					{
						ws.Cells[currentRow, column++].Value = item.District;
						ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.WrapText = true;

					}
					else
					{
						ws.Cells[currentRow, column++].Value = item.ContractorInn + " - " + item.Contractor;
						ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.WrapText = true;

					}
					ws.Cells[currentRow, column++].Value = item.TotalApplication.TotalCount;
					ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.WrapText = true;

					ws.Cells[currentRow, column++].Value = item.TotalApplication.TotalNewVacanciesCount;
					ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.WrapText = true;

					foreach (var subItem in item.CountApplication.Where(a => a.Key != 0).OrderBy(a => a.Key))
					{
						ws.Cells[currentRow, column++].Value = subItem.Value.Count;
						ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.WrapText = true;

						ws.Cells[currentRow, column++].Value = subItem.Value.NewVacanciesCount;
						ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.WrapText = true;
					}

					ws.Cells[currentRow, column++].Value = item.TotalContract.TotalCount;
					ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.WrapText = true;

					ws.Cells[currentRow, column++].Value = item.TotalContract.TotalNewVacanciesCount;
					ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.WrapText = true;

					foreach (var subItem in item.CountContractsCreated.Where(a => a.Key != 0).OrderBy(a => a.Key))
					{
						ws.Cells[currentRow, column++].Value = subItem.Value.Count;
						ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.WrapText = true;

						ws.Cells[currentRow, column++].Value = subItem.Value.NewVacanciesCount;
						ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.WrapText = true;
					}

					ws.Cells[currentRow, column++].Value = item.TotalContractCreated.TotalCount;
					ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.WrapText = true;

					ws.Cells[currentRow, column++].Value = item.TotalContractCreated.TotalNewVacanciesCount;
					ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.WrapText = true;

					foreach (var subItem in item.CountContracts.Where(a => a.Key != 0).OrderBy(a => a.Key))
					{
						ws.Cells[currentRow, column++].Value = subItem.Value.Count;
						ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.WrapText = true;

						ws.Cells[currentRow, column++].Value = subItem.Value.NewVacanciesCount;
						ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.WrapText = true;
					}

					ws.Cells[currentRow, column++].Value = item.TotalCertificate.TotalCount;
					ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.WrapText = true;

					ws.Cells[currentRow, column++].Value = item.TotalCertificate.TotalNewVacanciesCount;
					ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.WrapText = true;

					foreach (var subItem in item.CountContractsSigned.Where(a => a.Key != 0).OrderBy(a => a.Key))
					{
						ws.Cells[currentRow, column++].Value = subItem.Value.Count;
						ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.WrapText = true;

						ws.Cells[currentRow, column++].Value = subItem.Value.NewVacanciesCount;
						ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.WrapText = true;
					}

					ws.Cells[currentRow, column++].Value = item.TotalContractSigned.TotalCount;
					ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.WrapText = true;

					ws.Cells[currentRow, column++].Value = item.TotalContractSigned.TotalNewVacanciesCount;
					ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, column - 1].Style.WrapText = true;

					foreach (var subItem in item.CountCertificates.Where(a => a.Key != 0).OrderBy(a => a.Key))
					{
						ws.Cells[currentRow, column++].Value = subItem.Value.Count;
						ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.WrapText = true;

						ws.Cells[currentRow, column++].Value = subItem.Value.NewVacanciesCount;
						ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
						ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						ws.Cells[currentRow, column - 1].Style.WrapText = true;
					}

					currentRow++;
				}
				//JAMI
				var totalColumn = 1;
				ws.Cells[currentRow, totalColumn].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				totalColumn += 1;
				ws.Cells[currentRow, totalColumn++].Value = "Jami";
				ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;

				ws.Cells[currentRow, totalColumn++].Value = data.ApplicationTotals.TotalCount;
				ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;

				ws.Cells[currentRow, totalColumn++].Value = data.ApplicationTotals.TotalNewVacanciesCount;
				ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;
				foreach (var applicationColumnTotal in data.ApplicationColumnTotals)
				{
					ws.Cells[currentRow, totalColumn++].Value = applicationColumnTotal.Value.TotalCount;
					ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;

					ws.Cells[currentRow, totalColumn++].Value = applicationColumnTotal.Value.TotalNewVacanciesCount;
					ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;
				}

				ws.Cells[currentRow, totalColumn++].Value = data.ContractCreatedTotals.TotalCount;
				ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;

				ws.Cells[currentRow, totalColumn++].Value = data.ContractCreatedTotals.TotalNewVacanciesCount;
				ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;

				foreach (var contractCreateTotal in data.ContractColumnThatCreatedTotals)
				{
					ws.Cells[currentRow, totalColumn++].Value = contractCreateTotal.Value.TotalCount;
					ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;

					ws.Cells[currentRow, totalColumn++].Value = contractCreateTotal.Value.TotalNewVacanciesCount;
					ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;
				}

				ws.Cells[currentRow, totalColumn++].Value = data.ContractTotals.TotalCount;
				ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;

				ws.Cells[currentRow, totalColumn++].Value = data.ContractTotals.TotalNewVacanciesCount;
				ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;

				foreach (var contractTotal in data.ContractColumnTotals)
				{
					ws.Cells[currentRow, totalColumn++].Value = contractTotal.Value.TotalCount;
					ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;

					ws.Cells[currentRow, totalColumn++].Value = contractTotal.Value.TotalNewVacanciesCount;
					ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;
				}

				ws.Cells[currentRow, totalColumn++].Value = data.ContractSignedTotals.TotalCount;
				ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;

				ws.Cells[currentRow, totalColumn++].Value = data.ContractSignedTotals.TotalNewVacanciesCount;
				ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;

				foreach (var contractSignedTotal in data.ContractColumnThatSignedTotals)
				{
					ws.Cells[currentRow, totalColumn++].Value = contractSignedTotal.Value.TotalCount;
					ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;

					ws.Cells[currentRow, totalColumn++].Value = contractSignedTotal.Value.TotalNewVacanciesCount;
					ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;
				}

				ws.Cells[currentRow, totalColumn++].Value = data.CertificateTotals.TotalCount;
				ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;

				ws.Cells[currentRow, totalColumn++].Value = data.CertificateTotals.TotalNewVacanciesCount;
				ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;

				foreach (var certificateColumnTotal in data.CertificateColumnTotals)
				{
					ws.Cells[currentRow, totalColumn++].Value = certificateColumnTotal.Value.TotalCount;
					ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;

					ws.Cells[currentRow, totalColumn++].Value = certificateColumnTotal.Value.TotalNewVacanciesCount;
					ws.Cells[currentRow, totalColumn - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					ws.Cells[currentRow, totalColumn - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					ws.Cells[currentRow, totalColumn - 1].Style.Font.Bold = true;
				}
				result = new MemoryStream(excelPackage.GetAsByteArray());
				excelPackage.Dispose();
			}
			result.Position = 0;
			return result;
		}
		public async Task<List<PrtnReportOnProjectImplementationAndBenefitsGranted>> ReportOnProjectImplementationAndBenefitsGranted(PrtnFilterDto dto)
		{
			if (dto.StartDate.IsNullOrEmptyObject() || dto.StartDate.Year == 1)
				dto.StartDate = new DateTime(2022, 01, 01);
			if (dto.EndDate.IsNullOrEmptyObject() || dto.EndDate.Year == 1)
				dto.EndDate = new DateTime(DateTime.Now.Year, 12, 31);


			int lang_id = (int)(_cultureHelper.CurrentCulture.Id.IsNullOrEmptyObject() ? 3 : _cultureHelper.CurrentCulture.Id);

			// region or district
			var regOrDist = _unitOfWork.Context.RegionOrDistrict(
				 (dto.HasRegion) ? dto.RegionId : 0, lang_id, 0).ToList();

			int region = 0;
			int district = 0;
			if (!dto.ByContractor)
			{
				district = dto.ByContractor ? dto.DistrictId.Value : 6;
				region = dto.ByContractor ? dto.RegionId.Value : 1;
			}

			var contrctor = _unitOfWork.Context.Contractors.IsActive()
				.Where(a => a.RegionId == region && a.DistrictId == district)
				.Select(a => new ContractorIdInnName
				{
					Id = a.Id,
					Inn = a.Inn,
					Name = a.FullName
				}).AsQueryable().ToArray();
			//sertificat coni 
			//var result = _unitOfWork.Context.ReportOnProjectImplementationAndBenefitsGranted(
			//  (dto.HasRegion) ? dto.RegionId : 0,
			//  lang_id,
			//  dto.ByContractor,
			//  (dto.HasDistrict) ? dto.DistrictId : 0
			//   ).ToList();

			var result = GetPrtnApplicationByContractTypeMethod(new()
			{
				ByRegion = dto.ByRegion,
				ByDistrict = dto.ByDistrict,
				ByContractor = dto.ByContractor,
				RegionId = dto.HasRegion ? dto.RegionId : null,
				DistrictId = dto.HasDistrict ? dto.DistrictId : null
			}).Rows;
			//ishga tushgan liyixalar
			var result2 = _unitOfWork.Context.GetReportPrtnCertificateWithGraph(
			 (dto.HasRegion) ? dto.RegionId : 0,
			 dto.StartDate.Year,
			 dto.StartDate.Month,
			 dto.EndDate.Month,
			 dto.EndDate.Year,
			 lang_id,
			 dto.ByContractor,
			  (dto.HasDistrict) ? dto.DistrictId : 0
			  ).ToList();

			//soliqdan ma'lumot

			//ajratilgan kredit
			var taxCreditReport = GetTaxCreditReport(new TaxCreditReportDtoFilter
			{
				Year = DateTime.Now.Year,
				ByRegion = dto.ByRegion,
				ByDistrict = dto.ByDistrict,
				ByContractor = dto.ByContractor,
				RegionId = dto.HasRegion ? dto.RegionId : null,
				DistrictId = dto.HasDistrict ? dto.DistrictId : null
			}).ToList();

			//Soliqdan olingan tadbirkor
			var taxReport = _unitOfWork.Context.GetTaxReportByContractor(
				null,
				null,
			 dto.StartDate.Year,
			 dto.EndDate.Year,
			 dto.StartDate.Month,
			 dto.EndDate.Month,
			  (dto.HasRegion) ? dto.RegionId : null,
			 (dto.HasDistrict) ? dto.DistrictId : null,
			  dto.ByRegion,
			  dto.ByDistrict,
			  dto.ByContractor,
			 true,
			 lang_id
			  ).ToList();

			//hokimyatdan kelgan ma,lot
			var reportGovernment = _unitOfWork.Context.GetReportExecutionApplicationContractor(
			  (dto.HasRegion) ? dto.RegionId : 0,
			  dto.StartDate.Year,
			 dto.StartDate.Month,
			 dto.EndDate.Month,
			 dto.EndDate.Year,
			 lang_id,
			 dto.ByContractor,
			 (dto.HasDistrict) ? dto.DistrictId : 0
			  ).ToList();
			//bojxona
			var FromCustomsReport = GetBojxonaImtiyozReportByContractor(new BojxonaImtiyozReportByContractorDtoFilter
			{
				ByRegion = dto.ByRegion,
				HasCertificate = true,
				ByDistrict = dto.ByDistrict,
				RegionId = dto.HasRegion ? dto.RegionId : null,
				ByContractor = dto.ByContractor,
				DistrictId = dto.HasDistrict ? dto.DistrictId : null,

			}).ToList();

			List<PrtnReportOnProjectImplementationAndBenefitsGranted> data = new();
			if (!dto.ByContractor && dto.ByDistrict)
			{

				var bankCreditReport = await GetBankCreditReport(new ContractorBankCreditReportDtoFilter
				{
					ByRegion = dto.ByRegion
				});
				//berilgan kafillik

				var businessActivityReport = GetBusinessActivityTypeReportByRegion(new BusinessActivityTypeReportByRegionFilter
				{
					ByRegion = dto.ByRegion,
					ByDistrict = dto.ByDistrict,
					RegionId = dto.HasRegion ? dto.RegionId : null,
				}).ToList();

				foreach (var item in regOrDist)
				{
					var temp = new PrtnReportOnProjectImplementationAndBenefitsGranted();

					temp.Region = item.RegionName;
					temp.RegionId = item.RegionId;
					temp.District = item.DistrictName;
					temp.DistrictId = item.DistrictId;
					long s = result?.FirstOrDefault(x => x?.DistrictId == item?.DistrictId)?.TotalApplication.TotalCount ?? -1;
					temp.CertificateHolders = new()
					{
						ContractorCount = result?.FirstOrDefault(x => x?.DistrictId == item?.DistrictId)?.TotalApplication.TotalCount ?? 0,
						JobCount = result?.FirstOrDefault(x => x?.DistrictId == item?.DistrictId)?.TotalApplication.TotalNewVacanciesCount ?? 0
					};
					temp.RelationToLaunchedProjects = new()
					{
						ContractorCount = result2?.FirstOrDefault(x => x.DistrictId == item.DistrictId)?.ContractorCount ?? 0,
						JobCount = result2?.FirstOrDefault(x => x.DistrictId == item.DistrictId)?.JobCount ?? 0
					};
					temp.DataFromTaxContractor = new()
					{
						Count = taxReport?.FirstOrDefault(x => x.DistrictId == item?.DistrictId)?.ContractorCount ?? 0,
						JobCount = taxReport?.FirstOrDefault(x => x.DistrictId == item?.DistrictId)?.NumberEmployee1 ?? 0,
						Summ = 0
					};
					temp.DataFromGovernmentContractorCount = new()
					{
						Count = reportGovernment?.FirstOrDefault(x => x.DistrictId == item?.DistrictId)?.ContractorCount ?? 0,
						JobCount = reportGovernment?.FirstOrDefault(x => x.DistrictId == item?.DistrictId)?.JobCount ?? 0,
						Summ = reportGovernment?.FirstOrDefault(x => x.DistrictId == item?.DistrictId)?.AverageSalary ?? 0m
					};
					temp.AprovedCredit = new()
					{
						Count = bankCreditReport?.FirstOrDefault(x => x.DistrictId == item?.DistrictId)?.Application.SubmittedCount ?? 0,
						Summ = bankCreditReport?.FirstOrDefault(x => x.DistrictId == item?.DistrictId)?.Application.SubmittedSum ?? 0.0
					};
					temp.SeparatePreferentialCredit = new()
					{
						Count = businessActivityReport?.FirstOrDefault(x => x.DistrictId == item?.DistrictId)?.BusinessActivity.UserPrivilegeCount ?? 0,
						Summ = businessActivityReport?.FirstOrDefault(x => x.DistrictId == item?.DistrictId)?.BusinessActivity.ApprovedFinancialHelpAmount ?? 0m
					};
					temp.PropertyAndLandTax = new()
					{
						Count = taxCreditReport?.FirstOrDefault(x => x.DistrictId == item?.DistrictId)?.ContractorPropertyTaxCount ?? 0,
						Summ = taxCreditReport?.FirstOrDefault(x => x.DistrictId == item?.DistrictId)?.PropertyTaxSum ?? 0m
					};
					temp.IncomeTax = new()
					{
						Count = taxCreditReport?.FirstOrDefault(x => x.DistrictId == item?.DistrictId)?.ContractorIncomeTaxCount ?? 0,
						Summ = taxCreditReport?.FirstOrDefault(x => x.DistrictId == item?.DistrictId)?.IncomeTaxSum ?? 0m
					};
					temp.FiftyPercentOfTheTaxRate = new()
					{
						Count = taxCreditReport?.FirstOrDefault(x => x.DistrictId == item?.DistrictId)?.ContractorSocialTaxCount ?? 0,
						Summ = taxCreditReport?.FirstOrDefault(x => x.DistrictId == item?.DistrictId)?.SocialTaxSum ?? 0m
					};
					temp.CrossAccountingOfVAT = new()
					{
						Count = 0,
						Summ = 0
					};
					temp.FromCustoms = new()
					{
						Count = FromCustomsReport?.FirstOrDefault(x => x.DistrictId == item?.DistrictId)?.TotalContractorCount ?? 0,
						GreenLand = FromCustomsReport?.FirstOrDefault(x => x.DistrictId == item?.DistrictId)?.AppContractorCount ?? 0,

						Summ = 0
					};
					temp.PeopleTaxesWithoutInsuranceAndWithoutInterest = new()
					{
						Count = FromCustomsReport?.FirstOrDefault(x => x.DistrictId == item?.DistrictId)?.CertificateCount ?? 0,
						Summ = 0,
					};
					temp.PracticalMonocenter = new()
					{
						AuctionBuildings = 0,
						BuildingsCPC = 0
					};

					data.Add(temp);
				}
			}
			else if (dto.ByRegion)
			{
				var bankCreditReport = await GetBankCreditReport(new ContractorBankCreditReportDtoFilter
				{
					ByRegion = dto.ByRegion
				});
				//berilgan kafillik

				var businessActivityReport = GetBusinessActivityTypeReportByRegion(new BusinessActivityTypeReportByRegionFilter
				{
					ByRegion = dto.ByRegion,
					ByDistrict = dto.ByDistrict,
					RegionId = dto.HasRegion ? dto.RegionId : null,
				}).ToList();

				foreach (var item in regOrDist)
				{
					var temp = new PrtnReportOnProjectImplementationAndBenefitsGranted();

					temp.Region = item.RegionName;
					temp.RegionId = item.RegionId;
					temp.District = item.DistrictName;
					temp.DistrictId = item.DistrictId;
					long s = result?.FirstOrDefault(x => x?.RegionId == item?.RegionId)?.TotalApplication.TotalCount ?? -1;
					temp.CertificateHolders = new()
					{
						ContractorCount = result?.FirstOrDefault(x => x?.RegionId == item?.RegionId)?.TotalApplication.TotalCount ?? 0,
						JobCount = result?.FirstOrDefault(x => x?.RegionId == item?.RegionId)?.TotalApplication.TotalNewVacanciesCount ?? 0
					};
					temp.RelationToLaunchedProjects = new()
					{
						ContractorCount = result2?.FirstOrDefault(x => x.RegionId == item?.RegionId)?.ContractorCount ?? 0,
						JobCount = result2?.FirstOrDefault(x => x.RegionId == item?.RegionId)?.JobCount ?? 0
					};
					temp.DataFromTaxContractor = new()
					{
						Count = taxReport?.FirstOrDefault(x => x.RegionId == item.RegionId)?.ContractorCount ?? 0,
						JobCount = taxReport?.FirstOrDefault(x => x.RegionId == item.RegionId)?.NumberEmployee1 ?? 0,
						Summ = 0
					};
					temp.DataFromGovernmentContractorCount = new()
					{
						Count = reportGovernment?.FirstOrDefault(x => x.RegionId == item.RegionId)?.ContractorCount ?? 0,
						JobCount = reportGovernment?.FirstOrDefault(x => x.RegionId == item.RegionId)?.JobCount ?? 0,
						Summ = reportGovernment?.FirstOrDefault(x => x.RegionId == item.RegionId)?.AverageSalary ?? 0m
					};
					temp.AprovedCredit = new()
					{
						Count = bankCreditReport?.FirstOrDefault(x => x.RegionId == item.RegionId)?.Application.SubmittedCount ?? 0,
						Summ = bankCreditReport?.FirstOrDefault(x => x.RegionId == item.RegionId)?.Application.SubmittedSum ?? 0.0
					};
					temp.SeparatePreferentialCredit = new()
					{
						Count = businessActivityReport?.FirstOrDefault(x => x.RegionId == item.RegionId)?.BusinessActivity.UserPrivilegeCount ?? 0,
						Summ = businessActivityReport?.FirstOrDefault(x => x.RegionId == item.RegionId)?.BusinessActivity.ApprovedFinancialHelpAmount ?? 0m
					};
					temp.PropertyAndLandTax = new()
					{
						Count = taxCreditReport?.FirstOrDefault(x => x.RegionId == item.RegionId)?.ContractorPropertyTaxCount ?? 0,
						Summ = taxCreditReport?.FirstOrDefault(x => x.RegionId == item.RegionId)?.PropertyTaxSum ?? 0m
					};
					temp.IncomeTax = new()
					{
						Count = taxCreditReport?.FirstOrDefault(x => x.RegionId == item.RegionId)?.ContractorIncomeTaxCount ?? 0,
						Summ = taxCreditReport?.FirstOrDefault(x => x.RegionId == item.RegionId)?.IncomeTaxSum ?? 0m
					};
					temp.FiftyPercentOfTheTaxRate = new()
					{
						Count = taxCreditReport?.FirstOrDefault(x => x.RegionId == item.RegionId)?.ContractorSocialTaxCount ?? 0,
						Summ = taxCreditReport?.FirstOrDefault(x => x.RegionId == item.RegionId)?.SocialTaxSum ?? 0m
					};
					temp.CrossAccountingOfVAT = new()
					{
						Count = 0,
						Summ = 0
					};
					temp.FromCustoms = new()
					{
						Count = FromCustomsReport?.FirstOrDefault(x => x.RegionId == item.RegionId)?.TotalContractorCount ?? 0,
						GreenLand = FromCustomsReport?.FirstOrDefault(x => x.RegionId == item.RegionId)?.AppContractorCount ?? 0,

						Summ = 0
					};
					temp.PeopleTaxesWithoutInsuranceAndWithoutInterest = new()
					{
						Count = FromCustomsReport?.FirstOrDefault(x => x.RegionId == item.RegionId)?.CertificateCount ?? 0,
						Summ = 0,
					};
					temp.PracticalMonocenter = new()
					{
						AuctionBuildings = 0,
						BuildingsCPC = 0
					};

					data.Add(temp);
				}
			}
			else
			{
				foreach (var item in contrctor)
				{
					var temp = new PrtnReportOnProjectImplementationAndBenefitsGranted();
					temp.Contractor = item.Name;
					temp.ContractorId = item.Id;
					temp.ContractorInn = item.Inn;

					temp.CertificateHolders = new()
					{
						ContractorCount = result?.FirstOrDefault(x => x?.ContractorId == item?.Id)?.TotalApplication.TotalCount ?? 0,
						JobCount = result?.FirstOrDefault(x => x?.ContractorId == item?.Id)?.TotalApplication.TotalNewVacanciesCount ?? 0
					};
					temp.RelationToLaunchedProjects = new()
					{
						ContractorCount = result2?.FirstOrDefault(x => x?.ContractorId == item?.Id)?.ContractorCount ?? 0,
						JobCount = result2?.FirstOrDefault(x => x?.ContractorId == item?.Id)?.JobCount ?? 0
					};
					temp.DataFromTaxContractor = new()
					{
						Count = taxReport?.FirstOrDefault(x => x?.ContractorId == item?.Id)?.ContractorCount ?? 0,
						JobCount = taxReport?.FirstOrDefault(x => x?.ContractorId == item?.Id)?.NumberEmployee1 ?? 0,
						Summ = 0
					};
					temp.DataFromGovernmentContractorCount = new()
					{
						Count = reportGovernment?.FirstOrDefault(x => x?.ContractorId == item?.Id)?.ContractorCount ?? 0,
						JobCount = reportGovernment?.FirstOrDefault(x => x?.ContractorId == item?.Id)?.JobCount ?? 0,
						Summ = reportGovernment?.FirstOrDefault(x => x?.ContractorId == item?.Id)?.AverageSalary ?? 0m
					};
					temp.AprovedCredit = new()
					{
						Count = 0,//bankCreditReport?.FirstOrDefault(x => x.RegionId == item.RegionId || x.DistrictId == item?.DistrictId)?.Application.SubmittedCount ?? 0,
						Summ = 0//bankCreditReport?.FirstOrDefault(x => x.RegionId == item.RegionId || x.DistrictId == item?.DistrictId)?.Application.SubmittedSum ?? 0.0
					};
					temp.SeparatePreferentialCredit = new()
					{
						Count = 0,//businessActivityReport?.FirstOrDefault(x => x?.ContractorId == item?.Id)?.BusinessActivity.UserPrivilegeCount ?? 0,
						Summ = 0//SbusinessActivityReport?.FirstOrDefault(x => x?.ContractorId == item?.Id)?.BusinessActivity.ApprovedFinancialHelpAmount ?? 0m
					};
					temp.PropertyAndLandTax = new()
					{
						Count = taxCreditReport?.FirstOrDefault(x => x?.ContractorId == item?.Id)?.ContractorPropertyTaxCount ?? 0,
						Summ = taxCreditReport?.FirstOrDefault(x => x?.ContractorId == item?.Id)?.PropertyTaxSum ?? 0m
					};
					temp.IncomeTax = new()
					{
						Count = taxCreditReport?.FirstOrDefault(x => x?.ContractorId == item?.Id)?.ContractorIncomeTaxCount ?? 0,
						Summ = taxCreditReport?.FirstOrDefault(x => x?.ContractorId == item?.Id)?.IncomeTaxSum ?? 0m
					};
					temp.FiftyPercentOfTheTaxRate = new()
					{
						Count = taxCreditReport?.FirstOrDefault(x => x?.ContractorId == item?.Id)?.ContractorSocialTaxCount ?? 0,
						Summ = taxCreditReport?.FirstOrDefault(x => x?.ContractorId == item?.Id)?.SocialTaxSum ?? 0m
					};
					temp.CrossAccountingOfVAT = new()
					{
						Count = 0,
						Summ = 0
					};
					temp.FromCustoms = new()
					{
						Count = FromCustomsReport?.FirstOrDefault(x => x?.ContractorId == item?.Id)?.TotalContractorCount ?? 0,
						GreenLand = FromCustomsReport?.FirstOrDefault(x => x?.ContractorId == item?.Id)?.AppContractorCount ?? 0,

						Summ = 0
					};
					temp.PeopleTaxesWithoutInsuranceAndWithoutInterest = new()
					{
						Count = FromCustomsReport?.FirstOrDefault(x => x?.ContractorId == item?.Id)?.CertificateCount ?? 0,
						Summ = 0,
					};
					temp.PracticalMonocenter = new()
					{
						AuctionBuildings = 0,
						BuildingsCPC = 0
					};

					data.Add(temp);
				}
			}

			return data;
		}

		#endregion

		#region Appeal
		public List<AppealReportByTypeReportDto> GetAppealApplicationReport(AppealReportDtoFilter filter)
		{
			IQueryable<AppealApplication> baseQuery = _unitOfWork.Context.Set<AppealApplication>()
			   .Where(a => a.StatusId != StatusIdConst.DELETED);

			if (filter.ByRegion)
			{
				var regionData = baseQuery.
					Select(x => new
					{
						x.RegionId,
						RegionName = x.Region.FullName,
						RegionCode = x.Region.OrderCode,
						x.AppealTypeId,
						x.StatusId
					})
					.ToList()
					.GroupBy(x => new { x.RegionId, x.RegionName, x.RegionCode })
					.Select(r => new
					{
						RegionId = r.Key.RegionId,
						RegionName = r.Key.RegionName,
						RegionCode = r.Key.RegionCode,
						TotalAppealApplicationType =
						(
							Application: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Application),
							Proposal: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Proposal),
							Complaint: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Proposal)
						),
						TotalAppealApplicationTypeSent =
						(
							Application: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Application && a.StatusId == StatusIdConst.SENT),
							Proposal: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Proposal && a.StatusId == StatusIdConst.SENT),
							Complaint: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Proposal && a.StatusId == StatusIdConst.SENT)
						),
						TotalAppealApplicationTypeCreate =
						(
							Application: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Application && a.StatusId == StatusIdConst.CREATED),
							Proposal: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Proposal && a.StatusId == StatusIdConst.CREATED),
							Complaint: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Proposal && a.StatusId == StatusIdConst.CREATED)
						),
						TotalAppealApplicationTypeInExecution =
						(
							Application: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Application && a.StatusId == StatusIdConst.IN_EXECUTION),
							Proposal: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Proposal && a.StatusId == StatusIdConst.IN_EXECUTION),
							Complaint: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Proposal && a.StatusId == StatusIdConst.IN_EXECUTION)
						),
						TotalAppealApplicationTypeExecuted =
						(
							Application: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Application && a.StatusId == StatusIdConst.EXECUTED),
							Proposal: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Proposal && a.StatusId == StatusIdConst.EXECUTED),
							Complaint: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Complaint && a.StatusId == StatusIdConst.EXECUTED)
						),
						TotalAppealApplicationCanceled = r.Count(a => a.StatusId == StatusIdConst.CANCELED)
					});

				var result = regionData.Select(x => new AppealReportByTypeReportDto
				{
					RegionId = x.RegionId,
					RegionName = x.RegionName,
					RegionCode = x.RegionCode,
					TotalAppealApplicationType = x.TotalAppealApplicationType,
					TotalAppealApplicationTypeSent = x.TotalAppealApplicationTypeSent,
					TotalAppealApplicationTypeCreate = x.TotalAppealApplicationTypeCreate,
					TotalAppealApplicationTypeInExecution = x.TotalAppealApplicationTypeInExecution,
					TotalAppealApplicationTypeExecuted = x.TotalAppealApplicationTypeExecuted,
					TotalAppealApplicationCanceled = x.TotalAppealApplicationCanceled,
				}).OrderBy(x => x.RegionCode).ToList();

				return result;
			}
			if (filter.ByDistrict)
			{
				baseQuery = baseQuery.Where(a => a.RegionId == filter.RegionId);

				var districtData = baseQuery.
					Select(x => new
					{
						x.RegionId,
						x.DistrictId,
						RegionCode = x.Region.OrderCode,
						DistrictName = x.District.FullName,
						x.AppealTypeId,
						x.StatusId
					})
					.ToList()
					.GroupBy(x => new { x.RegionId, x.DistrictId, x.DistrictName, x.RegionCode })
					.Select(r => new
					{
						RegionId = r.Key.RegionId,
						RegionCode = r.Key.RegionCode,
						DistrictId = r.Key.DistrictId,
						DistrictName = r.Key.DistrictName,
						TotalAppealApplicationType =
						(
							Application: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Application),
							Proposal: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Proposal),
							Complaint: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Proposal)
						),
						TotalAppealApplicationTypeSent =
						(
							Application: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Application && a.StatusId == StatusIdConst.SENT),
							Proposal: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Proposal && a.StatusId == StatusIdConst.SENT),
							Complaint: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Proposal && a.StatusId == StatusIdConst.SENT)
						),
						TotalAppealApplicationTypeCreate =
						(
							Application: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Application && a.StatusId == StatusIdConst.CREATED),
							Proposal: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Proposal && a.StatusId == StatusIdConst.CREATED),
							Complaint: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Proposal && a.StatusId == StatusIdConst.CREATED)
						),
						TotalAppealApplicationTypeInExecution =
						(
							Application: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Application && a.StatusId == StatusIdConst.IN_EXECUTION),
							Proposal: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Proposal && a.StatusId == StatusIdConst.IN_EXECUTION),
							Complaint: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Proposal && a.StatusId == StatusIdConst.IN_EXECUTION)
						),
						TotalAppealApplicationTypeExecuted =
						(
							Application: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Application && a.StatusId == StatusIdConst.EXECUTED),
							Proposal: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Proposal && a.StatusId == StatusIdConst.EXECUTED),
							Complaint: r.Count(a => a.AppealTypeId == AppealTypeIdConst.Complaint && a.StatusId == StatusIdConst.EXECUTED)
						),
						TotalAppealApplicationCanceled = r.Count(a => a.StatusId == StatusIdConst.CANCELED)
					});

				var result = districtData.Select(x => new AppealReportByTypeReportDto
				{
					RegionId = x.RegionId,
					RegionCode = x.RegionCode,
					DistrictId = x.DistrictId,
					DistrictName = x.DistrictName,
					TotalAppealApplicationType = x.TotalAppealApplicationType,
					TotalAppealApplicationTypeSent = x.TotalAppealApplicationTypeSent,
					TotalAppealApplicationTypeCreate = x.TotalAppealApplicationTypeCreate,
					TotalAppealApplicationTypeInExecution = x.TotalAppealApplicationTypeInExecution,
					TotalAppealApplicationTypeExecuted = x.TotalAppealApplicationTypeExecuted,
					TotalAppealApplicationCanceled = x.TotalAppealApplicationCanceled,
				}).OrderBy(x => x.RegionCode).ToList();

				return result;
			}
			return new List<AppealReportByTypeReportDto>();
		}
		#endregion

		#region CLAIM
		/// Даъво аризасига юборилган мурожаатлар бўйича (Ариза стасуси кесимида)
		public List<AppealsSentToClaimAppDto> GetAppealsSentToClaimApplication(ClaimApplicationReportsDtoFilter filter)
		{
			List<AppealsSentToClaimAppDto> res = new();

			res = this.StorageClaimApplication(filter)
				.Select(a => new AppealsSentToClaimAppDto
				{
					RegionId = filter.ByRegion ? a.Application.RegionId : null,
					RegionOrderCode = filter.ByRegion ? a.Application.Region.OrderCode : null,
					Region = filter.ByRegion
						? a.Application.Region.Translates.AsQueryable()
							.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
							.TranslateText ?? a.Application.Region.FullName
						: null,

					DistrictId = filter.ByDistrict ? a.Application.DistrictId : null,
					DistrictOrderCode = filter.ByDistrict ? a.Application.District.OrderCode : null,
					District = filter.ByDistrict
						? a.Application.District.Translates.AsQueryable()
							.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
							.TranslateText ?? a.Application.District.FullName
						: null,

					ContractorId = filter.ByContractor ? a.Application.ContractorId : null,
					Contractor = filter.ByContractor ? a.Application.Contractor.FullName : null,
					ContractorInn = filter.ByContractor ? a.Application.Contractor.Inn : null,
					ContractorPhoneNumber = filter.ByContractor ? a.Application.Contractor.PhoneNumber : null,

					ClaimApplicationTypeId = filter.ClaimApplicationTypeId.HasValue ? a.ClaimApplicationTypeId : null,
					ClaimApplicationType = filter.ClaimApplicationTypeId.HasValue ? (a.ClaimApplicationType.Translates.AsQueryable()
						.FirstOrDefault(ClaimApplicationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
						.TranslateText ?? a.ClaimApplicationType.FullName) : null,

					ApplicationCount = new
					(
						(a.Application.Contractor.Inn.StartsWith("2") || a.Application.Contractor.Inn.StartsWith("3")) ? 1 : 0,
						a.Application.Contractor.Pinfl != null ? 1 : 0
					),

					MediationPlanCount = new
					(
						(a.Application.MediationPlans.FirstOrDefault(x => x.StatusId != StatusIdConst.DELETED).Contractor.Inn.StartsWith("2")
							|| a.Application.MediationPlans.FirstOrDefault(x => x.StatusId != StatusIdConst.DELETED).Contractor.Inn.StartsWith("3")) ? 1 : 0,
						a.Application.MediationPlans.FirstOrDefault(x => x.StatusId != StatusIdConst.DELETED).Contractor.Pinfl != null ? 1 : 0
					),

					MediationCount = new
					(
						(a.Application.MediationPlans.FirstOrDefault(x => x.StatusId != StatusIdConst.DELETED).Mediations.FirstOrDefault(a => a.StatusId != StatusIdConst.DELETED).Contractor.Inn.StartsWith("2")
							|| a.Application.MediationPlans.FirstOrDefault(x => x.StatusId != StatusIdConst.DELETED).Mediations.FirstOrDefault(a => a.StatusId != StatusIdConst.DELETED).Contractor.Inn.StartsWith("3")) ? 1 : 0,
						a.Application.MediationPlans.FirstOrDefault(x => x.StatusId != StatusIdConst.DELETED).Mediations.FirstOrDefault(a => a.StatusId != StatusIdConst.DELETED).Contractor.Pinfl != null ? 1 : 0
					),

					ClaimApplicationForCourtCount = new
					(
						(a.Application.MediationPlans.FirstOrDefault(x => x.StatusId != StatusIdConst.DELETED).Mediations.FirstOrDefault(a => a.StatusId != StatusIdConst.DELETED).ApplicationForCourts.Contractor.Inn.StartsWith("2")
							|| a.Application.MediationPlans.FirstOrDefault(x => x.StatusId != StatusIdConst.DELETED).Mediations.FirstOrDefault(a => a.StatusId != StatusIdConst.DELETED).ApplicationForCourts.Contractor.Inn.StartsWith("3")) ? 1 : 0,
						a.Application.MediationPlans.FirstOrDefault(x => x.StatusId != StatusIdConst.DELETED).Mediations.FirstOrDefault(a => a.StatusId != StatusIdConst.DELETED).ApplicationForCourts.Contractor.Pinfl != null ? 1 : 0
					),

					CancelApplicationCount = a.Application.StatusId == StatusIdConst.CANCELED ? 1 : 0,
					CancelMediationPlanCount = a.Application.MediationPlans.FirstOrDefault(x => x.StatusId != StatusIdConst.DELETED).StatusId == StatusIdConst.CANCELED ? 1 : 0,
					CancelMediationCount = a.Application.MediationPlans.FirstOrDefault(x => x.StatusId != StatusIdConst.DELETED).Mediations.FirstOrDefault(a => a.StatusId != StatusIdConst.DELETED).StatusId == StatusIdConst.CANCELED ? 1 : 0,

					RejectApplicationCount = a.Application.StatusId == StatusIdConst.REJECTED ? 1 : 0,
					RejectMediationPlanCount = a.Application.MediationPlans.FirstOrDefault(x => x.StatusId != StatusIdConst.DELETED).StatusId == StatusIdConst.REJECTED ? 1 : 0,
					RejectMediationCount = a.Application.MediationPlans.FirstOrDefault(x => x.StatusId != StatusIdConst.DELETED).Mediations.FirstOrDefault(a => a.StatusId != StatusIdConst.DELETED).StatusId == StatusIdConst.REJECTED ? 1 : 0,

					RejectCancelApplicationCancel = new(0, 0)
				})
				.AsEnumerable()
				.GroupBy(a => new
				{
					a.ClaimApplicationTypeId,
					a.ClaimApplicationType,
					a.DistrictId,
					a.DistrictOrderCode,
					a.District,
					a.RegionId,
					a.RegionOrderCode,
					a.Region,
					a.ContractorId,
					a.Contractor,
					a.ContractorInn,
					a.ContractorPhoneNumber,
				})
				.Select(a => new AppealsSentToClaimAppDto
				{
					ClaimApplicationTypeId = a.Key.ClaimApplicationTypeId,
					ClaimApplicationType = a.Key.ClaimApplicationType,
					DistrictId = a.Key.DistrictId,
					DistrictOrderCode = a.Key.DistrictOrderCode,
					District = a.Key.District,
					RegionId = a.Key.RegionId,
					RegionOrderCode = a.Key.RegionOrderCode,
					Region = a.Key.Region,
					ContractorId = a.Key.ContractorId,
					Contractor = a.Key.Contractor,
					ContractorInn = a.Key.ContractorInn,
					ContractorPhoneNumber = a.Key.ContractorPhoneNumber,

					TotalApplicationAmountInArea =
						a.Sum(b => b.ApplicationCount.PhysicalPersonCount) + a.Sum(b => b.ApplicationCount.LegalPersonCount) +
						a.Sum(b => b.MediationPlanCount.PhysicalPersonCount) + a.Sum(b => b.MediationPlanCount.LegalPersonCount) +
						a.Sum(b => b.MediationCount.PhysicalPersonCount) + a.Sum(b => b.MediationCount.LegalPersonCount) +
						a.Sum(b => b.ClaimApplicationForCourtCount.PhysicalPersonCount) + a.Sum(b => b.ClaimApplicationForCourtCount.LegalPersonCount) +
						a.Sum(b => b.CancelApplicationCount) + a.Sum(b => b.CancelMediationPlanCount) + a.Sum(b => b.CancelMediationCount) +
						a.Sum(b => b.RejectApplicationCount) + a.Sum(b => b.RejectMediationPlanCount) + a.Sum(b => b.RejectMediationCount),

					ApplicationCount = (a.Sum(b => b.ApplicationCount.PhysicalPersonCount), a.Sum(b => b.ApplicationCount.LegalPersonCount)),

					MediationPlanCount = (a.Sum(b => b.MediationPlanCount.PhysicalPersonCount), a.Sum(b => b.MediationPlanCount.LegalPersonCount)),

					MediationCount = (a.Sum(b => b.MediationCount.PhysicalPersonCount), a.Sum(b => b.MediationCount.Item2)),

					ClaimApplicationForCourtCount = (a.Sum(b => b.ClaimApplicationForCourtCount.PhysicalPersonCount), a.Sum(b => b.ClaimApplicationForCourtCount.LegalPersonCount)),

					RejectCancelApplicationCancel = (0, a.Sum(b => b.CancelApplicationCount) + a.Sum(b => b.CancelMediationPlanCount) + a.Sum(b => b.CancelMediationCount) +
														a.Sum(b => b.RejectApplicationCount) + a.Sum(b => b.RejectMediationPlanCount) + a.Sum(b => b.RejectMediationCount)),
				})
				.ToList();

			if (filter.ByRegion)
			{
				var regions = _unitOfWork.RegionRepository.AllAsQueryable
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.Id)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							OrderCode = a.OrderCode,
							FullName = a.Translates.AsQueryable().FirstOrDefault(RegionTranslate
								.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
								?? a.FullName
						}
					);

				foreach (var region in regions)
				{
					if (res.Select(a => a.RegionId).Contains(region.Key))
						continue;

					res.Add(new AppealsSentToClaimAppDto
					{
						Region = region.Value.FullName,
						RegionOrderCode = region.Value.OrderCode,
						RegionId = region.Key,
						ApplicationCount = (0, 0),
						MediationPlanCount = (0, 0),
						MediationCount = (0, 0),
						ClaimApplicationForCourtCount = (0, 0),
						RejectCancelApplicationCancel = (0, 0)
					});
				}

				res = res.OrderBy(a => a.RegionOrderCode).ToList();
			}

			if (filter.ByDistrict)
			{
				var districts = _unitOfWork.DistrictRepository.AllAsQueryable
					.Include(a => a.Region).ThenInclude(a => a.Translates)
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.RegionId
						&& !filter.DistrictId.HasValue || filter.DistrictId == a.Id
					)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							DistrictOrderCode = a.OrderCode,
							RegionId = a.RegionId,
							Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate
								.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
								?? a.Region.FullName,
							District = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate
								.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
								?? a.FullName
						}
					);

				foreach (var district in districts)
				{
					if (res.Select(a => a.DistrictId).Contains(district.Key))
						continue;

					res.Add(new AppealsSentToClaimAppDto
					{
						DistrictOrderCode = district.Value.DistrictOrderCode,
						Region = district.Value.Region,
						RegionId = district.Value.RegionId,
						District = district.Value.District,
						DistrictId = district.Key,
						ApplicationCount = (0, 0),
						MediationPlanCount = (0, 0),
						MediationCount = (0, 0),
						ClaimApplicationForCourtCount = (0, 0),
						RejectCancelApplicationCancel = (0, 0)
					});
				}

				res = res.OrderBy(a => a.DistrictOrderCode).ToList();
			}
			return res;
		}
		/// Палатага аъзо бўлган тадбиркорлик келиб тушган Даъво аризалари(Мурожаат предмети бўйича)
		public ReceivedClaimAppDto GetReceivedClaimApplication(ClaimApplicationReportsDtoFilter filter)
		{
			ReceivedClaimAppDto res = new();
			ReceivedClaimAppDto res2 = new();

			res.Columns = _unitOfWork.ClaimThemeRepository.AllAsQueryable
				.Include(x => x.Translates)
				.IsActive().Select(a => new
				{
					Id = a.Id,
					FullName = a.Translates.AsQueryable()
						.FirstOrDefault(ClaimThemeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
						.TranslateText ?? a.FullName
				}).ToList()
			   .ToDictionary(a => a.Id, a => a.FullName);

			if (filter.IsSsp == false)
			{
				res.Rows = this.StorageClaimApplication(filter).Where(a => a.OrganizationId != 1)
			  .Select(a => new ReceivedClaimAppRowsDto
			  {
				  RegionId = filter.ByRegion ? a.Application.RegionId : null,
				  RegionOrderCode = filter.ByRegion ? a.Application.Region.OrderCode : null,
				  Region = filter.ByRegion
					  ? a.Application.Region.Translates.AsQueryable()
						  .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
						  .TranslateText ?? a.Application.Region.FullName
					  : null,

				  DistrictId = filter.ByDistrict ? a.Application.DistrictId : null,
				  DistrictOrderCode = filter.ByDistrict ? a.Application.District.OrderCode : null,
				  District = filter.ByDistrict
					  ? a.Application.District.Translates.AsQueryable()
						  .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
						  .TranslateText ?? a.Application.District.FullName
					  : null,

				  ContractorId = filter.ByContractor ? a.Application.ContractorId : null,
				  Contractor = filter.ByContractor ? a.Application.Contractor.FullName : null,
				  ContractorInn = filter.ByContractor ? a.Application.Contractor.Inn : null,
				  ContractorPhoneNumber = filter.ByContractor ? a.Application.Contractor.PhoneNumber : null,

				  ClaimApplicationTypeId = filter.ClaimApplicationTypeId.HasValue ? a.ClaimApplicationTypeId : null,
				  ClaimApplicationType = filter.ClaimApplicationTypeId.HasValue ? (a.ClaimApplicationType.Translates.AsQueryable()
					  .FirstOrDefault(ClaimApplicationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
					  .TranslateText ?? a.ClaimApplicationType.FullName) : null,

				  ClaimThemeId = a.ClaimThemeId,
				  DocumentCount = 1,

				  ClaimThemeCount = new Dictionary<int, long>()
			  })
			  .AsEnumerable()
			  .GroupBy(a => new
			  {
				  a.ClaimApplicationTypeId,
				  a.ClaimApplicationType,
				  a.DistrictId,
				  a.DistrictOrderCode,
				  a.District,
				  a.RegionId,
				  a.RegionOrderCode,
				  a.Region,
				  a.ContractorId,
				  a.Contractor,
				  a.ContractorInn,
				  a.ContractorPhoneNumber,
			  })
			  .Select(a => new ReceivedClaimAppRowsDto
			  {
				  ClaimApplicationTypeId = a.Key.ClaimApplicationTypeId,
				  ClaimApplicationType = a.Key.ClaimApplicationType,
				  DistrictId = a.Key.DistrictId,
				  DistrictOrderCode = a.Key.DistrictOrderCode,
				  District = a.Key.District,
				  RegionId = a.Key.RegionId,
				  RegionOrderCode = a.Key.RegionOrderCode,
				  Region = a.Key.Region,
				  ContractorId = a.Key.ContractorId,
				  Contractor = a.Key.Contractor,
				  ContractorInn = a.Key.ContractorInn,
				  ContractorPhoneNumber = a.Key.ContractorPhoneNumber,
				  TotalApplicationAmountInArea = a.Sum(x => x.DocumentCount),
				  ClaimThemeCount = a.GroupBy(b => b.ClaimThemeId.GetValueOrDefault()).ToDictionary(
						  b => b.Key,
						  b => (b.Sum(c => c.DocumentCount))
					  ),
			  })
			  .ToList();
			}


			if (filter.ByRegion || filter.IsSsp)
			{
				res.Rows = new List<ReceivedClaimAppRowsDto>();
				res2.Rows = this.StorageClaimApplication(filter).Where(a => a.OrganizationId == 1)
				 .Select(a => new ReceivedClaimAppRowsDto
				 {
					 OrganisationId = a.OrganizationId,
					 Organisation = a.Organization.ShortName,

					 RegionId = 15,
					 RegionOrderCode = filter.ByRegion ? a.Application.Region.OrderCode : null,
					 Region = filter.ByRegion
						 ? a.Application.Region.Translates.AsQueryable()
							 .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
							 .TranslateText ?? a.Application.Region.FullName
						 : null,

					 DistrictId = filter.ByDistrict ? a.Application.DistrictId : null,
					 DistrictOrderCode = filter.ByDistrict ? a.Application.District.OrderCode : null,
					 District = filter.ByDistrict
						 ? a.Application.District.Translates.AsQueryable()
							 .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
							 .TranslateText ?? a.Application.District.FullName
						 : null,

					 ContractorId = filter.ByContractor ? a.Application.ContractorId : null,
					 Contractor = filter.ByContractor ? a.Application.Contractor.FullName : null,
					 ContractorInn = filter.ByContractor ? a.Application.Contractor.Inn : null,
					 ContractorPhoneNumber = filter.ByContractor ? a.Application.Contractor.PhoneNumber : null,

					 ClaimApplicationTypeId = filter.ClaimApplicationTypeId.HasValue ? a.ClaimApplicationTypeId : null,
					 ClaimApplicationType = filter.ClaimApplicationTypeId.HasValue ? (a.ClaimApplicationType.Translates.AsQueryable()
						 .FirstOrDefault(ClaimApplicationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
						 .TranslateText ?? a.ClaimApplicationType.FullName) : null,

					 ClaimThemeId = a.ClaimThemeId,
					 DocumentCount = 1,

					 ClaimThemeCount = new Dictionary<int, long>()
				 })
				 .AsEnumerable()
				 .GroupBy(a => new
				 {
					 a.OrganisationId,
					 a.Organisation,
					 a.Contractor,
					 a.ContractorId
				 })
				 .Select(a => new ReceivedClaimAppRowsDto
				 {

					 OrganisationId = a.Key.OrganisationId,
					 Region = a.Key.Organisation,
					 Contractor = a.Key.Contractor,
					 ContractorId = a.Key.ContractorId,
					 TotalApplicationAmountInArea = a.Sum(x => x.DocumentCount),
					 ClaimThemeCount = a.GroupBy(b => b.ClaimThemeId.GetValueOrDefault()).ToDictionary(
							 b => b.Key,
							 b => (b.Sum(c => c.DocumentCount))
						 ),
				 })
				 .ToList();

				res.Rows.AddRange(res2.Rows);
			}

			foreach (var row in res.Rows)
			{
				if (row.ClaimThemeCount.Count() != res.Columns.Count)
				{
					foreach (var column in res.Columns)
					{
						if (!row.ClaimThemeCount.ContainsKey(column.Key))
							row.ClaimThemeCount.Add(column.Key, 0);
					}
				}
			}

			if (filter.ByRegion)
			{
				var regions = _unitOfWork.RegionRepository.AllAsQueryable
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.Id)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							OrderCode = a.OrderCode,
							FullName = a.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.FullName
						}
					);

				foreach (var region in regions)
				{
					if (res.Rows.Select(a => a.RegionId).Contains(region.Key))
						continue;

					res.Rows.Add(new ReceivedClaimAppRowsDto
					{
						Region = region.Value.FullName,
						RegionOrderCode = region.Value.OrderCode,
						RegionId = region.Key,

						ClaimThemeCount = res.Columns.ToDictionary(
							a => a.Key,
							a => (long)0
						),
					});
				}

				res.Rows = res.Rows.OrderBy(a => a.RegionOrderCode).ToList();
			}

			if (filter.ByDistrict)
			{
				var districts = _unitOfWork.DistrictRepository.AllAsQueryable
					.Include(a => a.Region).ThenInclude(a => a.Translates)
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.RegionId
						&& !filter.DistrictId.HasValue || filter.DistrictId == a.Id
					)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							DistrictOrderCode = a.OrderCode,
							RegionId = a.RegionId,
							Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.Region.FullName,
							District = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.FullName
						}
					);

				foreach (var district in districts)
				{
					if (res.Rows.Select(a => a.DistrictId).Contains(district.Key))
						continue;

					res.Rows.Add(new ReceivedClaimAppRowsDto
					{
						DistrictOrderCode = district.Value.DistrictOrderCode,
						Region = district.Value.Region,
						RegionId = district.Value.RegionId,
						District = district.Value.District,
						DistrictId = district.Key,
						ClaimThemeCount = res.Columns.ToDictionary(
							a => a.Key,
							a => (long)0
						),
					});
				}

				res.Rows = res.Rows.OrderBy(a => a.DistrictOrderCode).ToList();
			}

			return res;
		}
		/// Палатага аъзо бўлган тадбиркорлик келиб тушган Даъво аризаларининг суммаси бўйича
		public List<SummaOfClaimAppDto> GetSummaOfClaimApplication(ClaimApplicationReportsDtoFilter filter)
		{
			List<SummaOfClaimAppDto> res = new();
			List<SummaOfClaimAppDto> res2 = new();

			if (filter.IsSsp == false)
			{
				res = this.StorageClaimApplication(filter).Where(a => a.OrganizationId != 1)
	.Select(a => new SummaOfClaimAppDto
	{
		RegionId = filter.ByRegion ? a.Application.RegionId : null,
		RegionOrderCode = filter.ByRegion ? a.Application.Region.OrderCode : null,
		Region = filter.ByRegion
			? a.Application.Region.Translates.AsQueryable()
				.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
				.TranslateText ?? a.Application.Region.FullName
			: null,

		DistrictId = filter.ByDistrict ? a.Application.DistrictId : null,
		DistrictOrderCode = filter.ByDistrict ? a.Application.District.OrderCode : null,
		District = filter.ByDistrict
			? a.Application.District.Translates.AsQueryable()
				.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
				.TranslateText ?? a.Application.District.FullName
			: null,

		ContractorId = filter.ByContractor ? a.Application.ContractorId : null,
		Contractor = filter.ByContractor ? a.Application.Contractor.FullName : null,
		ContractorInn = filter.ByContractor ? a.Application.Contractor.Inn : null,
		ContractorPhoneNumber = filter.ByContractor ? a.Application.Contractor.PhoneNumber : null,

		ClaimApplicationTypeId = filter.ClaimApplicationTypeId.HasValue ? a.ClaimApplicationTypeId : null,
		ClaimApplicationType = filter.ClaimApplicationTypeId.HasValue ? (a.ClaimApplicationType.Translates.AsQueryable()
			.FirstOrDefault(ClaimApplicationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
			.TranslateText ?? a.ClaimApplicationType.FullName) : null,

		TreatedSum = new SummaOfClaimAppColumnsDto
		{
			LegalSumma = a.Tables.Where(x => x.ClaimResponsibleTypeId == ClaimResponsibleTypeIdConst.LEGAL_PERSON)
								 .Sum(x => (decimal)x.Owner.TotalAmount),

			YATTSumma = a.Tables.Where(x => x.ClaimResponsibleTypeId == ClaimResponsibleTypeIdConst.YATT)
								 .Sum(x => (decimal)x.Owner.TotalAmount),

			IndividualsSumma = a.Tables.Where(x => x.ClaimResponsibleTypeId == ClaimResponsibleTypeIdConst.INDIVIDUAL_PERSON)
								 .Sum(x => (decimal)x.Owner.TotalAmount),

			StateOrganizationSumma = a.Tables.Where(x => x.ClaimResponsibleTypeId == ClaimResponsibleTypeIdConst.STATE_ORGANIZATION)
								 .Sum(x => (decimal)x.Owner.TotalAmount),

			ForeignCitizenSumma = a.Tables.Where(x => x.ClaimResponsibleTypeId == ClaimResponsibleTypeIdConst.FOREIGN_CITIZEN)
								 .Sum(x => (decimal)x.Owner.TotalAmount)
		},

		UnidirectionalSum = new SummaOfClaimAppColumnsDto(0, 0, 0, 0, 0, 0) // 0 lar o'rniga hali aniq bo'lsa shartlar yoziladi.
	})
	.AsEnumerable()
	.GroupBy(a => new
	{
		a.ClaimApplicationTypeId,
		a.ClaimApplicationType,
		a.DistrictId,
		a.DistrictOrderCode,
		a.District,
		a.RegionId,
		a.RegionOrderCode,
		a.Region,
		a.ContractorId,
		a.Contractor,
		a.ContractorInn,
		a.ContractorPhoneNumber,
	})
	.Select(a => new SummaOfClaimAppDto
	{
		ClaimApplicationTypeId = a.Key.ClaimApplicationTypeId,
		ClaimApplicationType = a.Key.ClaimApplicationType,
		DistrictId = a.Key.DistrictId,
		DistrictOrderCode = a.Key.DistrictOrderCode,
		District = a.Key.District,
		RegionId = a.Key.RegionId,
		RegionOrderCode = a.Key.RegionOrderCode,
		Region = a.Key.Region,
		ContractorId = a.Key.ContractorId,
		Contractor = a.Key.Contractor,
		ContractorInn = a.Key.ContractorInn,
		ContractorPhoneNumber = a.Key.ContractorPhoneNumber,

		TotalSummaInArea = a.Sum(x => x.TreatedSum.LegalSumma)
				+ a.Sum(x => x.TreatedSum.YATTSumma)
				+ a.Sum(x => x.TreatedSum.IndividualsSumma)
				+ a.Sum(x => x.TreatedSum.StateOrganizationSumma)
				+ a.Sum(x => x.TreatedSum.ForeignCitizenSumma)

				+ a.Sum(x => x.UnidirectionalSum.LegalSumma)
				+ a.Sum(x => x.UnidirectionalSum.YATTSumma)
				+ a.Sum(x => x.UnidirectionalSum.IndividualsSumma)
				+ a.Sum(x => x.UnidirectionalSum.StateOrganizationSumma)
				+ a.Sum(x => x.UnidirectionalSum.ForeignCitizenSumma),

		TreatedSum = new SummaOfClaimAppColumnsDto
		{
			TotalSumma = a.Sum(x => x.TreatedSum.LegalSumma)
				+ a.Sum(x => x.TreatedSum.YATTSumma)
				+ a.Sum(x => x.TreatedSum.IndividualsSumma)
				+ a.Sum(x => x.TreatedSum.StateOrganizationSumma)
				+ a.Sum(x => x.TreatedSum.ForeignCitizenSumma),

			LegalSumma = a.Sum(x => x.TreatedSum.LegalSumma),
			YATTSumma = a.Sum(x => x.TreatedSum.YATTSumma),
			IndividualsSumma = a.Sum(x => x.TreatedSum.IndividualsSumma),
			StateOrganizationSumma = a.Sum(x => x.TreatedSum.StateOrganizationSumma),
			ForeignCitizenSumma = a.Sum(x => x.TreatedSum.ForeignCitizenSumma),
		},

		UnidirectionalSum = new SummaOfClaimAppColumnsDto
		{
			TotalSumma = a.Sum(x => x.UnidirectionalSum.LegalSumma)
				+ a.Sum(x => x.UnidirectionalSum.YATTSumma)
				+ a.Sum(x => x.UnidirectionalSum.IndividualsSumma)
				+ a.Sum(x => x.UnidirectionalSum.StateOrganizationSumma)
				+ a.Sum(x => x.UnidirectionalSum.ForeignCitizenSumma),

			LegalSumma = a.Sum(x => x.UnidirectionalSum.LegalSumma),
			YATTSumma = a.Sum(x => x.UnidirectionalSum.YATTSumma),
			IndividualsSumma = a.Sum(x => x.UnidirectionalSum.IndividualsSumma),
			StateOrganizationSumma = a.Sum(x => x.UnidirectionalSum.StateOrganizationSumma),
			ForeignCitizenSumma = a.Sum(x => x.UnidirectionalSum.ForeignCitizenSumma)
		}
	})
	.ToList();
			}


			if (filter.ByRegion || filter.IsSsp)
			{
				res2 = this.StorageClaimApplication(filter).Where(a => a.OrganizationId == 1)
				.Select(a => new SummaOfClaimAppDto
				{
					OrganisationId = a.OrganizationId,
					Organisation = a.Organization.ShortName,
					RegionId = 15,
					RegionOrderCode = filter.ByRegion ? a.Application.Region.OrderCode : null,
					Region = filter.ByRegion
						? a.Application.Region.Translates.AsQueryable()
							.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
							.TranslateText ?? a.Application.Region.FullName
						: null,

					DistrictId = filter.ByDistrict ? a.Application.DistrictId : null,
					DistrictOrderCode = filter.ByDistrict ? a.Application.District.OrderCode : null,
					District = filter.ByDistrict
						? a.Application.District.Translates.AsQueryable()
							.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
							.TranslateText ?? a.Application.District.FullName
						: null,

					ContractorId = filter.ByContractor ? a.Application.ContractorId : null,
					Contractor = filter.ByContractor ? a.Application.Contractor.FullName : null,
					ContractorInn = filter.ByContractor ? a.Application.Contractor.Inn : null,
					ContractorPhoneNumber = filter.ByContractor ? a.Application.Contractor.PhoneNumber : null,

					ClaimApplicationTypeId = filter.ClaimApplicationTypeId.HasValue ? a.ClaimApplicationTypeId : null,
					ClaimApplicationType = filter.ClaimApplicationTypeId.HasValue ? (a.ClaimApplicationType.Translates.AsQueryable()
						.FirstOrDefault(ClaimApplicationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
						.TranslateText ?? a.ClaimApplicationType.FullName) : null,

					TreatedSum = new SummaOfClaimAppColumnsDto
					{
						LegalSumma = a.Tables.Where(x => x.ClaimResponsibleTypeId == ClaimResponsibleTypeIdConst.LEGAL_PERSON)
											 .Sum(x => (decimal)x.Owner.TotalAmount),

						YATTSumma = a.Tables.Where(x => x.ClaimResponsibleTypeId == ClaimResponsibleTypeIdConst.YATT)
											 .Sum(x => (decimal)x.Owner.TotalAmount),

						IndividualsSumma = a.Tables.Where(x => x.ClaimResponsibleTypeId == ClaimResponsibleTypeIdConst.INDIVIDUAL_PERSON)
											 .Sum(x => (decimal)x.Owner.TotalAmount),

						StateOrganizationSumma = a.Tables.Where(x => x.ClaimResponsibleTypeId == ClaimResponsibleTypeIdConst.STATE_ORGANIZATION)
											 .Sum(x => (decimal)x.Owner.TotalAmount),

						ForeignCitizenSumma = a.Tables.Where(x => x.ClaimResponsibleTypeId == ClaimResponsibleTypeIdConst.FOREIGN_CITIZEN)
											 .Sum(x => (decimal)x.Owner.TotalAmount)
					},

					UnidirectionalSum = new SummaOfClaimAppColumnsDto(0, 0, 0, 0, 0, 0) // 0 lar o'rniga hali aniq bo'lsa shartlar yoziladi.
				})
				.AsEnumerable()
				.GroupBy(a => new
				{
					a.OrganisationId,
					a.Organisation,
					a.Contractor,
					a.ContractorId
				})
				.Select(a => new SummaOfClaimAppDto
				{
					OrganisationId = a.Key.OrganisationId,
					Region = a.Key.Organisation,
					Contractor = a.Key.Contractor,
					ContractorId = a.Key.ContractorId,

					TotalSummaInArea = a.Sum(x => x.TreatedSum.LegalSumma)
							+ a.Sum(x => x.TreatedSum.YATTSumma)
							+ a.Sum(x => x.TreatedSum.IndividualsSumma)
							+ a.Sum(x => x.TreatedSum.StateOrganizationSumma)
							+ a.Sum(x => x.TreatedSum.ForeignCitizenSumma)

							+ a.Sum(x => x.UnidirectionalSum.LegalSumma)
							+ a.Sum(x => x.UnidirectionalSum.YATTSumma)
							+ a.Sum(x => x.UnidirectionalSum.IndividualsSumma)
							+ a.Sum(x => x.UnidirectionalSum.StateOrganizationSumma)
							+ a.Sum(x => x.UnidirectionalSum.ForeignCitizenSumma),

					TreatedSum = new SummaOfClaimAppColumnsDto
					{
						TotalSumma = a.Sum(x => x.TreatedSum.LegalSumma)
							+ a.Sum(x => x.TreatedSum.YATTSumma)
							+ a.Sum(x => x.TreatedSum.IndividualsSumma)
							+ a.Sum(x => x.TreatedSum.StateOrganizationSumma)
							+ a.Sum(x => x.TreatedSum.ForeignCitizenSumma),

						LegalSumma = a.Sum(x => x.TreatedSum.LegalSumma),
						YATTSumma = a.Sum(x => x.TreatedSum.YATTSumma),
						IndividualsSumma = a.Sum(x => x.TreatedSum.IndividualsSumma),
						StateOrganizationSumma = a.Sum(x => x.TreatedSum.StateOrganizationSumma),
						ForeignCitizenSumma = a.Sum(x => x.TreatedSum.ForeignCitizenSumma),
					},

					UnidirectionalSum = new SummaOfClaimAppColumnsDto
					{
						TotalSumma = a.Sum(x => x.UnidirectionalSum.LegalSumma)
							+ a.Sum(x => x.UnidirectionalSum.YATTSumma)
							+ a.Sum(x => x.UnidirectionalSum.IndividualsSumma)
							+ a.Sum(x => x.UnidirectionalSum.StateOrganizationSumma)
							+ a.Sum(x => x.UnidirectionalSum.ForeignCitizenSumma),

						LegalSumma = a.Sum(x => x.UnidirectionalSum.LegalSumma),
						YATTSumma = a.Sum(x => x.UnidirectionalSum.YATTSumma),
						IndividualsSumma = a.Sum(x => x.UnidirectionalSum.IndividualsSumma),
						StateOrganizationSumma = a.Sum(x => x.UnidirectionalSum.StateOrganizationSumma),
						ForeignCitizenSumma = a.Sum(x => x.UnidirectionalSum.ForeignCitizenSumma)
					}
				})
				.ToList();

				res.AddRange(res2);
			}


			if (filter.ByRegion)
			{
				var regions = _unitOfWork.RegionRepository.AllAsQueryable
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.Id)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							OrderCode = a.OrderCode,
							FullName = a.Translates.AsQueryable().FirstOrDefault(RegionTranslate
								.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
								?? a.FullName
						}
					);

				foreach (var region in regions)
				{
					if (res.Select(a => a.RegionId).Contains(region.Key))
						continue;

					res.Add(new SummaOfClaimAppDto
					{
						Region = region.Value.FullName,
						RegionOrderCode = region.Value.OrderCode,
						RegionId = region.Key,
						TotalSummaInArea = 0,
						TreatedSum = new SummaOfClaimAppColumnsDto(0, 0, 0, 0, 0, 0),
						UnidirectionalSum = new SummaOfClaimAppColumnsDto(0, 0, 0, 0, 0, 0)
					});
				}

				res = res.OrderBy(a => a.RegionOrderCode).ToList();
			}

			if (filter.ByDistrict)
			{
				var districts = _unitOfWork.DistrictRepository.AllAsQueryable
					.Include(a => a.Region).ThenInclude(a => a.Translates)
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.RegionId
						&& !filter.DistrictId.HasValue || filter.DistrictId == a.Id
					)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							DistrictOrderCode = a.OrderCode,
							RegionId = a.RegionId,
							Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate
								.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
								?? a.Region.FullName,
							District = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate
								.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
								?? a.FullName
						}
					);

				foreach (var district in districts)
				{
					if (res.Select(a => a.DistrictId).Contains(district.Key))
						continue;

					res.Add(new SummaOfClaimAppDto
					{
						DistrictOrderCode = district.Value.DistrictOrderCode,
						Region = district.Value.Region,
						RegionId = district.Value.RegionId,
						District = district.Value.District,
						DistrictId = district.Key,
						TotalSummaInArea = 0,
						TreatedSum = new SummaOfClaimAppColumnsDto(0, 0, 0, 0, 0, 0),
						UnidirectionalSum = new SummaOfClaimAppColumnsDto(0, 0, 0, 0, 0, 0),
					});
				}

				res = res.OrderBy(a => a.DistrictOrderCode).ToList();
			}

			return res;
		}
		private IQueryable<ClaimApplication> StorageClaimApplication(ClaimApplicationReportsDtoFilter filter)
		{
			var query = _unitOfWork.Context.Set<ClaimApplication>()
				.Where(x => x.Application.StatusId != StatusIdConst.DELETED);

			query = query.Where(a =>
						(!filter.ClaimApplicationTypeId.HasValue || a.ClaimApplicationTypeId == filter.ClaimApplicationTypeId)
					 && (!filter.RegionId.HasValue || filter.RegionId == a.Application.RegionId)
					 && (!filter.DistrictId.HasValue || filter.DistrictId == a.Application.DistrictId)
					 && (!filter.ContractorId.HasValue || filter.ContractorId == a.Application.ContractorId)
			);

			return query;
		}
		public List<ClaimApplicationDto> GetClaimApplicationReport(ClaimApplicationDtoFilter filter)
		{
			List<ClaimApplicationDto> res = new();
			List<ClaimApplicationDto> res2 = new();

			var query = _unitOfWork.Context.Set<ClaimApplication>()
				.Where(x => x.Application.StatusId != StatusIdConst.DELETED);

			query = query.Where(a =>
						 (!filter.RegionId.HasValue || filter.RegionId == a.Application.RegionId)
					 && (!filter.DistrictId.HasValue || filter.DistrictId == a.Application.DistrictId)
					 && (!filter.ContractorId.HasValue || filter.ContractorId == a.Application.ContractorId)
			);

			if (filter.IsSsp == false)
			{
				res = query.Where(q => q.OrganizationId != 1)
			   .Select(a => new ClaimApplicationDto
			   {
				   RegionId = filter.ByRegion ? a.Application.RegionId : null,
				   RegionOrderCode = filter.ByRegion ? a.Application.Region.OrderCode : null,
				   Region = filter.ByRegion
					   ? a.Application.Region.Translates.AsQueryable()
						   .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
						   .TranslateText ?? a.Application.Region.FullName
					   : null,

				   DistrictId = filter.ByDistrict ? a.Application.DistrictId : null,
				   DistrictOrderCode = filter.ByDistrict ? a.Application.District.OrderCode : null,
				   District = filter.ByDistrict
					   ? a.Application.District.Translates.AsQueryable()
						   .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
						   .TranslateText ?? a.Application.District.FullName
					   : null,

				   ContractorId = filter.ByContractor ? a.Application.ContractorId : null,
				   Contractor = filter.ByContractor ? a.Application.Contractor.FullName : null,

				   TotalClaimApplicationCount = a.Application.StatusId != StatusIdConst.DELETED ? 1 : 0,

				   TotalClaimApplicationAmount = ValueTuple.Create(
											  a.CurrencyId == CurrencyIdConst.UZS ? a.TotalAmount : (decimal?)null,
											  a.CurrencyId == CurrencyIdConst.USD ? a.TotalAmount : (decimal?)null,
											  a.CurrencyId == CurrencyIdConst.EURO ? a.TotalAmount : (decimal?)null
										  ),

				   TotalEconomicCourt = a.Application.MediationPlans.FirstOrDefault(a => a.StatusId == StatusIdConst.ACCEPTED).Mediations.FirstOrDefault(b => b.StatusId == StatusIdConst.ACCEPTED && b.MediationResultId == MediationResultIdConst.NO_AGREEMANT_REACHED).ApplicationForCourts.ClaimOrganizationId == 1 != null ? 1 : 0,

				   TotalCivilCourt = a.Application.MediationPlans.FirstOrDefault(a => a.StatusId == StatusIdConst.ACCEPTED).Mediations.FirstOrDefault(b => b.StatusId == StatusIdConst.ACCEPTED && b.MediationResultId == MediationResultIdConst.NO_AGREEMANT_REACHED).ApplicationForCourts.ClaimOrganizationId == 2 != null ? 1 : 0,

				   TotalAdministrativeCourt = a.Application.MediationPlans.FirstOrDefault(a => a.StatusId == StatusIdConst.ACCEPTED).Mediations.FirstOrDefault(b => b.StatusId == StatusIdConst.ACCEPTED && b.MediationResultId == MediationResultIdConst.NO_AGREEMANT_REACHED).ApplicationForCourts.ClaimOrganizationId == 3 != null ? 1 : 0,

				   TotalAppilationCount = a.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APILATION_CASSATION ? 1 : 0,

				   TotalAppilationAmount = a.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APILATION_CASSATION ? a.TotalAmount : (decimal?)null,

				   TotalAppilationAcceptedCount = a.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APILATION_CASSATION && a.Application.StatusId == StatusIdConst.ACCEPTED ? 1 : 0,

				   TotalAppilationCanceledCount = a.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APILATION_CASSATION && a.Application.StatusId == StatusIdConst.CANCELED ? 1 : 0,

				   TotalRevisionCount = a.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.CLAIM_REVISION ? 1 : 0,

				   TotalRevisionAmount = a.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.CLAIM_REVISION ? a.TotalAmount : (decimal?)null,

				   TotalRevisionAcceptedCount = a.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.CLAIM_REVISION && a.Application.StatusId == StatusIdConst.ACCEPTED ? 1 : 0,

				   TotalRevisionCanceledCount = a.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.CLAIM_REVISION && a.Application.StatusId == StatusIdConst.CANCELED ? 1 : 0,

				   //TotalMediationCount = a.Application.MediationPlans.FirstOrDefault(a => a.StatusId == StatusIdConst.ACCEPTED).Mediations.FirstOrDefault(b => b.MediationResultId == MediationResultIdConst.AGREEMANT_REACHED) != null ? 1 : 0,
				   TotalMediationCount = a.Application.MediationPlans.Any(mp => mp.StatusId == StatusIdConst.ACCEPTED && mp.Mediations.Any(m => m.MediationResultId == MediationResultIdConst.AGREEMANT_REACHED)) ? 1 : 0
			   })
			   .AsEnumerable()
			   .GroupBy(a => new
			   {
				   a.RegionId,
				   a.RegionOrderCode,
				   a.Region,
				   a.DistrictId,
				   a.DistrictOrderCode,
				   a.District,
				   a.ContractorId,
				   a.Contractor,
			   })
			   .Select(a => new ClaimApplicationDto
			   {
				   RegionId = a.Key.RegionId,
				   Region = a.Key.Region,
				   RegionOrderCode = a.Key.RegionOrderCode,
				   DistrictId = a.Key.DistrictId,
				   DistrictOrderCode = a.Key.DistrictOrderCode,
				   District = a.Key.District,
				   ContractorId = a.Key.ContractorId,
				   Contractor = a.Key.Contractor,

				   TotalClaimApplicationCount = a.Sum(a => a.TotalClaimApplicationCount),
				   TotalClaimApplicationAmount = (a.Sum(b => b.TotalClaimApplicationAmount.Uzs), a.Sum(b => b.TotalClaimApplicationAmount.Usd), a.Sum(b => b.TotalClaimApplicationAmount.Euro)),
				   TotalEconomicCourt = a.Sum(a => a.TotalEconomicCourt),
				   TotalCivilCourt = a.Sum(a => a.TotalCivilCourt),
				   TotalAdministrativeCourt = a.Sum(a => a.TotalAdministrativeCourt),
				   TotalAppilationCount = a.Sum(a => a.TotalAppilationCount),
				   TotalAppilationAmount = a.Sum(a => a.TotalAppilationAmount),
				   TotalAppilationAcceptedCount = a.Sum(a => a.TotalAppilationAcceptedCount),
				   TotalAppilationCanceledCount = a.Sum(a => a.TotalAppilationCanceledCount),
				   TotalRevisionCount = a.Sum(a => a.TotalRevisionCount),
				   TotalRevisionAmount = a.Sum(a => a.TotalRevisionAmount),
				   TotalRevisionAcceptedCount = a.Sum(a => a.TotalRevisionAcceptedCount),
				   TotalRevisionCanceledCount = a.Sum(a => a.TotalRevisionCanceledCount),
				   TotalMediationCount = a.Sum(a => a.TotalMediationCount),
			   })
			   .ToList();
			}

			if (filter.ByRegion || filter.IsSsp)
			{
				res2 = query.Where(a => a.OrganizationId == 1)
				.Select(a => new ClaimApplicationDto
				{
					OrganisationId = a.OrganizationId,
					Organisation = a.Organization.ShortName,

					RegionId = 15,
					RegionOrderCode = filter.ByRegion ? a.Application.Region.OrderCode : null,
					Region = filter.ByRegion
						? a.Application.Region.Translates.AsQueryable()
							.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
							.TranslateText ?? a.Application.Region.FullName
						: null,

					DistrictId = filter.ByDistrict ? a.Application.DistrictId : null,
					DistrictOrderCode = filter.ByDistrict ? a.Application.District.OrderCode : null,
					District = filter.ByDistrict
						? a.Application.District.Translates.AsQueryable()
							.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
							.TranslateText ?? a.Application.District.FullName
						: null,

					ContractorId = filter.ByContractor ? a.Application.ContractorId : null,
					Contractor = filter.ByContractor ? a.Application.Contractor.FullName : null,

					TotalClaimApplicationCount = a.Application.StatusId != StatusIdConst.DELETED ? 1 : 0,

					TotalClaimApplicationAmount = ValueTuple.Create(
											   a.CurrencyId == CurrencyIdConst.UZS ? a.TotalAmount : (decimal?)null,
											   a.CurrencyId == CurrencyIdConst.USD ? a.TotalAmount : (decimal?)null,
											   a.CurrencyId == CurrencyIdConst.EURO ? a.TotalAmount : (decimal?)null
										   ),

					TotalEconomicCourt = a.Application.MediationPlans.FirstOrDefault(a => a.StatusId == StatusIdConst.ACCEPTED).Mediations.FirstOrDefault(b => b.StatusId == StatusIdConst.ACCEPTED && b.MediationResultId == MediationResultIdConst.NO_AGREEMANT_REACHED).ApplicationForCourts.ClaimOrganizationId == 1 != null ? 1 : 0,

					TotalCivilCourt = a.Application.MediationPlans.FirstOrDefault(a => a.StatusId == StatusIdConst.ACCEPTED).Mediations.FirstOrDefault(b => b.StatusId == StatusIdConst.ACCEPTED && b.MediationResultId == MediationResultIdConst.NO_AGREEMANT_REACHED).ApplicationForCourts.ClaimOrganizationId == 2 != null ? 1 : 0,

					TotalAdministrativeCourt = a.Application.MediationPlans.FirstOrDefault(a => a.StatusId == StatusIdConst.ACCEPTED).Mediations.FirstOrDefault(b => b.StatusId == StatusIdConst.ACCEPTED && b.MediationResultId == MediationResultIdConst.NO_AGREEMANT_REACHED).ApplicationForCourts.ClaimOrganizationId == 3 != null ? 1 : 0,

					TotalAppilationCount = a.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APILATION_CASSATION ? 1 : 0,

					TotalAppilationAmount = a.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APILATION_CASSATION ? a.TotalAmount : (decimal?)null,

					TotalAppilationAcceptedCount = a.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APILATION_CASSATION && a.Application.StatusId == StatusIdConst.ACCEPTED ? 1 : 0,

					TotalAppilationCanceledCount = a.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APILATION_CASSATION && a.Application.StatusId == StatusIdConst.CANCELED ? 1 : 0,

					TotalRevisionCount = a.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.CLAIM_REVISION ? 1 : 0,

					TotalRevisionAmount = a.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.CLAIM_REVISION ? a.TotalAmount : (decimal?)null,

					TotalRevisionAcceptedCount = a.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.CLAIM_REVISION && a.Application.StatusId == StatusIdConst.ACCEPTED ? 1 : 0,

					TotalRevisionCanceledCount = a.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.CLAIM_REVISION && a.Application.StatusId == StatusIdConst.CANCELED ? 1 : 0,

					//TotalMediationCount = a.Application.MediationPlans.FirstOrDefault(a => a.StatusId == StatusIdConst.ACCEPTED).Mediations.FirstOrDefault(b => b.MediationResultId == MediationResultIdConst.AGREEMANT_REACHED) != null ? 1 : 0,
					TotalMediationCount = a.Application.MediationPlans.Any(mp => mp.StatusId == StatusIdConst.ACCEPTED && mp.Mediations.Any(m => m.MediationResultId == MediationResultIdConst.AGREEMANT_REACHED)) ? 1 : 0
				}).AsEnumerable()
				 .GroupBy(a => new
				 {
					 a.OrganisationId,
					 a.Organisation,
					 a.Contractor,
					 a.ContractorId
				 })
				 .Select(a => new ClaimApplicationDto
				 {
					 OrganisationId = a.Key.OrganisationId,
					 Region = a.Key.Organisation,
					 Contractor = a.Key.Contractor,
					 ContractorId = a.Key.ContractorId,
					 TotalClaimApplicationCount = a.Sum(a => a.TotalClaimApplicationCount),
					 TotalClaimApplicationAmount = (a.Sum(b => b.TotalClaimApplicationAmount.Uzs), a.Sum(b => b.TotalClaimApplicationAmount.Usd), a.Sum(b => b.TotalClaimApplicationAmount.Euro)),
					 TotalEconomicCourt = a.Sum(a => a.TotalEconomicCourt),
					 TotalCivilCourt = a.Sum(a => a.TotalCivilCourt),
					 TotalAdministrativeCourt = a.Sum(a => a.TotalAdministrativeCourt),
					 TotalAppilationCount = a.Sum(a => a.TotalAppilationCount),
					 TotalAppilationAmount = a.Sum(a => a.TotalAppilationAmount),
					 TotalAppilationAcceptedCount = a.Sum(a => a.TotalAppilationAcceptedCount),
					 TotalAppilationCanceledCount = a.Sum(a => a.TotalAppilationCanceledCount),
					 TotalRevisionCount = a.Sum(a => a.TotalRevisionCount),
					 TotalRevisionAmount = a.Sum(a => a.TotalRevisionAmount),
					 TotalRevisionAcceptedCount = a.Sum(a => a.TotalRevisionAcceptedCount),
					 TotalRevisionCanceledCount = a.Sum(a => a.TotalRevisionCanceledCount),
					 TotalMediationCount = a.Sum(a => a.TotalMediationCount),
				 })
				 .ToList();

				res.AddRange(res2);
			}


			if (filter.ByRegion)
			{
				var regions = _unitOfWork.RegionRepository.AllAsQueryable
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.Id)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							OrderCode = a.OrderCode,
							FullName = a.Translates.AsQueryable().FirstOrDefault(RegionTranslate
								.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
								?? a.FullName
						}
					);

				foreach (var region in regions)
				{
					if (res.Select(a => a.RegionId).Contains(region.Key))
						continue;

					res.Add(new ClaimApplicationDto
					{
						Region = region.Value.FullName,
						RegionOrderCode = region.Value.OrderCode,
						RegionId = region.Key,
						TotalClaimApplicationCount = 0,
						TotalClaimApplicationAmount = (0, 0, 0),
						TotalEconomicCourt = 0,
						TotalCivilCourt = 0,
						TotalAdministrativeCourt = 0,
						TotalAppilationCount = 0,
						TotalAppilationAmount = 0,
						TotalAppilationAcceptedCount = 0,
						TotalAppilationCanceledCount = 0,
						TotalRevisionCount = 0,
						TotalRevisionAmount = 0,
						TotalRevisionAcceptedCount = 0,
						TotalRevisionCanceledCount = 0,
						TotalMediationCount = 0,
					});
				}

				res = res.OrderBy(a => a.RegionOrderCode).ToList();
			}

			if (filter.ByDistrict)
			{
				var districts = _unitOfWork.DistrictRepository.AllAsQueryable
					.Include(a => a.Region).ThenInclude(a => a.Translates)
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.RegionId
						&& !filter.DistrictId.HasValue || filter.DistrictId == a.Id
					)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							DistrictOrderCode = a.OrderCode,
							RegionId = a.RegionId,
							Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate
								.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
								?? a.Region.FullName,
							District = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate
								.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
								?? a.FullName
						}
					);

				foreach (var district in districts)
				{
					if (res.Select(a => a.DistrictId).Contains(district.Key))
						continue;

					res.Add(new ClaimApplicationDto
					{
						DistrictOrderCode = district.Value.DistrictOrderCode,
						Region = district.Value.Region,
						RegionId = district.Value.RegionId,
						District = district.Value.District,
						DistrictId = district.Key,
						TotalClaimApplicationCount = 0,
						TotalClaimApplicationAmount = (0, 0, 0),
						TotalEconomicCourt = 0,
						TotalCivilCourt = 0,
						TotalAdministrativeCourt = 0,
						TotalAppilationCount = 0,
						TotalAppilationAmount = 0,
						TotalAppilationAcceptedCount = 0,
						TotalAppilationCanceledCount = 0,
						TotalRevisionCount = 0,
						TotalRevisionAmount = 0,
						TotalRevisionAcceptedCount = 0,
						TotalRevisionCanceledCount = 0,
						TotalMediationCount = 0,
					});
				}

				res = res.OrderBy(a => a.DistrictOrderCode).ToList();
			}
			return res;
		}
		public List<ClaimApplicationReportDto> ClaimApplicationReport(ClaimApplicationDtoFilter filter)
		{
			List<ClaimApplicationReportDto> res = new();


			var repo = _unitOfWork.ApplicationForCourtRepository.ReadAsNoTracked<ApplicationForCourtListDto>();
			IQueryable<ClaimApplication> query = _unitOfWork.Context.Set<ClaimApplication>().Include(i => i.Application).ThenInclude(a => a.Region.Translates).Include(i => i.Application).ThenInclude(a => a.MediationPlans).ThenInclude(w => w.Mediations).ThenInclude(y => y.ApplicationForCourts).ThenInclude(x => x.ClaimOrganization)
				.Where(x => x.Application.StatusId != StatusIdConst.DELETED);

			query = query.Where(a =>
						 (!filter.RegionId.HasValue || filter.RegionId == a.Application.RegionId)
					 && (!filter.DistrictId.HasValue || filter.DistrictId == a.Application.DistrictId)
					 && (!filter.ContractorId.HasValue || filter.ContractorId == a.Application.ContractorId)
			);

			res = query
			  .Select(a => new ClaimApplicationReportDto
			  {

				  RegionId = filter.ByRegion ? a.Application.RegionId : null,
				  RegionOrderCode = filter.ByRegion ? a.Application.Region.OrderCode : null,
				  Region = filter.ByRegion
					  ? a.Application.Region.Translates.AsQueryable()
						  .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
						  .TranslateText ?? a.Application.Region.FullName
					  : null,

				  DistrictId = filter.ByDistrict ? a.Application.DistrictId : null,
				  DistrictOrderCode = filter.ByDistrict ? a.Application.District.OrderCode : null,
				  District = filter.ByDistrict
					  ? a.Application.District.Translates.AsQueryable()
						  .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
						  .TranslateText ?? a.Application.District.FullName
					  : null,

				  ContractorId = filter.ByContractor ? a.Application.ContractorId : null,
				  Contractor = filter.ByContractor ? a.Application.Contractor.FullName : null,

				  TotalClaimApplicationCount = a.Application.StatusId != StatusIdConst.DELETED ? 1 : 0,
				  TotalClaimApplicationAmount = ValueTuple.Create(
											  a.CurrencyId == CurrencyIdConst.UZS ? a.TotalAmount : (decimal?)null,
											  a.CurrencyId == CurrencyIdConst.USD ? a.TotalAmount : (decimal?)null,
											  a.CurrencyId == CurrencyIdConst.EURO ? a.TotalAmount : (decimal?)null
										  ),


				  TotalEconomicCourt = (a.Application.MediationPlans.Count() != 0 && a.Application.MediationPlans.FirstOrDefault().Mediations.Count() != 0 && a.Application.MediationPlans.FirstOrDefault().Mediations.FirstOrDefault().ApplicationForCourts != null && a.Application.MediationPlans.FirstOrDefault().Mediations.FirstOrDefault().ApplicationForCourts.ClaimOrganizationId == 1) ? 1 : 0,

				  TotalCivilCourt = (a.Application.MediationPlans.Count() != 0 && a.Application.MediationPlans.FirstOrDefault().Mediations.Count() != 0 && a.Application.MediationPlans.FirstOrDefault().Mediations.FirstOrDefault().ApplicationForCourts != null && a.Application.MediationPlans.FirstOrDefault().Mediations.FirstOrDefault().ApplicationForCourts.ClaimOrganizationId == 2) ? 1 : 0,

				  TotalAdministrativeCourt = (a.Application.MediationPlans.Count() != 0 && a.Application.MediationPlans.FirstOrDefault().Mediations.Count() != 0 && a.Application.MediationPlans.FirstOrDefault().Mediations.FirstOrDefault().ApplicationForCourts != null && a.Application.MediationPlans.FirstOrDefault().Mediations.FirstOrDefault().ApplicationForCourts.ClaimOrganizationId == 3) ? 1 : 0,

				  //LeganClaims = 0,
				  //SatisfiedClaims  = 0,
				  //CanceledClaims  = a.Application.StatusId == StatusIdConst.CANCELED || a.Application.StatusId == StatusIdConst.REVOKED ? 1 : 0,
				  //RejectedClaims  = a.Application.StatusId == StatusIdConst.REJECTED ? 1 : 0,

				  TotalAppilationCount = a.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APILATION_CASSATION ? 1 : 0,

				  TotalAppilationAmount = a.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APILATION_CASSATION ? a.TotalAmount : (decimal?)null,

				  TotalAppilationAcceptedCount = 0, // integratsiyadan keyin yoziladi,

				  // TotalAppilationRejectedCount = a.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APILATION_CASSATION && a.Application.StatusId == StatusIdConst.REJECTED ? 1 : 0,
				  TotalMediationCount = a.Application.MediationPlans.Any(mp => mp.StatusId == StatusIdConst.ACCEPTED && mp.Mediations.Any(m => m.MediationResultId == MediationResultIdConst.AGREEMANT_REACHED)) ? 1 : 0
			  }).AsEnumerable()
			  .GroupBy(a => new
			  {
				  a.RegionId,
				  a.RegionOrderCode,
				  a.Region,
				  a.DistrictId,
				  a.DistrictOrderCode,
				  a.District,
				  a.ContractorId,
				  a.Contractor,
			  })
			  .Select(a => new ClaimApplicationReportDto
			  {
				  RegionId = a.Key.RegionId,
				  Region = a.Key.Region,
				  RegionOrderCode = a.Key.RegionOrderCode,
				  DistrictId = a.Key.DistrictId,
				  DistrictOrderCode = a.Key.DistrictOrderCode,
				  District = a.Key.District,
				  ContractorId = a.Key.ContractorId,
				  Contractor = a.Key.Contractor,

				  //LeganClaims = a.Sum(b=>b.LeganClaims),
				  //SatisfiedClaims = a.Sum(b=>b.SatisfiedClaims),
				  //RejectedClaims = a.Sum(b=>b.RejectedClaims),
				  //CanceledClaims = a.Sum(b=>b.CanceledClaims),
				  TotalClaimApplicationCount = a.Sum(a => a.TotalClaimApplicationCount),
				  TotalClaimApplicationAmount = (a.Sum(b => b.TotalClaimApplicationAmount.Uzs), a.Sum(b => b.TotalClaimApplicationAmount.Usd), a.Sum(b => b.TotalClaimApplicationAmount.Euro)),
				  TotalEconomicCourt = a.Sum(a => a.TotalEconomicCourt),
				  TotalCivilCourt = a.Sum(a => a.TotalCivilCourt),
				  TotalAdministrativeCourt = a.Sum(a => a.TotalAdministrativeCourt),
				  TotalAppilationCount = a.Sum(a => a.TotalAppilationCount),
				  TotalAppilationAmount = a.Sum(a => a.TotalAppilationAmount),
				  TotalAppilationAcceptedCount = a.Sum(a => a.TotalAppilationAcceptedCount),
				  // TotalAppilationRejectedCount = a.Sum(a => a.TotalAppilationRejectedCount),

				  TotalMediationCount = a.Sum(a => a.TotalMediationCount),
			  })
			  .ToList();


			if (filter.ByRegion)
			{
				var regions = _unitOfWork.RegionRepository.AllAsQueryable
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.Id)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							OrderCode = a.OrderCode,
							FullName = a.Translates.AsQueryable().FirstOrDefault(RegionTranslate
								.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
								?? a.FullName
						}
					);

				foreach (var region in regions)
				{
					if (res.Select(a => a.RegionId).Contains(region.Key))
						continue;

					res.Add(new ClaimApplicationReportDto
					{
						Region = region.Value.FullName,
						RegionOrderCode = region.Value.OrderCode,
						RegionId = region.Key,
						TotalClaimApplicationCount = 0,
						TotalClaimApplicationAmount = (0, 0, 0),
						TotalEconomicCourt = 0,
						TotalCivilCourt = 0,
						TotalAdministrativeCourt = 0,
						TotalAppilationCount = 0,
						TotalAppilationAmount = 0,
						TotalAppilationAcceptedCount = 0,
						//  TotalAppilationRejectedCount = 0,
						TotalMediationCount = 0,
					});
				}

				res = res.OrderBy(a => a.RegionOrderCode).ToList();
			}

			if (filter.ByDistrict)
			{
				var districts = _unitOfWork.DistrictRepository.AllAsQueryable
					.Include(a => a.Region).ThenInclude(a => a.Translates)
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.RegionId
						&& !filter.DistrictId.HasValue || filter.DistrictId == a.Id
					)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							DistrictOrderCode = a.OrderCode,
							RegionId = a.RegionId,
							Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate
								.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
								?? a.Region.FullName,
							District = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate
								.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
								?? a.FullName
						}
					);

				foreach (var district in districts)
				{
					if (res.Select(a => a.DistrictId).Contains(district.Key))
						continue;

					res.Add(new ClaimApplicationReportDto
					{
						DistrictOrderCode = district.Value.DistrictOrderCode,
						Region = district.Value.Region,
						RegionId = district.Value.RegionId,
						District = district.Value.District,
						DistrictId = district.Key,
						TotalClaimApplicationCount = 0,
						TotalClaimApplicationAmount = (0, 0, 0),
						TotalEconomicCourt = 0,
						TotalCivilCourt = 0,
						TotalAdministrativeCourt = 0,
						TotalAppilationCount = 0,
						TotalAppilationAmount = 0,
						TotalAppilationAcceptedCount = 0,
						//TotalAppilationRejectedCount = 0,
						TotalMediationCount = 0,
					});
				}

				res = res.OrderBy(a => a.DistrictOrderCode).ToList();
			}
			return res;
		}
		public List<ClaimApplicationAmountDto> GetClaimApplicationAmount(ClaimApplicationAmountDtoFilter filter)
		{
			var regions = _unitOfWork.Context.Set<Region>()
				.Include(r => r.Translates)
				.ToList();

			var calimApplication = _unitOfWork.Context.Set<ClaimApplication>()
				.Include(a => a.Application)
					.ThenInclude(c => c.Contractor)
				.Include(a => a.Application)
					.ThenInclude(r => r.Region)
					.ThenInclude(rt => rt.Translates)
				.Include(ap => ap.Application)
					.ThenInclude(d => d.District)
					.ThenInclude(dt => dt.Translates)
				.Where(a => a.Application.StatusId != StatusIdConst.DELETED);

			var result11 = calimApplication
								.Where(a =>
									(string.IsNullOrEmpty(filter.ContractorInn) || filter.ContractorInn == a.Application.Contractor.Inn) &&
									(!filter.FromDate.HasValue || a.Application.DocOn >= filter.FromDate) &&
									(!filter.ToDate.HasValue || a.Application.DocOn <= filter.ToDate) &&
									//(string.IsNullOrEmpty(filter.Pinfl) || a.Application.Contractor.Pinfl == filter.Pinfl) &&
									(!filter.ClaimThemeId.HasValue || a.ClaimThemeId == filter.ClaimThemeId)).ToList();

			if (filter.ByRegion)
			{
				var groupedApplications = calimApplication
					.GroupBy(c => new { c.Application.RegionId, c.Application.Region.FullName, c.Application.Region.OrderCode })
					.Select(group => new
					{
						RegionId = group.Key.RegionId,
						RegionFullName = group.Key.FullName,
						RegionOrderCode = group.Key.OrderCode,
						TotalClaimApplicationAmount = group.Sum(s => s.TotalAmount),
						TotalChargedAmount = group
							.Where(a => a.Application.StatusId == StatusIdConst.ACCEPTED && a.Application.CurrentStepId == StepIdConst.ACCEPT)
							.Sum(s => s.TotalAmount),
					})
					.ToList();

				var result = regions
					.GroupJoin(groupedApplications,
						region => region.Id,
						application => application.RegionId,
						(region, applications) => new { region, applications = applications.DefaultIfEmpty() })
					.SelectMany(
						x => x.applications.Select(a => new ClaimApplicationAmountDto
						{
							RegionId = x.region.Id,
							Region = x.region.FullName,
							RegionOrderCode = x.region.OrderCode,
							TotalClaimApplicationAmount = a?.TotalClaimApplicationAmount ?? 0,
							TotalChargedAmount = a?.TotalChargedAmount ?? 0,
						}))
					.OrderBy(a => a.RegionOrderCode)
					.ToList();

				return result;
			}
			else if (filter.ByDistrict)
			{
				var districts = _unitOfWork.Context.Set<District>()
						.Include(d => d.Translates)
						.Include(d => d.Region)
							.ThenInclude(r => r.Translates)
						.Where(d => d.RegionId == filter.RegionId.Value)
						.ToList();

				var filteredCalimApplications = calimApplication
						.Where(r => r.Application.RegionId == filter.RegionId.Value)
						.ToList();

				var groupedApplications = filteredCalimApplications
						.GroupBy(c => new
						{
							c.Application.RegionId,
							RegionName = c.Application.Region.FullName,
							c.Application.DistrictId,
							DistrictName = c.Application.District.FullName,
							c.Application.District.OrderCode
						})
						.Select(group => new
						{
							RegionId = group.Key.RegionId,
							RegionFullName = group.Key.RegionName,
							DistrictId = group.Key.DistrictId,
							DistrictFullName = group.Key.DistrictName,
							DistrictOrderCode = group.Key.OrderCode,
							TotalClaimApplicationAmount = group.Sum(s => s.TotalAmount),
							TotalChargedAmount = group
								.Where(a => a.Application.StatusId == StatusIdConst.ACCEPTED && a.Application.CurrentStepId == StepIdConst.ACCEPT)
								.Sum(s => s.TotalAmount),
						})
						.ToList();

				var result = districts
						.GroupJoin(groupedApplications,
							district => district.Id,
							application => application.DistrictId,
							(district, applications) => new { district, applications = applications.DefaultIfEmpty() })
						.SelectMany(
							x => x.applications.Select(a => new ClaimApplicationAmountDto
							{
								RegionId = x.district.Region.Id,
								Region = x.district.Region.FullName,
								DistrictId = x.district.Id,
								District = x.district.FullName,
								DistrictOrderCode = x.district.OrderCode,
								TotalClaimApplicationAmount = a?.TotalClaimApplicationAmount ?? 0,
								TotalChargedAmount = a?.TotalChargedAmount ?? 0,
							}))
						.OrderBy(a => a.DistrictOrderCode)
						.ToList();

				return result;
			}
			else if (filter.ByContractor)
			{
				if (filter.DistrictId.HasValue && filter.RegionId.HasValue)
				{
					calimApplication = calimApplication.Where(r => r.Application.DistrictId == filter.DistrictId.Value && r.Application.RegionId == filter.RegionId.Value);
				}

				var dataResult = calimApplication.GroupBy(c => new { c.Application.RegionId, RegionName = c.Application.Region.FullName, c.Application.DistrictId, DistrictName = c.Application.District.FullName, c.Application.ContractorId, /*c.Application.Contractor.Pinfl,*/ c.Application.Contractor.Inn, c.Application.Contractor.FullName, c.ClaimThemeId, ClaimTheme = c.ClaimTheme.FullName })
												 .Select(group => new ClaimApplicationAmountDto
												 {
													 RegionId = group.Key.RegionId,
													 Region = group.Key.RegionName,
													 DistrictId = group.Key.DistrictId,
													 District = group.Key.DistrictName,
													 Contractor = group.Key.FullName,
													 //ContractorPinfl = group.Key.Pinfl,
													 ContractorInn = group.Key.Inn,
													 ClaimTheme = group.Key.ClaimTheme,
													 ClaimThemeId = group.Key.ClaimThemeId,
													 TotalClaimApplicationAmount = group.Sum(s => s.TotalAmount),
													 TotalChargedAmount = group.Where(a => a.Application.StatusId == StatusIdConst.ACCEPTED && a.Application.CurrentStepId == StepIdConst.ACCEPT).Sum(s => s.TotalAmount),
												 })
										.ToList();
				return dataResult;
			}
			return new List<ClaimApplicationAmountDto>();
		}
		#endregion

		#region Arbitration
		public List<ArbitrationApplicationDto> GetArbitrationApplicationReport(ArbitrationApplicationDtoFilter filter)
		{
			List<ArbitrationApplicationDto> res = new();

			var query = _unitOfWork.Context.Set<ArbitrationCourtApplication>()
					   .Where(x => x.Application.StatusId != StatusIdConst.DELETED);

			query = query.Where(a =>
						 (!filter.RegionId.HasValue || filter.RegionId == a.Application.RegionId)
					 && (!filter.DistrictId.HasValue || filter.DistrictId == a.Application.DistrictId)
					 && (!filter.ContractorId.HasValue || filter.ContractorId == a.Application.ContractorId)
			);

			res = query
				.Select(a => new ArbitrationApplicationDto
				{
					RegionId = filter.ByRegion ? a.Application.RegionId : null,
					RegionOrderCode = filter.ByRegion ? a.Application.Region.OrderCode : null,
					Region = filter.ByRegion
						? a.Application.Region.Translates.AsQueryable()
							.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
							.TranslateText ?? a.Application.Region.FullName
						: null,

					DistrictId = filter.ByDistrict ? a.Application.DistrictId : null,
					DistrictOrderCode = filter.ByDistrict ? a.Application.District.OrderCode : null,
					District = filter.ByDistrict
						? a.Application.District.Translates.AsQueryable()
							.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
							.TranslateText ?? a.Application.District.FullName
						: null,

					ContractorId = filter.ByContractor ? a.Application.ContractorId : null,
					Contractor = filter.ByContractor ? a.Application.Contractor.FullName : null,

					TotalArbitrationApplicationCount = a.Application.StatusId != StatusIdConst.DELETED ? 1 : 0,

					TotalArbitrationApplicationAmount = ValueTuple.Create(
											   a.CurrencyId == CurrencyIdConst.UZS ? a.ArbitrationAmount : (decimal?)null,
											   a.CurrencyId == CurrencyIdConst.USD ? a.ArbitrationAmount : (decimal?)null,
											   a.CurrencyId == CurrencyIdConst.EURO ? a.ArbitrationAmount : (decimal?)null
										   ),

					TotalArbitrationPaidApplicationAmount = ValueTuple.Create(
											   a.CurrencyId == CurrencyIdConst.UZS ? a.ArbitrationAmount : (decimal?)null,
											   a.CurrencyId == CurrencyIdConst.USD ? a.ArbitrationAmount : (decimal?)null,
											   a.CurrencyId == CurrencyIdConst.EURO ? a.ArbitrationAmount : (decimal?)null
										   ),

					TotalArbitrationLatePaidApplicationAmount = ValueTuple.Create(
											   a.CanByDivided == true && a.CurrencyId == CurrencyIdConst.UZS ? a.ArbitrationAmount : (decimal?)null,
											   a.CanByDivided == true && a.CurrencyId == CurrencyIdConst.USD ? a.ArbitrationAmount : (decimal?)null,
											   a.CanByDivided == true && a.CurrencyId == CurrencyIdConst.EURO ? a.ArbitrationAmount : (decimal?)null
										   ),

					TotalArbitrationAcceptedCount = a.ArbitrationResult.StatusId == StatusIdConst.SIGNED && a.ArbitrationResult.CanByDivided ? 1 : 0,

					TotalArbitrationPartiallyAcceptedCount = a.ArbitrationResult.StatusId == StatusIdConst.SIGNED && (!a.ArbitrationResult.CanByDivided) ? 1 : 0,

					TotalArbitrationCanceledCount = a.Application.StatusId == StatusIdConst.CANCELED ? 1 : 0,

					TotalArbitrationAcceptedAmount = ValueTuple.Create(
											  a.Application.StatusId == StatusIdConst.ACCEPTED && a.CurrencyId == CurrencyIdConst.UZS ? a.ArbitrationAmount : (decimal?)null,
											  a.Application.StatusId == StatusIdConst.ACCEPTED && a.CurrencyId == CurrencyIdConst.USD ? a.ArbitrationAmount : (decimal?)null,
											  a.Application.StatusId == StatusIdConst.ACCEPTED && a.CurrencyId == CurrencyIdConst.EURO ? a.ArbitrationAmount : (decimal?)null
										   ),

					TotalArbitrationPartiallyAcceptedAmount = ValueTuple.Create(
											  a.Application.CurrentStepId == StepIdConst.COURT_DECISION && a.ArbitrationResult.CanByDivided && a.CurrencyId == CurrencyIdConst.UZS ? a.ArbitrationAmount : (decimal?)null,
											  a.Application.CurrentStepId == StepIdConst.COURT_DECISION && a.ArbitrationResult.CanByDivided && a.CurrencyId == CurrencyIdConst.USD ? a.ArbitrationAmount : (decimal?)null,
											  a.Application.CurrentStepId == StepIdConst.COURT_DECISION && a.ArbitrationResult.CanByDivided && a.CurrencyId == CurrencyIdConst.EURO ? a.ArbitrationAmount : (decimal?)null
										   ),

					TotalArbitrationCanceledAmount = ValueTuple.Create(
											   a.Application.StatusId == StatusIdConst.CANCELED && a.CurrencyId == CurrencyIdConst.UZS ? a.ArbitrationAmount : (decimal?)null,
											   a.Application.StatusId == StatusIdConst.CANCELED && a.CurrencyId == CurrencyIdConst.USD ? a.ArbitrationAmount : (decimal?)null,
											   a.Application.StatusId == StatusIdConst.CANCELED && a.CurrencyId == CurrencyIdConst.EURO ? a.ArbitrationAmount : (decimal?)null
										   ),

				})
				.AsEnumerable()
				.GroupBy(a => new
				{
					a.RegionId,
					a.RegionOrderCode,
					a.Region,
					a.DistrictId,
					a.DistrictOrderCode,
					a.District,
					a.ContractorId,
					a.Contractor,
				})
				.Select(a => new ArbitrationApplicationDto
				{
					RegionId = a.Key.RegionId,
					Region = a.Key.Region,
					RegionOrderCode = a.Key.RegionOrderCode,
					DistrictId = a.Key.DistrictId,
					DistrictOrderCode = a.Key.DistrictOrderCode,
					District = a.Key.District,
					ContractorId = a.Key.ContractorId,
					Contractor = a.Key.Contractor,

					TotalArbitrationApplicationCount = a.Sum(a => a.TotalArbitrationApplicationCount),

					TotalArbitrationApplicationAmount = (a.Sum(b => b.TotalArbitrationApplicationAmount.Uzs), a.Sum(b => b.TotalArbitrationApplicationAmount.Usd), a.Sum(b => b.TotalArbitrationApplicationAmount.Euro)),

					TotalArbitrationPaidApplicationAmount = (a.Sum(b => b.TotalArbitrationPaidApplicationAmount.Uzs), a.Sum(b => b.TotalArbitrationPaidApplicationAmount.Usd), a.Sum(b => b.TotalArbitrationPaidApplicationAmount.Euro)),

					TotalArbitrationLatePaidApplicationAmount = (a.Sum(b => b.TotalArbitrationLatePaidApplicationAmount.Uzs), a.Sum(b => b.TotalArbitrationLatePaidApplicationAmount.Usd), a.Sum(b => b.TotalArbitrationLatePaidApplicationAmount.Euro)),

					TotalArbitrationAcceptedCount = a.Sum(a => a.TotalArbitrationAcceptedCount),

					TotalArbitrationPartiallyAcceptedCount = a.Sum(a => a.TotalArbitrationPartiallyAcceptedCount),

					TotalArbitrationCanceledCount = a.Sum(a => a.TotalArbitrationCanceledCount),

					TotalArbitrationAcceptedAmount = (a.Sum(b => b.TotalArbitrationAcceptedAmount.Uzs), a.Sum(b => b.TotalArbitrationAcceptedAmount.Usd), a.Sum(b => b.TotalArbitrationAcceptedAmount.Euro)),

					TotalArbitrationPartiallyAcceptedAmount = (a.Sum(b => b.TotalArbitrationPartiallyAcceptedAmount.Uzs), a.Sum(b => b.TotalArbitrationPartiallyAcceptedAmount.Usd), a.Sum(b => b.TotalArbitrationPartiallyAcceptedAmount.Euro)),

					TotalArbitrationCanceledAmount = (a.Sum(b => b.TotalArbitrationCanceledAmount.Uzs), a.Sum(b => b.TotalArbitrationCanceledAmount.Usd), a.Sum(b => b.TotalArbitrationCanceledAmount.Euro)),
				})
				.ToList();

			if (filter.ByRegion)
			{
				var regions = _unitOfWork.RegionRepository.AllAsQueryable
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.Id)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							OrderCode = a.OrderCode,
							FullName = a.Translates.AsQueryable().FirstOrDefault(RegionTranslate
								.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
								?? a.FullName
						}
					);

				foreach (var region in regions)
				{
					if (res.Select(a => a.RegionId).Contains(region.Key))
						continue;

					res.Add(new ArbitrationApplicationDto
					{
						Region = region.Value.FullName,
						RegionOrderCode = region.Value.OrderCode,
						RegionId = region.Key,
						TotalArbitrationApplicationCount = 0,
						TotalArbitrationApplicationAmount = (0, 0, 0),
						TotalArbitrationPaidApplicationAmount = (0, 0, 0),
						TotalArbitrationLatePaidApplicationAmount = (0, 0, 0),
						TotalArbitrationAcceptedCount = 0,
						TotalArbitrationPartiallyAcceptedCount = 0,
						TotalArbitrationCanceledCount = 0,
						TotalArbitrationAcceptedAmount = (0, 0, 0),
						TotalArbitrationPartiallyAcceptedAmount = (0, 0, 0),
						TotalArbitrationCanceledAmount = (0, 0, 0),
					});
				}

				res = res.OrderBy(a => a.RegionOrderCode).ToList();
			}

			if (filter.ByDistrict)
			{
				var districts = _unitOfWork.DistrictRepository.AllAsQueryable
					.Include(a => a.Region).ThenInclude(a => a.Translates)
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.RegionId
						&& !filter.DistrictId.HasValue || filter.DistrictId == a.Id
					)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							DistrictOrderCode = a.OrderCode,
							RegionId = a.RegionId,
							Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate
								.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
								?? a.Region.FullName,
							District = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate
								.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
								?? a.FullName
						}
					);

				foreach (var district in districts)
				{
					if (res.Select(a => a.DistrictId).Contains(district.Key))
						continue;

					res.Add(new ArbitrationApplicationDto
					{
						DistrictOrderCode = district.Value.DistrictOrderCode,
						Region = district.Value.Region,
						RegionId = district.Value.RegionId,
						District = district.Value.District,
						DistrictId = district.Key,
						TotalArbitrationApplicationCount = 0,
						TotalArbitrationApplicationAmount = (0, 0, 0),
						TotalArbitrationPaidApplicationAmount = (0, 0, 0),
						TotalArbitrationLatePaidApplicationAmount = (0, 0, 0),
						TotalArbitrationAcceptedCount = 0,
						TotalArbitrationPartiallyAcceptedCount = 0,
						TotalArbitrationCanceledCount = 0,
						TotalArbitrationAcceptedAmount = (0, 0, 0),
						TotalArbitrationPartiallyAcceptedAmount = (0, 0, 0),
						TotalArbitrationCanceledAmount = (0, 0, 0),
					});
				}

				res = res.OrderBy(a => a.DistrictOrderCode).ToList();
			}
			return res;
		}
		#endregion

		#region HRM
		public EmployeeCardDto GetEmployeeCard(EmployeeCardDtoFilter dto)
		{
			var person = _unitOfWork.Context.Set<Person>().Select(a => new
			{
				a.Id,
				a.Pinfl,
				a.StateId,
				StateName = a.State.FullName,
				a.GenderId,
				a.PassportSeria,
				a.PassportNumber,
				a.BirthDate,
				GenderName = a.Gender.FullName,
				NationalityName = a.Nationality.FullName,
				a.CitizenshipId,
				CitizenshipName = a.Citizenship.FullName,
				BirthCountryName = a.BirthCountry.FullName,
				BirthRegionName = a.BirthRegion.FullName,
				a.FullName,
				a.ShortName,
				LivingRegionName = a.LivingRegion.FullName,
				LivingDistrictName = a.LivingDistrict.FullName,
				a.PictureId
			}).FirstOrDefault(a => a.Pinfl == dto.Pinfl);

			var emoployeeTables = _unitOfWork.Context.Set<Employee>()
				.Include(a => a.Relatives)
				.ThenInclude(a => a.Region)
				.Include(a => a.Relatives)
				.ThenInclude(a => a.Nationality)
				.Include(a => a.Relatives)
				.ThenInclude(a => a.District)
				.Include(a => a.Relatives)
				.ThenInclude(a => a.Citizenship)
				.Include(a => a.Relatives)
				.ThenInclude(a => a.Country)
				.Include(a => a.Relatives)
				.ThenInclude(a => a.RelativeDegree)
				.ThenInclude(b => b.Translates)
				.Include(a => a.Relatives)
				.ThenInclude(a => a.Nationality)
				.Include(a => a.HigherEdu)
				.ThenInclude(a => a.Institute)
				.Include(a => a.HigherEdu)
				.ThenInclude(a => a.Specialty)
				.Include(a => a.HigherEdu)
				.ThenInclude(a => a.EmployeeHigherEduDegrees)
				.Include(a => a.PlaceOfWorks)
				.ThenInclude(a => a.EmploymentType)
				.Include(a => a.AcademicDegrees)
				.ThenInclude(a => a.AcademicDegree)
				.Include(a => a.DegreeTitles)
				.ThenInclude(a => a.DegreeTitle)
				.Include(a => a.ElectionMembers)
				.ThenInclude(a => a.ElectionMember)
				.Include(a => a.LanguageProficiencys)
				.ThenInclude(a => a.Languagperoficiency)
				.Include(a => a.LanguageProficiencys)
				.ThenInclude(a => a.LanguageDegrees)
				.Include(a => a.Partisanships)
				.ThenInclude(a => a.Partisanship)
				.Include(a => a.ScientificDegrees)
				.ThenInclude(a => a.ScientificDegree)
				.Include(a => a.StateAwards)
				.ThenInclude(a => a.StateAwards)
				.Include(a => a.MilitaryRanks)
				.ThenInclude(a => a.MilitaryRanks)
				.FirstOrDefault(a => a.PersonId == person.Id && a.StateId == StateIdConst.ACTIVE);

			if (person == null || HasErrors)
				return null;

			var employeeManage = _unitOfWork.Context.Set<EmployeeManage>()
				.Include(d => d.Department)
				.ThenInclude(d => d.Translates)
				.Include(p => p.Position)
				.ThenInclude(p => p.Translates)
				.FirstOrDefault(a => a.EmployeeId == emoployeeTables.Id && a.EndOn == null && !a.IsDeleted);

			EmployeeCardDto employeeCard = new EmployeeCardDto();

			employeeCard.Id = person.Id;
			employeeCard.StateId = person.StateId;
			employeeCard.State = person.StateName;
			employeeCard.Gender = person.GenderName;
			employeeCard.Nationality = person.NationalityName;
			employeeCard.Citizenship = person.CitizenshipName;
			employeeCard.BirthCountry = person.BirthCountryName;
			employeeCard.BirthRegion = person.BirthRegionName;
			employeeCard.BirthDate = person.BirthDate;
			employeeCard.PassportSeria = person.PassportSeria;
			employeeCard.PassportNumber = person.PassportNumber;
			employeeCard.Pinfl = person.Pinfl;
			employeeCard.FullName = person.FullName;
			employeeCard.ShortName = person.ShortName;
			employeeCard.LivingRegion = person.LivingRegionName;
			employeeCard.LivingDistrict = person.LivingDistrictName;
			employeeCard.PictureId = person.PictureId;

			employeeCard.Department = employeeManage?.Department?.Translates.AsQueryable()
	.FirstOrDefault(DepartmentTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? employeeManage?.Department?.FullName;
			employeeCard.Position = employeeManage?.Position?.Translates.AsQueryable()
				.FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? employeeManage?.Position?.FullName;



			employeeCard.PlaceOfWorks = emoployeeTables.PlaceOfWorks.Select(a => new EmployeePlaceOfWorkDto
			{
				ContractorId = a.ContractorId,
				Contractor = a.ContractorName,
				EmploymentType = a.EmploymentType.FullName,
				PositionName = a.PositionName,
				DepartmentName = a.DepartmentName,
				EmploymentTypeId = a.EmploymentTypeId,
				StartOn = a.StartOn,
				EndOn = a.EndOn,
				Id = a.Id,
				AdditionId = a.AdditionId,
			}).OrderBy(a => a.StartOn).ToList();
			employeeCard.Relatives = emoployeeTables.Relatives.Select(a => new EmployeeRelativeDto
			{
				Nationality = a.Nationality != null ? a.Nationality.FullName : null,
				RelativeDegree = a.RelativeDegree != null ? a.RelativeDegree.Translates.AsQueryable()
				.FirstOrDefault(RelativeDegreeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.RelativeDegree.FullName : null,
				FamilyName = a.FamilyName,
				FirstName = a.FirstName,
				LastName = a.LastName,
				ShortName = a.ShortName,
				FullName = a.FullName,
				Pinfl = a.Pinfl,
				HasDied = a.HasDied,
				DateOfBirth = a.DateOfBirth,
				Address = a.Address,
				RelativeWorkPlace = a.RelativeWorkPlace,
				DocumentSeries = a.DocumentSeries,
				DocumentNumber = a.DocumentNumber,
				IssueOrganization = a.IssueOrganization,
				DateOfIssue = a.DateOfIssue,
				Citizenship = a.Citizenship != null ? a.Citizenship.FullName : null,
				Country = a.Country != null ? a.Country.FullName : null,
				Region = a.Region != null ? a.Region.FullName : null,
				District = a.District != null ? a.District.FullName : null
			}).ToList();
			employeeCard.HigherEdu = emoployeeTables.HigherEdu.Select(a => new EmployeeHigherEduDto
			{
				Specialty = a.Specialty.FullName,
				Institute = a.Institute.FullName,
				InstituteName = a.InstituteName,
				DateOfIssue = a.DateOfIssue,
				DocumentSeries = a.DocumentSeries,
				DocumentNumber = a.DocumentNumber,
				EmployeeHigherEduDegree = a.EmployeeHigherEduDegrees.Translates.AsQueryable().FirstOrDefault(EmployeeHigherEduDegreeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.EmployeeHigherEduDegrees.FullName
			}).ToList();
			employeeCard.AcademicDegrees = emoployeeTables.AcademicDegrees.Select(a => new EmployeeAcademicDegreeDto
			{
				AcademicDegree = a.AcademicDegree.FullName
			}).ToList();
			employeeCard.DegreeTitles = emoployeeTables.DegreeTitles.Select(a => new EmployeeDegreeTitleDto
			{
				DegreeTitle = a.DegreeTitle.FullName,
				Year = a.Year
			}).ToList();
			employeeCard.ElectionMembers = emoployeeTables.ElectionMembers.Select(a => new EmployeeElectionMemberDto
			{
				ElectionMember = a.ElectionMember.FullName,
				Year = a.Year
			}).ToList();
			employeeCard.LanguageProficiencys = emoployeeTables.LanguageProficiencys.Select(a => new EmployeeLanguageProficiencyDto
			{
				LanguageDegree = a.LanguageDegree,
				Languagperoficiency = a.Languagperoficiency.FullName,
				LanguageDegrees = a.LanguageDegrees?.FullName
			}).ToList();
			employeeCard.Partisanships = emoployeeTables.Partisanships.Select(a => new EmployeePartisanshipDto
			{
				Partisanship = a.Partisanship.FullName,
				Year = a.Year
			}).ToList();
			employeeCard.ScientificDegrees = emoployeeTables.ScientificDegrees.Select(a => new EmployeeScientificDegreeDto
			{
				ScientificDegree = a.ScientificDegree.FullName,
			}).ToList();
			employeeCard.StateAwards = emoployeeTables.StateAwards.Select(a => new EmployeeStateAwardDto
			{
				StateAwards = a.StateAwards.FullName
			}).ToList();
			employeeCard.MilitaryRanks = emoployeeTables.MilitaryRanks.Select(a => new EmployeeMilitaryRankDto
			{
				MilitaryRanks = a.MilitaryRanks.FullName
			}).ToList();
			return employeeCard;
		}
		public List<StaffingSinglePageReportDto> GetStaffingSingleReport(StaffingSinglePageReportDtoFilter dto)
		{
            bool ignoreOrganizationFilter = _authService.Organization.Id == OrganizationIdConst.SSP;
            var staffingPositions = _unitOfWork.Context.Set<StaffingPosition>()
													   .Include(a => a.Department)
													   .Include(a => a.Position)
													   .Where(a => a.Owner.StatusId == StatusIdConst.RECEIVED && a.Owner.OrganizationId == _authService.User.OrganizationId
                                                            && (dto.DepartmentId == null || a.DepartmentId == dto.DepartmentId)).OrderByDescending(a => a.Owner.DocOn)
													   .Select(a => new StaffingSinglePageReportDto
													   {
														   DepartmentId = a.DepartmentId,
														   Department = a.Department.FullName,
														   PositionId = a.PositionId,
														   Position = a.Position.FullName,
														   Quantity = a.Quantity,
														   QuantityForNow = a.Quantity - _unitOfWork.Context.Set<EmployeeManage>()
																											.Where(b => b.OrganizationId == _authService.User.OrganizationId &&
																																			b.EndOn == null && b.IsDeleted == false &&
																																			b.PositionId == a.PositionId &&
																																			b.DepartmentId == a.DepartmentId)
																											.Sum(a => a.EmploymentRate.Value),
														   EmployeeManageTables = _unitOfWork.Context.Set<EmployeeManage>()
																				.Include(c => c.Employee)
																				.ThenInclude(p => p.Person)
																				.Where(b => b.EndOn == null && b.IsDeleted == false && b.PositionId == a.PositionId && b.DepartmentId == a.DepartmentId && b.OrganizationId == _authService.User.OrganizationId )
																				.Where(b => b.PositionId == a.PositionId && b.DepartmentId == a.DepartmentId
																							&& (dto.HasSearch() ? b.Employee.Person.FullName.ToLower().Contains(dto.Search.ToLower()) : true))
																				.Select(b => new EmployeeManageTable
																				{
																					EmployeeId = b.EmployeeId,
																					EmployeeManageId = b.Id,
																					Pinfl = b.Employee.Person.Pinfl,
																					Employees = b.Employee.Person.FullName,
																					GenderId = b.Employee.Person.GenderId,
																					EmployeeRate = b.EmploymentRate,
																					EmployeeManageDocId = b.DocId,
																					AppointEmployees = _unitOfWork.Context.Set<AppointEmployeeTable>().Include(app => app.Owner)
																						.Where(p => p.OwnerId == b.DocId && p.Owner.StatusId == StatusIdConst.ACCEPTED && p.Employee.OrganizationId == _authService.User.OrganizationId /*&& p.EndOn == null && dto.ByQuantity == false*/)
																						.Select(p => new AppointEmployeeTables
																						{
																							DocNumber = p.Owner.DocNumber,
																							DocOn = p.Owner.DocOn,
																							CreatedAt = p.Owner.CreatedAt,
																							StatusId = p.Owner.StatusId,
																							Acting = p.Acting,
																							Interm = p.Interm,
																							IsProbation = p.IsProbation

																						}).Where(s => s.Acting == dto.Acting && s.Interm == dto.Interm && s.IsProbation == dto.IsProbation || !(dto.Interm || dto.IsProbation || dto.Acting)).ToList()
																				})
																				.Where(b => b.AppointEmployees.Any() || !(dto.Interm || dto.IsProbation || dto.Acting)).ToList(),
													   }).AsQueryable();

			return staffingPositions.SortFilter(dto).ToList();
		}
		public WEBASE.Models.PagedResult<StaffingSinglePageReportDto> GetStaffingSingleReportForParent(StaffingSinglePageReportDtoFilter dto)
		{
			bool ignoreOrganizationFilter = _authService.Organization.Id == 1;
			var staffingPositions = _unitOfWork.Context.Set<StaffingPosition>()
				.Include(a => a.Department)
				.Include(a => a.Position)
                .Where(a => a.Owner.StatusId == StatusIdConst.RECEIVED &&
												((ignoreOrganizationFilter && a.Owner.OrganizationId == dto.OrganizationId)||
												(_authService.Organization.Id == dto.OrganizationId && dto.OrganizationId == a.Owner.OrganizationId)))
                .OrderByDescending(a => a.Owner.DocOn)
				.Select(a => new StaffingSinglePageReportDto
				{
					DepartmentId = a.DepartmentId,
					Department = a.Department.FullName,
					PositionId = a.PositionId,
					Position = a.Position.FullName,
					Quantity = a.Quantity,
					OrganizationId = a.Owner.OrganizationId,
					QuantityForNow = a.Quantity - _unitOfWork.Context.Set<EmployeeManage>()
					.Where(b => (!dto.OrganizationId.HasValue || dto.OrganizationId == b.OrganizationId) && b.EndOn == null && b.IsDeleted == false && b.PositionId == a.PositionId && b.DepartmentId == a.DepartmentId).Sum(a => a.EmploymentRate.Value),
					EmployeeManageTables = _unitOfWork.Context.Set<EmployeeManage>()
					.Where(b => (!dto.OrganizationId.HasValue || dto.OrganizationId == b.OrganizationId)
						&& b.EndOn == null && b.IsDeleted == false
						&& b.PositionId == a.PositionId
						&& b.DepartmentId == a.DepartmentId)
						.Where(x => dto.HasSearch() ? x.Employee.Person.FullName.ToLower().Contains(dto.Search.ToLower()) : true)
						.Select(b => new EmployeeManageTable
						{
							EmployeeId = b.EmployeeId,
							EmployeeManageId = b.Id,
							Pinfl = b.Employee.Person.Pinfl,
							Employees = b.Employee.Person.FullName,
							EmployeeRate = b.EmploymentRate,
							EmployeeManageDocId = b.DocId,
							GenderId = b.Employee.Person.GenderId,
							AppointEmployees = _unitOfWork.Context.Set<AppointEmployeeTable>().Include(app => app.Owner).Where(p => p.OwnerId == b.DocId && p.Owner.StatusId == StatusIdConst.ACCEPTED).Select(p => new AppointEmployeeTables
							{
								DocNumber = p.Owner.DocNumber,
								DocOn = p.Owner.DocOn,
								CreatedAt = p.Owner.CreatedAt,
								StatusId = p.Owner.StatusId,
								Acting = p.Acting,
								Interm = p.Interm,
								IsProbation = p.IsProbation
							}).ToList()
						}).ToList(),
				}).AsQueryable();

			return staffingPositions.SortFilter(dto).AsPagedResult(dto);
		}
		public List<OffertaCalculateInfoDto> GetOffertaCalculate(OffertaCalculateInfoDtoFilter filter)
		{
			/*var region = _unitOfWork.Context.Set<Region>().Where(a => a.StateId == StateIdConst.ACTIVE);
            var query = _unitOfWork.Context.Set<Contractor>()
                .Include(a => a.Region)
                .Include(a => a.District)
                .Where(a => a.StateId == StateIdConst.ACTIVE).ToList();*/

			var query = _unitOfWork.Context.Set<Contractor>().IsActive()
				.Where(con => (filter.DistrictId == null || filter.DistrictId == con.DistrictId) &&
							  (filter.RegionId == null || filter.RegionId == con.RegionId) && (filter.ContractorId == null || filter.ContractorId == con.Id))
				/*.Select(con => new OffertaCalculateInfoDto
                {
                    RegionId = con.RegionId,
                    Region = con.Region.FullName,
                    No = con.Where(nc => !nc.IsLastOffer).Count(),
                    Yes = groupedRes.Where(nc => nc.IsLastOffer).Count(),
                    TotalDocCount = groupedRes.Count()
                })*/;

			var result = new List<OffertaCalculateInfoDto>();

			if (filter.ByRegion || (!filter.ByRegion && !filter.ByDistrict))
			{
				result = (from con in query
						  group con by new
						  {
							  con.RegionId,
							  con.Region.OrderCode,
							  RegionName = con.Region.FullName,
						  } into groupedRes
						  select new OffertaCalculateInfoDto
						  {
							  RegionId = groupedRes.Key.RegionId,
							  Region = groupedRes.Key.RegionName,
							  RegionOrderCode = groupedRes.Key.OrderCode,
							  No = groupedRes.Where(nc => !nc.IsLastOffer).Count(),
							  Yes = groupedRes.Where(nc => nc.IsLastOffer).Count(),
							  TotalDocCount = groupedRes.Count()
						  }).OrderBy(a => a.RegionOrderCode)
							.ToList();
			}

			if (filter.ByDistrict)
			{
				result = (from con in query
						  group con by new
						  {
							  OrderCode = con.District.OrderCode,
							  DistrictId = con.DistrictId,
							  DistrictName = con.District.FullName,
						  } into groupedRes
						  select new OffertaCalculateInfoDto
						  {
							  RegionOrderCode = groupedRes.Key.OrderCode,
							  DistrictId = groupedRes.Key.DistrictId,
							  District = groupedRes.Key.DistrictName,
							  No = groupedRes.Where(nc => !nc.IsLastOffer).Count(),
							  Yes = groupedRes.Where(nc => nc.IsLastOffer).Count(),
							  TotalDocCount = groupedRes.Count()
						  }).OrderBy(a => a.RegionOrderCode)
							.ToList();
			}
			if (filter.ByContractor)
			{
				result = (from con in query
						  group con by new
						  {
							  Id = con.Id,
							  ContractorInn = con.Inn,
							  ContractName = con.FullName,
						  } into groupedRes
						  select new OffertaCalculateInfoDto
						  {
							  ContractorId = groupedRes.Key.Id,
							  Contractor = groupedRes.Key.ContractName,
							  ContractorInn = groupedRes.Key.ContractorInn,
							  No = groupedRes.Where(nc => !nc.IsLastOffer).Count(),
							  Yes = groupedRes.Where(nc => nc.IsLastOffer).Count(),
							  TotalDocCount = groupedRes.Count()
						  }).OrderBy(a => a.Contractor)
							.ToList();
			}



			return result;
		}
		public List<StaffCountDto> GetStaffCountReport(StaffCountDtoFilter filter)
		{
			var result = new List<StaffCountDto>();

			var staffQuery = _unitOfWork.Context.Set<Staffing>()
				.Where(a => a.StatusId == StatusIdConst.RECEIVED
				&& (a.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP
				|| a.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH));

			if (filter.RegionId.HasValue)
				staffQuery = staffQuery.Where(a => a.Organization.RegionId == filter.RegionId);
			if (filter.OrganizationId.HasValue)
				staffQuery = staffQuery.Where(a => a.OrganizationId == filter.OrganizationId);

			if (filter.FromDate.HasValue && filter.ToDate.HasValue)
				staffQuery = staffQuery.Where(x => x.DocOn >= filter.FromDate && x.DocOn <= filter.ToDate);
			else if (filter.FromDate.HasValue)
				staffQuery = staffQuery.Where(x => x.DocOn >= filter.FromDate);
			else if (filter.ToDate.HasValue)
				staffQuery = staffQuery.Where(x => x.DocOn <= filter.ToDate);

			//var staffIds = staffQuery
			//	.Select(a => a.OrganizationId)
			//	.Distinct()
			//	.Select(orgId => staffQuery
			//		.Where(a => orgId == a.OrganizationId)
			//		.OrderBy(a => a.Id)
			//		.Select(a => a.Id))
			//	.ToArray();

			var staffIds = staffQuery
								 .Select(a => a.OrganizationId)
								 .Distinct()
								 .Select(orgId => staffQuery
									 .Where(a => orgId == a.OrganizationId)
									 .OrderBy(a => a.Id)
									 .Select(a => a.Id)
									 .ToArray())
								 .SelectMany(ids => ids)
								 .ToArray();

			var query = _unitOfWork.Context.Set<StaffingPosition>().Where(sp => staffIds.Contains(sp.OwnerId));

			if (filter.DepartmentId.HasValue)
				query = query.Where(a => a.DepartmentId == filter.DepartmentId);
			if (filter.PositionId.HasValue)
				query = query.Where(a => a.PositionId == filter.PositionId);

			result = query
				.Select(a => new StaffCountDto
				{
					RegionId = filter.ByRegion ? a.Owner.Organization.RegionId : null,
					RegionOrderCode = filter.ByRegion ? a.Owner.Organization.Region.OrderCode : null,
					Region = filter.ByRegion
						? (a.Owner.Organization.Region.Translates.AsQueryable()
								.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Owner.Organization.Region.FullName)
						: null,

					OrganizationId = filter.ByOrganization ? a.Owner.OrganizationId : null,
					Organization = filter.ByOrganization ? a.Owner.Organization.ShortName : null,
					OrganizationOrderCode = filter.ByOrganization ? a.Owner.Organization.OrderCode : null,

					DepartmentId = filter.ByDepartment ? a.DepartmentId : null,
					DepartmentOrderCode = filter.ByDepartment ? a.Department.OrderCode : null,
					Department = filter.ByDepartment ? a.Department.FullName : null,

					PositionId = filter.ByPosition ? a.PositionId : null,
					PositionOrderCode = filter.ByPosition ? a.Position.OrderCode : null,
					Position = filter.ByPosition ? a.Position.FullName : null,

					TotalStaffingRate = a.Quantity,
					TotalEmployeeManageRate = _unitOfWork.Context.Set<EmployeeManage>()
					.Where(b => a.Owner.OrganizationId == b.OrganizationId && b.EndOn == null && b.IsDeleted == false && b.PositionId == a.PositionId && b.DepartmentId == a.DepartmentId)
					.Sum(a => a.EmploymentRate),
					TotalCount = a.Quantity - _unitOfWork.Context.Set<EmployeeManage>()
					.Where(b => a.Owner.OrganizationId == b.OrganizationId && b.EndOn == null && b.IsDeleted == false && b.PositionId == a.PositionId && b.DepartmentId == a.DepartmentId)
					.Sum(a => a.EmploymentRate),
				})
				.AsEnumerable()
				.GroupBy(a => new
				{
					a.RegionId,
					a.RegionOrderCode,
					a.Region,
					a.OrganizationId,
					a.Organization,
					a.OrganizationOrderCode,
					a.DepartmentId,
					a.DepartmentOrderCode,
					a.Department,
					a.PositionId,
					a.PositionOrderCode,
					a.Position
				})
				.Select(a => new StaffCountDto
				{
					RegionId = a.Key.RegionId,
					RegionOrderCode = a.Key.RegionOrderCode,
					Region = a.Key.Region,
					Organization = a.Key.Organization,
					OrganizationOrderCode = a.Key.OrganizationOrderCode,
					OrganizationId = a.Key.OrganizationId,
					DepartmentId = a.Key.DepartmentId,
					Department = a.Key.Department,
					PositionId = a.Key.PositionId,
					Position = a.Key.Position,
					TotalStaffingRate = a.Sum(a => a.TotalStaffingRate),
					TotalEmployeeManageRate = a.Sum(a => a.TotalEmployeeManageRate),
					TotalCount = a.Sum(a => a.TotalCount)
				})
				.ToList();

			// Hamma viloyatlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
			if (filter.ByRegion)
			{
				var regions = _unitOfWork.RegionRepository.AllAsQueryable
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.Id)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							OrderCode = a.OrderCode,
							FullName = a.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.FullName
						}
					);

				foreach (var region in regions)
				{
					if (result.Select(a => a.RegionId).Contains(region.Key))
						continue;

					result.Add(new StaffCountDto
					{
						Region = region.Value.FullName,
						RegionOrderCode = region.Value.OrderCode,
						RegionId = region.Key
					});
				}

				result = result.OrderBy(a => a.RegionOrderCode).ToList();
			}

			if (filter.ByOrganization)
			{
				var organizations = _unitOfWork.OrganizationRepository.AllAsQueryable
					.Include(a => a.Region)
					.IsActive()
					.Where(a => /*!filter.RegionId.HasValue || filter.RegionId == a.RegionId*/
					(!filter.OrganizationId.HasValue || filter.OrganizationId == a.Id) && (
						a.OrganizationGroupId == OrganizationGroupIdConst.SSP
						|| a.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH))
					.ToDictionary(
						a => a.Id,
						a => new
						{
							RegionId = a.RegionId,
							Region = a.Region.FullName,
							Organization = a.ShortName,
							OrganizationOrderCode = a.OrderCode
						}
					);

				foreach (var organization in organizations)
				{
					if (result.Select(a => a.OrganizationId).Contains(organization.Key))
						continue;

					result.Add(new StaffCountDto
					{
						Region = organization.Value.Region,
						RegionId = organization.Value.RegionId,
						Organization = organization.Value.Organization,
						OrganizationId = organization.Key,
						OrganizationOrderCode = organization.Value.OrganizationOrderCode

					});
				}

				result = result.OrderBy(a => a.OrganizationOrderCode).ToList();
			}
			//if (filter.ByDepartment)
			//{
			//    var departments = _unitOfWork.DepartmentRepository.AllAsQueryable
			//        .Include(a => a.Organization).ThenInclude(a => a.Translates)
			//        .IsActive()
			//        .Where(a => !filter.OrganizationId.HasValue || filter.OrganizationId == a.OrganizationId
			//            && !filter.DepartmentId.HasValue || filter.DepartmentId == a.Id && a.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP && a.StateId == StateIdConst.ACTIVE)
			//        .ToDictionary(
			//            a => a.Id,
			//            a => new
			//            {
			//                OrganizationId = a.OrganizationId,
			//                DepartmentOrderCode = a.OrderCode,
			//                Organization = a.Organization.Translates.AsQueryable().FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.Organization.FullName,
			//                Department = a.FullName
			//            }
			//        );

			//    foreach (var department in departments)
			//    {
			//        if (result.Select(a => a.DepartmentId).Contains(department.Key))
			//            continue;

			//        result.Add(new StaffCountDto
			//        {
			//            OrganizationId = department.Value.OrganizationId,
			//            Organization = department.Value.Organization,
			//            Department = department.Value.Department,
			//            DepartmentOrderCode = department.Value.DepartmentOrderCode,
			//            DepartmentId = department.Key
			//        });
			//    }
			//    result = result.OrderBy(a => a.DepartmentOrderCode).ToList();
			//}
			if (filter.ByPosition)
			{
				if (filter.PositionId.HasValue)
				{
					var positions = _unitOfWork.PositionRepository.AllAsQueryable.Where(a => !filter.PositionId.HasValue || filter.PositionId == a.Id && a.StateId == StateIdConst.ACTIVE)
						  .ToDictionary(
						  a => a.Id,
						  a => new
						  {
							  PositionId = a.Id,
							  PositionOrderCode = a.OrderCode,
							  Position = /*a.Translates.AsQueryable().FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.full_name,             ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ??*/ a.FullName,
						  });

					foreach (var position in positions)
					{
						if (result.Select(a => a.PositionId).Contains(position.Key))
							continue;

						result.Add(new StaffCountDto
						{
							Position = position.Value.Position,
							PositionOrderCode = position.Value.PositionOrderCode,
							PositionId = position.Key
						});
					}
					result = result.OrderBy(a => a.PositionOrderCode).ToList();
				}
				else
					result = result.OrderBy(a => a.PositionOrderCode).ToList();
			}
			return result;
		}
		public List<StaffCountByGenderDto> GetStaffCountByGenderReport(StaffCountByGenderDtoFilter filter)
		{
			var result = new List<StaffCountByGenderDto>();

			var staffQuery = _unitOfWork.Context.Set<Staffing>()
				.Where(a => a.StatusId == StatusIdConst.RECEIVED
				&& (a.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP
				|| a.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH));

			if (filter.RegionId.HasValue)
				staffQuery = staffQuery.Where(a => a.Organization.RegionId == filter.RegionId);
			if (filter.OrganizationId.HasValue)
				staffQuery = staffQuery.Where(a => a.OrganizationId == filter.OrganizationId);

			if (filter.FromDate.HasValue && filter.ToDate.HasValue)
				staffQuery = staffQuery.Where(x => x.DocOn >= filter.FromDate && x.DocOn <= filter.ToDate);
			else if (filter.FromDate.HasValue)
				staffQuery = staffQuery.Where(x => x.DocOn >= filter.FromDate);
			else if (filter.ToDate.HasValue)
				staffQuery = staffQuery.Where(x => x.DocOn <= filter.ToDate);


			//var staffIds = staffQuery
			//	.Select(a => a.OrganizationId)
			//	.Distinct()
			//	.Select(orgId => staffQuery
			//		.Where(a => orgId == a.OrganizationId)
			//		.OrderBy(a => a.Id)
			//		.Select(a => a.Id))
			//	.ToArray();

			var staffIds = staffQuery
								 .Select(a => a.OrganizationId)
								 .Distinct()
								 .Select(orgId => staffQuery
									 .Where(a => orgId == a.OrganizationId)
									 .OrderBy(a => a.Id)
									 .Select(a => a.Id)
									 .ToArray())
								 .SelectMany(ids => ids)
								 .ToArray();

			var query = _unitOfWork.Context.Set<StaffingPosition>().Include(a => a.Owner).ThenInclude(a => a.Organization).ThenInclude(a => a.Region)
				.Include(a => a.Position).ThenInclude(a => a.PositionCategory).Where(sp => staffIds.Contains(sp.OwnerId));

			if (filter.DepartmentId.HasValue)
				query = query.Where(a => a.DepartmentId == filter.DepartmentId);
			if (filter.PositionId.HasValue)
				query = query.Where(a => a.PositionId == filter.PositionId);

			if (!filter.PositionCategoryId.HasValue)
			{
				query = query.Where(a => a.Position.PositionCategoryId == 1 || a.Position.PositionCategoryId == 2 || a.Position.PositionCategoryId == 3 || a.Position.PositionCategoryId == 4);
			};

			result = query
				.Select(a => new StaffCountByGenderDto
				{
					RegionId = filter.ByRegion ? a.Owner.Organization.RegionId : null,
					RegionOrderCode = filter.ByRegion ? a.Owner.Organization.Region.OrderCode : null,
					Region = filter.ByRegion
						? (a.Owner.Organization.Region.Translates.AsQueryable()
								.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name,
								ServiceProvider.CultureHelper.CurrentCulture.Id))
								.TranslateText ?? a.Owner.Organization.Region.FullName)
						: null,

					OrganizationId = filter.ByOrganization ? a.Owner.OrganizationId : null,
					Organization = filter.ByOrganization ? a.Owner.Organization.ShortName : null,
					OrganizationInn = filter.ByOrganization ? a.Owner.Organization.Inn : null,
					OrganizationOrderCode = filter.ByOrganization ? a.Owner.Organization.OrderCode : null,

					DepartmentId = filter.ByDepartment ? a.DepartmentId : null,
					DepartmentOrderCode = filter.ByDepartment ? a.Department.OrderCode : null,
					Department = filter.ByDepartment ? a.Department.FullName : null,

					PositionId = filter.ByPosition ? a.PositionId : null,
					PositionOrderCode = filter.ByPosition ? a.Position.OrderCode : null,
					Position = filter.ByPosition ? a.Position.FullName : null,

					TotalStaffingRate = a.Quantity,
					TotalEmployeeManageRate = _unitOfWork.Context.Set<EmployeeManage>().Include(a => a.Organization)
					.Where(b => a.Owner.OrganizationId == b.OrganizationId
							&& (b.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP
								|| b.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)
							&& b.EndOn == null
							&& b.IsDeleted == false
							&& b.PositionId == a.PositionId
							&& b.DepartmentId == a.DepartmentId)
					.Sum(a => a.EmploymentRate),
					TotalCount = a.Quantity - _unitOfWork.Context.Set<EmployeeManage>()
					.Where(b => a.Owner.OrganizationId == b.OrganizationId
						&& (b.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP
							|| b.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)
						&& b.EndOn == null
						&& b.IsDeleted == false
						&& b.PositionId == a.PositionId
						&& b.DepartmentId == a.DepartmentId)
					.Sum(a => a.EmploymentRate),

					TotalEmployee = _unitOfWork.Context.Set<EmployeeManage>()
					.Where(b => a.Owner.OrganizationId == b.OrganizationId
						&& (b.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP
							|| b.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)
						&& b.EndOn == null
						&& b.IsDeleted == false
						&& b.PositionId == a.PositionId
						&& b.DepartmentId == a.DepartmentId).Count(),
					TotalEmployeeMen = _unitOfWork.Context.Set<EmployeeManage>().Include(a => a.Employee).ThenInclude(p => p.Person)
					.Where(b => a.Owner.OrganizationId == b.OrganizationId
						&& (b.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP
							|| b.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)
						&& b.EndOn == null
						&& b.IsDeleted == false
						&& b.PositionId == a.PositionId
						&& b.DepartmentId == a.DepartmentId
						&& b.Employee.Person.GenderId == 1).Count(),
					TotalEmployeeWomen = _unitOfWork.Context.Set<EmployeeManage>().Include(a => a.Employee).ThenInclude(p => p.Person)
					.Where(b => a.Owner.OrganizationId == b.OrganizationId
						&& (b.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP
							|| b.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)
						&& b.EndOn == null
						&& b.IsDeleted == false
						&& b.PositionId == a.PositionId
						&& b.DepartmentId == a.DepartmentId
						&& b.Employee.Person.GenderId == 2).Count(),


				})
				.AsEnumerable()
				.GroupBy(a => new
				{
					a.RegionId,
					a.RegionOrderCode,
					a.OrganizationOrderCode,
					a.Region,
					a.OrganizationId,
					a.Organization,
					a.DepartmentId,
					a.DepartmentOrderCode,
					a.Department,
					a.PositionId,
					a.PositionOrderCode,
					a.Position,
				})
				.Select(a => new StaffCountByGenderDto
				{
					RegionId = a.Key.RegionId,
					RegionOrderCode = a.Key.RegionOrderCode,
					Region = a.Key.Region,
					Organization = a.Key.Organization,
					OrganizationId = a.Key.OrganizationId,
					DepartmentId = a.Key.DepartmentId,
					Department = a.Key.Department,
					PositionId = a.Key.PositionId,
					Position = a.Key.Position,
					OrganizationOrderCode = a.Key.OrganizationOrderCode,
					PositionCategorys = query
				.GroupBy(x => new
				{
					x.Position.PositionCategoryId,
					x.Position.PositionCategory.FullName
				})
				.Select(x => new PositionCategoryDtoGendre
				{
					Count = _unitOfWork.Context.Set<EmployeeManage>()
						.Include(b => b.Employee)
						.Include(b => b.Position)
						.Include(b => b.Organization)
						.Where(b => (!a.Key.OrganizationId.HasValue || a.Key.OrganizationId == b.OrganizationId) && (!a.Key.RegionId.HasValue || a.Key.RegionId == b.Organization.RegionId) &&
									b.EndOn == null && b.IsDeleted == false &&
									(b.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP || b.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH) &&
									b.Position.PositionCategoryId == x.Key.PositionCategoryId).Count(),
					Men = _unitOfWork.Context.Set<EmployeeManage>()
						.Include(b => b.Employee)
						.ThenInclude(b => b.Person)
						.Include(b => b.Position)
						.Include(b => b.Organization)
						.Where(b => (!a.Key.OrganizationId.HasValue || a.Key.OrganizationId == b.OrganizationId)
									&& (!a.Key.RegionId.HasValue || a.Key.RegionId == b.Organization.RegionId)
									&& (b.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP || b.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)
									&& b.EndOn == null && b.IsDeleted == false
									&& b.Position.PositionCategoryId == x.Key.PositionCategoryId && b.Employee.Person.GenderId == 1)
						.Count(),
					Women = _unitOfWork.Context.Set<EmployeeManage>()
						.Include(b => b.Employee)
						.ThenInclude(b => b.Person)
						.Include(b => b.Position)
						.Include(b => b.Organization)
						.Where(b => (!a.Key.OrganizationId.HasValue || a.Key.OrganizationId == b.OrganizationId)
									&& (!a.Key.RegionId.HasValue || a.Key.RegionId == b.Organization.RegionId)
									&& (b.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP || b.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH) && b.EndOn == null
									&& b.Position.PositionCategoryId == x.Key.PositionCategoryId && b.Employee.Person.GenderId == 2)
						.Count(),
					PositionCategoryId = x.Key.PositionCategoryId,
					PositionCategory = x.Key.FullName,
				}).OrderBy(a => a.PositionCategoryId).ToList(),
					TotalStaffingRate = a.Sum(a => a.TotalStaffingRate),
					TotalEmployeeManageRate = a.Sum(a => a.TotalEmployeeManageRate),
					TotalCount = a.Sum(a => a.TotalCount),
					TotalEmployee = a.Sum(a => a.TotalEmployee),
					TotalEmployeeMen = a.Sum(a => a.TotalEmployeeMen),
					TotalEmployeeWomen = a.Sum(a => a.TotalEmployeeWomen),
				})
				.ToList();

			// Hamma viloyatlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
			if (filter.ByRegion)
			{
				var regions = _unitOfWork.RegionRepository.AllAsQueryable
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.Id)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							OrderCode = a.OrderCode,
							FullName = a.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.FullName
						}
					);

				foreach (var region in regions)
				{
					if (result.Select(a => a.RegionId).Contains(region.Key))
						continue;

					result.Add(new StaffCountByGenderDto
					{
						Region = region.Value.FullName,
						RegionOrderCode = region.Value.OrderCode,
						RegionId = region.Key,
					});
				}
				result = result.OrderBy(a => a.RegionOrderCode).ToList();
			}
			if (filter.ByOrganization)
			{
				var organizations = _unitOfWork.OrganizationRepository.AllAsQueryable
					.Include(a => a.Region)
					.IsActive()
					.Where(a => /*!filter.RegionId.HasValue || filter.RegionId == a.RegionId*/
					(!filter.OrganizationId.HasValue || filter.OrganizationId == a.Id) && (
						a.OrganizationGroupId == OrganizationGroupIdConst.SSP
						|| a.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH))
					.ToDictionary(
						a => a.Id,
						a => new
						{
							RegionId = a.RegionId,
							Region = a.Region.FullName,
							RegionOrderCode = a.Region.OrderCode,
							Organization = a.ShortName,
							OrganizationOrderCode = a.OrderCode
						}
					);

				foreach (var organization in organizations)
				{
					if (result.Select(a => a.OrganizationId).Contains(organization.Key))
						continue;

					result.Add(new StaffCountByGenderDto
					{
						Region = organization.Value.Region,
						RegionOrderCode = organization.Value.RegionOrderCode,
						RegionId = organization.Value.RegionId,
						Organization = organization.Value.Organization,
						OrganizationId = organization.Key,
						OrganizationOrderCode = organization.Value.OrganizationOrderCode
					});
				}

				result = result.OrderBy(a => a.OrganizationOrderCode).ToList();
			}
			if (filter.ByDepartment)
			{
				var departments = _unitOfWork.Context.Set<StaffingPosition>()
					.Include(a => a.Owner).ThenInclude(a => a.Organization).Include(a => a.Department)
					.Where(a => filter.OrganizationId == a.Owner.OrganizationId && a.Owner.StatusId == StatusIdConst.RECEIVED)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							OrganizationId = a.Owner.OrganizationId,
							DepartmentId = a.DepartmentId,
							DepartmentOrderCode = a.Department.OrderCode,
							Department = a.Department.FullName
						}
					);

				foreach (var department in departments)
				{
					if (result.Select(a => a.DepartmentId).Contains(department.Value.DepartmentId))
						continue;

					result.Add(new StaffCountByGenderDto
					{
						OrganizationId = department.Value.OrganizationId,
						Department = department.Value.Department,
						DepartmentOrderCode = department.Value.DepartmentOrderCode,
						DepartmentId = department.Value.DepartmentId
					});
				}
				result = result.OrderBy(a => a.DepartmentOrderCode).ToList();
			}
			if (filter.ByPosition)
			{

				var positions = _unitOfWork.Context.Set<StaffingPosition>()
					.Include(a => a.Owner).ThenInclude(a => a.Organization).Include(a => a.Position)
					.Where(a => filter.OrganizationId == a.Owner.OrganizationId && filter.DepartmentId == a.DepartmentId && a.Owner.StatusId == StatusIdConst.RECEIVED)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							PositionId = a.PositionId,
							DepartmentId = a.DepartmentId,
							PositionOrderCode = a.Position.OrderCode,
							Position = /*a.Position.Translates.AsQueryable().FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ??*/ a.Position.FullName,
						}
					);

				foreach (var position in positions)
				{
					if (result.Select(a => a.PositionId).Contains(position.Value.PositionId))
						continue;

					result.Add(new StaffCountByGenderDto
					{
						DepartmentId = position.Value.DepartmentId,
						Position = position.Value.Position,
						PositionOrderCode = position.Value.PositionOrderCode,
						PositionId = position.Value.PositionId
					});
				}
				result = result.OrderBy(a => a.PositionOrderCode).ToList();
			}
			if (filter.ByEmployee)
			{
				var employees = _unitOfWork.EmployeeManageRepository.AllAsQueryable
				 .Include(a => a.Position)
				 .Include(a => a.Department)
				 .Include(a => a.Organization)
				 .ThenInclude(a => a.Region)
				 .Include(a => a.Employee)
				 .ThenInclude(a => a.Person)
				 .ThenInclude(a => a.Nationality)
				 .Include(a => a.Employee)
				 .ThenInclude(a => a.Person)
				 .ThenInclude(a => a.Gender)
				 .Include(a => a.EmpAppointOrderType)
				 .Include(a => a.WorkSchedule)
				 .Include(a => a.EmploymentType)
				 .Where(a => filter.OrganizationId == a.OrganizationId &&
					 filter.DepartmentId == a.DepartmentId && filter.PositionId == a.PositionId && a.EndOn == null && !a.IsDeleted)
				 .ToDictionary(
					 a => a.Id,
					 a => new
					 {
						 Region = a.Organization.Region.FullName,
						 OrganizationId = a.OrganizationId,
						 Organization = a.Organization.ShortName,
						 Department = a.Department.FullName,
						 DepartmentCode = a.Department.OrderCode,
						 Postion = a.Position.FullName,
						 PositionOrderCode = a.Position.OrderCode,
						 DocId = a.DocId,
						 StartOn = a.StartOn,
						 EndOn = a?.EndOn,
						 EmpAppointOrderType = a.EmpAppointOrderType.FullName,
						 EmployeeId = a.EmployeeId,
						 EmploymentType = a.EmploymentType.FullName,
						 EmploymentRate = a.EmploymentRate,
						 WorkSchedule = a.WorkSchedule.FullName,
						 Employee = a.Employee.Person.FullName,
						 EmployeeBirthDate = a.Employee.Person.BirthDate,
						 EmployeeGender = a.Employee.Person.Gender.FullName,
						 EmployeePinfl = a.Employee.Person.Pinfl,
						 EmployeePhoneNumber = a.Employee.PhoneNumber,
						 EmployeeNationality = a.Employee.Person.Nationality.FullName
					 }
				 );

				foreach (var employee in employees)
				{
					if (result.Select(a => a.EmployeeId).Contains(employee.Key))
						continue;

					result.Add(new StaffCountByGenderDto
					{
						Region = employee.Value.Region,
						OrganizationId = employee.Value.OrganizationId,
						Organization = employee.Value.Organization,
						Department = employee.Value.Department,
						DepartmentOrderCode = employee.Value.DepartmentCode,
						Position = employee.Value.Postion,
						PositionOrderCode = employee.Value.PositionOrderCode,
						DocId = employee.Value.DocId,
						StartOn = employee.Value.StartOn,
						EndOn = employee.Value.EndOn,
						EmpAppointOrderType = employee.Value.EmpAppointOrderType,
						EmploymentType = employee.Value.EmploymentType,
						EmploymentRate = employee.Value.EmploymentRate,
						WorkSchedule = employee.Value.WorkSchedule,
						Employee = employee.Value.Employee,
						EmployeeBirthDate = employee.Value.EmployeeBirthDate,
						EmployeeGender = employee.Value.EmployeeGender,
						EmployeePinfl = employee.Value.EmployeePinfl,
						EmployeePhoneNumber = employee.Value.EmployeePhoneNumber,
						EmployeeNationality = employee.Value.EmployeeNationality,
						EmployeeId = employee.Key
					});
				}
				result = result.OrderBy(a => a.DepartmentOrderCode).ThenBy(a => a.PositionOrderCode).ToList();
			}
			return result;
		}
		public List<HrmEmployeeDocumentDto> GetReportDocumentsForHrm(HrmEmployeeDocumentDtoFilter dto)
		{

			var appointEmployee = _unitOfWork.Context.AppointEmployeeTables.Include(b => b.Owner)
				.Where(a => dto.OrganizationId != null ? a.Owner.OrganizationId == dto.OrganizationId : true
				&& (dto.FromDate != null && dto.ToDate != null) ? dto.FromDate <= a.Owner.DocOn && dto.ToDate >= a.Owner.DocOn : true).Select(x => new
				{
					AppointEmployeeId = x.Owner,
					AppointStatusId = x.Owner.StatusId,
					AppointEmpTypeId = x.EmpAppointOrderTypeId,
					OrganizationId = x.Owner.OrganizationId,
					DepartmentId = x.DepartmentId,
					PositionId = x.PositionId,
					EmployeeId = x.EmployeeId
				}).Where(x => x.AppointStatusId == StatusIdConst.ACCEPTED).ToArray();

			var chastisementEmployee = _unitOfWork.Context.ChastisementTables.Include(b => b.Owner)
				.Where(a => dto.OrganizationId != null ? a.Owner.OrganizationId == dto.OrganizationId : true
				&& (dto.FromDate != null && dto.ToDate != null) ? dto.FromDate <= a.Owner.DocOn && dto.ToDate >= a.Owner.DocOn : true).Select(x => new
				{
					ChastisementEmployeeId = x.OwnerId,
					ChastisementStatusId = x.Owner.StatusId,
					HasPenalty = x.HasPenalty,
					HasReprimand = x.HasReprimand,
					OrganizationId = x.Owner.OrganizationId,
					DepartmentId = x.DepartmentId,
					PositionId = x.PositionId,
					EmployeeId = x.EmployeeId
				}).Where(x => x.ChastisementStatusId == StatusIdConst.ACCEPTED).ToArray();

			var tempCalcKindEmployee = _unitOfWork.Context.TempCalcKindTables
				.Where(a => dto.OrganizationId != null ? a.Owner.OrganizationId == dto.OrganizationId : true
				&& (dto.FromDate != null && dto.ToDate != null) ? dto.FromDate <= a.Owner.DocOn && dto.ToDate >= a.Owner.DocOn : true).Select(x => new
				{
					TempCalcKindEmployeeId = x.OwnerId,
					TempCalcKindStatusId = x.Owner.StatusId,
					TemCalcKindTypeId = x.Owner.TempCalcKindTypeId,
					OrganizationId = x.Owner.OrganizationId,
					DepartmentId = x.DepartmentId,
					EmployeeId = x.EmployeeId
				}).Where(x => x.TempCalcKindStatusId == StatusIdConst.ACCEPTED).ToArray();

			var orderToSendBusinessTripEmployee = _unitOfWork.Context.OrderToSendBusinessTripTables.Include(b => b.Owner)
				.Where(a => dto.OrganizationId != null ? a.Owner.OrganizationId == dto.OrganizationId : true
				&& (dto.FromDate != null && dto.ToDate != null) ? dto.FromDate <= a.Owner.DocOn && dto.ToDate >= a.Owner.DocOn : true).Select(x => new
				{
					OrderToSendBusinessTripEmployeeId = x.OwnerId,
					OrderToSendBusinessTripStatusId = x.Owner.StatusId,
					BusinessTripTypeId = x.BusinessTripTypeId,
					OrganizationId = x.Owner.OrganizationId,
					DepartmentId = x.DepartmentId,
					EmployeeId = x.EmployeeId
				}).Where(x => x.OrderToSendBusinessTripStatusId == StatusIdConst.ACCEPTED).ToArray();

			var employeeLeaveOrderEmployee = _unitOfWork.Context.EmployeeLeaveOrderTables.Include(b => b.Owner)
				.Where(a => dto.OrganizationId != null ? a.Owner.OrganizationId == dto.OrganizationId : true
				&& (dto.FromDate != null && dto.ToDate != null) ? dto.FromDate <= a.Owner.DocOn && dto.ToDate >= a.Owner.DocOn : true).Select(x => new
				{
					EmployeeLeaveOrderEmployeeId = x.OwnerId,
					EmployeeLeaveOrderStatusId = x.Owner.StatusId,
					OrganizationId = x.Owner.OrganizationId,
					DepartmentId = x.DepartmentId,
					EmployeeId = x.EmployeeId
				}).Where(x => x.EmployeeLeaveOrderStatusId == StatusIdConst.ACCEPTED).ToArray();

			var sendTrainEmployee = _unitOfWork.Context.EmployeeSendTrainTables.Include(b => b.Owner)
				.Where(a => dto.OrganizationId != null ? a.Owner.OrganizationId == dto.OrganizationId : true
				&& (dto.FromDate != null && dto.ToDate != null) ? dto.FromDate <= a.Owner.DocOn && dto.ToDate >= a.Owner.DocOn : true).Select(x => new
				{
					EmployeeSendTrainId = x.OwnerId,
					EmployeeSendTrainStatusId = x.Owner.StatusId,
					OrganizationId = x.Owner.OrganizationId,
					DepartmentId = x.DepartmentId,
					EmployeeId = x.EmployeeId
				}).Where(x => x.EmployeeSendTrainStatusId == StatusIdConst.ACCEPTED).ToArray();

			var sickLeaveEmployee = _unitOfWork.Context.EmployeeSickLeaveTables.Include(b => b.Owner)
				.Where(a => dto.OrganizationId != null ? a.Owner.OrganizationId == dto.OrganizationId : true
				&& (dto.FromDate != null && dto.ToDate != null) ? dto.FromDate <= a.Owner.DocOn && dto.ToDate >= a.Owner.DocOn : true).Select(x => new
				{
					EmployeeSickLeaveId = x.OwnerId,
					EmployeeSickLeaveStatusId = x.Owner.StatusId,
					EmployeeSickLeaveTypeId = x.Owner.EmployeeSickLeaveTypeId,
					OrganizationId = x.Owner.OrganizationId,
					DepartmentId = x.DepartmentId,
					EmployeeId = x.EmployeeId
				}).Where(x => x.EmployeeSickLeaveStatusId == StatusIdConst.ACCEPTED).ToArray();

			var recallLeaveEmployee = _unitOfWork.Context.RecallLeaveTables.Include(b => b.Owner)
				.Where(a => dto.OrganizationId != null ? a.Owner.OrganizationId == dto.OrganizationId : true
				&& (dto.FromDate != null && dto.ToDate != null) ? dto.FromDate <= a.Owner.DocOn && dto.ToDate >= a.Owner.DocOn : true).Select(x => new
				{
					RecallLeaveId = x.OwnerId,
					RecallLeaveStatusId = x.Owner.StatusId,
					OrganizationId = x.Owner.OrganizationId,
					DepartmentId = x.DepartmentId,
					EmployeeId = x.EmployeeId
				}).Where(x => x.RecallLeaveStatusId == StatusIdConst.ACCEPTED).ToArray();

			var data = new List<HrmEmployeeDocumentDto>();

			if (dto.ByOrganization)
			{
				var organizations = _unitOfWork.Context.Organizations
										.Where(a =>
											(a.OrganizationGroupId == OrganizationGroupIdConst.SSP ||
											 a.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH) &&
											(!dto.OrganizationId.HasValue || dto.OrganizationId == a.Id))
										.Include(x => x.Translates)
										.Include(x => x.Region)
										.ToArray();


				foreach (var item in organizations)
				{
					data.Add(new HrmEmployeeDocumentDto()
					{
						Region = item.Region.FullName,
						OrganizationId = item.Id,
						OrganizationName = /*item.Translates.AsQueryable().FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ??*/ item.ShortName,
						TotalAppoimtEmployeesHireCount = appointEmployee.Count(a => a.OrganizationId == item.Id && a.AppointEmpTypeId == EmpAppointOrderTypeIdConst.HIRE),

						TotalAppoimtEmployeesTransferCount = appointEmployee.Count(a => a.OrganizationId == item.Id && a.AppointEmpTypeId == EmpAppointOrderTypeIdConst.TRANSFER),

						TotalAppoimtEmployeesDismissilCount = appointEmployee.Count(a => a.OrganizationId == item.Id && a.AppointEmpTypeId == EmpAppointOrderTypeIdConst.DISMISSAL),

						TotalChastisementPenaltyCount = chastisementEmployee.Count(a => a.OrganizationId == item.Id && a.HasPenalty),

						TotalChastisementReprimandCount = chastisementEmployee.Count(a => a.OrganizationId == item.Id && a.HasReprimand),

						TotalTempCalcKindFinancialCount = tempCalcKindEmployee.Count(a => a.OrganizationId == item.Id && a.TemCalcKindTypeId == 1),

						TotalTempCalcKindIncentiveCount = tempCalcKindEmployee.Count(a => a.OrganizationId == item.Id && a.TemCalcKindTypeId == 2),

						TotalOrderToSendBusinessTripCount = orderToSendBusinessTripEmployee.Count(a => a.OrganizationId == item.Id && a.BusinessTripTypeId == 1),

						TotalOrderToSendBusinessTripCountryCount = orderToSendBusinessTripEmployee.Count(a => a.OrganizationId == item.Id && a.BusinessTripTypeId == 2),

						TotalOrderToSendBusinessTripAnotherOrgCount = orderToSendBusinessTripEmployee.Count(a => a.OrganizationId == item.Id && a.BusinessTripTypeId == 3),

						TotalEmployeeLeaveOrderCount = employeeLeaveOrderEmployee.Count(a => a.OrganizationId == item.Id),

						TotalEmployeeSendTrainCount = sendTrainEmployee.Count(a => a.OrganizationId == item.Id),

						TotalEmployeeSickLeaveHomladorlikCount = sickLeaveEmployee.Count(a => a.OrganizationId == item.Id && a.EmployeeSickLeaveTypeId == 2),

						TotalEmployeeSickLeaveBolaParvarishiCount = sickLeaveEmployee.Count(a => a.OrganizationId == item.Id && a.EmployeeSickLeaveTypeId == 1),

						TotalRecallLeaveCount = recallLeaveEmployee.Count(a => a.OrganizationId == item.Id),
					});

				}

				return data.OrderBy(x => x.OrganizationId).ToList();
			}
			if (dto.ByDepartment)
			{
				var departments = _unitOfWork.Context.StaffingPositions
							  .Include(b => b.Owner)
								  .ThenInclude(o => o.Organization)
								  .ThenInclude(o => o.Region)
							  .Include(b => b.Department)
							  .Where(x => x.Owner.OrganizationId == dto.OrganizationId && x.Owner.StatusId == StatusIdConst.RECEIVED)
							  .Select(x => new
							  {
								  DepartmentId = x.Department.Id,
								  Department = x.Department.FullName,
								  DepartmentCode = x.Department.OrderCode,
								  Organization = x.Owner.Organization.ShortName,
								  Region = x.Owner.Organization.Region.FullName
							  }).AsEnumerable().DistinctBy(a => a.DepartmentId)
							  .ToArray();


				foreach (var item in departments)
				{
					data.Add(new HrmEmployeeDocumentDto()
					{
						Region = item.Region,
						OrganizationId = dto.OrganizationId,
						OrganizationName = item.Organization,

						DepartmentId = item.DepartmentId,
						Department = item.Department,
						DepartmentCode = item.DepartmentCode,
						TotalAppoimtEmployeesHireCount = appointEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.AppointEmpTypeId == EmpAppointOrderTypeIdConst.HIRE),

						TotalAppoimtEmployeesTransferCount = appointEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.AppointEmpTypeId == EmpAppointOrderTypeIdConst.TRANSFER),

						TotalAppoimtEmployeesDismissilCount = appointEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.AppointEmpTypeId == EmpAppointOrderTypeIdConst.DISMISSAL),

						TotalChastisementPenaltyCount = chastisementEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.HasPenalty),

						TotalChastisementReprimandCount = chastisementEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.HasReprimand),

						TotalTempCalcKindFinancialCount = tempCalcKindEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.TemCalcKindTypeId == 1),

						TotalTempCalcKindIncentiveCount = tempCalcKindEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.TemCalcKindTypeId == 2),

						TotalOrderToSendBusinessTripCount = orderToSendBusinessTripEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.BusinessTripTypeId == 1),

						TotalOrderToSendBusinessTripCountryCount = orderToSendBusinessTripEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.BusinessTripTypeId == 2),

						TotalOrderToSendBusinessTripAnotherOrgCount = orderToSendBusinessTripEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.BusinessTripTypeId == 3),

						TotalEmployeeLeaveOrderCount = employeeLeaveOrderEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId),

						TotalEmployeeSendTrainCount = sendTrainEmployee.Count(a => a.OrganizationId == item.DepartmentId && a.DepartmentId == item.DepartmentId),

						TotalEmployeeSickLeaveHomladorlikCount = sickLeaveEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.EmployeeSickLeaveTypeId == 2),

						TotalEmployeeSickLeaveBolaParvarishiCount = sickLeaveEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.EmployeeSickLeaveTypeId == 1),

						TotalRecallLeaveCount = recallLeaveEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId),
					});
				}
				return data.OrderBy(x => x.DepartmentCode).ToList();
			}
			if (dto.ByPosition)
			{
				//var positions = _unitOfWork.Context.StaffingPositions.Include(b => b.Position).Include(a => a.Owner).ThenInclude(a => a.Organization).Include(b => b.Department).Where(x => x.DepartmentId == dto.DepartmentId && x.Owner.OrganizationId == dto.OrganizationId).ToArray();

				var positions = _unitOfWork.Context.StaffingPositions
							  .Include(b => b.Owner)
								  .ThenInclude(o => o.Organization)
								  .ThenInclude(o => o.Region)
							  .Include(b => b.Department)
							  .Where(x => x.Owner.OrganizationId == dto.OrganizationId && x.DepartmentId == dto.DepartmentId && x.Owner.StatusId == StatusIdConst.RECEIVED)
							  .Select(x => new
							  {
								  DepartmentId = x.Department.Id,
								  Department = x.Department.FullName,
								  PositionId = x.Position.Id,
								  Position = x.Position.FullName,
								  PositionCode = x.Position.OrderCode,
								  DepartmentCode = x.Department.OrderCode,
								  Organization = x.Owner.Organization.ShortName,
								  Region = x.Owner.Organization.Region.FullName
							  }).AsEnumerable().DistinctBy(a => a.PositionId)
							  .ToArray();

				foreach (var item in positions)
				{
					data.Add(new HrmEmployeeDocumentDto()
					{
						Region = item.Region,
						OrganizationId = dto.OrganizationId,
						OrganizationName = item.Organization,

						DepartmentId = item.DepartmentId,
						Department = item.Department,
						DepartmentCode = item.DepartmentCode,
						PositionId = item.PositionId,
						Position = item.Position,
						PositionCode = item.PositionCode,

						TotalAppoimtEmployeesHireCount = appointEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.PositionId == item.PositionId && a.AppointEmpTypeId == EmpAppointOrderTypeIdConst.HIRE),

						TotalAppoimtEmployeesTransferCount = appointEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.PositionId == item.PositionId && a.AppointEmpTypeId == EmpAppointOrderTypeIdConst.TRANSFER),

						TotalAppoimtEmployeesDismissilCount = appointEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.PositionId == item.PositionId && a.AppointEmpTypeId == EmpAppointOrderTypeIdConst.DISMISSAL),

						TotalChastisementPenaltyCount = chastisementEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.PositionId == item.PositionId && a.HasPenalty),

						TotalChastisementReprimandCount = chastisementEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.PositionId == item.PositionId && a.HasReprimand),

						TotalTempCalcKindFinancialCount = tempCalcKindEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.TemCalcKindTypeId == 1),

						TotalTempCalcKindIncentiveCount = tempCalcKindEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.TemCalcKindTypeId == 2),

						TotalOrderToSendBusinessTripCount = orderToSendBusinessTripEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.BusinessTripTypeId == 1),

						TotalOrderToSendBusinessTripCountryCount = orderToSendBusinessTripEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.BusinessTripTypeId == 2),

						TotalOrderToSendBusinessTripAnotherOrgCount = orderToSendBusinessTripEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.BusinessTripTypeId == 3),

						TotalEmployeeLeaveOrderCount = employeeLeaveOrderEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId),

						TotalEmployeeSendTrainCount = sendTrainEmployee.Count(a => a.OrganizationId == item.DepartmentId && a.DepartmentId == item.DepartmentId),

						TotalEmployeeSickLeaveHomladorlikCount = sickLeaveEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.EmployeeSickLeaveTypeId == 2),

						TotalEmployeeSickLeaveBolaParvarishiCount = sickLeaveEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.EmployeeSickLeaveTypeId == 1),

						TotalRecallLeaveCount = recallLeaveEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId),
					});
				}
			}
			if (dto.ByEmployee)
			{
				var employees = _unitOfWork.Context.EmployeeManages
													  .Include(a => a.Organization)
													  .ThenInclude(a => a.Region)
													  .Include(b => b.Position)
													  .Include(b => b.Department)
													  .Include(e => e.Employee)
													  .ThenInclude(c => c.Person)
													  .Where(x => x.DepartmentId == dto.DepartmentId &&
																  x.OrganizationId == dto.OrganizationId &&
																  x.PositionId == dto.PositionId &&
																  x.EndOn == null &&
																  !x.IsDeleted)
													  .ToArray();


				foreach (var item in employees)
				{
					data.Add(new HrmEmployeeDocumentDto()
					{
						Region = item.Organization.Region.FullName,
						OrganizationId = dto.OrganizationId,
						OrganizationName = item.Organization.ShortName,

						DepartmentId = item.DepartmentId,
						Department = item.Department.FullName,

						PositionId = item.PositionId,
						Position = item.Position.FullName,

						EmployeeId = item.EmployeeId,
						Employee = item.Employee.Person.FullName,
						EmployeePinfl = item.Employee.Person.Pinfl,

						TotalAppoimtEmployeesHireCount = appointEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.EmployeeId == item.EmployeeId && a.AppointEmpTypeId == EmpAppointOrderTypeIdConst.HIRE),

						TotalAppoimtEmployeesTransferCount = appointEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.EmployeeId == item.EmployeeId && a.AppointEmpTypeId == EmpAppointOrderTypeIdConst.TRANSFER),

						TotalAppoimtEmployeesDismissilCount = appointEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.EmployeeId == item.EmployeeId && a.AppointEmpTypeId == EmpAppointOrderTypeIdConst.DISMISSAL),

						TotalChastisementPenaltyCount = chastisementEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.EmployeeId == item.EmployeeId && a.HasPenalty),

						TotalChastisementReprimandCount = chastisementEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.EmployeeId == item.EmployeeId && a.HasReprimand),

						TotalTempCalcKindFinancialCount = tempCalcKindEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.EmployeeId == item.EmployeeId && a.TemCalcKindTypeId == 1),

						TotalTempCalcKindIncentiveCount = tempCalcKindEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.EmployeeId == item.EmployeeId && a.TemCalcKindTypeId == 2),

						TotalOrderToSendBusinessTripCount = orderToSendBusinessTripEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.EmployeeId == item.EmployeeId && a.BusinessTripTypeId == 1),

						TotalOrderToSendBusinessTripCountryCount = orderToSendBusinessTripEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.DepartmentId == item.DepartmentId && a.EmployeeId == item.EmployeeId && a.BusinessTripTypeId == 2),

						TotalOrderToSendBusinessTripAnotherOrgCount = orderToSendBusinessTripEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.EmployeeId == item.EmployeeId && a.BusinessTripTypeId == 3),

						TotalEmployeeLeaveOrderCount = employeeLeaveOrderEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.EmployeeId == item.EmployeeId),

						TotalEmployeeSendTrainCount = sendTrainEmployee.Count(a => a.OrganizationId == item.DepartmentId && a.DepartmentId == item.DepartmentId && a.EmployeeId == item.EmployeeId),

						TotalEmployeeSickLeaveHomladorlikCount = sickLeaveEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.EmployeeSickLeaveTypeId == 2 && a.EmployeeId == item.EmployeeId),

						TotalEmployeeSickLeaveBolaParvarishiCount = sickLeaveEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.EmployeeSickLeaveTypeId == 1 && a.EmployeeId == item.EmployeeId),

						TotalRecallLeaveCount = recallLeaveEmployee.Count(a => a.OrganizationId == dto.OrganizationId && a.EmployeeId == item.EmployeeId),
					});
				}
			}
			return data;
		}
		#endregion

		#region INTEGRATION
		public async Task<List<GetFreeAreaByInnDataDto>> GetFreeAreaFromBandlik()
		{
			var data = await _bandlikService.GetFreeAreaByInn();

			CombineStatuses(_bandlikService);
			if (HasErrors)
				return null;
			var regions = _unitOfWork.Context.Regions.Include(a => a.Translates);
			var districts = _unitOfWork.Context.Districts.Include(a => a.Translates);
			Region region;
			District district;
			foreach (var item in data)
			{
				district = districts.FirstOrDefault(a => a.Soato == item.Soato.Substring(0, 7));
				region = regions.FirstOrDefault(a => a.Soato == item.Soato.Substring(0, 4));
				item.DistrictId = district.Id;
				item.District = district.Translates.AsQueryable()
					.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? district.FullName;

				item.RegionId = region.Id;
				item.Region = region.Translates.AsQueryable()
					.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? region.FullName;

			}
			CombineStatuses(_bandlikService);
			return data;
		}
		public async Task<List<ContractorBankCreditReportResponse>> GetBankCreditReport(ContractorBankCreditReportDtoFilter dto)
		{
			var result = new List<ContractorBankCreditReportResponse>();
			if (!dto.ByContractor)
			{
				var data = _bankCreditService.GetReport().Result;
				CombineStatuses(_bankCreditService);
				if (HasErrors)
				{
					return null;
				}

				var res = data.SortFilter(new BankCreditReportDtoFilter()
				{
					BankMfo = dto.BankMfo,
					ByBank = dto.ByBank,
					DistrictId = dto.DistrictId,
					ByDistrict = dto.ByDistrict,
					ByRegion = dto.ByRegion,
					RegionId = dto.RegionId,
					Year = dto.Year
				});
				result = res.Select(a => new ContractorBankCreditReportResponse()
				{
					RegionId = dto.ByRegion ? a.RegionId : null,
					RegionName = dto.ByRegion ? a.RegionName : null,
					DistrictId = dto.ByDistrict ? a.DistrictId : null,
					DistrictName = dto.ByDistrict ? a.DistrictName : null,
					DistrictCode = dto.ByDistrict ? a.DistrictCode : null,
					Year = a.Year,
					BankName = dto.ByBank ? a.BankName : null,
					BankMfo = dto.ByBank ? a.BankMfo : null,
					Application = new ContractorContractType()
					{
						ApprovedCount = a.Application.ApprovedCount,
						SubmittedCount = a.Application.SubmittedCount,
						CanceledCount = a.Application.CanceledCount,
						IssuanceCount = a.Application.IssuanceCount,
						RejectedCount = a.Application.RejectedCount,
						CanceledSum = a.Application.CanceledSum,
						ApprovedSum = a.Application.ApprovedSum,
						SubmittedSum = a.Application.SubmittedSum,
						RejectedSum = a.Application.RejectedSum,
						IssuanceSum = a.Application.IssuanceSum
					},
					ContractType1 = new ContractorContractType()
					{
						ApprovedCount = a.ContractType1.ApprovedCount,
						SubmittedCount = a.ContractType1.SubmittedCount,
						CanceledCount = a.ContractType1.CanceledCount,
						IssuanceCount = a.ContractType1.IssuanceCount,
						RejectedCount = a.ContractType1.RejectedCount,
						CanceledSum = a.ContractType1.CanceledSum,
						ApprovedSum = a.ContractType1.ApprovedSum,
						SubmittedSum = a.ContractType1.SubmittedSum,
						RejectedSum = a.ContractType1.RejectedSum,
						IssuanceSum = a.ContractType1.IssuanceSum
					},
					ContractType2 = new ContractorContractType()
					{
						ApprovedCount = a.ContractType2.ApprovedCount,
						SubmittedCount = a.ContractType2.SubmittedCount,
						CanceledCount = a.ContractType2.CanceledCount,
						IssuanceCount = a.ContractType2.IssuanceCount,
						RejectedCount = a.ContractType2.RejectedCount,
						CanceledSum = a.ContractType2.CanceledSum,
						ApprovedSum = a.ContractType2.ApprovedSum,
						SubmittedSum = a.ContractType2.SubmittedSum,
						RejectedSum = a.ContractType2.RejectedSum,
						IssuanceSum = a.ContractType2.IssuanceSum
					},
					ContractType3 = new ContractorContractType()
					{
						ApprovedCount = a.ContractType3.ApprovedCount,
						SubmittedCount = a.ContractType3.SubmittedCount,
						CanceledCount = a.ContractType3.CanceledCount,
						IssuanceCount = a.ContractType3.IssuanceCount,
						RejectedCount = a.ContractType3.RejectedCount,
						CanceledSum = a.ContractType3.CanceledSum,
						ApprovedSum = a.ContractType3.ApprovedSum,
						SubmittedSum = a.ContractType3.SubmittedSum,
						RejectedSum = a.ContractType3.RejectedSum,
						IssuanceSum = a.ContractType3.IssuanceSum
					}
				}
				)
				.GroupBy(b => new { b.RegionId, b.RegionName, b.DistrictId, b.DistrictName, b.DistrictCode, b.BankName, b.BankMfo })
				.Select(a => new ContractorBankCreditReportResponse
				{
					RegionId = a.Key.RegionId,
					RegionName = a.Key.RegionName,
					DistrictId = a.Key.DistrictId,
					DistrictName = a.Key.DistrictName,
					DistrictCode = a.Key.DistrictCode,
					//Year = a.Key.Year,
					BankName = a.Key.BankName,
					BankMfo = a.Key.BankMfo,
					Application = new ContractorContractType
					{
						SubmittedCount = a.Sum(b => b.Application.SubmittedCount),
						ApprovedCount = a.Sum(b => b.Application.ApprovedCount),
						IssuanceCount = a.Sum(b => b.Application.IssuanceCount),
						SubmittedSum = a.Sum(b => b.Application.SubmittedSum),
						RejectedSum = a.Sum(b => b.Application.RejectedSum),
						CanceledSum = a.Sum(b => b.Application.CanceledSum),
						CanceledCount = a.Sum(b => b.Application.CanceledCount),
						RejectedCount = a.Sum(b => b.Application.RejectedCount),
						IssuanceSum = a.Sum(b => b.Application.IssuanceSum),
						ApprovedSum = a.Sum(b => b.Application.ApprovedSum),
					},
					ContractType1 = new ContractorContractType
					{
						SubmittedCount = a.Sum(b => b.ContractType1.SubmittedCount),
						ApprovedCount = a.Sum(b => b.ContractType1.ApprovedCount),
						IssuanceCount = a.Sum(b => b.ContractType1.IssuanceCount),
						SubmittedSum = a.Sum(b => b.ContractType1.SubmittedSum),
						RejectedSum = a.Sum(b => b.ContractType1.RejectedSum),
						CanceledSum = a.Sum(b => b.ContractType1.CanceledSum),
						CanceledCount = a.Sum(b => b.ContractType1.CanceledCount),
						RejectedCount = a.Sum(b => b.ContractType1.RejectedCount),
						IssuanceSum = a.Sum(b => b.ContractType1.IssuanceSum),
						ApprovedSum = a.Sum(b => b.ContractType1.ApprovedSum),
					},
					ContractType2 = new ContractorContractType
					{
						SubmittedCount = a.Sum(b => b.ContractType2.SubmittedCount),
						ApprovedCount = a.Sum(b => b.ContractType2.ApprovedCount),
						IssuanceCount = a.Sum(b => b.ContractType2.IssuanceCount),
						SubmittedSum = a.Sum(b => b.ContractType2.SubmittedSum),
						RejectedSum = a.Sum(b => b.ContractType2.RejectedSum),
						CanceledSum = a.Sum(b => b.ContractType2.CanceledSum),
						CanceledCount = a.Sum(b => b.ContractType2.CanceledCount),
						RejectedCount = a.Sum(b => b.ContractType2.RejectedCount),
						IssuanceSum = a.Sum(b => b.ContractType2.IssuanceSum),
						ApprovedSum = a.Sum(b => b.ContractType2.ApprovedSum),
					},
					ContractType3 = new ContractorContractType
					{
						SubmittedCount = a.Sum(b => b.ContractType3.SubmittedCount),
						ApprovedCount = a.Sum(b => b.ContractType3.ApprovedCount),
						IssuanceCount = a.Sum(b => b.ContractType3.IssuanceCount),
						SubmittedSum = a.Sum(b => b.ContractType3.SubmittedSum),
						RejectedSum = a.Sum(b => b.ContractType3.RejectedSum),
						CanceledSum = a.Sum(b => b.ContractType3.CanceledSum),
						CanceledCount = a.Sum(b => b.ContractType3.CanceledCount),
						RejectedCount = a.Sum(b => b.ContractType3.RejectedCount),
						IssuanceSum = a.Sum(b => b.ContractType3.IssuanceSum),
						ApprovedSum = a.Sum(b => b.ContractType3.ApprovedSum),
					}
				}).OrderBy(a => a.RegionId).ThenBy(a => a.DistrictId).ToList();
			}
			else
			{
				var data = _unitOfWork.Context.GetBankCreditReport(dto.Year.HasValue ? dto.Year.Value.ToString() : null, dto.RegionId, dto.DistrictId, dto.ContractorInn, dto.BankMfo).ToList();

                result = data.Select(a => new ContractorBankCreditReportResponse
                {
                    ContractorInn = a.contractorinn,
                    Contractor = a.contractor,
					Year = string.IsNullOrEmpty(a.year) ? 0 : int.TryParse(a.year.Split('.')[2], out int year) ? year : 0,
                    Application = new ContractorContractType
                    {
                        ApprovedCount = (int)a.approvedcount,
                        ApprovedSum = (double)a.approvedsum,
						SubmittedCount = a.submittedcount,
                        SubmittedSum = (double)a.submittedsum,
                        IssuanceCount = (int)a.issuancecount,
                        IssuanceSum = (double)a.issuancesum,
                        CanceledCount = (int)a.canceledcount,
                        CanceledSum = (double)a.canceledsum,
                        RejectedCount = (int)a.rejectedcount,
                        RejectedSum = (double)a.rejectedsum
                    },
                    ContractType1 = new ContractorContractType
                    {
                        ApprovedCount = (int)a.approvedcount1,
                        ApprovedSum = (double)a.approvedsum1,
						SubmittedCount = a.submittedcount1,
                        SubmittedSum = (double)a.submittedsum1,
                        IssuanceCount = (int)a.issuancecount1,
                        IssuanceSum = (double)a.issuancesum1,
                        CanceledCount = (int)a.canceledcount1,
                        CanceledSum = (double)a.canceledsum1,
                        RejectedCount = (int)a.rejectedcount1,
                        RejectedSum = (double)a.rejectedsum1
                    },
                    ContractType2 = new ContractorContractType
                    {
                        ApprovedCount = (int)a.approvedcount2,
                        ApprovedSum = (double)a.approvedsum2,
						SubmittedCount = a.submittedcount2,
                        SubmittedSum = (double)a.submittedsum2,
                        IssuanceCount = (int)a.issuancecount2,
                        IssuanceSum = (double)a.issuancesum2,
                        CanceledCount = (int)a.canceledcount2,
                        CanceledSum = (double)a.canceledsum2,
                        RejectedCount = (int)a.rejectedcount2,
                        RejectedSum = (double)a.rejectedsum2
                    },
                    ContractType3 = new ContractorContractType
                    {
                        ApprovedCount = (int)a.approvedcount3,
                        ApprovedSum = (double)a.approvedsum3,
						SubmittedCount = a.submittedcount3,
                        SubmittedSum = (double)a.submittedsum3,
                        IssuanceCount = (int)a.issuancecount3,
                        IssuanceSum = (double)a.issuancesum3,
                        CanceledCount = (int)a.canceledcount3,
                        CanceledSum = (double)a.canceledsum3,
                        RejectedCount = (int)a.rejectedcount3,
                        RejectedSum = (double)a.rejectedsum3
                    }
                }).ToList();
            }
            return result;

		}
		public async Task<WEBASE.Models.PagedResult<ContractorBankCreditReportResponse>> GetPagedBankCreditReport(ContractorBankCreditReportDtoFilterPageOptions options)
		{
			var res = await GetBankCreditReport(new ContractorBankCreditReportDtoFilter
			{
				RegionId = options.RegionId,
				ByRegion = options.ByRegion,
				ByBank = options.ByBank,
				ByBankCode = options.ByBankCode,
				ByContractor = options.ByContractor,
				BankMfo = options.BankMfo,
				ByDistrict = options.ByDistrict,
				ContractorInn = options.ContractorInn,
				DistrictId = options.DistrictId,
				Year = options.Year
			});

			return res.AsQueryable().AsPagedResult(options);
		}
		public List<GetSoliqReportByContractorDto> GetSoliqReportByContractor(GetSoliqReportByContractorDtoFilter options)
		{
			var languageId = _cultureHelper.CurrentCulture.Id;
			var result = _unitOfWork.Context.GetSoliqReportByContractor(
				options.Id,
				options.Inn,
				options.Year,
				options.RegionId,
				options.DistrictId,
				options.ByRegion,
				options.ByDistrict,
				options.ByContractor,
				options.HasCertificate,
				languageId
				).ToList();

			var PrtnApplicationByContractType = GetPrtnApplicationByContractType(new PrtnApplicationByContractTypeDtoFilter
			{
				ByRegion = options.ByRegion,
				RegionId = options.RegionId,
				ByContractor = options.ByContractor,
				ByDistrict = options.ByDistrict,
				StartDate = options.Year.HasValue ? new DateOnly(options.Year.Value, 1, 1) : null,
				EndDate = options.Year.HasValue ? new DateOnly(options.Year.Value, 12, 31) : null,
			});

			foreach (var item in result)
			{
				if (item.RegionId != null)
				{
					item.ContractorCount = PrtnApplicationByContractType.Rows.FirstOrDefault(a => a.RegionId == item.RegionId).TotalCertificate.TotalCount == null
					 ? 0
					 : PrtnApplicationByContractType.Rows.FirstOrDefault(a => a.RegionId == item.RegionId).TotalCertificate.TotalCount.Value;
				}

				else if (item.DistrictId != null)
				{
					item.ContractorCount = PrtnApplicationByContractType.Rows.FirstOrDefault(a => a.DistrictId == item.DistrictId).TotalCertificate.TotalCount == null
					  ? 0
					  : PrtnApplicationByContractType.Rows.FirstOrDefault(a => a.DistrictId == item.DistrictId).TotalCertificate.TotalCount.Value;

				}
			}

			return result;
		}
		//public List<AppealReportDto> AppealReportByAppealType(AppealReportDtoFilter option)
		//{
		//	var array = new int[] { StatusIdConst.BANK_RECEIVED,
		//							StatusIdConst.CREATED,
		//							StatusIdConst.IN_EXECUTION,
		//							StatusIdConst.EXECUTED,
		//							StatusIdConst.REJECTED,
		//							};
		//	var res = _unitOfWork.Context.GetReportByAppealType(array, option.RegionId, option.DistrictId, option.LanguageId).ToList();
		//	return res;
		//}
		public WEBASE.Models.PagedResult<TaxCreditreportDto> GetTaxCreditReport(TaxCreditReportDtoFilterPageOptions options)
		{
			PagedResult<TaxCreditreportDto> res = GetTaxCreditReport(new TaxCreditReportDtoFilter
			{
				RegionId = options.RegionId,
				ByRegion = options.ByRegion,
				DistrictId = options.DistrictId,
				ByDistrict = options.ByDistrict,
				ContractorId = options.ContractorId,
				ContractorInn = options.ContractorInn,
				ByContractor = options.ByContractor,
				Year = options.Year,
				LanguageId = options.LanguageId,
				ContarctTypeId = options.ContarctTypeId,
				ByContactType = options.ByContactType,
				HasCertificate = options.HasCertificate,
				TaxCrediteType = options.TaxCrediteType,
				Tab = options.Tab
			}).AsPagedResult(options);
			return res;
		}
		public IQueryable<TaxCreditreportDto> GetTaxCreditReport(TaxCreditReportDtoFilter options)
		{
			var languageId = _cultureHelper.CurrentCulture.Id;
			var result = _unitOfWork.Context.GetTaxCreditReport(
				options.ContractorId,
				options.ContractorInn,
				options.RegionId,
				options.DistrictId,
				options.ByRegion,
				options.ByDistrict,
				options.ByContractor,
				options.Year,
				options.ContarctTypeId,
				options.ByContactType,
				options.HasCertificate,
				languageId
				);

			return result;
		}
		public List<BojxonaImtiyozReportByContractorDto> GetBojxonaImtiyozReportByContractor(BojxonaImtiyozReportByContractorDtoFilter options)
		{
			var languageId = _cultureHelper.CurrentCulture.Id;
			var result = _unitOfWork.Context.BojxonaImtiyozReportByContractor(
				options.ContractorId,
				options.ContractorInn,
				options.RegionId,
				options.DistrictId,
				options.ByRegion,
				options.ByDistrict,
				options.ByContractor,
				options.Year,
				options.ContarctTypeId,
				options.ByContactType,
				options.HasCertificate,
				languageId
				).ToList();

			return result;
		}
		public List<BusinessActivityTypeReportByRegion> GetBusinessActivityTypeReportByRegion(BusinessActivityTypeReportByRegionFilter dto)
		{
			var allData = _unitOfWork.Context.FundTadbirkors
				.Include(a => a.Credits)
				.Where(a => a.AidAmount != 0)
				.Select(a => new
				{
					Tin = a.TinPinfl,
					Amount = a.Credits.Sum(b => b.Amount),
					AidAmount = a.AidAmount
				})
				.AsEnumerable()
				.GroupBy(a => a.Tin)
				.Select(a => new
				{
					Tin = a.Key,
					Amount = a.Sum(b => b.Amount),
					AidAmount = a.Sum(b => b.AidAmount)
				})
				.ToArray();
			var aviableContractorInns = allData.Select(a => a.Tin).ToArray();

			var contractorDatas = _unitOfWork.Context.Contractors
				.IsActive()
				.Where(a => aviableContractorInns.Contains(a.Inn))
				.ToArray()
				.ToDictionary(a => a.Inn, a => new { a.Id, a.DistrictId, a.RegionId });
			var aviableContractorIds = contractorDatas.Values.Select(a => a.Id).ToArray();

			var vacansiesData = _unitOfWork.Context.PrtnContracts
				.Where(a => a.StatusId == StatusIdConst.FORMED && aviableContractorIds.Contains(a.ContractorId))
				.ToArray()
				.ToDictionary(a => a.ContractorId, a => new { a.PrtnContractTypeId, a.NewVacanciesCount });


			var regionData = _unitOfWork.Context.Regions
			.IsActive()
			.ToArray()
			.ToDictionary(
					a => a.Id,
					a => a.Translates.AsQueryable().FirstOrDefault(RegionTranslate
							.GetExpr(TranslateColumn.short_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
							?? a.ShortName);

			var result = new List<BusinessActivityTypeReportByRegion>();
			var data = allData;

			if (dto.ByRegion)
			{
				foreach (var region in regionData)
				{
					data = allData.Where(a => contractorDatas[a.Tin].RegionId == region.Key).ToArray();
					var item = new BusinessActivityTypeReportByRegion()
					{
						RegionId = region.Key,
						RegionName = region.Value,
						BusinessActivity = new BusinessActivityReport
						{
							UserPrivilegeCount = data.Count(),
							CreatedVacanciesCount = data.Sum(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) ? vacansiesData[contractorDatas[a.Tin].Id].NewVacanciesCount : 0),
							FinancialHelpAmount = data.Sum(a => a.AidAmount),
							ApprovedFinancialHelpAmount = data.Sum(a => a.Amount)
						},
						BusinessType1 = new BusinessActivityReport
						{
							UserPrivilegeCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._50_100).Count(),
							CreatedVacanciesCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => vacansiesData[contractorDatas[a.Tin].Id].NewVacanciesCount),
							FinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.AidAmount),
							ApprovedFinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Amount)
						},
						BusinessType2 = new BusinessActivityReport
						{
							UserPrivilegeCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._101_200).Count(),
							CreatedVacanciesCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => vacansiesData[contractorDatas[a.Tin].Id].NewVacanciesCount),
							FinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.AidAmount),
							ApprovedFinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Amount)
						},
						BusinessType3 = new BusinessActivityReport
						{
							UserPrivilegeCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._201__).Count(),
							CreatedVacanciesCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => vacansiesData[contractorDatas[a.Tin].Id].NewVacanciesCount),
							FinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.AidAmount),
							ApprovedFinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Amount)
						}

					};
					result.Add(item);
				}
				return result;

			}
			if (dto.ByDistrict)
			{
				var districtData = _unitOfWork.Context.Districts
				   .IsActive()
				   .Where(a => dto.RegionId != null ? a.RegionId == dto.RegionId : true)
				   .ToDictionary(
					   ent => ent.Id,
					   ent => new
					   {
						   DistrictName = ent.Translates.AsQueryable().FirstOrDefault(DistrictTranslate
							.GetExpr(TranslateColumn.short_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
							?? ent.ShortName,
						   ent.RegionId
					   });

				foreach (var district in districtData)
				{
					data = allData.Where(a => contractorDatas[a.Tin].DistrictId == district.Key).ToArray();
					var item = new BusinessActivityTypeReportByRegion()
					{
						RegionId = districtData[district.Key].RegionId,
						RegionName = regionData[districtData[district.Key].RegionId],
						DistrictId = district.Key,
						DistrictName = districtData[district.Key].DistrictName,
						BusinessActivity = new BusinessActivityReport
						{
							UserPrivilegeCount = data.Count(),
							CreatedVacanciesCount = data.Sum(a => vacansiesData[contractorDatas[a.Tin].Id].NewVacanciesCount),
							FinancialHelpAmount = data.Sum(a => a.AidAmount),
							ApprovedFinancialHelpAmount = data.Sum(a => a.Amount)
						},
						BusinessType1 = new BusinessActivityReport
						{
							UserPrivilegeCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._50_100).Count(),
							CreatedVacanciesCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => vacansiesData[contractorDatas[a.Tin].Id].NewVacanciesCount),
							FinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.AidAmount),
							ApprovedFinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Amount)
						},
						BusinessType2 = new BusinessActivityReport
						{
							UserPrivilegeCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._101_200).Count(),
							CreatedVacanciesCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => vacansiesData[contractorDatas[a.Tin].Id].NewVacanciesCount),
							FinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.AidAmount),
							ApprovedFinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Amount)
						},
						BusinessType3 = new BusinessActivityReport
						{
							UserPrivilegeCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._201__).Count(),
							CreatedVacanciesCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => vacansiesData[contractorDatas[a.Tin].Id].NewVacanciesCount),
							FinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.AidAmount),
							ApprovedFinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Amount)
						}

					};
					result.Add(item);
				}
				return result;
			}
			return result;
		}
		public List<BusinessActivityTypeReportDto> GetBusinessActivityTypeReport(BusinessActivityTypeReprotFilter dto)
		{

			var result = new List<BusinessActivityTypeReportDto>();

			var bankCodes = _unitOfWork.Context.BankCodes.ToDictionary(a => a.Id, a => a.ShortName);

			var allData = _unitOfWork.Context.FundTadbirkors
				.Include(a => a.Credits)
				.Where(a => a.AidAmount != 0)
				.Select(a => new
				{
					Tin = a.TinPinfl,
					BankCode = a.BankCode,
					Amount = a.Credits.Sum(b => b.Amount),
					AidAmount = a.AidAmount
				})
				.AsEnumerable()
				.GroupBy(a => new { a.Tin, a.BankCode })
				.Select(a => new
				{
					Tin = a.Key.Tin,
					BankCode = a.Key.BankCode,
					Amount = a.Sum(b => b.Amount),
					AidAmount = a.Sum(b => b.AidAmount)
				})
				.ToArray();
			var aviableCodes = allData.Select(a => a.BankCode).ToArray();

			var bankDatas = _unitOfWork.Context.Banks.IsActive().Select(a => new { a.BankCodeId, a.Code, a.Id, a.BankName }).ToArray();

			var aviableContractorInns = allData.Select(a => a.Tin).ToArray();

			var contractorDatas = _unitOfWork.Context.Contractors
				.IsActive()
				.Where(a => aviableContractorInns.Contains(a.Inn))
				.ToList()
				.ToDictionary(a => a.Inn, a => new { a.Id, a.DistrictId, a.RegionId });
			var aviableContractorIds = contractorDatas.Values.Select(a => a.Id).ToArray();

			var vacansiesData = _unitOfWork.Context.PrtnContracts
				.Where(a => a.StatusId == StatusIdConst.FORMED && aviableContractorIds.Contains(a.ContractorId))
				.ToArray()
				.ToDictionary(a => a.ContractorId, a => new { a.PrtnContractTypeId, a.NewVacanciesCount });

			var data = allData;

			if (dto.ByBank)
			{
				bankDatas = bankDatas.Where(a => a.BankCodeId == dto.BankCodeId && aviableCodes.Contains(a.Code)).ToArray();
				foreach (var bankData in bankDatas)
				{
					data = allData.Where(a => a.BankCode == bankData.Code).ToArray();
					var item = new BusinessActivityTypeReportDto()
					{
						BankName = bankData.BankName,
						BankCodeId = bankData.Id,
						BusinessActivity = new BusinessActivityReport
						{
							UserPrivilegeCount = data.Count(),
							CreatedVacanciesCount = data.Sum(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) ? vacansiesData[contractorDatas[a.Tin].Id].NewVacanciesCount : 0),
							FinancialHelpAmount = data.Sum(a => a.AidAmount),
							ApprovedFinancialHelpAmount = data.Sum(a => a.Amount)
						},
						BusinessType1 = new BusinessActivityReport
						{
							UserPrivilegeCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._50_100).Count(),
							CreatedVacanciesCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => vacansiesData[contractorDatas[a.Tin].Id].NewVacanciesCount),
							FinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.AidAmount),
							ApprovedFinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Amount)
						},
						BusinessType2 = new BusinessActivityReport
						{
							UserPrivilegeCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._101_200).Count(),
							CreatedVacanciesCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => vacansiesData[contractorDatas[a.Tin].Id].NewVacanciesCount),
							FinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.AidAmount),
							ApprovedFinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Amount)
						},
						BusinessType3 = new BusinessActivityReport
						{
							UserPrivilegeCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._201__).Count(),
							CreatedVacanciesCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => vacansiesData[contractorDatas[a.Tin].Id].NewVacanciesCount),
							FinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.AidAmount),
							ApprovedFinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Amount)
						}

					};
					result.Add(item);
				}
				return result;

			}
			else
			{
				foreach (var bankCode in bankCodes)
				{
					var bankDataItems = bankDatas.Where(a => a.BankCodeId == bankCode.Key).Select(a => a.Code).ToArray();
					data = allData.Where(a => bankDataItems.Contains(a.BankCode)).ToArray();
					var item = new BusinessActivityTypeReportDto()
					{
						BankName = bankCode.Value,
						BankCodeId = bankCode.Key,
						BusinessActivity = new BusinessActivityReport
						{
							UserPrivilegeCount = data.Count(),
							CreatedVacanciesCount = data.Sum(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) ? vacansiesData[contractorDatas[a.Tin].Id].NewVacanciesCount : 0),
							FinancialHelpAmount = data.Sum(a => a.AidAmount),
							ApprovedFinancialHelpAmount = data.Sum(a => a.Amount)
						},
						BusinessType1 = new BusinessActivityReport
						{
							UserPrivilegeCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._50_100).Count(),
							CreatedVacanciesCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => vacansiesData[contractorDatas[a.Tin].Id].NewVacanciesCount),
							FinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.AidAmount),
							ApprovedFinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Amount)
						},
						BusinessType2 = new BusinessActivityReport
						{
							UserPrivilegeCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._101_200).Count(),
							CreatedVacanciesCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => vacansiesData[contractorDatas[a.Tin].Id].NewVacanciesCount),
							FinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.AidAmount),
							ApprovedFinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Amount)
						},
						BusinessType3 = new BusinessActivityReport
						{
							UserPrivilegeCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._201__).Count(),
							CreatedVacanciesCount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => vacansiesData[contractorDatas[a.Tin].Id].NewVacanciesCount),
							FinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.AidAmount),
							ApprovedFinancialHelpAmount = data.Where(a => vacansiesData.ContainsKey(contractorDatas[a.Tin].Id) && vacansiesData[contractorDatas[a.Tin].Id].PrtnContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Amount)
						}

					};
					result.Add(item);
				}
				return result;
			}


			return result;
		}
		public IQueryable<BankCreditApplicationReportByRegionAndDistrictDto> GetBankCreditApplicationReportByRegionAndDistrict(BankCreditApplicationReportByRegionAndDistrictDtoFilter options)
		{
			var languageId = _cultureHelper.CurrentCulture.Id;
			var result = _unitOfWork.Context.GetBankCreditApplicationReportByRegionAndDistrict(
				options.ContractorId,
				options.ContractorInn,
				options.RegionId,
				options.DistrictId,
				options.ByRegion,
				options.ByDistrict,
				options.ByContractor,
				options.ContractTypeId,
				options.ByContactType,
				options.HasCertificate,
				languageId
				);
			return result;
		}
		public List<TadbirkorFundReportDto> GetTadbirkorFundReport(TadbirkorFundReportDtoFilter options)
		{
			var languageId = _cultureHelper.CurrentCulture.Id;
			var result = _unitOfWork.Context.GetTadbirkorFundReport(
				options.ContractorId,
				options.ContractorInn,
				options.RegionId,
				options.DistrictId,
				options.ByRegion,
				options.ByDistrict,
				options.ByContractor,
				options.BankId,
				options.ByBank,
				options.ContractTypeId,
				options.ByContractType,
				languageId
				).ToList();

			return result;
		}
		public PagedResult<GetTaxQqsAylanmaDto> GetTaxQqsAylanmaReport(GetTaxQqsAylanmaDtoFilter options)
		{
			var languageId = _cultureHelper.CurrentCulture.Id;
			var result = _unitOfWork.Context.GetTaxQqsAylanmaReport(
				options.ContractorId,
				options.ContractorInn,
				options.RegionId,
				options.DistrictId,
				options.ByRegion,
				options.ByDistrict,
				options.ByContractor,
				options.Year,
				options.Month,
				languageId
				).AsPagedResult(options);

			return result;
		}
		#endregion

		#region OTHERS
		public List<StateAssetApplicationReportDto> GetStateAssetApplicationReport(StateAssetApplicationFilter dto)

		{
			var stateAssetApplications = _unitOfWork.Context.StateAssetApplications
				.Include(x => x.PrtnCertificate)
				.ThenInclude(x => x.PrtnContract)
					.ThenInclude(a => a.Contractor)
					  .ThenInclude(x => x.Region)
				.Include(x => x.PrtnCertificate).ThenInclude(x => x.Contractor).ThenInclude(x => x.District)
				.Include(x => x.Application)
				.AsSplitQuery()
				.ToArray();
			if (dto.RegionId != null)
				stateAssetApplications = stateAssetApplications.Where(a => a.PrtnCertificate.Contractor.RegionId == dto.RegionId).ToArray();
			if (dto.DistrictId != null)
				stateAssetApplications = stateAssetApplications.Where(a => a.PrtnCertificate.PrtnContract.Contractor.RegionId == dto.DistrictId).ToArray();
			var res = stateAssetApplications.Select(a => new StateAssetApplicationReportDto()
			{
				RegionId = dto.ByRegion ? a.PrtnCertificate.PrtnContract.Contractor.RegionId : 0,
				RegionName = dto.ByRegion ? a.PrtnCertificate.PrtnContract.Contractor.Region.FullName : null,
				DistrictId = dto.ByDistrict ? a.PrtnCertificate.PrtnContract.Contractor.DistrictId : 0,
				DistrictName = dto.ByDistrict ? a.PrtnCertificate.PrtnContract.Contractor.District.FullName : null,
				StateAssetType = new StateAssetType()
				{
					TotalApplicationCount = 0,
					CertificateCount = 0,
					StateAssetApplicationCount = 0,
					NewVacanciesCount = 0
				},
				StateAssetType1 = new StateAssetType()
				{
					CertificateCount = a.PrtnCertificate.PrtnContractTypeId == 1 && a.Application.StatusId == StatusIdConst.ACCEPTED ? 1 : 0,
					StateAssetApplicationCount = a.PrtnCertificate.PrtnContractTypeId == 1 && a.Application.StatusId == StatusIdConst.ACCEPTED ? 1 : 0,
					NewVacanciesCount = a.PrtnCertificate.PrtnContractTypeId == 1 && a.Application.StatusId == StatusIdConst.ACCEPTED ? a.PrtnCertificate.PrtnContract.NewVacanciesCount : 0
				},
				StateAssetType2 = new StateAssetType()
				{
					TotalApplicationCount = a.PrtnCertificate.PrtnContractTypeId == 2 ? 1 : 0,
					CertificateCount = a.PrtnCertificate.PrtnContractTypeId == 2 && a.Application.StatusId == StatusIdConst.ACCEPTED ? 1 : 0,
					StateAssetApplicationCount = a.PrtnCertificate.PrtnContractTypeId == 2 && a.Application.StatusId == StatusIdConst.ACCEPTED ? 1 : 0,
					NewVacanciesCount = a.PrtnCertificate.PrtnContractTypeId == 2 && a.Application.StatusId == StatusIdConst.ACCEPTED ? a.PrtnCertificate.PrtnContract.NewVacanciesCount : 0
				},
				StateAssetType3 = new StateAssetType()
				{
					TotalApplicationCount = a.PrtnCertificate.PrtnContractTypeId == 3 ? 1 : 0,
					CertificateCount = a.PrtnCertificate.PrtnContractTypeId == 3 && a.Application.StatusId == StatusIdConst.ACCEPTED ? 1 : 0,
					StateAssetApplicationCount = a.PrtnCertificate.PrtnContractTypeId == 3 && a.Application.StatusId == StatusIdConst.ACCEPTED ? 1 : 0,
					NewVacanciesCount = a.PrtnCertificate.PrtnContractTypeId == 3 && a.Application.StatusId == StatusIdConst.ACCEPTED ? a.PrtnCertificate.PrtnContract.NewVacanciesCount : 0
				}
			})
			.GroupBy(b => new { b.RegionId, b.RegionName, b.DistrictId, b.DistrictName })
			.Select(a => new StateAssetApplicationReportDto()
			{
				RegionId = a.Key.RegionId,
				RegionName = a.Key.RegionName,
				DistrictId = a.Key.DistrictId,
				DistrictName = a.Key.DistrictName,
				StateAssetType = new StateAssetType()
				{
					StateAssetApplicationCount = _unitOfWork.Context.StateAssetApplications.Where(x => (x.Application.RegionId == a.Key.RegionId || x.Application.DistrictId == a.Key.DistrictId) && x.Application.StatusId == StatusIdConst.ACCEPTED).Count(),
					CertificateCount = a.Sum(c => c.StateAssetType1.CertificateCount) + a.Sum(c => c.StateAssetType2.CertificateCount) + a.Sum(c => c.StateAssetType3.CertificateCount),
					NewVacanciesCount = a.Sum(c => c.StateAssetType1.NewVacanciesCount) + a.Sum(c => c.StateAssetType2.NewVacanciesCount) + a.Sum(c => c.StateAssetType3.NewVacanciesCount)
				},
				StateAssetType1 = new StateAssetType()
				{
					TotalApplicationCount = a.Sum(c => c.StateAssetType1.TotalApplicationCount),
					StateAssetApplicationCount = a.Sum(c => c.StateAssetType1.StateAssetApplicationCount),
					CertificateCount = a.Sum(c => c.StateAssetType1.CertificateCount),
					NewVacanciesCount = a.Sum(c => c.StateAssetType1.NewVacanciesCount)
				},
				StateAssetType2 = new StateAssetType()
				{
					TotalApplicationCount = a.Sum(c => c.StateAssetType2.TotalApplicationCount),
					StateAssetApplicationCount = a.Sum(c => c.StateAssetType2.StateAssetApplicationCount),
					CertificateCount = a.Sum(c => c.StateAssetType2.CertificateCount),
					NewVacanciesCount = a.Sum(c => c.StateAssetType2.NewVacanciesCount)
				},
				StateAssetType3 = new StateAssetType()
				{
					TotalApplicationCount = a.Sum(c => c.StateAssetType3.TotalApplicationCount),
					StateAssetApplicationCount = a.Sum(c => c.StateAssetType3.StateAssetApplicationCount),
					CertificateCount = a.Sum(c => c.StateAssetType3.CertificateCount),
					NewVacanciesCount = a.Sum(c => c.StateAssetType3.NewVacanciesCount)
				}
			}).ToList();

			if (dto.ByRegion)
			{
				var regions = _unitOfWork.Context.Regions.ToArray();
				foreach (var region in regions)
				{
					if (!res.Where(a => a.RegionId == region.Id).Any())
					{
						res.Add(new StateAssetApplicationReportDto()
						{
							RegionId = region.Id,
							RegionName = region.FullName,
							AssetApplicationSum = 0,
							StateAssetType = new StateAssetType()
							{
								CertificateCount = 0,
								StateAssetApplicationCount = 0,
								NewVacanciesCount = 0
							},
							StateAssetType1 = new StateAssetType()
							{
								CertificateCount = 0,
								StateAssetApplicationCount = 0,
								NewVacanciesCount = 0
							},
							StateAssetType2 = new StateAssetType()
							{
								CertificateCount = 0,
								StateAssetApplicationCount = 0,
								NewVacanciesCount = 0
							},
							StateAssetType3 = new StateAssetType()
							{
								CertificateCount = 0,
								StateAssetApplicationCount = 0,
								NewVacanciesCount = 0
							}
						});
					}

				}
			}
			else if (dto.ByDistrict)
			{
				var districts = _unitOfWork.Context.Districts.Where(x => x.RegionId == dto.RegionId).ToArray();
				foreach (var district in districts)
				{
					if (!res.Where(a => a.DistrictId == district.Id).Any())
					{
						res.Add(new StateAssetApplicationReportDto()
						{
							DistrictId = district.Id,
							DistrictName = district.FullName,
							AssetApplicationSum = 0,
							StateAssetType = new StateAssetType()
							{
								CertificateCount = 0,
								StateAssetApplicationCount = 0,
								NewVacanciesCount = 0
							},
							StateAssetType1 = new StateAssetType()
							{
								CertificateCount = 0,
								StateAssetApplicationCount = 0,
								NewVacanciesCount = 0
							},
							StateAssetType2 = new StateAssetType()
							{
								CertificateCount = 0,
								StateAssetApplicationCount = 0,
								NewVacanciesCount = 0
							},
							StateAssetType3 = new StateAssetType()
							{
								CertificateCount = 0,
								StateAssetApplicationCount = 0,
								NewVacanciesCount = 0
							}
						});
					}
				}
			}
			return res;

		}
		public (List<SmsLogReportDto>, long) GetSmsLogReportList(SmsLogReportFilterDto options)
		{
			var log = _unitOfWork.Context.Set<SendSmsLog>()
				.Join(_unitOfWork.Context.Set<BusinessmanUser>(),
					sms => sms.PhoneNumer, bus => bus.UserName,
					(sms, bus) => new { smslog = sms, businessman = bus })
				.Where(x =>
					   (!options.HasSearch() || (x.smslog.PhoneNumer.Contains(options.Search.ToLower()) || x.smslog.SmsText.Contains(options.Search.ToLower())))
					&& (!options.FromStatusId.HasValue || x.smslog.FromStatusId == options.FromStatusId)
					&& (!options.ToStatusId.HasValue || x.smslog.ToStatusId == options.ToStatusId)
					&& (options.Inn.IsNullOrEmpty() || x.businessman.Inn == options.Inn))
				.Select(x => new
				{
					x.smslog,
					x.businessman.UserName,
					x.businessman.Inn
				})
				.Skip((options.Page - 1) * options.PageSize)
				.Take(options.PageSize);

			return (log.Select(x => new SmsLogReportDto
			{
				Id = x.smslog.Id,
				CreatedUserId = x.smslog.CreatedUserId,
				CreatedAt = x.smslog.CreatedAt,
				PhoneNumer = x.smslog.PhoneNumer,
				TableId = x.smslog.TableId,
				Inn = x.Inn,
				Table = x.smslog.Table.FullName,
				SmsText = x.smslog.SmsText,
				ErrorText = x.smslog.ErrorText,
				FromStatusId = x.smslog.FromStatusId,
				FromStatus = x.smslog.Status.Translates.AsQueryable()
							.FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name,
								ServiceProvider.CultureHelper.CurrentCulture.Id))
							.TranslateText ?? x.smslog.Status.FullName,
				ToStatusId = x.smslog.ToStatusId,
				ToStatus = x.smslog.ToStatus.Translates.AsQueryable()
							.FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name,
								ServiceProvider.CultureHelper.CurrentCulture.Id))
							.TranslateText ?? x.smslog.ToStatus.FullName,

			}).OrderByDescending(x => x.CreatedAt).ToList(), (long)_unitOfWork.Context.Set<SendSmsLog>().Count());
		}
		public List<CharterMembersRegisterReportDto> GetCharterMembersRegisterReportList(CharterMembersRegisterFilterDto options)
		{
			List<CharterMembersRegisterReportDto> res = new();
			var certificates = _unitOfWork.Context.Set<JoinAntiCorruptionCertificate>()
				.Where(x =>
					   (!options.RegionId.HasValue || x.Contractor.RegionId == options.RegionId)
					&& (!options.DistrictId.HasValue || x.Contractor.DistrictId == options.DistrictId)
					&& (options.Inn.IsNullOrEmpty() || x.Contractor.Inn == options.Inn)
					&& (!options.FromDate.HasValue || x.DocOn >= options.FromDate.Value)
					&& (!options.ToDate.HasValue || x.DocOn <= options.ToDate.Value)
					&& (x.StatusId == StatusIdConst.FORMED));

			return certificates.Select(x => new CharterMembersRegisterReportDto
			{
				Id = x.Id,
				Id2 = x.Id2,
				DocOn = x.DocOn,
				DocNumber = x.DocNumber,
				ContractorId = x.ContractorId,
				ContractorInn = x.Contractor.Inn,
				ContractorRegionId = x.Contractor.RegionId,
				ContractorRegion = x.Contractor.Region.Translates.AsQueryable().FirstOrDefault(
					RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
					.TranslateText ?? x.Contractor.Region.FullName,
				ContractorDistrictId = x.Contractor.DistrictId,
				ContractorDistrict = x.Contractor.District.Translates.AsQueryable().FirstOrDefault(
					DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
					.TranslateText ?? x.Contractor.District.FullName,
				ContractorFullName = x.Contractor.FullName,
				ContractorPhoneNumber = x.JoinAntiCorruptionResultTable.Application.CreatedUser.UserName,
				ContractorOpf = x.Contractor.OpfId.ToString(),
				StatusId = x.StatusId,
				Status = x.Status.Translates.AsQueryable()
							.FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name,
								ServiceProvider.CultureHelper.CurrentCulture.Id))
							.TranslateText ?? x.Status.FullName,
				ExpireOn = x.ExpireOn,
				CancelOn = x.CancelOn
			}).ToList();
		}
		#endregion

		#region SRV
		public List<GetSrvServiceInfoResponseDto> GetSrvServicesInfo(GetSrvServiceInfoRequestDto dto)
		{
			if (_authService.Organization.Id != OrganizationIdConst.SSP)
				dto.RegionId = _authService.Organization.RegionId;

			var serviceApplications = _unitOfWork.Context.ServiceApplication.Where(a => dto.RegionId != null ? a.RegionId == dto.RegionId : true).Select(x => new
			{
				IsFree = x.IsFree,
				DocOn = x.Application.DocOn,
				ContractorId = x.Application.ContractorId,
				AppRegId = x.RegionId != 0 ? x.RegionId : x.Application.RegionId,
				AppDisId = x.DistrictId != 0 ? x.DistrictId : x.Application.DistrictId,
				AppStatusId = x.Application.StatusId
			}).Where(x => x.AppStatusId == StatusIdConst.ACCEPTED).ToArray();

			var plans = _unitOfWork.Context.SrvApplicationYearlyPlans.Where(x => x.StatusId == StatusIdConst.ACCEPTED).Select(a => new
			{
				RegionId = a.RegionId,
				PlanPaidSum = a.Tables.Sum(a => a.Amount) + a.RegionAmount,
				LegalAmount = a.Tables.Sum(a => a.LegalAmount) + a.RegionLegalAmount,
				PlanPaidCount = a.Tables.Sum(a => a.PaidCount) + a.RegionPaidCount,
				PlanFreeCount = a.Tables.Sum(a => a.FreeCount) + a.RegionFreeCount,
				EconomomyAmount = a.Tables.Sum(a => a.EconomyAmount) + a.RegionEconomyAmount
			}).ToArray();


			var secondplans = _unitOfWork.Context.SrvApplicationYearlyPlans.Where(x => x.StatusId == StatusIdConst.ACCEPTED).Select(a => new
			{
				RegionId = a.RegionId,
				PlanPaidSum = a.RegionAmount,
				LegalAmount = a.RegionLegalAmount,
				PlanPaidCount = a.RegionPaidCount,
				PlanFreeCount = a.RegionFreeCount,
				EconomomyAmount = a.RegionEconomyAmount
			}).ToArray();


			var planDistricts = _unitOfWork.Context.SrvApplicationYearlyPlanTables.Where(x => x.Owner.StatusId == StatusIdConst.ACCEPTED).Select(a => new
			{
				DistrictId = a.DistrictId,
				PlanPaidSum = a.Amount,
				PlanPaidCount = a.PaidCount,
				PlanFreeCount = a.FreeCount,
				LegalAmount = a.LegalAmount,
				EconomomyAmount = a.EconomyAmount
			}).ToArray();




			var memshipPaymentOrder = _unitOfWork.Context.MemshipPaymentOrders.Where(x => x.ApplicationTypeId == ApplicationTypeIdConst.SERVICE && x.StatusId == StatusIdConst.ACCEPTED).Select(x => new
			{
				DocOn = x.DocOn,
				RegionId = x.ServiceContract.Application.ServiceApplication.RegionId,
				DistrictId = x.ServiceContract.Application.ServiceApplication.DistrictId,
				ContractorId = x.ContractorId,
				TotalAmount = x.Tables.Sum(a => a.Amount),
				LegalAmount = x.ServiceContract.Groups.Where(a => a.NeedChamberServiceGroup.Id == 16).Sum(c => c.Tables.Sum(d => d.Price)),
				EconomyAmount = x.ServiceContract.Groups.Where(a => (a.NeedChamberServiceGroup.Id != 16 || a.NeedChamberServiceGroup.Id != 18)).Sum(c => c.Tables.Sum(d => d.Price)),
				BirjaAmount = x.ServiceContract.Groups.Where(a => a.NeedChamberServiceGroup.Id == 18).Sum(c => c.Tables.Sum(d => d.Price))
			}).ToArray();

			var memshipPaymentOrder123123 = _unitOfWork.Context.MemshipPaymentOrders.Where(x => x.ApplicationTypeId == ApplicationTypeIdConst.SERVICE && x.StatusId == StatusIdConst.ACCEPTED).Select(x => new
			{
				DocOn = x.DocOn,
				RegionId = x.ServiceContract.Application.ServiceApplication.RegionId,
				DistrictId = x.ServiceContract.Application.ServiceApplication.DistrictId,
				StatusId = x.ServiceContract.Application.StatusId,
				ContractorId = x.ContractorId,
				TotalAmount = x.Tables.Sum(a => a.Amount),
				LegalAmount = x.ServiceContract.Groups.Where(a => a.NeedChamberServiceGroup.Id == 11).Sum(c => c.Tables.Sum(d => d.Price)),
				EconomyAmount = x.ServiceContract.Groups.Where(a => (a.NeedChamberServiceGroup.Id != 11 || a.NeedChamberServiceGroup.Id != 9)).Sum(c => c.Tables.Sum(d => d.Price)),
				BirjaAmount = x.ServiceContract.Groups.Where(a => a.NeedChamberServiceGroup.Id == 9).Sum(c => c.Tables.Sum(d => d.Price))
			}).ToArray();

			var data = new List<GetSrvServiceInfoResponseDto>();
			var serviceContracts = _unitOfWork.Context.ServiceContract.Include(a => a.Application).ThenInclude(b => b.ServiceApplication).Select(a => new
			{
				DocOn = a.DocOn,
				ContractorId = a.ContractorId,
				RegionId = a.Application.RegionId,
				StatusId = a.StatusId,
				DistrictId = a.Application.DistrictId,
				Sum = a.Groups.Sum(b => b.Tables.Sum(c => c.Price))
			}).ToArray();

			if (dto.FromDocDate.HasValue)
			{
				serviceApplications = serviceApplications.Where(x => x.DocOn >= dto.FromDocDate).ToArray();
				//serviceContracts = serviceContracts.Where(x => x.DocOn >= dto.FromDocDate).ToArray();
				memshipPaymentOrder = memshipPaymentOrder.Where(x => x.DocOn >= dto.FromDocDate).ToArray();
			}

			if (dto.ToDocDate.HasValue)
			{
				serviceApplications = serviceApplications.Where(x => x.DocOn <= dto.ToDocDate).ToArray();
				//serviceContracts = serviceContracts.Where(x => x.DocOn < dto.ToDocDate).ToArray();
				memshipPaymentOrder = memshipPaymentOrder.Where(x => x.DocOn <= dto.ToDocDate).ToArray();
			}

			if (dto.ByRegion)
			{
				var regions = _unitOfWork.Context.Regions.Where(x => x.StateId == StateIdConst.ACTIVE).Include(x => x.Translates).ToArray();
				foreach (var item in regions)
				{
					decimal per = plans.Where(x => x.RegionId == item.Id).Sum(x => x.PlanPaidSum) == 0 ? 0 : memshipPaymentOrder.Where(x => x.RegionId == item.Id).Sum(x => x.TotalAmount) * 100 / plans.Where(x => x.RegionId == item.Id).Sum(x => x.PlanPaidSum);
					decimal perCount = plans.Where(x => x.RegionId == item.Id).Sum(x => x.PlanPaidCount) == 0 ? 0 : (decimal)memshipPaymentOrder.Where(x => x.RegionId == item.Id).Count() * 100 / (decimal)plans.Where(x => x.RegionId == item.Id).Sum(x => x.PlanPaidCount);
					decimal freePer = plans.Where(x => x.RegionId == item.Id).Sum(x => x.PlanFreeCount) == 0 ? 0 : (decimal)serviceApplications.Where(x => x.AppRegId == item.Id && x.IsFree).Count() * 100 / plans.Where(x => x.RegionId == item.Id).Sum(x => x.PlanFreeCount);
					data.Add(new GetSrvServiceInfoResponseDto()
					{
						RegionId = item.Id,
						RegionOrderCode = item.OrderCode,
						Region = item.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? item.FullName,
						PlanPaidCount = plans.Where(x => x.RegionId == item.Id).Sum(x => x.PlanPaidCount),
						PlanPaidSum = plans.Where(x => x.RegionId == item.Id).Sum(x => x.PlanPaidSum),
						PlanFreeCount = plans.Where(x => x.RegionId == item.Id).Sum(x => x.PlanFreeCount),
						EconomomyAmount = plans.Where(x => x.RegionId == item.Id).Sum(x => x.EconomomyAmount),
						LegalAmount = plans.Where(x => x.RegionId == item.Id).Sum(x => x.LegalAmount),
						TotalMemshipPaymentOrderCount = new TotalMemshipPaymentOrder()
						{
							TotalCount = memshipPaymentOrder.Where(x => x.RegionId == item.Id).Count(),
							TotalSum = memshipPaymentOrder.Where(x => x.RegionId == item.Id).Sum(x => x.EconomyAmount + x.LegalAmount + x.BirjaAmount),
							EconomomyAmount = memshipPaymentOrder.Where(x => x.RegionId == item.Id).Sum(x => x.EconomyAmount),
							LegalAmount = memshipPaymentOrder.Where(x => x.RegionId == item.Id).Sum(x => x.LegalAmount),
							BirjaAmount = memshipPaymentOrder.Where(x => x.RegionId == item.Id).Sum(x => x.BirjaAmount),
						},
						AcceptedPaidSumPercentage = Math.Round(per, 2),
						PaidCoef = Math.Round(per / 100, 6),
						FreeCoef = 0,
						AcceptedFreeCount = serviceApplications.Where(x => x.AppRegId == item.Id && x.IsFree).Count(),
						AcceptedPaidCountPercentage = Math.Round(perCount, 2),
						AcceptedFreePercentage = Math.Round(freePer, 2)
					});
				}

				return data.OrderBy(x => x.RegionOrderCode).ToList();
			}

			if (dto.ByDistrict)
			{
				var districts = _unitOfWork.Context.Districts.Where(x => x.RegionId == dto.RegionId && x.StateId == StateIdConst.ACTIVE).Include(x => x.Translates).ToArray();

				foreach (var item in districts)
				{
					decimal per = planDistricts.Where(x => x.DistrictId == item.Id).Sum(x => x.PlanPaidSum) == 0 ? 0 : memshipPaymentOrder.Where(x => x.DistrictId == item.Id).Sum(a => a.TotalAmount) * 100 / planDistricts.Where(x => x.DistrictId == item.Id).Sum(x => x.PlanPaidSum);
					decimal perCount = planDistricts.Where(x => x.DistrictId == item.Id).Sum(x => x.PlanPaidCount) == 0 ? 0 : memshipPaymentOrder.Where(x => x.DistrictId == item.Id).Count() * 100 / planDistricts.Where(x => x.DistrictId == item.Id).Sum(x => x.PlanPaidCount);
					decimal freePer = planDistricts.Where(x => x.DistrictId == item.Id).Sum(x => x.PlanFreeCount) == 0 ? 0 : (decimal)serviceApplications.Where(x => x.AppDisId == item.Id && x.IsFree).Count() * 100 / planDistricts.Where(x => x.DistrictId == item.Id).Sum(x => x.PlanFreeCount);
					data.Add(new GetSrvServiceInfoResponseDto()
					{
						RegionId = dto.RegionId,
						DistrictId = item.Id,
						DistrictOrderCode = item.OrderCode,
						District = item.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? item.FullName,
						PlanPaidCount = planDistricts.Where(x => x.DistrictId == item.Id).Sum(x => x.PlanPaidCount),
						PlanPaidSum = planDistricts.Where(x => x.DistrictId == item.Id).Sum(x => x.PlanPaidSum),
						PlanFreeCount = planDistricts.Where(x => x.DistrictId == item.Id).Sum(x => x.PlanFreeCount),
						EconomomyAmount = planDistricts.Where(x => x.DistrictId == item.Id).Sum(x => x.EconomomyAmount),
						LegalAmount = planDistricts.Where(x => x.DistrictId == item.Id).Sum(x => x.LegalAmount),
						TotalMemshipPaymentOrderCount = new TotalMemshipPaymentOrder()
						{
							TotalCount = memshipPaymentOrder.Where(x => x.DistrictId == item.Id).Count(),
							TotalSum = memshipPaymentOrder.Where(x => x.DistrictId == item.Id).Sum(x => x.EconomyAmount + x.LegalAmount + x.BirjaAmount),
							EconomomyAmount = memshipPaymentOrder.Where(x => x.DistrictId == item.Id).Sum(x => x.EconomyAmount),
							LegalAmount = memshipPaymentOrder.Where(x => x.DistrictId == item.Id).Sum(x => x.LegalAmount),
							BirjaAmount = memshipPaymentOrder.Where(x => x.DistrictId == item.Id).Sum(x => x.BirjaAmount),
						},
						AcceptedPaidSumPercentage = Math.Round(per, 2),
						PaidCoef = Math.Round(per / 100, 6),
						FreeCoef = 0,
						AcceptedFreeCount = serviceApplications.Where(x => x.AppDisId == item.Id && x.IsFree).Count(),
						AcceptedPaidCountPercentage = Math.Round(perCount, 2),
						AcceptedFreePercentage = Math.Round(freePer, 2),
					});
				}
				var region = _unitOfWork.Context.Regions.Include(x => x.Translates).FirstOrDefault(x => x.StateId == StateIdConst.ACTIVE && x.Id == dto.RegionId);

				decimal perregion = plans.Where(x => x.RegionId == region.Id).Sum(x => x.PlanPaidSum) == 0 ? 0 : memshipPaymentOrder.Where(x => x.RegionId == region.Id).Sum(x => x.TotalAmount) * 100 / plans.Where(x => x.RegionId == region.Id).Sum(x => x.PlanPaidSum);
				decimal perCountregion = plans.Where(x => x.RegionId == region.Id).Sum(x => x.PlanPaidCount) == 0 ? 0 : (decimal)memshipPaymentOrder.Where(x => x.RegionId == region.Id).Count() * 100 / (decimal)plans.Where(x => x.RegionId == region.Id).Sum(x => x.PlanPaidCount);
				decimal freePerregion = plans.Where(x => x.RegionId == dto.RegionId).Sum(x => x.PlanFreeCount) == 0 ? 0 : (decimal)serviceApplications.Where(x => x.AppRegId == region.Id && x.IsFree).Count() * 100 / plans.Where(x => x.RegionId == region.Id).Sum(x => x.PlanFreeCount);

				data.Add(new GetSrvServiceInfoResponseDto()
				{
					RegionId = region.Id,
					RegionOrderCode = region.OrderCode,
					Region = region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? region.FullName,
					PlanPaidCount = secondplans.Where(x => x.RegionId == region.Id).Sum(x => x.PlanPaidCount),
					PlanPaidSum = secondplans.Where(x => x.RegionId == region.Id).Sum(x => x.PlanPaidSum),
					PlanFreeCount = secondplans.Where(x => x.RegionId == region.Id).Sum(x => x.PlanFreeCount),
					EconomomyAmount = secondplans.Where(x => x.RegionId == region.Id).Sum(x => x.EconomomyAmount),
					LegalAmount = secondplans.Where(x => x.RegionId == region.Id).Sum(x => x.LegalAmount),
					TotalMemshipPaymentOrderCount = new TotalMemshipPaymentOrder()
					{
						TotalCount = memshipPaymentOrder123123.Where(x => x.RegionId == region.Id && x.DistrictId == null).Count(),
						TotalSum = memshipPaymentOrder123123.Where(x => x.RegionId == region.Id && x.DistrictId == null).Sum(x => x.EconomyAmount + x.LegalAmount + x.BirjaAmount),
						EconomomyAmount = memshipPaymentOrder123123.Where(x => x.RegionId == region.Id && x.DistrictId == null).Sum(x => x.EconomyAmount),
						LegalAmount = memshipPaymentOrder123123.Where(x => x.RegionId == region.Id && x.DistrictId == null).Sum(x => x.LegalAmount),
						BirjaAmount = memshipPaymentOrder123123.Where(x => x.RegionId == region.Id && x.DistrictId == null).Sum(x => x.BirjaAmount),
					},
					AcceptedPaidSumPercentage = Math.Round(perregion, 2),
					PaidCoef = Math.Round(perregion / 100, 6),
					FreeCoef = 0,
					AcceptedFreeCount = serviceApplications.Where(x => x.AppRegId == region.Id && x.IsFree).Count(),
					AcceptedPaidCountPercentage = Math.Round(perCountregion, 2),
					AcceptedFreePercentage = Math.Round(freePerregion, 2)
				});

				return data.OrderBy(x => x.DistrictOrderCode).ToList();
			}

			if (dto.ByContractor)
			{
				var contractors = _unitOfWork.Context.Contractors.Where(x => x.DistrictId == dto.DistrictId).ToArray();

				foreach (var item in contractors)
				{
					if (memshipPaymentOrder.Where(x => x.ContractorId == item.Id).Count() > 0)
					{
						data.Add(new GetSrvServiceInfoResponseDto()
						{
							ContractorId = item.Id,
							ContractorInn = item.Inn,
							Contractor = item.FullName,
							TotalFreeApplicationCount = serviceApplications.Where(x => x.ContractorId == item.Id && x.IsFree).Count(),
							TotalPaidApplicationCount = serviceApplications.Where(x => x.ContractorId == item.Id && !x.IsFree).Count(),
							RejectedApplicationCount = serviceApplications.Where(x => x.ContractorId == item.Id && x.AppStatusId == StatusIdConst.REJECTED).Count(),
							TotalSignedCertificateCount = serviceContracts.Where(x => x.ContractorId == item.Id && x.StatusId == StatusIdConst.SIGNED).Count(),
							TotalSignedCertificateSum = serviceContracts.Where(x => x.ContractorId == item.Id && x.StatusId == StatusIdConst.SIGNED).Count(),
							TotalMemshipPaymentOrderCount = new TotalMemshipPaymentOrder()
							{
								TotalCount = memshipPaymentOrder.Where(x => x.ContractorId == item.Id).Count(),
								TotalSum = memshipPaymentOrder.Where(x => x.ContractorId == item.Id).Sum(x => x.EconomyAmount + x.LegalAmount + x.BirjaAmount),
								EconomomyAmount = memshipPaymentOrder.Where(x => x.ContractorId == item.Id).Sum(x => x.EconomyAmount),
								LegalAmount = memshipPaymentOrder.Where(x => x.ContractorId == item.Id).Sum(x => x.LegalAmount),
								BirjaAmount = memshipPaymentOrder.Where(x => x.ContractorId == item.Id).Sum(x => x.BirjaAmount),
							}
						});
					}
				}
			}
			return data;
		}
		#endregion

		#region CallCenter
		public List<CallCenterAppealReportByOkedTypeDto> CallCenterAppealReportByOkedType(CallCenterAppealReportByOkedTypeFilter options)
		{
			var languageId = _cultureHelper.CurrentCulture.Id;
			var result = _unitOfWork.Context.CallCenterAppealReportByOkedType(
				 pr_by_oked_type_id: options.ByOkedType,
				 pr_from_doc_date: options.FromDocOn,
				 pr_to_doc_date: options.ToDocOn,
				 pr_language_id: languageId
				 ).ToList();
			return result;
		}
		public List<CallCenterReportByWeekDto> GetCallCenterReportByWeek(CallCenterReportByWeekFilterDto options)
		{
			var query = _unitOfWork.Context.Set<CallCenterAppeal>()
												  //.Include(r => r.Region)
												  //.ThenInclude(rt => rt.Translates)
												  //.Include(d => d.District)
												  //.ThenInclude(dt => dt.Translates)
												  .Where(c => c.StatusId != StatusIdConst.DELETED);

			#region Filters
			//if (_authService.User.OrganizationId != OrganizationIdConst.SSP)
			//    options.RegionId = _authService.Organization.RegionId;

			if (options.RegionId.HasValue)
				query = query.Where(x => x.RegionId == options.RegionId);

			if (options.DistrictId.HasValue)
				query = query.Where(x => x.DistrictId == options.DistrictId);

			if (options.FromDay.HasValue)
				query = query.Where(x => x.CreatedAt.Date.AsDateOnly() >= options.FromDay.Value);

			if (options.ToDay.HasValue)
				query = query.Where(x => x.CreatedAt.Date.AsDateOnly() <= options.ToDay.Value);

			#endregion


			DateTime startWorkDay = DateTime.Today.AddHours(9); // 9:00 AM
			DateTime endWorkDay = DateTime.Today.AddHours(18);  // 6:00 PM
			List<CallCenterReportByWeekDto> result;
			if (options.ByWeek)
				result = query
					 .GroupBy(c => c.CreatedAt.DayOfWeek)
					 .Select(gr => new CallCenterReportByWeekDto
					 {
						 WeekDay = (int)gr.Key,
						 WeekDayName = gr.Key.ToString(),
						 Total = gr.Count(),

						 InWorkTime = gr.Count(c => c.CreatedAt.Hour >= startWorkDay.Hour
							 && (c.CreatedAt.Hour > startWorkDay.Hour || c.CreatedAt.Minute >= startWorkDay.Minute)
							 && c.CreatedAt.Hour <= endWorkDay.Hour
							 && (c.CreatedAt.Hour < endWorkDay.Hour || c.CreatedAt.Minute <= endWorkDay.Minute)),

						 OutWorkTime = gr.Count(c => c.CreatedAt.Hour < startWorkDay.Hour
							 || c.CreatedAt.Hour > endWorkDay.Hour
							 || (c.CreatedAt.Hour == startWorkDay.Hour && c.CreatedAt.Minute < startWorkDay.Minute)
							 || (c.CreatedAt.Hour == endWorkDay.Hour && c.CreatedAt.Minute > endWorkDay.Minute)),

					 }).ToList();
			else
				result = query
					.GroupBy(c => c.CreatedUserId)
					.Select(gr => new CallCenterReportByWeekDto
					{
						//WeekDay = (int)gr.Key,
						//WeekDayName = gr.Key.ToString(),
						UserId = gr.Key.Value,
						UserFullName = gr.FirstOrDefault().CreatedUser.Person.FullName ?? gr.FirstOrDefault().CreatedUser.UserName,
						Total = gr.Count(),

						InWorkTime = gr.Count(c => c.CreatedAt.Hour >= startWorkDay.Hour
							&& (c.CreatedAt.Hour > startWorkDay.Hour || c.CreatedAt.Minute >= startWorkDay.Minute)
							&& c.CreatedAt.Hour <= endWorkDay.Hour
							&& (c.CreatedAt.Hour < endWorkDay.Hour || c.CreatedAt.Minute <= endWorkDay.Minute)),

						OutWorkTime = gr.Count(c => c.CreatedAt.Hour < startWorkDay.Hour
							|| c.CreatedAt.Hour > endWorkDay.Hour
							|| (c.CreatedAt.Hour == startWorkDay.Hour && c.CreatedAt.Minute < startWorkDay.Minute)
							|| (c.CreatedAt.Hour == endWorkDay.Hour && c.CreatedAt.Minute > endWorkDay.Minute)),

					}).ToList();


			var total = result.Sum(x => x.Total);
			var inTotal = result.Sum(x => x.InWorkTime);
			var outTotal = result.Sum(x => x.OutWorkTime);
			List<CallCenterReportByWeekDto> totalResult = new();
			totalResult.Add(new()
			{
				UserId = -1,
				WeekDay = -1,
				WeekDayName = "Jami: ",
				UserFullName = "Jami: ",
				Total = total,
				TotalPercentage = 100,
				InWorkTime = inTotal,
				OutWorkTime = outTotal
			});
			foreach (var item in result.OrderBy(x => options.ByWeek ? x.WeekDay : x.Total))
			{
				if (item.Total.HasValue && total.HasValue && total.Value != 0)
					item.TotalPercentage = Math.Round(100m * item.Total.Value / total.Value, 2);
				else
					item.TotalPercentage = 0;
				totalResult.Add(item);
			}


			return totalResult;
		}
		public List<CallCenterDto> GetCallCenterByRegion(CallCenterDtoFilter dto)
		{
			var dataList = _unitOfWork.Context.Set<CallCenterAppeal>()
												  .Include(r => r.Region)
												  .ThenInclude(rt => rt.Translates)
												  .Include(d => d.District)
												  .ThenInclude(dt => dt.Translates)
												  .Where(c => c.StatusId != StatusIdConst.DELETED);

			if (dto.StartDate.HasValue)
			{
				dataList = dataList.Where(a => a.DocOn >= dto.StartDate.Value);
			}

			if (dto.EndDate.HasValue)
			{
				dataList = dataList.Where(a => a.DocOn < dto.EndDate.Value);
			}
			if (dto.ReportType)
			{
				if (dto.ByRegion)
				{
					var totalCallCenterCount = dataList.Count();

					var reportData = dataList
										 .GroupBy(c => new { c.RegionId, c.Region.FullName, c.Region.OrderCode })
										 .Select(group => new CallCenterDto
										 {
											 RegionId = group.Key.RegionId,
											 Region = group.Key.FullName,
											 RegionOrderCode = group.Key.OrderCode,
											 TotalCallCenter = totalCallCenterCount,

											 TotalCallCenterAppeal = group.Count(),
											 TotalCallCenterAppealPercent = (group.Count() * 100) / totalCallCenterCount,

											 TotalPhysicalCount = group.Where(a => a.AppealTypeArriveId == AppealTypeArriveIdConst.PhysicalPerson).Count(),
											 TotalPhysicalCountPercent = (group.Where(a => a.AppealTypeArriveId == AppealTypeArriveIdConst.PhysicalPerson).Count() * 100) / totalCallCenterCount,

											 TotalLegalCount = group.Where(a => a.AppealTypeArriveId == AppealTypeArriveIdConst.LegalContractor).Count(),
											 TotalLegalCountPercent = (group.Where(a => a.AppealTypeArriveId == AppealTypeArriveIdConst.LegalContractor).Count() * 100) / totalCallCenterCount,

											 TotalInvestorCount = group.Where(a => a.AppealTypeArriveId == AppealTypeArriveIdConst.Investor).Count(),
											 TotalInvestorPercent = (group.Where(a => a.AppealTypeArriveId == AppealTypeArriveIdConst.Investor).Count() * 100) / totalCallCenterCount,

											 TotalPhysicalContractorCount = group.Where(a => a.AppealTypeArriveId == AppealTypeArriveIdConst.PhysicalContractor).Count(),
											 TotalPhysicalContractorPercent = (group.Where(a => a.AppealTypeArriveId == AppealTypeArriveIdConst.PhysicalContractor).Count() * 100) / totalCallCenterCount,
										 }).OrderBy(a => a.RegionOrderCode)
										 .ToList();
					return reportData;
				}
				else if (dto.ByDistrict)
				{
					var totalCallCenterCount = dataList.Count();

					if (dto.RegionId.HasValue)
					{
						dataList = dataList.Where(a => a.RegionId == dto.RegionId.Value);
						if (dto.DistrictId.HasValue)
						{
							dataList = dataList.Where(a => a.DistrictId == dto.DistrictId.Value);
						}
					}

					var reportData = dataList
										 .GroupBy(c => new { c.RegionId, RegionName = c.Region.FullName, c.DistrictId, DistrictName = c.District.FullName, c.District.OrderCode })
										 .Select(group => new CallCenterDto
										 {
											 RegionId = group.Key.RegionId,
											 Region = group.Key.RegionName,
											 DistrictId = group.Key.DistrictId,
											 District = group.Key.DistrictName,
											 DistrictOrderCode = group.Key.OrderCode,
											 TotalCallCenter = totalCallCenterCount,

											 TotalCallCenterAppeal = group.Count(),
											 TotalCallCenterAppealPercent = (group.Count() * 100) / totalCallCenterCount,

											 TotalPhysicalCount = group.Where(a => a.AppealTypeArriveId == AppealTypeArriveIdConst.PhysicalPerson).Count(),
											 TotalPhysicalCountPercent = (group.Where(a => a.AppealTypeArriveId == AppealTypeArriveIdConst.PhysicalPerson).Count() * 100) / totalCallCenterCount,

											 TotalLegalCount = group.Where(a => a.AppealTypeArriveId == AppealTypeArriveIdConst.LegalContractor).Count(),
											 TotalLegalCountPercent = (group.Where(a => a.AppealTypeArriveId == AppealTypeArriveIdConst.LegalContractor).Count() * 100) / totalCallCenterCount,

											 TotalInvestorCount = group.Where(a => a.AppealTypeArriveId == AppealTypeArriveIdConst.Investor).Count(),
											 TotalInvestorPercent = (group.Where(a => a.AppealTypeArriveId == AppealTypeArriveIdConst.Investor).Count() * 100) / totalCallCenterCount,

											 TotalPhysicalContractorCount = group.Where(a => a.AppealTypeArriveId == AppealTypeArriveIdConst.PhysicalContractor).Count(),
											 TotalPhysicalContractorPercent = (group.Where(a => a.AppealTypeArriveId == AppealTypeArriveIdConst.PhysicalContractor).Count() * 100) / totalCallCenterCount,
										 }).OrderBy(a => a.DistrictOrderCode)
										 .ToList();
					return reportData;
				}
			}
			else
			{
				if (dto.ByRegion)
				{
					var totalCallCenterCount = dataList.Count();

					var reportData = dataList
										 .GroupBy(c => new { c.RegionId, c.Region.FullName, c.Region.OrderCode })
										 .Select(group => new CallCenterDto
										 {
											 RegionId = group.Key.RegionId,
											 Region = group.Key.FullName,
											 RegionOrderCode = group.Key.OrderCode,
											 TotalCallCenter = totalCallCenterCount,

											 TotalCallCenterAppeal = group.Count(),
											 TotalCallCenterAppealPercent = (group.Count() * 100) / totalCallCenterCount,

											 TotalMikroContractorCount = group.Where(a => a.ContractorCategoryId == ContractorCategoryIdConst.MIKROFIRMA).Count(),
											 TotalMikroContractorCountPercent = (group.Where(a => a.ContractorCategoryId == ContractorCategoryIdConst.MIKROFIRMA).Count() * 100) / totalCallCenterCount,

											 TotalLitteContractorCount = group.Where(a => a.ContractorCategoryId == ContractorCategoryIdConst.KICHIK_KORXONA).Count(),
											 TotalLittleContractorCountPercent = (group.Where(a => a.ContractorCategoryId == ContractorCategoryIdConst.KICHIK_KORXONA).Count() * 100) / totalCallCenterCount,

											 TotalMiddleContractorCount = group.Where(a => a.ContractorCategoryId == ContractorCategoryIdConst.URTA_KORHONA).Count(),
											 TotalMiddleContractorCountPercent = (group.Where(a => a.ContractorCategoryId == ContractorCategoryIdConst.URTA_KORHONA).Count() * 100) / totalCallCenterCount,

											 TotalHigheContractorCount = group.Where(a => a.ContractorCategoryId == ContractorCategoryIdConst.YIRIK_KORXONA).Count(),
											 TotalHigheContractorCountPercent = (group.Where(a => a.ContractorCategoryId == ContractorCategoryIdConst.YIRIK_KORXONA).Count() * 100) / totalCallCenterCount,
										 }).OrderBy(a => a.RegionOrderCode)
										 .ToList();
					return reportData;
				}
				else if (dto.ByDistrict)
				{
					var totalCallCenterCount = dataList.Count();

					if (dto.RegionId.HasValue)
					{
						dataList = dataList.Where(a => a.RegionId == dto.RegionId.Value);
						if (dto.DistrictId.HasValue)
						{
							dataList = dataList.Where(a => a.DistrictId == dto.DistrictId.Value);
						}
					}

					var reportData = dataList
										 .GroupBy(c => new { c.RegionId, RegionName = c.Region.FullName, c.DistrictId, DistrictName = c.District.FullName, c.District.OrderCode })
										 .Select(group => new CallCenterDto
										 {
											 RegionId = group.Key.RegionId,
											 Region = group.Key.RegionName,
											 DistrictId = group.Key.DistrictId,
											 District = group.Key.DistrictName,
											 DistrictOrderCode = group.Key.OrderCode,
											 TotalCallCenter = totalCallCenterCount,

											 TotalCallCenterAppeal = group.Count(),
											 TotalCallCenterAppealPercent = (group.Count() * 100) / totalCallCenterCount,

											 TotalMikroContractorCount = group.Where(a => a.ContractorCategoryId == ContractorCategoryIdConst.MIKROFIRMA).Count(),
											 TotalMikroContractorCountPercent = (group.Where(a => a.ContractorCategoryId == ContractorCategoryIdConst.MIKROFIRMA).Count() * 100) / totalCallCenterCount,

											 TotalLitteContractorCount = group.Where(a => a.ContractorCategoryId == ContractorCategoryIdConst.KICHIK_KORXONA).Count(),
											 TotalLittleContractorCountPercent = (group.Where(a => a.ContractorCategoryId == ContractorCategoryIdConst.KICHIK_KORXONA).Count() * 100) / totalCallCenterCount,

											 TotalMiddleContractorCount = group.Where(a => a.ContractorCategoryId == ContractorCategoryIdConst.URTA_KORHONA).Count(),
											 TotalMiddleContractorCountPercent = (group.Where(a => a.ContractorCategoryId == ContractorCategoryIdConst.URTA_KORHONA).Count() * 100) / totalCallCenterCount,

											 TotalHigheContractorCount = group.Where(a => a.ContractorCategoryId == ContractorCategoryIdConst.YIRIK_KORXONA).Count(),
											 TotalHigheContractorCountPercent = (group.Where(a => a.ContractorCategoryId == ContractorCategoryIdConst.YIRIK_KORXONA).Count() * 100) / totalCallCenterCount,
										 }).OrderBy(a => a.DistrictOrderCode)
										 .ToList();
					return reportData;
				}
			}

			return new List<CallCenterDto>();
		}
		//public List<ExpiredContractorsReportDto> GetExpiredContractorsReport(PrtnDocumentSortFilterOptions options)
		//{
		//    var newData = GetPrtnApplicationAndContractInfo(new PrtnApplicationAndContractInfoDtoFilter()
		//    {
		//        ByRegion = true,
		//    });

		//    var query = _applicationRepository.ReadAsNoTracked<SspUis.BizLogicLayer.ApplicationServices.ApplicationListDto>(applyFilter: false)
		//        .Where(a => a.ApplicationTypeId == ApplicationTypeIdConst.PARTNER && a.StatusId != StatusIdConst.REJECTED)
		//        .SortFilter2(options);


		//    IQueryable<PrtnContract> prtnQuery = _unitOfWork.Context.Set<PrtnContract>().Include(q => q.PrtnCertificate).Include(a => a.Signs).AsQueryable();


		//    List<ExpiredContractorsReportDto> newRportDtos = new List<ExpiredContractorsReportDto>();


		//    if (options.RegionId != null && options.DistrictId != null)
		//    {
		//        var elements = query.Where(a => a.ContractorDistrictId == options.DistrictId);
		//        foreach (var item in elements)
		//        {
		//            var cureContractor = item;
		//            var curPrtQuery = cureContractor.PrtnContract;


		//            ExpiredContractorsReportDto newRportDto = new ExpiredContractorsReportDto();
		//            newRportDto.ContractorInn = cureContractor.ContractorInn;
		//            newRportDto.District = cureContractor.ContractorDistrict;
		//            newRportDto.DistrictId = cureContractor.ContractorDistrictId.Value;
		//            newRportDto.Contractor = cureContractor.Contractor;
		//            newRportDto.ContractorId = (int)cureContractor.ContractorId.Value;
		//            newRportDto.AllApplicationsCount = 1; //hamma shartnomalar
		//            newRportDto.IsBeingCosideredApplicationCount = 1;  //korib chiqishga yuborilgan arizalar soni

		//            TimeSpan ExpiredMahallaDate = TimeSpan.FromDays(NewReportConst.ExpiredMahallaDate);


		//            //var ExpiredIsBeingCosideredApplicationCount1 = cureContractors.Where(c => c.StatusId == StatusIdConst.ACCEPTED &&
		//            //   c.PrtnApplication.PassExpertiseExpireOn - c.PrtnApplication.StatusChangeExpireOn.Value > ExpiredMahallaDate).Count();

		//            var ExpiredIsBeingCosideredApplicationCount1 = (cureContractor.StatusId == StatusIdConst.ACCEPTED && (cureContractor.PrtnApplication.PassExpertiseExpireOn - cureContractor.PrtnApplication.StatusChangeExpireOn) > ExpiredMahallaDate) ? 1 : 0;

		//            var ExpiredIsBeingCosideredApplicationCount12 = (cureContractor.StatusId == StatusIdConst.SENT_FOR_REVIEW && (DateTime.Now - cureContractor.PrtnApplication.StatusChangeExpireOn) > ExpiredMahallaDate) ? 1 : 0;

		//            newRportDto.ExpiredIsBeingCosideredApplicationCount = ExpiredIsBeingCosideredApplicationCount1 + ExpiredIsBeingCosideredApplicationCount12;  //korib chiqishi kechikayotgan yoki kechikib o'tkazilgan arizalar soni

		//            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//            TimeSpan ExpiredExpertizeDate = TimeSpan.FromDays(NewReportConst.ExpiredExpertizeDate);

		//            newRportDto.SendToExpertiseCount = curPrtQuery != null ? 1 : 0; // ekspertizaga kelgan shartnomalar
		//            if (curPrtQuery == null) continue;

		//            var ExpiredSendToExpertiseCount1 = (curPrtQuery.StatusId == StatusIdConst.SENT_FOR_EXPERTISE && curPrtQuery.StatusChangeExpireOn != null && (DateTime.Now - curPrtQuery.StatusChangeExpireOn) > ExpiredExpertizeDate) ? 1 : 0;


		//            var ExpiredSendToExpertiseCount2 = (curPrtQuery.StatusId != StatusIdConst.SENT_FOR_EXPERTISE && (curPrtQuery.StatusChangeExpireOn - curPrtQuery.PassExpertiseExpireOn) > ExpiredExpertizeDate) ? 1 : 0;  //or notpass createat vaqtincha

		//            var ExpiredSendToExpertiseCount3 = (curPrtQuery.StatusId != StatusIdConst.SENT_FOR_EXPERTISE && (curPrtQuery.StatusChangeExpireOn - curPrtQuery.NotPassExpertiseExpireOn) > ExpiredExpertizeDate) ? 1 : 0;//or pass createat vaqtincha

		//            newRportDto.ExpiredSendToExpertiseCount = ExpiredSendToExpertiseCount1 + ExpiredSendToExpertiseCount2 + ExpiredSendToExpertiseCount3;  //otgan va otmgan yoki kechikkanlar

		//            //////////////////////////////////////////////////////////////////////////////////
		//            TimeSpan ExpiredRecentExpertizeDate = TimeSpan.FromDays(NewReportConst.ExpiredRecentExpertizeDate);

		//            // var NotPassCount = curPrtQuery.Where(a => a.NotPassExpertiseExpireOn != null).OrderBy(a=>a.StatusId).ToList();  //expertizadan otmagan shartnomalar

		//            newRportDto.NotPassCount = curPrtQuery.NotPassExpertiseExpireOn != null ? 1 : 0; //expertizadan otmagan shartnomalar

		//            var ExpiredResentToExpiredCount1 = (curPrtQuery.StatusId == StatusIdConst.NOT_PASS_EXPERTISE && (DateTime.Now - curPrtQuery.NotPassExpertiseExpireOn.Value) > ExpiredRecentExpertizeDate) ? 1 : 0;  //ekspertizadan otmagan va qayta yuborlishi kechikayotgan


		//            var ExpiredResentToExpiredCount2 = (curPrtQuery.NotPassExpertiseExpireOn != null && (curPrtQuery.StatusChangeExpireOn - curPrtQuery.NotPassExpertiseExpireOn) > ExpiredRecentExpertizeDate) ? 1 : 0; //ekpertizaga qayta yuborilgan ammo kechikkan
		//            //ok
		//            newRportDto.ExpiredResentToExpiredCount = ExpiredResentToExpiredCount1 + ExpiredResentToExpiredCount2;

		//            ///////////////////////////////////////////////////////////////////////
		//            TimeSpan ExpiredPassToSignDate = TimeSpan.FromDays(NewReportConst.ExpiredPassToSignDate);

		//            newRportDto.PassCount1 = (curPrtQuery.PassExpertiseExpireOn != null && (curPrtQuery.PrtnContractTypeId == 1 || curPrtQuery.PrtnContractTypeId == 2)) ? 1 : 0;  //ekpertizadan otgan shartnomalar 50-100 101-200


		//            var SignExpireOnCount1 = (curPrtQuery.StatusId == StatusIdConst.PASS_EXPERTISE && (curPrtQuery.PrtnContractTypeId == 1 || curPrtQuery.PrtnContractTypeId == 2) && (DateTime.Now - curPrtQuery.PassExpertiseExpireOn) > ExpiredPassToSignDate) ? 1 : 0;  //ekspaerizadan otgan ammo imzolash kechikayotgan

		//            var SignExpireOnCount2 = (curPrtQuery.SignExpireOn != null && (curPrtQuery.PrtnContractTypeId == 1 || curPrtQuery.PrtnContractTypeId == 2) && (curPrtQuery.SignExpireOn - curPrtQuery.PassExpertiseExpireOn) > ExpiredPassToSignDate) ? 1 : 0;  //ekspaerizadan otgan va imzolangan ammo kechikkan 50-100 101-200

		//            newRportDto.SignExpireOnCount1 = SignExpireOnCount1 + SignExpireOnCount2;


		//            //////////////////////////////////////////////////////////////////
		//            TimeSpan ExpiredPassToSigningDate = TimeSpan.FromDays(NewReportConst.ExpiredPassToSigningDate);

		//            newRportDto.PassCount2 = (curPrtQuery.PassExpertiseExpireOn != null && curPrtQuery.PrtnContractTypeId == 3) ? 1 : 0;//ekpertizadan otgan shartnomalar 200+




		//            var SigningExpireOnCount1 = (curPrtQuery.StatusId == StatusIdConst.PASS_EXPERTISE && curPrtQuery.PrtnContractTypeId == 3 && (DateTime.Now - curPrtQuery.PassExpertiseExpireOn) > ExpiredPassToSigningDate) ? 1 : 0;   //ekpertizadan otgandan song kambagillik imzolashni kechiktirgan  tadbirkor kechikitrsa ham kambagallik javobgar


		//            var SigningExpireOnCount2 = (curPrtQuery.StatusId == StatusIdConst.SIGNING && curPrtQuery.PrtnContractTypeId == 3 && curPrtQuery.Signs.Count(s => s.SignedAt != null) == 1 && (DateTime.Now - curPrtQuery.Signs.Where(s => s.SignedAt != null).OrderBy(s => s.SignedAt).FirstOrDefault().SignedAt) > ExpiredPassToSigningDate) ? 1 : 0;
		//            //imzolanayotgan tadbirkor imzolangan ammo kambagallik imzolamgan 

		//            var SigningExpireOnCount3 = (curPrtQuery.PrtnContractTypeId == 3 && curPrtQuery.Signs.Count() != 0 && curPrtQuery.Signs.Count(s => s.SignedAt != null) != 1 && (curPrtQuery.SigningExpireOn - curPrtQuery.Signs.Where(s => s.SignedAt != null).OrderBy(s => s.SignedAt).FirstOrDefault().SignedAt) > ExpiredPassToSigningDate) ? 1 : 0;
		//            //imzolanayotgan tadbirkor imzolangan va kambagallik kechikkan holda imzolagan

		//            newRportDto.SigningExpireOnCount = SigningExpireOnCount1 + SigningExpireOnCount2 + SigningExpireOnCount3;

		//            //////////////////////////////////////////
		//            TimeSpan ExpiredToSignDate = TimeSpan.FromDays(NewReportConst.ExpiredToSignDate);

		//            newRportDto.SigningCount = (curPrtQuery.SigningExpireOn != null && curPrtQuery.PrtnContractTypeId == 3) ? 1 : 0;// moliya vazilgi uchun jami arizalar

		//            var SigningCount1 = (curPrtQuery.StatusId == StatusIdConst.SIGNING && curPrtQuery.PrtnContractTypeId == 3 && curPrtQuery.Signs.Count(s => s.SignedAt != null) == 2 && (DateTime.Now - curPrtQuery.SigningExpireOn) > ExpiredToSignDate) ? 1 : 0;
		//            //moliya vaziri imzo qoyisni kechiktirgan


		//            var SigningCount2 = (curPrtQuery.SignExpireOn != null && curPrtQuery.PrtnContractTypeId == 3 && curPrtQuery.Signs.Count(s => s.SignedAt != null) == 3 && (curPrtQuery.SignExpireOn - curPrtQuery.SigningExpireOn) > ExpiredToSignDate) ? 1 : 0;
		//            //moliya vaziri kechikib imzo qoygan

		//            newRportDto.SignExpireOnCount2 = SigningCount1 + SigningCount2;


		//            TimeSpan ExpireToGenerateCertificate = TimeSpan.FromDays(NewReportConst.ExpireToGenerateCertificate);
		//            newRportDto.NotGeneratedCertificatesCount = (curPrtQuery.StatusId == StatusIdConst.SIGNED) ? 1 : 0;



		//            var GeneratedCertificatesCount1 = (curPrtQuery.StatusId == StatusIdConst.SIGNED && curPrtQuery.PrtnCertificate == null && (DateTime.Now - curPrtQuery.SignExpireOn) > ExpireToGenerateCertificate) ? 1 : 0;

		//            var GeneratedCertificatesCount2 = (curPrtQuery.StatusId == StatusIdConst.SIGNED && curPrtQuery.PrtnCertificate != null && (curPrtQuery.PrepareCertificateExpireOn - curPrtQuery.SignExpireOn) > ExpireToGenerateCertificate) ? 1 : 0;

		//            newRportDto.GeneratedCertificatesCount = GeneratedCertificatesCount1 + GeneratedCertificatesCount2;



		//            newRportDtos.Add(newRportDto);

		//        }
		//    }

		//    else if (options.RegionId != null)
		//    {

		//        var districts = _unitOfWork.DistrictRepository.ReadAsNoTracked<DistrictListDto>(applyFilter: false).Where(a => a.RegionId == options.RegionId).AsNoTracking().ToList();
		//        foreach (var item in districts)
		//        {
		//            var cureContractors = query.Where(q => q.ContractorDistrictId == item.Id);
		//            var curPrtQuery = prtnQuery.Where(a => cureContractors.Any(p => p.Id == a.ApplicationId));
		//            ExpiredContractorsReportDto newRportDto = new ExpiredContractorsReportDto();

		//            newRportDto.District = item.FullName;
		//            newRportDto.DistrictId = item.Id;
		//            newRportDto.AllApplicationsCount = cureContractors.Count();  //hamma shartnomalar
		//            newRportDto.IsBeingCosideredApplicationCount = cureContractors.Count();  //korib chiqishga yuborilgan arizalar soni

		//            TimeSpan ExpiredMahallaDate = TimeSpan.FromDays(NewReportConst.ExpiredMahallaDate);


		//            var ExpiredIsBeingCosideredApplicationCount1 = cureContractors.Where(c => c.StatusId == StatusIdConst.ACCEPTED &&
		//               c.PrtnApplication.PassExpertiseExpireOn - c.PrtnApplication.StatusChangeExpireOn.Value > ExpiredMahallaDate).Count();

		//            var ExpiredIsBeingCosideredApplicationCount12 = cureContractors.Where(c => c.StatusId == StatusIdConst.SENT_FOR_REVIEW &&
		//               DateTime.Now - c.PrtnApplication.StatusChangeExpireOn.Value > ExpiredMahallaDate).Count();

		//            newRportDto.ExpiredIsBeingCosideredApplicationCount = ExpiredIsBeingCosideredApplicationCount1 + ExpiredIsBeingCosideredApplicationCount12;  //korib chiqishi kechikayotgan yoki kechikib o'tkazilgan arizalar soni

		//            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//            TimeSpan ExpiredExpertizeDate = TimeSpan.FromDays(NewReportConst.ExpiredExpertizeDate);

		//            newRportDto.SendToExpertiseCount = curPrtQuery/*/*.Where(a => a.StatusId == StatusIdConst.SENT_FOR_EXPERTISE)*/.Count(); // ekspertizaga kelgan shartnomalar

		//            var ExpiredSendToExpertiseCount1 = curPrtQuery.Where(a => a.StatusId == StatusIdConst.SENT_FOR_EXPERTISE &&
		//              DateTime.Now - a.StatusChangeExpireOn.Value > ExpiredExpertizeDate).Count();

		//            var ExpiredSendToExpertiseCount2 = curPrtQuery.Where(a => a.StatusId != StatusIdConst.SENT_FOR_EXPERTISE &&
		//                a.StatusChangeExpireOn.Value - a.PassExpertiseExpireOn.Value > ExpiredExpertizeDate).Count();  //or notpass createat vaqtincha

		//            var ExpiredSendToExpertiseCount3 = curPrtQuery.Where(a => a.StatusId != StatusIdConst.SENT_FOR_EXPERTISE &&
		//               a.StatusChangeExpireOn.Value - a.NotPassExpertiseExpireOn.Value > ExpiredExpertizeDate).Count();  //or pass createat vaqtincha


		//            newRportDto.ExpiredSendToExpertiseCount = ExpiredSendToExpertiseCount1 + ExpiredSendToExpertiseCount2 + ExpiredSendToExpertiseCount3;  //otgan va otmgan yoki kechikkanlar

		//            ////////////////////////////////////////////////////////////////////////////////
		//            TimeSpan ExpiredRecentExpertizeDate = TimeSpan.FromDays(NewReportConst.ExpiredRecentExpertizeDate);

		//            // var NotPassCount = curPrtQuery.Where(a => a.NotPassExpertiseExpireOn != null).OrderBy(a=>a.StatusId).ToList();  //expertizadan otmagan shartnomalar

		//            newRportDto.NotPassCount = curPrtQuery.Where(a => a.NotPassExpertiseExpireOn != null).Count();  //expertizadan otmagan shartnomalar

		//            //error
		//            var ExpiredResentToExpiredCount1 = curPrtQuery.Where(a => a.StatusId == StatusIdConst.NOT_PASS_EXPERTISE &&
		//            DateTime.Now - a.NotPassExpertiseExpireOn.Value > ExpiredRecentExpertizeDate).Count();     //ekspertizadan otmagan va qayta yuborlishi kechikayotgan


		//            var ExpiredResentToExpiredCount2 = curPrtQuery.Where(a =>
		//             a.NotPassExpertiseExpireOn != null
		//            && a.StatusChangeExpireOn.Value - a.NotPassExpertiseExpireOn.Value > ExpiredRecentExpertizeDate).Count();  //ekpertizaga qayta yuborilgan ammo kechikkan
		//                                                                                                                       //ok
		//            newRportDto.ExpiredResentToExpiredCount = ExpiredResentToExpiredCount1 + ExpiredResentToExpiredCount2;

		//            /////////////////////////////////////////////////////////////////////
		//            TimeSpan ExpiredPassToSignDate = TimeSpan.FromDays(NewReportConst.ExpiredPassToSignDate);

		//            newRportDto.PassCount1 = curPrtQuery.Where(a => a.PassExpertiseExpireOn != null && (a.PrtnContractTypeId == 1 || a.PrtnContractTypeId == 2)).Count();  //ekpertizadan otgan shartnomalar 50-100 101-200


		//            var SignExpireOnCount1 = curPrtQuery.Where(a => a.StatusId == StatusIdConst.PASS_EXPERTISE && (a.PrtnContractTypeId == 1 || a.PrtnContractTypeId == 2)
		//            && DateTime.Now - a.PassExpertiseExpireOn.Value > ExpiredPassToSignDate).Count();  //ekspaerizadan otgan ammo imzolash kechikayotgan

		//            var SignExpireOnCount2 = curPrtQuery.Where(a => a.SignExpireOn != null && (a.PrtnContractTypeId == 1 || a.PrtnContractTypeId == 2)
		//             && a.SignExpireOn.Value - a.PassExpertiseExpireOn.Value > ExpiredPassToSignDate).Count();  //ekspaerizadan otgan va imzolangan ammo kechikkan 50-100 101-200

		//            newRportDto.SignExpireOnCount1 = SignExpireOnCount1 + SignExpireOnCount2;


		//            ////////////////////////////////////////////////////////////////
		//            TimeSpan ExpiredPassToSigningDate = TimeSpan.FromDays(NewReportConst.ExpiredPassToSigningDate);

		//            newRportDto.PassCount2 = curPrtQuery.Where(a => a.PassExpertiseExpireOn != null && a.PrtnContractTypeId == 3).Count();  //ekpertizadan otgan shartnomalar 200+




		//            var SigningExpireOnCount1 = curPrtQuery.Where(a => a.StatusId == StatusIdConst.PASS_EXPERTISE && a.PrtnContractTypeId == 3
		//            && DateTime.Now - a.PassExpertiseExpireOn > ExpiredPassToSigningDate).AsNoTracking().Count();   //ekpertizadan otgandan song kambagillik imzolashni kechiktirgan  tadbirkor kechikitrsa ham kambagallik javobgar


		//            var SigningExpireOnCount2 = curPrtQuery.Where(a => a.StatusId == StatusIdConst.SIGNING && a.PrtnContractTypeId == 3 && a.Signs.Count(s => s.SignedAt != null) == 1 && DateTime.Now - a.Signs.Where(s => s.SignedAt != null).OrderBy(s => s.SignedAt).FirstOrDefault().SignedAt > ExpiredPassToSigningDate).Count();   //imzolanayotgan tadbirkor imzolangan ammo kambagallik imzolamgan 

		//            var SigningExpireOnCount3 = curPrtQuery.Where(a => a.PrtnContractTypeId == 3 && a.Signs.Count(s => s.SignedAt != null) != 1 && a.SigningExpireOn - a.Signs.Where(s => s.SignedAt != null).OrderBy(s => s.SignedAt).FirstOrDefault().SignedAt > ExpiredPassToSigningDate).Count();   //imzolanayotgan tadbirkor imzolangan va kambagallik kechikkan holda imzolagan

		//            newRportDto.SigningExpireOnCount = SigningExpireOnCount1 + SigningExpireOnCount2 + SigningExpireOnCount3;

		//            ////////////////////////////////////////
		//            TimeSpan ExpiredToSignDate = TimeSpan.FromDays(NewReportConst.ExpiredToSignDate);

		//            newRportDto.SigningCount = curPrtQuery.Where(a => a.SigningExpireOn != null && a.PrtnContractTypeId == 3).Count(); // moliya vazilgi uchun jami arizalar

		//            var SigningCount1 = curPrtQuery.Where(a => a.StatusId == StatusIdConst.SIGNING && a.PrtnContractTypeId == 3 && a.Signs.Count(s => s.SignedAt != null) == 2 && DateTime.Now - a.SigningExpireOn > ExpiredToSignDate).Count();  //moliya vaziri imzo qoyisni kechiktirgan


		//            var SigningCount2 = curPrtQuery.Where(a => a.SignExpireOn != null && a.PrtnContractTypeId == 3 && a.Signs.Count(s => s.SignedAt != null) == 3 && a.SignExpireOn - a.SigningExpireOn > ExpiredToSignDate).Count(); //moliya vaziri kechikib imzo qoygan

		//            newRportDto.SignExpireOnCount2 = SigningCount1 + SigningCount2;


		//            TimeSpan ExpireToGenerateCertificate = TimeSpan.FromDays(NewReportConst.ExpireToGenerateCertificate);
		//            newRportDto.NotGeneratedCertificatesCount = curPrtQuery.Where(a => a.StatusId == StatusIdConst.SIGNED).Count();


		//            var GeneratedCertificatesCount1 = curPrtQuery.Where(a => a.StatusId == StatusIdConst.SIGNED && a.PrtnCertificate == null && DateTime.Now - a.SignExpireOn > ExpireToGenerateCertificate).Count();
		//            var GeneratedCertificatesCount2 = curPrtQuery.Where(a => a.StatusId == StatusIdConst.SIGNED && a.PrtnCertificate != null && a.PrepareCertificateExpireOn - a.SignExpireOn > ExpireToGenerateCertificate).Count();

		//            newRportDto.GeneratedCertificatesCount = GeneratedCertificatesCount1 + GeneratedCertificatesCount2;



		//            newRportDtos.Add(newRportDto);
		//        }

		//    }
		//    //byregion
		//    else if (options.RegionId == null)
		//    {
		//        var regions = _unitOfWork.RegionRepository.ReadAsNoTracked<RegionListDto>(applyFilter: false).ToList();
		//        foreach (var item in regions)
		//        {
		//            var cureContractors = query.Where(q => q.ContractorRegionId == item.Id);
		//            var curPrtQuery = prtnQuery.Where(a => cureContractors.Any(p => p.Id == a.ApplicationId));
		//            ExpiredContractorsReportDto newRportDto = new ExpiredContractorsReportDto();

		//            newRportDto.Region = item.FullName;
		//            newRportDto.RegionId = item.Id;
		//            newRportDto.AllApplicationsCount = cureContractors.Count();  //hamma shartnomalar
		//            newRportDto.IsBeingCosideredApplicationCount = newData.Where(a => a.RegionId == item.Id)
		//           .Sum(a => a.TotalPrtnApplicationSentForReviewCount +
		//            a.TotalPrtnApplicationCanceledWhithOutRejectCount +
		//            a.TotalPrtnApplicationSentAcceptedCount);  //cureContractors.Count();  //korib chiqishga yuborilgan arizalar soni

		//            TimeSpan ExpiredMahallaDate = TimeSpan.FromDays(NewReportConst.ExpiredMahallaDate);


		//            var ExpiredIsBeingCosideredApplicationCount1 = cureContractors.Where(c => c.StatusId == StatusIdConst.ACCEPTED &&
		//               c.PrtnApplication.PassExpertiseExpireOn - c.PrtnApplication.StatusChangeExpireOn.Value > ExpiredMahallaDate).Count();

		//            var ExpiredIsBeingCosideredApplicationCount12 = cureContractors.Where(c => c.StatusId == StatusIdConst.SENT_FOR_REVIEW &&
		//               DateTime.Now - c.PrtnApplication.StatusChangeExpireOn.Value > ExpiredMahallaDate).Count();

		//            newRportDto.ExpiredIsBeingCosideredApplicationCount = ExpiredIsBeingCosideredApplicationCount1 + ExpiredIsBeingCosideredApplicationCount12;  //korib chiqishi kechikayotgan yoki kechikib o'tkazilgan arizalar soni

		//            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//            TimeSpan ExpiredExpertizeDate = TimeSpan.FromDays(NewReportConst.ExpiredExpertizeDate);

		//            newRportDto.SendToExpertiseCount = curPrtQuery/*/*.Where(a => a.StatusId == StatusIdConst.SENT_FOR_EXPERTISE)*/.Count(); // ekspertizaga kelgan shartnomalar

		//            var ExpiredSendToExpertiseCount1 = curPrtQuery.Where(a => a.StatusId == StatusIdConst.SENT_FOR_EXPERTISE &&
		//              DateTime.Now - a.StatusChangeExpireOn.Value > ExpiredExpertizeDate).Count();

		//            var ExpiredSendToExpertiseCount2 = curPrtQuery.Where(a => a.StatusId != StatusIdConst.SENT_FOR_EXPERTISE &&
		//                a.StatusChangeExpireOn.Value - a.PassExpertiseExpireOn.Value > ExpiredExpertizeDate).Count();  //or notpass createat vaqtincha

		//            var ExpiredSendToExpertiseCount3 = curPrtQuery.Where(a => a.StatusId != StatusIdConst.SENT_FOR_EXPERTISE &&
		//               a.StatusChangeExpireOn.Value - a.NotPassExpertiseExpireOn.Value > ExpiredExpertizeDate).Count();  //or pass createat vaqtincha


		//            newRportDto.ExpiredSendToExpertiseCount = ExpiredSendToExpertiseCount1 + ExpiredSendToExpertiseCount2 + ExpiredSendToExpertiseCount3;  //otgan va otmgan yoki kechikkanlar

		//            ////////////////////////////////////////////////////////////////////////////////
		//            TimeSpan ExpiredRecentExpertizeDate = TimeSpan.FromDays(NewReportConst.ExpiredRecentExpertizeDate);

		//            // var NotPassCount = curPrtQuery.Where(a => a.NotPassExpertiseExpireOn != null).OrderBy(a=>a.StatusId).ToList();  //expertizadan otmagan shartnomalar

		//            newRportDto.NotPassCount = curPrtQuery.Where(a => a.NotPassExpertiseExpireOn != null).Count();  //expertizadan otmagan shartnomalar

		//            //error
		//            var ExpiredResentToExpiredCount1 = curPrtQuery.Where(a => a.StatusId == StatusIdConst.NOT_PASS_EXPERTISE &&
		//            DateTime.Now - a.NotPassExpertiseExpireOn.Value > ExpiredRecentExpertizeDate).Count();     //ekspertizadan otmagan va qayta yuborlishi kechikayotgan


		//            var ExpiredResentToExpiredCount2 = curPrtQuery.Where(a =>
		//             a.NotPassExpertiseExpireOn != null
		//            && a.StatusChangeExpireOn.Value - a.NotPassExpertiseExpireOn.Value > ExpiredRecentExpertizeDate).Count();  //ekpertizaga qayta yuborilgan ammo kechikkan
		//                                                                                                                       //ok
		//            newRportDto.ExpiredResentToExpiredCount = ExpiredResentToExpiredCount1 + ExpiredResentToExpiredCount2;

		//            /////////////////////////////////////////////////////////////////////
		//            TimeSpan ExpiredPassToSignDate = TimeSpan.FromDays(NewReportConst.ExpiredPassToSignDate);

		//            newRportDto.PassCount1 = curPrtQuery.Where(a => a.PassExpertiseExpireOn != null && (a.PrtnContractTypeId == 1 || a.PrtnContractTypeId == 2)).Count();  //ekpertizadan otgan shartnomalar 50-100 101-200


		//            var SignExpireOnCount1 = curPrtQuery.Where(a => a.StatusId == StatusIdConst.PASS_EXPERTISE && (a.PrtnContractTypeId == 1 || a.PrtnContractTypeId == 2)
		//            && DateTime.Now - a.PassExpertiseExpireOn.Value > ExpiredPassToSignDate).Count();  //ekspaerizadan otgan ammo imzolash kechikayotgan

		//            var SignExpireOnCount2 = curPrtQuery.Where(a => a.SignExpireOn != null && (a.PrtnContractTypeId == 1 || a.PrtnContractTypeId == 2)
		//             && a.SignExpireOn.Value - a.PassExpertiseExpireOn.Value > ExpiredPassToSignDate).Count();  //ekspaerizadan otgan va imzolangan ammo kechikkan 50-100 101-200

		//            newRportDto.SignExpireOnCount1 = SignExpireOnCount1 + SignExpireOnCount2;


		//            ////////////////////////////////////////////////////////////////
		//            TimeSpan ExpiredPassToSigningDate = TimeSpan.FromDays(NewReportConst.ExpiredPassToSigningDate);

		//            newRportDto.PassCount2 = curPrtQuery.Where(a => a.PassExpertiseExpireOn != null && a.PrtnContractTypeId == 3).Count();  //ekpertizadan otgan shartnomalar 200+




		//            var SigningExpireOnCount1 = curPrtQuery.Where(a => a.StatusId == StatusIdConst.PASS_EXPERTISE && a.PrtnContractTypeId == 3
		//            && DateTime.Now - a.PassExpertiseExpireOn > ExpiredPassToSigningDate).AsNoTracking().Count();   //ekpertizadan otgandan song kambagillik imzolashni kechiktirgan  tadbirkor kechikitrsa ham kambagallik javobgar


		//            var SigningExpireOnCount2 = curPrtQuery.Where(a => a.StatusId == StatusIdConst.SIGNING && a.PrtnContractTypeId == 3 && a.Signs.Count(s => s.SignedAt != null) == 1 && DateTime.Now - a.Signs.Where(s => s.SignedAt != null).OrderBy(s => s.SignedAt).FirstOrDefault().SignedAt > ExpiredPassToSigningDate).Count();   //imzolanayotgan tadbirkor imzolangan ammo kambagallik imzolamgan 

		//            var SigningExpireOnCount3 = curPrtQuery.Where(a => a.PrtnContractTypeId == 3 && a.Signs.Count(s => s.SignedAt != null) != 1 && a.SigningExpireOn - a.Signs.Where(s => s.SignedAt != null).OrderBy(s => s.SignedAt).FirstOrDefault().SignedAt > ExpiredPassToSigningDate).Count();   //imzolanayotgan tadbirkor imzolangan va kambagallik kechikkan holda imzolagan

		//            newRportDto.SigningExpireOnCount = SigningExpireOnCount1 + SigningExpireOnCount2 + SigningExpireOnCount3;

		//            ////////////////////////////////////////
		//            TimeSpan ExpiredToSignDate = TimeSpan.FromDays(NewReportConst.ExpiredToSignDate);

		//            newRportDto.SigningCount = curPrtQuery.Where(a => a.SigningExpireOn != null && a.PrtnContractTypeId == 3).Count(); // moliya vazilgi uchun jami arizalar

		//            var SigningCount1 = curPrtQuery.Where(a => a.StatusId == StatusIdConst.SIGNING && a.PrtnContractTypeId == 3 && a.Signs.Count(s => s.SignedAt != null) == 2 && DateTime.Now - a.SigningExpireOn > ExpiredToSignDate).Count();  //moliya vaziri imzo qoyisni kechiktirgan


		//            var SigningCount2 = curPrtQuery.Where(a => a.SignExpireOn != null && a.PrtnContractTypeId == 3 && a.Signs.Count(s => s.SignedAt != null) == 3 && a.SignExpireOn - a.SigningExpireOn > ExpiredToSignDate).Count(); //moliya vaziri kechikib imzo qoygan

		//            newRportDto.SignExpireOnCount2 = SigningCount1 + SigningCount2;


		//            TimeSpan ExpireToGenerateCertificate = TimeSpan.FromDays(NewReportConst.ExpireToGenerateCertificate);
		//            newRportDto.NotGeneratedCertificatesCount = curPrtQuery.Where(a => a.StatusId == StatusIdConst.SIGNED).Count();


		//            var GeneratedCertificatesCount1 = curPrtQuery.Where(a => a.StatusId == StatusIdConst.SIGNED && a.PrtnCertificate == null && DateTime.Now - a.SignExpireOn > ExpireToGenerateCertificate).Count();
		//            var GeneratedCertificatesCount2 = curPrtQuery.Where(a => a.StatusId == StatusIdConst.SIGNED && a.PrtnCertificate != null && a.PrepareCertificateExpireOn - a.SignExpireOn > ExpireToGenerateCertificate).Count();

		//            newRportDto.GeneratedCertificatesCount = GeneratedCertificatesCount1 + GeneratedCertificatesCount2;



		//            newRportDtos.Add(newRportDto);
		//        }
		//    }
		//    //#region bydistrict





		//    return newRportDtos;
		//}
		public PagedResult<PrtnApplicationAndContractInfoDto> GetExpiredContractorsReportPageResult(PrtnDocumentSortFilterOptions filter)
		{

			var result = GetExpiredContractorsReportMethod(filter);

			return result.AsPagedResult(filter);
		}
		public IQueryable<PrtnApplicationAndContractInfoDto> GetExpiredContractorsReportMethod(PrtnDocumentSortFilterOptions filter)
		{

			int extraExpireDay = 0;
			if (filter.ExpireDay != null)
			{
				if (filter.ExpireDay == 8)
					extraExpireDay = 8;
				else if (filter.ExpireDay == 9)
					extraExpireDay = 30;
				else
					extraExpireDay = filter.ExpireDay.Value;

			}


			filter.ByRegion = true;
			filter.ByContractor = true;

			int ExpiredMahallaDate = (NewReportConst.ExpiredMahallaDate + extraExpireDay);
			int ExpiredExpertizeDate = (NewReportConst.ExpiredExpertizeDate + extraExpireDay);
			int ExpiredRecentExpertizeDate = (NewReportConst.ExpiredRecentExpertizeDate + extraExpireDay);
			int ExpiredPassToSignDate = (NewReportConst.ExpiredPassToSignDate + extraExpireDay);
			int ExpiredPassToSigningDate = (NewReportConst.ExpiredPassToSigningDate + extraExpireDay);
			int ExpiredToSignDate = (NewReportConst.ExpiredToSignDate + extraExpireDay);
			int ExpireToGenerateCertificate = (NewReportConst.ExpireToGenerateCertificate + extraExpireDay);


			var result = new List<PrtnApplicationAndContractInfoDto>();

			IQueryable<Application> query = _unitOfWork.Context.Set<Application>()
				.Include(a => a.PrtnApplication)
				.Include(a => a.PrtnContract).ThenInclude(a => a.PrtnCertificate)
				.Include(a => a.Contractor).ThenInclude(c => c.Oked).ThenInclude(a => a.OkedType)
				.Where(a => new int[]
					{
						StatusIdConst.SENT_FOR_REVIEW, StatusIdConst.SENT,
						StatusIdConst.ACCEPTED, StatusIdConst.EXECUTING,
						StatusIdConst.REJECTED, StatusIdConst.PASS_EXPERTISE,
						StatusIdConst.NOT_PASS_EXPERTISE, StatusIdConst.SIGNED,
						StatusIdConst.SIGNING, StatusIdConst.SENT_FOR_EXPERTISE,
						StatusIdConst.CANCELED, StatusIdConst.REVOKED
					}.Contains(a.StatusId)
					&& a.ApplicationTypeId == ApplicationTypeIdConst.PARTNER
				)
				.Where(a => !filter.PrtnContractTypeId.HasValue || filter.PrtnContractTypeId == a.PrtnApplication.PrtnContractTypeId);

			query = query.Where(a => (filter.StartDate.HasValue ? a.DocOn >= filter.StartDate.Value : true)
				  && (filter.EndDate.HasValue ? a.DocOn <= filter.EndDate.Value : true));

			query = query.Where(a => (!filter.RegionId.HasValue || filter.RegionId == (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId : a.RegionId))
				&& (!filter.DistrictId.HasValue || filter.DistrictId == (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId))
				&& (!filter.ContractorId.HasValue || filter.ContractorId == a.ContractorId) && (!filter.OkedTypeId.HasValue || filter.OkedTypeId == a.Contractor.Oked.OkedTypeId));


			//var query2 = query.Where(a =>  DateTime.Now - a.PrtnApplication.StatusChangeExpireOn.Value > ExpiredMahallaDate && a.PrtnApplication.Id == 3218 && a.PrtnApplication.PassExpertiseExpireOn== null).ToList();

			result = query
			.Select(a => new PrtnApplicationAndContractInfoDto
			{
				PrtnContractTypeId = filter.PrtnContractTypeId.HasValue ? a.PrtnApplication.PrtnContractTypeId : 0,
				PrtnContractType = filter.PrtnContractTypeId.HasValue ? (a.PrtnApplication.PrtnContractType.Translates.AsQueryable()
						.FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.PrtnContractType.FullName) : "",

				RegionId = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId : a.RegionId) : null,
				RegionOrderCode = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegion.OrderCode : a.Region.OrderCode) : null,
				Region = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? (a.PrtnApplication.ChoosedRegion.Translates.AsQueryable()
								.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.ChoosedRegion.FullName)
							: (a.Region.Translates.AsQueryable()
								.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Region.FullName))
						: null,

				DistrictId = filter.ByDistrict ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId) : null,
				District = filter.ByDistrict
						? (a.PrtnApplication.ChooseLocation
							? (a.PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
								.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.ChoosedDistrict.FullName)
							: (a.District.Translates.AsQueryable()
								.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.District.FullName))
						: null,

				ContractorId = filter.ByContractor ? a.ContractorId : null,
				Contractor = filter.ByContractor ? a.Contractor.FullName : null,
				ContractorInn = filter.ByContractor ? a.Contractor.Inn : null,
				ContractorPhoneNumber = filter.ByContractor ? a.Contractor.PhoneNumber : null,

				NewVacanciesCount = a.PrtnApplication.NewVacanciesCount,

				//xatoroq
				ExpiredIsBeingCosideredApplicationCount = (CountBusinessDays(a.PrtnApplication.PassExpertiseExpireOn, a.PrtnApplication.StatusChangeExpireOn) > ExpiredMahallaDate || (a.PrtnApplication.PassExpertiseExpireOn == null && CountBusinessDays(DateTime.Now, a.PrtnApplication.StatusChangeExpireOn) > ExpiredMahallaDate)) ? 1 : 0,

				ExpiredSendToExpertiseCount = (a.PrtnContract.StatusId == StatusIdConst.SENT_FOR_EXPERTISE && (CountBusinessDays(DateTime.Now, a.PrtnContract.StatusChangeExpireOn)) > ExpiredExpertizeDate) || (a.StatusId != StatusIdConst.SENT_FOR_EXPERTISE && ((CountBusinessDays(a.PrtnContract.StatusChangeExpireOn, a.PrtnContract.PassExpertiseExpireOn) > ExpiredExpertizeDate) || (CountBusinessDays(a.PrtnContract.StatusChangeExpireOn, a.PrtnContract.NotPassExpertiseExpireOn) > ExpiredExpertizeDate))) ? 1 : 0,

				//resent
				NotPassCount = a.PrtnContract.NotPassExpertiseExpireOn != null ? 1 : 0,
				ExpiredResentToExpiredCount = (a.PrtnContract.StatusId == StatusIdConst.NOT_PASS_EXPERTISE && CountBusinessDays(DateTime.Now, a.PrtnContract.NotPassExpertiseExpireOn) > ExpiredRecentExpertizeDate) || (a.PrtnContract.NotPassExpertiseExpireOn != null && a.PrtnContract.StatusChangeExpireOn > a.PrtnContract.NotPassExpertiseExpireOn && CountBusinessDays(a.PrtnContract.StatusChangeExpireOn, a.PrtnContract.NotPassExpertiseExpireOn) > ExpiredRecentExpertizeDate) ? 1 : 0,



				TotalPrtnApplicationSentCount = a.StatusId == StatusIdConst.SENT ? 1 : 0,
				TotalPrtnApplicationPassExpertisesCount = a.PrtnContract.StatusId == StatusIdConst.PASS_EXPERTISE ? 1 : 0,

				TotalPrtnApplicationSentForExpertisesCount = a.PrtnContract.StatusId == StatusIdConst.SENT_FOR_EXPERTISE && a.PrtnContract.PrtnContractTypeId != PrtnContractTypeIdConst._201__ ? 1 : 0,
				TotalPrtnApplicationNotPassExpertisesCount = a.PrtnContract.StatusId == StatusIdConst.NOT_PASS_EXPERTISE && a.PrtnContract.PrtnContractTypeId != PrtnContractTypeIdConst._201__ ? 1 : 0,


				TotalPrtnApplicationSentForExpertisesCount2 = a.PrtnContract.StatusId == StatusIdConst.SENT_FOR_EXPERTISE && a.PrtnContract.PrtnContractTypeId == PrtnContractTypeIdConst._201__ ? 1 : 0,
				TotalPrtnApplicationNotPassExpertisesCount2 = a.PrtnContract.StatusId == StatusIdConst.NOT_PASS_EXPERTISE && a.PrtnContract.PrtnContractTypeId == PrtnContractTypeIdConst._201__ ? 1 : 0,

				TotalPrtnApplicationSignedCount = a.PrtnContract.StatusId == StatusIdConst.SIGNED ? 1 : 0,
				TotalPrtnApplicationSignningCount = a.PrtnContract.StatusId == StatusIdConst.SIGNING ? 1 : 0,
				TotalPrtnApplicationSentForReviewCount = a.StatusId == StatusIdConst.SENT_FOR_REVIEW ? 1 : 0,
				TotalPrtnApplicationSentAcceptedCount = a.StatusId == StatusIdConst.ACCEPTED ? 1 : 0,
				TotalPrtnApplicationSentRejectedCount = a.StatusId == StatusIdConst.REJECTED ? 1 : 0,
				TotalPrtnApplicationSentRevokedCount = a.StatusId == StatusIdConst.REVOKED ? 1 : 0,
				TotalPrtnApplicationCanceledCount = a.StatusId == StatusIdConst.CANCELED ? 1 : 0,

				TotalPrtnApplicationCanceledWhithOutContractCount = a.PrtnContract == null && a.StatusId == StatusIdConst.CANCELED ? 1 : 0,
				TotalPrtnApplicationCanceledWhithOutRejectCount = a.PrtnContract == null && a.StatusId == StatusIdConst.REJECTED ? 1 : 0,  //

				TotalPrtnContractCanceledWhithOutCertificateCount = a.PrtnContract.PrtnCertificate == null && a.PrtnContract.StatusId == StatusIdConst.CANCELED ? 1 : 0,
				TotalPrtnContractRejectWhithOutCertificateCount = a.PrtnContract.PrtnCertificate == null && a.PrtnContract.StatusId == StatusIdConst.REJECTED ? 1 : 0,

				TotalPrtnCertificateCanceledApplicationCount = (a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.CANCELED) ? 1 : 0,
				TotalPrtnCertificateRejectApplicationCount = (a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.REJECTED) ? 1 : 0,

				TotalPrtnContractCount = a.PrtnContract != null && a.PrtnContract.StatusId != StatusIdConst.REJECTED ? 1 : 0,

				PassCount1 = a.PrtnContract != null && a.PrtnContract.StatusId != StatusIdConst.REJECTED && a.PrtnContract.PrtnContractTypeId != PrtnContractTypeIdConst._201__ ? 1 : 0,
				SignExpireOnCount1 = (a.PrtnContract.StatusId == StatusIdConst.PASS_EXPERTISE && (a.PrtnContract.PrtnContractTypeId == 1 || a.PrtnContract.PrtnContractTypeId == 2) && CountBusinessDays(DateTime.Now, a.PrtnContract.PassExpertiseExpireOn) > ExpiredPassToSignDate) || (a.PrtnContract.SignExpireOn != null && (a.PrtnContract.PrtnContractTypeId == 1 || a.PrtnContract.PrtnContractTypeId == 2) && CountBusinessDays(a.PrtnContract.SignExpireOn, a.PrtnContract.PassExpertiseExpireOn) > ExpiredPassToSignDate) ? 1 : 0,



				TotalPrtnContractCancelCount = a.PrtnContract != null && a.PrtnContract.StatusId == StatusIdConst.CANCELED ? 1 : 0,
				TotalPrtnContractRejectedCount = a.PrtnContract != null && a.PrtnContract.StatusId == StatusIdConst.REJECTED ? 1 : 0,
				TotalPrtnCertificateCount = a.PrtnContract.PrtnCertificate != null && a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED ? 1 : 0,
				TotalPrtnCertificateCanceledCount = a.PrtnContract.PrtnCertificate != null && a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.CANCELED ? 1 : 0,
				TotalNewVacanciesCount = a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED ? a.PrtnApplication.NewVacanciesCount : 0,

				TotalPrtnApplicationIsOffersCount = a.Contractor.IsLastOffer ? 1 : 0,

				PassCount2 = a.PrtnContract != null && a.PrtnContract.StatusId != StatusIdConst.REJECTED && a.PrtnContract.PrtnContractTypeId == PrtnContractTypeIdConst._201__ ? 1 : 0,
				SigningCount = (a.PrtnContract.PrtnContractTypeId == 3 && (a.PrtnContract.Signs.Count(a => a.SignedAt != null) == 2 || a.PrtnContract.Signs.Count(a => a.SignedAt != null) == 3)) ? 1 : 0,
				SigningExpireOnCount = (a.PrtnContract.StatusId == StatusIdConst.PASS_EXPERTISE && a.PrtnContract.PrtnContractTypeId == 3 && CountBusinessDays(DateTime.Now, a.PrtnContract.PassExpertiseExpireOn) > ExpiredPassToSigningDate) || (a.PrtnContract.StatusId == StatusIdConst.SIGNING && a.PrtnContract.PrtnContractTypeId == 3 && a.PrtnContract.Signs.Count(s => s.SignedAt != null) == 1 && CountBusinessDays(DateTime.Now, a.PrtnContract.Signs.Where(s => s.SignedAt != null).OrderBy(s => s.SignedAt).FirstOrDefault().SignedAt) > ExpiredPassToSigningDate) || (a.PrtnContract.PrtnContractTypeId == 3 && a.PrtnContract.Signs.Count(s => s.SignedAt != null) != 1 && CountBusinessDays(a.PrtnContract.SigningExpireOn, a.PrtnContract.Signs.Where(s => s.SignedAt != null).OrderBy(s => s.SignedAt).FirstOrDefault().SignedAt) > ExpiredPassToSigningDate) ? 1 : 0,
				SignExpireOnCount2 = (a.PrtnContract.StatusId == StatusIdConst.SIGNING && a.PrtnContract.PrtnContractTypeId == 3 && a.PrtnContract.Signs.Count(s => s.SignedAt != null) == 2 && CountBusinessDays(DateTime.Now, a.PrtnContract.SigningExpireOn) > ExpiredToSignDate) || (a.PrtnContract.SignExpireOn != null && a.PrtnContract.PrtnContractTypeId == 3 && a.PrtnContract.Signs.Count(s => s.SignedAt != null) == 3 && CountBusinessDays(a.PrtnContract.SignExpireOn, a.PrtnContract.SigningExpireOn) > ExpiredToSignDate) ? 1 : 0,
				NotGeneratedCertificatesCount = a.PrtnContract.StatusId == StatusIdConst.SIGNED ? 1 : 0,
				GeneratedCertificatesCount = (a.PrtnContract.StatusId == StatusIdConst.SIGNED && a.PrtnContract.PrtnCertificate == null && CountBusinessDays(DateTime.Now, a.PrtnContract.SignExpireOn) > ExpireToGenerateCertificate) || (a.PrtnContract.StatusId == StatusIdConst.SIGNED && a.PrtnContract.PrtnCertificate != null && CountBusinessDays(a.PrtnContract.PrepareCertificateExpireOn, a.PrtnContract.SignExpireOn) > ExpireToGenerateCertificate) ? 1 : 0


			})
				.AsEnumerable()
				.GroupBy(a => new
				{
					//a.PrtnContractTypeId,
					//a.PrtnContractType,
					a.DistrictId,
					a.District,
					a.RegionId,
					a.RegionOrderCode,
					a.Region,
					a.ContractorId,
					a.Contractor,
					a.ContractorInn,
					a.ContractorPhoneNumber,
				})
				.Select(a => new PrtnApplicationAndContractInfoDto
				{
					//PrtnContractTypeId = a.Key.PrtnContractTypeId,
					//PrtnContractType = a.Key.PrtnContractType,
					DistrictId = a.Key.DistrictId,
					District = a.Key.District,
					RegionId = a.Key.RegionId,
					RegionOrderCode = a.Key.RegionOrderCode,
					Region = a.Key.Region,
					ContractorId = a.Key.ContractorId,
					Contractor = a.Key.Contractor,
					ContractorInn = a.Key.ContractorInn,
					ContractorPhoneNumber = a.Key.ContractorPhoneNumber,
					TotalPrtnApplicationSentCount = a.Sum(b => b.TotalPrtnApplicationSentCount),

					TotalApplication = ((long)a.Count(), a.Sum(c => c.NewVacanciesCount)),

					TotalPrtnApplicationCanceledWhithOutContractCount = a.Sum(b => b.TotalPrtnApplicationCanceledWhithOutContractCount),
					TotalPrtnApplicationCanceledWhithOutRejectCount = a.Sum(b => b.TotalPrtnApplicationCanceledWhithOutRejectCount),


					TotalPrtnContractCanceledWhithOutCertificateCount = a.Sum(b => b.TotalPrtnContractCanceledWhithOutCertificateCount),
					TotalPrtnContractRejectWhithOutCertificateCount = a.Sum(b => b.TotalPrtnContractRejectWhithOutCertificateCount),

					TotalPrtnCertificateCanceledApplicationCount = a.Sum(b => b.TotalPrtnCertificateCanceledApplicationCount),
					TotalPrtnCertificateRejectApplicationCount = a.Sum(b => b.TotalPrtnCertificateRejectApplicationCount),
					TotalPrtnApplicationSentForReviewCount = a.Sum(b => b.TotalPrtnApplicationSentForReviewCount),
					TotalPrtnApplicationSentAcceptedCount = a.Sum(b => b.TotalPrtnApplicationSentAcceptedCount),
					TotalPrtnApplicationPassExpertisesCount = a.Sum(b => b.TotalPrtnApplicationPassExpertisesCount),
					TotalPrtnApplicationSentForExpertisesCount = a.Sum(b => b.TotalPrtnApplicationSentForExpertisesCount),
					TotalPrtnApplicationNotPassExpertisesCount = a.Sum(b => b.TotalPrtnApplicationNotPassExpertisesCount),
					TotalPrtnApplicationSignedCount = a.Sum(b => b.TotalPrtnApplicationSignedCount),
					TotalPrtnApplicationSignningCount = a.Sum(b => b.TotalPrtnApplicationSignningCount),
					TotalPrtnApplicationCanceledCount = a.Sum(b => b.TotalPrtnApplicationCanceledCount),
					TotalPrtnApplicationSentRejectedCount = a.Sum(b => b.TotalPrtnApplicationSentRejectedCount),
					TotalPrtnApplicationSentRevokedCount = a.Sum(b => b.TotalPrtnApplicationSentRevokedCount),
					TotalPrtnContractCancelCount = a.Sum(b => b.TotalPrtnContractCancelCount),
					TotalPrtnContractRejectedCount = a.Sum(b => b.TotalPrtnContractRejectedCount),
					TotalPrtnCertificateCount = a.Sum(b => b.TotalPrtnCertificateCount),
					TotalPrtnCertificateCanceledCount = a.Sum(b => b.TotalPrtnCertificateCanceledCount),
					TotalNewVacanciesCount = a.Sum(b => b.TotalNewVacanciesCount),
					TotalPrtnApplicationCount = a.Sum(b => b.TotalPrtnApplicationSentCount) + a.Sum(b => b.TotalPrtnApplicationSentForReviewCount) + a.Sum(b => b.TotalPrtnApplicationSentAcceptedCount),
					TotalPrtnContractCount = a.Sum(b => b.TotalPrtnContractCount),
					TotalPrtnApplicationIsOffersCount = a.Sum(b => b.TotalPrtnApplicationIsOffersCount),

					ExpiredIsBeingCosideredApplicationCount = a.Sum(b => b.ExpiredIsBeingCosideredApplicationCount),
					IsBeingCosideredApplicationCount = a.Sum(b => b.TotalPrtnApplicationCanceledWhithOutRejectCount) + a.Sum(b => b.TotalPrtnApplicationSentForReviewCount) + a.Sum(b => b.TotalPrtnApplicationSentAcceptedCount),
					SendToExpertiseCount = a.Sum(b => b.TotalPrtnContractCount),
					ExpiredSendToExpertiseCount = a.Sum(b => b.ExpiredSendToExpertiseCount),
					NotPassCount = a.Sum(a => a.NotPassCount),
					ExpiredResentToExpiredCount = a.Sum(a => a.ExpiredResentToExpiredCount),
					PassCount1 = a.Sum(a => a.PassCount1) - a.Sum(b => b.TotalPrtnApplicationSentForExpertisesCount) - a.Sum(b => b.TotalPrtnApplicationNotPassExpertisesCount),
					SignExpireOnCount1 = a.Sum(b => b.SignExpireOnCount1),
					PassCount2 = a.Sum(a => a.PassCount2) - a.Sum(b => b.TotalPrtnApplicationSentForExpertisesCount2) - a.Sum(b => b.TotalPrtnApplicationNotPassExpertisesCount2),
					SigningExpireOnCount = a.Sum(a => a.SigningExpireOnCount),
					SigningCount = a.Sum(a => a.SigningCount),
					SignExpireOnCount2 = a.Sum(a => a.SignExpireOnCount2),
					NotGeneratedCertificatesCount = a.Sum(a => a.NotGeneratedCertificatesCount),
					GeneratedCertificatesCount = a.Sum(a => a.GeneratedCertificatesCount)

				}).ToList();

			// Hamma viloyatlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
			if (filter.ByRegion)
			{
				var regions = _unitOfWork.RegionRepository.AllAsQueryable
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.Id)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							OrderCode = a.OrderCode,
							FullName = a.FullName
						}
					);

				foreach (var region in regions)
				{
					if (result.Select(a => a.RegionId).Contains(region.Key))
						continue;

					result.Add(new PrtnApplicationAndContractInfoDto
					{
						Region = region.Value.FullName,
						RegionOrderCode = region.Value.OrderCode,
						RegionId = region.Key
					});
				}

				result = result.OrderBy(a => a.RegionOrderCode).ToList();
			}

			// Viloyatdagi hamma tumanlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
			if (filter.ByDistrict)
			{
				var districts = _unitOfWork.DistrictRepository.AllAsQueryable
					.Include(a => a.Region).ThenInclude(a => a.Translates)
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.RegionId
						&& !filter.DistrictId.HasValue || filter.DistrictId == a.Id
					)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							RegionId = a.RegionId,
							Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
								?.TranslateText
							?? a.Region.FullName,
							District = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
								?.TranslateText
							?? a.FullName,
							DistrictOrderCode = a.OrderCode,
						}
					);

				foreach (var district in districts)
				{
					if (result.Select(a => a.DistrictId).Contains(district.Key))
						continue;

					result.Add(new PrtnApplicationAndContractInfoDto
					{
						Region = district.Value.Region,
						RegionId = district.Value.RegionId,
						District = district.Value.District,
						RegionOrderCode = district.Value.DistrictOrderCode,
						DistrictId = district.Key
					});
				}

				result = result.OrderBy(a => a.RegionOrderCode).ToList();
			}

			return result.AsQueryable();
		}
		#endregion

		public PagedResult<PrtnApplicationByPetitionInfoDto> GetPrtnApplicationByPetitionInfoWithPagination(PrtnDocumentSortFilterOptions filter)
		=> GetPrtnApplicationByPetitionInfo(filter).AsPagedResult(filter);
		public IQueryable<PrtnApplicationByPetitionInfoDto> GetPrtnApplicationByPetitionInfo(PrtnDocumentSortFilterOptions filter)
		{
			var query = _unitOfWork.Context.Set<Application>()
								   .Include(a => a.PrtnApplication)
								   .Include(a => a.Region).ThenInclude(b => b.Translates)
								   .Where(a =>
											(!filter.PrtnContractTypeId.HasValue ||
												(a.PrtnApplication != null && filter.PrtnContractTypeId == a.PrtnApplication.PrtnContractTypeId)) &&
											(!filter.StartDate.HasValue || a.DocOn >= filter.StartDate.Value) &&
											(!filter.EndDate.HasValue || a.DocOn <= filter.EndDate.Value) &&
											(!filter.RegionId.HasValue ||
												(a.PrtnApplication.ChooseLocation
													? filter.RegionId == a.PrtnApplication.ChoosedRegionId
													: filter.RegionId == a.RegionId)) &&
											(!filter.DistrictId.HasValue ||
												(a.PrtnApplication.ChooseLocation
													? filter.DistrictId == a.PrtnApplication.ChoosedDistrictId
													: filter.DistrictId == a.DistrictId)) &&
											(string.IsNullOrEmpty(filter.Inn) || a.Contractor.Inn == filter.Inn));


			int ExpiredMahallaDate = NewReportConst.ExpiredMahallaDate;

			var result = query.Select(a => new PrtnApplicationByPetitionInfoDto
			{
				RegionId = a.PrtnApplication.ChooseLocation ? (a.PrtnApplication.ChoosedRegionId ?? a.RegionId) : a.RegionId,
				Region = a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegion.Translates.AsQueryable()
								.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
								?? a.PrtnApplication.ChoosedRegion.FullName
								: a.Region.Translates.AsQueryable()
								.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
								?? a.Region.FullName,

				DistrictId = a.PrtnApplication.ChooseLocation
								? a.PrtnApplication.ChoosedDistrictId
								: a.DistrictId,

				District = a.PrtnApplication.ChooseLocation
							? (a.PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
								.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
								?? a.PrtnApplication.ChoosedDistrict.FullName)
							: (a.District.Translates.AsQueryable()
								.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
								?? a.District.FullName),

				MFY = a.PrtnApplication != null ? a.PrtnApplication.MfyName : null,
				MfyId = a.PrtnApplication != null ? a.PrtnApplication.MfyId : (int?)null,
				Organization = a.PrtnContract != null && a.PrtnContract.Organization != null ? a.PrtnContract.Organization.FullName : null,
				OrganizationId = a.PrtnContract != null ? a.PrtnContract.OrganizationId : (int?)null,
				INN = a.Contractor != null ? a.Contractor.Inn : null,

				DateSendedOfApplication = a.PrtnApplication != null ? a.PrtnApplication.StatusChangeExpireOn : null,
				DateApplicationSigned = a.PrtnApplication != null ? a.PrtnApplication.PassExpertiseExpireOn : null,

				DaysLateToApplicationSign = CountBusinessDays(a.PrtnApplication.PassExpertiseExpireOn, a.PrtnApplication.StatusChangeExpireOn) > ExpiredMahallaDate
											   ? Math.Abs(CountBusinessDays(a.PrtnApplication.PassExpertiseExpireOn, a.PrtnApplication.StatusChangeExpireOn) - ExpiredMahallaDate)
											   : 0,
				Status = a.PrtnContract != null ? a.PrtnContract.Status.FullName : null,
				StatusId = a.PrtnContract != null ? a.PrtnContract.StatusId : null,

			});

			return result;
		}
		public PagedResult<PrtnApplicationByContractInfoDto> GetPrtnApplicationByContractNewInfoWithPagination(PrtnDocumentSortFilterOptions filter)
		=> GetPrtnApplicationByContractNewInfo(filter).AsPagedResult(filter);
		public IQueryable<PrtnApplicationByContractInfoDto> GetPrtnApplicationByContractNewInfo(PrtnDocumentSortFilterOptions filter)
		{

			var query = _unitOfWork.Context.Set<Application>()
										   .Include(a => a.PrtnApplication)
										   .Include(a => a.Region).ThenInclude(b => b.Translates)
										   .Where(a =>
											   (!filter.PrtnContractTypeId.HasValue ||
												   (a.PrtnApplication != null && filter.PrtnContractTypeId == a.PrtnApplication.PrtnContractTypeId)) &&
											   (!filter.StartDate.HasValue || a.DocOn >= filter.StartDate.Value) &&
											   (!filter.EndDate.HasValue || a.DocOn <= filter.EndDate.Value) &&
											   (!filter.RegionId.HasValue ||
												   (a.PrtnApplication.ChooseLocation
													   ? filter.RegionId == a.PrtnApplication.ChoosedRegionId
													   : filter.RegionId == a.RegionId)) &&
											   (!filter.DistrictId.HasValue ||
												   (a.PrtnApplication.ChooseLocation
													   ? filter.DistrictId == a.PrtnApplication.ChoosedDistrictId
													   : filter.DistrictId == a.DistrictId)) &&
											   (string.IsNullOrEmpty(filter.Inn) || a.Contractor.Inn == filter.Inn)
										   );


			int ExpiredPassToSigningDate = NewReportConst.ExpiredPassToSigningDate;

			var result = query.Select(a => new PrtnApplicationByContractInfoDto
			{
				RegionId = a.PrtnApplication.ChooseLocation ? (a.PrtnApplication.ChoosedRegionId ?? a.RegionId) : a.RegionId,
				Region = a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegion.Translates.AsQueryable()
								.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
								?? a.PrtnApplication.ChoosedRegion.FullName
								: a.Region.Translates.AsQueryable()
								.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
								?? a.Region.FullName,

				DistrictId = a.PrtnApplication.ChooseLocation
								? a.PrtnApplication.ChoosedDistrictId
								: a.DistrictId,

				District = a.PrtnApplication.ChooseLocation
							? (a.PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
								.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
								?? a.PrtnApplication.ChoosedDistrict.FullName)
							: (a.District.Translates.AsQueryable()
								.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
								?? a.District.FullName),


				MFY = a.PrtnApplication != null ? a.PrtnApplication.MfyName : null,
				MFYId = a.PrtnApplication != null ? a.PrtnApplication.MfyId : (int?)null,
				Organization = a.PrtnContract != null && a.PrtnContract.Organization != null ? a.PrtnContract.Organization.FullName : null,
				OrganizationId = a.PrtnContract != null ? a.PrtnContract.OrganizationId : (int?)null,
				INN = a.Contractor != null ? a.Contractor.Inn : null,
				DateSendedOfApplication = a.PrtnContract != null ? a.PrtnContract.StatusChangeExpireOn : (DateTime?)null,
				DateBusinessmanApplicationSigned = a.PrtnContract != null ? a.PrtnContract.Signs
												  .Where(s => s.SignedAt != null && s.PrtnContractTypeTable.OrderNumber == 1)
												  .Select(s => s.SignedAt)
												  .FirstOrDefault() : (DateTime?)null,

				DaysBusinessmanApplicationSigned = CountBusinessDays(a.PrtnContract.Signs
												   .Where(s => s.SignedAt != null && s.PrtnContractTypeTable.OrderNumber == 1)
												   .Select(s => s.SignedAt)
												   .FirstOrDefault(), a.PrtnContract.StatusChangeExpireOn),

				BusinessmanStatus = (a.PrtnContract.StatusId == StatusIdConst.SIGNING &&
									   a.PrtnContract.Signs.Any(s => s.PrtnContractTypeTable.OrderNumber == 1 &&
																	 (s.SignedAt != null || s.SignedAt == null)))
										  ? (a.PrtnContract.Signs.Any(s => s.SignedAt != null) ? "Имзоланган" : "Имзоланмоқда")
									  : (a.PrtnContract.StatusId == StatusIdConst.SIGNED) ? "Имзоланган"
									  : (a.PrtnContract.StatusId == StatusIdConst.REJECTED &&
										 a.PrtnContract.Signs.Any(s => s.SignedAt == null && s.PrtnContractTypeTable.OrderNumber == 1))
										  ? "Тадбиркор рад этган"
									  : null,


				EmploymentStatus = (a.PrtnContract.StatusId == StatusIdConst.SIGNING &&
									   a.PrtnContract.Signs.Any(s => s.PrtnContractTypeTable.OrderNumber == 2 &&
																	 (s.SignedAt != null || s.SignedAt == null)))
										  ? (a.PrtnContract.Signs.Any(s => s.SignedAt != null) ? "Имзоланган" : "Имзоланмоқда")
									  : (a.PrtnContract.StatusId == StatusIdConst.SIGNED) ? "Имзоланган"
									  : (a.PrtnContract.StatusId == StatusIdConst.REJECTED &&
										 a.PrtnContract.Signs.Any(s => s.SignedAt == null && s.PrtnContractTypeTable.OrderNumber == 1))
										  ? "Тадбиркор рад этган" : "Қайтарилган",


				EconomyStatus = (a.PrtnContract.StatusId == StatusIdConst.SIGNING &&
								   a.PrtnContract.Signs.Any(s => s.PrtnContractTypeTable.OrderNumber == 2))
									  ? (a.PrtnContract.Signs.Any(s => s.SignedAt != null && s.PrtnContractTypeTable.OrderNumber == 2) ? "Имзоланган" : "0")
								  : (a.PrtnContract.StatusId == StatusIdConst.SIGNED)
									  ? "Имзоланган"
								  : (a.PrtnContract.StatusId == StatusIdConst.REJECTED &&
									 a.PrtnContract.Signs.Any(s => s.SignedAt == null && s.PrtnContractTypeTable.OrderNumber == 1))
									  ? "Тадбиркор рад этган"
								  : (a.PrtnContract.StatusId == StatusIdConst.REJECTED &&
									 a.PrtnContract.Signs.Any(s => s.SignedAt == null && s.PrtnContractTypeTable.OrderNumber == 2))
									  ? "0"
								  : null,

				StatusId = a.PrtnContract != null ? a.PrtnContract.StatusId : (int?)null,

				DateEmploymentApplicationSigned = a.PrtnContract != null ? a.PrtnContract.Signs
												   .Where(s => s.SignedAt != null && s.PrtnContractTypeTable.OrderNumber == 2)
												   .Select(s => s.SignedAt)
												   .FirstOrDefault() : (DateTime?)null,

				DaysEmploymentApplicationSigned = CountBusinessDays(
												  a.PrtnContract.Signs
													  .Where(s => s.SignedAt != null && s.PrtnContractTypeTable.OrderNumber == 2)
													  .Select(s => s.SignedAt)
													  .FirstOrDefault(),
												  a.PrtnContract.Signs
													  .Where(s => s.SignedAt != null && s.PrtnContractTypeTable.OrderNumber == 1)
													  .Select(s => s.SignedAt)
													  .FirstOrDefault()
												   ),

				DateEconomyApplicationSigned = a.PrtnContract != null ? a.PrtnContract.Signs
											   .Where(s => s.SignedAt != null && s.PrtnContractTypeTable.OrderNumber == 3)
											   .Select(s => s.SignedAt)
											   .FirstOrDefault() : (DateTime?)null,

				DaysEconomyApplicationSigned = CountBusinessDays(
											   a.PrtnContract.Signs
												   .Where(s => s.SignedAt != null && s.PrtnContractTypeTable.OrderNumber == 3)
												   .Select(s => s.SignedAt)
												   .FirstOrDefault(),
											   a.PrtnContract.Signs
												   .Where(s => s.SignedAt != null && s.PrtnContractTypeTable.OrderNumber == 2)
												   .Select(s => s.SignedAt)
												   .FirstOrDefault()
											   ),

				DateOfCertificate = a.PrtnContract != null && a.PrtnContract.PrtnCertificate != null ? a.PrtnContract.PrtnCertificate.DocOn : (DateOnly?)null,
				PrtnContractTypeName = a.PrtnContract != null && a.PrtnContract.PrtnContractType != null ? a.PrtnContract.PrtnContractType.FullName : null,
				PrtnContractTypeId = a.PrtnContract != null ? a.PrtnContract.PrtnContractTypeId : (int?)null,
			});

			return result;
		}
		public PagedResult<PrtnApplicationByFullInfoDto> GetPrtnApplicationByFullInfoWithPagination(PrtnDocumentSortFilterOptions filter)
		=> GetPrtnApplicationByFullInfo(filter).AsPagedResult(filter);
		public IQueryable<PrtnApplicationByFullInfoDto> GetPrtnApplicationByFullInfo(PrtnDocumentSortFilterOptions filter)
		{
			var contractor = _unitOfWork.Context.Set<Application>()
												 .Include(a => a.Contractor)
												 .Include(a => a.PrtnApplication)
												 .Include(a => a.PrtnContract)
												 .ThenInclude(a => a.PrtnCertificate)
												 .Where(a =>
															(!filter.PrtnContractTypeId.HasValue ||
																(a.PrtnApplication != null && filter.PrtnContractTypeId == a.PrtnApplication.PrtnContractTypeId)) &&
															(!filter.StartDate.HasValue || a.DocOn >= filter.StartDate.Value) &&
															(!filter.EndDate.HasValue || a.DocOn <= filter.EndDate.Value) &&
															(!filter.RegionId.HasValue ||
																(a.PrtnApplication.ChooseLocation
																	? filter.RegionId == a.PrtnApplication.ChoosedRegionId
																	: filter.RegionId == a.RegionId)) &&
															(!filter.DistrictId.HasValue ||
																(a.PrtnApplication.ChooseLocation
																	? filter.DistrictId == a.PrtnApplication.ChoosedDistrictId
																	: filter.DistrictId == a.DistrictId)) &&
															(string.IsNullOrEmpty(filter.Inn) || a.Contractor.Inn == filter.Inn));


			int ExpiredMahallaDate = NewReportConst.ExpiredMahallaDate;
			int ExpiredExperticeDate = NewReportConst.ExpiredExpertizeDate;
			int ExpiredSendToSignDate = NewReportConst.ExpiredSendToSignDate;

			var result = contractor.Select(a => new PrtnApplicationByFullInfoDto
			{
				ContractorRegion = a.Contractor.Region.FullName,
				Region = a.RegionName,
				District = a.DistrictName,
				Organization = a.PrtnContract.Organization.FullName,
				MFY = a.PrtnApplication.MfyName,
				IsRegion = a.PrtnApplication.ChooseLocation ? "Yo\'q" : "Ha",
				INN = a.Contractor.Inn,
				DateSendedOfApplication = a.PrtnApplication.StatusChangeExpireOn,
				DateApplicationSigned = a.PrtnApplication.PassExpertiseExpireOn,

				DaysLateToApplicationSign = CountBusinessDays(a.PrtnApplication.PassExpertiseExpireOn, a.PrtnApplication.StatusChangeExpireOn) > ExpiredMahallaDate
											   ? Math.Abs(CountBusinessDays(a.PrtnApplication.PassExpertiseExpireOn, a.PrtnApplication.StatusChangeExpireOn) - ExpiredMahallaDate)
											   : 0,

				DateOfExpertOpinion = a.PrtnContract.NotPassExpertiseExpireOn ?? a.PrtnContract.PassExpertiseExpireOn,

				DateLateToExpertOpinion = CountBusinessDays(a.PrtnContract.NotPassExpertiseExpireOn ?? a.PrtnContract.PassExpertiseExpireOn, a.PrtnApplication.PassExpertiseExpireOn) > ExpiredExperticeDate
											 ? Math.Abs(CountBusinessDays(a.PrtnContract.NotPassExpertiseExpireOn ?? a.PrtnContract.PassExpertiseExpireOn, a.PrtnApplication.PassExpertiseExpireOn) - ExpiredExperticeDate)
											 : 0,

				DateSignedByBusinessman = a.PrtnContract.Signs.Where(s => s.SignedAt != null && s.PrtnContractTypeTable.OrderNumber == 1)
															  .Select(s => s.SignedAt)
															  .FirstOrDefault(),

				DateOfOrganization1 = a.PrtnContract.Signs.Where(s => s.SignedAt != null && s.PrtnContractTypeTable.OrderNumber == 2)
																.Select(s => s.SignedAt)
																.FirstOrDefault(),

				DateOfOrganization2 = a.PrtnContract.Signs.Where(s => s.SignedAt != null && s.PrtnContractTypeTable.OrderNumber == 3)
																.Select(s => s.SignedAt)
																.FirstOrDefault(),
				DateLateToOrganization2 = a.PrtnContract.Signs.Any(s => s.PrtnContractTypeTable.OrderNumber == 3 && s.SignedAt != null) && a.PrtnContract.PassExpertiseExpireOn != null
										  ? (CountBusinessDays(a.PrtnContract.Signs.Where(s => s.PrtnContractTypeTable.OrderNumber == 3)
																	.Select(s => s.SignedAt)
																	.FirstOrDefault().Value, a.PrtnContract.PassExpertiseExpireOn.Value) > ExpiredSendToSignDate
											  ? CountBusinessDays(a.PrtnContract.Signs.Where(s => s.PrtnContractTypeTable.OrderNumber == 3)
																	.Select(s => s.SignedAt)
																	.FirstOrDefault().Value, a.PrtnContract.PassExpertiseExpireOn.Value) - ExpiredSendToSignDate
											  : 0)
										  : 0,


				DateOfCertificate = a.PrtnContract.PrtnCertificate.DocOn,


			});

			return result;
		}
		public PagedResult<PrtnEmploymentGraphReportNewDto> GetPrtnEmploymentGraphNewReportWithPagination(PrtnDocumentSortFilterOptions filter)
		=> GetPrtnEmploymentGraphNewReport(filter).AsPagedResult(filter);
		public IQueryable<PrtnEmploymentGraphReportNewDto> GetPrtnEmploymentGraphNewReport(PrtnDocumentSortFilterOptions filter)
		{
			var query = _unitOfWork.Context.Set<Application>().Include(a => a.PrtnApplication)
																   .Include(a => a.PrtnContract)
																   .Where(a =>
																		   (!filter.PrtnContractTypeId.HasValue ||
																			   (a.PrtnApplication != null && filter.PrtnContractTypeId == a.PrtnApplication.PrtnContractTypeId)) &&
																		   (!filter.StartDate.HasValue || a.DocOn >= filter.StartDate.Value) &&
																		   (!filter.EndDate.HasValue || a.DocOn <= filter.EndDate.Value) &&
																		   (!filter.RegionId.HasValue ||
																			   (a.PrtnApplication.ChooseLocation
																				   ? filter.RegionId == a.PrtnApplication.ChoosedRegionId
																				   : filter.RegionId == a.RegionId)) &&
																		   (!filter.DistrictId.HasValue ||
																			   (a.PrtnApplication.ChooseLocation
																				   ? filter.DistrictId == a.PrtnApplication.ChoosedDistrictId
																				   : filter.DistrictId == a.DistrictId)) &&
																		   (string.IsNullOrEmpty(filter.Inn) || a.Contractor.Inn == filter.Inn));


			int ExpiredExpertizeDate = (NewReportConst.ExpiredExpertizeDate);

			var result = query.Select(a => new PrtnEmploymentGraphReportNewDto
			{

				RegionId = a.PrtnApplication.ChooseLocation ? (a.PrtnApplication.ChoosedRegionId ?? a.RegionId) : a.RegionId,
				Region = a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegion.Translates.AsQueryable()
								.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
								?? a.PrtnApplication.ChoosedRegion.FullName
								: a.Region.Translates.AsQueryable()
								.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
								?? a.Region.FullName,

				DistrictId = a.PrtnApplication.ChooseLocation
								? a.PrtnApplication.ChoosedDistrictId
								: a.DistrictId,

				District = a.PrtnApplication.ChooseLocation
							? (a.PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
								.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
								?? a.PrtnApplication.ChoosedDistrict.FullName)
							: (a.District.Translates.AsQueryable()
								.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
								?? a.District.FullName),

				Mfy = a.PrtnApplication != null ? a.PrtnApplication.MfyName : null,
				MfyId = a.PrtnApplication != null ? a.PrtnApplication.MfyId : (int?)null,
				OrganizationName = a.PrtnContract != null && a.PrtnContract.Organization != null ? a.PrtnContract.Organization.FullName : null,
				OrganizationId = a.PrtnContract != null ? a.PrtnContract.OrganizationId : (int?)null,
				ContractorInn = a.Contractor != null ? a.Contractor.Inn : null,
				ContractorId = a.PrtnContract.Contractor.Id,
				SentForExamination = a.PrtnContract.StatusChangeExpireOn,
				Status = a.PrtnContract.Status.FullName,
				StatusId = a.PrtnContract.StatusId,
				DateOfConclusionByJustice = a.PrtnContract.NotPassExpertiseExpireOn ?? a.PrtnContract.PassExpertiseExpireOn,

				DaysLate = CountBusinessDays(a.PrtnContract.NotPassExpertiseExpireOn ?? a.PrtnContract.PassExpertiseExpireOn,
												a.PrtnContract.StatusChangeExpireOn) > ExpiredExpertizeDate ? CountBusinessDays(a.PrtnContract.NotPassExpertiseExpireOn ?? a.PrtnContract.PassExpertiseExpireOn,
												a.PrtnContract.StatusChangeExpireOn) - ExpiredExpertizeDate : 0,

			});

			return result;
		}
		public List<PrtnApplicationAndContractInfoDto> GetExpiredContractorsReport(PrtnApplicationAndContractInfoDtoFilter filter)
		{
			int extraExpireDay = 0;
			if (filter.ExpireDay != null)
			{
				if (filter.ExpireDay == 8)
					extraExpireDay = 8;
				else if (filter.ExpireDay == 9)
					extraExpireDay = 30;
				else
					extraExpireDay = filter.ExpireDay.Value;

			}

			if (filter.RegionId == null) filter.ByRegion = true;
			else if (filter.RegionId != null && filter.DistrictId == null) filter.ByDistrict = true;
			else filter.ByContractor = true;

			int ExpiredMahallaDate = (NewReportConst.ExpiredMahallaDate + extraExpireDay);
			int ExpiredExpertizeDate = (NewReportConst.ExpiredExpertizeDate + extraExpireDay);
			int ExpiredRecentExpertizeDate = (NewReportConst.ExpiredRecentExpertizeDate + extraExpireDay);
			int ExpiredPassToSignDate = (NewReportConst.ExpiredPassToSignDate + extraExpireDay);
			int ExpiredPassToSigningDate = (NewReportConst.ExpiredPassToSigningDate + extraExpireDay);
			int ExpiredToSignDate = (NewReportConst.ExpiredToSignDate + extraExpireDay);
			int ExpireToGenerateCertificate = (NewReportConst.ExpireToGenerateCertificate + extraExpireDay);


			var result = new List<PrtnApplicationAndContractInfoDto>();

			IQueryable<Application> query = _unitOfWork.Context.Set<Application>()
				.Include(a => a.PrtnApplication)
				.Include(a => a.PrtnContract).ThenInclude(a => a.PrtnCertificate)
				.Include(a => a.Contractor).ThenInclude(c => c.Oked).ThenInclude(a => a.OkedType)
				.Where(a => new int[]
					{
						StatusIdConst.SENT_FOR_REVIEW, StatusIdConst.SENT,
						StatusIdConst.ACCEPTED, StatusIdConst.EXECUTING,
						StatusIdConst.REJECTED, StatusIdConst.PASS_EXPERTISE,
						StatusIdConst.NOT_PASS_EXPERTISE, StatusIdConst.SIGNED,
						StatusIdConst.SIGNING, StatusIdConst.SENT_FOR_EXPERTISE,
						StatusIdConst.CANCELED, StatusIdConst.REVOKED
					}.Contains(a.StatusId)
					&& a.ApplicationTypeId == ApplicationTypeIdConst.PARTNER
				)
				.Where(a => !filter.PrtnContractTypeId.HasValue || filter.PrtnContractTypeId == a.PrtnApplication.PrtnContractTypeId);

			query = query.Where(a => (filter.StartDate.HasValue ? a.DocOn >= filter.StartDate.Value : true)
				  && (filter.EndDate.HasValue ? a.DocOn <= filter.EndDate.Value : true));

			query = query.Where(a => (!filter.RegionId.HasValue || filter.RegionId == (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId : a.RegionId))
				&& (!filter.DistrictId.HasValue || filter.DistrictId == (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId))
				&& (!filter.ContractorId.HasValue || filter.ContractorId == a.ContractorId) && (!filter.OkedTypeId.HasValue || filter.OkedTypeId == a.Contractor.Oked.OkedTypeId));


			//var query2 = query.Where(a =>  DateTime.Now - a.PrtnApplication.StatusChangeExpireOn.Value > ExpiredMahallaDate && a.PrtnApplication.Id == 3218 && a.PrtnApplication.PassExpertiseExpireOn== null).ToList();

			result = query
			.Select(a => new PrtnApplicationAndContractInfoDto
			{
				PrtnContractTypeId = filter.PrtnContractTypeId.HasValue ? a.PrtnApplication.PrtnContractTypeId : 0,
				PrtnContractType = filter.PrtnContractTypeId.HasValue ? (a.PrtnApplication.PrtnContractType.Translates.AsQueryable()
						.FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.PrtnContractType.FullName) : "",

				RegionId = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId : a.RegionId) : null,
				RegionOrderCode = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegion.OrderCode : a.Region.OrderCode) : null,
				Region = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? (a.PrtnApplication.ChoosedRegion.Translates.AsQueryable()
								.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.ChoosedRegion.FullName)
							: (a.Region.Translates.AsQueryable()
								.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Region.FullName))
						: null,

				DistrictId = filter.ByDistrict ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId) : null,
				District = filter.ByDistrict
						? (a.PrtnApplication.ChooseLocation
							? (a.PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
								.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.ChoosedDistrict.FullName)
							: (a.District.Translates.AsQueryable()
								.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.District.FullName))
						: null,

				ContractorId = filter.ByContractor ? a.ContractorId : null,
				Contractor = filter.ByContractor ? a.Contractor.FullName : null,
				ContractorInn = filter.ByContractor ? a.Contractor.Inn : null,
				ContractorPhoneNumber = filter.ByContractor ? a.Contractor.PhoneNumber : null,

				NewVacanciesCount = a.PrtnApplication.NewVacanciesCount,

				//xatoroq
				ExpiredIsBeingCosideredApplicationCount = (CountBusinessDays(a.PrtnApplication.PassExpertiseExpireOn, a.PrtnApplication.StatusChangeExpireOn) > ExpiredMahallaDate || (a.PrtnApplication.PassExpertiseExpireOn == null && CountBusinessDays(DateTime.Now, a.PrtnApplication.StatusChangeExpireOn) > ExpiredMahallaDate)) ? 1 : 0,

				ExpiredSendToExpertiseCount = (a.PrtnContract.StatusId == StatusIdConst.SENT_FOR_EXPERTISE && (CountBusinessDays(DateTime.Now, a.PrtnContract.StatusChangeExpireOn)) > ExpiredExpertizeDate) || (a.StatusId != StatusIdConst.SENT_FOR_EXPERTISE && ((CountBusinessDays(a.PrtnContract.StatusChangeExpireOn, a.PrtnContract.PassExpertiseExpireOn) > ExpiredExpertizeDate) || (CountBusinessDays(a.PrtnContract.StatusChangeExpireOn, a.PrtnContract.NotPassExpertiseExpireOn) > ExpiredExpertizeDate))) ? 1 : 0,

				//resent
				NotPassCount = a.PrtnContract.NotPassExpertiseExpireOn != null ? 1 : 0,
				ExpiredResentToExpiredCount = (a.PrtnContract.StatusId == StatusIdConst.NOT_PASS_EXPERTISE && CountBusinessDays(DateTime.Now, a.PrtnContract.NotPassExpertiseExpireOn) > ExpiredRecentExpertizeDate) || (a.PrtnContract.NotPassExpertiseExpireOn != null && a.PrtnContract.StatusChangeExpireOn > a.PrtnContract.NotPassExpertiseExpireOn && CountBusinessDays(a.PrtnContract.StatusChangeExpireOn, a.PrtnContract.NotPassExpertiseExpireOn) > ExpiredRecentExpertizeDate) ? 1 : 0,



				TotalPrtnApplicationSentCount = a.StatusId == StatusIdConst.SENT ? 1 : 0,
				TotalPrtnApplicationPassExpertisesCount = a.PrtnContract.StatusId == StatusIdConst.PASS_EXPERTISE ? 1 : 0,

				TotalPrtnApplicationSentForExpertisesCount = a.PrtnContract.StatusId == StatusIdConst.SENT_FOR_EXPERTISE && a.PrtnContract.PrtnContractTypeId != PrtnContractTypeIdConst._201__ ? 1 : 0,
				TotalPrtnApplicationNotPassExpertisesCount = a.PrtnContract.StatusId == StatusIdConst.NOT_PASS_EXPERTISE && a.PrtnContract.PrtnContractTypeId != PrtnContractTypeIdConst._201__ ? 1 : 0,


				TotalPrtnApplicationSentForExpertisesCount2 = a.PrtnContract.StatusId == StatusIdConst.SENT_FOR_EXPERTISE && a.PrtnContract.PrtnContractTypeId == PrtnContractTypeIdConst._201__ ? 1 : 0,
				TotalPrtnApplicationNotPassExpertisesCount2 = a.PrtnContract.StatusId == StatusIdConst.NOT_PASS_EXPERTISE && a.PrtnContract.PrtnContractTypeId == PrtnContractTypeIdConst._201__ ? 1 : 0,

				TotalPrtnApplicationSignedCount = a.PrtnContract.StatusId == StatusIdConst.SIGNED ? 1 : 0,
				TotalPrtnApplicationSignningCount = a.PrtnContract.StatusId == StatusIdConst.SIGNING ? 1 : 0,
				TotalPrtnApplicationSentForReviewCount = a.StatusId == StatusIdConst.SENT_FOR_REVIEW ? 1 : 0,
				TotalPrtnApplicationSentAcceptedCount = a.StatusId == StatusIdConst.ACCEPTED ? 1 : 0,
				TotalPrtnApplicationSentRejectedCount = a.StatusId == StatusIdConst.REJECTED ? 1 : 0,
				TotalPrtnApplicationSentRevokedCount = a.StatusId == StatusIdConst.REVOKED ? 1 : 0,
				TotalPrtnApplicationCanceledCount = a.StatusId == StatusIdConst.CANCELED ? 1 : 0,

				TotalPrtnApplicationCanceledWhithOutContractCount = a.PrtnContract == null && a.StatusId == StatusIdConst.CANCELED ? 1 : 0,
				TotalPrtnApplicationCanceledWhithOutRejectCount = a.PrtnContract == null && a.StatusId == StatusIdConst.REJECTED ? 1 : 0,  //

				TotalPrtnContractCanceledWhithOutCertificateCount = a.PrtnContract.PrtnCertificate == null && a.PrtnContract.StatusId == StatusIdConst.CANCELED ? 1 : 0,
				TotalPrtnContractRejectWhithOutCertificateCount = a.PrtnContract.PrtnCertificate == null && a.PrtnContract.StatusId == StatusIdConst.REJECTED ? 1 : 0,

				TotalPrtnCertificateCanceledApplicationCount = (a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.CANCELED) ? 1 : 0,
				TotalPrtnCertificateRejectApplicationCount = (a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.REJECTED) ? 1 : 0,

				TotalPrtnContractCount = a.PrtnContract != null && a.PrtnContract.StatusId != StatusIdConst.REJECTED ? 1 : 0,

				PassCount1 = a.PrtnContract != null && a.PrtnContract.StatusId != StatusIdConst.REJECTED && a.PrtnContract.PrtnContractTypeId != PrtnContractTypeIdConst._201__ ? 1 : 0,
				SignExpireOnCount1 = (a.PrtnContract.StatusId == StatusIdConst.PASS_EXPERTISE && (a.PrtnContract.PrtnContractTypeId == 1 || a.PrtnContract.PrtnContractTypeId == 2) && CountBusinessDays(DateTime.Now, a.PrtnContract.PassExpertiseExpireOn) > ExpiredPassToSignDate) || (a.PrtnContract.SignExpireOn != null && (a.PrtnContract.PrtnContractTypeId == 1 || a.PrtnContract.PrtnContractTypeId == 2) && CountBusinessDays(a.PrtnContract.SignExpireOn, a.PrtnContract.PassExpertiseExpireOn) > ExpiredPassToSignDate) ? 1 : 0,



				TotalPrtnContractCancelCount = a.PrtnContract != null && a.PrtnContract.StatusId == StatusIdConst.CANCELED ? 1 : 0,
				TotalPrtnContractRejectedCount = a.PrtnContract != null && a.PrtnContract.StatusId == StatusIdConst.REJECTED ? 1 : 0,
				TotalPrtnCertificateCount = a.PrtnContract.PrtnCertificate != null && a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED ? 1 : 0,
				TotalPrtnCertificateCanceledCount = a.PrtnContract.PrtnCertificate != null && a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.CANCELED ? 1 : 0,
				TotalNewVacanciesCount = a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED ? a.PrtnApplication.NewVacanciesCount : 0,

				TotalPrtnApplicationIsOffersCount = a.Contractor.IsLastOffer ? 1 : 0,

				PassCount2 = a.PrtnContract != null && a.PrtnContract.StatusId != StatusIdConst.REJECTED && a.PrtnContract.PrtnContractTypeId == PrtnContractTypeIdConst._201__ ? 1 : 0,
				SigningCount = (a.PrtnContract.PrtnContractTypeId == 3 && (a.PrtnContract.Signs.Count(a => a.SignedAt != null) == 2 || a.PrtnContract.Signs.Count(a => a.SignedAt != null) == 3)) ? 1 : 0,
				SigningExpireOnCount = (a.PrtnContract.StatusId == StatusIdConst.PASS_EXPERTISE && a.PrtnContract.PrtnContractTypeId == 3 && CountBusinessDays(DateTime.Now, a.PrtnContract.PassExpertiseExpireOn) > ExpiredPassToSigningDate) || (a.PrtnContract.StatusId == StatusIdConst.SIGNING && a.PrtnContract.PrtnContractTypeId == 3 && a.PrtnContract.Signs.Count(s => s.SignedAt != null) == 1 && CountBusinessDays(DateTime.Now, a.PrtnContract.Signs.Where(s => s.SignedAt != null).OrderBy(s => s.SignedAt).FirstOrDefault().SignedAt) > ExpiredPassToSigningDate) || (a.PrtnContract.PrtnContractTypeId == 3 && a.PrtnContract.Signs.Count(s => s.SignedAt != null) != 1 && CountBusinessDays(a.PrtnContract.SigningExpireOn, a.PrtnContract.Signs.Where(s => s.SignedAt != null).OrderBy(s => s.SignedAt).FirstOrDefault().SignedAt) > ExpiredPassToSigningDate) ? 1 : 0,
				SignExpireOnCount2 = (a.PrtnContract.StatusId == StatusIdConst.SIGNING && a.PrtnContract.PrtnContractTypeId == 3 && a.PrtnContract.Signs.Count(s => s.SignedAt != null) == 2 && CountBusinessDays(DateTime.Now, a.PrtnContract.SigningExpireOn) > ExpiredToSignDate) || (a.PrtnContract.SignExpireOn != null && a.PrtnContract.PrtnContractTypeId == 3 && a.PrtnContract.Signs.Count(s => s.SignedAt != null) == 3 && CountBusinessDays(a.PrtnContract.SignExpireOn, a.PrtnContract.SigningExpireOn) > ExpiredToSignDate) ? 1 : 0,
				NotGeneratedCertificatesCount = a.PrtnContract.StatusId == StatusIdConst.SIGNED ? 1 : 0,
				GeneratedCertificatesCount = (a.PrtnContract.StatusId == StatusIdConst.SIGNED && a.PrtnContract.PrtnCertificate == null && CountBusinessDays(DateTime.Now, a.PrtnContract.SignExpireOn) > ExpireToGenerateCertificate) || (a.PrtnContract.StatusId == StatusIdConst.SIGNED && a.PrtnContract.PrtnCertificate != null && CountBusinessDays(a.PrtnContract.PrepareCertificateExpireOn, a.PrtnContract.SignExpireOn) > ExpireToGenerateCertificate) ? 1 : 0


			})
				.AsEnumerable()
				.GroupBy(a => new
				{
					//a.PrtnContractTypeId,
					//a.PrtnContractType,
					a.DistrictId,
					a.District,
					a.RegionId,
					a.RegionOrderCode,
					a.Region,
					a.ContractorId,
					a.Contractor,
					a.ContractorInn,
					a.ContractorPhoneNumber,
				})
				.Select(a => new PrtnApplicationAndContractInfoDto
				{
					//PrtnContractTypeId = a.Key.PrtnContractTypeId,
					//PrtnContractType = a.Key.PrtnContractType,
					DistrictId = a.Key.DistrictId,
					District = a.Key.District,
					RegionId = a.Key.RegionId,
					RegionOrderCode = a.Key.RegionOrderCode,
					Region = a.Key.Region,
					ContractorId = a.Key.ContractorId,
					Contractor = a.Key.Contractor,
					ContractorInn = a.Key.ContractorInn,
					ContractorPhoneNumber = a.Key.ContractorPhoneNumber,
					TotalPrtnApplicationSentCount = a.Sum(b => b.TotalPrtnApplicationSentCount),

					TotalApplication = ((long)a.Count(), a.Sum(c => c.NewVacanciesCount)),

					TotalPrtnApplicationCanceledWhithOutContractCount = a.Sum(b => b.TotalPrtnApplicationCanceledWhithOutContractCount),
					TotalPrtnApplicationCanceledWhithOutRejectCount = a.Sum(b => b.TotalPrtnApplicationCanceledWhithOutRejectCount),


					TotalPrtnContractCanceledWhithOutCertificateCount = a.Sum(b => b.TotalPrtnContractCanceledWhithOutCertificateCount),
					TotalPrtnContractRejectWhithOutCertificateCount = a.Sum(b => b.TotalPrtnContractRejectWhithOutCertificateCount),

					TotalPrtnCertificateCanceledApplicationCount = a.Sum(b => b.TotalPrtnCertificateCanceledApplicationCount),
					TotalPrtnCertificateRejectApplicationCount = a.Sum(b => b.TotalPrtnCertificateRejectApplicationCount),
					TotalPrtnApplicationSentForReviewCount = a.Sum(b => b.TotalPrtnApplicationSentForReviewCount),
					TotalPrtnApplicationSentAcceptedCount = a.Sum(b => b.TotalPrtnApplicationSentAcceptedCount),
					TotalPrtnApplicationPassExpertisesCount = a.Sum(b => b.TotalPrtnApplicationPassExpertisesCount),
					TotalPrtnApplicationSentForExpertisesCount = a.Sum(b => b.TotalPrtnApplicationSentForExpertisesCount),
					TotalPrtnApplicationNotPassExpertisesCount = a.Sum(b => b.TotalPrtnApplicationNotPassExpertisesCount),
					TotalPrtnApplicationSignedCount = a.Sum(b => b.TotalPrtnApplicationSignedCount),
					TotalPrtnApplicationSignningCount = a.Sum(b => b.TotalPrtnApplicationSignningCount),
					TotalPrtnApplicationCanceledCount = a.Sum(b => b.TotalPrtnApplicationCanceledCount),
					TotalPrtnApplicationSentRejectedCount = a.Sum(b => b.TotalPrtnApplicationSentRejectedCount),
					TotalPrtnApplicationSentRevokedCount = a.Sum(b => b.TotalPrtnApplicationSentRevokedCount),
					TotalPrtnContractCancelCount = a.Sum(b => b.TotalPrtnContractCancelCount),
					TotalPrtnContractRejectedCount = a.Sum(b => b.TotalPrtnContractRejectedCount),
					TotalPrtnCertificateCount = a.Sum(b => b.TotalPrtnCertificateCount),
					TotalPrtnCertificateCanceledCount = a.Sum(b => b.TotalPrtnCertificateCanceledCount),
					TotalNewVacanciesCount = a.Sum(b => b.TotalNewVacanciesCount),
					TotalPrtnApplicationCount = a.Sum(b => b.TotalPrtnApplicationSentCount) + a.Sum(b => b.TotalPrtnApplicationSentForReviewCount) + a.Sum(b => b.TotalPrtnApplicationSentAcceptedCount),
					TotalPrtnContractCount = a.Sum(b => b.TotalPrtnContractCount),
					TotalPrtnApplicationIsOffersCount = a.Sum(b => b.TotalPrtnApplicationIsOffersCount),

					ExpiredIsBeingCosideredApplicationCount = a.Sum(b => b.ExpiredIsBeingCosideredApplicationCount),
					IsBeingCosideredApplicationCount = a.Sum(b => b.TotalPrtnApplicationCanceledWhithOutRejectCount) + a.Sum(b => b.TotalPrtnApplicationSentForReviewCount) + a.Sum(b => b.TotalPrtnApplicationSentAcceptedCount),
					SendToExpertiseCount = a.Sum(b => b.TotalPrtnContractCount),
					ExpiredSendToExpertiseCount = a.Sum(b => b.ExpiredSendToExpertiseCount),
					NotPassCount = a.Sum(a => a.NotPassCount),
					ExpiredResentToExpiredCount = a.Sum(a => a.ExpiredResentToExpiredCount),
					PassCount1 = a.Sum(a => a.PassCount1) - a.Sum(b => b.TotalPrtnApplicationSentForExpertisesCount) - a.Sum(b => b.TotalPrtnApplicationNotPassExpertisesCount),
					SignExpireOnCount1 = a.Sum(b => b.SignExpireOnCount1),
					PassCount2 = a.Sum(a => a.PassCount2) - a.Sum(b => b.TotalPrtnApplicationSentForExpertisesCount2) - a.Sum(b => b.TotalPrtnApplicationNotPassExpertisesCount2),
					SigningExpireOnCount = a.Sum(a => a.SigningExpireOnCount),
					SigningCount = a.Sum(a => a.SigningCount),
					SignExpireOnCount2 = a.Sum(a => a.SignExpireOnCount2),
					NotGeneratedCertificatesCount = a.Sum(a => a.NotGeneratedCertificatesCount),
					GeneratedCertificatesCount = a.Sum(a => a.GeneratedCertificatesCount)

				})
				.ToList();

			// Hamma viloyatlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
			if (filter.ByRegion)
			{
				var regions = _unitOfWork.RegionRepository.AllAsQueryable
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.Id)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							OrderCode = a.OrderCode,
							FullName = a.FullName
						}
					);

				foreach (var region in regions)
				{
					if (result.Select(a => a.RegionId).Contains(region.Key))
						continue;

					result.Add(new PrtnApplicationAndContractInfoDto
					{
						Region = region.Value.FullName,
						RegionOrderCode = region.Value.OrderCode,
						RegionId = region.Key
					});
				}

				result = result.OrderBy(a => a.RegionOrderCode).ToList();
			}

			// Viloyatdagi hamma tumanlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
			if (filter.ByDistrict)
			{
				var districts = _unitOfWork.DistrictRepository.AllAsQueryable
					.Include(a => a.Region).ThenInclude(a => a.Translates)
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.RegionId
						&& !filter.DistrictId.HasValue || filter.DistrictId == a.Id
					)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							RegionId = a.RegionId,
							Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
								?.TranslateText
							?? a.Region.FullName,
							District = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
								?.TranslateText
							?? a.FullName,
							DistrictOrderCode = a.OrderCode,
						}
					);

				foreach (var district in districts)
				{
					if (result.Select(a => a.DistrictId).Contains(district.Key))
						continue;

					result.Add(new PrtnApplicationAndContractInfoDto
					{

						Region = district.Value.Region,
						RegionId = district.Value.RegionId,
						District = district.Value.District,
						RegionOrderCode = district.Value.DistrictOrderCode,
						DistrictId = district.Key
					});
				}

				result = result.OrderBy(a => a.RegionOrderCode).ToList();
			}

			return result;
		}
		public List<SrvDeedDto> GetSrvDeedReport(SrvDeedListReportSortFilter filter)
		{


			List<SrvDeedDto> data = null;
			if (filter.ByRegion && filter.RegionId == null)
			{
				data = _serviceDeedRepository.ReadAsNoTracked<SrvDeedListDto>().SortFilterReport(filter).AsEnumerable()
			   .Select(a => new SrvDeedDto
			   {
				   OrderCode = a.OrderCode,
				   Name = a.ContractorRegion,
				   RegionId = a.ContractorRegionId,
				   Sum = a.Price,
				   GroupName = GetGroupName(a),
				   ServiceName = GetServiceName(a),
				   DeedGroupCollection = new DeedGroupCollection
				   {
					   DeedGroup = new List<DeedGroup>()
				   },

				   Count = 1

			   })
			   .GroupBy(a => new
			   {
				   a.Name,
				   a.RegionId,
				   a.OrderCode
			   })

			   .Select(a => new SrvDeedDto
			   {
				   DeedGroupCollection = new DeedGroupCollection
				   {
					   DeedGroup = a.GroupBy(g => g.GroupName)
				   .Select(g => new DeedGroup
				   {
					   GroupName = g.Key,
					   ServiceNames = g.GroupBy(x => x.ServiceName)
						   .Select(serviceGroup => new ServiceName
						   {
							   ServiceNames = serviceGroup.Key,
							   Count = serviceGroup.Count(),
							   Sum = (decimal)serviceGroup.Sum(x => x.Sum)
						   }).ToList()
				   }).ToList() // Convert to List
				   },

				   OrderCode = a.Key.OrderCode,
				   Name = a.Key.Name,
				   RegionId = a.Key.RegionId,
				   Count = a.Count(),
				   Sum = a.Sum(a => a.Sum)



			   }).ToList();


				var regions = _unitOfWork.RegionRepository.AllAsQueryable
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.Id).OrderBy(r => r.OrderCode)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							OrderCode = a.OrderCode,
							FullName = a.FullName,

						}
					);

				foreach (var region in regions)
				{
					var subData = data.FirstOrDefault(a => a.RegionId == region.Key);

					if (subData is null)
					{

						data.Add(new SrvDeedDto
						{
							DeedGroupCollection = new DeedGroupCollection
							{
								DeedGroup = new List<DeedGroup>()
							},
							Name = region.Value.FullName,
							RegionId = region.Key,
							OrderCode = region.Value.OrderCode

						});
					}
					subData = data.First(a => a.RegionId == region.Key);
					//if (data.Select(a => a.RegionId).Contains(region.Key))
					//continue;



					var groups = _unitOfWork.Context.NeedChamberServiceGroups
				   .Include(a => a.Translates)
				   .IsActive()
				   .OrderBy(r => r.OrderCode)
				   .ToDictionary(
					   a => a.Id,
					   a => new
					   {
						   OrderCode = a.OrderCode,
						   FullName = a.FullName,
					   }
				   );

					foreach (var group in groups)
					{
						var subgroupdata = subData.DeedGroupCollection.DeedGroup.FirstOrDefault(a => a.GroupId == group.Key);

						if (subgroupdata is null)
						{
							subData.DeedGroupCollection.DeedGroup.Add(new DeedGroup
							{
								GroupId = group.Key,
								GroupName = group.Value.FullName,
								ServiceNames = new()
							});
						}

						subgroupdata = subData.DeedGroupCollection.DeedGroup.First(a => a.GroupId == group.Key);

						var services = _unitOfWork.Context.NeedChamberServices
							 .Include(a => a.Translates)
							 .IsActive()
							 .Where(a => a.NeedChamberServiceGroupId == group.Key && a.ServicePriceTypeId != 2)
							 .OrderBy(r => r.OrderCode)
							 .ToDictionary(
								 a => a.Id,
								 a => new
								 {
									 OrderCode = a.OrderCode,
									 FullName = a.FullName,
								 }
							 );
						foreach (var ser in services)
						{
							var subservicedata = subgroupdata.ServiceNames.FirstOrDefault(a => a.Id == ser.Key);

							if (subservicedata is null)
							{
								subgroupdata.ServiceNames.Add(new ServiceName
								{
									Id = ser.Key,
									Count = 0,
									Sum = 0,
									ServiceNames = ser.Value.FullName
								});
							}
						}
					}
				}
			}



			if (filter.ByDistrict && filter.RegionId != null)
			{
				var Organization = _unitOfWork.Context.Set<Organization>().Where(a => a.OrganizationGroupId == 3 && a.RegionId == filter.RegionId).FirstOrDefault().FullName;
				data = _serviceDeedRepository.ReadAsNoTracked<SrvDeedListDto>().SortFilterReport(filter).AsEnumerable().Where(a => a.ContractorRegionId == filter.RegionId)
				.Select(a => new SrvDeedDto
				{
					RegionId = a.ContractorRegionId,
					SrvApplicationOrganizationId = a.SrvApplicationOrganizationId,
					Name = a.SrvApplicationOrganizationId == null ? a.ContractorDistrict : Organization,
					DistrictId = a.SrvApplicationOrganizationId == null ? a.ContractorDistrictId : 0,
					Sum = a.Price,//GetSum(a.Id, dataOrg, Groups),
					GroupName = GetGroupName(a),
					ServiceName = GetServiceName(a),
					DeedGroupCollection = new DeedGroupCollection
					{
						DeedGroup = new List<DeedGroup>()
					},

					Count = 1

				})
				.GroupBy(a => new
				{
					a.Name,
					a.DistrictId,
					a.RegionId,
					//a.SrvApplicationOrganizationId,
				})

				.Select(a => new SrvDeedDto
				{
					DeedGroupCollection = new DeedGroupCollection
					{
						DeedGroup = a.GroupBy(g => g.GroupName)
					.Select(g => new DeedGroup
					{
						GroupName = g.Key,
						ServiceNames = g.GroupBy(x => x.ServiceName)
							.Select(serviceGroup => new ServiceName
							{
								ServiceNames = serviceGroup.Key,
								Count = serviceGroup.Count(),
								Sum = (decimal)serviceGroup.Sum(x => x.Sum)
							}).ToList()
					}).ToList() // Convert to List
					},
					RegionId = a.Key.RegionId,
					DistrictId = a.Key.DistrictId,
					//SrvApplicationOrganizationId = a.Key.SrvApplicationOrganizationId,
					Name = a.Key.Name,
					Count = a.Count(),
					Sum = a.Sum(a => a.Sum)



				}).ToList();

				var districts = _unitOfWork.DistrictRepository.AllAsQueryable.Where(a => a.RegionId == filter.RegionId)
				   .Include(a => a.Region).ThenInclude(a => a.Translates)
				   .Include(a => a.Translates)
				   .IsActive()
				   .Where(a => !filter.RegionId.HasValue || filter.RegionId == a.RegionId
					   && !filter.DistrictId.HasValue || filter.DistrictId == a.Id
				   )
				   .ToDictionary(
					   a => a.Id,
					   a => new
					   {
						   RegionId = a.RegionId,
						   Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
							   ?.TranslateText
						   ?? a.Region.FullName,
						   District = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
							   ?.TranslateText
						   ?? a.FullName,
						   DistrictOrderCode = a.OrderCode,
					   }
				   );

				foreach (var district in districts)
				{
					var subData = data.FirstOrDefault(a => a.DistrictId == district.Key);

					if (subData is null)
					{
						data.Add(new SrvDeedDto
						{
							DeedGroupCollection = new DeedGroupCollection
							{
								DeedGroup = new List<DeedGroup>()
							},
							Name = district.Value.District,
							DistrictId = district.Key,
							OrderCode = district.Value.DistrictOrderCode

						});
					}

					subData = data.First(a => a.DistrictId == district.Key);

					var groups = _unitOfWork.Context.NeedChamberServiceGroups
				   .Include(a => a.Translates)
				   .IsActive()
				   .OrderBy(r => r.OrderCode)
				   .ToDictionary(
					   a => a.Id,
					   a => new
					   {
						   OrderCode = a.OrderCode,
						   FullName = a.FullName,
					   }
				   );

					foreach (var group in groups)
					{
						var subgroupdata = subData.DeedGroupCollection.DeedGroup.FirstOrDefault(a => a.GroupId == group.Key);

						if (subgroupdata is null)
						{
							subData.DeedGroupCollection.DeedGroup.Add(new DeedGroup
							{
								GroupId = group.Key,
								GroupName = group.Value.FullName,
								ServiceNames = new()
							});
						}

						subgroupdata = subData.DeedGroupCollection.DeedGroup.First(a => a.GroupId == group.Key);

						var services = _unitOfWork.Context.NeedChamberServices
							 .Include(a => a.Translates)
							 .IsActive()
							 .Where(a => a.NeedChamberServiceGroupId == group.Key && a.ServicePriceTypeId != 2)
							 .OrderBy(r => r.OrderCode)
							 .ToDictionary(
								 a => a.Id,
								 a => new
								 {
									 OrderCode = a.OrderCode,
									 FullName = a.FullName,
								 }
							 );



						foreach (var ser in services)
						{
							var subservicedata = subgroupdata.ServiceNames.FirstOrDefault(a => a.Id == ser.Key);

							if (subservicedata is null)
							{
								subgroupdata.ServiceNames.Add(new ServiceName
								{
									Id = ser.Key,
									Count = 0,
									Sum = 0,
									ServiceNames = ser.Value.FullName
								});
							}
						}
					}


				}
			}
			return data.OrderBy(a => a.OrderCode).ToList();
		}
		private string GetGroupName(SrvDeedListDto a)
		{


			var data = a.Groups.Where(a => a.Tables.Count() != 0)?.FirstOrDefault()?.Group;
			return data ?? string.Empty;
		}
		private string GetServiceName(SrvDeedListDto a)
		{
			var data = a.Groups.Where(a => a.Tables.Count() != 0)?.FirstOrDefault()?.Tables.FirstOrDefault()?.NeedChamberService;
			return data ?? string.Empty;
		}
		public List<SrvDeedDto> GetSrvFreeDeedReport(SrvDeedListReportSortFilter filter)
		{
			var langId = ServiceProvider.CultureHelper.CurrentCulture.Id;
			List<FreeDeedReportDto> data = _unitOfWork.Context.GetRegionOrDistrict(filter.FromDocDate, filter.ToDocDate, filter.RegionId, filter.DistrictId, langId).ToList();
			var sortData = data.Where(w => w.DistrictId != 0 || w.OrganizationId != 0);

			var result = new List<SrvDeedDto>();

			if (filter.ByRegion)
			{
				result = sortData.GroupBy(a => new { a.RegionId, a.RegionName }).Select(a => new SrvDeedDto
				{
					RegionId = a.Key.RegionId,
					Name = a.Key.RegionName,
					Count = a.Sum(b => b.Count),
					DeedGroup = a.GroupBy(b => new { b.GroupId, b.GroupName }).Select(b => new DeedGroup
					{
						GroupId = b.Key.GroupId,
						GroupName = b.Key.GroupName,
						ServiceNames = b.Select(c => new ServiceName
						{
							Id = c.ServiceId,
							ServiceNames = c.ServiceName,
							Count = c.Count
						}).ToList()
					}).ToList()

				}).ToList();
			}

			if (filter.ByDistrict)
			{
				
				var Organization = _unitOfWork.Context.Set<Organization>().
					Include(q=>q.Translates).
					Where(a => a.OrganizationGroupId == 3 && a.RegionId == data.FirstOrDefault().RegionId).
					FirstOrDefault();

				   string orgName = Organization.Translates.AsQueryable().FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, langId))?.TranslateText ?? Organization.FullName;

					result = sortData.GroupBy(a => new { a.DistrictId, a.OrganizationId }).Select(a => new SrvDeedDto
					{
						RegionId = filter.RegionId.Value,
						Name = a.Key.OrganizationId == 0 ? a.First().DistrictName : orgName,
						DistrictId = a.Key.OrganizationId == 0 ? a.Key.DistrictId : 0,
						OrganizationId = a.Key.OrganizationId,
						Count = a.Sum(b => b.Count),
						DeedGroup = a.GroupBy(b => new { b.GroupId, b.GroupName }).Select(b => new DeedGroup
						{
							GroupId = b.Key.GroupId,
							GroupName = b.Key.GroupName,
							ServiceNames = b.Select(c => new ServiceName
							{
								Id = c.ServiceId,
								ServiceNames = c.ServiceName,
								Count = c.Count
							}).ToList()
						}).ToList()

					}).ToList();
				}

				return result.OrderByDescending(a => a.OrganizationId).ToList();
			}
		public List<SrvDeedDto> GetSrvFreeDeedReportFromFunction(SrvDeedListReportSortFilter filter)
		{
			var start = new DateOnly(2023, 01, 01);
			var end = new DateOnly(2024, 09, 26);
			var data = _unitOfWork.Context.GetRegionOrDistrict(start, end, 0, 0, 1).ToList();
			return null;
		}
		private string GetServiceName(SrvApplicationListDto a)
		{
			var data = a.Groups.FirstOrDefault().Tables.FirstOrDefault().NeedChamberService;
			return data;
		}
		private string GetGroupName(SrvApplicationListDto a)
		{
			var data = a.Groups.FirstOrDefault().Group;
			return data;
		}
		private decimal? GetSum(long id, List<SrvDeedListDto> dataOrg, List<SrvDeedGroupDto> groups)
		{

			var data = groups.Where(a => a.Tables.FirstOrDefault().Id == id).FirstOrDefault().Tables.FirstOrDefault().Price;

			return data.HasValue ? data : 0;
		}
		private string GetServiceName(long id, List<SrvDeedListDto> dataOrg, List<SrvDeedGroupDto> groups)
		{

			var name = groups.Where(a => a.Tables.FirstOrDefault().Id == id).FirstOrDefault().Tables.FirstOrDefault().NeedChamberService;
			return string.IsNullOrWhiteSpace(name) ? string.Empty : name;

		}
		private string GetGroupName(long id, List<SrvDeedListDto> dataOrg, List<SrvDeedGroupDto> groups)
		{
			var group = groups.Where(a => a.Tables.FirstOrDefault().Id == id).FirstOrDefault().Group;
			return string.IsNullOrWhiteSpace(group) ? string.Empty : group;
		}
		//private string GetGroupNameFree(long id, List<SrvDeedListDto> dataOrg, List<SrvDeedGroupDto> groups)
		//{

		//    return groups.Where(a => a.Tables.FirstOrDefault().Id == id).FirstOrDefault().Group;
		//}
		static int CountBusinessDays(DateTime? end, DateTime? start)  //
		{
			if (end == null || start == null || end < start)
			{
				return 0;
			}

			DateTime startDate = start.Value;
			DateTime endDate = end.Value;

			// Total number of days between the start and end dates
			int totalDays = (endDate - startDate).Days + 1;

			// Calculate the number of complete weeks between the dates
			int fullWeeks = totalDays / 7;
			int businessDays = fullWeeks * 5;

			// Calculate the remaining days outside the full weeks
			int extraDays = totalDays % 7;

			// Determine the day of the week for the start date
			DayOfWeek startDayOfWeek = startDate.DayOfWeek;

			// Check the remaining days and count the business days
			for (int i = 0; i < extraDays; i++)
			{
				DayOfWeek currentDay = (DayOfWeek)(((int)startDayOfWeek + i) % 7);
				if (currentDay != DayOfWeek.Saturday && currentDay != DayOfWeek.Sunday)
				{
					businessDays++;
				}
			}

			return businessDays;
		}
		public List<MonoApplicationReportDto> MonoApplicationReport(MonoApplicationReportSortFilter filter)
		{
			
			List<MonoApplicationReportDto> monoApplications = new List<MonoApplicationReportDto>();

			if (filter.RegionId is null)
			{
				
				monoApplications = _unitOfWork.Context.Set<MonoApplication>().Include(w => w.MonoApplicationBandlikResults).Include(w => w.Application).Include(w => w.Application).ThenInclude(q => q.Region).
					SortFilter(filter)
					.AsEnumerable()
					.Select(p => new MonoApplicationReportDto
					{
						Id = p.Id,
						RegionId = p.Application.RegionId,
						DistrictId = p.Application.DistrictId,
						StatusId = p.StatusId,
						RegionName = p.Application.RegionName,

						OrderCode = p.Application.Region.OrderCode,
						ReviewApplications = new ReviewApplications
						{
							Count = p.StatusId == StatusIdConst.WAITING ? 1 : 0,
							TotalAmount = p.StatusId == StatusIdConst.WAITING ? p.TotalAmount : 0,
							TotalCost = p.StatusId == StatusIdConst.WAITING ? p.TotalCost : 0
						},

						AskApplications = new AskApplications
						{
							AcceptApplicaions = new AcceptApplicaions
							{

								Count = p.StatusId == StatusIdConst.ACCEPTED ? 1 : 0,
								TotalAmount = p.StatusId == StatusIdConst.ACCEPTED ? p.TotalAmount : 0,
								TotalCost = p.StatusId == StatusIdConst.ACCEPTED ? p.TotalCost : 0,
								SubsidyAmount = p.MonoApplicationBandlikResults.Any() ?
									decimal.Parse(p.MonoApplicationBandlikResults.FirstOrDefault().SubsidyAmount != null && p.MonoApplicationBandlikResults.FirstOrDefault().SubsidyAmount.Length > 0
										? p.MonoApplicationBandlikResults.FirstOrDefault().SubsidyAmount
										: "0")
									: 0
								//SubsidyAmount = 0
							},
							RejectApplicaions = new RejectApplicaions
							{
								Count = p.StatusId == StatusIdConst.REJECTED ? 1 : 0,
								TotalAmount = p.StatusId == StatusIdConst.REJECTED ? p.TotalAmount : 0,
								TotalCost = p.StatusId == StatusIdConst.REJECTED ? p.TotalCost : 0
							}
						},

					})
					.GroupBy(a => new
					{
						a.RegionId,
						a.RegionName,
					})
					.Select(a => new MonoApplicationReportDto
					{
						RegionId = a.Key.RegionId,
						RegionName = a.Key.RegionName,

						ReviewApplications = new ReviewApplications
						{
							Count = a.Sum(b => b.ReviewApplications.Count),
							TotalAmount = a.Sum(b => b.ReviewApplications.TotalAmount),
							TotalCost = a.Sum(b => b.ReviewApplications.TotalCost)
						},

						AskApplications = new AskApplications
						{
							AcceptApplicaions = new AcceptApplicaions
							{
								Count = a.Sum(b => b.AskApplications.AcceptApplicaions.Count),
								TotalAmount = a.Sum(b => b.AskApplications.AcceptApplicaions.TotalAmount),
								TotalCost = a.Sum(b => b.AskApplications.AcceptApplicaions.TotalCost),
								SubsidyAmount = a.Sum(b => b.AskApplications.AcceptApplicaions.SubsidyAmount)
							
							},
							RejectApplicaions = new RejectApplicaions
							{
								Count = a.Sum(b => b.AskApplications.RejectApplicaions.Count),
								TotalAmount = a.Sum(b => b.AskApplications.RejectApplicaions.TotalAmount),
								TotalCost = a.Sum(b => b.AskApplications.RejectApplicaions.TotalCost)
							}
						},
						AllApplicaions = new AllApplicaions
						{
							Count = a.Sum(b =>
								b.ReviewApplications.Count +
								b.AskApplications.AcceptApplicaions.Count +
								b.AskApplications.RejectApplicaions.Count),

							TotalAmount = a.Sum(b =>
								b.ReviewApplications.TotalAmount +
								b.AskApplications.AcceptApplicaions.TotalAmount +
								b.AskApplications.RejectApplicaions.TotalAmount),
							TotalCost = a.Sum(b =>
								b.ReviewApplications.TotalCost +
								b.AskApplications.AcceptApplicaions.TotalCost +
								b.AskApplications.RejectApplicaions.TotalCost)
						}

					}).ToList();

						var regions = _unitOfWork.RegionRepository.AllAsQueryable
							.Include(a => a.Translates)
							.IsActive()
							.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.Id)
							.ToDictionary(
								a => a.Id,
								a => new
								{
									OrderCode = a.OrderCode,
									FullName = /*a.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ??*/ a.FullName
								}
							);

						foreach (var region in regions)
						{
							if (monoApplications.Select(a => a.RegionId).Contains(region.Key))
								continue;

							monoApplications.Add(new MonoApplicationReportDto
							{
								OrderCode = region.Value.OrderCode,
								RegionName = region.Value.FullName,
								RegionId = region.Key,
								AllApplicaions = new AllApplicaions
								{
									Count = 0,
									TotalAmount = 0,
									TotalCost = 0
								},
								ReviewApplications = new ReviewApplications
								{
									Count = 0,
									TotalAmount = 0,
									TotalCost = 0
								},
								AskApplications = new AskApplications
								{
									AcceptApplicaions = new AcceptApplicaions
									{
										Count = 0,
										TotalAmount = 0,
										TotalCost = 0,
										SubsidyAmount = 0
									},
									RejectApplicaions = new RejectApplicaions
									{
										Count = 0,
										TotalAmount = 0,
										TotalCost = 0
									}
								}
								
							});
						}

				monoApplications = monoApplications.OrderBy(a => a.OrderCode).ToList();
			}
			else if(filter.RegionId is not null && filter.DistrictId is null)
			{
				monoApplications = _unitOfWork.Context.Set<MonoApplication>().
					Include(w => w.Application).ThenInclude(q => q.District).
					Where(y=>y.Application.RegionId == filter.RegionId).
					SortFilter(filter).
					Select(p => new MonoApplicationReportDto
					{
						Id = p.Id,
						RegionId = p.Application.RegionId,
						DistrictId = p.Application.DistrictId,
						DistricName = p.Application.DistrictName,
						StatusId = p.StatusId,
						OrderCode = p.Application.District.OrderCode,
						RegionName = p.Application.RegionName,
						ReviewApplications = new ReviewApplications
						{
							Count = p.StatusId == StatusIdConst.WAITING ? 1 : 0,
							TotalAmount = p.StatusId == StatusIdConst.WAITING ? p.TotalAmount : 0,
							TotalCost = p.StatusId == StatusIdConst.WAITING ? p.TotalCost : 0
						},

						AskApplications = new AskApplications
						{
							AcceptApplicaions = new AcceptApplicaions
							{

								Count = p.StatusId == StatusIdConst.ACCEPTED ? 1 : 0,
								TotalAmount = p.StatusId == StatusIdConst.ACCEPTED ? p.TotalAmount : 0,
								TotalCost = p.StatusId == StatusIdConst.ACCEPTED ? p.TotalCost : 0,
								SubsidyAmount = p.MonoApplicationBandlikResults.Any() ?
									decimal.Parse(p.MonoApplicationBandlikResults.FirstOrDefault().SubsidyAmount != null && p.MonoApplicationBandlikResults.FirstOrDefault().SubsidyAmount.Length > 0
										? p.MonoApplicationBandlikResults.FirstOrDefault().SubsidyAmount
										: "0")
									: 0
							},
							RejectApplicaions = new RejectApplicaions
							{
								Count = p.StatusId == StatusIdConst.REJECTED ? 1 : 0,
								TotalAmount = p.StatusId == StatusIdConst.REJECTED ? p.TotalAmount : 0,
								TotalCost = p.StatusId == StatusIdConst.REJECTED ? p.TotalCost : 0
							}
						},

					})
					.GroupBy(a => new
					{
						a.DistrictId,
						a.DistricName,
					})
					.Select(a => new MonoApplicationReportDto
					{
						DistrictId = a.Key.DistrictId,
						DistricName = a.Key.DistricName,

						ReviewApplications = new ReviewApplications
						{
							Count = a.Sum(b => b.ReviewApplications.Count),
							TotalAmount = a.Sum(b => b.ReviewApplications.TotalAmount),
							TotalCost = a.Sum(b => b.ReviewApplications.TotalCost)
						},

						AskApplications = new AskApplications
						{
							AcceptApplicaions = new AcceptApplicaions
							{
								Count = a.Sum(b => b.AskApplications.AcceptApplicaions.Count),
								TotalAmount = a.Sum(b => b.AskApplications.AcceptApplicaions.TotalAmount),
								TotalCost = a.Sum(b => b.AskApplications.AcceptApplicaions.TotalCost)
							 	//SubsidyAmount = a.Sum(b => b.AskApplications.AcceptApplicaions.SubsidyAmount)

							},
							RejectApplicaions = new RejectApplicaions
							{
								Count = a.Sum(b => b.AskApplications.RejectApplicaions.Count),
								TotalAmount = a.Sum(b => b.AskApplications.RejectApplicaions.TotalAmount),
								TotalCost = a.Sum(b => b.AskApplications.RejectApplicaions.TotalCost)
							}
						},
						AllApplicaions = new AllApplicaions
						{
							Count = a.Sum(b =>
								b.ReviewApplications.Count +
								b.AskApplications.AcceptApplicaions.Count +
								b.AskApplications.RejectApplicaions.Count),

							TotalAmount = a.Sum(b =>
								b.ReviewApplications.TotalAmount +
								b.AskApplications.AcceptApplicaions.TotalAmount +
								b.AskApplications.RejectApplicaions.TotalAmount),
							TotalCost = a.Sum(b =>
								b.ReviewApplications.TotalCost +
								b.AskApplications.AcceptApplicaions.TotalCost +
								b.AskApplications.RejectApplicaions.TotalCost)
						}

					}).ToList();


				var districts = _unitOfWork.DistrictRepository.AllAsQueryable
					.Include(a => a.Region).ThenInclude(a => a.Translates)
					.Include(a => a.Translates)
					.IsActive()
					.Where(a => !filter.RegionId.HasValue || filter.RegionId == a.RegionId
						&& !filter.DistrictId.HasValue || filter.DistrictId == a.Id
					)
					.ToDictionary(
						a => a.Id,
						a => new
						{
							RegionId = a.RegionId,
							Region =  a.Region.FullName,
							District =  a.FullName,
							DistrictOrderCode = a.OrderCode,
							AllApplicaions = new AllApplicaions
							{
								Count = 0,
								TotalAmount = 0,
								TotalCost = 0
							},
							ReviewApplications = new ReviewApplications
							{
								Count = 0,
								TotalAmount = 0,
								TotalCost = 0
							},
							AskApplications = new AskApplications
							{
								AcceptApplicaions = new AcceptApplicaions
								{
									Count = 0,
									TotalAmount = 0,
									TotalCost = 0,
									SubsidyAmount = 0
								},
								RejectApplicaions = new RejectApplicaions
								{
									Count = 0,
									TotalAmount = 0,
									TotalCost = 0
								}
							}
						}
					);

				foreach (var district in districts)
				{
					if (monoApplications.Select(a => a.DistrictId).Contains(district.Key))
						continue;

					monoApplications.Add(new MonoApplicationReportDto
					{
						RegionName = district.Value.Region,
						RegionId = district.Value.RegionId,
						DistricName = district.Value.District,
						OrderCode = district.Value.DistrictOrderCode,
						DistrictId = district.Key
					});
				}

				monoApplications = monoApplications.OrderBy(a => a.OrderCode).ToList();
			}
			else if(filter.RegionId is not null && filter.DistrictId is not null)
			{
					monoApplications = _unitOfWork.Context.Set<MonoApplication>().
					Include(w => w.Application).ThenInclude(q => q.Contractor).
					Include(w => w.MonoApplicationBandlikResults).
					Where(y => y.Application.RegionId == filter.RegionId && y.Application.DistrictId == filter.DistrictId).
					SortFilter(filter).
					AsEnumerable().
					Select(p => 
					{

						  // Review Applications
						var reviewCount = p.StatusId == StatusIdConst.WAITING ? 1 : 0;
						var reviewTotalAmount = reviewCount > 0 ? p.TotalAmount : 0;
						var reviewTotalCost = reviewCount > 0 ? p.TotalCost : 0;

						// Accepted Applications
						var acceptCount = p.StatusId == StatusIdConst.ACCEPTED ? 1 : 0;
						var acceptTotalAmount = acceptCount > 0 ? p.TotalAmount : 0;
						var acceptTotalCost = acceptCount > 0 ? p.TotalCost : 0;
						var subsidyAmount = p.MonoApplicationBandlikResults.Any()
							? decimal.Parse(p.MonoApplicationBandlikResults.FirstOrDefault()?.SubsidyAmount ?? "0")
							: 0;

						// Rejected Applications
						var rejectCount = p.StatusId == StatusIdConst.REJECTED ? 1 : 0;
						var rejectTotalAmount = rejectCount > 0 ? p.TotalAmount : 0;
						var rejectTotalCost = rejectCount > 0 ? p.TotalCost : 0;

						// All Applications
						var totalCount = reviewCount + acceptCount + rejectCount;
						var totalAmount = reviewTotalAmount + acceptTotalAmount + rejectTotalAmount;
						var totalCost = reviewTotalCost + acceptTotalCost + rejectTotalCost;

						return new MonoApplicationReportDto
						{
							Id = p.Id,
							ContractorName = p.Application?.Contractor?.FullName ?? " ",
							ReviewApplications = new ReviewApplications
							{
								Count = reviewCount,
								TotalAmount = reviewTotalAmount,
								TotalCost = reviewTotalCost
							},
							AskApplications = new AskApplications
							{
								AcceptApplicaions = new AcceptApplicaions
								{
									Count = acceptCount,
									TotalAmount = acceptTotalAmount,
									TotalCost = acceptTotalCost,
									SubsidyAmount = subsidyAmount
								},
								RejectApplicaions = new RejectApplicaions
								{
									Count = rejectCount,
									TotalAmount = rejectTotalAmount,
									TotalCost = rejectTotalCost
								}
							},
							AllApplicaions = new AllApplicaions
							{
								Count = totalCount,
								TotalAmount = totalAmount,
								TotalCost = totalCost
							}
						};
					

					}).ToList();

			
			}

			return monoApplications;
		}
        //static int CountBusinessDays(DateTime? end, DateTime? start)
        //{
        //    if(end == null || start == null)
        //    {
        //        return 0;
        //    }

        //    int totalBusinessDays = 0;
        //    for (DateTime date = start.Value; date <= end.Value; date = date.AddDays(1))
        //    {
        //        if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
        //        {
        //            totalBusinessDays++;
        //        }
        //    }
        //    return totalBusinessDays;
        //}
        public List<PrtnEmployeeJobReportDto> GetPrtnEmployeeJobReport(PrtnEmployeeJobDtoFilter filter)
        {
			var result = new List<PrtnEmployeeJobReportDto>();

			var query = _unitOfWork.Context.Set<Application>()
				.Include(x => x.PrtnApplication)
				.Include(x => x.PrtnContract).ThenInclude(x => x.PrtnCertificate)
				.Include(x => x.Contractor)
				.Where(x => x.ApplicationTypeId == ApplicationTypeIdConst.PARTNER &&
							x.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED);

            var previousMonth = DateTime.Today.AddMonths(-2);
            int prevYear = previousMonth.Year;
            int prevMonth = previousMonth.Month;

            var actualEmployeeCounts = _unitOfWork.Context.Set<EmployeeCount>()
			   .Where(emp => emp.Year == prevYear && emp.Month == prevMonth)
			   .GroupBy(emp => emp.Tin)
			   .Select(emp => new
			   {
				   Inn = emp.Key,
				   EmployeeCount = emp.Sum(a => a.MonthlyNumberEmployees)
			   })
			   .ToDictionary(emp => emp.Inn, emp => emp.EmployeeCount);

            var untilFounded = _unitOfWork.Context.Set<EmployeeCount>()
                .Where(emp => emp.Year == CommonConst._2023 && emp.Month == CommonConst.MAY)
                .GroupBy(emp => emp.Tin)
				.Select(emp => new
				{
					Inn = emp.Key,
					EmployeeCount = emp.Sum(a => a.MonthlyNumberEmployees)
				})
				.ToDictionary(emp => emp.Inn, emp => emp.EmployeeCount);

            if (filter.PrtnContractTypeId.HasValue)
				query = query.Where(x => x.PrtnApplication.PrtnContractTypeId == filter.PrtnContractTypeId);
            if (filter.StartDate.HasValue)
                query = query.Where(x => x.DocOn >= filter.StartDate);
            if (filter.EndDate.HasValue)
                query = query.Where(x => x.DocOn <= filter.EndDate);
            if (filter.RegionId.HasValue)
                query = query.Where(x => x.RegionId == filter.RegionId);
            if (filter.DistrictId.HasValue)
                query = query.Where(x => x.DistrictId == filter.DistrictId);
            if (filter.ContractorId.HasValue)
                query = query.Where(x => x.ContractorId == filter.ContractorId);

			var currenTime = DateTime.Now;

            result = query.Select(x => new PrtnEmployeeJobReportDto
				{
					PrtnContractTypeId = filter.PrtnContractTypeId.HasValue ? x.PrtnApplication.PrtnContractTypeId : 0,
					PrtnContractType = filter.PrtnContractTypeId.HasValue ? (x.PrtnApplication.PrtnContractType.Translates.AsQueryable()
						.FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.PrtnApplication.PrtnContractType.FullName) : "",

					RegionId = filter.ByRegion ? (x.PrtnApplication.ChooseLocation ? x.PrtnApplication.ChoosedRegionId : x.RegionId) : null,
					RegionOrderCode = filter.ByRegion ? (x.PrtnApplication.ChooseLocation ? x.PrtnApplication.ChoosedRegion.OrderCode : x.Region.OrderCode) : null,
					Region = filter.ByRegion ? (x.PrtnApplication.ChooseLocation ? (x.PrtnApplication.ChoosedRegion.Translates.AsQueryable()
							.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.PrtnApplication.ChoosedRegion.FullName)
						: (x.Region.Translates.AsQueryable()
							.FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.Region.FullName))
						: null,

					DistrictId = filter.ByDistrict ? (x.PrtnApplication.ChooseLocation ? x.PrtnApplication.ChoosedDistrictId : x.DistrictId) : null,
					District = filter.ByDistrict
						? (x.PrtnApplication.ChooseLocation
							? (x.PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
								.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.PrtnApplication.ChoosedDistrict.FullName)
							: (x.District.Translates.AsQueryable()
								.FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.District.FullName))
						: null,

					ContractorId = filter.ByContractor ? x.ContractorId : null,
					Contractor = filter.ByContractor ? x.Contractor.FullName : null,
					ContractorInn = filter.ByContractor ? x.Contractor.Inn : null,
					ContractorPhoneNumber = filter.ByContractor ? x.Contractor.PhoneNumber : null,

					PrtnCertificateCount = x.PrtnContract.PrtnCertificate != null && x.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED ? 1 : 0,
					TotalPlannedJobs = x.StatusId == StatusIdConst.ACCEPTED ? x.PrtnApplication.NewVacanciesCount : 0,
					
					TodayPlan = x.PrtnApplication.Graphs.Where(a => a.YearIn == currenTime.Year).Sum(a => a.NewVacanciesCount),
					ActualEmployeesCount = actualEmployeeCounts.ContainsKey(x.Contractor.Inn) ? actualEmployeeCounts[x.Contractor.Inn] : 0,
					InitialEmployeesCount = untilFounded.ContainsKey(x.Contractor.Inn) ? untilFounded[x.Contractor.Inn] : 0,
					TotalEmployeesCount = (untilFounded.ContainsKey(x.Contractor.Inn) ? untilFounded[x.Contractor.Inn] : 0) + (actualEmployeeCounts.ContainsKey(x.Contractor.Inn) ? actualEmployeeCounts[x.Contractor.Inn] : 0),

					PercentageOfCompletedPlan = x.PrtnApplication.Graphs.Where(a => a.YearIn == currenTime.Year).Sum(a => a.NewVacanciesCount) > 0
						? ((actualEmployeeCounts.ContainsKey(x.Contractor.Inn) ? actualEmployeeCounts[x.Contractor.Inn] : 0) * 100M) / x.PrtnApplication.Graphs.Where(a => a.YearIn == currenTime.Year).Sum(a => a.NewVacanciesCount)
						: 0,

					PercentageGrowth = untilFounded.ContainsKey(x.Contractor.Inn) && untilFounded[x.Contractor.Inn] > 0
						? ((((untilFounded.ContainsKey(x.Contractor.Inn) ? untilFounded[x.Contractor.Inn] : 0) + (actualEmployeeCounts.ContainsKey(x.Contractor.Inn) ? actualEmployeeCounts[x.Contractor.Inn] : 0) - untilFounded[x.Contractor.Inn]) * 100M) / untilFounded[x.Contractor.Inn])
						: 0
						})
					.AsEnumerable()
					.GroupBy(x => new
					{
						x.PrtnContractTypeId,
						x.PrtnContractType,
						x.DistrictId,
						x.District,
						x.RegionId,
						x.RegionOrderCode,
						x.Region,
						x.ContractorId,
						x.Contractor,
						x.ContractorInn,
						x.ContractorPhoneNumber
					})
					.Select(x => new PrtnEmployeeJobReportDto
					{
						PrtnContractTypeId = x.Key.PrtnContractTypeId,
						PrtnContractType = x.Key.PrtnContractType,
						DistrictId = x.Key.DistrictId,
						District = x.Key.District,
						RegionId = x.Key.RegionId,
						RegionOrderCode = x.Key.RegionOrderCode,
						Region = x.Key.Region,
						ContractorId = x.Key.ContractorId,
						Contractor = x.Key.Contractor,
						ContractorInn = x.Key.ContractorInn,
						ContractorPhoneNumber = x.Key.ContractorPhoneNumber,
						PrtnCertificateCount = x.Sum(y => y.PrtnCertificateCount),
						TotalPlannedJobs = x.Sum(y => y.TotalPlannedJobs),
						TodayPlan = x.Sum(y => y.TodayPlan),
						InitialEmployeesCount = x.Sum(y => y.InitialEmployeesCount),
						ActualEmployeesCount = x.Sum(y => y.ActualEmployeesCount),
						TotalEmployeesCount = x.Sum(y => y.InitialEmployeesCount) + x.Sum(y => y.ActualEmployeesCount),
                        PercentageOfCompletedPlan = x.Sum(y => y.TotalPlannedJobs) > 0 ? (x.Sum(y => y.ActualEmployeesCount) * 100M) / x.Sum(y => y.TotalPlannedJobs) : 0,
                        PercentageGrowth = x.Sum(y => y.InitialEmployeesCount) > 0 ? ((x.Sum(y => y.TotalEmployeesCount) - x.Sum(y => y.InitialEmployeesCount)) * 100M) / x.Sum(y => y.InitialEmployeesCount) : 0
                    })
					.ToList();

			if (filter.ByRegion)
			{
				var regions = _unitOfWork.RegionRepository.AllAsQueryable
					.Include(a => a.Translates)
                    .IsActive()
                    .Where(a => !filter.RegionId.HasValue || filter.RegionId == a.Id)
                    .ToDictionary(
                        a => a.Id,
                        a => new
                        {
                            OrderCode = a.OrderCode,
                            FullName = a.FullName
                        }
                    );

                foreach (var region in regions)
				{
                    if (result.Select(a => a.RegionId).Contains(region.Key))
                        continue;

                    result.Add(new PrtnEmployeeJobReportDto
                    {
                        Region = region.Value.FullName,
                        RegionOrderCode = region.Value.OrderCode,
                        RegionId = region.Key
                    });

                    result = result.OrderBy(a => a.RegionOrderCode).ToList();
                }
            }

            if (filter.ByDistrict)
            {
                var districts = _unitOfWork.DistrictRepository.AllAsQueryable
                    .Include(a => a.Region).ThenInclude(a => a.Translates)
                    .Include(a => a.Translates)
                    .IsActive()
                    .Where(a => !filter.RegionId.HasValue || filter.RegionId == a.RegionId
                        && !filter.DistrictId.HasValue || filter.DistrictId == a.Id
                    )
                    .ToDictionary(
                        a => a.Id,
                        a => new
                        {
                            RegionId = a.RegionId,
                            Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                                ?.TranslateText
                            ?? a.Region.FullName,
                            District = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                                ?.TranslateText
                            ?? a.FullName,
                            DistrictOrderCode = a.OrderCode,
                        }
                    );

                foreach (var district in districts)
                {
                    if (result.Select(a => a.DistrictId).Contains(district.Key))
                        continue;

                    result.Add(new PrtnEmployeeJobReportDto
                    {
                        Region = district.Value.Region,
                        RegionId = district.Value.RegionId,
                        District = district.Value.District,
                        RegionOrderCode = district.Value.DistrictOrderCode,
                        DistrictId = district.Key
                    });
                }

                result = result.OrderBy(a => a.RegionOrderCode).ToList();
            }

            return result;
        }

    }
}