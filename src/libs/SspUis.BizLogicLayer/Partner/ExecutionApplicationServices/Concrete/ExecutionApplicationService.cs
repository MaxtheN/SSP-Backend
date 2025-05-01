using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.DocxToPdf;
using SspUis.ServiceLayer.NumberServices;
using WEBASE;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.ExecutionApplicationServices
{
    public class ExecutionApplicationService
    : BaseEntityService<long,
        ExecutionApplication,
        ExecutionApplicationListDto,
        ExecutionApplicationDto,
        CreateExecutionApplicationDlDto,
        UpdateExecutionApplicationDlDto,
        IExecutionApplicationRepository,
        ExecutionApplicationSortFilterOptions>
    , IExecutionApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly INumberService _numberService;
        private readonly IEImzoService _eImzoService;
        private readonly IDocumentChangeLogService _documentChangeLogService;
        private readonly IStorageService _storageService;

        public ExecutionApplicationService(IUnitOfWork unitOfWork,
            IAuthService authService,
            INumberService numberService,
            IDocumentChangeLogService documentChangeLogService,
            IEImzoService eImzoService,
            DbContext context,
            IApiRequestLogRepository apiRequestLogRepository,
            IStorageService storageService,
            IConvertService pdfConverter)
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _numberService = numberService;
            _storageService = storageService;
            _documentChangeLogService = documentChangeLogService;
            _eImzoService = eImzoService;
        }
        public PagedResult<ExecutionApplicationListDto> GetList(ExecutionApplicationSortFilterOptions options)
        {
            var result = Repository.ReadAsNoTracked<ExecutionApplicationListDto>()
                               .SortFilter(options)
                               .AsPagedResult(options);
            return result;
        }
        public override ExecutionApplicationDto Get()
        {
            var orgByRegion = UnitOfWork.Context.Set<Organization>()
            .FirstOrDefault(org => org.Id == _authService.User.OrganizationId);
            return new ExecutionApplicationDto()
            {

                DocOn = DateOnly.FromDateTime(DateTime.Now),
                DocNumber = this._numberService.GetNext(
                NumberTemplateDocumentConst.DOC_EXECUTION_APPLICATION,
                organizationId: orgByRegion.Id,
                regionId: orgByRegion.RegionId,
                districtId: orgByRegion.DistrictId.Value).Item2,
            };
        }
        public override ExecutionApplicationDto Get(long id)
        {
            var dto = Repository.ById<ExecutionApplicationDto>(id);

            if (dto == null)
                return null;

            dto.CanAccept = StatusIdConst.CanExecutionApplicationApplyStatus(dto.StatusId, StatusIdConst.ACCEPTED)
                                && _authService.HasPermission(ModuleCode.ExecutionApplicationAccept);
            dto.CanCancel = StatusIdConst.CanExecutionApplicationApplyStatus(dto.StatusId, StatusIdConst.CANCELED)
                                && _authService.HasPermission(ModuleCode.ExecutionApplicationCancel);
            dto.CanSign = StatusIdConst.CanExecutionApplicationApplyStatus(dto.StatusId, StatusIdConst.SIGNED)
                               && _authService.HasPermission(ModuleCode.ExecutionApplicationSign);


            return dto;
        }
        public async Task<HaveId<long>> Create(CreateExecutionApplicationDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = Repository.Create(dto, ent => Validation(dto, ent));
                    CombineStatuses(Repository);
                    if (HasErrors) return null;

                    UnitOfWork.Save();

                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return null;
                    }

                    var res = CreateDocumentChangeLog(entity.Id, entity.StatusId);
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
        public override void Update(UpdateExecutionApplicationDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var entity = Repository.Update(dto, ent =>
                {
                    if (!StatusIdConst.CanEditStatuses.Contains(ent.StatusId))
                        AddError("Tahrirlash mumkin emas / Невозможно редактировать");
                });

                CombineStatuses(Repository);


                if (IsValid)
                    UnitOfWork.Save();

                var res = CreateDocumentChangeLog(entity.Id, entity.StatusId);

                if (IsValid) transaction.Commit();
            }
        }
        public void Accept(AcceptStatusExecutionApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<ExecutionApplication>().Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanExecutionApplicationApplyStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                    else
                    {
                        ent.StatusId = dto.StatusId;
                    }
                });
                CombineStatuses(Repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(dto.Id, dto.StatusId);
                if (IsValid && canCommit)
                    transaction.Commit();
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }
        public void Cancel(CancelStatusExecutionApplicationDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<ExecutionApplication>().Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanExecutionApplicationApplyStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");
                });

                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(id: dto.Id, dto.StatusId);

                if (IsValid && canCommit)
                    transaction.Commit();
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }
        public async Task Sign(SignStatusExecutionApplicationDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = Repository.ById(dto.Id);


                    var eImzoTimstampDto = new EImzoTimeStampDto
                    {
                        SignData = dto.SignedData,
                        Inn = null,
                        Pinfl = _authService.User.Pinfl
                    };

                    var timeStamp = _eImzoService.TimeStamp(eImzoTimstampDto).Result;

                    CombineStatuses(_eImzoService);
                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return;
                    };

                    var eImzoVerifyAttached = _eImzoService.VerifyAttached(new EImzoVerifyDto
                    {
                        SignData = timeStamp.Pkcs7b64,
                        Inn = null,
                        Pinfl = _authService.User.Pinfl
                    }).Result;

                    dto.SignFile = SaveFile(dto.Id, timeStamp.Pkcs7b64, "sign.txt");
                    dto.DataFile = SaveFile(dto.Id, JsonConvert.SerializeObject(dto), "data.txt");
                    dto.SignedAt = DateTime.Now;
                    dto.SignedUserInfo = _authService.User.ToTextForDocumentLog();

                    _unitOfWork.Save();

                    var ent = Repository.UpdateStatus(dto, ent =>
                    {
                        if (!StatusIdConst.CanExecutionApplicationApplyStatus(ent.StatusId, StatusIdConst.SIGNED))
                            Repository.AddError("Нет доступа");
                    });
                    CombineStatuses(Repository);
                    if (HasErrors)
                        return;

                    _unitOfWork.Save();
                    if (IsValid)
                    {
                        var res = CreateDocumentChangeLog(entity.Id, StatusIdConst.SIGNED, Message);
                        transaction.Commit();
                    }
                }
                catch (DbUpdateException)
                {
                    AddError("Запись не может быть удален");
                    transaction.Rollback();
                }
            }
        }
        private Guid SaveFile(long docId, string data, string fileName)
        {
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(data);
            writer.Flush();
            ms.Position = 0;
            var fileInfo = _storageService.Save($"{nameof(TableIdConst.DOC_EXECUTION_APPLICATION)}_SIGN_DATA", docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

            return fileInfo.FirstOrDefault().FileId;
        }
        public async ValueTask<byte[]> DownloadPdf(Guid id2, string? lang)
        {
            //lang = "uz-cyrl";

            //var languageId = UnitOfWork.Context.Set<Language>()
            //    .FirstOrDefault(l => l.Code == lang)?.Id ?? 1;

            //var claimApplication = await _unitOfWork.Context
            //   .Set<ExecutionApplication>()
            //   .Include(c => c.Application)
            //   .Include(x => x.Organization)
            //   .ThenInclude(a => a.Translates)
            //   .Include(c => c.ExecutionApplicationType)
            //   .Include(c => c.Currency)
            //   .FirstOrDefaultAsync(c => c.Application.Id2 == id2);

            //MemoryStream wordFile = new MemoryStream();
            //if (id2 == Guid.Empty)
            //{
            //    wordFile = _storageService.GetStaticFile(
            //        StaticFileConst.WordTemplate.GetFileName(
            //            lang,
            //            StaticFileConst.WordTemplate.CLAIM_APPLICATION_ADMINISTRATIVE_COURT)
            //        );
            //    return await _pdfConverter.DocxToPdfAsync(wordFile, new object());
            //}

            //var tables = _unitOfWork.Context.Set<ExecutionApplicationTable>()
            //     .Include(t => t.ClaimResponsibleType)
            //     .ThenInclude(t => t.Translates)
            //     .Where(t => t.OwnerId == claimApplication.Id);

            //if (claimApplication == null)
            //{
            //    AddError("Ariza topilmadi / Заявление не найдено!");
            //    return null;
            //}

            //if (claimApplication.ExecutionApplicationTypeId == ExecutionApplicationTypeIdConst.APPLICATION_FOR_COURT && claimApplication.OrganizationId == OrganizationIdConst.SSP)
            //    wordFile = _storageService.GetStaticFile(
            //        StaticFileConst.WordTemplate.GetFileName(
            //            lang,
            //            StaticFileConst.WordTemplate.CLAIM_APPLICATION_ADMINISTRATIVE_COURT_1SSP)
            //        );

            //else if (claimApplication.ExecutionApplicationTypeId == ExecutionApplicationTypeIdConst.APPLICATION_FOR_COURT)
            //    wordFile = _storageService.GetStaticFile(
            //        StaticFileConst.WordTemplate.GetFileName(
            //            lang,
            //            StaticFileConst.WordTemplate.CLAIM_APPLICATION_ADMINISTRATIVE_COURT)
            //        );

            //else if ((claimApplication.ExecutionApplicationTypeId == ExecutionApplicationTypeIdConst.CLAIM_APPLICATION || claimApplication.ExecutionApplicationTypeId == ExecutionApplicationTypeIdConst.COUNTER_CLAIM) && claimApplication.OrganizationId == OrganizationIdConst.SSP)
            //    wordFile = _storageService.GetStaticFile(
            //        StaticFileConst.WordTemplate.GetFileName(
            //            lang,
            //            StaticFileConst.WordTemplate.CLAIM_0R_COUNTERCLAIM_APPLICATION_SSP)
            //        );
            //else if (claimApplication.ExecutionApplicationTypeId == ExecutionApplicationTypeIdConst.CLAIM_APPLICATION || claimApplication.ExecutionApplicationTypeId == ExecutionApplicationTypeIdConst.COUNTER_CLAIM)
            //    wordFile = _storageService.GetStaticFile(
            //        StaticFileConst.WordTemplate.GetFileName(
            //            lang,
            //            StaticFileConst.WordTemplate.CLAIM_0R_COUNTERCLAIM_APPLICATION)
            //        );

            //else if (claimApplication.ExecutionApplicationTypeId == ExecutionApplicationTypeIdConst.APILATION_CASSATION || claimApplication.ExecutionApplicationTypeId == ExecutionApplicationTypeIdConst.CLAIM_REVISION)
            //    wordFile = _storageService.GetStaticFile(
            //        StaticFileConst.WordTemplate.GetFileName(
            //            lang,
            //            StaticFileConst.WordTemplate.CLAIM_APPLICATION_APILATION_OR_CASSATION)
            //        );
            //if (wordFile == null)
            //{
            //    AddError("Undefined document Type.");
            //    return null;
            //}

            //var contractor = await UnitOfWork.Context.Set<DataLayer.EfClasses.Contractor>()
            //    .Include(c => c.SettlementAccounts)
            //    .FirstOrDefaultAsync(c => c.Id == claimApplication.Application.ContractorId);

            //var region = await UnitOfWork.Context.Set<Region>()
            //    .Include(r => r.Translates)
            //    .FirstOrDefaultAsync(r => r.Id == contractor.RegionId);

            //var orgregion = await UnitOfWork.Context.Set<Region>()
            //    .Include(r => r.Translates)
            //    .FirstOrDefaultAsync(r => r.Id == claimApplication.Organization.RegionId);

            //var district = await UnitOfWork.Context.Set<District>()
            //    .Include(d => d.Translates)
            //    .FirstOrDefaultAsync(d => d.Id == contractor.DistrictId);

            //var bank = await UnitOfWork.Context.Set<DataLayer.EfClasses.Bank>()
            //    .Include(b => b.Translates)
            //    .FirstOrDefaultAsync(b => b.Id == contractor.BankId);

            //var theme = await UnitOfWork.Context.Set<ClaimTheme>()
            //    .Include(r => r.Translates)
            //    .FirstOrDefaultAsync(t => t.Id == claimApplication.ClaimThemeId);

            //var applicationForCourt = await UnitOfWork.Context.Set<ApplicationForCourt>()
            //    .Include(ac => ac.ClaimOrganization)
            //    .Include(afc => afc.Mediation)
            //    .ThenInclude(m => m.MediationPlan)
            //    .ThenInclude(mp => mp.Application)
            //    .ThenInclude(a => a.ExecutionApplication)
            //    .AsSplitQuery()
            //    .FirstOrDefaultAsync(ac => claimApplication.Id == ac.Mediation.MediationPlan.Application.ExecutionApplication.Id);

            //var plh = new Placeholders();
            //var link = _systemConf.QrImagePrintMy + "/ExecutionApplication/DownloadPdf?id2=" + claimApplication.Application.Id2.ToString();
            //var qrCode = new MemoryStream(QRCodeHelper.GeneratePng(link));
            //plh.ImagePlaceholders.Add("QrCode", new() { Dpi = 512, MemStream = qrCode });

            //plh.TextPlaceholders.Add(nameof(contractor.Region), region.Translates
            //    .FirstOrDefault(t => t.LanguageId == languageId
            //        && t.ColumnName == TranslateColumn.full_name.ToString()
            //        )?.TranslateText ?? region.FullName ?? "");

            //plh.TextPlaceholders.Add(nameof(contractor.District), district.Translates
            //    .FirstOrDefault(t => t.LanguageId == languageId
            //        && t.ColumnName == TranslateColumn.full_name.ToString()
            //        )?.TranslateText ?? district.FullName ?? "");

            //plh.TextPlaceholders.Add(nameof(contractor.Bank), bank == null ? "" : bank.Translates
            //        .FirstOrDefault(t => t.LanguageId == languageId && t.ColumnName == BankTranslateColumn.bank_name.ToString())
            //            ?.TranslateText ?? bank.BankName
            //            + " "
            //            + contractor.SettlementAccounts.FirstOrDefault(a => a.IsMain)?.AccountCode ?? "");

            //plh.TextPlaceholders.Add(nameof(contractor.FullName), contractor.FullName ?? "");
            //plh.TextPlaceholders.Add(nameof(contractor.Address), contractor.Address ?? "");
            //plh.TextPlaceholders.Add(nameof(contractor.Inn), contractor.Inn ?? "");
            //plh.TextPlaceholders.Add("Organization", orgregion.Translates
            //     .FirstOrDefault(t => t.LanguageId == languageId
            //         && t.ColumnName == TranslateColumn.full_name.ToString()
            //         )?.TranslateText ?? orgregion.FullName ?? "");
            //plh.TextPlaceholders.Add(nameof(claimApplication.Application.DocNumber), claimApplication.Application.DocNumber ?? "");
            //plh.TextPlaceholders.Add(nameof(claimApplication.Application.DocOn), claimApplication.Application.DocOn.ToString("dd.MM.yyyy") ?? "");

            //plh.TextPlaceholders.Add(nameof(claimApplication.ClaimTheme),
            //    theme.Translates
            //    .FirstOrDefault(t => t.LanguageId == languageId
            //        && t.ColumnName == TranslateColumn.full_name.ToString()
            //        )?.TranslateText ?? theme.FullName ?? "");

            //plh.TextPlaceholders.Add("CourtAppData", applicationForCourt?.DocOn.ToString("dd.MM.yyyy") ?? "");
            //plh.TextPlaceholders.Add("CourtDocNumber", applicationForCourt?.DocNumber ?? "");

            //plh.TextPlaceholders.Add("ClaimAppTypeName",
            //    claimApplication.ExecutionApplicationType.Translates
            //    .FirstOrDefault(t => t.LanguageId == languageId
            //        && t.ColumnName == TranslateColumn.full_name.ToString()
            //        )?.TranslateText ?? claimApplication.ExecutionApplicationType.FullName ?? "");

            //plh.TextPlaceholders.Add(nameof(applicationForCourt.ClaimOrganization),
            //    applicationForCourt?.ClaimOrganization.Translates
            //    .FirstOrDefault(t => t.LanguageId == languageId
            //        && t.ColumnName == TranslateColumn.full_name.ToString()
            //        )?.TranslateText ?? applicationForCourt?.ClaimOrganization.FullName ?? "");

            //plh.TextPlaceholders.Add(nameof(claimApplication.Tables), String.Join(", ", tables.Select(t => t.FullName)));

            //var items = new List<Placeholders>();
            //foreach (var item in tables)
            //{
            //    var lplh = new Placeholders();
            //    lplh.TextPlaceholders.Add("ResponsibleName", item.FullName ?? "");
            //    lplh.TextPlaceholders.Add("ResponsibleAddress", item.Address ?? "");
            //    lplh.TextPlaceholders.Add("ResponsiblePhone", item.PhoneNumber ?? "");
            //    lplh.TextPlaceholders.Add("ResponsibleInn", item.InnOrPinfl ?? "");
            //    items.Add(lplh);
            //}
            //plh.TemplateListPlaceholders.Add(nameof(claimApplication.Tables), items);
            //plh.TextPlaceholders.Add(nameof(claimApplication.TotalAmount), claimApplication.TotalAmount?.ToString() ?? "-");
            //plh.TextPlaceholders.Add(nameof(claimApplication.MainDebt), claimApplication.MainDebt?.ToString() ?? "-");
            //plh.TextPlaceholders.Add(nameof(claimApplication.CalculedPenalty), claimApplication.CalculedPenalty?.ToString() ?? "-");
            //plh.TextPlaceholders.Add(nameof(claimApplication.Penalty), claimApplication.Penalty?.ToString() ?? "-");
            //plh.TextPlaceholders.Add(nameof(claimApplication.Percent), claimApplication.Percent?.ToString() ?? "-");
            //plh.TextPlaceholders.Add(nameof(claimApplication.Currency), claimApplication?.Currency.FullName ?? "-");
            //plh.TextPlaceholders.Add(nameof(claimApplication.CurrentPrincipalInterest), claimApplication?.CurrentPrincipalInterest?.ToString() ?? "-");
            //plh.TextPlaceholders.Add(nameof(claimApplication.CurrentInterestRate), claimApplication?.CurrentInterestRate?.ToString() ?? "-");
            //plh.TextPlaceholders.Add(nameof(claimApplication.OtherDebtRepayment), claimApplication?.OtherDebtRepayment?.ToString() ?? "-");
            //plh.TextPlaceholders.Add("ContractorDirector", contractor.Director);

            //wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();

            //return await _pdfConverter.DocxToPdfAsync(wordFile, new());
            return null;
        }
        public List<ExecutionApplicationCellDto> GetFromGraph(int year, int month)
        {
            var organization = _unitOfWork.Context.Set<Organization>()
                                  .FirstOrDefault(a => a.Id == _authService.User.OrganizationId);

            if (organization == null)
            {
                return new List<ExecutionApplicationCellDto>();
            }

            var graph = _unitOfWork.Context.Set<PrtnApplication>()
                    .Include(a => a.Application)
                    .ThenInclude(c => c.Contractor)
                    .Include(g => g.Graphs)
                    .Include(con => con.Application.PrtnContract)
                    .ThenInclude(cert => cert.PrtnCertificate)
                .Where(a => a.Application.RegionId == organization.RegionId
                         && a.Application.DistrictId == organization.DistrictId.Value
                         && a.Application.StatusId == StatusIdConst.ACCEPTED
                         && a.Application.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED
                         && a.Application.PrtnApplication.Graphs.Any(b => b.YearIn < year || (b.YearIn == year && b.MonthIn <= month))
                         )
                .ToList();

            if (graph.Any())
            {
                var result = graph.Select(a => new ExecutionApplicationCellDto
                {
                    PrtnCertificateId = a.Application.PrtnContract.PrtnCertificate.Id,
                    Contractor = a.Application.Contractor.FullName,
                    ContractorInn = a.Application.Contractor.Inn,
                    ContractorId = a.Application.ContractorId.Value,
                    PrtnNewVacanciesCount = a.Application.PrtnApplication.Graphs.Sum(a => a.NewVacanciesCount),
                    ProjectNewVacanciesCount = 0,
                    AverageSalary = 0,
                    Salary = 0,
                }).ToList();

                return result;
            }
            else
            {
                AddError("Ma'lumot topilmadi!");
                return null;
            }
        }
        #region H E L P E R
        private HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
        {
            var moveDto = Repository.ById<ExecutionApplicationDto>(id, applyFilter: false);
            _documentChangeLogService.Create(
                dto: moveDto,
                tableId: TableIdConst.DOC_EXECUTION_APPLICATION,
                organizationId: null,
                statusId: statusId,
                message: message);
            CombineStatuses(_documentChangeLogService);

            if (HasErrors)
                return null;

            return HaveId.Create(id);
        }
        private void Validation<TDto>(ExecutionApplicationDlDto<TDto> dto, ExecutionApplication entity)
            where TDto : ExecutionApplicationDlDto<TDto>
        {
            var query = Repository.AllAsQueryable;

            //if (!_unitOfWork.Context.Set<MemshipContract>().Any(x => x.Id == dto.MemshipContractId
            //    && x.StatusId != StatusIdConst.DELETED))
            //{
            //    Repository.AddError("A'zolik shartnomasi mavjud emas / Нет соглашения о членстве");
            //}

            //if (dto.ExecutionApplicationTypeId == ExecutionApplicationTypeIdConst.APILATION_CASSATION
            //    || dto.ExecutionApplicationTypeId == ExecutionApplicationTypeIdConst.CLAIM_REVISION)
            //{
            //    if (!dto.PrevApplicationId.HasValue)
            //        Repository.AddError("Oldingi murojaatni tanlang / Выберите предыдущее заявление");
            //}
        }
        #endregion
    }
}
