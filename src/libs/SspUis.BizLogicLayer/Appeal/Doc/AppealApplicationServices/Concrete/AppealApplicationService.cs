using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OfficeOpenXml;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.DistrictServices;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.RegionServices;
using SspUis.BizLogicLayer.Srv;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Extensions;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Appeal;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Appeal;
using SspUis.Integration.DocxToPdf;
using SspUis.Integration.Edoc;
using SspUis.Integration.Edoc.Models;
using SspUis.ServiceLayer.NumberServices;
using WbImzo.Models;
using WbImzo.Proxy.Sdk;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.OfficeTools.Factory;
using WEBASE.OfficeTools.Handlers;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Appeal;

public class AppealApplicationService
    : BaseEntityService<long,
        AppealApplication,
        AppealApplicationListDto,
        AppealApplicationDto,
        CreateAppealApplicationDlDto,
        UpdateAppealApplicationDlDto,
        IAppealApplicationRepository,
        AppealApplicationSortFilterOptions>
    ,   IAppealApplicationService
{
    private readonly IDocumentChangeLogService _documentChangeLogService;
    private readonly INumberService _numberService;
    //private readonly IAppealApplicationRepository Repository;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEImzoService _eImzoService;
    private readonly IConvertService _pdfConverter;
    private readonly IStorageService _storageService;
    private readonly IContractorService _contractorService;
    private readonly IEdocRegistrateService _edocRegistrateService;
    private readonly IApiRequestLogRepository _apiRequestLogRepository;
    private readonly ICultureHelper _cultureHelper;
    private readonly IExternalDocFromEdocService _externalDocFromEdocService;
    private readonly IWbImzoService _wbImzoService;
    private readonly WbImzoConfig _wbImzoConfig;
    private readonly LinkConfig _linkConfig;

    public AppealApplicationService(
        IUnitOfWork unitOfWork,
        INumberService numberService,
        IAuthService authService,
        IDocumentChangeLogService documentChangeLogService,
        IEImzoService eImzoService,
        IStorageService storageService,
        ICultureHelper cultureHelper,
        IConvertService pdfConverter,
        IContractorService contractorService,
        WbImzoConfig wbImzoConfig,
        List<LinkConfig> linkConfigs,
        IWbImzoService wbImzoService,
        IEdocRegistrateService edocRegistrateService,
        IApiRequestLogRepository apiRequestLogRepository,
        IExternalDocFromEdocService externalDocFromEdocService) : base(unitOfWork)
    {
        _documentChangeLogService = documentChangeLogService;
        _numberService = numberService;
        _unitOfWork = unitOfWork;
        //Repository = unitOfWork.AppealApplicationRepository;
        _authService = authService;
        _cultureHelper = cultureHelper;
        _eImzoService = eImzoService;
        _storageService = storageService;
        _pdfConverter = pdfConverter;
        _edocRegistrateService = edocRegistrateService;
        _contractorService = contractorService;
        _apiRequestLogRepository = apiRequestLogRepository;
        _externalDocFromEdocService = externalDocFromEdocService;
        _wbImzoService = wbImzoService;
        _wbImzoConfig = wbImzoConfig;
        _linkConfig = linkConfigs.FirstOrDefault(config => config.Name == "AppealApplication");
    }

    #region CRUD
    public PagedResult<AppealApplicationListDto> GetList(AppealApplicationSortFilterOptions dto)
    {
        var result = Repository.ReadAsNoTracked<AppealApplicationListDto>()
                                .SortFilter(dto)
                                .AsPagedResult(dto);
        return result;
    }


	public int GetCountMy()
	{
        AppealApplicationSortFilterOptions dto = new AppealApplicationSortFilterOptions();
		var result = Repository.ReadAsNoTracked<AppealApplicationListDto>()
							  .SortFilter(dto);
		return result.Count();
	}
	public long GetCount()
    {
        return _unitOfWork.Context.Set<AppealApplication>()
            .Where(x => x.StatusId != StatusIdConst.DELETED)
            .Count();
    }
    public SelectList<long> AsSelectList(AppealApplicationSortFilterOptions options)
    {
        return Repository.ReadAsNoTracked<AppealApplicationListDto>()
            .SortFilter(options)
            .AsSelectList();
    }
    public AppealApplicationDto Get()
    {
        if (_authService.Contractor != null)
        {
            var contractor = _unitOfWork.ContractorRepository.ById<ContractorDto>(_authService.Contractor.Id);
            var region = _unitOfWork.RegionRepository.ById<RegionListDto>(contractor.RegionId);
            var district = _unitOfWork.DistrictRepository.ById<DistrictListDto>(contractor.DistrictId);
            return new AppealApplicationDto
            {
                DocOn = DateOnly.FromDateTime(DateTime.Now),
                DistrictId = contractor.DistrictId,
                RegionId = contractor.RegionId,
                District = district.FullName,
                Region = region.FullName,
                Contractor = contractor,
                ContractorInn = contractor.Inn,
                DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_APPEAL_APPLICATION, 1).Item2,
                Person = new()
            };
        }
        else
            return new AppealApplicationDto
            {
                DocOn = DateOnly.FromDateTime(DateTime.Now),
                DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_APPEAL_APPLICATION, 1).Item2,
                //Person = new()
                //{
                //    Inn = "",
                //    PassportSeria = ""
                //},
            };
    }
    public AppealApplicationDto Get(long id)
    {
        var dto = Repository.ById<AppealApplicationDto>(id);
        CombineStatuses(Repository);
        if (IsValid)
        {
            if (_authService.Contractor == null)
            {
                dto.CanAccept = StatusIdConst.CanAppealApplicationApplyStatus(dto.StatusId, StatusIdConst.ACCEPTED);
                dto.CanReject = StatusIdConst.CanAppealApplicationApplyStatus(dto.StatusId, StatusIdConst.REJECTED);
                dto.CanSendToEdoc = (dto.StatusId == StatusIdConst.CREATED || dto.StatusId == StatusIdConst.SENT || dto.StatusId == StatusIdConst.MODIFIED)
                    && _authService.HasPermission(ModuleCode.AppealApplicationSendToEdoc);
            }
            else
            {
                dto.CanEdit = StatusIdConst.CanAppealApplicationApplyStatus(dto.StatusId, StatusIdConst.MODIFIED);
                dto.CanSign = StatusIdConst.CanAppealApplicationApplyStatus(dto.StatusId, StatusIdConst.SENT);
                dto.CanDelete = StatusIdConst.CanAppealApplicationApplyStatus(dto.StatusId, StatusIdConst.DELETED);
            }
            if (dto.StatusId == StatusIdConst.IN_EXECUTION || dto.StatusId == StatusIdConst.EXECUTED || dto.StatusId == StatusIdConst.HAS_EDOC_RESPONSE)
            {
                var edocInfoDb = _externalDocFromEdocService.GetByAppealId(dto.Id);

                dto.EdocInfo = _edocRegistrateService.Get(dto.Id).Result;
                if (edocInfoDb == null && dto.EdocInfo != null)
                {
                    _externalDocFromEdocService.Create(new()
                    {
                        TermExecution = dto.EdocInfo.TermExecution,
                        Assignment = dto.EdocInfo.Assignment,
                        OrganizationId = dto.OrganizationId,
                        ProcessId = dto.EdocInfo.ProcessId,
                        AppealApplicationId = dto.Id,
                        CallCenterAppealId = null,
                        RegNumber = dto.EdocInfo.RegNumber,
                        RegDate = dto.EdocInfo.RegDate
                    });
                    CombineStatuses(_externalDocFromEdocService);
                }

                if (dto.EdocInfo != null)
                {
                    if (dto.EdocInfo?.Attachments == null || dto.EdocInfo?.Attachments.Count == 0 || dto.StatusId == StatusIdConst.IN_EXECUTION)
                    {
                        dto.EdocInfo.Attachments = new();
                    }

                    if (dto.EdocInfo.ProcessId != 27)//   Revork (Qoralamada turgan ProcessId = 27 bo'ladi)
                    {
                        dto.CanEdit = false;
                    }

                    dto.EdocInfo.Organization = dto.Organization;
                }
            }
        }
        return dto;
    }
    public AppealApplicationDto Get(string docNumber)
    {
        var dto = Repository.ReadAsNoTracked<AppealApplicationDto>(applyFilter: false).FirstOrDefault(x => x.DocNumber == docNumber);
        CombineStatuses(Repository);
        if (IsValid)
        {
            if (_authService.Contractor == null)
            {
                dto.CanAccept = StatusIdConst.CanAppealApplicationApplyStatus(dto.StatusId, StatusIdConst.ACCEPTED);
                dto.CanReject = StatusIdConst.CanAppealApplicationApplyStatus(dto.StatusId, StatusIdConst.REJECTED);
                dto.CanSendToEdoc = (dto.StatusId == StatusIdConst.CREATED || dto.StatusId == StatusIdConst.SENT || dto.StatusId == StatusIdConst.MODIFIED || dto.StatusId == StatusIdConst.REJECTED)
                    && _authService.HasPermission(ModuleCode.AppealApplicationSendToEdoc);
            }
            else
            {
                dto.CanEdit = StatusIdConst.CanAppealApplicationApplyStatus(dto.StatusId, StatusIdConst.MODIFIED);
                dto.CanSign = StatusIdConst.CanAppealApplicationApplyStatus(dto.StatusId, StatusIdConst.SENT);
                dto.CanDelete = StatusIdConst.CanAppealApplicationApplyStatus(dto.StatusId, StatusIdConst.DELETED);
            }
        }

        return dto;
    }
    public async Task<HaveId<long>> Create(CreateAppealApplicationDlDto dto)
    {
        var canCommit = _unitOfWork.Context.Database.CurrentTransaction == null;
        var transaction = canCommit ? await _unitOfWork.Context.Database.BeginTransactionAsync() : null;
        try
        {
            if (dto.Person != null)
            {
                Person person = _unitOfWork.PersonRepository.ByPinfl(dto.Person.Pinfl);

                if (person == null)
                {
                    person = _unitOfWork.PersonRepository.Create(dto.Person);
                    CombineStatuses(_unitOfWork.PersonRepository);
                    if (HasErrors)
                        return null;
                    UnitOfWork.Save();
                }
                dto.PersonId = person.Id;
                dto.PersonFullName = person.FullName;
            }
            if (dto.ContractorInn != null)
            {
                Contractor contractor = _unitOfWork.ContractorRepository.ByInn(dto.ContractorInn);

                if (contractor != null)
                    dto.ContractorId = contractor.Id;
                else if (contractor == null && dto.ContractorInn != null)
                {
                    var contractorDto = _contractorService.GetByInnFromSoliq(dto.ContractorInn);
                    var mc = new AutoMapper.MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<ContractorDto, CreateContractorDlDto>();
                    });
                    var createContractorDlDto = mc.CreateMapper().Map<CreateContractorDlDto>(contractorDto.Result);

                    var contractorEntity = _contractorService.Create(createContractorDlDto);

                    CombineStatuses(_contractorService);
                    if (HasErrors)
                        return null;
                    _unitOfWork.Save();
                    dto.ContractorId = contractorEntity.Id;
                }
            }

            if (dto.ContractorId == null && dto.PersonId == null
                && dto.PersonFullName.NullOrWhiteSpace())
            {
                AddError("Murojatchi ma'lum emas. ContractorId va PersonId null");
            }
            dto.Person = null;
            var entity = Repository.Create(dto, ent => Validation(dto, ent));
            CombineStatuses(Repository);
            if (HasErrors)
            {
                if (canCommit)
                    transaction.Rollback();
                return null;
            }
            UnitOfWork.Save();
            _storageService.MoveToPersistent(DocumentStorageConst.DOC_APPEAL_APPLICATION, entity.Id.ToString(), dto.Files.Select(x => x.Id).ToArray());
            var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, message: "Hujjat yaratildi");

            if (dto.IsCreatedByChamber)
                await SendToEdoc(entity.Id, OrganizationIdConst.SSP, true);
            if (IsValid && canCommit)
            {
                transaction.Commit();
            }
            return HaveId.Create(entity.Id);
        }
        catch (Exception e)
        {
            AddError(e.Message + "Inner:" + e.InnerException);
            //if (canCommit)
            //    transaction.Rollback();
        }
        return null;

    }
    public async Task Update(UpdateAppealApplicationDlDto dto)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.Update(dto, ent => Validation(dto, ent));
                UnitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "UpdateAppealApplication");
                CombineStatuses(Repository);
                _storageService.ResolveMarkedFiles(DocumentStorageConst.DOC_APPEAL_APPLICATION, dto.Id.ToString());

                if (entity.StatusId == StatusIdConst.IN_EXECUTION)
                    await UpdateEdoc(dto);

                if (IsValid)
                    await transaction.CommitAsync();
                else
                    await transaction.RollbackAsync();
            }
            catch (DbUpdateException e)
            {
                AddError(e.Message);
                transaction.Rollback();
            }
        }
    }
    public override void Delete(long id)

    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.ById(id);
                var statusDto = new UpdateStatusAppealApplicationDlDto()
                {
                    Id = id,
                    StatusId = StatusIdConst.DELETED
                };
                UpdateStatus(statusDto, ent =>
                {
                    if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, statusDto.StatusId))
                        Repository.AddError("Нет доступа");
                });
                Repository.UpdateStatus(statusDto);
                UnitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, StatusIdConst.DELETED, "DeleteAppealApplication");

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

    public async Task SendToEdoc(long id, int organizationId, bool IsSendByChamber = false)
    {

        var log = new CreateApiRequestLogDlDto
        {
            DocumentId = id,
            TableId = TableIdConst.APPEAL__DOC_APPEAL_APPLICATION,
            UserId = (IsSendByChamber) ? 1 : (int)_authService.UserId,
            UserInfo = _authService.User?.ToString() ?? "",
            RequestAt = DateTime.Now,
        };

        //bool canCommit = UnitOfWork.CurrentTransaction == null;
        //using var transaction = canCommit ? UnitOfWork.BeginTransaction() : UnitOfWork.CurrentTransaction;

        var canCommit = _unitOfWork.Context.Database.CurrentTransaction == null;
        var transaction = canCommit ? _unitOfWork.BeginTransaction() : null;

        try
        {
            var entity = UnitOfWork.Context.Set<AppealApplication>().Include(a => a.Contractor).Include(a => a.Person).FirstOrDefault(x => x.Id == id);
            AppealApplicationDto doc = null;
            if (!IsSendByChamber)
            { doc = Get(id); }
            entity.OrganizationId = organizationId;
            UnitOfWork.Save();
            if (doc == null && !IsSendByChamber)
            {
                AddError("Malumot topilmadi");
                throw new Exception("Document not found");
            }

            if (!IsSendByChamber && !doc.CanSendToEdoc)
            {
                AddError("Имкони йўқ / Нет доступа");
                throw new Exception("Cannot send document to Edoc");
            }

            var employeId = _unitOfWork.Context.Set<Organization>()
                .FirstOrDefault(a => a.Id == entity.OrganizationId)?.IncomingDocReceiverEmployeeId;

            if (employeId == null)
            {
                AddError("Bu tashkilotga hodim biriktirilmagan");
            }

            var empManage = _unitOfWork.Context.Set<EmployeeManage>().FirstOrDefault(a => a.EmployeeId == employeId && !a.IsDeleted && a.EndOn == null);

            List<FileResultResponse> uploadedAttachments = new();
            if (entity.Files != null && entity.Files.Count > 0)
            {
                foreach (var file in entity.Files)
                {
                    var attachment = DownloadFile(file.Id);
                    if (attachment != null)
                        uploadedAttachments.Add((await UploadAttachment(attachment)).FirstOrDefault());
                }
            }

            var downloadedPdf = await DownloadPdf(id, null);
            IEnumerable<FileResultResponse> uploadedFiles = null;
            if (downloadedPdf != null)
                uploadedFiles = await UploadFile(downloadedPdf);

            CombineStatuses(_edocRegistrateService);
            if (HasErrors)
            {
                if (canCommit)
                    transaction.Rollback();
                return;
            }

            // Determine external document type
            int externalDocumentTypeId = (entity.Contractor != null) ? 1 : 2;

            // Prepare Edoc request
            var edocAppealApplication = new EdocRegisterRequestDto
            {
                //IsGenerate = false,
                DocDate = DateTime.Now,
                SenderName = (entity.Contractor?.FullName) ?? (entity.PersonFullName) ?? (entity.Person?.FullName ?? "Unknown"),
                DocNumber = entity.DocNumber,
                SpecialNotes = String.Empty,
                ExternalDocumentTypeId = externalDocumentTypeId,
                Document = new Document
                {
                    DocumentTypeId = 14,
                    Summary = entity.Details,
                    //TermExecution = DateTime.Now,
                    ActivityFieldId = null,
                    //RegDate = DateTime.Now,
                    //RegNumber = null,
                    ImportanceId = null,
                    ExternalDocumentId = entity.Id,
                    CallCenterAppealId = null,
                    EmployeeManageId = empManage?.Id
                },
                Files = uploadedFiles?.Select(fileId => new FileForEdoc { Id = fileId.FileId }).ToList() ?? new List<FileForEdoc>(),
                Attachments = uploadedAttachments?.Select(attachmentId => new AttachmentForEdoc { Id = attachmentId.FileId }).ToList() ?? new List<AttachmentForEdoc>()
            };
            await InExecution(new InExecutionStatusAppealApplicationDto { Id = entity.Id });
            if (HasErrors)
            {
                if (canCommit)
                    transaction.Rollback();
                return;
            }
            // Register document with Edoc
            ExternalIncomingDocumentDto result = await _edocRegistrateService.RegistrateEdoc(edocAppealApplication);
            CombineStatuses(_edocRegistrateService);

            // Check registration result
            if (result == null)
            {
                AddError("Result from Edoc is null");
                if (canCommit)
                    transaction.Rollback();
                return;
            }
            _externalDocFromEdocService.Create(new()
            {
                TermExecution = result.TermExecution,
                CallCenterAppealId = null,
                AppealApplicationId = entity.Id,
                Assignment = result.Assignment,
                OrganizationId = entity.OrganizationId,
                ProcessId = result.ProcessId,
                RegDate = result.RegDate,
                RegNumber = result.RegNumber
            });

            if (HasErrors)
            {
                if (canCommit)
                    transaction.Rollback();
                return;
            }
            // Further processing if successful

            log.IsSuccess = true;
            log.ResponseAt = DateTime.Now;
            log.ResponseContent = JsonConvert.SerializeObject(edocAppealApplication);

            // Commit transaction
            if (IsValid && canCommit)
                await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            // Rollback transaction on exception
            if (canCommit)
                transaction.Rollback();
            log.Exception = ex.Message;
            AddError($"Exception occurred: {ex.Message}");
            throw;
        }
        finally
        {
            //_apiRequestLogRepository.Create(log);
            //_unitOfWork.Save();
        }

    }

    #endregion

    #region Status control
    public async Task Sign(SignStatusAppealApplicationDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ??
            await _unitOfWork.Context.Database.BeginTransactionAsync();
        try
        {
            var entity = Repository.AllAsQueryable
                   .Include(x => x.Signs)
                   .FirstOrDefault(x => x.Id == dto.Id);

            if (entity == null)
            {
                AddError("Not found");
                transaction.Rollback();
                return;
            }
            var eImzoTimstampDto = new EImzoTimeStampDto
            {
                SignData = dto.SignedData,
                Inn = null,
                Pinfl = _authService.Contractor == null ? _authService.User.Pinfl : _authService.Contractor.Pinfl
            };
            var timeStamp = await _eImzoService.TimeStamp(eImzoTimstampDto);

            CombineStatuses(_eImzoService);
            if (HasErrors)
            {
                transaction.Rollback();
                return;
            };


            var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
            {
                SignData = timeStamp.Pkcs7b64,
                Inn = null,
                Pinfl = _authService.Contractor == null ? _authService.User.Pinfl : _authService.Contractor.Pinfl
            });

            var signer = new AppealApplicationSign()
            {
                SignFile = SaveFile(dto.Id, timeStamp.Pkcs7b64, "sign.txt"),
                DataFile = SaveFile(dto.Id, JsonConvert.SerializeObject(dto), "data.txt"),
                SignedAt = DateTime.Now,
                SignedUserInfo = _authService.User != null
                ? _authService.User.ToTextForDocumentLog()
                : _authService.Contractor.FullName + " - " + _authService.Contractor.Inn,
                StatusId = dto.StatusId,
            };
            entity.Signs.Add(signer);

            var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, dto.Message);

            AppealApplication ent = Repository.UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanAppealApplicationApplyStatus(ent.StatusId, StatusIdConst.SENT))
                    Repository.AddError("Нет доступа");
            });
            CombineStatuses(Repository);
            if (HasErrors)
                return;

            await UnitOfWork.Context.SaveChangesAsync();

            if (IsValid && canCommit)
                await transaction.CommitAsync();
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " - " + e.InnerException);
            if (canCommit)
                await transaction.RollbackAsync();
        }
    }
    public async Task Accept(AcceptStatusAppealApplicationDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? await _unitOfWork.Context.Database.BeginTransactionAsync();
        try
        {
            _unitOfWork.Context.Set<AppealApplication>().Lock(dto.Id);
            UpdateStatus(dto, ent => { });
            var res = CreateDocumentChangeLog(dto.Id, StatusIdConst.ACCEPTED, dto.Message);
            if (IsValid && canCommit)
                await transaction.CommitAsync();
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " - " + e.InnerException);
            if (canCommit)
                await transaction.RollbackAsync();
        }
    }
    public async Task Reject(RejectStatusAppealApplicationDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? await _unitOfWork.Context.Database.BeginTransactionAsync();
        try
        {
            _unitOfWork.Context.Set<AppealApplication>().Lock(dto.Id);
            UpdateStatus(dto, ent => { });
            var res = CreateDocumentChangeLog(dto.Id, StatusIdConst.REJECTED, dto.Message);
            if (IsValid && canCommit)
                await transaction.CommitAsync();
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " - " + e.InnerException);
            if (canCommit)
                await transaction.RollbackAsync();
        }
    }

    /// <summary>
    /// Edoc uchun metod. Har bir ariza edocga o'tganda shu metodni chaqirib ketadi.
    /// </summary>
    /// <param name="dto"> <see cref="UpdateStatusAppealApplicationDlDto.Id"/> ga qiymat brish yetarli.
    /// "<see cref="InExecutionStatusAppealApplicationDto.StatusId"/>" - ga o'zi qiymat set qiladi </param>
    /// <returns></returns>
    //public async Task InExecution(InExecutionStatusAppealApplicationDto dto)
    //{
    //    //var canCommit = _unitOfWork.CurrentTransaction == null;
    //    //var transaction = _unitOfWork.CurrentTransaction ?? await _unitOfWork.Context.Database.BeginTransactionAsync();
    //    var canCommit = _unitOfWork.Context.Database.CurrentTransaction == null;
    //    var transaction = canCommit ? _unitOfWork.BeginTransaction() : _unitOfWork.CurrentTransaction;
    //    try
    //    {
    //        _unitOfWork.Context.Set<AppealApplication>().Lock(dto.Id);
    //        UpdateStatus(dto, ent =>
    //        {
    //            if (!StatusIdConst.CanAppealApplicationApplyStatus(ent.StatusId, StatusIdConst.IN_EXECUTION))
    //                Repository.AddError("Нет доступа");

    //        });

    //        CombineStatuses(Repository);
    //        if (HasErrors)
    //            return;

    //        await UnitOfWork.Context.SaveChangesAsync();

    //        if (IsValid && canCommit)
    //            await transaction.CommitAsync();
    //    }
    //    catch (DbUpdateException e)
    //    {
    //        AddError(e.Message + " - " + e.InnerException);
    //        if (canCommit)
    //            await transaction.RollbackAsync();
    //    }
    //}
    public async Task InExecution(InExecutionStatusAppealApplicationDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = canCommit ? await UnitOfWork.Context.Database.BeginTransactionAsync() : null;
        try
        {
            //_unitOfWork.Context.Set<AppealApplication>().Lock(dto.Id);
            UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanAppealApplicationApplyStatus(ent.StatusId, StatusIdConst.IN_EXECUTION))
                    Repository.AddError("Нет доступа");

            });

            CombineStatuses(Repository);
            if (HasErrors)
                return;

            await UnitOfWork.Context.SaveChangesAsync();

            if (IsValid && canCommit)
                await transaction.CommitAsync();
            var res = CreateDocumentChangeLog(dto.Id, StatusIdConst.IN_EXECUTION, message: "Hujjat jo'natilgan");
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " - " + e.InnerException);
            if (canCommit)
                await transaction.RollbackAsync();
            //throw; // Rethrow the exception to be handled by the caller
        }
        finally
        {
            if (canCommit)
                transaction.Dispose();
        }
    }

    /// <summary>
    /// Edoc dan javob hati kelganda hujjat statusi shu statusga o'tadi.
    /// <list type="bullet">1. <see cref="InExecution(InExecutionStatusAppealApplicationDto)"/> chaqirganda edoc ga ketadi.</list>
    /// <list type="bullet">2. <see cref="Executed(ExecutedStatusAppealApplicationDto)"/> chaqirganda edoc dan javob hati kelgan bo'ladi.</list>
    /// <see cref="InExecution(InExecutionStatusAppealApplicationDto)"/> bilan bir hil ishlashi.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task Executed(ExecutedStatusAppealApplicationDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? await _unitOfWork.Context.Database.BeginTransactionAsync();
        try
        {
            //_unitOfWork.Context.Set<AppealApplication>().Lock(dto.Id);
            UpdateStatus(dto, ent => { });

            if (IsValid && canCommit)
                await transaction.CommitAsync();
            var res = CreateDocumentChangeLog(dto.Id, StatusIdConst.EXECUTED, message: "Ijro taminlangan");
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " - " + e.InnerException);
            if (canCommit)
                await transaction.RollbackAsync();
        }
    }

    public async Task HasEdocResponce(HasEdocResponceStatusAppealApplicationDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? await _unitOfWork.Context.Database.BeginTransactionAsync();
        try
        {
            //_unitOfWork.Context.Set<AppealApplication>().Lock(dto.Id);
            UpdateStatus(dto, ent => { });
            _externalDocFromEdocService.UpdateForAppeal(new()
            {
                AppealApplicationId = dto.Id,
                OutgoingDocCreatedData = DateTime.Now,
            });

            if (IsValid && canCommit)
            {

                await transaction.CommitAsync();
            }
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " - " + e.InnerException);
            if (canCommit)
                await transaction.RollbackAsync();
        }
    }
    private HaveId<long> UpdateStatus(UpdateStatusAppealApplicationDlDto dto, Action<AppealApplication> validation)
    {
        var canCommit = UnitOfWork.CurrentTransaction == null;
        var transaction = canCommit ? UnitOfWork.BeginTransaction() : null;
        validation += Validation(dto);
        try
        {
            var entity = Repository.UpdateStatus(dto, validation, false);// chamberdan keganda muammo bolgani uchun false qilib qo'ydim
            CombineStatuses(Repository);
            if (HasErrors)
            {
                if (canCommit)
                    transaction.Rollback();
                return null;
            }
            UnitOfWork.Save();
            var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, message: "AppealApplication");
            if (IsValid && canCommit)
                transaction.Commit();
            return res;
        }
        finally
        {
            if (canCommit)
                transaction.Dispose();
        }
    }
    #endregion

    #region Log, Validation, Pdf
    public HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
    {
        var moveDto = Repository.ById<AppealApplicationDto>(id, applyFilter: false);
        _documentChangeLogService.Create(
            dto: moveDto,
            tableId: TableIdConst.APPEAL__DOC_APPEAL_APPLICATION,
            organizationId: null,
            statusId: statusId,
            message: message);
        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }
    private Action<AppealApplication> Validation(UpdateStatusAppealApplicationDlDto dto)
    {
        return ent =>
        {
            if (!StatusIdConst.CanAppealApplicationApplyStatus(ent.StatusId, dto.StatusId))
                AddError("Имкони йўқ / Нет доступа");
        };
    }
    private void Validation<TDto>(AppealApplicationDlDto<TDto> dto, AppealApplication entity)
       where TDto : AppealApplicationDlDto<TDto>
    {
        var query = Repository.AllAsQueryable;

        if (entity != null)
        {
            query = query.Where(a => a.Id != entity.Id);

            if (!StatusIdConst.CanModifyStatus.Contains(entity.StatusId))
                AddError("Таҳрирлаш мумкин эмас / Невозможно редактировать");
        }

        if (query.ByDocNumber(dto.DocNumber).Any())
            Repository.AddError($"Документ с этим кодом {dto.DocNumber} уже существует", nameof(dto.DocNumber));

    }
    public Guid SaveFile(long docId, string data, string fileName)
    {
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(data);
        writer.Flush();
        ms.Position = 0;
        var fileInfo = _storageService.Save(
            $"{nameof(TableIdConst.APPEAL__DOC_APPEAL_APPLICATION)}_SIGN_DATA",
            docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

        return fileInfo.FirstOrDefault().FileId;
    }
    public async Task<byte[]> DownloadPdf(long id, string lang)
    {
        lang = lang ?? "uz-latn";
        var dto = Repository.ById<AppealApplicationDto>(id, applyFilter: false);
        MemoryStream wordFile = null;

        wordFile = _storageService.GetStaticFile(
           StaticFileConst.WordTemplate.GetFileName(
        lang,
        StaticFileConst.WordTemplate.APPEAL_APPLICATION));

        var plh = WordFactory.MakePlaceholders(dto);
        var handler = new DocXHandler(wordFile, plh);
        handler.ReplaceLists();
        handler.ReplaceTexts();
        handler.ReplaceImages();
        var res = await _pdfConverter.DocxToPdfAsync(wordFile, new());
        CombineStatuses(_pdfConverter);
        return res;
    }
    #endregion

    #region File
    public IEnumerable<AppealApplicationFileDto> UploadFiles(params StorageFile[] files)
    {
        var result = _storageService.SaveTemp(DocumentStorageConst.DOC_APPEAL_APPLICATION, files)
            .Select(a => new AppealApplicationFileDto
            {
                Id = a.FileId,
                FileName = a.FileName
            });
        CombineStatuses(_storageService);
        return IsValid ? result : null;
    }
    public StorageFile DownloadFile(Guid fileId)
    {
        var entity = _unitOfWork.Context.Set<AppealApplicationFile>().FirstOrDefault(a => a.Id == fileId);
        return Download(fileId, entity, DocumentStorageConst.DOC_APPEAL_APPLICATION);
    }
    public async Task<byte[]> DownloadAttachment(Guid fileId, bool isView)
    {
        var res = await _edocRegistrateService.DownloadAttachment(fileId, isView);
        CombineStatuses(_edocRegistrateService);
        return res;
    }
    public void DeleteFile(Guid fileId)
    {
        var entity = _unitOfWork.Context
            .Set<AppealApplicationFile>()
            .FirstOrDefault(a => a.Id == fileId);

        Delete(fileId, entity, DocumentStorageConst.DOC_APPEAL_APPLICATION);
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
            CombineStatuses(_storageService);

            if (IsValid)
                file.FileName = entity.FileName;
        }

        return file;
    }
    public Stream PrinAppealApplicationExcel(AppealApplicationSortFilterOptions dto)
    {
        //var data = GetList(dto);
        var data = Repository.ReadAsNoTracked<AppealApplicationListDto>()
                       .SortFilter(dto)
                       .Take(1000000)
                       .ToList();


        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.DOC_APPEAL_APPLICATION_LIST));

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
                ws.Cells[currentRow, column++].Value = item.PersonFullName ?? item.PersonName;
                ws.Cells[currentRow, column++].Value = item.ContractorInn ?? "";
                ws.Cells[currentRow, column++].Value = item.Contractor ?? "";
                ws.Cells[currentRow, column++].Value = item.Region;
                ws.Cells[currentRow, column++].Value = item.District;
                ws.Cells[currentRow, column++].Value = item.AppealType ?? "";
                ws.Cells[currentRow, column++].Value = item.AppealFormatType ?? "";
                ws.Cells[currentRow, column++].Value = item.PhoneNumber ?? "";
                ws.Cells[currentRow, column++].Value = item.AppealTypeArrive ?? "";
                ws.Cells[currentRow, column++].Value = item.Status;
                currentRow++;
            }
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    #endregion

    #region EDOC
    public async Task<IEnumerable<FileResultResponse>> UploadFile(byte[] file)
    {
        try
        {
            var res = await _edocRegistrateService.UploadFile(file);
            CombineStatuses(_edocRegistrateService);
            return (IEnumerable<FileResultResponse>)res;
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
            return null;
        }
    }
    public async Task<IEnumerable<FileResultResponse>> UploadAttachment(StorageFile file)
    {
        try
        {
            var res = await _edocRegistrateService.UploadAttachment(file);
            CombineStatuses(_edocRegistrateService);
            return (IEnumerable<FileResultResponse>)res;
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
            return null;
        }
    }
    private async Task UpdateEdoc(UpdateAppealApplicationDlDto dto)
    {
        var log = new CreateApiRequestLogDlDto
        {
            DocumentId = dto.Id,
            TableId = TableIdConst.APPEAL__DOC_APPEAL_APPLICATION,
            UserId = (int)_authService.UserId,
            UserInfo = _authService.User?.ToString() ?? "",
            RequestAt = DateTime.Now,
        };

        try
        {
            var doc = Get(dto.Id);
            int externalDocumentTypeId = (doc.Contractor != null) ? 1 : 2;
            var employeId = _unitOfWork.Context.Set<Organization>()
                .FirstOrDefault(a => a.Id == doc.OrganizationId)?.IncomingDocReceiverEmployeeId;

            if (employeId == null)
            {
                AddError("Bu tashkilotga hodim biriktirilmagan");
                return;
            }

            var empManage = _unitOfWork.Context.Set<EmployeeManage>().FirstOrDefault(a => a.EmployeeId == employeId);

            List<FileResultResponse> uploadedAttachments = new();
            if (doc.Files != null && doc.Files.Count > 0)
            {
                foreach (var file in doc.Files)
                {
                    var attachment = DownloadFile(file.Id);
                    if (attachment != null)
                        uploadedAttachments.Add((await UploadAttachment(attachment)).FirstOrDefault());
                }
            }

            var downloadedPdf = await DownloadPdf(dto.Id, null);
            IEnumerable<FileResultResponse> uploadedFiles = null;
            if (downloadedPdf != null)
                uploadedFiles = await UploadFile(downloadedPdf);

            var edocAppealApplication = new EdocRegisterRequestDto
            {
                SenderName = (doc.Contractor?.FullName) ?? (doc.PersonFullName) ?? (doc.Person?.FullName ?? "Unknown"),
                DocNumber = doc.DocNumber,
                SpecialNotes = string.Empty,
                ExternalDocumentTypeId = externalDocumentTypeId,
                Document = new Document
                {
                    DocumentTypeId = 14,
                    Summary = doc.Details,
                    //TermExecution = DateTime.Now,
                    ActivityFieldId = null,
                    //RegDate = DateTime.Now,
                    //RegNumber = null,
                    ImportanceId = null,
                    ExternalDocumentId = doc.Id,
                    CallCenterAppealId = null,
                    EmployeeManageId = empManage?.Id
                },
                Files = uploadedFiles?.Select(fileId => new FileForEdoc { Id = fileId.FileId }).ToList() ?? new List<FileForEdoc>(),
                Attachments = uploadedAttachments?.Select(attachmentId => new AttachmentForEdoc { Id = attachmentId.FileId }).ToList() ?? new List<AttachmentForEdoc>()
            };


            await _edocRegistrateService.UpdateEdoc(edocAppealApplication);
            CombineStatuses(_edocRegistrateService);
        }
        catch (Exception ex)
        {
            if (_unitOfWork.CurrentTransaction != null)
                await _unitOfWork.CurrentTransaction.RollbackAsync();
            log.Exception = ex.Message;
            AddError($"Exception occurred: {ex.Message}");
        }
        finally
        {
            _apiRequestLogRepository.Create(log);
            _unitOfWork.Save();
        }

    }

    public async ValueTask<string> WebImzoSign(WebImzoSignedFilter filter)
    {
        var doc = _unitOfWork.Context.Set<AppealApplication>().Include(a => a.Contractor)
                            .Include(x => x.Signs)
                            .FirstOrDefault(x => x.Id == filter.Id && x.StatusId != StatusIdConst.DELETED);

        if (doc == null)
        {
            AddError("По вашему запросу запись не найдено");
            return null;
        }

        return await PostToIMZOAndSentUrl(doc);
    }
    #endregion
    public async ValueTask<string> PostToIMZOAndSentUrl(AppealApplication application)
    {
        var canDispose = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        WbImzoCreateSignRequestDto signRequestCreateDto = CreatDtoForRequestSign(application);
        signRequestCreateDto.SignRequestUsers = new List<WbImzoCreateSignRequestUserDto>
                                                    {
                                                        new WbImzoCreateSignRequestUserDto
                                                        {
                                                            UserKey = !string.IsNullOrEmpty(application.Contractor.Inn) ? application.Contractor.Inn : application.Contractor.Pinfl,
                                                            UserInfo = _authService.User != null
                                                                                   ? _authService.User.ToTextForDocumentLog()
                                                                                   : _authService.Contractor.FullName + " - " + _authService.Contractor.Inn,
                                                            UserId = (int)application.PersonId,
                                                            DocStatusId = StatusIdConst.SIGNED,
                                                            SignPriority = 1,
                                                            IpAddress = _authService.UserIp,
                                                            UserAgent = _authService.UserAgent,
                                                            UserPhoneNumber = application.PhoneNumber
                                                        }
                                                    };

        var wbImzoResult = await _wbImzoService.UpsertStringFilePrintableLinkRequestAsync(signRequestCreateDto);
        if (!wbImzoResult.IsSuccess && wbImzoResult.Response is null)
            CombineStatuses(wbImzoResult.GetStatusGeneric());

        if (HasErrors || wbImzoResult.Response is null)
        {
            AddError("Murojat imzolash uchun yuborilayotgan jarayonda xatolik yuz berdi");
            return null;
        }

        try
        {
            application.WebImzoRequestId = wbImzoResult.Response.RequestId;
            application.WebImzoSecretKey = wbImzoResult.Response.SecretKey;

            _unitOfWork.Save();
            CombineStatuses(this);
            if (canDispose)
                transaction.Commit();
        }
        catch (Exception ex)
        {
            AddError("Document did not update" + ex.Message);
        }

        return await SendUrl(application.Id);
    }

    private async ValueTask<string?> SendUrl(long applicationId)
    {

        var canDispose = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        var contract = _unitOfWork.Context.Set<AppealApplication>().FirstOrDefault(a => a.Id == applicationId);

        if (contract.WebImzoRequestId == null || contract.WebImzoSecretKey.IsNullOrEmpty())
        {
            AddError("Murojat imzolash uchun yuborilayotgan jarayonda qaytgan keylani saqlashda xatolik yuz berdi");
            return null;
        }

        var url = (_wbImzoConfig.Api.Contains("api/") ? _wbImzoConfig.Api.Replace("api/", null) : _wbImzoConfig.Api) + $"app?requestId={contract.WebImzoRequestId}&secretKey={contract.WebImzoSecretKey}";

        return url;

    }

    public WbImzoCreateSignRequestDto CreatDtoForRequestSign(AppealApplication application)
    {
        var organization = _unitOfWork.OrganizationRepository.AllAsQueryable.FirstOrDefault(a => a.Id == application.OrganizationId);

        if (organization == null)
        {
            AddError("Organization topilmadi!");
        }

        if (application.ContractorId == null)
        {
            AddError("Contractor topilmadi");
        }

		string documentDataAsString = JsonConvert.SerializeObject(application, new JsonSerializerSettings
		{
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore
		});
		var filePrintableLink = $"{_linkConfig.Api}{_linkConfig.DownloadRoute}{application.Id}";

        return new WbImzoCreateSignRequestDto
        {
            ApiKey = _wbImzoConfig.ApiKey,
            Title = application.DocNumber,
            IsForceCreate = true,
            TableId = TableIdConst.APPEAL__DOC_APPEAL_APPLICATION,
            SignData = documentDataAsString,
            OrganizationInn = organization != null ? organization.Inn : null,
            OrganizationName = organization != null ? organization.FullName : null,
            PrintableLink = filePrintableLink,
            SignRequestActionTypes = new()
            {
                new WbImzoCreateSignRequestActionTypeDto
                {
                    ActionTypeId =  StatusIdConst.SIGNED,
                    ActionTypeName = "SIGNED",
                        Translates = new()
                        {
                            new ActionTranslateTypeDto
                            {
                                LanguageCode = LanguageCodeConst.RU,
                                Name =  "Я согласен"
                            },
                           new ActionTranslateTypeDto
                           {
                                LanguageCode = LanguageCodeConst.UZ_LATN,
                                Name = "Roziman"
                           },
                           new ActionTranslateTypeDto
                           {
                                LanguageCode = LanguageCodeConst.UZ_CYRL,
                                Name = "Розиман"
                           },
                        }
                },
            },
            SignatureMethodIds = new List<int> { SignatureMethodIdConst.E_IMZO },
            TemplateId = 28
        };
    }
}
