using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using Newtonsoft.Json;
using SspUis.BizLogicLayer.Claim;
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
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Memship;
using SspUis.Integration.DocxToPdf;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WEBASE.i18n;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.OfficeTools;
using WEBASE.OfficeTools.Handlers;
using WEBASE.QRCode;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.ClaimApplicationServices
{
    public class ErpClaimApplicationServiceService : BaseApplicationService
        <ClaimApplication,
        ClaimApplicationListDto,
        ClaimApplicationDto,
        CreateClaimApplicationDlDto,
        UpdateClaimApplicationDlDto,
        IClaimApplicationRepository,
        ClaimApplicationSortFilterOptions>, IErpClaimApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly INumberService _numberService;
        private readonly IEImzoService _eImzoService;
        private readonly IDocumentChangeLogService _documentChangeLogService;
        private readonly IBaseReportService _baseReportService;
        private readonly SystemConf _systemConf;
        private readonly IMemshipContractRepository _memshipContractRepository;
        private readonly DbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApiRequestLogRepository _apiRequestLogRepository;
        private readonly IStorageService _storageService;
        private readonly IConvertService _pdfConverter;
        private readonly ICultureHelper _cultureHelper;

        public ErpClaimApplicationServiceService(IUnitOfWork unitOfWork,
            IAuthService authService,
            INumberService numberService,
            IDocumentChangeLogService documentChangeLogService,
            IBaseReportService baseReportService,
            SystemConf systemConf,
            IEImzoService eImzoService,
            IMemshipContractRepository memshipContractRepository,
            DbContext context,
            IHttpContextAccessor httpContextAccessor,
            IApiRequestLogRepository apiRequestLogRepository,
            IStorageService storageService,
            ICultureHelper cultureHelper,
            IConvertService pdfConverter)
            : base(unitOfWork, documentChangeLogService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _numberService = numberService;
            _documentChangeLogService = documentChangeLogService;
            _baseReportService = baseReportService;
            _systemConf = systemConf;
            _eImzoService = eImzoService;
            _memshipContractRepository = memshipContractRepository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _apiRequestLogRepository = apiRequestLogRepository;
            _storageService = storageService;
            _cultureHelper = cultureHelper;
            this._pdfConverter = pdfConverter;
        }

        public override PagedResult<ClaimApplicationListDto> GetList(ClaimApplicationSortFilterOptions options)
        {
            if (options.IsEmployee)
                options.OrganizationId = _authService.User.OrganizationId;

            var dto = base.GetList(options);

            return dto;
        }
        public List<DocNumbersByClaimAppTypeDto> GetDataDocNumbersByClaimAppTypeId(ByClaimAppTypeFilter dto)
        {
            var data = _unitOfWork.Context.Set<Application>().Include(a => a.ClaimApplication)
                .Where(a => a.ContractorId == dto.ContractorId
                    && a.ApplicationTypeId == ApplicationTypeIdConst.CLAIM
                    && a.StatusId != StatusIdConst.DELETED
                    && a.ClaimApplication.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APILATION_CASSATION);

            if (data == null)
                return null;

            return data.Select(a => new DocNumbersByClaimAppTypeDto()
            {
                Id = a.Id,
                DocNumber = a.DocNumber
            }).ToList();
        }

        public override ClaimApplicationDto Get()
        {
            var region = _unitOfWork.RegionRepository.ById<RegionListDto>(_authService.Contractor.RegionId);

            var district = _unitOfWork.DistrictRepository.ById<DistrictListDto>(_authService.Contractor.DistrictId);

            var contractor = _unitOfWork.ContractorRepository.ById<ContractorListDto>(_authService.Contractor.Id);

            var memshipContract = _unitOfWork.Context.Set<MemshipContract>()
                .OrderByDescending(a => a.Id)
                .FirstOrDefault(a => a.ContractorId == _authService.Contractor.Id && a.StatusId == StatusIdConst.SIGNED);

            if (memshipContract == null)
            {
                AddError("Азолик шартнома яратилмаган !");
                return null;
            }

            return new ClaimApplicationDto
            {
                Application = new()
                {
                    Id2 = Guid.NewGuid(),
                    Contractor = _authService.Contractor.FullName,
                    ContractorInn = _authService.Contractor.Inn,
                    DocOn = DateTime.Now.AsDateOnly(),
                    ContractorId = _authService.Contractor.Id,
                    ContractorPositionName = "Директор",
                    ContractorAddress = contractor.Address,
                    ContractorDirector = contractor.Director,
                    RegionId = _authService.Contractor.RegionId,
                    Region = region.FullName,
                    DistrictId = _authService.Contractor.DistrictId,
                    District = district.FullName,
                    ApplicationTypeId = ApplicationTypeIdConst.CLAIM,
                    DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_APPLICATION, 1).Item2,
                },
                Tables = new(),
                Files = new(),
                CanEdit = true,
                MemshipContractId = memshipContract.Id,
                MemshipContractDocOn = memshipContract.DocOn,
                MemshipContractDocNumber = memshipContract.DocNumber,
            };
        }

        public override ClaimApplicationDto Get(long id)
        {
            var dto = Repository.ById<ClaimApplicationDto>(id);

            if (dto == null)
                return null;

            if (_authService.Contractor != null)
            {
                dto.CanEdit = StatusIdConst.CanClaimApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.MODIFIED);
                dto.CanSend = StatusIdConst.CanClaimApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.SENT);
                dto.CanRevoke = StatusIdConst.CanClaimApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.REVOKED);
            }
            else if (dto.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APILATION_CASSATION
                       || dto.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.CLAIM_REVISION || dto.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.COUNTER_CLAIM || dto.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APPLICATION_FOR_COURT)
            {
                dto.CanCreateApplicationForCourt = dto.Application.StatusId == StatusIdConst.SENT || dto.Tables.Any(a => a.ClaimResponsibleTypeId == ClaimResponsibleTypeIdConst.INDIVIDUAL_PERSON);
                dto.CanCancel = StatusIdConst.CanClaimApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.CANCELED)
                                    && _authService.HasPermission(ModuleCode.ClaimApplicationCancel);
            }
            else
            {
                dto.CanAccept = StatusIdConst.CanClaimApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.ACCEPTED)
                                    && _authService.HasPermission(ModuleCode.ClaimApplicationAccept);
                //dto.CanReject = StatusIdConst.CanClaimApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.REJECTED)
                //                    && _authService.HasPermission(ModuleCode.ClaimApplicationReject);
                dto.CanCancel = StatusIdConst.CanClaimApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.CANCELED)
                                    && _authService.HasPermission(ModuleCode.ClaimApplicationCancel);

                dto.CanCreateWhithOutMediation = dto.Application.StatusId == StatusIdConst.ACCEPTED;

                dto.CanCreateMediationPlan = !UnitOfWork.Context.Set<MediationPlan>().Any(p => p.ApplicationId == dto.ApplicationId)
                                    && dto.Application.StatusId == StatusIdConst.ACCEPTED;

                dto.CanEmployeeAttechment = dto.EmployeeManageId == null && dto.Application.StatusId != StatusIdConst.CANCELED &&
                    dto.Application.StatusId != StatusIdConst.REJECTED;
            }

            return dto;
        }

        public ClaimAppApplicationForCourtMediationDto GetForInfo(long prevAppId, int stepId)
        {
            var dto = new ClaimAppApplicationForCourtMediationDto();

            var claimApp = UnitOfWork.ClaimApplicationRepository.AllAsQueryable
                .Include(x => x.Application)
                .Include(x => x.MemshipCertificate)
                .Include(x => x.MemshipContract)
                .Include(x => x.ClaimApplicationType)
                .Include(x => x.Currency)
                .Include(x => x.ClaimTheme)
                .FirstOrDefault(x => x.ApplicationId == prevAppId);
            
            if (claimApp == null)
            {
                AddError("Da'vo arizasi topilmadi!");
                return null;
            }

            dto.ClaimApplication = new ClaimApplicationDto()
            {
                ClaimThemeId = claimApp.ClaimThemeId,
                CalculedPenalty = claimApp.CalculedPenalty,
                ClaimApplicationType = claimApp.ClaimApplicationType.FullName,
                ClaimApplicationTypeId = claimApp.ClaimApplicationTypeId,
                ClaimTheme = claimApp.ClaimTheme.FullName,
                ContractIdentificationNumber = claimApp.ContractIdentificationNumber,
                Currency = claimApp.Currency.FullName,
                CurrencyId = claimApp.CurrencyId,
                CurrentInterestRate = claimApp.CurrentInterestRate,
                CurrentPrincipalInterest = claimApp.CurrentPrincipalInterest,
                MemshipCertificateId = claimApp.MemshipCertificateId,
                MainDebt = claimApp.MainDebt,
                MemshipContractDocNumber = claimApp.MemshipContract.DocNumber,
                MemshipContractDocOn = claimApp.MemshipContract.DocOn,
                MemshipContractId = claimApp.MemshipContractId,
                EmployeeManageId = claimApp.EmployeeManageId,
                ApplicationId = claimApp.ApplicationId,
                PrevApplicationId = claimApp.PrevApplicationId,
                Penalty = claimApp.Penalty,
                Percent = claimApp.Percent,
                TotalAmount = claimApp.TotalAmount,
                Files = claimApp.Files.Select(x => new ClaimApplicationFileDlDto()
                {
                    Id = x.Id
                }).ToList(),
                Id = claimApp.Id,
                Details = claimApp.Details,
                OrganizationId = claimApp.OrganizationId,
                OtherDebtRepayment = claimApp.OtherDebtRepayment
            };

            if (stepId == StepIdConst.MEDIATION_PLAN_CREATE)
            {
                var mediationPlan = UnitOfWork.MediationPlanRepository.AllAsQueryable
                .Include(x => x.Application)
                    .ThenInclude(x => x.ClaimApplication)
                        .ThenInclude(x => x.ClaimApplicationType)
                .Include(x => x.Contractor)
                .Include(x => x.Status)
                .Include(x => x.Organization)
                .FirstOrDefault(x => x.ApplicationId == prevAppId);

                if (mediationPlan == null)
                {
                    AddError("Nizoni sudgacha hal qilish rejasi topilmadi!");
                    return null;
                }

                dto.MediationPlan = new MediationPlanDto()
                {
                    Id = mediationPlan.Id,
                    Id2 = mediationPlan.Id2,
                    DocNumber = mediationPlan.DocNumber,
                    DocOn = mediationPlan.DocOn,
                    ApplicaionDocNumber = mediationPlan.Application.DocNumber,
                    ClaimApplicationType = mediationPlan.Application
                        .ClaimApplication.ClaimApplicationType.ShortName,
                    Contractor = mediationPlan.Contractor.FullName,
                    ContractorId = mediationPlan.ContractorId,
                    ContractorInn = mediationPlan.Contractor.Inn,
                    AddressOrUrl = mediationPlan.AddressOrUrl,
                    ApplicationId = mediationPlan.ApplicationId,
                    ApplicaionDocOn = mediationPlan.Application.DocOn,
                    ApplicationTypeId = mediationPlan.Application.ApplicationTypeId,
                    MeditionAt = mediationPlan.MeditionAt,
                    ChamberPerson = mediationPlan.ChamberPerson,
                    Organization = mediationPlan.Organization.FullName,
                    MeetingTypeId = mediationPlan.MeetingTypeId,
                    StatusId = mediationPlan.StatusId,
                    Status = mediationPlan.Status.FullName,
                };

                return dto;
            }

            long mediationPlanId = 0;
            if(stepId == StepIdConst.MEDIATION_CREATE)
            {
                var mediationPlan = UnitOfWork.MediationPlanRepository.AllAsQueryable
                .Include(x => x.Application)
                    .ThenInclude(x => x.ClaimApplication)
                        .ThenInclude(x => x.ClaimApplicationType)
                .Include(x => x.Contractor)
                .Include(x => x.Status)
                .Include(x => x.Organization)
                .FirstOrDefault(x => x.ApplicationId == prevAppId);

                if (mediationPlan != null)
                    mediationPlanId = mediationPlan.Id;

                var mediation = UnitOfWork.MediationRepository.AllAsQueryable
                    .Include(x => x.Files)
                    .Include(x => x.ApplicationForCourts)
                    .Include(x => x.MediationPlan)
                    .Include(x => x.Contractor)
                    .Include(x => x.ClaimNeedCourt)
                    .Include(x => x.MediationResult)
                    .FirstOrDefault(x => x.MediationPlanId == mediationPlanId);
            
                if (mediation == null)
                {
                    AddError("Nizoni sudgacha hal qilish bayonnomasi topilmadi!");
                    return null;
                }

                dto.Mediation = new MediationDto()
                {
                    DocNumber = mediation.DocNumber,
                    Status = mediation.Status.FullName,
                    Id = mediation.Id,
                    StatusId = mediation.Status.Id,
                    DocOn = mediation.DocOn,
                    ContractorDetails = mediation.ContractorDetails,
                    ChamberPerson = mediation.ChamberPerson,
                    ClaimantPersonName = mediation.ClaimantPersonName,
                    ClaimNeedCourt = mediation.ClaimNeedCourt.FullName,
                    ClaimNeedCourtId = mediation.ClaimNeedCourtId,
                    Contractor = mediation.Contractor.FullName,
                    ContractorId = mediation.ContractorId,
                    ContractorInn = mediation.Contractor.Inn,
                    CourtAt = mediation.CourtAt,
                    Files = mediation.Files.Select(x => new MediationServices.MediationFileDto()
                    {
                        CreatedAt = x.CreatedAt,
                        FileName = x.FileName,
                        Id = x.Id
                    }).ToList(),
                    Id2 = mediation.Id2,
                    MediationPlanId = mediation.MediationPlanId,
                    MediationResult = mediation.MediationResult.FullName,
                    MediationResultId = mediation.MediationResultId,
                    ResponsibleDetails = mediation.ResponsibleDetails,
                    ResponsiblePersonName = mediation.ResponsiblePersonName,
                };

                return dto;
            }

            if(stepId == StepIdConst.APPLICATION_FOR_COURT_CREATE)
            {
                var mediationPlan = UnitOfWork.MediationPlanRepository.AllAsQueryable
                .Include(x => x.Application)
                    .ThenInclude(x => x.ClaimApplication)
                        .ThenInclude(x => x.ClaimApplicationType)
                .Include(x => x.Contractor)
                .Include(x => x.Status)
                .Include(x => x.Organization)
                .FirstOrDefault(x => x.ApplicationId == prevAppId);

                var mediation = UnitOfWork.MediationRepository.AllAsQueryable
                    .Include(x => x.Files)
                    .Include(x => x.ApplicationForCourts)
                    .Include(x => x.MediationPlan)
                    .Include(x => x.Contractor)
                    .Include(x => x.ClaimNeedCourt)
                    .Include(x => x.MediationResult)
                    .FirstOrDefault(x => x.MediationPlanId == mediationPlan.Id);

                var applicationForCourt = UnitOfWork.ApplicationForCourtRepository.AllAsQueryable
                    .Include(x => x.Files)
                    .Include(x => x.Department)
                    .Include(x => x.Position)
                    .Include(x => x.Step)
                    .Include(x => x.Status)
                    .Include(x => x.ClaimOrganization)
                    .Include(x => x.Contractor)
                    .Include(x => x.EmployeeManage)
                        .ThenInclude(m => m.Employee)
                            .ThenInclude(p => p.Person)
                    .FirstOrDefault(x => x.MediationId == mediation.Id);
            
                if (applicationForCourt == null)
                {
                    AddError("Davo arizasi(sudga) topilmadi!");
                    return null;
                }

                dto.ApplicationForCourt = new Claim.ApplicationForCourtDto()
                {
                    Status = applicationForCourt.Status.FullName,
                    Step = applicationForCourt.Step.FullName,
                    StatusId = applicationForCourt.StatusId,
                    StepId = applicationForCourt.StepId,
                    ClaimApplicationForCourtTypeId = applicationForCourt.ClaimApplicationForCourtTypeId,
                    ClaimOrganizationId = applicationForCourt.ClaimOrganizationId,
                    ContractorId = applicationForCourt.ContractorId,
                    Department = applicationForCourt.Department.FullName,
                    DepartmentId = applicationForCourt.DepartmentId,
                    DocNumber = applicationForCourt.DocNumber,
                    DocOn = applicationForCourt.DocOn,
                    EmployeeManageId = applicationForCourt.EmployeeManageId,
                    PositionId = applicationForCourt.PositionId,
                    Position = applicationForCourt.Position.FullName,
                    TableId = TableIdConst.CLAIM__DOC_APPLICATION_FOR_COURT,
                    MediationId = applicationForCourt.MediationId,
                    Contractor = applicationForCourt.Contractor.FullName,
                    Id = applicationForCourt.Id,
                    Files = applicationForCourt.Files.Select(x => new ApplicationForCourtFileDto()
                    {
                        Id = x.Id,
                        CreatedAt = x.CreatedAt,
                        FileName = x.FileName
                    }).ToList(),
                    Id2 = applicationForCourt.Id2,
                    ClaimOrganization = applicationForCourt.ClaimOrganization.FullName,
                    EmployeeManage = applicationForCourt.EmployeeManage.Employee.Person.FullName
                };

                return dto;
            }

            return dto;
        }

        public ClaimApplicationDto Get(Guid id2)
        {
            var dto = GetQuery<ClaimApplicationDto>().FirstOrDefault(a => a.Application.Id2 == id2
                && new int[]
                {
                    StatusIdConst.SENT, StatusIdConst.ACCEPTED
                }.Contains(a.Application.StatusId));

            if (dto == null)
            {
                AddError("Ariza topilmadi / Заявление не найдено!");
                return null;
            }

            return dto;
        }

        public async Task<HaveId<long>> CreateClaimApplication(CreateClaimApplicationDlDto dto)
        {
            foreach (var tableItem in dto.Tables)
                tableItem.IsRegistred = _unitOfWork.Context.Set<BusinessmanUser>()
                        .Any(a => a.Inn == tableItem.InnOrPinfl || a.Pinfl == tableItem.InnOrPinfl)
                ? true : false;

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = Repository.Create(dto, ent => ClaimValidation(dto, ent));
                    CombineStatuses(Repository);
                    if (HasErrors) return null;

                    UnitOfWork.Save();

                    _storageService.MoveToPersistent(DocumentStorageConst.DOC_CLAIM_APPLICATION_FILES, $"{entity.Id}", dto.Files.Select(a => a.Id).ToArray());
                    CombineStatuses(_storageService);
                    if (HasErrors) { AddError("Филе юклаш мажбурий !"); return null; };

                    await this.Send(new SendStatusClaimApplicationDto()
                    {
                        SignedData = dto.SignedData,
                        Id = entity.Id
                    });

                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return null;
                    }

                    var res = CreateDocumentChangeLog(entity.Id);
                    if (IsValid)
                    {
                        transaction.Commit();
                        return HaveId.Create(entity.Id);
                    }
                }
                catch (Exception ex)
                {
                    AddError($"{ex.Message} : {ex.InnerException}");
                }
            }
            return null;
        }

        public bool CanCreate(string inn = null)
        {
            inn ??= _authService.Contractor.Inn;
            var dto = Repository.CrudServices.ReadManyNoTracked<ClaimApplicationDto>()
                        .Where(a => a.ContractorInn == inn
                                && a.Application.ApplicationTypeId == ApplicationTypeIdConst.CLAIM
                                && !new int[] { StatusIdConst.DELETED, StatusIdConst.REJECTED, StatusIdConst.CANCELED }.Contains(a.Application.StatusId))
                        .ToList();

            if (dto.Any()) return false;
            return true;
        }

        public override void Update(UpdateClaimApplicationDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var entity = Repository.Update(dto, ent =>
                {
                    if (!StatusIdConst.CanEditStatuses.Contains(ent.Application.StatusId))
                        AddError("Tahrirlash mumkin emas / Невозможно редактировать");
                });

                CombineStatuses(Repository);

                _storageService.ResolveMarkedFiles(DocumentStorageConst.DOC_CLAIM_APPLICATION_FILES, $"{dto.Id}");

                if (IsValid)
                    UnitOfWork.Save();

                var res = CreateDocumentChangeLog(entity.Id);

                if (IsValid) transaction.Commit();
            }
        }

        public async Task Send(SendStatusClaimApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            var doc = Repository.ById<ClaimApplicationDto>(dto.Id);

            if (doc == null)
            {
                AddError("По вашему запросу запись не найдено");
                return;
            }
            else if (doc.Application.ContractorId != _authService.Contractor.Id)
            {
                AddError("Нет доступа");
                return;
            }

            var timeStamp = await _eImzoService.TimeStamp(new EImzoTimeStampDto
            {
                SignData = dto.SignedData,
                Inn = dto.IsPinfl ? null : _authService.Contractor?.Inn,
                Pinfl = dto.IsPinfl
                   ? (_authService.Contractor == null ? _authService.User.Pinfl : _authService.Contractor.Pinfl)
                   : null
            });

            CombineStatuses(_eImzoService);
            if (HasErrors) return;

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
            if (HasErrors) return;

            dto.SignFile = SaveFile(doc.Id, timeStamp.Pkcs7b64, "sign.txt");
            dto.DataFile = SaveFile(doc.Id, JsonConvert.SerializeObject(doc), "data.txt");
            dto.SignedUserInfo = _authService.User.ToTextForDocumentLog();

            try
            {
                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanClaimApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
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

        public void Revoke(RevokeStatusClaimApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<Application>().Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanClaimApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                    else
                    {
                        ent.Application.StatusId = dto.StatusId;
                        ent.Application.Message = dto.Message;
                    }
                });

                CombineStatuses(Repository);
                if (HasErrors) return;
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

        public void Reject(RejectStatusClaimApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<Application>().Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanClaimApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                    else
                    {
                        ent.Application.StatusId = dto.StatusId;
                        ent.Application.Message = dto.Message;
                        ent.Application.CurrentStepId = StepIdConst.CLAIM_APPLICATION_REJECTED;
                    }
                });

                CombineStatuses(Repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(id: dto.Id, message: dto.Message);

                if (IsValid && canCommit)
                    transaction.Commit();
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }

        public void Accept(AcceptStatusClaimApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<Application>().Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanClaimApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
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

                var res = CreateDocumentChangeLog(dto.Id, dto.Message);
                if (IsValid && canCommit)
                    transaction.Commit();
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }

        public void Cancel(CancelStatusClaimApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<Application>().Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanClaimApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                    else
                    {
                        ent.Application.StatusId = dto.StatusId;
                        ent.Application.Message = dto.Message;
                        ent.Application.CurrentStepId = StepIdConst.CLAIM_APPLICATION_CANCEL;
                    }
                });

                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(id: dto.Id, message: dto.Message);

                if (IsValid && canCommit)
                    transaction.Commit();
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }

        public async ValueTask<byte[]> DownloadPdf(Guid id2, string? langu)
        {
           var   lang =  langu ?? "uz-latn";

            var languageId = UnitOfWork.Context.Set<Language>()
                .FirstOrDefault(l => l.Code == _cultureHelper.CurrentCulture.Code)?.Id ?? 1;


            MemoryStream wordFile = new MemoryStream();
            if (id2 == Guid.Empty)
            {
                wordFile = _storageService.GetStaticFile(
                    StaticFileConst.WordTemplate.GetFileName(
                       lang,
                        StaticFileConst.WordTemplate.CLAIM_APPLICATION_ADMINISTRATIVE_COURT)
                    );
                return await _pdfConverter.DocxToPdfAsync(wordFile, new object());
            }


            var claimApplication = await _unitOfWork.Context
                                   .Set<ClaimApplication>()
                                   .Include(w=>w.ClaimTheme).ThenInclude(c=>c.Translates)
                                   .Include(c => c.Application)
                                   .Include(x => x.Organization)
                                   .ThenInclude(a => a.Translates)
                                   .Include(c => c.ClaimApplicationType)
                                   .Include(c => c.Currency)
                                   .FirstOrDefaultAsync(c => c.Application.Id2 == id2);

            var tables = _unitOfWork.Context.Set<ClaimApplicationTable>()
                 .Include(t => t.ClaimResponsibleType)
                 .ThenInclude(t => t.Translates)
                 .Where(t => t.OwnerId == claimApplication.Id);

            if (claimApplication == null)
            {
                AddError("Ariza topilmadi / Заявление не найдено!");
                return null;
            }

            if (claimApplication.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APPLICATION_FOR_COURT && claimApplication.OrganizationId == OrganizationIdConst.SSP)
                wordFile = _storageService.GetStaticFile(
                    StaticFileConst.WordTemplate.GetFileName(
                       lang,
                        StaticFileConst.WordTemplate.CLAIM_APPLICATION_ADMINISTRATIVE_COURT_1SSP)
                    );

            else if (claimApplication.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APPLICATION_FOR_COURT)
                wordFile = _storageService.GetStaticFile(
                    StaticFileConst.WordTemplate.GetFileName(
                        lang,
                        StaticFileConst.WordTemplate.CLAIM_APPLICATION_ADMINISTRATIVE_COURT)
                    );

            else if ((claimApplication.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.CLAIM_APPLICATION || claimApplication.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.COUNTER_CLAIM) && claimApplication.OrganizationId == OrganizationIdConst.SSP)
                wordFile = _storageService.GetStaticFile(
                    StaticFileConst.WordTemplate.GetFileName(
                       lang,
                        StaticFileConst.WordTemplate.CLAIM_0R_COUNTERCLAIM_APPLICATION_SSP)
                    );


            else if (claimApplication.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.CLAIM_APPLICATION || claimApplication.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.COUNTER_CLAIM)
                wordFile = _storageService.GetStaticFile(
                   StaticFileConst.WordTemplate.GetFileName(
                       lang,
                       StaticFileConst.WordTemplate.CLAIM_0R_COUNTERCLAIM_APPLICATION)
                   );

            else if (claimApplication.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APILATION_CASSATION || claimApplication.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.CLAIM_REVISION)
                wordFile = _storageService.GetStaticFile(
                    StaticFileConst.WordTemplate.GetFileName(
                    lang,
                        StaticFileConst.WordTemplate.CLAIM_APPLICATION_APILATION_OR_CASSATION)
                    );
            if (wordFile == null)
            {
                AddError("Undefined document Type.");
                return null;
            }

            var contractor = await UnitOfWork.Context.Set<DataLayer.EfClasses.Contractor>()
                .Include(c => c.SettlementAccounts)
                .FirstOrDefaultAsync(c => c.Id == claimApplication.Application.ContractorId);

            var region = await UnitOfWork.Context.Set<Region>()
                .Include(r => r.Translates)
                .FirstOrDefaultAsync(r => r.Id == contractor.RegionId);

            var district = await UnitOfWork.Context.Set<District>()
                .Include(d => d.Translates)
                .FirstOrDefaultAsync(d => d.Id == contractor.DistrictId);

            var orgregion = await UnitOfWork.Context.Set<Region>()
                .Include(r => r.Translates)
                .FirstOrDefaultAsync(r => r.Id == claimApplication.Organization.RegionId);

            var bank = await UnitOfWork.Context.Set<DataLayer.EfClasses.Bank>()
                .Include(b => b.Translates)
                .FirstOrDefaultAsync(b => b.Id == contractor.BankId);

            var theme = await UnitOfWork.Context.Set<ClaimTheme>()
                .Include(r => r.Translates)
                .FirstOrDefaultAsync(t => t.Id == claimApplication.ClaimThemeId);

            var applicationForCourt = await UnitOfWork.Context.Set<ApplicationForCourt>()
                .Include(ac => ac.ClaimOrganization)
                .Include(afc => afc.Mediation)
                .ThenInclude(m => m.MediationPlan)
                .ThenInclude(mp => mp.Application)
                .ThenInclude(a => a.ClaimApplication)
                .AsSplitQuery()
                .FirstOrDefaultAsync(ac => claimApplication.Id == ac.Mediation.MediationPlan.Application.ClaimApplication.Id);

            var plh = new Placeholders();
            var link = _systemConf.QrImagePrintMy + "/ClaimApplication/DownloadPdf?id2=" + claimApplication.Application.Id2.ToString();
            var qrCode = new MemoryStream(QRCodeHelper.GeneratePng(link));
            plh.ImagePlaceholders.Add("QrCode", new() { Dpi = 512, MemStream = qrCode });

            plh.TextPlaceholders.Add(nameof(contractor.Region), region.Translates
                .FirstOrDefault(t => t.LanguageId == languageId
                    && t.ColumnName == TranslateColumn.full_name.ToString()
                    )?.TranslateText ?? region.FullName ?? "");

            plh.TextPlaceholders.Add(nameof(contractor.District), district.Translates
                .FirstOrDefault(t => t.LanguageId == languageId
                    && t.ColumnName == TranslateColumn.full_name.ToString()
                    )?.TranslateText ?? district.FullName ?? "");

            plh.TextPlaceholders.Add(nameof(contractor.Bank), bank == null ? "" : bank.Translates
                    .FirstOrDefault(t => t.LanguageId == languageId && t.ColumnName == BankTranslateColumn.bank_name.ToString())
                        ?.TranslateText ?? bank.BankName
                        + " "
                        + contractor.SettlementAccounts.FirstOrDefault(a => a.IsMain)?.AccountCode ?? "");

            plh.TextPlaceholders.Add(nameof(contractor.FullName), string.IsNullOrEmpty(claimApplication.BankBranchName) ? contractor.FullName : claimApplication.BankBranchName);
            plh.TextPlaceholders.Add(nameof(contractor.Address), contractor.Address ?? "");
            plh.TextPlaceholders.Add(nameof(contractor.Inn), contractor.Inn ?? "");
            plh.TextPlaceholders.Add("Organization", orgregion.Translates
                 .FirstOrDefault(t => t.LanguageId == languageId
                     && t.ColumnName == TranslateColumn.full_name.ToString()
                     )?.TranslateText ?? orgregion.FullName ?? "");
            plh.TextPlaceholders.Add(nameof(claimApplication.Application.DocNumber), claimApplication.Application.DocNumber ?? "");
            plh.TextPlaceholders.Add(nameof(claimApplication.Application.DocOn), claimApplication.Application.DocOn.ToString("dd.MM.yyyy") ?? "");

            plh.TextPlaceholders.Add(nameof(claimApplication.ClaimTheme),
                theme.Translates
                .FirstOrDefault(t => t.LanguageId == languageId
                    && t.ColumnName == TranslateColumn.full_name.ToString()
                    )?.TranslateText ?? theme.FullName ?? "");

            plh.TextPlaceholders.Add("CourtAppData", applicationForCourt?.DocOn.ToString("dd.MM.yyyy") ?? "");
            plh.TextPlaceholders.Add("CourtDocNumber", applicationForCourt?.DocNumber ?? "");

            plh.TextPlaceholders.Add("ClaimAppTypeName",
                claimApplication.ClaimApplicationType.Translates
                .FirstOrDefault(t => t.LanguageId == languageId
                    && t.ColumnName == TranslateColumn.full_name.ToString()
                    )?.TranslateText ?? claimApplication.ClaimApplicationType.FullName ?? "");

            plh.TextPlaceholders.Add(nameof(applicationForCourt.ClaimOrganization),
                applicationForCourt?.ClaimOrganization.Translates
                .FirstOrDefault(t => t.LanguageId == languageId
                    && t.ColumnName == TranslateColumn.full_name.ToString()
                    )?.TranslateText ?? applicationForCourt?.ClaimOrganization.FullName ?? "");

            plh.TextPlaceholders.Add(nameof(claimApplication.Tables), String.Join(", ", tables.Select(t => t.FullName)));

            var items = new List<Placeholders>();
            foreach (var item in tables)
            {
                var lplh = new Placeholders();
                lplh.TextPlaceholders.Add("ResponsibleName", item.FullName ?? "");
                lplh.TextPlaceholders.Add("ResponsibleAddress", item.Address ?? "");
                lplh.TextPlaceholders.Add("ResponsiblePhone", item.PhoneNumber ?? "");
                lplh.TextPlaceholders.Add("ResponsibleInn", item.InnOrPinfl ?? "");
                items.Add(lplh);
            }
            plh.TemplateListPlaceholders.Add(nameof(claimApplication.Tables), items);
            plh.TextPlaceholders.Add(nameof(claimApplication.TotalAmount), claimApplication.TotalAmount.ToString() ?? "");
            plh.TextPlaceholders.Add(nameof(claimApplication.MainDebt), claimApplication.MainDebt?.ToString() ?? "");
            plh.TextPlaceholders.Add(nameof(claimApplication.CalculedPenalty), claimApplication.CalculedPenalty?.ToString() ?? "");
            plh.TextPlaceholders.Add(nameof(claimApplication.Penalty), claimApplication.Penalty?.ToString() ?? "");
            plh.TextPlaceholders.Add(nameof(claimApplication.Percent), claimApplication.Percent?.ToString() ?? "");
            plh.TextPlaceholders.Add(nameof(claimApplication.Currency), claimApplication?.Currency.FullName ?? "");
            plh.TextPlaceholders.Add(nameof(claimApplication.CurrentPrincipalInterest), claimApplication?.CurrentPrincipalInterest?.ToString() ?? "");
            plh.TextPlaceholders.Add(nameof(claimApplication.CurrentInterestRate), claimApplication?.CurrentInterestRate?.ToString() ?? "");
            plh.TextPlaceholders.Add(nameof(claimApplication.OtherDebtRepayment), claimApplication?.OtherDebtRepayment?.ToString() ?? "");
            plh.TextPlaceholders.Add("ContractorDirector", string.IsNullOrEmpty(claimApplication.BankResponsiblePerson) ? contractor.Director : claimApplication.BankResponsiblePerson);

            wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();

            return await _pdfConverter.DocxToPdfAsync(wordFile, new());
        }

        public ClaimApplicationIntegrationForGetDto GetForFiles(long mediationId)
        {
            var applicationForCourt = _unitOfWork.Context.Set<ApplicationForCourt>().Select(a => new
            {
                a.MediationId,
                ApplicationForCourtStatus = a.Status.FullName,
                ApplicationForCourtStatusId = a.StatusId,
                a.Id2,
            }).FirstOrDefault(a => a.MediationId == mediationId);

            var mediation = _unitOfWork.Context.Set<Mediation>().Select(a => new
            {
                a.Id,
                a.MediationPlanId,
                a.Id2,
                MediationStatus = a.Status.FullName,
                MediationStatusId = a.StatusId
            }).FirstOrDefault(a => a.MediationPlanId == applicationForCourt.MediationId);

            var mediationPlan = _unitOfWork.Context.Set<MediationPlan>().Select(a => new
            {
                a.Application,
                a.Id,
                a.Id2,
                a.MeditionAt,
                ClaimApplicationId = a.Application.ClaimApplication.Id,
                MediatoinPlanStatus = a.Status.FullName,
                MediatoinPlanStatusId = a.StatusId
            }).FirstOrDefault(a => a.ClaimApplicationId == mediation.MediationPlanId);

            if (mediationPlan == null) return null;

            var claimApplication = _unitOfWork.Context.Set<ClaimApplication>().Include(f => f.Files).Select(a => new
            {
                a.Id,
                a.ApplicationId,
                Id2 = a.Application.Id2,
                ClaimApplicationStatus = a.Application.Status.FullName,
                ClaimApplicationStatusId = a.Application.StatusId
            }).FirstOrDefault(a => a.ApplicationId == mediationPlan.ClaimApplicationId);

            if (claimApplication == null) return null;

            ClaimApplicationIntegrationForGetDto result = new();

            result.ClaimApplicationStatus = claimApplication.ClaimApplicationStatus;
            result.claimApplicationIntegrationStatusId.ClaimApplicationStatusId = claimApplication.ClaimApplicationStatusId;
            result.claimApplicationIntegrationFileUrl.MediationLink = "https://erp-api.chamber.uz/Mediation/DownloadPdf?Id2=" + mediation.Id2 + "&lang=uz-cyrl";
            result.claimApplicationIntegrationFileUrl.MediationPlanLink = "https://erp-api.chamber.uz/MediationPlan/DownloadPdf?Id2=" + mediationPlan.Id2 + "&lang=uz-cyrl";

            if (mediationPlan != null)
            {
                result.claimApplicationIntegrationFileUrl.MediationLink = _systemConf.QrImagePrintPath + "/Mediation/DownloadPdf?Id2=" + mediation.Id2 + "&lang=uz-cyrl";
                result.claimApplicationIntegrationStatusId.ApplicationForCourtStatusId = applicationForCourt?.ApplicationForCourtStatusId;
                result.claimApplicationIntegrationFileUrl.ApplicationForCourtLink = "https://erp-api.chamber.uz/ApplicationForCourt/DownloadPdf?Id2=" + applicationForCourt?.Id2 + "&lang=uz-cyrl";
            }
            return result;
        }

        public void EmployeeAttachment(UpdateEmployeeAttechmentDlDto dto)
        {
            Repository.UpdateEmployeeAttachment(dto);

            CombineStatuses(Repository);

            if (IsValid)
                UnitOfWork.Save();
        }

        #region H E L P E R
        protected override IQueryable<ClaimApplicationListDto> SortFilter(IQueryable<ClaimApplicationListDto> query, ClaimApplicationSortFilterOptions options)
        {
            return base.SortFilter(query, options)
                .SortFilter(options)
                .Where(a => a.Application.ApplicationTypeId == ApplicationTypeIdConst.CLAIM);
        }

        private HaveId<long> CreateDocumentChangeLog(long id, string message = null, string userIp = null, string userAgent = null)
        {
            var entityDto = Repository.ById<ClaimApplicationDto>(id, applyFilter: false);
            _documentChangeLogService.CreateApplication(
                dto: entityDto,
                organizationId: null,
                message: message);
            CombineStatuses(_documentChangeLogService);

            if (HasErrors)
                return null;

            return HaveId.Create(id);
        }

        private void ClaimValidation<TDto>(ClaimApplicationDlDto<TDto> dto, ClaimApplication entity)
            where TDto : ClaimApplicationDlDto<TDto>
        {
            var query = Repository.AllAsQueryable;

            if (!_unitOfWork.Context.Set<MemshipContract>().Any(x => x.Id == dto.MemshipContractId
                && x.StatusId != StatusIdConst.DELETED))
            {
                Repository.AddError("A'zolik shartnomasi mavjud emas / Нет соглашения о членстве");
            }

            if (dto.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.APILATION_CASSATION
                || dto.ClaimApplicationTypeId == ClaimApplicationTypeIdConst.CLAIM_REVISION)
            {
                if (!dto.PrevApplicationId.HasValue)
                    Repository.AddError("Oldingi murojaatni tanlang / Выберите предыдущее заявление");
            }
        }

        //public async Task<ClaimApplicationIntegrationResponseDto> CreateFromIntegration(ClaimApplicationIntegrationRequestDto dto)
        //{
        //    var log = new CreateApiRequestLogDlDto
        //    {
        //        TableId = TableIdConst.CLAIM__DOC_CLAIM_APPLICATION,
        //        RequestContent = JsonConvert.SerializeObject(dto),
        //        ResponseAt = DateTime.Now,
        //    };

        //    try
        //    {
        //        var contractor = _context.Set<DataLayer.EfClasses.Contractor>()
        //            //.Include(a => a.BusinessmanUserInContractors)
        //            //    .ThenInclude(a => a.BusinessmanUser)
        //                .Include(a => a.Region)
        //                .Include(a => a.District)
        //            .FirstOrDefault(a => a.Inn == _integrationAuthService.User.Inn);
        //        //||
        //        //!contractor.BusinessmanUserInContractors.Any()
        //        // shuni qoshib qoyish kere banklar user yaratgandan keyin
        //        if (contractor == null)
        //        {
        //            AddError("(815) Сиз https://my.chamber.uz/ тизимдан рўйхатдан ўтмагансиз / Вы не зарегистрированы в системе https://my.chamber.uz/");
        //            log.IsSuccess = false;
        //            log.ResponseContent = "(817) Сиз https://my.chamber.uz/ тизимдан рўйхатдан ўтмагансиз / Вы не зарегистрированы в системе https://my.chamber.uz/";
        //            log.ResponseStatus = 400;
        //            return null;
        //        }

        //        var checkMemshipContract =
        //            _memshipContractRepository.IsThereMembershipContractForBank(_integrationAuthService.User.Inn);

        //        if (!checkMemshipContract.hasDocument)
        //        {
        //            AddError("(827) A'zolik shartnomasi mavjud emas / Нет соглашения о членстве");
        //            log.IsSuccess = false;
        //            log.ResponseContent = "A'zolik shartnomasi mavjud emas / Нет соглашения о членстве";
        //            log.ResponseStatus = 400;
        //            return null;
        //        }

        //        if (_unitOfWork.Context.Set<ClaimApplication>()
        //            .Any(ca => ca.ContractIdentificationNumber == dto.ContractIdentificationNumber))
        //        {
        //            AddError($"(837) Siz da'volik arizasi yaratib bo'lgansiz {dto.ContractIdentificationNumber} / Вы создали претензию {dto.ContractIdentificationNumber}");
        //            log.IsSuccess = false;
        //            log.ResponseContent = "Siz da'volik arizasi yaratib bo'lgansiz / Вы создали претензию";
        //            log.ResponseStatus = 400;
        //            return null;
        //        }

        //        var httpContext = _httpContextAccessor.HttpContext;

        //        httpContext.Response.Cookies.Append("contractor-id",
        //                                            contractor.Id.ToString());
        //        // banklar user qo'shilgandan keyin buni commentda olish kere
        //        //_authService.ResetUserName(contractor.BusinessmanUserInContractors.FirstOrDefault()
        //        //    .BusinessmanUser.UserName);

        //        /*int organizationId = _unitOfWork.Context.Set<MemshipContract>()
        //            .FirstOrDefault(a => a.Id == checkMemshipContract.documentId)
        //            .OrganizationId;*/

        //       // log.UserId = (int)_authService.UserId;
        //        log.UserInfo = JsonConvert.SerializeObject(new {info =  _integrationAuthService.User.UserName ?? "" });

        //        var storedFileIds = new List<string> { };
        //        Random rand = new Random();
        //        foreach (var file in dto.Document.Files)
        //        {
        //            var fileId = SaveFile($"{_integrationAuthService.User.Inn}_{rand.Next(10000)}", file.FileAsBase64);
        //            if (HasErrors)
        //            {
        //                AddError($"(866) Тизимда хатолик юз берди({String.Join(",\n", Errors)})");
        //                log.IsSuccess = false;
        //                log.ResponseContent = String.Join(",\n", Errors);
        //                log.ResponseStatus = 400;
        //                log.RequestAt = DateTime.Now;
        //                return null;
        //            }
        //            storedFileIds.Add(fileId);
        //        }


        //        /*var fileResult1 = _storageService.Save(DocumentStorageConst.DOC_CLAIM_APPLICATION_FILES, dto.OrganizationInn, storageFiles.ToArray());

        //        var fileResult = fileResult1.Select(a => new ClaimApplicationFileDto
        //        {
        //            Id = a.FileId,
        //            FileName = a.FileName,
        //            CreatedAt = DateTime.Now
        //        });*/


        //        CombineStatuses(_storageService);

        //        var createDto = new CreateClaimApplicationDlDto(contractor)
        //        {
        //            OrganizationId = dto.OrganizationId,
        //            ClaimApplicationTypeId = dto.Document.ClaimApplicationTypeId,
        //            Details = dto.Document.Details,
        //            MemshipContractId = checkMemshipContract.documentId,
        //            //DocOn = DateTime.Now.AsDateOnly(),
        //            //ContractorPositionName = "Директор",
        //            ClaimThemeId = dto.Document.ClaimThemeId,
        //            CurrencyId = dto.Document.CurrencyId,
        //            MainDebt = dto.Document.MainDebt,
        //            CalculedPenalty = dto.Document.CalculedPenalty,
        //            Penalty = dto.Document.Penalty,
        //            Percent = dto.Document.Percent,
        //            CurrentPrincipalInterest = dto.Document.CurrentPrincipalInterest,
        //            CurrentInterestRate = dto.Document.CurrentInterestRate,
        //            OtherDebtRepayment = dto.Document.OtherDebtRepayment,
        //            ContractIdentificationNumber = dto.ContractIdentificationNumber,
        //            //ApplicationId = checkMemshipContract.applicationId,
        //            Application = new ApplicationDlDto
        //            {
        //                ApplicationTypeId = ApplicationTypeIdConst.CLAIM,
        //                DocOn = DateTime.Now.AsDateOnly(),
        //                DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_APPLICATION, 1).Item2,
        //                ContractorPositionName = "test",
        //            },
        //            /*Application = _unitOfWork.ApplicationRepository.CrudServices.ReadManyNoTracked<ApplicationDto>().FirstOrDefault(a => a.Id == checkMemshipContract.applicationId),*/
        //            //DocNumber = "test",
        //            Files = storedFileIds.Select(fileId => new ClaimApplicationFileDlDto
        //            {
        //                Id = new Guid(fileId)
        //            }).ToList()
        //        };

        //        int orderNumber = 1;

        //        createDto.Tables = dto.Document.Tables
        //            .Select(a =>
        //            {
        //                var tableDto = new ClaimApplicationTableDlDto
        //                {
        //                    Address = a.Address,
        //                    ClaimResponsibleTypeId = a.ClaimResponsibleTypeId,
        //                    FullName = a.FullName,
        //                    InnOrPinfl = a.InnOrPinfl,
        //                    OrderNumber = orderNumber.ToString(),
        //                    PhoneNumber = a.PhoneNumber,
        //                };

        //                orderNumber++;

        //                return tableDto;
        //            })
        //            .ToList();

        //        var entity = Create(createDto);

        //        if (HasErrors)
        //        {
        //            AddError($"(943) Тизимда хатолик юз берди {String.Join(",\n", Errors)}");
        //            log.IsSuccess = false;
        //            log.ResponseContent = String.Join(",\n", Errors);
        //            log.ResponseStatus = 400;
        //            log.RequestAt = DateTime.Now;
        //            return null;
        //        }

        //        var result = new ClaimApplicationIntegrationResponseDto
        //        {
        //            Id = entity.Id,
        //            ContractIdentificationNumber = createDto.ContractIdentificationNumber
        //        };
        //        log.ResponseStatus = 200;
        //        log.ResponseContent = JsonConvert.SerializeObject(result);
        //        log.RequestAt = DateTime.Now;

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        AddError($"(960) Тизимда хатолик юз берди ({ex.InnerException}||{ex.Message}||{ex.InnerException})");
        //        log.IsSuccess = false;
        //        log.ResponseContent = "Тизимда хатолик юз берди";
        //        log.ResponseStatus = 400;
        //        log.Exception = $"{ex.Message}: {ex.InnerException}";
        //        return null;
        //    }
        //    finally
        //    {
        //        log.RequestAt = DateTime.Now;
        //        _apiRequestLogRepository.Create(log);
        //        CombineStatuses(_apiRequestLogRepository);
        //        //if (HasErrors)
        //            _unitOfWork.Save();
        //    }
        //}

        private string SaveFile(string documentId, string data)
        {
            try
            {

                var ms = new MemoryStream();

                var writer = new StreamWriter(ms);

                writer.Write(data);
                writer.Flush();
                ms.Position = 0;

                var fileInfo = _storageService.Save(DocumentStorageConst.DOC_CLAIM_APPLICATION_FILES,
                                                    documentId.ToString(),
                                                    new StorageFile(Guid.NewGuid(),
                                                                    $"{documentId}.pdf",
                                                                    ms)).FirstOrDefault()!;

                CombineStatuses(_storageService);

                if (HasErrors)
                {
                    return null!;
                }

                return fileInfo.FileId.ToString();
            }
            catch (Exception ex)
            {
                AddError($"Message: {ex.Message} , StackTrace: {ex.StackTrace}");
                return null!;
            }
        }

        private string GetFileDataFromMinio(string documentId, Guid fileId)
        {
            try
            {
                var fileInfo = _storageService.GetFile(DocumentStorageConst.DOC_CLAIM_APPLICATION_FILES,
                                                       documentId,
                                                       fileId);

                CombineStatuses(_storageService);

                if (HasErrors)
                    return null!;

                Stream fileStream = fileInfo.GetStream();


                string existingRequestDataFromMinio = String.Empty;

                using (StreamReader reader = new StreamReader(fileStream))
                    existingRequestDataFromMinio = reader.ReadToEnd();


                return existingRequestDataFromMinio;
            }
            catch (Exception ex)
            {
                AddError($"Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                return null!;
            }

        }

        private Guid SaveFile(long docId, string data, string fileName)
        {
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);

            writer.Write(data);
            writer.Flush();
            ms.Position = 0;

            var fileInfo = _storageService.Save(
                $"{nameof(TableIdConst.CLAIM__DOC_CLAIM_APPLICATION)}_SIGN_DATA",
                docId.ToString(),
                new StorageFile(Guid.NewGuid(), fileName, ms));

            return fileInfo.FirstOrDefault().FileId;
        }

        private IQueryable<TDto> GetQuery<TDto>() where TDto : class
        {
            return Repository.ReadAsNoTracked<TDto>();
        }
        #endregion

        #region F I L E S
        public IEnumerable<ClaimApplicationFileDto> UploadFiles(params StorageFile[] files)
        {
            var result = _storageService.SaveTemp(DocumentStorageConst.DOC_CLAIM_APPLICATION_FILES, files).Select(a => new ClaimApplicationFileDto
            {
                Id = a.FileId,
                FileName = a.FileName,
                CreatedAt = DateTime.Now
            });
            CombineStatuses(_storageService);
            return IsValid ? result : null;
        }

        public StorageFile DownloadFile(Guid fileId)
        {
            var entity = _unitOfWork.Context.Set<ClaimApplicationFile>()
                .Include(a => a.Owner)
                .FirstOrDefault(a => a.Id == fileId);


            return Download(fileId, entity, DocumentStorageConst.DOC_CLAIM_APPLICATION_FILES);
        }

        public void DeleteFile(Guid fileId)
        {
            var entity = _unitOfWork.Context
                .Set<ClaimApplicationFile>()
                .FirstOrDefault(a => a.Id == fileId);

            Delete(fileId, entity, DocumentStorageConst.DOC_CLAIM_APPLICATION_FILES);
        }

        private void Delete(Guid fileId, FileEntity<long> entity, string storageDocument)
        {
            if (entity != null)
            {
                _storageService.DeleteTemp(storageDocument, fileId);
                CombineStatuses(_storageService);
            }
        }

        private StorageFile Download(Guid fileId, ClaimApplicationFile entity, string storageDocument)
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
        #endregion

        public MemoryStream FindWordFile(int regionId, string lang)
        {
            MemoryStream wordFile = new MemoryStream();

            if (regionId == 1)
            {
                wordFile = _storageService.GetStaticFile(
               StaticFileConst.WordTemplate.GetFileName(
                   lang,
                   StaticFileConst.WordTemplate.CLAIM_0R_COUNTERCLAIM_APPLICATION_TOSHKENT_SHAHRI)
               );
            }
            else if (regionId == 2)
            {
                wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                    lang,
                    StaticFileConst.WordTemplate.CLAIM_0R_COUNTERCLAIM_APPLICATION_TOSHKENT_VILOYATI)
                );
            }
            else if (regionId == 3)
            {
                wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                    lang,
                    StaticFileConst.WordTemplate.CLAIM_0R_COUNTERCLAIM_APPLICATION_ANDIJON)
                );
            }
            else if (regionId == 4)
            {
                wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                    lang,
                    StaticFileConst.WordTemplate.CLAIM_0R_COUNTERCLAIM_APPLICATION_BUXORO)
                );
            }
            else if (regionId == 5)
            {
                wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                    lang,
                    StaticFileConst.WordTemplate.CLAIM_0R_COUNTERCLAIM_APPLICATION_JIZZAX)
                );
            }
            else if (regionId == 6)
            {
                wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                    lang,
                    StaticFileConst.WordTemplate.CLAIM_0R_COUNTERCLAIM_APPLICATION_QORAQALPOGISTON)
                );
            }
            else if (regionId == 7)
            {
                wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                    lang,
                    StaticFileConst.WordTemplate.CLAIM_0R_COUNTERCLAIM_APPLICATION_QASHQADARYO)
                );
            }
            else if (regionId == 8)
            {
                wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                    lang,
                    StaticFileConst.WordTemplate.CLAIM_0R_COUNTERCLAIM_APPLICATION_NAVOIY)
                );
            }
            else if (regionId == 9)
            {
                wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                    lang,
                    StaticFileConst.WordTemplate.CLAIM_0R_COUNTERCLAIM_APPLICATION_NAMANGAN)
                );
            }
            else if (regionId == 10)
            {
                wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                    lang,
                    StaticFileConst.WordTemplate.CLAIM_0R_COUNTERCLAIM_APPLICATION_SAMARQAND)
                );
            }
            else if (regionId == 11)
            {
                wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                    lang,
                    StaticFileConst.WordTemplate.CLAIM_0R_COUNTERCLAIM_APPLICATION_SURXONDARYO)
                );
            }
            else if (regionId == 12)
            {
                wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                    lang,
                    StaticFileConst.WordTemplate.CLAIM_0R_COUNTERCLAIM_APPLICATION_SIRDARYO)
                );
            }
            else if (regionId == 13)
            {
                wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                    lang,
                    StaticFileConst.WordTemplate.CLAIM_0R_COUNTERCLAIM_APPLICATION_FARGONA)
                );
            }
            else if (regionId == 14)
            {
                wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                    lang,
                    StaticFileConst.WordTemplate.CLAIM_0R_COUNTERCLAIM_APPLICATION_XORAZM)
                );
            }
            return wordFile;
        }

        public bool UpdateClaimApplicationTable(long id, string newAddress)
        {
            var claimApplicationTable = _unitOfWork.Context.Set<ClaimApplicationTable>().FirstOrDefault(a => a.Id == id);
            if (claimApplicationTable == null)
            {
                AddError("Da'vo arizasi ma'lumoti topilmadi!");
                return false;
            }

            claimApplicationTable.Address = newAddress;
            claimApplicationTable.ModifiedAt = DateTime.Now;

            _unitOfWork.Save();
            return true;
        }
    }
}
