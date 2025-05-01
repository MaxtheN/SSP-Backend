using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using SspUis.BizLogicLayer.ApplicationServices;
using SspUis.BizLogicLayer.BankServices;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.Doc;
using SspUis.BizLogicLayer.Doc.PrtnContractServices;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.Notify;
using SspUis.BizLogicLayer.OrganizationServices;
using SspUis.BizLogicLayer.RegionServices;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.ServiceLayer.NumberServices;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.PrtnContractServices
{
    public class PrtnContractService
        : BaseEntityService<long, PrtnContract, PrtnContractListDto, PrtnContractDto, CreatePrtnContractDlDto, UpdatePrtnContractDlDto, IPrtnContractRepository, PrtnDocumentSortFilterOptions>
        , IPrtnContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IApplicationService _applicationService;
        private readonly IDocumentChangeLogService _documentChangeLogService;
        private readonly INumberService _numberService;
        private readonly IEImzoService _eImzoService;
        private readonly IStorageService _storageService;
        private readonly IBaseReportService _baseReportService;
        private readonly IOrganizationService _organizationService;
        private readonly ICultureHelper _cultureHelper;
        private readonly ISendSmsService _sendSmsService;
        public PrtnContractService(
            IUnitOfWork unitOfWork,
            IAuthService authService,
               ICultureHelper cultureHelper,
            IApplicationService applicationService,
            IDocumentChangeLogService documentChangeLogService,
            INumberService numberService,
            IEImzoService eImzoService,
            IStorageService storageService,
            IBaseReportService baseReportService,
            IOrganizationService organizationService,
            ISendSmsService sendSmsService)
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _applicationService = applicationService;
            _documentChangeLogService = documentChangeLogService;
            _numberService = numberService;
            _eImzoService = eImzoService;
            _cultureHelper = cultureHelper;
            _storageService = storageService;
            _baseReportService = baseReportService;
            _organizationService = organizationService;
            _sendSmsService = sendSmsService;
        }

        protected override IQueryable<PrtnContractListDto> SortFilter(IQueryable<PrtnContractListDto> query, PrtnDocumentSortFilterOptions options)
        {
            if(options.StatusId == 27 && options.PrtnContractTypeId == 3)
            {
                Organization organization = _unitOfWork.Context.Set<Organization>().FirstOrDefault(o => o.Id == 177);
                return base.SortFilter(query, options).SortFilter(options, organization);
            }

            return base.SortFilter(query, options).SortFilter(options);
        }

        public override PrtnContractDto Get(long id)
        {
            var dto = base.Get(id);
            if (dto == null)
            {
                AddError("Bunday document mavjud emas !");
                return null;
            }

            var currentSigner = dto.Signs.FirstOrDefault(a => a.Id == dto.CurrentPrtnContractSignId);

            if (currentSigner != null)
            {
                if (currentSigner.SignOrganizationTypeId == SignOrganizationTypeIdConst.BUSINESSMAN)
                {
                    dto.CanSign = _authService.Contractor != null
                        && dto.ContractorId == _authService.Contractor.Id
                        && StatusIdConst.CanContractApplyStatus(dto.StatusId, StatusIdConst.SIGNING);
                }
                else
                {
                    if (dto.PrtnContractTypeId == PrtnContractTypeIdConst._201__)
                        dto.CanSign = _authService.Contractor == null && _authService.HasPermission(ModuleCode.PrtnContractSign) && StatusIdConst.CanContractApplyStatus(dto.StatusId, StatusIdConst.SIGNING)
                            && dto.Signs.Any(a => !a.IsSigned && a.OrganizationSignPinfl == _authService.User.Pinfl);
                    else
                        dto.CanSign = _authService.Contractor == null
                            && currentSigner.OrganizationId == _authService.Organization.Id
                            && currentSigner.OrganizationSignPinfl == _authService.User.Pinfl
                            && _authService.HasPermission(ModuleCode.PrtnContractSign)
                            && StatusIdConst.CanContractApplyStatus(dto.StatusId, StatusIdConst.SIGNING);
                }
            }

            dto.CanCancel = StatusIdConst.CanContractApplyStatus(dto.StatusId, StatusIdConst.CANCELED)
                && _authService.HasPermission(ModuleCode.PrtnContractCancel);
            dto.CanReject = StatusIdConst.CanContractApplyStatus(dto.StatusId, StatusIdConst.REJECTED)
                && _authService.HasPermission(ModuleCode.PrtnContractReject);
            dto.CanPassExpertise = StatusIdConst.CanContractApplyStatus(dto.StatusId, StatusIdConst.PASS_EXPERTISE)
                && _authService.HasPermission(ModuleCode.PrtnContractPassExpertise);
            dto.CanNotPassExpertise = StatusIdConst.CanContractApplyStatus(dto.StatusId, StatusIdConst.NOT_PASS_EXPERTISE)
                && _authService.HasPermission(ModuleCode.PrtnContractNotPassExpertise);
            dto.ReSendForExpertise = dto.StatusId == StatusIdConst.NOT_PASS_EXPERTISE
                || dto.StatusId == StatusIdConst.PASS_EXPERTISE
                || dto.StatusId == StatusIdConst.SIGNING
             && _authService.HasPermission(ModuleCode.PrtnContractReSendForExpertise);
            return dto;
        }

        public SelectList<long> AsSelectList()
        {
            return Repository.ReadAsNoTracked<PrtnContractListDto>().AsSelectList();
        }

        public PrtnContractDto GetByApplicationId(long applicationId)
        {
            var application = _applicationService.GetPrtnApplication(applicationId);
            var dto = base.Get();
            dto.ApplicationDocOn = application.DocOn;
            dto.ApplicationDocNumber = application.DocNumber;
            dto.DocOn = DateTime.Now.AsDateOnly();
            dto.DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_PRTN_CONTRACT, 1).Item2;
            dto.ContractorId = application.ContractorId;
            dto.PrtnContractTypeId = application.PrtnContractTypeId;
            dto.NewVacanciesCount = application.NewVacanciesCount;
            dto.Contractor = application.Contractor;
            dto.ContractorInn = application.ContractorInn;
            dto.PrtnContractType = application.PrtnContractType;
            dto.ApplicationId = applicationId;


            var orgQuery = _unitOfWork.OrganizationRepository.AllAsQueryable
                .IsActive();

            if (application.PrtnContractTypeId == PrtnContractTypeIdConst._50_100)
            {
                orgQuery = orgQuery.Where(a => a.DistrictId == (application.ChooseLocation ? application.ChoosedDistrictId : application.DistrictId)
                    && a.RegionId == (application.ChooseLocation ? application.ChoosedRegionId : application.RegionId)
                    && a.SignOrganizationTypeId == SignOrganizationTypeIdConst.DISTRICT
                );
            }
            else if (application.PrtnContractTypeId == PrtnContractTypeIdConst._101_200)
            {
                orgQuery = orgQuery.Where(a => a.RegionId == (application.ChooseLocation ? application.ChoosedRegionId : application.RegionId)
                    && a.SignOrganizationTypeId == SignOrganizationTypeIdConst.REGION
                );
            }
            else if (application.PrtnContractTypeId == PrtnContractTypeIdConst._201__)
            {
                orgQuery = orgQuery.Where(a => a.SignOrganizationTypeId == SignOrganizationTypeIdConst.MINISTRY);
            }

            var organizationId = orgQuery.FirstOrDefault()?.Id;
            if (!organizationId.HasValue)
            {
                AddError($"{application.PrtnContractType} uchun tashkilot topilmadi / Организация для {application.PrtnContractType} не найдено");
                return null;
            }
            dto.OrganizationId = organizationId.Value;

            return dto;
        }

        public override HaveId<long> Create(CreatePrtnContractDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var application = _applicationService.GetPrtnApplication(dto.ApplicationId);
                if (application == null)
                {
                    AddError("Ariza topilmadi / Заявления не найдено");
                    return null;
                }

                var orgQuery = _unitOfWork.OrganizationRepository.AllAsQueryable
                    .IsActive();

                if (application.PrtnContractTypeId == PrtnContractTypeIdConst._50_100)
                {
                    orgQuery = orgQuery.Where(a => a.DistrictId == (application.ChooseLocation ? application.ChoosedDistrictId : application.DistrictId)
                        && a.RegionId == (application.ChooseLocation ? application.ChoosedRegionId : application.RegionId)
                        && a.SignOrganizationTypeId == SignOrganizationTypeIdConst.DISTRICT
                    );
                }
                else if (application.PrtnContractTypeId == PrtnContractTypeIdConst._101_200)
                {
                    orgQuery = orgQuery.Where(a => a.RegionId == (application.ChooseLocation ? application.ChoosedRegionId : application.RegionId)
                        && a.SignOrganizationTypeId == SignOrganizationTypeIdConst.REGION
                    );
                }
                else if (application.PrtnContractTypeId == PrtnContractTypeIdConst._201__)
                {
                    orgQuery = orgQuery.Where(a => a.SignOrganizationTypeId == SignOrganizationTypeIdConst.MINISTRY);
                }

                var organizationId = orgQuery.FirstOrDefault()?.Id;
                if (!organizationId.HasValue)
                {
                    AddError($"{application.PrtnContractType} uchun tashkilot topilmadi / Организация для {application.PrtnContractType} не найдено");
                    return null;
                }

                _applicationService.Accept(dto.ApplicationId);
                CombineStatuses(_applicationService);
                if (HasErrors)
                    return null;

                var ent = Repository.Create(dto, ent => Validation(dto, ent));
                if (HasErrors)
                    return null;
                ent.OrganizationId = organizationId.Value;
                ent.PrtnContractTypeId = application.PrtnContractTypeId;
                ent.NewVacanciesCount = application.NewVacanciesCount;
                ent.IsRead = false;

                #region Set signers
                var prtnContractTypeTables = _unitOfWork.PrtnContractTypeRepository.ById(dto.PrtnContractTypeId)?.Tables;
                List<PrtnContractTypeTable> signers = prtnContractTypeTables.ToList();
                bool existCurrentRegion = prtnContractTypeTables.Any(a => a.RegionId == _authService.Organization.RegionId);
                if (existCurrentRegion)
                    signers = prtnContractTypeTables.Where(a => a.SignOrganizationTypeId == SignOrganizationTypeIdConst.BUSINESSMAN
                        || a.RegionId == _authService.Organization.RegionId
                    ).ToList();
                else
                    signers = prtnContractTypeTables.Where(a => a.SignOrganizationTypeId == SignOrganizationTypeIdConst.BUSINESSMAN
                        || !a.RegionId.HasValue
                    ).ToList();

                foreach (var signer in signers.OrderBy(a => a.OrderNumber))
                {
                    if (signer.SignOrganizationTypeId == SignOrganizationTypeIdConst.BUSINESSMAN)
                        ent.Signs.Add(new PrtnContractSign
                        {
                            IsSigned = false,
                            PrtnContractTypeTableId = signer.Id,
                        });
                    else if (signer.SignOrganizationTypeId == SignOrganizationTypeIdConst.MINISTRY)
                    {
                        int signerOrganizationId = organizationId.Value;
                        if (dto.PrtnContractTypeId == PrtnContractTypeIdConst._201__)
                        {
                            // Agar vazirlik qaysi tashkilot ekanligi ko'rsatilmagan bo'sa, oshibka beradi
                            if (!signer.OrganizationId.HasValue)
                            {
                                AddError("Vazirliklar shartnoma turida hali ko'rsatilmagan");
                                return null;
                            }

                            signerOrganizationId = signer.OrganizationId.Value;
                        }

                        var signerOrganization = _unitOfWork.OrganizationRepository.ById(signerOrganizationId);
                        var organizationSignId = signerOrganization.Signs
                                .FirstOrDefault(a => a.PrtnContractTypeTableId == signer.Id && !a.ExpireOn.HasValue);
                        if (organizationSignId == null)
                        {
                            var orgName = signerOrganization.Translates.AsQueryable().FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? signerOrganization.FullName;
                            AddError($"{orgName} tashkilot sozlamalarida vazir ko'rsatilmagan / Министр не указан в настройках организации {orgName}");
                            return null;
                        }

                        ent.Signs.Add(new PrtnContractSign
                        {
                            IsSigned = false,
                            PrtnContractTypeTableId = signer.Id,
                            OrganizationId = signer.OrganizationId,
                            OrganizationSignId = organizationSignId.Id,
                        });
                    }
                    else
                    {
                        int signerOrganizationId = organizationId.Value;
                        if (dto.PrtnContractTypeId == PrtnContractTypeIdConst._201__)
                        {
                            // Agar vazirlik qaysi tashkilot ekanligi ko'rsatilmagan bo'sa, oshibka beradi
                            if (!signer.OrganizationId.HasValue)
                            {
                                AddError("Vazirliklar shartnoma turida hali ko'rsatilmagan");
                                return null;
                            }

                            signerOrganizationId = signer.OrganizationId.Value;
                        }

                        var signerOrganization = _unitOfWork.OrganizationRepository.ById(signerOrganizationId);
                        var organizationSignId = signerOrganization.Signs
                                .FirstOrDefault(a => a.PrtnContractTypeTableId == signer.Id && !a.ExpireOn.HasValue);
                        if (organizationSignId == null)
                        {
                            var orgName = signerOrganization.Translates.AsQueryable().FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? signerOrganization.FullName;
                            AddError($"{orgName} tashkilot sozlamalarida imzolovchilar ko'rsatilmagan / Подписант не указан в настройках организации {orgName}");
                            return null;
                        }
                        ent.Signs.Add(new PrtnContractSign
                        {
                            IsSigned = false,
                            PrtnContractTypeTableId = signer.Id,
                            OrganizationId = organizationId.Value,
                            OrganizationSignId = organizationSignId.Id
                        });
                    }
                }
                #endregion

                ent.Application.PrtnContract.StatusChangeExpireOn = DateTime.Now;
                CombineStatuses(Repository);
                if (HasErrors)
                    return null;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(ent.Id, StatusIdConst.CREATED);
                if (IsValid)
                {
                    transaction.Commit();
                }

                return res;
            }
        }

        public override void Update(UpdatePrtnContractDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                Repository.AllAsQueryable.Lock(dto.Id);

                var ent = Repository.Update(dto, ent => Validation(dto, ent));
                CombineStatuses(Repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(ent.Id, StatusIdConst.MODIFIED);
                if (IsValid)
                {
                    transaction.Commit();
                }
            }
        }

		public int GetCount()
		{
            PrtnDocumentSortFilterOptions prtnDocumentSortFilterOptions = new PrtnDocumentSortFilterOptions();
            return GetList(prtnDocumentSortFilterOptions).Rows.Count();
		}
		public override void Delete(long id)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                Repository.AllAsQueryable.Lock(id);
                var dto = new UpdateStatusPrtnContractDlDto { Id = id, StatusId = StatusIdConst.DELETED, IsRead = false}; 

                var ent = Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanDeleteStatuses.Contains(ent.StatusId))
                        AddError("O'chirish mumkin emas / Невозможно удалить");
                });
                CombineStatuses(Repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(ent.Id, StatusIdConst.DELETED);
                if (IsValid)
                {
                    transaction.Commit();
                }
            }
        }

        public void Revoke(RevokeStatusPrtnContractDto dto)
        {
         
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                Repository.AllAsQueryable.Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanContractApplyStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");

                    ent.IsRead = false;
                });
                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(dto.Id, dto.StatusId, dto.Message);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                }
                _unitOfWork.Save();

                if (IsValid)
                    transaction.Commit();
            }
        }

        public void NotPassExpertise(NotPassExpertiseStatusPrtnContractDto dto)
        {
            if (dto.Files == null || !dto.Files.Any())
                AddError("Файл бириктирилмаган / Файл не прикреплен");

            //var entityfileIds = _unitOfWork.Context.Set<PrtnContractFile>().Where(a => a.OwnerId == dto.Id).Select(a => a.Id).ToList();

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                Repository.AllAsQueryable.Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanContractApplyStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");

                    dto.IsRead = false;
                    ent.NotPassExpertiseExpireOn = DateTime.Now;
                });
                CombineStatuses(Repository);

                if (HasErrors)
                    return;

                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(dto.Id, dto.StatusId, dto.Message);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                }
                _unitOfWork.Save();

                _storageService.MoveToPersistent(DocumentStorageConst.DOC_PRTN_CONTRACT_FILES, dto.Id.ToString(), dto.Files.Select(a => a.Id).ToArray());
                CombineStatuses(_storageService);

                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                }

                if (IsValid)
                    transaction.Commit();
            }
        }

        public async Task ResendExpertise(ResendExpertiseStatusPrtnContractDto dto) 
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                Repository.AllAsQueryable.Lock(dto.Id);

                try
                {
                    var prtnContract = Repository.ById(dto.Id);

                    if (HasErrors || prtnContract == null)
                    {
                        transaction.Rollback();
                        return;
                    }

                    //////// GRAPHS
                    var graphs = _unitOfWork.Context.Set<PrtnApplication>().Include(a => a.Graphs)
                        .FirstOrDefault(app => app.ApplicationId == prtnContract.ApplicationId).Graphs;

                    var prtnApplication = _unitOfWork.Context.Set<PrtnApplication>().Include(a => a.Graphs).FirstOrDefault(app => app.ApplicationId == prtnContract.ApplicationId);
                    var now = DateTime.Now.AddMonths(1);

                    var totalNewVacanciesCount = graphs
                        .Where(gr => gr.YearIn < now.Year || (gr.YearIn == now.Year && gr.MonthIn <= now.Month))
                        .Sum(gr => gr.NewVacanciesCount);

                    var currentMonthGraph = graphs.FirstOrDefault(gr => gr.YearIn == now.Year && gr.MonthIn == now.Month);
                    if (currentMonthGraph == null)
                    {
                        prtnApplication.Graphs.Add(new PrtnApplicationGraph
                        {
                            YearIn = now.Year,
                            MonthIn = now.Month,
                            NewVacanciesCount = totalNewVacanciesCount,
                        });
                    }
                    else if (currentMonthGraph != null)
                    {
                        currentMonthGraph.NewVacanciesCount = totalNewVacanciesCount;
                    }

                    foreach (var graph in graphs.Where(gr => gr.YearIn < now.Year || (gr.YearIn == now.Year && gr.MonthIn < now.Month)))
                        graph.NewVacanciesCount = 0;


                    ////////// ORGANIZATION
                    var org = _unitOfWork.Context.Set<Organization>()
                        .Include(x => x.Signs)
                        .FirstOrDefault(x => x.Id == prtnContract.OrganizationId);

                    if (org == null)
                    { AddError("Bunday organization mavjud emas !"); return; }
                    var signsDictionary = org.Signs.OrderByDescending(x => x.Id).ToDictionary(sign => sign.Id);

                    foreach (var prtnSign in prtnContract.Signs)
                    {
                        prtnSign.IsSigned = false;
                        if (prtnSign.OrganizationSignId != null)
                        {
                            var orgSignDict = signsDictionary.ContainsKey((int)prtnSign.OrganizationSignId)
                                ? signsDictionary[(int)prtnSign.OrganizationSignId]
                                : null;

                            if (orgSignDict == null)
                                continue;

                            if (orgSignDict.ExpireOn.HasValue && orgSignDict.ExpireOn < DateTime.Today.AsDateOnly())
                            {
                                var orgSign = signsDictionary.FirstOrDefault(x => !x.Value.ExpireOn.HasValue && x.Value.PrtnContractTypeTableId == prtnSign.PrtnContractTypeTableId).Value;

                                if (orgSign != null && orgSign.PrtnContractTypeTableId == prtnSign.PrtnContractTypeTableId)
                                {
                                    prtnContract.DocOn = DateTime.Today.AsDateOnly();
                                    prtnSign.OrganizationSignId = orgSign.Id;
                                }
                            }
                        }
                    }

                    // Create a dictionary with SignsId as the key and Sign object as the value
                    Repository.UpdateStatus(dto, ent =>
                    {
                        if (!StatusIdConst.CanContractApplyStatus(ent.StatusId, dto.StatusId))
                            AddError("Имкони йўқ / Нет доступа");
                        dto.IsRead = false;
                        ent.DocOn = DateTime.Today.AsDateOnly();
                    });
                    CombineStatuses(Repository);
                    if (HasErrors)
                        return;

                    prtnContract.ResendExpertiseExpireOn = DateTime.Now;

                    _unitOfWork.Save();

                    var res = CreateDocumentChangeLog(dto.Id, dto.StatusId, dto.Message);
                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return;
                    }
                    _unitOfWork.Save();

                    var phones = UnitOfWork.Context.Set<BusinessmanUserInContractor>()
                        .Include(x => x.BusinessmanUser)
                        .Where(x => x.ContractorId == prtnContract.ContractorId)
                        .Select(x => x.BusinessmanUser.UserName).ToList();

                    foreach (var phone in phones)
                        await _sendSmsService.SendSms(TableIdConst.DOC_PRTN_CONTRACT, StatusIdConst.SIGNING, StatusIdConst.SENT_FOR_EXPERTISE, phone);

                    if (IsValid)
                        transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }

            }
        }

        public async Task PassExpertise(PassExpertiseStatusPrtnContractDto dto)
        {
            if (dto.Files == null || !dto.Files.Any())
            {
                AddError("Файл бириктирилмаган / Файл не прикреплен");
                return;
            }
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                Repository.AllAsQueryable.Lock(dto.Id);

                var entity = Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanContractApplyStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");

                    dto.IsRead = false;
                });

                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                entity.PassExpertiseExpireOn = DateTime.Now;

                _storageService.MoveToPersistent(DocumentStorageConst.DOC_PRTN_CONTRACT_FILES, dto.Id.ToString(), dto.Files.Select(a => a.Id).ToArray());
                CombineStatuses(_storageService);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                }

                var res = CreateDocumentChangeLog(dto.Id, dto.StatusId, dto.Message);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                }
                _unitOfWork.Save();

                var phones = UnitOfWork.Context.Set<BusinessmanUserInContractor>()
                        .Include(x => x.BusinessmanUser)
                        .Where(x => x.ContractorId == entity.ContractorId)
                        .Select(x => x.BusinessmanUser.UserName).ToList();

                foreach (var phone in phones)
                    await _sendSmsService.SendSms(TableIdConst.DOC_PRTN_CONTRACT, StatusIdConst.SENT_FOR_EXPERTISE, StatusIdConst.PASS_EXPERTISE, phone);

                if (IsValid)
                    transaction.Commit();
            }
        }

        #region Files
        public IEnumerable<PrtnContractFileDto> UploadFiles(params StorageFile[] files)
        {
            var result = _storageService.SaveTemp(DocumentStorageConst.DOC_PRTN_CONTRACT_FILES, files).Select(f => new PrtnContractFileDto
            {
                Id = f.FileId,
                FileName = f.FileName
            });
            CombineStatuses(_storageService);
            return IsValid ? result : null;
        }

        public StorageFile DownloadFile(Guid fileId)
        {
            var entity = _unitOfWork.Context
                .Set<PrtnContractFile>()
                .FirstOrDefault(a => a.Id == fileId);

            return Download(fileId, entity, DocumentStorageConst.DOC_PRTN_CONTRACT_FILES);
        }

        public void DeleteFile(Guid fileId)
        {
            var entity = _unitOfWork.Context
                .Set<PrtnContractFile>()
                .FirstOrDefault(a => a.Id == fileId);

            Delete(fileId, entity, DocumentStorageConst.DOC_PRTN_CONTRACT_FILES);
        }

        private void Delete(Guid fileId, FileEntity<long> entity, string storageDocument)
        {
            if (entity != null)
            {
                _storageService.DeleteTemp(storageDocument, fileId);
                CombineStatuses(_storageService);
            }
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
                if (file == null)
                    AddError("По вашему запросу запись не найдено");
                CombineStatuses(_storageService);

                if (IsValid)
                    file.FileName = entity.FileName;
            }

            return file;
        }
        #endregion

        public void Reject(RejectStatusPrtnContractDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                Repository.AllAsQueryable.Lock(dto.Id);

                dto.StatusId = StatusIdConst.REJECTED;

                var contract = Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanContractApplyStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");

                    dto.IsRead = false;
                });
                CombineStatuses(Repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                //_applicationService.Reject(new RejectStatusPrtnApplicationDto
                //{
                //    Id = contract.ApplicationId,
                //    Message = dto.Message,
                //});

                //if (dto.CancelApplication)
                //{
                _applicationService.Cancel(new CancelStatusPrtnApplicationDto { Id = contract.ApplicationId });
                CombineStatuses(_applicationService);
                if (HasErrors)
                    return;
                //}

                var res = CreateDocumentChangeLog(dto.Id, dto.StatusId, dto.Message);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                }
                _unitOfWork.Save();

                if (IsValid)
                    transaction.Commit();
            }
        }

        public void Cancel(CancelStatusPrtnContractDto dto)
        {
            var certificate = _unitOfWork.PrtnCertificateRepository.DbSet.Where(a => a.PrtnContractId == dto.Id && new int[] { StatusIdConst.FORMED, StatusIdConst.ACCEPTED }.Contains(a.StatusId));
            if (certificate.Any())
            {
                AddError("Imkoniyat yo'q. Tasdiqlangan sertifikat mavjud");
                return;
            }
            var canCommit = UnitOfWork.CurrentTransaction == null;
            var transaction = canCommit ? UnitOfWork.BeginTransaction() : UnitOfWork.CurrentTransaction;

            Repository.AllAsQueryable.Lock(dto.Id);

            PrtnContract contract = Repository.UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanContractApplyStatus(ent.StatusId, dto.StatusId))
                    AddError("Имкони йўқ / Нет доступа");


                dto.IsRead = false;
            });
            CombineStatuses(Repository);
            if (HasErrors)
                return;
            _unitOfWork.Save();

            if (dto.CancelApplication)
            {
                _applicationService.Cancel(new CancelStatusPrtnApplicationDto { Id = contract.ApplicationId });
                CombineStatuses(_applicationService);
                if (HasErrors)
                    return;
            }

            var res = CreateDocumentChangeLog(dto.Id, dto.StatusId, dto.Message);
            if (HasErrors)
            {
                transaction.Rollback();
                return;
            }
            _unitOfWork.Save();

            if (IsValid && canCommit)
                transaction.Commit();

        }

        public async Task Sign(SignStatusPrtnContractDto dto)
        {
            var doc = Repository.ById<PrtnContractDto>(dto.Id, true);
            //var doc = this.Get(dto.Id);

            if (doc == null)
            { AddError("Bunday shartnoma mavjud emas ! // yoki tashkilotingiz boshqa "); return; }

            if (!doc.CurrentPrtnContractSignId.HasValue)
            {
                AddError("Hujjat imzolangan / Документ уже подписан");
                return;
            }

            var currentPrtnContractSign = _unitOfWork.Context.Set<PrtnContractSign>()
                .Select(a => new
                {
                    Id = a.Id,
                    SignOrganizationTypeId = a.PrtnContractTypeTable.SignOrganizationTypeId,
                    Pinfl = a.OrganizationSign.Pinfl
                })
                .FirstOrDefault(a => a.Id == doc.CurrentPrtnContractSignId.Value);

            var eImzoTimeStampDto = new EImzoTimeStampDto
            {
                SignData = dto.SignedData, 
            };

            if (currentPrtnContractSign.SignOrganizationTypeId == SignOrganizationTypeIdConst.BUSINESSMAN)
            {
                if (doc.ContractorId != _authService.Contractor.Id)
                    AddError("Sizda imzolash huquqi yo'q / Вы не имеете права подписи");
                eImzoTimeStampDto.Inn = _authService.Contractor.Inn;
                dto.PrtnContractSignId = doc.CurrentPrtnContractSignId.Value;
            }
            else
            {
                if (doc.PrtnContractTypeId == PrtnContractTypeIdConst._201__)
                {
                    // Vazirlar ixtiyoriy ketma-ketlikda imzolashi mumkin
                    dto.PrtnContractSignId = doc.Signs.FirstOrDefault(a => a.OrganizationSignPinfl == _authService.User.Pinfl)?.Id ?? 0;
                    if (dto.PrtnContractSignId == 0)
                    {
                        AddError($"Imzolayotgan odam - {_authService.User.Pinfl} Imzolashi kerak bolgan odam - {doc.CurrentPrtnContractSignPinfl} -  Imzolovchilar mos emas");
                        return;
                    }
                }
                else
                {
                    if (currentPrtnContractSign.Pinfl != _authService.User.Pinfl)
                        AddError("Sizda imzolash huquqi yo'q / Вы не имеете права подписи");
                    else
                        dto.PrtnContractSignId = doc.CurrentPrtnContractSignId.Value;
                }

                if (_authService.Contractor != null)
                    eImzoTimeStampDto.Pinfl = dto.IsPinfl ? _authService.Contractor.Pinfl : null;
                else
                    eImzoTimeStampDto.Pinfl = dto.IsPinfl ? _authService.User.Pinfl : null;
            }
            if (HasErrors)
                return;

            var timeStampResult = await _eImzoService.TimeStamp(eImzoTimeStampDto);

            CombineStatuses(_eImzoService);
            if (HasErrors)
                return;
             
            if (_authService.Contractor != null)
            {
                var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
                {
                    SignData = timeStampResult.Pkcs7b64,
                    Pinfl = _authService.Contractor.Pinfl,
                    Inn = dto.IsPinfl ? null : _authService.Contractor.Inn
                });
            }
            else
            {
                var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
                {
                    SignData = timeStampResult.Pkcs7b64,
                    Inn = null,
                    Pinfl = _authService.User.Pinfl
                });
            }

            CombineStatuses(_eImzoService);
            if (HasErrors)
                return;

            dto.SignFile = SaveFile(doc.Id, timeStampResult.Pkcs7b64, "prtnSign.txt");
            dto.DataFile = SaveFile(doc.Id, JsonConvert.SerializeObject(doc), "data.txt");
            dto.SignedUserInfo = _authService.User.ToTextForDocumentLog();

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                // oxirgi odam imzolaganda statusi SIGNED ga ozgarishi kerak
                if (doc.Signs.Where(a => !a.IsSigned).Count() == 1)
                    dto.StatusId = StatusIdConst.SIGNED;

                Repository.AllAsQueryable.Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanContractApplyStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");

                    if (doc.PrtnContractTypeId == PrtnContractTypeIdConst._50_100 || doc.PrtnContractTypeId == PrtnContractTypeIdConst._101_200)
                    {
                        ent.SignExpireOn = DateTime.Now;
                    }
                    else if(doc.PrtnContractTypeId == PrtnContractTypeIdConst._201__)
                    {
                        if(ent.Signs.Where(x=> !x.IsSigned).Count() == 3)
                        {
                            ent.SigningExpireOn = DateTime.Now;
                        }
                        else if(ent.Signs.Where(x=> !x.IsSigned).Count() == 1)
                        {
                            ent.SignExpireOn = DateTime.Now;
                        }
                    }
                });
                CombineStatuses(Repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(dto.Id, dto.StatusId, dto.Message);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                }

                if (IsValid)
                    transaction.Commit();
            }
        }

        public string GetHtmlTemplate(PrtnContractDto dto)
        {
            var contractor = _unitOfWork.ContractorRepository.ById<ContractorListDto>(dto.ContractorId);
            var contractorBank = _unitOfWork.BankRepository.ById<BankListDto>(contractor.BankId ?? 0);
            var contractorSettlementAccount = _unitOfWork.Context.Set<ContractorSettlementAccount>().Where(a => a.OwnerId == contractor.Id).Select(a => a.AccountCode).FirstOrDefault();
            var organization = _unitOfWork.OrganizationRepository.ById<OrganizationListDto>(dto.OrganizationId);
            var organizationSettlementAccount = _unitOfWork.Context.Set<OrganizationSettlementAccount>().Where(a => a.OrganizationId == organization.Id).Select(a => new { a.AccountCode, a.BankId }).FirstOrDefault();
            var organizationBank = _unitOfWork.BankRepository.ById<BankListDto>(organizationSettlementAccount?.BankId ?? 0);
            var contractPosition1 = _unitOfWork.Context.Set<OrganizationSign>().FirstOrDefault(a => a.OwnerId == organization.Id && a.PrtnContractTypeTable.OrderNumber == 3)?./*User?.Person?.*/ShortName ?? "";
            var contractPosition2 = _unitOfWork.Context.Set<OrganizationSign>().FirstOrDefault(a => a.OwnerId == organization.Id && a.PrtnContractTypeTable.OrderNumber == 2)?./*User?.Person?.*/ShortName ?? "";
            var region = _unitOfWork.RegionRepository.ById<RegionListDto>(contractor.RegionId);

            //var applicationGraph = _unitOfWork.Context.Set<PrtnApplicationGraph>().Where(a => a.OwnerId == dto.PrtnApplicationId).Select(a => new AplicationGraphDto
            //{
            //    Year = a.YearIn,
            //    OwnerId = a.OwnerId,
            //    NewVacanciesCount = a.NewVacanciesCount,
            //    Month = a.MonthIn
            //}).ToList();

            var graphResult = new List<GrapthYear>();
            var applicationGraph = _unitOfWork.Context.Set<PrtnApplicationGraph>()
                .Where(a => a.OwnerId == dto.PrtnApplicationId)
                .ToList();
            var applicationGraphYears = applicationGraph.Select(a => a.YearIn).OrderBy(a => a).Distinct();
            if (applicationGraphYears.Any())
                for (int i = applicationGraphYears.Min(); i <= applicationGraphYears.Max(); i++)
                {
                    var graphResultItem = new GrapthYear()
                    {
                        YearIn = i,
                        Months = new List<GrapthMonth>()
                    };

                    for (int j = 1; j <= 12; j++)
                    {
                        graphResultItem.Months.Add(new GrapthMonth
                        {
                            MonthIn = j,
                            NewVacanciesCount = applicationGraph.Where(a => a.YearIn == i && a.MonthIn == j).Sum(a => a.NewVacanciesCount)
                        });
                    }

                    graphResult.Add(graphResultItem);
                }

            Dictionary<string, string> data
                = new Dictionary<string, string>()
                {
                    {"${DocDate}",dto.DocOn.ToString(Constants.DATE_FORMAT)},
                    {"${ContractorRegion}",contractor.Region},
                    {"${ContractorDistrict}",contractor.District},
                    {"${ContractorName}",contractor.FullName},
                    {"${ContractorDirector}",contractor.Director},
                    {"${ContractorInn}",contractor.Inn},
                    {"${ContractorBankCode}",contractorBank?.Code},
                    {"${ContractorBankName}",contractor?.Bank},
                    {"${ContractorSettlementAccount}",contractorSettlementAccount},
                    {"${OrgRegion}",organization.Region},
                    {"${OrgDistrict}",organization.District},
                    {"${OrgAdress}",organization.Address},
                    {"${OrgBankSettlementAccount}",organizationSettlementAccount?.AccountCode},
                    {"${OrgBankCode}",organizationBank?.Code},
                    {"${OrgBankName}",organizationBank?.BankName},
                    {"${OrgInn}",organization.Inn},
                    {"${ContractorPosition1}",contractPosition1},
                    {"${ContractorPosition2}",contractPosition2},
                };

            //string fileName = "ownership_contract_ministry_AB.html";

            string fileName = "";

            if (dto.PrtnContractTypeId == PrtnContractTypeIdConst._50_100)
                fileName = "ownership_contract_district_AB.html";
            else if (dto.PrtnContractTypeId == PrtnContractTypeIdConst._101_200)
                if (region.Soato == RegionSoatoConst.Karakalpakstan)
                    fileName = "ownership_contract_kr_vk_ab.html";
                else
                    fileName = "ownership_contract_region_AB.html";
            else if (dto.PrtnContractTypeId == PrtnContractTypeIdConst._201__)
            {
                fileName = "ownership_contract_ministry_AB.html";

            }
            else
            {
                AddError("Неверный тип контракта");
                return null;
            }

            return _baseReportService.ReturnReadyHtmlAsString(filename: fileName,
                                                      data: data,
                                                      tableData: null);
        }

        public byte[] GetPdfTemplate(long applicationId)
        {
            var dto = GetByApplicationId(applicationId);
            return _baseReportService.ReturnReadyPdf(GetHtmlTemplate(dto));
        }

        private void Validation<TDto>(PrtnContractDlDto<TDto> dto, PrtnContract entity)
            where TDto : PrtnContractDlDto<TDto>
        {
            var query = Repository.AllAsQueryable;

            if (entity != null && entity.Id != 0)
            {
                query = query.Where(a => a.Id != entity.Id);

                if (!StatusIdConst.CanEditStatuses.Contains(entity.StatusId))
                    AddError("Tahrirlash mumkin emas / Невозможно редактировать");
            }

            var application = _unitOfWork.Context.Set<Application>().FirstOrDefault(a => a.Id == dto.ApplicationId);


            // Tashkilotdagi imzolovchilarning hammasinni ma'lumoti kiritilganmi yo'qmi tekshirish
            var organization
                = _organizationService.GetOrganizationByLocation(
                    application.RegionId,
                    application.DistrictId,
                    dto.PrtnContractTypeId);

            /*var organization = _unitOfWork.OrganizationRepository.AllAsQueryable
                .Include(a => a.Signs)
                .First(a => a.Id == _authService.Organization.Id);*/

            if (!organization.SignOrganizationTypeId.HasValue)
            {
                AddError("Imzo chekuvchi tashkilot turi tanlanmagan / Тип подписываемый организации не выбрана");
            }
            else
            {
                var prtnContractTypeTables = _unitOfWork.PrtnContractTypeRepository.ById(entity.PrtnContractTypeId).Tables
                    .Where(a => a.SignOrganizationTypeId == organization.SignOrganizationTypeId);

                var signOptions = prtnContractTypeTables.Where(a => a.SignOrganizationTypeId == SignOrganizationTypeIdConst.REGION
                        && (
                            prtnContractTypeTables.Any(b => b.RegionId == organization.RegionId)
                                ? a.RegionId == organization.RegionId
                                : !a.RegionId.HasValue
                        )
                    )
                    .Select(a => a.Id)
                    .Distinct()
                    .ToList();

                var signers = organization.Signs.Where(a => !a.ExpireOn.HasValue || a.ExpireOn >= dto.DocOn)
                    .Select(a => a.PrtnContractTypeTableId)
                    .Distinct();

                if (signOptions.Intersect(signers).Count() != signOptions.Count)
                    AddError("Tashkilot imzo chekuvchilar to'liq kiritilmagan yoki muddati tugagan / Подписанты организации представлены не полностью или крайний срок истек");
            }
        }

        private HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
        {
            int? orgId = null;
            if (_authService.Contractor == null)
                orgId = _authService.Organization.Id;

            var doc = Repository.ById<PrtnContractDto>(id, applyFilter: false);

            _documentChangeLogService.Create(
                dto: doc,
                tableId: TableIdConst.DOC_PRTN_CONTRACT,
                organizationId: orgId,
                statusId: statusId,
                message: message,
                userIp: _authService.UserIp,
                userAgent: _authService.UserAgent);
            CombineStatuses(_documentChangeLogService);

            if (HasErrors)
                return null;

            return HaveId.Create(id);
        }


        public IQueryable<PrtnContractListDto> GetPrtnContractListDto(PrtnDocumentSortFilterOptions dto)
        {
           
            var data = Repository.ReadAsNoTracked<PrtnContractListDto>()
                      .SortFilter(dto)
                      .Filter(dto.Filters);

            return data;
        }
        public Stream SaveAsExecel(PrtnDocumentSortFilterOptions dto)
        {
            //var data = GetList(dto);
            var data = Repository.ReadAsNoTracked<PrtnContractListDto>()
                          .SortFilter(dto)
                          .Filter(dto.Filters)
                          .Take(1000000)
                          .ToList();

            MemoryStream result = new MemoryStream();
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.PRNTCONTRACT_LIST));

            if (IsValid && data != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excelPackage = new ExcelPackage(template);

                var namerange = excelPackage.Workbook.Names["Organization"];
                namerange.Value = "";

                var ws = namerange.Worksheet;

                var importRow = excelPackage.Workbook.Names["ImportRow"];
                int currentRow = importRow.Start.Row + 1;
                int index = 1;
                foreach (var item in data)
                {
                    var column = 1;
                    ws.InsertRow(currentRow, 1, importRow.Start.Row);
                    ws.Cells[currentRow, column++].Value = index++;
                    ws.Cells[currentRow, column++].Value = item.DocNumber;
                    ws.Cells[currentRow, column++].Value = item.DocOn.ToString(Constants.DATE_FORMAT);
                    ws.Cells[currentRow, column++].Value = item.ContractorRegion;
                    ws.Cells[currentRow, column++].Value = item.ContractorDistrict;
                    ws.Cells[currentRow, column++].Value = item.PrtnContractType;
                    ws.Cells[currentRow, column++].Value = item.ContractorInn;
                    ws.Cells[currentRow, column++].Value = item.Contractor;
                    ws.Cells[currentRow, column++].Value = item.ContractorRegestrationDate.Value.ToString(Constants.DATE_FORMAT);
                    ws.Cells[currentRow, column++].Value = item.Organization;
                    ws.Cells[currentRow, column++].Value = String.Join(", ", item.Signed.Select(a => a.FullName).ToList()) + " " + String.Join(", ", item.Signed.Select(a => a.Fio).ToList());
                    ws.Cells[currentRow, column++].Value = String.Join(", ", item.Signed.Select(a => a.FullName).ToList()) + " " + String.Join(", ", item.NotSigned.Select(a => a.Fio).ToList());
                    ws.Cells[currentRow, column++].Value = item.Status;
                    ws.Cells[currentRow, column++].Value = item.PrtnCertificateStatus;

                    currentRow++;
                }
                ws.DeleteRow(importRow.Start.Row);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }
            result.Position = 0;
            return result;
        }

        private Guid SaveFile(long docId, string data, string fileName)
        {
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(data);
            writer.Flush();
            ms.Position = 0;
            var fileInfo = _storageService.Save($"{nameof(TableIdConst.DOC_APPLICATION)}_SIGN_DATA", docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

            return fileInfo.FirstOrDefault().FileId;
        }

        public async Task InsertPrtnContractDataBase()
        {

            var logsToUpdate = await (from docChangeLog in _unitOfWork.Context.Set<DocumentChangeLog>()
                                      join prtnContract in _unitOfWork.Context.Set<PrtnContract>()
                                      on docChangeLog.DocId equals prtnContract.Id
                                      where docChangeLog.TableId == TableIdConst.DOC_PRTN_CONTRACT
                                      && docChangeLog.StatusId != StatusIdConst.DELETED && prtnContract.Application.ApplicationTypeId == ApplicationTypeIdConst.PARTNER
                                      select new
                                      {
                                          prtnContract,
                                          docChangeLog.DateAt,
                                          docChangeLog.StatusId,
                                      }).ToListAsync();

            int count = 0;
            foreach (var item in logsToUpdate)
            {

                if (item.DateAt == DateTime.MinValue) // -infinity holatini tekshirish
                {
                    var prtnContractSign = await _unitOfWork.Context.Set<PrtnContractSign>()
                        .Where(x => x.OwnerId == item.prtnContract.Id)
                        .ToListAsync();


                    var prtnContractFile = await _unitOfWork.Context.Set<PrtnContractFile>()
                                                             .Where(x => x.OwnerId == item.prtnContract.Id)
                                                             .ToListAsync();


                    foreach (var itemFile in prtnContractFile)
                    {
                         if(item.StatusId == StatusIdConst.PASS_EXPERTISE)
                         {
                            item.prtnContract.PassExpertiseExpireOn = itemFile.CreatedAt;
                         }
                         if (item.StatusId == StatusIdConst.NOT_PASS_EXPERTISE)
                         {
                            item.prtnContract.NotPassExpertiseExpireOn = itemFile.CreatedAt;
                            item.prtnContract.ResendExpertiseExpireOn = itemFile.CreatedAt;
                         }
                    }

                    foreach (var item1 in prtnContractSign)
                    {
                        if (prtnContractSign != null)
                        {
                            if (item1.StatusId == StatusIdConst.SIGNED)
                            {
                                item.prtnContract.SignExpireOn = item1.SignedAt;
                            }
                            if (item1.StatusId == StatusIdConst.SIGNING)
                            {
                                item.prtnContract.SigningExpireOn = item1.SignedAt;
                            }
                        }
                    }

                                item.prtnContract.StatusChangeExpireOn = item.prtnContract.CreatedAt;

                }
                else
                {

                    if (item.StatusId == StatusIdConst.PASS_EXPERTISE)
                    {
                        item.prtnContract.PassExpertiseExpireOn = item.DateAt;
                    }
                    if (item.StatusId == StatusIdConst.NOT_PASS_EXPERTISE)
                    {
                        item.prtnContract.NotPassExpertiseExpireOn = item.DateAt;
                    }
                    if (item.StatusId == StatusIdConst.SIGNED)
                    {
                        item.prtnContract.SignExpireOn = item.DateAt;
                    }
                    if (item.StatusId == StatusIdConst.SIGNING)
                    {
                        item.prtnContract.SigningExpireOn = item.DateAt;
                    }
                    
                        item.prtnContract.StatusChangeExpireOn = item.prtnContract.CreatedAt;
                }


            }


            if (HasErrors)
                return;

                _unitOfWork.Context.SaveChanges(); 
        }


       public async Task  InsertPrtnCertifcatCulumn()
       {
            var prtnCertficat = await (from prtnCer in _unitOfWork.Context.Set<PrtnCertificate>() 
                                join  prtnCon  in  _unitOfWork.Context.Set<PrtnContract>()
                                on prtnCer.PrtnContractId equals prtnCon.Id 
                                select new
                                {
                                    prtnCer,
                                    prtnCon,
                                }).ToListAsync();
            int count = 0;
            foreach (var item in prtnCertficat)
            {
                if(item.prtnCer.CreatedAt != null)
                item.prtnCon.PrepareCertificateExpireOn =  item.prtnCer.CreatedAt;

                
            }
            _unitOfWork.Context.SaveChanges();

        }

	}
    public class GrapthYear
    {
        public int YearIn { get; set; }
        public int TotalForYear { get; set; }
        public List<GrapthMonth> Months { get; set; } = new();
    }

    public class GrapthMonth
    {
        public int MonthIn { get; set; }
        public int TotalForMonth { get; set; }
        public int NewVacanciesCount { get; set; }
    }
}
