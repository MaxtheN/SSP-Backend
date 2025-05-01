using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.ManualServices;
using SspUis.BizLogicLayer.Notify;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Claim;
using SspUis.Integration.DocxToPdf;
using SspUis.ServiceLayer.NumberServices;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using WEBASE.OfficeTools;
using WEBASE.OfficeTools.Handlers;
using WEBASE.QRCode;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Claim
{
    public class MediationPlanService : StatusGenericHandler, IMediationPlanService
    {

        private readonly IMediationPlanRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IDocumentChangeLogService _documentChangeLogService;
        private readonly ISendSmsService _sendSmsService;
        private readonly IClaimApplicationRepository _claimApplicationRepository;
        private readonly IConvertService _pdfConverter;
        private readonly IStorageService _storageService;
        private readonly IManualService _manualService;
        private readonly SystemConf _systemConf;
        public MediationPlanService(
            IUnitOfWork unitOfWork,
            IAuthService authService,
            IDocumentChangeLogService documentChangeLogService,
            ISendSmsService sendSmsService,
            IClaimApplicationRepository claimApplicationRepository,
            IConvertService pdfConverter,
            IStorageService storageService,
            IManualService manualService,
            SystemConf systemConf)
        {
            this._repository = unitOfWork.MediationPlanRepository;
            this._unitOfWork = unitOfWork;
            this._authService = authService;
            this._documentChangeLogService = documentChangeLogService;
            this._sendSmsService = sendSmsService;
            this._claimApplicationRepository = claimApplicationRepository;
            this._pdfConverter = pdfConverter;
            this._storageService = storageService;
            this._manualService = manualService;
            this._systemConf = systemConf;
        }

        public SelectList<long> AsSelectList(MediationPlanSortFilterOptions options)
        {
            return _repository.ReadAsNoTracked<MediationPlanListDto>()
                .SortFilter(options)
                .AsSelectList();
        }
		public WEBASE.Models.PagedResult<MediationPlanListDto> GetList(MediationPlanSortFilterOptions options)
        {
            var data = _repository.ReadAsNoTracked<MediationPlanListDto>()
                .SortFilter(options)
                .AsPagedResult(options);

            if (options.IsEmployee)
                data.Rows = data.Rows.Where(x => x.EmployeeManageId == _authService.User.EmployeeManageId);

            return data;
        }
        public MediationPlanDto Get()
        {
            return new MediationPlanDto()
            {
                DocOn = DateOnly.FromDateTime(DateTime.Now),
                //DocNumber = _numberTemplateService.GetNext(TableIdConst.CLAIM__MediationPlan, isOrdinary: true)
            };
        }
        public MediationPlanDto GetByApplication(int applicationId)
        {
            var application = _unitOfWork.Context.Set<Application>()
                .Include(a => a.Contractor)
                .Include(a => a.CreatedUser)
                .Include(x => x.ClaimApplication).ThenInclude(x => x.ClaimApplicationType)
                .Include(x => x.ClaimApplication.EmployeeManage).ThenInclude(x => x.Employee).ThenInclude(x => x.Person)
                .Include(x => x.ClaimApplication.ClaimTheme)
                .Include(x => x.ClaimApplication.CreatedUser)
                .Include(x => x.ClaimApplication.Tables).ThenInclude(a => a.ClaimResponsibleType)
                .AsSplitQuery()
                .FirstOrDefault(c => c.Id == applicationId);

            if (application == null) { AddError("Bunday ariza mavjud emas"); return null; }

            var plan = new MediationPlanDto()
            {
                ApplicationId = application.Id,
                ContractorId = application.ContractorId.Value,
                Contractor = application.Contractor.FullName,
                ClaimThemeId = application.ClaimApplication.ClaimThemeId,
                ClaimTheme = application.ClaimApplication.ClaimTheme.FullName,
                ContractorInn = application.Contractor.Inn,
                ApplicationTypeId = application.ApplicationTypeId,
                ApplicaionDocNumber = application.DocNumber,
                ChamberPerson = _authService.User.FullName,
                EmployeeManage = application.ClaimApplication.EmployeeManageId != null
                    ? application.ClaimApplication.EmployeeManage.Employee.Person.FullName
                    : _authService.User.FullName,
                DurationGivenPerformer = application.ClaimApplication.DurationGivenPerformer,
                RegPhoneNumber = application.CreatedUser != null ? application.CreatedUser.UserName : string.Empty,
                DocNumber = application.DocNumber,
                ApplicaionDocOn = application.DocOn,
                StepId = application.CurrentStepId,
                Tables = application.ClaimApplication.Tables.Select(a => new MediationClaimApplicationTableDto
                {
                    Id = a.Id,
                    OrderNumber = a.OrderNumber,
                    Address = a.Address,
                    FullName = a.FullName,
                    InnOrPinfl = a.InnOrPinfl,
                    PhoneNumber = a.PhoneNumber,
                    IsRegistred = a.IsRegistred,
                    ClaimResponsibleTypeId = a.ClaimResponsibleTypeId,
                    ClaimResponsibleType = a.ClaimResponsibleType.FullName,
                }).ToList(),
                ClaimApplicationType = application.ClaimApplication.ClaimApplicationType.FullName,
                DocOn = DateTime.Now.AsDateOnly()
            };

            return plan;
        }
        public MediationPlanDto Get(long id)
        {
            var dto = _repository.ById<MediationPlanDto>(id);
            CombineStatuses(_repository);
            if (IsValid)
            {
                dto.CanModify = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.MediationPlanEdit);
                dto.CanAccept = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.MediationPlanAccept);
                dto.CanCancel = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(ModuleCode.MediationPlanCancel);
                dto.CanDelete = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.MediationPlanDelete);
            }
            return dto;
        }
        public HaveId<long> Create(CreateMediationPlanDlDto dto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                var entity = _repository.Create(dto, ent => Validation(dto, ent));
                CombineStatuses(_repository);
                if (HasErrors) return null;

                if (IsValid)
                {
                    _unitOfWork.Save();
                    var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "CreateMediationPlan");

                    if (canCommit)
                        transaction.Commit();

                    return HaveId.Create(entity.Id);
                }
            }
            catch (DbUpdateException e)
            {
                AddError(e.Message);
                if (e.InnerException != null)
                    AddError(e.InnerException.Message);

                if (canCommit)
                    transaction.Rollback();
            }
            return null;
        }
        public async Task Accept(UpdateStatusMediationPlanDto dTo)
        {
            var dto = new UpdateStatusMediationPlanDlDto { Id = dTo.Id, StatusId = StatusIdConst.ACCEPTED };
            await UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, dto.StatusId))
                    _repository.AddError("Нет доступа");

                CreateDocumentChangeLog(dTo.Id, dto.StatusId);
            });
        }
        public async Task Cancel(UpdateStatusMediationPlanDto dTo)
        {
            var dto = new UpdateStatusMediationPlanDlDto { Id = dTo.Id, StatusId = StatusIdConst.NOT_ACCEPTED };
            await UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, dto.StatusId))
                    _repository.AddError("Нет доступа");

                CreateDocumentChangeLog(dTo.Id, dto.StatusId, dTo.Message);
            });
        }
        public void Update(UpdateMediationPlanDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = _repository.Update(dto, ent => Validation(dto, ent));
                    CombineStatuses(_repository);
                    _unitOfWork.Save();
                    if (IsValid)
                    {
                        _unitOfWork.Save();
                        var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "UpdateMediationPlan");
                        transaction.Commit();
                    }
                }
                catch (DbUpdateException e)
                {
                    AddError(e.Message);
                    transaction.Rollback();
                }
            }
        }
		public byte[] DownloadPdf(Guid id2, string? langu)
		{
            var language = "uz-latn";

			var mediationPlan = _unitOfWork.Context.Set<MediationPlan>()
                                                    .Include(x => x.Application)
                                                        .ThenInclude(x => x.ClaimApplication)
                                                            .ThenInclude(x => x.ClaimApplicationType)
                                                                .ThenInclude(x => x.Translates)
			                                        .Include(x => x.Application)
                                                        .ThenInclude(x => x.ClaimApplication)
                                                            .ThenInclude(x => x.ClaimTheme)
                                                                .ThenInclude(x => x.Translates)
			                                        .Include(x => x.Application)
                                                        .ThenInclude(x => x.ClaimApplication)
                                                            .ThenInclude(x => x.EmployeeManage)
                                                                .ThenInclude(x => x.Employee)
                                                                    .ThenInclude(x => x.Person)
			                                        .Include(t => t.Organization)
			                                        .Include(m => m.Contractor).ThenInclude(x => x.Region).ThenInclude(x => x.Translates)
			                                        .Include(m => m.Contractor.District).ThenInclude(x => x.Translates)
			                                        .Include(x => x.MeetingType).ThenInclude(x => x.Translates)
                                                    .AsSplitQuery()
			                                        .FirstOrDefault(m => m.Id2 == id2);

            if (mediationPlan == null)
            {
                AddError("Медиация режаси топилмади !");
                return null;
            }

            var orgId = mediationPlan.OrganizationId;
            var templateMap = new Dictionary<int, string>
            {
                { OrganizationIdConst.SSP, StaticFileConst.WordTemplate.MEDIATION_PLAN_SSP },
                { OrganizationIdConst.ANDIJON, StaticFileConst.WordTemplate.MEDIATION_PLAN_ANDIJON },
                { OrganizationIdConst.QASHQADARYO, StaticFileConst.WordTemplate.MEDIATION_PLAN_QASHQADARYO },
                { OrganizationIdConst.FARGONA, StaticFileConst.WordTemplate.MEDIATION_PLAN_FARGONA },
                { OrganizationIdConst.XORAZM, StaticFileConst.WordTemplate.MEDIATION_PLAN_XORAZM },
                { OrganizationIdConst.NAVOIY, StaticFileConst.WordTemplate.CLAIM_0R_COUNTERCLAIM_APPLICATION_NAVOIY },
                { OrganizationIdConst.SAMARQAND, StaticFileConst.WordTemplate.MEDIATION_PLAN_SAMARQAND },
                { OrganizationIdConst.JIZZAX, StaticFileConst.WordTemplate.MEDIATION_PLAN_JIZZAX },
                { OrganizationIdConst.SIRDARYO, StaticFileConst.WordTemplate.MEDIATION_PLAN_SIRDARYO },
                { OrganizationIdConst.BUXORO, StaticFileConst.WordTemplate.MEDIATION_PLAN_BUXORO },
                { OrganizationIdConst.QORAQALPOQ, StaticFileConst.WordTemplate.MEDIATION_PLAN_QORAQALPOQ },
                { OrganizationIdConst.SURXONDARYO, StaticFileConst.WordTemplate.MEDIATION_PLAN_SURXONDARYO },
                { OrganizationIdConst.TOSHKENT_VIL, StaticFileConst.WordTemplate.MEDIATION_PLAN_TOSHKENT_VIL },
                { OrganizationIdConst.TOSHKENT, StaticFileConst.WordTemplate.MEDIATION_PLAN_TOSHKENT },
                { OrganizationIdConst.NAMANGAN, StaticFileConst.WordTemplate.MEDIATION_PLAN_NAMANGAN }
            };

            var wordFile = templateMap.TryGetValue(orgId, out var template)
                ? _storageService.GetStaticFile(StaticFileConst.WordTemplate.GetFileName(language, template))
                : _storageService.GetStaticFile(StaticFileConst.WordTemplate.GetFileName(language, StaticFileConst.WordTemplate.MEDIATION_PLAN));

            var lang = _unitOfWork.Context.Set<Language>()
				.FirstOrDefault(l => l.Code == language)?.Id ?? 1;

			var month = _manualService
				.GetMonthSelectList()
				.ToDictionary(month => month.Value, month => month.Text);


			if (mediationPlan == null)
			{
				AddError("Медиацийта режаси топилмади !");
				return null;
			}

			var plh = new Placeholders();
			var link = _systemConf.QrImagePrintMy + "/MediationPlan/DownloadPdf?id2=" + mediationPlan.Id2.ToString();
			var qrDownload = new MemoryStream(QRCodeHelper.GeneratePng(link));
			plh.ImagePlaceholders.Add("QrCode", new() { Dpi = 512, MemStream = qrDownload });

			plh.TextPlaceholders.Add("DocNumber", mediationPlan.DocNumber);
			plh.TextPlaceholders.Add("DocOn", mediationPlan.DocOn.ToString());
			plh.TextPlaceholders.Add("Direktor", mediationPlan.DocOn.ToString());

			plh.TextPlaceholders.Add("ContractorFullName", mediationPlan.Contractor?.FullName);

			plh.TextPlaceholders.Add("ContractorRegionName", mediationPlan.Contractor.Region.Translates.AsQueryable()
				.FirstOrDefault(RegionTranslate.GetExpr(
					DataLayer.TranslateColumn.full_name, lang))
				?.TranslateText ?? mediationPlan.Contractor.Region.FullName);

			plh.TextPlaceholders.Add("ContractorDistrictName", mediationPlan.Contractor.District.Translates.AsQueryable()
				.FirstOrDefault(DistrictTranslate.GetExpr(
					DataLayer.TranslateColumn.full_name, lang))
				?.TranslateText ?? mediationPlan.Contractor.District.FullName);

			var responsibles = _unitOfWork.Context.Set<ClaimApplicationTable>()
                .Include(x => x.Owner)
				.Where(a => a.Owner.ApplicationId == mediationPlan.ApplicationId).ToList();

			plh.TextPlaceholders.Add("Address", responsibles.FirstOrDefault()?.Address ?? mediationPlan.Contractor.Address);

			List<Placeholders> tables = new List<Placeholders>();
			foreach (var item in responsibles)
			{
                if (item != null && !string.IsNullOrEmpty(item.FullName) && !string.IsNullOrEmpty(item.Address))
                {
                    var tplh = new Placeholders();
                    tplh.TextPlaceholders.Add("ResponsiblePersonFullName", item.FullName);
                    tplh.TextPlaceholders.Add("ResponsiblePersonAddress", item.Address);
                    tables.Add(tplh);
                }
            }
            if (tables.Any())
                plh.TemplateListPlaceholders.Add("Tables", tables);

            plh.TextPlaceholders.Add("ResponsiblePersonFullNames", string.Join(", ", responsibles.Select(x => x.FullName)));

			plh.TextPlaceholders.Add("ClaimApplicationTypeName", mediationPlan?.Application?.ClaimApplication?.ClaimApplicationType?.Translates?.AsQueryable()
				?.FirstOrDefault(ClaimApplicationTypeTranslate.GetExpr(
					DataLayer.TranslateColumn.full_name, lang))?.TranslateText ?? mediationPlan?.Application?.ClaimApplication?.ClaimApplicationType?.FullName ?? "");

			plh.TextPlaceholders.Add("ClaimThemeName", mediationPlan?.Application?.ClaimApplication?.ClaimTheme?.Translates?.AsQueryable()
				?.FirstOrDefault(ClaimThemeTranslate.GetExpr(
					DataLayer.TranslateColumn.full_name, lang))?.TranslateText ?? mediationPlan?.Application?.ClaimApplication?.ClaimTheme?.FullName ?? "");

			//plh.TextPlaceholders.Add("MeetingTypeName", mediationPlan.MeetingType.Translates.AsQueryable()
			//    .FirstOrDefault(MeetingTypeTranslate.GetExpr(
			//        DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
			//    ?.TranslateText ?? mediationPlan.MeetingType.FullName);

			plh.TextPlaceholders.Add("MeetingTypeName", mediationPlan.MeetingType.Code == "002" ? null :
			mediationPlan.MeetingType.Translates.AsQueryable()
				.FirstOrDefault(MeetingTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, lang))?.TranslateText ?? mediationPlan.MeetingType.FullName);

			plh.TextPlaceholders.Add("AddressOrUrl", mediationPlan.AddressOrUrl);
            string formattedDate = string.Empty;
            var meetingAt = mediationPlan.MeditionAt;
            string[][] months = {
                new[] { "yanvar", "fevral", "mart", "aprel", "may", "iyun", "iyul", "avgust", "sentyabr", "oktyabr", "noyabr", "dekabr" },
                new[] { "январь", "февраль", "март", "апрель", "май", "июнь", "июль", "август", "сентябрь", "октябрь", "ноябрь", "декабрь" }
            };

            int langIndex = language.Equals("uz-latn") ? 0 : 1;
            formattedDate = $"{meetingAt.Year}-yilning {meetingAt.Day}- {months[langIndex][meetingAt.Month - 1]} kuni soat {meetingAt.Hour:D2}:{meetingAt.Minute:D2} da";

            var createdUser = _unitOfWork.Context.Set<User>().Include(u => u.Person).FirstOrDefault(u => u.Id == mediationPlan.CreatedUserId);
            string fullName = createdUser?.Person?.FullName ?? "";

            plh.TextPlaceholders.Add("MeetingAtData", formattedDate);
            plh.TextPlaceholders.Add("UserName", fullName);
			plh.TextPlaceholders.Add("PhoneNumber", mediationPlan.Application.ClaimApplication?.EmployeeManage?.Employee?.PhoneNumber ?? "");

			wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();
			var res = _pdfConverter.DocxToPdfAsync(wordFile, new()).Result;
			CombineStatuses(_pdfConverter);
			return res;
		}
		private HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
        {
            var moveDto = _repository.ById<MediationPlanDto>(id, applyFilter: false);
            _documentChangeLogService.Create(
                dto: moveDto,
                tableId: TableIdConst.CLAIM__DOC_MEDIATION_PLAN,
                organizationId: null,
                statusId: statusId,
                message: message);
            CombineStatuses(_documentChangeLogService);

            if (HasErrors)
                return null;

            return HaveId.Create(id);
        }
        public void Delete(long id)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = _repository.ById(id);
                    if (entity != null && entity.PreviousMediationPlan != null)
                    {
                        entity.PreviousMediationPlan.NextMediationPlanId = null;
                    }

                    if (_unitOfWork.Context.Set<Application>()
                        .FirstOrDefault(a => a.Id == entity.ApplicationId).ApplicationTypeId == ApplicationTypeIdConst.CLAIM)
                    {
                        AddError("Медиация режаси тури 'Даволик азираси'. Буни ўчира олмайсиз.");
                        return;
                    }

                    var statusDto = new UpdateStatusMediationPlanDlDto()
                    {
                        Id = id,
                        StatusId = StatusIdConst.DELETED
                    };

                    var res = CreateDocumentChangeLog(entity.Id, StatusIdConst.DELETED, "DeleteMediationPlan");
                    _repository.UpdateStatus(statusDto);
                    _unitOfWork.Save();

                    if (IsValid)
                    {
                        transaction.Commit();
                    }
                }
                catch (DbUpdateException ex)
                {
                    AddError(ex.Message);
                    transaction.Rollback();
                }
            }
        }
        private async Task<HaveId<long>> UpdateStatus(UpdateStatusMediationPlanDlDto dto, Action<MediationPlan> validation)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var meditionPlan = _repository.ById(dto.Id);
                    if (meditionPlan is null)
                    {
                        AddError("Такого плана посредничества не существует");
                        return null;
                    }
                    int oldStatusId = meditionPlan.StatusId;

                    var entity = _repository.UpdateStatus(dto);
                    CombineStatuses(_repository);
                    if (HasErrors) return null;

                    _claimApplicationRepository.UpdateStep(
                        new UpdateStepDlDto
                        {
                            Id = meditionPlan.ApplicationId,
                            StepId = dto.StatusId == StatusIdConst.ACCEPTED ? StepIdConst.MEDIATION_PLAN_CREATE : StepIdConst.MEDIATION_PLAN_CANCEL
                        });

                    CombineStatuses(_claimApplicationRepository);
                    if (HasErrors) return null;

                    _unitOfWork.Save();

                    var res = CreateDocumentChangeLog(entity.Id, entity.StatusId);

                    if (IsValid)
                    {
                        var businessmanUserId = _unitOfWork.Context.Set<Application>()
                            .FirstOrDefault(a => a.Id == entity.ApplicationId).CreatedUserId;

                        if (businessmanUserId.HasValue)
                        {
                            var phoneNumber = _unitOfWork.Context
                                .Set<BusinessmanUser>()
                                .FirstOrDefault(a => a.Id == businessmanUserId && a.StateId != StateIdConst.PASSIVE)
                                .UserName;

                            await _sendSmsService.SendSms(Convert.ToInt32(entity.Id), oldStatusId, dto.StatusId, phoneNumber);
                        }

                        transaction.Commit();

                        return res;
                    }
                }
                catch (Exception ex)
                {
                    AddError(ex.Message);
                    transaction.Rollback();
                }
            }
            return null;
        }
        private void Validation<TDto>(MediationPlanDlDto<TDto> dto, MediationPlan entity)
            where TDto : MediationPlanDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
            {
                query = query.Where(a => a.Id != entity.Id);

                if (!StatusIdConst.CanModifyStatus.Contains(entity.StatusId))
                    AddError("Таҳрирлаш мумкин эмас / Невозможно редактировать");
            }
            //if(query.ByDocNumber(dto.DocNumber).Any())
            //    _repository.AddError($"Документ с этим кодом {dto.DocNumber} уже существует", nameof(dto.DocNumber));

            var application = _unitOfWork.ApplicationRepository.ById(dto.ApplicationId);
            if (application.ApplicationTypeId != ApplicationTypeIdConst.CLAIM)
                AddError("Ariza turi xato tanlangan");

            if (dto.MeetingTypeId == MeetingTypeIdConst.ONLINE)
            {
                if (!(Uri.TryCreate(dto.AddressOrUrl, UriKind.Absolute, out Uri uriResult)
                      && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)))
                {
                    AddError("Видео конференция учун киритилган ҳавола нотӯғри форматда / Ссылка на видеоконференцию неверном формате");
                }
            }
            if (entity.MeetingTypeId == MeetingTypeIdConst.OFFLINE)
            {

            }
        }
    }
}