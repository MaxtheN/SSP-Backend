using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using Aspose.Cells;
using DocumentFormat.OpenXml.Spreadsheet;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml.Style.XmlAccess;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.Doc.ApplicationServices;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.BizLogicLayer.ReportServices.Main;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Exapidata.Tax;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.EfClasses.Report;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.OnlineMahalla;
using SspUis.ServiceLayer.NumberServices;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.ApplicationServices
{
    public partial class ApplicationService
        : BaseEntityService<long, Application, ApplicationListDto, ApplicationDto, CreateApplicationDlDto, UpdateApplicationDlDto, IApplicationRepository, PrtnDocumentSortFilterOptions>
        , IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly INumberService _numberService;
        private readonly IDocumentChangeLogService _documentChangeLogService;
        private readonly IStorageService _storageService;
        private readonly ICultureHelper _cultureHelper;
        private readonly INotBudgetContractorRepository _notBudgetContractorRepository;
        private readonly IOnlineMahallaService _onlineMahallaService;
        private readonly ICrudServices _crudServices;
        private readonly SystemConf _systemConf;
        private readonly IContractorService _contractorService;
        private readonly IApiRequestLogRepository _apiRequestLogRepository;
        private readonly IStateAssetApplicationService _stateAssetApplicationService;


        public ApplicationService(IUnitOfWork unitOfWork,
            IAuthService authService,
            INumberService numberService,
            IDocumentChangeLogService documentChangeLogService,
            IStorageService storageService,
            ICultureHelper cultureHelper,
            INotBudgetContractorRepository notBudgetContractorRepository,
            IOnlineMahallaService onlineMahallaService,
            ICrudServices crudServices,
            SystemConf systemConf,
            IContractorService contractorService,
            IApiRequestLogRepository apiRequestLogRepository,
            IStateAssetApplicationService stateAssetApplicationService)
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _numberService = numberService;
            _cultureHelper = cultureHelper;
            _storageService = storageService;
            _documentChangeLogService = documentChangeLogService;
            _notBudgetContractorRepository = notBudgetContractorRepository;
            _onlineMahallaService = onlineMahallaService;
            _crudServices = crudServices;
            _systemConf = systemConf;
            _contractorService = contractorService;
            _apiRequestLogRepository = apiRequestLogRepository;
            _stateAssetApplicationService = stateAssetApplicationService;
        }

        public override WEBASE.Models.PagedResult<ApplicationListDto> GetList(PrtnDocumentSortFilterOptions options)
        {
            IQueryable<ApplicationListDto> query = GetListMethod(options);
            return query.AsPagedResult(options);
        }

        public IQueryable<ApplicationListDto> GetListMethod(PrtnDocumentSortFilterOptions options)
        {
            var query = Repository.ReadAsNoTracked<ApplicationListDto>()
                .Where(a => a.ApplicationTypeId == ApplicationTypeIdConst.PARTNER)
                .SortFilter(options);
            return query;
        }

		public int GetCount()
		{
			PrtnDocumentSortFilterOptions prtnDocumentSortFilterOptions = new PrtnDocumentSortFilterOptions();
            return GetListMethod(prtnDocumentSortFilterOptions).Count();
		}

		public SelectList<long> AsSelectList()
        {
            return Repository.ReadAsNoTracked<ApplicationListDto>().AsSelectList();
        }

        public void Revoke(long id)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                Repository.AllAsQueryable.Lock(id);

                var dto = new UpdateStatusApplicationDlDto { Id = id, StatusId = StatusIdConst.REVOKED };

                Repository.UpdateStatus(dto, ent =>
                {
                    /*if (!StatusIdConst.CanApplicationApplyStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");*/
                });

                CombineStatuses(Repository);

                if (HasErrors)
                    return;

                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(id);

                if (IsValid)
                    transaction.Commit();
            }
        }

        public override void Delete(long id)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                Repository.AllAsQueryable.Lock(id);
                var dto = new UpdateStatusApplicationDlDto { Id = id, StatusId = StatusIdConst.DELETED };

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplicationApplyStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                });
                CombineStatuses(Repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(id);
                if (IsValid)
                {
                    transaction.Commit();
                }
            }
        }

        private HaveId<long> CreateDocumentChangeLog(long id, string message = null, string userIp = null, string userAgent = null)
        {
            var entityDto = Repository.ById<ApplicationDto>(id, applyFilter: false);
            _documentChangeLogService.Create(
                dto: entityDto,
                tableId: TableIdConst.DOC_APPLICATION,
                organizationId: null,
                statusId: entityDto.StatusId,
                message: message,
                userIp: userIp,
                userAgent: userAgent);
            CombineStatuses(_documentChangeLogService);

            if (HasErrors)
                return null;

            return HaveId.Create(id);
        }

        public ApplicationVacanсiesDto GetApplicationVacancies()
        {
            var contractor = _authService.Contractor;
            if(contractor is null)
            {
                AddError("Foydalanuvchi topilmadi");
                return null;
            }

            var userTin = _authService.Contractor.Inn;
            if (userTin.IsNullOrEmpty())
            {
                AddError("Inn topilmadi");
                return null;
            }

            var UserId = _authService.Contractor.Id;
            var application = _unitOfWork.Context.Set<PrtnApplication>().FirstOrDefault(a => a.Application.ContractorId == UserId);
            if(application == null)
            {
				return new ApplicationVacanсiesDto()
				{
					NewVacanciesCount = 0,
					ReportVacanciesCount = 0
				};
			}
            var data = _unitOfWork.Context.Set<EmployeeCount>().Where(a => a.Tin == userTin).ToArray();
          
            if (data == null || data.Length < 6 )
            {

                return new ApplicationVacanсiesDto()
                {
                    NewVacanciesCount = 0,
                    ReportVacanciesCount = 0
                };
            }
        
            ApplicationVacanсiesDto applicationVacanсiesDto = new ApplicationVacanсiesDto();
            decimal newEmployees = 0;
            int june = CommonConst.IYUN - 1;
            var oldEmployees = data[june - 1].MonthlyNumberEmployees;
            applicationVacanсiesDto.NewVacanciesCount = application.NewVacanciesCount;
            for (int i = june; i < data.Length; i++)
            {
                var tax = data[i].MonthlyNumberEmployees - oldEmployees;
                oldEmployees = data[i].MonthlyNumberEmployees;

                if (tax < 0)
                    tax = 0;

                newEmployees += tax;
            }
            applicationVacanсiesDto.ReportVacanciesCount = newEmployees;
            return applicationVacanсiesDto;
        }

        public List<AvailableBenefits> GetAvailableBenefits()
        {
            List<AvailableBenefits> Benefits = new List<AvailableBenefits>();
            var contractorInn = _authService.Contractor.Inn;

            var bankData = _unitOfWork.Context.
                Set<DataLayer.EfClasses.Exapidata.BankCredit.BankCreditApplication>().
                Where(a => a.Tin == contractorInn).  //200047118
                Include(a => a.Tables).FirstOrDefault();
            int _statusId = 0;
 

       
            
            if (bankData != null)
            {
				if (bankData.Tables.Count() != 0)
				{
					switch (bankData.Tables.LastOrDefault().DocStatus)
					{
						case "CANCELED": _statusId = StatusIdConst.CANCELED; break;
						case "REJECTED": _statusId = StatusIdConst.REJECTED; break;
						case "APPROVED": _statusId = StatusIdConst.APPROVED; break;
						case "SUBMITTED": _statusId = StatusIdConst.CREATED; break;
						default: break;
					}
				}

				AvailableBenefits benefitBank = new AvailableBenefits()
                {
                    OrganizationName = "Bank",
                    Status = bankData.Tables.Count() == 0 ? "Imtiyozdan foydalanilmagan" : bankData.Tables.LastOrDefault().DocStatus,
                    StatusId = bankData.Tables.Count() == 0 ? 0 : _statusId,
					Sum = bankData.Tables.Count() == 0 ? null : bankData.Tables.LastOrDefault().IssuanceSum,
                };

                Benefits.Add(benefitBank);
            }

            else
            {
                AvailableBenefits benefitBank = new AvailableBenefits()
                {
                    OrganizationName = "Bank",
                    Status = "Imtiyozdan foydalanilmagan",
                    StatusId = 0,
                    Sum = null,
                };

                Benefits.Add(benefitBank);
            }


            var davlatActivlariData = _stateAssetApplicationService.GetList(new StateAssetDocumentSortFilterOptions()).Rows.OrderByDescending(a => a.Id).FirstOrDefault(a => a.ContractorInn == contractorInn);
            if (davlatActivlariData != null)
            {
                AvailableBenefits benefitAtivlar = new AvailableBenefits()
                {
                    OrganizationName = "Davlat aktivlari",
                    Status = davlatActivlariData.Status,
                    StatusId = davlatActivlariData.StatusId,
					Sum = null

                };

                Benefits.Add(benefitAtivlar);
            }

            else
            {
                AvailableBenefits benefitAtivlar = new AvailableBenefits()
                {
                    OrganizationName = "Davlat aktivlari",
                    Status = "Imtiyozdan foydalanilmagan",
                    StatusId = 0,
                    Sum = null,
                };

                Benefits.Add(benefitAtivlar);
            }

            var dataTax = _unitOfWork.Context.
                GetTaxCreditReport(
                _authService.Contractor.Id,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null).FirstOrDefault();


            AvailableBenefits benefitTax = new AvailableBenefits()
            {
                OrganizationName = "Soliq",
                Status = dataTax.CertificateCount != 0 ? "Imtiyozdan foydalanilgan" : "Imtiyozdan foydalanilmagan",
                StatusId = dataTax.CertificateCount != 0 ? StatusIdConst.CREATED : 0,
				Sum = null
            };

            Benefits.Add(benefitTax);

            var customs = _unitOfWork.Context.BojxonaImtiyozReportByContractor
                (
                    _authService.Contractor.CountryId,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                ).FirstOrDefault();
            //Rows.Where(a=>a.ContractorId == 3223 && a.StatusId != StatusIdConst.DELETED && a.ApplicationTypeId == ApplicationTypeIdConst.STATE_ASSET);

            AvailableBenefits customsTax = new AvailableBenefits()
            {
                OrganizationName = "Bojxona",
                Status = customs.CertificateCount != 0 ? (customs.DevCount == 0 ? "Rad etilgan" : "Imtiyozdan foydalanilgan") : "Imtiyozdan foydalanilmagan",
                StatusId = customs.CertificateCount != 0 ? (customs.DevCount == 0 ? StatusIdConst.REJECTED : StatusIdConst.CREATED) : 0,
                Sum = customs.Sum != 0 ? customs.Sum : null
            };

            Benefits.Add(customsTax);

            return Benefits;
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

        public List<ApplicationNotificationDto> GetApplicationNotification()
        {
            try
            {
                List<ApplicationNotificationDto> applicationNotificationDtos = new List<ApplicationNotificationDto>();
                ContractorAuthModel contractor = _authService.Contractor;
                var prtnContract = _unitOfWork.Context.Set<PrtnContract>().Include(a => a.Status).ThenInclude(a => a.Translates).Where(a => a.ContractorId == contractor.Id).ToList();
				List<PrtnApplication> application = _unitOfWork.Context.Set<PrtnApplication>().Include(a => a.Application).ThenInclude(a => a.Status).ThenInclude(a => a.Translates).Where(a => a.Application.ContractorId == contractor.Id && a.Application.ApplicationTypeId == ApplicationTypeIdConst.PARTNER).ToList();
				List<MemshipApplication> memshipApplication = _unitOfWork.Context.Set<MemshipApplication>().Include(a => a.Application).ThenInclude(a => a.Status).Where(a => a.Application.ContractorId == contractor.Id && a.Application.StatusId != StatusIdConst.DELETED).ToList();
                var memshipContract = _unitOfWork.Context.Set<MemshipContract>().Include(a => a.Status).Include(a => a.Application).Where(a => a.Application.ContractorId == contractor.Id && a.StatusId != StatusIdConst.DELETED).ToList();
                List<MemshipCertificate> memshipCertificate = _unitOfWork.Context.Set<MemshipCertificate>().Include(a => a.Status).Where(a => a.ContractorId == contractor.Id && a.StatusId != StatusIdConst.DELETED).ToList();
                if (prtnContract == null)
                {
                    foreach (var item in application) 
                    {
						if (item?.IsRead == false)
						{
							ApplicationNotificationDto applicationNotificationDto = new ApplicationNotificationDto();
							string baseUrl = _systemConf.IsTest ? "http://sspmyuis.apptest.uz/" : "https://my.chamber.uz/";
							var path = "partnership/application";
							string url = Path.Combine(baseUrl, path);
							var status = item.Application.Status.Translates.AsQueryable().FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? item.Application.Status.FullName;
							var langId = ServiceProvider.CultureHelper.CurrentCulture.Id;
							applicationNotificationDto.Message = $"Sizning arizangiz : {status}";
							applicationNotificationDto.Url = url;
							applicationNotificationDto.Type = "application";
                            applicationNotificationDto.Id = item.Id;

							applicationNotificationDtos.Add(applicationNotificationDto);
						}
					}
                   
                }
                else
                {
                    foreach (var item in prtnContract)
                    {
						if (item?.IsRead == false)
						{
							ApplicationNotificationDto applicationNotificationDto = new ApplicationNotificationDto();
							string baseUrl = _systemConf.IsTest ? "http://sspmyuis.apptest.uz/" : "https://my.chamber.uz/";
							var path = "partnership/contract";
							string url = Path.Combine(baseUrl, path);
							var status = item.Status.Translates.AsQueryable().FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? item.Status.FullName;
							applicationNotificationDto.Message = $"Sizning shartnomangiz : {status}";
							applicationNotificationDto.Url = url;
							applicationNotificationDto.Type = "contract";
							applicationNotificationDto.Id = item.Id;
							applicationNotificationDtos.Add(applicationNotificationDto);
						}
					}
                   
                }
                foreach (var item in memshipApplication)
                {
					if (item?.IsRead == false)
					{
						ApplicationNotificationDto applicationNotificationDto = new ApplicationNotificationDto();
						string baseUrl = _systemConf.IsTest ? "http://sspmyuis.apptest.uz/" : "https://my.chamber.uz/";
						var path = "memship/application";
						string url = Path.Combine(baseUrl, path);
						var status = item.Application.Status.Translates.AsQueryable().FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? item.Application.Status.FullName;
						var langId = ServiceProvider.CultureHelper.CurrentCulture.Id;
						applicationNotificationDto.Message = $"Sizning a'zolik arizangiz : {status}";
						applicationNotificationDto.Url = url;
						applicationNotificationDto.Type = "memshipapplication";
						applicationNotificationDto.Id = item.Id;
						applicationNotificationDtos.Add(applicationNotificationDto);
					}
				}

               
                if (memshipApplication != null)
                {
					foreach (MemshipApplication item in memshipApplication)
					{
						if (item?.IsRead == false)
						{
							ApplicationNotificationDto applicationNotificationDto = new ApplicationNotificationDto();
							string baseUrl = _systemConf.IsTest ? "http://sspmyuis.apptest.uz/" : "https://my.chamber.uz/";
							var path = "memship/contract";
							string url = Path.Combine(baseUrl, path);
							var status = item.Application.Status.Translates.AsQueryable().FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? item.Application.Status.FullName;
							applicationNotificationDto.Message = $"Sizning a'zolik shartnomangiz: {status}";
							applicationNotificationDto.Url = url;
							applicationNotificationDto.Type = "MemshipContract";
							applicationNotificationDto.Id = item.Id;
							applicationNotificationDtos.Add(applicationNotificationDto);
						}
					}

                    foreach (var item in memshipCertificate)
                    {
						if (item?.IsRead == false)
						{
							ApplicationNotificationDto applicationNotificationDto = new ApplicationNotificationDto();
							string baseUrl = _systemConf.IsTest ? "http://sspmyuis.apptest.uz/" : "https://my.chamber.uz/";
							var path = "memship/certificate";
							string url = Path.Combine(baseUrl, path);
							var status = item.Status.Translates.AsQueryable().FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? item.Status.FullName;
							applicationNotificationDto.Message = $"A'zolik guvohnomasi: {status}";
							applicationNotificationDto.Url = url;
							applicationNotificationDto.Type = "MemshipCertificate";
							applicationNotificationDto.Id = item.Id;
							applicationNotificationDtos.Add(applicationNotificationDto);
						}
					}
                   
                }

                foreach (var item in memshipCertificate)
                {
					if (item?.ExpireOn < DateOnly.FromDateTime(DateTime.Today) && item?.IsRead == false)
					{
						ApplicationNotificationDto applicationNotificationDto = new ApplicationNotificationDto();
						string baseUrl = _systemConf.IsTest ? "http://sspmyuis.apptest.uz/" : "https://my.chamber.uz/";
						var path = "memship/certificate";
						string url = Path.Combine(baseUrl, path);
						applicationNotificationDto.Message = $"A'zolik guvohnomasi muddati tugadi: {item.ExpireOn}";
						applicationNotificationDto.Url = url;
						applicationNotificationDto.Type = "MemshipCertificate";
						applicationNotificationDto.Id = item.Id;
						applicationNotificationDtos.Add(applicationNotificationDto);
					}
				}
               

                return applicationNotificationDtos;
            }
            catch (Exception)
            {

                AddError("Xatolik yuzaga keldi");
                return new List<ApplicationNotificationDto>();
            }
        }

        public bool IsRead(string typeName, long Id)
        {

            try
            {
              ContractorAuthModel contractor = _authService.Contractor;
                switch (typeName)
                {
                    case "application":
                        {
                            var application = _unitOfWork.Context.Set<PrtnApplication>().
                                      Include(a => a.Application).
                                      ThenInclude(a => a.Status).
                                      ThenInclude(a => a.Translates).
                                      FirstOrDefault(a => a.Id == Id);

                            application.IsRead = true;
                            _unitOfWork.Save();
                            return true;
                        }
                    case "contract":
                        {
                            var prtnContract = _unitOfWork.Context.Set<PrtnContract>().
                                     Include(a => a.Status).
                                     ThenInclude(a => a.Translates).
                                     FirstOrDefault(a => a.Id == Id);

                            prtnContract.IsRead = true;
                            _unitOfWork.Save();
                            return true;
                        }
                    case "memshipapplication":
                        {
                            var memshipApplication = _unitOfWork.Context.Set<MemshipApplication>().
                                      Include(a => a.Application).ThenInclude(a => a.Status).
                                      FirstOrDefault(a => a.Id == Id);
                            memshipApplication.IsRead = true;
                            _unitOfWork.Save();
                            return true;
                        }
                    case "MemshipContract":
                        {
                            var memshipContract = _unitOfWork.Context.Set<MemshipContract>().
                                    Include(a => a.Status).Include(a => a.Application).
                                    FirstOrDefault(a => a.Id == Id);
                            memshipContract.IsRead = true;
                            _unitOfWork.Save();
                            return true;
                        }
                    case "MemshipCertificate": //MemshipCertificate
						{
                            var memshipCertificate = _unitOfWork.Context.Set<MemshipCertificate>().
                                Include(a => a.Status).FirstOrDefault(a => a.Id == Id);

                            memshipCertificate.IsRead = true;
                            _unitOfWork.Save();
                            AddError("KontractorId: " + _authService.Contractor.Id);
                            return true;
                        }
                    default: return false;
                }
            }
            catch (Exception e)
            {
                AddError($"Xatolik yuzaga keldi | ({e.Message}) | ({e.InnerException})");
                return false;
            }
      
        }

        public MemshipPaymentsInfo GetMemshipPayments()
        {
            try
            {
                MemshipPaymentsInfo memshipPaymentsInfo = null;
                var now = DateOnly.FromDateTime(DateTime.Now);
                var contractor = _authService.Contractor;
                var data = _unitOfWork.Context.Set<MemshipCertificate>().
                    Include(a => a.MemshipContract).
                    FirstOrDefault(a => a.ContractorId == contractor.Id);

                if (data != null && (data.MemshipContract.MemshipContractTypeId == 2 || data.ExpireOn < now))
                {
                    memshipPaymentsInfo = new MemshipPaymentsInfo()
                    {
                        Payment = "0",
                        PaymentPayment = "0"
                    };

                    return memshipPaymentsInfo;
                }

                var dateForNew = new DateOnly(2023, 10, 1);
                if (data != null && data.MemshipContract.DocOn >= dateForNew)
                {
                    var minBHM = _unitOfWork.Context.Set<FixedMinimumValue>().OrderByDescending(a => a.DateOn)
                        .FirstOrDefault(a => a.MinimumValueTypeId == MinimumValueTypeIdConst.BRV &&
                        a.DateOn < data.DocOn).FixedValue;

                    var BXM = data.MemshipContract?.BaseFixedMinimumValue ?? 0 * minBHM;

                    var orderPayments = _unitOfWork.Context.Set<MemshipPaymentOrder>().
                        Where(a => a.ContractorId == contractor.Id)?.Sum(a => a.Amount) ?? 0;

                    var dif = BXM - orderPayments;
                    string debt = dif > 0 ? $"-{dif}" : "0";
                    memshipPaymentsInfo = new MemshipPaymentsInfo()
                    {
                        Payment = BXM.ToString(),
                        PaymentPayment = debt
                    };
                                
                    return memshipPaymentsInfo;

                }

                return new MemshipPaymentsInfo()
                {
                    Payment = "0",
                    PaymentPayment = "0",
                };
            }
            catch (Exception e)
            {
                AddError($"Xatolik yuzaga keldi | ({e.Message}) | ({e.InnerException})");
                return null;
            }
         
        }

	}
}