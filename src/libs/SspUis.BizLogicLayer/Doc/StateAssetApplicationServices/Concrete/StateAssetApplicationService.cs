using Humanizer;
using Newtonsoft.Json;
using SspUis.BizLogicLayer.BusinessmanAccountServices;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.DavAktiv;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.ApplicationServices
{
    public class StateAssetApplicationService : BaseEntityService<long, Application, StateAssetApplicationListDto, StateAssetApplicationDto, CreateStateAssetApplicationDlDto, CreateApplicationDlDto, UpdateStateAssetApplicationDlDto, UpdateApplicationDlDto, IApplicationRepository, StateAssetDocumentSortFilterOptions>, IStateAssetApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly INumberService _numberService;
        private readonly IDocumentChangeLogService _documentChangeLogService;
        private readonly IBaseReportService _baseReportService;
        private readonly SystemConf _systemConf;
        private readonly IDavAktivApplicationService _davAktivApplicationService;
        private readonly IApiRequestLogRepository _apiRequestLogRepository;

        public StateAssetApplicationService(IUnitOfWork unitOfWork,
            IAuthService authService,
            INumberService numberService,
           IBaseReportService baseReportService,
           SystemConf systemConf,
           IDocumentChangeLogService documentChangeLogService,
           IDavAktivApplicationService davAktivApplicationService,
           IApiRequestLogRepository apiRequestLogRepository)
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _numberService = numberService;
            _baseReportService = baseReportService;
            _systemConf = systemConf;
            _documentChangeLogService = documentChangeLogService;
            _davAktivApplicationService = davAktivApplicationService;
            _apiRequestLogRepository = apiRequestLogRepository;
        }

        protected override IQueryable<StateAssetApplicationListDto> SortFilter(IQueryable<StateAssetApplicationListDto> query, StateAssetDocumentSortFilterOptions options)
        {
            return base.SortFilter(query, options).SortFilter(options).Where(a => a.ApplicationTypeId == ApplicationTypeIdConst.STATE_ASSET);
        }

        public override PagedResult<StateAssetApplicationListDto> GetList(StateAssetDocumentSortFilterOptions options)
        {
            return base.GetList(options);
        }
		public int GetCount()
		{
            StateAssetDocumentSortFilterOptions options = new StateAssetDocumentSortFilterOptions();
			int result = base.GetList(options).Rows.Count();
            return result;
		}
		public override StateAssetApplicationDto Get()
        {
            //if (Repository.AllAsQueryable.Any(a => a.StatusId != StatusIdConst.REJECTED && a.StatusId != StatusIdConst.CANCELED && a.ApplicationTypeId == ApplicationTypeIdConst.STATE_ASSET))
            //{
            //    AddError("Ariza allaqachon yaratilgan / Заявка уже создана");
            //    return null;
            //}
            var certificate = _unitOfWork.PrtnCertificateRepository.AllAsQueryable.FirstOrDefault(a => StatusIdConst.FORMED == a.StatusId);
            if (certificate == null)
            {
                AddError("Sertifikat mavjud emas");
                return null;
            }

            var contractor = _unitOfWork.ContractorRepository.ById<ContractorDto>(_authService.Contractor.Id);
            var bUser = _unitOfWork.MyAccountRepository.ById<BusinessmanAccountUserDto>(_authService.User.Id);
            bUser.Contractor = contractor;
            return new StateAssetApplicationDto
            {
                Contractor = _authService.Contractor.FullName,
                ContractorInn = _authService.Contractor.Inn,
                DocOn = DateTime.Now.AsDateOnly(),
                ContractorPositionName = "Директор",
                DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_APPLICATION, 1).Item2,
                CanEdit = true,
                RegionId = contractor.RegionId,
                DistrictId = contractor.DistrictId,
                Region = contractor.Region,
                District = contractor.District,
                ContractorDirector = contractor.Director,
                ContractorAddress = contractor.Address,
                ContractorForm = "",//contractor.Form,
                Email = bUser.Email,
                PhoneNumber = bUser.UserName,
                PrtnCertificateId = certificate.Id,
                PrtnCertificateDocNumber = certificate.DocNumber,
                PrtnCertificateDocOn = certificate.DocOn
            };
        }

        public override StateAssetApplicationDto Get(long id)
        {
            var dto = Repository.ById<StateAssetApplicationDto>(id);

            if (dto == null)
                return null;

            //dto.CanAccept = StatusIdConst.CanApplicationApplyStatus(dto.StatusId, StatusIdConst.ACCEPTED)
            //                && _authService.HasPermission(ModuleCode.ApplicationAccept);
            //dto.CanReject = StatusIdConst.CanApplicationApplyStatus(dto.StatusId, StatusIdConst.REJECTED)
            //                && _authService.HasPermission(ModuleCode.ApplicationReject);
            //dto.CanSendForReview = StatusIdConst.CanApplicationApplyStatus(dto.StatusId, StatusIdConst.SENT_FOR_REVIEW);
            //dto.CanEdit = StatusIdConst.CanApplicationApplyStatus(dto.StatusId, StatusIdConst.MODIFIED);
            return dto;
        }

        public StateAssetApplicationDto Get(Guid id2)
        {
            var dto = Repository.DbSet.Select(a => new StateAssetApplicationDto
            {
                Id = a.Id,
                Id2 = a.Id2,
                StatusId = a.StatusId,
                Status = a.Status.FullName,
                Contractor = a.Contractor.FullName,
                ContractorDirector = a.Contractor.Director,
                ContractorInn = a.Contractor.Inn,
                ContractorAddress = a.Contractor.Address,
                ContractorForm = "", //a.Contractor.Form,
                Email = a.Contractor.BusinessmanUserInContractors.FirstOrDefault(b => b.BusinessmanUserId == a.CreatedUserId).BusinessmanUser.Email,
                PhoneNumber = a.Contractor.BusinessmanUserInContractors.FirstOrDefault(b => b.BusinessmanUserId == a.CreatedUserId).BusinessmanUser.UserName,
                ApplicationType = a.ApplicationType.FullName,
                Region = a.Region.Translates.FirstOrDefault(t => t.ColumnName == TranslateColumn.full_name.ToString() && t.LanguageId == LanguageIdConst.UZ_LATN).TranslateText ?? a.Region.FullName,
                District = a.District.Translates.FirstOrDefault(t => t.ColumnName == TranslateColumn.full_name.ToString() && t.LanguageId == LanguageIdConst.UZ_LATN).TranslateText ?? a.District.FullName,
            }).FirstOrDefault(a => a.Id2 == id2 && new int[] { StatusIdConst.SENT, StatusIdConst.ACCEPTED, StatusIdConst.REJECTED }.Contains(a.StatusId));
            if (dto == null)
                AddError("Not found!");
            return dto;
        }

        public override HaveId<long> Create(CreateStateAssetApplicationDlDto dto)
        {
            if (Repository.AllAsQueryable.Any(a => a.StatusId != StatusIdConst.REJECTED && a.StatusId != StatusIdConst.CANCELED && a.ApplicationTypeId == ApplicationTypeIdConst.STATE_ASSET && a.StateAssetApplication.AuctionDocNumber == dto.AuctionDocNumber))
            {
                AddError("Ariza allaqachon yaratilgan / Заявка уже создана");
                return null;
            }
            var certificate = _unitOfWork.PrtnCertificateRepository.AllAsQueryable.FirstOrDefault(a => StatusIdConst.FORMED == a.StatusId);
            if (certificate == null)
            {
                AddError("Sertifikat mavjud emas");
                return null;
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _unitOfWork.Context.Set<Contractor>().Lock(_authService.Contractor.Id);

                    var entity = Repository.Create(dto, ent =>
                    {
                    });
                    CombineStatuses(Repository);
                    if (HasErrors)
                        return null;
                    UnitOfWork.Save();

                    var res = CreateDocumentChangeLog(entity.Id);
                    if (IsValid)
                    {
                        transaction.Commit();
                        return HaveId.Create(entity.Id);
                    }
                }
                catch (Exception ex)
                {
                    AddError($"{ex.Message}: {ex.InnerException}");
                }
            }
            return null;
        }

        public override void Update(UpdateStateAssetApplicationDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var entity = Repository.Update(dto, ent =>
                {
                    if (!StatusIdConst.CanStateAssetApplicationApplyStatus(ent.StatusId, StatusIdConst.MODIFIED))
                        AddError("Имкони йўқ / Нет доступа");
                });
                CombineStatuses(Repository);
                if (IsValid)
                    UnitOfWork.Save();

                var res = CreateDocumentChangeLog(entity.Id);

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

        public bool CanCreate(string inn = null)
        {
            inn ??= _authService.Contractor.Inn;
            var dto = Repository.CrudServices.ReadManyNoTracked<StateAssetApplicationDto>()
                                            .Where(a => a.ContractorInn == inn
                                                     && a.ApplicationTypeId == ApplicationTypeIdConst.STATE_ASSET
                                                     && !new int[] { StatusIdConst.DELETED, StatusIdConst.REJECTED, StatusIdConst.CANCELED }.Contains(a.StatusId)).ToList();
            if (dto.Any())
                return false;
            return true;
        }

        public void Accept(AcceptStatusStateAssetApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<Application>().Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanStateAssetApplicationApplyStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                });
                CombineStatuses(Repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(dto.Id);
                if (IsValid && canCommit)
                    transaction.Commit();
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }

        public void Reject(RejectStatusStateAssetApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;

            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<Application>().Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplicationApplyStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                });

                CombineStatuses(Repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(id: dto.Id);

                if (IsValid && canCommit)
                    transaction.Commit();
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }

        public DavAktivApplicationResponseDto UpdateStateAssetStatus(UpdateStateAssetStatusStateAssetApplicationDto dto)
        {
            var application = _unitOfWork.Context.Set<Application>().FirstOrDefault(a => a.Id2 == dto.Id2);
            if (application == null)
            {
                return new DavAktivApplicationResponseDto
                {
                    Succes = false,
                    ErrorMsg = "Ariza topilmadi"
                };
            }
            switch (dto.StateAssetStatusId)
            {
                case StateAssetStatusIdConst.AGREED:
                    Accept(new AcceptStatusStateAssetApplicationDto
                    {
                        Id = application.Id,
                        Message = dto.Message
                    });
                    break;
                case StateAssetStatusIdConst.REJECTED:
                    Reject(new RejectStatusStateAssetApplicationDto
                    {
                        Id = application.Id,
                        Message = dto.Message
                    });
                    break;
                default:
                    return new DavAktivApplicationResponseDto
                    {
                        Succes = false,
                        ErrorMsg = "Dav. aktiv tizimidan noma'lum status qabul qilindi"
                    };
            }
            if (HasErrors)
                return new DavAktivApplicationResponseDto
                {
                    Succes = false,
                    ErrorMsg = GetAllErrors()
                };
            return new DavAktivApplicationResponseDto
            {
                Succes = true
            };
        }

        public void Cancel(CancelStatusStateAssetApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<Application>().Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanStateAssetApplicationApplyStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                });

                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(id: dto.Id);

                if (IsValid && canCommit)
                    transaction.Commit();
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }

        public async Task Send(SendStatusStateAssetApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<Application>().Lock(dto.Id);

                //Repository.UpdateStatus(dto, ent =>
                //{
                //    if (!StatusIdConst.CanStateAssetApplicationApplyStatus(ent.StatusId, dto.StatusId))
                //        AddError("Имкони йўқ / Нет доступа");
                //});


                var res = CreateDocumentChangeLog(id: dto.Id, userIp: dto.UserIp, userAgent: dto.UserAgent);

                if (!_systemConf.IsTest)
                    await SendToDavAktiv(dto.Id);
                if (IsValid && !HasErrors)
                {
                    Repository.UpdateStatus(dto, ent =>
                    {
                        if (!StatusIdConst.CanStateAssetApplicationApplyStatus(ent.StatusId, dto.StatusId))
                            AddError("Имкони йўқ / Нет доступа");
                    });
                }

                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                _unitOfWork.Save();

                if (IsValid && canCommit)
                    transaction.Commit();
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }

        public string GetAsHtml(StateAssetApplicationDto dto)
        {
            Dictionary<string, string> data
                = new Dictionary<string, string>()
                {
                {"${DocDate}",dto.DocOn.ToString(Constants.DATE_FORMAT)},
                {"${DocNumber}",dto.DocNumber},
                {"${ContractorInn}", dto.ContractorInn},
                {"${ContractorName}", dto.Contractor},
                {"${ContractorFrom}", dto.PhoneNumber},
                {"${ContractorDirector}", dto.ContractorDirector},
                {"${Region}", dto.Region},
                {"${District}", dto.District},
                {"${Address}", dto.ContractorAddress},
                {"${Email}",dto.Email},
                {"${PhoneNumber}", dto.PhoneNumber},
                {"${AuctionDocNumber}", dto.AuctionDocNumber},
                {"${AuctionDocOn}", dto.AuctionDocOn?.ToString(Constants.DATE_FORMAT)},
                {"${StateAssetName}", dto.StateAssetName }
                };
            if (dto.Id != 0)
                data.Add("${QrImage}", WEBASE.QRCode.QRCodeHelper.GenerateImageAsBase64($"{_systemConf.QrImagePrintPath}/api/stateassetapplication/getaspdf/{dto.Id2}?__lang=uz-latn"));

            string fileName = "state_asset_application.html";

            return _baseReportService.ReturnReadyHtmlAsString(filename: fileName,
                                                      data: data,
                                                      tableData: null);
        }

        public string GetAsHtml()
        {
            return GetAsHtml(Get());
        }

        public string GetAsHtml(long id)
        {
            return GetAsHtml(Get(id));
        }

        public string GetAsHtml(Guid id2)
        {
            var dto = Get(id2);
            if (HasErrors)
                return null;
            return GetAsHtml();
        }

        public byte[] GetAsPdf(Guid id2)
        {
            var html = GetAsHtml(id2);
            if (HasErrors)
                return null;
            return _baseReportService.ReturnReadyPdf(html);
        }

        public async Task SendToDavAktiv(long id)
        {
            var doc = Repository.ById<StateAssetApplicationDto>(id);

            var log = new CreateApiRequestLogDlDto
            {
                DocumentId = id,
                TableId = TableIdConst.DOC_STATE_ASSET_APPLICATION,
                UserId = (int)_authService.UserId,
                UserInfo = _authService.User.ToString() ?? "",
                RequestAt = DateTime.Now,
            };
            try
            {
                var davAltivApplication = new DavAktivApplicationDto
                {
                    Id2 = doc.Id2,
                    SendAt = DateTime.Now,
                    DocOn = doc.DocOn,
                    DocNumber = doc.DocNumber,
                    Inn = int.Parse(doc.ContractorInn),
                    Contractor = doc.Contractor,
                    RegionSoato = int.Parse(doc.RegionSoato),
                    Region = doc.Region,
                    DistrictSoato = int.Parse(doc.DistrictSoato),
                    District = doc.District,
                    Address = doc.ContractorAddress,
                    PrtnCertificateLink = $"{_systemConf.QrImagePrintPath}/PrtnCertificate/PrintCertificatePdf?Id2={doc.PrtnCertificateId2}&lang=uz-cyrl",
                    AuctionDocOn = doc.AuctionDocOn,
                    AuctionDocNumber = doc.AuctionDocNumber,
                    StateAssetName = doc.StateAssetName,
                    PrtnContractTypeId = doc.PrtnContractTypeId,
                    PrtnContractType = doc.PrtnContractType,
                    StateAssetStatus = doc.StateAssetStatusId,
                    PhoneNumber = doc.PhoneNumber,
                };
                (DavAktivApplicationResponseDto result, HttpResponseMessage response, string responseText, string url) = await _davAktivApplicationService.PostApplication(davAltivApplication);

                CombineStatuses(_davAktivApplicationService);

                log.ResponseAt = DateTime.Now;
                log.IsSuccess = response.IsSuccessStatusCode;
                log.RequestContent = JsonConvert.SerializeObject(davAltivApplication);
                log.ResponseContent = responseText;
                log.ResponseStatus = (int)response.StatusCode;
                log.RequestUrl = url;
            }
            catch (Exception ex)
            {
                log.Exception = $"{ex.Message}: {ex.InnerException}";
            }
            finally
            {
                _apiRequestLogRepository.Create(log);
                _unitOfWork.Save();
            }
        }
        public void SetModifiedStatus(ModifiedStatusStateAssetApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<Application>().Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanStateAssetApplicationApplyStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                });
                CombineStatuses(Repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(dto.Id);
                if (IsValid && canCommit)
                    transaction.Commit();
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }
	}

}
