using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.DistrictServices;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.RegionServices;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Corruption;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.DocxToPdf;
using SspUis.ServiceLayer.NumberServices;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.OfficeTools;
using WEBASE.OfficeTools.Handlers;
using WEBASE.QRCode;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.JoinAntiCorruptionApplicationServices
{
    public class JoinAntiCorruptionApplicationService : BaseApplicationService
        <JoinAntiCorruptionApplication,
        JoinAntiCorruptionApplicationListDto,
        JoinAntiCorruptionApplicationDto,
        CreateJoinAntiCorruptionApplicationDlDto,
        UpdateJoinAntiCorruptionApplicationDlDto,
        IJoinAntiCorruptionApplicationRepository,
        JoinAntiCorruptionApplicationSortFilterOptions>, IJoinAntiCorruptionApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IEImzoService _eImzoService;
        private readonly INumberService _numberService;
        private readonly IDocumentChangeLogService _documentChangeLogService;
        private readonly IBaseReportService _baseReportService;
        private readonly SystemConf _systemConf;
        private readonly IContractorService _contractorService;
        private readonly IStorageService _storageService;
        private readonly ICultureHelper _cultureHelper;
        private readonly IConvertService _pdfConverter;

        public JoinAntiCorruptionApplicationService(ICultureHelper cultureHelper,
            IUnitOfWork unitOfWork,
            IAuthService authService,
            IEImzoService eImzoService,
            INumberService numberService,
            IDocumentChangeLogService documentChangeLogService,
            IBaseReportService baseReportService,
            SystemConf systemConf,
            IContractorService contractorService,
            IStorageService storageService,
            IConvertService pdfConverter)
            : base(unitOfWork, documentChangeLogService)
        {
            _cultureHelper = cultureHelper;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _eImzoService = eImzoService;
            _numberService = numberService;
            _documentChangeLogService = documentChangeLogService;
            _baseReportService = baseReportService;
            _systemConf = systemConf;
            _contractorService = contractorService;
            _storageService = storageService;
            this._pdfConverter = pdfConverter;
        }


        public PagedResult<JoinAntiCorruptionApplicationListDto> GetList(JoinAntiCorruptionApplicationSortFilterOptions options)
        {
            var data = Repository.ReadAsNoTracked<JoinAntiCorruptionApplicationListDto>()
                            .SortFilter(options).ToList().OrderByDescending(a=>a.CertificateId).DistinctBy(a=>a.Id).AsQueryable()
                            .AsPagedResult(options);

            return data;
        }


        public JoinAntiCorruptionApplicationDto Get()
        {
            var region = _unitOfWork.RegionRepository.ById<RegionListDto>(_authService.Contractor.RegionId);
            var district = _unitOfWork.DistrictRepository.ById<DistrictListDto>(_authService.Contractor.DistrictId);
            var contractor = _unitOfWork.ContractorRepository.ById<ContractorListDto>(_authService.Contractor.Id);
            return new JoinAntiCorruptionApplicationDto
            {
                Application = new()
                {
                    Contractor = _authService.Contractor.FullName,
                    ContractorInn = _authService.Contractor.Pinfl ?? _authService.Contractor.Inn,
                    ContractorId = _authService.Contractor.Id,
                    ContractorDirector = contractor.Director,
                    ContractorAddress = contractor.Address,
                    RegionId = _authService.Contractor.RegionId,
                    Region = region.FullName,
                    DistrictId = _authService.Contractor.DistrictId,
                    District = district.FullName,
                    DocOn = DateTime.Now.AsDateOnly(),
                    ApplicationTypeId = ApplicationTypeIdConst.CORRUPTION,
                    DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_JOIN_ANTI_CORRUPTION_APPLICATION, 1).Item2,
                    ContractorPositionName = "Директор"
                },
                Files = new(),
                Employees = new(),
                Participates = new(),
                Tables = new(),
                CanEdit = true,
            };
        }

        public JoinAntiCorruptionApplicationDto Get(long id)
        {
            var dto = Repository.ById<JoinAntiCorruptionApplicationDto>(id);
            if (dto == null)
                return null;

            if (_authService.Contractor == null)
            {
                dto.CanAcceptSSP = StatusIdConst.CanJoinAntiCorruptionApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.ACCEPTED_SSP)
                    && (_authService.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP
                    || _authService.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH
                    || _authService.Organization.OrganizationGroupId == OrganizationGroupIdConst.OTHER);

                dto.CanAcceptOmbudsman = StatusIdConst.CanJoinAntiCorruptionApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.ACCEPTED_OMBUDSMAN)
                    && (_authService.Organization.OrganizationGroupId == OrganizationGroupIdConst.OMBUDSMAN);

                dto.CanAccept = StatusIdConst.CanJoinAntiCorruptionApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.ACCEPTED)
                    && (_authService.Organization.OrganizationGroupId == OrganizationGroupIdConst.ANTI_CORRUPTION_AGENCY);

                dto.CanReject = 
                    _authService.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP ||
                    _authService.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH ||
                    _authService.Organization.OrganizationGroupId == OrganizationGroupIdConst.OTHER
                        ? StatusIdConst.CanJoinAntiCorruptionApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.REJECTED) && dto.Application.CurrentStep == null
                        : _authService.Organization.OrganizationGroupId == OrganizationGroupIdConst.OMBUDSMAN
                            ? StatusIdConst.CanJoinAntiCorruptionApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.REJECTED) &&
                              dto.Application.CurrentStep != null && dto.Application.CurrentStep.Id == StepIdConst.SENT_TO_OMBUDSMAN
                            : _authService.Organization.OrganizationGroupId == OrganizationGroupIdConst.ANTI_CORRUPTION_AGENCY
                                ? StatusIdConst.CanJoinAntiCorruptionApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.REJECTED)
                                : false;

                dto.CanEdit = StatusIdConst.CanJoinAntiCorruptionApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.MODIFIED);
                dto.CanDelete = StatusIdConst.CanJoinAntiCorruptionApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.DELETED);
            }
            else
            {
                dto.CanCancel = StatusIdConst.CanJoinAntiCorruptionApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.CANCELED)
                                  && _authService.HasPermission(ModuleCode.JoinAntiCorruptionApplicationCancel);
            }
            return dto;
        }

        public JoinAntiCorruptionApplicationDto Get(Guid id2)
        {
            var dto = GetQuery<JoinAntiCorruptionApplicationDto>()
                    .FirstOrDefault(a => a.Application.Id2 == id2 && new int[] { StatusIdConst.SENT, StatusIdConst.ACCEPTED }.Contains(a.Application.StatusId));
            if (dto == null)
                AddError("Ariza topilmadi / Заявление не найдено!");
            return dto;
        }

        public HaveId<long> Create(CreateJoinAntiCorruptionApplicationDlDto dto)
        {
            if (Repository.AllAsQueryable.Include(x => x.Application).Any(a => a.Application.StatusId != StatusIdConst.REJECTED && a.Application.StatusId != StatusIdConst.CANCELED
              && a.Application.StatusId != StatusIdConst.CANCELED && a.Application.ApplicationTypeId == ApplicationTypeIdConst.CORRUPTION))
            {
                AddError("Ariza allaqachon yaratilgan / Заявка уже создана");
                return null;
            }
            //if (!_systemConf.IsTest) // testda xalaqt bermasligi uchun
            //{
            //    var certificate = _unitOfWork.PrtnCertificateRepository.AllAsQueryable.FirstOrDefault(a => a.StatusId == StatusIdConst.FORMED);
            //    if (certificate == null)
            //    {
            //        AddError("Sertifikat mavjud emas");
            //        return null;
            //    }
            //}

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = Repository.Create(dto, ent => { });
                    CombineStatuses(Repository);

                    if (HasErrors) { transaction.Rollback(); return null; }

                    if (IsValid)
                    UnitOfWork.Save();

                    var res = CreateDocumentChangeLog(entity.Id);

                    if (HasErrors) { transaction.Rollback(); return null; }

                    if (IsValid)
                    {
                        transaction.Commit();
                        return HaveId.Create(entity.Id);
                    }
                }
                catch (Exception ex)
                {
                    AddError($"{ex.Message} : {ex.InnerException}");
                    transaction.Rollback();
                }
                finally
                {
                    transaction.Dispose();
                }
            }
            return null;
        }

        public void Update(UpdateJoinAntiCorruptionApplicationDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var entity = Repository.Update(dto, ent =>
                {

                });
                CombineStatuses(Repository);

                _storageService.ResolveMarkedFiles(DocumentStorageConst.DOC_JOIN_ANTI_CORRUPTION_APPLICATION_FILES, $"{dto.Id}");
                if (IsValid)
                    UnitOfWork.Save();

                var res = CreateDocumentChangeLog(entity.Id);

                if (IsValid)
                {
                    transaction.Commit();
                }
            }
        }

        public bool CanCreate(string inn = null)
        {
            inn ??= _authService.Contractor.Inn;
            var dto = Repository.CrudServices.ReadManyNoTracked<JoinAntiCorruptionApplicationDto>()
                        .Where(a => a.Application.ContractorInn == inn
                                && a.Application.ApplicationTypeId == ApplicationTypeIdConst.CORRUPTION
                                && !new int[] { StatusIdConst.DELETED, StatusIdConst.REJECTED, StatusIdConst.CANCELED }.Contains(a.Application.StatusId)).ToList();
            if (dto.Any())
                return false;
            return true;
        }

        public void Accept(AcceptStatusJoinAntiCorruptionApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();
            try
            {
                _unitOfWork.Context.Set<JoinAntiCorruptionApplication>().Lock(dto.Id);

               // var step = new UpdateStepDlDto();
                if (_authService.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP ||
                    _authService.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH ||
                    _authService.Organization.OrganizationGroupId == OrganizationGroupIdConst.OTHER)
                {
                    dto.StatusId = StatusIdConst.ACCEPTED_SSP;
                    //step.StepId = StepIdConst.SENT_TO_OMBUDSMAN;
                }
                else if (_authService.Organization.OrganizationGroupId == OrganizationGroupIdConst.OMBUDSMAN)
                {
                    dto.StatusId = StatusIdConst.ACCEPTED_OMBUDSMAN;
                   // step.StepId = StepIdConst.SENT_TO_ANTI_CORRUPTION_AGENCY;
                }
                else if (_authService.Organization.OrganizationGroupId == OrganizationGroupIdConst.ANTI_CORRUPTION_AGENCY)
                {
                    dto.StatusId = StatusIdConst.ACCEPTED;
                   // step.StepId = StepIdConst.ACCEPT;
                }
                else
                {
                    AddError($"Организационная группа xato! {_authService.Organization.OrganizationGroupId}");
                    return;
                }

                var entity = Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanJoinAntiCorruptionApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                    else
                    {
                        ent.Application.StatusId = dto.StatusId;
                        ent.Application.Message = dto.Message;
                    }
                });
                if (entity is null)
                    return;

                //step.Id = entity.ApplicationId;
              //  Repository.UpdateStep(step);

                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(dto.Id);
                if (IsValid && canCommit)
                    transaction.Commit();
            }
            catch (Exception ex)
            {
                AddError($"{ex.Message} - {ex.InnerException}");
                transaction.Rollback();
                return;
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }

        public void Reject(RejectStatusJoinAntiCorruptionApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<JoinAntiCorruptionApplication>().Lock(dto.Id);

                var step = new UpdateStepDlDto();
                if (_authService.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP ||
                    _authService.Organization.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH ||
                    _authService.Organization.OrganizationGroupId == OrganizationGroupIdConst.OTHER)
                    step.StepId = StepIdConst.REJECTED_SSP;
                else if (_authService.Organization.OrganizationGroupId == OrganizationGroupIdConst.OMBUDSMAN)
                    step.StepId = StepIdConst.REJECTED_OMBUDSMAN;
                else if (_authService.Organization.OrganizationGroupId == OrganizationGroupIdConst.ANTI_CORRUPTION_AGENCY)
                    step.StepId = StepIdConst.REJECTED_ANTI_CORRUPTION_AGENCY;
                else
                {
                    AddError($"Организационная группа xato! {_authService.Organization.OrganizationGroupId}");
                    return;
                }

                var entity = Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanJoinAntiCorruptionApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                    else
                    {
                        ent.Application.StatusId = dto.StatusId;
                        ent.Application.Message = dto.Message;
                    }
                });
                if (entity is null)
                    return;

                step.Id = entity.ApplicationId;
                Repository.UpdateStep(step);

                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                _unitOfWork.Save();
                var res = CreateDocumentChangeLog(id: dto.Id);

                if (IsValid && canCommit)
                    transaction.Commit();
            }
            catch (Exception ex)
            {
                AddError($"{ex.Message} - {ex.InnerException}");
                transaction.Rollback();
                return;
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }

        public void Cancel(CancelStatusJoinAntiCorruptionApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<Application>().Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplyStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                    else
                    {
                        ent.Application.StatusId = dto.StatusId;
                        ent.Application.Message = dto.Message;
                    }
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

        public void Revoke(RevokeStatusJoinAntiCorruptionApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<Application>().Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanMemshipApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                    else
                    {
                        ent.Application.StatusId = dto.StatusId;
                        ent.Application.Message = dto.Message;
                    }
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

        public async Task Send(SendStatusJoinAntiCorruptionApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            var doc = Get(dto.Id);

            if (doc == null)
            {
                AddError("По вашему запросу запись не найдено");
                return;
            }

            if (_authService.Contractor != null)
            {
                dto.StatusId = StatusIdConst.SENDING;
                if (doc.Application.ContractorId != _authService.Contractor.Id)
                {
                    AddError("Нет доступа");
                    return;
                }
            }
            else
                dto.StatusId = StatusIdConst.SENT;

            var eImzoTimstampDto = new EImzoTimeStampDto
            {
                SignData = dto.SignedData,
                Inn = _authService.User.Inn,
                Pinfl = _authService.User.Pinfl
            };

            var timeStamp = _eImzoService.TimeStamp(eImzoTimstampDto).Result;

            CombineStatuses(_eImzoService);
            if (HasErrors)
                return;

            if (_authService.Contractor != null)
            {
                var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
                {
                    SignData = timeStamp.Pkcs7b64,
                    Inn = dto.IsPinfl ? null : _authService.Contractor.Inn,
                    Pinfl = _authService.Contractor.Pinfl
                });
            }
            else
            {
                var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
                {
                    SignData = timeStamp.Pkcs7b64,
                    Inn = null,
                    Pinfl = _authService.User.Pinfl
                });
            }

            CombineStatuses(_eImzoService);
            if (HasErrors)
                return;

            dto.SignFile = SaveFile(doc.Id, timeStamp.Pkcs7b64, "sign.txt");
            dto.DataFile = SaveFile(doc.Id, JsonConvert.SerializeObject(doc), "data.txt");
            dto.SignedUserInfo = _authService.User.ToTextForDocumentLog();

            try
            {
                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanMemshipApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                    else
                    {
                        ent.Application.StatusId = dto.StatusId;
                        ent.Application.Message = dto.Message;
                    }
                });
                CombineStatuses(Repository);

                if (IsValid && canCommit)
                    transaction.Commit();
            }
            catch (DbUpdateException e)
            {
                AddError(e.Message);
                transaction.Rollback();
            }
        }

        public async ValueTask<byte[]> DownloadPdf(Guid id2, string? lang)
        {
            var language = lang ?? "uz-latn";
            var languageId = UnitOfWork.Context.Set<Language>()
                .FirstOrDefault(l => l.Code == ServiceProvider.CultureHelper.CurrentCulture.Code)?.Id ?? 1;

            var wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                  language,
                    StaticFileConst.WordTemplate.JOIN_ANTICORRUPTION_APPLICATION)
                );

            var application = await UnitOfWork.Context.Set<JoinAntiCorruptionApplication>()
                .Include(a => a.Application)
                .Include(a => a.Employees)
                .Include(a => a.Tables)
                .Include(a => a.Participates)
                .Include(a => a.ContractorActivityType)
                .Include(a => a.ContractorActivityType.Translates)
                .FirstOrDefaultAsync(a => a.Application.Id2 == id2);

            var contractor = await UnitOfWork.Context.Set<Contractor>()
                .Include(c => c.OrganizationLegalForm)
                .Include(c => c.Region)
                .Include(c => c.Region.Translates)
                .Include(c => c.District)
                .Include(c => c.District.Translates)
                .Include(c => c.Contacts)
                .FirstOrDefaultAsync(c => c.Id == application.Application.ContractorId);
            if (contractor == null || application == null)
            {
                AddError("Not found");
                return null;
            }

            var link = _systemConf.QrImagePrintMy + "/JoinAntiCorruptionApplication/DownloadPdf?id2=" + application.Application.Id2.ToString();
            var qrCode = QRCodeHelper.GeneratePng(link);
            var qrImage = new MemoryStream(qrCode);
            var plh = new Placeholders();
            plh.ImagePlaceholders.Add("QrCode", new ImageElement
            {
                Dpi = 512,
                MemStream = qrImage,
            });

            plh.TextPlaceholders.Add(nameof(contractor.FullName), contractor.FullName);
            plh.TextPlaceholders.Add(nameof(contractor.Director), contractor.Director);
            plh.TextPlaceholders.Add(nameof(application.Application.DocOn.Day), application.Application.DocOn.Day.ToString());
            plh.TextPlaceholders.Add(nameof(application.Application.DocOn.Month), application.Application.DocOn.Month.ToString());
            plh.TextPlaceholders.Add(nameof(application.Application.DocOn.Year), application.Application.DocOn.Year.ToString());
            plh.TextPlaceholders.Add(nameof(contractor.OrganizationLegalForm), contractor?.OrganizationLegalForm?.FullName ?? "-");
            plh.TextPlaceholders.Add(nameof(contractor.RegistrationDate), contractor.RegistrationDate.ToString("dd.MM.yyyy") ?? "-");
            plh.TextPlaceholders.Add(nameof(contractor.Region), contractor.Region.Translates
                .FirstOrDefault(x => x.LanguageId == languageId)?.TranslateText ?? contractor?.Region?.FullName ?? "-");
            plh.TextPlaceholders.Add(nameof(contractor.District), contractor.District.Translates
                .FirstOrDefault(x => x.LanguageId == languageId)?.TranslateText ?? contractor.District.FullName ?? "-");
            plh.TextPlaceholders.Add(nameof(contractor.Address), contractor.Address ?? "-");
            plh.TextPlaceholders.Add(nameof(contractor.Contacts),
                (contractor.Contacts.FirstOrDefault(c => c.ContactTypeId == ContactTypeIdConst.WEB_SITE)?.Contact ?? "-") + "-" +
                (contractor.Contacts.FirstOrDefault(c => c.ContactTypeId == ContactTypeIdConst.EMAIL)?.Contact ?? "-"));
            plh.TextPlaceholders.Add(nameof(contractor.Inn), contractor.Inn ?? "-");
            plh.TextPlaceholders.Add(nameof(application.UnionMemberCount), application.UnionMemberCount.ToString() ?? "-");
            plh.TextPlaceholders.Add(nameof(application.PrevYearlyEarnings), application.PrevYearlyEarnings.ToString() ?? "-");
            plh.TextPlaceholders.Add(nameof(application.ContractorActivityType), application.ContractorActivityType.Translates
                .FirstOrDefault(x => x.LanguageId == languageId)?.TranslateText ?? application.ContractorActivityType.FullName ?? "-");
            plh.TextPlaceholders.Add(nameof(application.AvgEmployeesCount), application.AvgEmployeesCount.ToString() ?? "-");
            var employee = application.Employees.FirstOrDefault() ?? new();
            plh.TextPlaceholders.Add(nameof(employee.Person), employee.Person ?? "-");
            plh.TextPlaceholders.Add(nameof(employee.Position), employee.Position ?? "-");
            plh.TextPlaceholders.Add(nameof(employee.PhoneNumber), employee.PhoneNumber ?? "-");
            plh.TextPlaceholders.Add(nameof(employee.Email), employee.Email ?? "-");

            var item = application.Tables.FirstOrDefault() ?? new();
            plh.TablePlaceholders.Add(new Dictionary<string, List<string>>()
            {
                { nameof(item.Measures), application.Tables.Select(t => t.Measures).ToList() },
                { nameof(item.ExpireOn), application.Tables.Select(t => t.ExpireOn.ToString("dd.MM.yyyy")).ToList() },
                { nameof(item.ResponsibleFio), application.Tables.Select(t => t.ResponsibleFio).ToList() },
                { nameof(item.MeasuresResult), application.Tables.Select(t => t.MeasuresResult).ToList() },
            });
            var participate = application.Participates.FirstOrDefault() ?? new();
            plh.TablePlaceholders.Add(new Dictionary<string, List<string>>
            {
                { nameof(participate.YearIn),application.Participates.Select(x=>x.YearIn.ToString()).ToList() },
                { nameof(participate.InvestigationOrganization),application.Participates.Select(x=>x.InvestigationOrganization).ToList() },
                { nameof(participate.BasisForInvestigation),application.Participates.Select(x=>x.BasisForInvestigation).ToList() },
                { nameof(participate.InvestigatedPersonFio),application.Participates.Select(x=>x.InvestigatedPersonFio).ToList() },
                { nameof(participate.InvestigatedResult),application.Participates.Select(x=>x.InvestigatedResult).ToList() },
            });
            wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();

            var res = await _pdfConverter.DocxToPdfAsync(wordFile, new());
            CombineStatuses(_pdfConverter);
            return res;
        }

        #region HELPER
        protected IQueryable<JoinAntiCorruptionApplicationListDto> SortFilter(IQueryable<JoinAntiCorruptionApplicationListDto> query, JoinAntiCorruptionApplicationSortFilterOptions options)
        {
            return base.SortFilter(query, options).SortFilter(options).Where(a => a.Application.ApplicationTypeId == ApplicationTypeIdConst.CORRUPTION);
        }
        private HaveId<long> CreateDocumentChangeLog(long id, string message = null, string userIp = null, string userAgent = null)
        {
            var entityDto = Repository.ById<JoinAntiCorruptionApplicationDto>(id, applyFilter: false);

            _documentChangeLogService.CreateApplication(
                dto: entityDto,
                organizationId: null,
                message: message);

            CombineStatuses(_documentChangeLogService);

            if (HasErrors)
                return null;

            return HaveId.Create(id);
        }
        private IQueryable<TDto> GetQuery<TDto>()
            where TDto : class
        {
            return Repository.ReadAsNoTracked<TDto>();
        }
        #endregion

        #region FILES
        public IEnumerable<JoinAntiCorruptionApplicationFileDto> UploadFiles(params StorageFile[] files)
        {
            var result = _storageService
                .SaveTemp(DocumentStorageConst.DOC_JOIN_ANTI_CORRUPTION_APPLICATION_FILES, files)
                .Select(a => new JoinAntiCorruptionApplicationFileDto
                {
                    Id = a.FileId,
                    FileName = a.FileName
                });
            CombineStatuses(_storageService);
            return IsValid ? result : null;
        }

        public StorageFile DownloadFile(Guid fileId)
        {
            var entity = _unitOfWork.Context.Set<JoinAntiCorruptionApplicationFile>().FirstOrDefault(a => a.Id == fileId);
            return Download(fileId, entity, DocumentStorageConst.DOC_JOIN_ANTI_CORRUPTION_APPLICATION_FILES);
        }

        private StorageFile Download(Guid fileId, FileEntity<long> entity, string storageDocument)
        {
            StorageFile file;

            if (entity == null)
            {
                file = _storageService.GetTempFile(storageDocument, fileId);
                CombineStatuses(_storageService);
            }
            else
            {
                file = _storageService.GetFile(storageDocument, entity.OwnerId.ToString(), fileId);
                CombineStatuses(_storageService);

                if (IsValid)
                    file.FileName = entity.FileName;
            }

            return file;
        }

        public void DeleteFile(Guid fileId)
        {
            var entity = _unitOfWork.Context
                .Set<JoinAntiCorruptionApplicationFile>()
                .FirstOrDefault(a => a.Id == fileId);

            Delete(fileId, entity, DocumentStorageConst.DOC_JOIN_ANTI_CORRUPTION_APPLICATION_FILES);
        }

        private void Delete(Guid fileId, FileEntity<long> entity, string storageDocument)
        {
            if (entity != null)
            {
                _storageService.DeleteTemp(storageDocument, fileId);
                CombineStatuses(_storageService);
            }
        }

        private Guid SaveFile(long docId, string data, string fileName)
        {
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(data);
            writer.Flush();
            ms.Position = 0;
            var fileInfo = _storageService.Save($"{nameof(TableIdConst.CORRUPTION__DOC_JOIN_ANTI_CORRUPTION_APPLICATION)}_SIGN_DATA", docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

            return fileInfo.FirstOrDefault().FileId;
        }
        #endregion
    }
}
