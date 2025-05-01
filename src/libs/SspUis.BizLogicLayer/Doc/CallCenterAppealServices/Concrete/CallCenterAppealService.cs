using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.DistrictServices;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.RegionServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.DocxToPdf;
using SspUis.Integration.Edoc;
using SspUis.Integration.Edoc.Models;
using SspUis.ServiceLayer.NumberServices;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.OfficeTools.Factory;
using WEBASE.OfficeTools.Handlers;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public class CallCenterAppealService
    : BaseEntityService<long, CallCenterAppeal, CallCenterAppealListDto, CallCenterAppealDto, CreateCallCenterAppealDlDto, UpdateCallCenterAppealDlDto, ICallCenterAppealRepository, CallCenterAppealSortFilterOptions>
    , ICallCenterAppealService
{

    private readonly IDocumentChangeLogService _documentChangeLogService;
    private readonly INumberService _numberService;
    private readonly ICallCenterAppealRepository _repository;
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

    public CallCenterAppealService(
        IUnitOfWork unitOfWork,
        INumberService numberService,
        IAuthService authService,
        IDocumentChangeLogService documentChangeLogService,
        IEImzoService eImzoService,
        IStorageService storageService,
        ICultureHelper cultureHelper,
        IConvertService pdfConverter,
        IContractorService contractorService,
        IEdocRegistrateService edocRegistrateService,
        IApiRequestLogRepository apiRequestLogRepository,
        IExternalDocFromEdocService externalDocFromEdocService) : base(unitOfWork)
    {
        _documentChangeLogService = documentChangeLogService;
        _numberService = numberService;
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.CallCenterAppealRepository;
        _authService = authService;
        _cultureHelper = cultureHelper;
        _eImzoService = eImzoService;
        _storageService = storageService;
        _pdfConverter = pdfConverter;
        _edocRegistrateService = edocRegistrateService;
        _contractorService = contractorService;
        _apiRequestLogRepository = apiRequestLogRepository;
        _externalDocFromEdocService = externalDocFromEdocService;
    }

    #region CRUD
    public PagedResult<CallCenterAppealListDto> GetList(CallCenterAppealSortFilterOptions dto)
    {
        var result = _repository.ReadAsNoTracked<CallCenterAppealListDto>()
                                .SortFilter(dto)
                                .AsPagedResult(dto);

        return result;
    }
    public SelectList<long> AsSelectList(CallCenterAppealSortFilterOptions options)
    {
        return _repository.ReadAsNoTracked<CallCenterAppealListDto>()
            .SortFilter(options)
            .AsSelectList();
    }
    public CallCenterAppealDto Get()
    {
        if (_authService.Contractor != null)
        {
            var contractor = _unitOfWork.ContractorRepository.ById<ContractorDto>(_authService.Contractor.Id);
            var region = _unitOfWork.RegionRepository.ById<RegionListDto>(contractor.RegionId);
            var district = _unitOfWork.DistrictRepository.ById<DistrictListDto>(contractor.DistrictId);
            return new CallCenterAppealDto
            {
                DocOn = DateOnly.FromDateTime(DateTime.Now),
                DistrictId = contractor.DistrictId,
                RegionId = contractor.RegionId,
                District = district.FullName,
                Region = region.FullName,
                Contractor = contractor,
                ContractorInn = contractor.Inn,
                // DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_APPEAL_APPLICATION, 1).Item2,
                Person = new()
            };
        }
        else
            return new CallCenterAppealDto
            {
                DocOn = DateOnly.FromDateTime(DateTime.Now),
                //DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_APPEAL_APPLICATION, 1).Item2,
                //Person = new()
                //{
                //    Inn = "",
                //    PassportSeria = ""
                //},
            };
    }
    public CallCenterAppealDto Get(long id)
    {
        var dto = _repository.ById<CallCenterAppealDto>(id);
        CombineStatuses(_repository);
        if (IsValid)
        {
            if (_authService.Contractor == null)
            {
                dto.CanAccept = StatusIdConst.CanCallCenterAppealApplyStatus(dto.StatusId, StatusIdConst.ACCEPTED);
                dto.CanReject = StatusIdConst.CanCallCenterAppealApplyStatus(dto.StatusId, StatusIdConst.REJECTED);
                dto.CanSendToEdoc = (dto.StatusId == StatusIdConst.CREATED || dto.StatusId == StatusIdConst.SENT || dto.StatusId == StatusIdConst.MODIFIED)
                    && _authService.HasPermission(ModuleCode.CallCenterAppealSendToEdoc);
            }
            else
            {
                dto.CanEdit = StatusIdConst.CanCallCenterAppealApplyStatus(dto.StatusId, StatusIdConst.MODIFIED);
                dto.CanSign = StatusIdConst.CanCallCenterAppealApplyStatus(dto.StatusId, StatusIdConst.SENT);
                dto.CanDelete = StatusIdConst.CanCallCenterAppealApplyStatus(dto.StatusId, StatusIdConst.DELETED);
            }
        }
        if (dto.StatusId == StatusIdConst.IN_EXECUTION
            || dto.StatusId == StatusIdConst.EXECUTED
            || dto.StatusId == StatusIdConst.HAS_EDOC_RESPONSE)
        {

            var edocInfoDb = _externalDocFromEdocService.GetByCallCenterAppealId(dto.Id);

            dto.EdocInfo = _edocRegistrateService.ForCallCenterGet(dto.Id).Result;

            if (edocInfoDb == null && dto.EdocInfo != null)
            {
                _externalDocFromEdocService.Create(new()
                {
                    TermExecution = dto.EdocInfo.TermExecution,
                    Assignment = dto.EdocInfo.Assignment,
                    OrganizationId = dto.OrganizationId,
                    ProcessId = dto.EdocInfo.ProcessId,
                    AppealApplicationId = null,
                    CallCenterAppealId = dto.Id,
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
                if (dto.DocNumber == null || dto.DocNumber != dto.EdocInfo.RegNumber)
                {
                    try
                    {
                        var ent = UnitOfWork.Context.Set<CallCenterAppeal>()
                            .FirstOrDefault(x => x.Id == dto.Id);
                        ent.DocNumber = dto.EdocInfo.RegNumber;
                        UnitOfWork.Save();
                    }
                    catch (DbUpdateException e)
                    {
                        AddError(e.Message);
                    }
                }


                if (dto.EdocInfo.ProcessId != 27)//   Revork (Qoralamada turgan ProcessId = 27 bo'ladi)
                {
                    dto.CanEdit = false;
                }
            }

        }
        return dto;
    }
    public CallCenterAppealDto Get(string docNumber)
    {
        var dto = _repository.ReadAsNoTracked<CallCenterAppealDto>().FirstOrDefault(x => x.DocNumber == docNumber);
        CombineStatuses(_repository);
        if (IsValid)
        {
            if (_authService.Contractor == null)
            {
                dto.CanAccept = StatusIdConst.CanCallCenterAppealApplyStatus(dto.StatusId, StatusIdConst.ACCEPTED);
                dto.CanReject = StatusIdConst.CanCallCenterAppealApplyStatus(dto.StatusId, StatusIdConst.REJECTED);
                dto.CanSendToEdoc = (dto.StatusId == StatusIdConst.CREATED || dto.StatusId == StatusIdConst.SENT || dto.StatusId == StatusIdConst.MODIFIED || dto.StatusId == StatusIdConst.REJECTED)
                    && _authService.HasPermission(ModuleCode.CallCenterAppealSendToEdoc);
            }
            else
            {
                dto.CanEdit = StatusIdConst.CanCallCenterAppealApplyStatus(dto.StatusId, StatusIdConst.MODIFIED);
                dto.CanSign = StatusIdConst.CanCallCenterAppealApplyStatus(dto.StatusId, StatusIdConst.SENT);
                dto.CanDelete = StatusIdConst.CanCallCenterAppealApplyStatus(dto.StatusId, StatusIdConst.DELETED);
            }
        }

        return dto;
    }
    public override HaveId<long> Create(CreateCallCenterAppealDlDto dto)
    {
        using var transaction = UnitOfWork.BeginTransaction();

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
                    _unitOfWork.Save();
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
                    if (contractorDto.Result == null)
                        AddError("Soliqdan Malumot kelmadi");

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
            var entity = _repository.Create(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            if (HasErrors)
            {
                transaction.Rollback();
                return null;
            }
            UnitOfWork.Save();
            _storageService.MoveToPersistent(DocumentStorageConst.DOC_CALL_CENTER_APPEAL, entity.Id.ToString(), dto.Files.Select(x => x.Id).ToArray());
            var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, message: "Hujjat yaratildi");
            if (IsValid)
            {
                transaction.Commit();
            }
            return HaveId.Create(entity.Id);
        }
        catch (Exception e)
        {
            AddError(e.Message + "Inner:" + e.InnerException);
            transaction.Rollback();
        }
        return null;

    }
    public async Task Update(UpdateCallCenterAppealDlDto dto)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = _repository.Update(dto, ent => Validation(dto, ent));
                UnitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "UpdateCallCenterAppeal");
                CombineStatuses(_repository);
                _storageService.ResolveMarkedFiles(DocumentStorageConst.DOC_CALL_CENTER_APPEAL, dto.Id.ToString());

                if (entity.StatusId == StatusIdConst.IN_EXECUTION)
                    await UpdateEdoc(dto);

                if (IsValid)
                    transaction.Commit();
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
                var entity = _repository.ById(id);
                var statusDto = new UpdateStatusCallCenterAppealDlDto()
                {
                    Id = id,
                    StatusId = StatusIdConst.DELETED
                };
                UpdateStatus(statusDto, ent => { });
                _repository.UpdateStatus(statusDto);
                UnitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, StatusIdConst.DELETED, "DeleteCallCenterAppeal");

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
    public async Task SendToEdoc(long id)
    {
        var log = new CreateApiRequestLogDlDto
        {
            DocumentId = id,
            TableId = TableIdConst.DOC_CALL_CENTER_APPLICATION,
            UserId = (int)_authService.UserId,
            UserInfo = _authService.User?.ToString() ?? "",
            RequestAt = DateTime.Now,
        };

        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = UnitOfWork.Context.Set<CallCenterAppeal>().FirstOrDefault(x => x.Id == id);
                var doc = Get(id);

                if (doc == null)
                {
                    AddError("Malumot topilmadi");
                    throw new Exception("Document not found");
                }

                if (!doc.CanSendToEdoc)
                {
                    AddError("Имкони йўқ / Нет доступа");
                    throw new Exception("Cannot send document to Edoc");
                }

                var employeId = _unitOfWork.Context.Set<Organization>()
                    .FirstOrDefault(a => a.Id == OrganizationIdConst.SSP)?.IncomingDocReceiverEmployeeId;

                if (employeId == null)
                {
                    AddError("Bu tashkilotga hodim biriktirilmagan");
                }

                var empManage = _unitOfWork.Context.Set<EmployeeManage>().FirstOrDefault(a => a.EmployeeId == employeId && !a.IsDeleted && a.EndOn == null);

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

                var downloadedPdf = await DownloadPdf(id, null);
                IEnumerable<FileResultResponse> uploadedFiles = null;
                if (downloadedPdf != null)
                    uploadedFiles = await UploadFile(downloadedPdf);

                CombineStatuses(_edocRegistrateService);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                }

                // Determine external document type
                int externalDocumentTypeId = (doc.Contractor != null) ? 1 : 2;

                // Prepare Edoc request
                var edocCallCenterAppeal = new EdocRegisterRequestDto
                {
                    IsGenerate = false,
                    DocDate = DateTime.Now,
                    SenderName = doc.ContractorFulName ?? entity.PersonFullName ?? doc.Person.FullName,
                    DocNumber = doc.DocNumber,
                    SpecialNotes = string.Empty,
                    ExternalDocumentTypeId = externalDocumentTypeId,
                    Document = new Document
                    {
                        DocumentTypeId = 12,
                        Summary = doc.Details,
                        TermExecution = null,
                        ActivityFieldId = null,
                        RegDate = DateTime.Now,
                        RegNumber = null,
                        ImportanceId = null,
                        ExternalDocumentId = null,
                        CallCenterAppealId = doc.Id,
                        EmployeeManageId = empManage?.Id
                    },
                    Files = uploadedFiles?.Select(fileId => new FileForEdoc { Id = fileId.FileId }).ToList() ?? new List<FileForEdoc>(),
                    Attachments = uploadedAttachments?.Select(attachmentId => new AttachmentForEdoc { Id = attachmentId.FileId }).ToList() ?? new List<AttachmentForEdoc>()
                };

                // Register document with Edoc
                await InExecution(new InExecutionStatusCallCenterAppealDto { Id = doc.Id });

                ExternalIncomingDocumentDto result = await _edocRegistrateService.RegistrateEdoc(edocCallCenterAppeal);

                CombineStatuses(_edocRegistrateService);

                // Check registration result
                if (result == null)
                {
                    AddError("Result from Edoc is null");
                    transaction.Rollback();
                    return;
                }
                _externalDocFromEdocService.Create(new()
                {
                    TermExecution = result.TermExecution,
                    CallCenterAppealId = entity.Id,
                    AppealApplicationId = null,
                    Assignment = result.Assignment,
                    OrganizationId = entity.OrganizationId,
                    ProcessId = result.ProcessId,
                    RegDate = result.RegDate,
                    RegNumber = result.RegNumber
                });


                entity.DocNumber = result.RegNumber;
                _unitOfWork.Save();

                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                }
                // Further processing if successful

                log.IsSuccess = true;
                log.ResponseAt = DateTime.Now;
                log.ResponseContent = JsonConvert.SerializeObject(edocCallCenterAppeal);
                if (IsValid)
                    // Commit transaction
                    transaction.Commit();
            }
            catch (Exception ex)
            {
                // Rollback transaction on exception
                transaction.Rollback();
                log.Exception = ex.Message;
                AddError($"Exception occurred: {ex.Message}");
                throw;
            }
            finally
            {
                _apiRequestLogRepository.Create(log);
                _unitOfWork.Save();
            }
        }
    }
    #endregion

    #region Status control
    //public async Task Sign(SignStatusCallCenterAppealDto dto)
    //{
    //    var canCommit = _unitOfWork.CurrentTransaction == null;
    //    var transaction = _unitOfWork.CurrentTransaction ??
    //        await _unitOfWork.Context.Database.BeginTransactionAsync();
    //    try
    //    {
    //        var entity = Repository.AllAsQueryable
    //               .Include(x => x.Signs)
    //               .FirstOrDefault(x => x.Id == dto.Id);

    //        if (entity == null)
    //        {
    //            AddError("Not found");
    //            transaction.Rollback();
    //            return;
    //        }
    //        var eImzoTimstampDto = new EImzoTimeStampDto
    //        {
    //            SignData = dto.SignedData,
    //            Inn = null,
    //            Pinfl = _authService.Contractor == null ? _authService.User.Pinfl : _authService.Contractor.Pinfl
    //        };
    //        var timeStamp = await _eImzoService.TimeStamp(eImzoTimstampDto);

    //        CombineStatuses(_eImzoService);
    //        if (HasErrors)
    //        {
    //            transaction.Rollback();
    //            return;
    //        };


    //        var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
    //        {
    //            SignData = timeStamp.Pkcs7b64,
    //            Inn = null,
    //            Pinfl = _authService.Contractor == null ? _authService.User.Pinfl : _authService.Contractor.Pinfl
    //        });

    //        var signer = new CallCenterAppealSign()
    //        {
    //            SignFile = SaveFile(dto.Id, timeStamp.Pkcs7b64, "sign.txt"),
    //            DataFile = SaveFile(dto.Id, JsonConvert.SerializeObject(dto), "data.txt"),
    //            SignedAt = DateTime.Now,
    //            SignedUserInfo = _authService.User != null
    //            ? _authService.User.ToTextForDocumentLog()
    //            : _authService.Contractor.FullName + " - " + _authService.Contractor.Inn,
    //            StatusId = dto.StatusId,
    //        };
    //        entity.Signs.Add(signer);

    //        var ent = _repository.UpdateStatus(dto, ent =>
    //        {
    //            if (!StatusIdConst.CanCallCenterAppealApplyStatus(ent.StatusId, StatusIdConst.SENT))
    //                _repository.AddError("Нет доступа");
    //        });
    //        CombineStatuses(_repository);
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
    public async Task Accept(AcceptStatusCallCenterAppealDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? await _unitOfWork.Context.Database.BeginTransactionAsync();
        try
        {
            _unitOfWork.Context.Set<CallCenterAppeal>().Lock(dto.Id);
            UpdateStatus(dto, ent => { });

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
    public async Task Reject(RejectStatusCallCenterAppealDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? await _unitOfWork.Context.Database.BeginTransactionAsync();
        try
        {
            _unitOfWork.Context.Set<CallCenterAppeal>().Lock(dto.Id);
            UpdateStatus(dto, ent => { });

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
    /// <param name="dto"> <see cref="UpdateStatusCallCenterAppealDlDto.Id"/> ga qiymat brish yetarli.
    /// "<see cref="InExecutionStatusCallCenterAppealDto.StatusId"/>" - ga o'zi qiymat set qiladi </param>
    /// <returns></returns>
    public async Task InExecution(InExecutionStatusCallCenterAppealDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? await _unitOfWork.Context.Database.BeginTransactionAsync();
        try
        {
            _unitOfWork.Context.Set<CallCenterAppeal>().Lock(dto.Id);
            UpdateStatus(dto, ent => { });

            CombineStatuses(_repository);
            if (HasErrors)
                return;
            await UnitOfWork.Context.SaveChangesAsync();
            var res = CreateDocumentChangeLog(dto.Id, StatusIdConst.IN_EXECUTION, message: "Hujjat jo'natilgan");

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
    /// Edoc dan javob hati kelganda hujjat statusi shu statusga o'tadi.
    /// <list type="bullet">1. <see cref="InExecution(InExecutionStatusCallCenterAppealDto)"/> chaqirganda edoc ga ketadi.</list>
    /// <list type="bullet">2. <see cref="Executed(ExecutedStatusCallCenterAppealDto)"/> chaqirganda edoc dan javob hati kelgan bo'ladi.</list>
    /// <see cref="InExecution(InExecutionStatusCallCenterAppealDto)"/> bilan bir hil ishlashi.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task Executed(ExecutedStatusCallCenterAppealDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? await _unitOfWork.Context.Database.BeginTransactionAsync();
        try
        {
            //_unitOfWork.Context.Set<CallCenterAppeal>().Lock(dto.Id);
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
    public async Task HasEdocResponce(HasEdocResponceStatusCallCenterAppealDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? await _unitOfWork.Context.Database.BeginTransactionAsync();
        try
        {
            //_unitOfWork.Context.Set<AppealApplication>().Lock(dto.Id);
            UpdateStatus(dto, ent => { });

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
    private HaveId<long> UpdateStatus(UpdateStatusCallCenterAppealDlDto dto, Action<CallCenterAppeal> validation)
    {
        var canCommit = UnitOfWork.CurrentTransaction == null;
        using var transaction = canCommit ? UnitOfWork.BeginTransaction() : null;
        validation += Validation(dto);
        try
        {
            var entity = _repository.UpdateStatus(dto, validation);
            CombineStatuses(_repository);
            if (HasErrors)
                return null;
            UnitOfWork.Save();
            var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, message: "CallCenterAppeal");
            if (IsValid && canCommit)
                transaction.Commit();
            return res;
        }
        finally
        {
            transaction?.Dispose();
        }

    }
    #endregion

    #region Log, Validation, Pdf
    private HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
    {
        var moveDto = Repository.ById<CallCenterAppealDto>(id, applyFilter: false);
        _documentChangeLogService.Create(
            dto: moveDto,
            tableId: TableIdConst.DOC_CALL_CENTER_APPLICATION,
            organizationId: null,
            statusId: statusId,
            message: message);
        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }
    private Action<CallCenterAppeal> Validation(UpdateStatusCallCenterAppealDlDto dto)
    {
        return ent =>
        {
            if (!StatusIdConst.CanCallCenterAppealApplyStatus(ent.StatusId, dto.StatusId))
                AddError("Имкони йўқ / Нет доступа");
        };
    }
    private void Validation<TDto>(CallCenterAppealDlDto<TDto> dto, CallCenterAppeal entity)
       where TDto : CallCenterAppealDlDto<TDto>
    {
        var query = Repository.AllAsQueryable;

        if (entity != null)
        {
            query = query.Where(a => a.Id != entity.Id);

            if (!StatusIdConst.CanCallCenterAppealApplyStatus(entity.StatusId, StatusIdConst.MODIFIED))
                AddError("Таҳрирлаш мумкин эмас / Невозможно редактировать");
        }

        //if (query.ByDocNumber(dto.DocNumber).Any())
        //_repository.AddError($"Документ с этим кодом {dto.DocNumber} уже существует", nameof(dto.DocNumber));

    }
    private Guid SaveFile(long docId, string data, string fileName)
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
        var dto = Repository.ById<CallCenterAppealDto>(id);
        MemoryStream wordFile = null;

        wordFile = _storageService.GetStaticFile(
           StaticFileConst.WordTemplate.GetFileName(
        lang,
        StaticFileConst.WordTemplate.APPEAL_CALLCENTR));

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
    public IEnumerable<CallCenterAppealFileDto> UploadFiles(params StorageFile[] files)
    {
        var result = _storageService.SaveTemp(DocumentStorageConst.DOC_CALL_CENTER_APPEAL, files)
            .Select(a => new CallCenterAppealFileDto
            {
                Id = a.FileId,
                FileName = a.FileName
            });
        CombineStatuses(_storageService);
        return IsValid ? result : null;
    }
    public StorageFile DownloadFile(Guid fileId)
    {
        var entity = _unitOfWork.Context.Set<CallCenterAppealFile>().FirstOrDefault(a => a.Id == fileId);
        return Download(fileId, entity, DocumentStorageConst.DOC_CALL_CENTER_APPEAL);
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
            .Set<CallCenterAppealFile>()
            .FirstOrDefault(a => a.Id == fileId);

        Delete(fileId, entity, DocumentStorageConst.DOC_CALL_CENTER_APPEAL);
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
    public Stream PrinCallCenterAppealExcel(CallCenterAppealSortFilterOptions dto)
    {
        //var data = GetList(dto);
        var data = Repository.ReadAsNoTracked<CallCenterAppealListDto>()
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
                ws.Cells[currentRow, column++].Value = item.CreatedAt.ToString(Constants.DATE_TIME_FORMAT);
                ws.Cells[currentRow, column++].Value = item.PersonFullName ?? item.PersonName ?? item.ContractorDirectorName;
                ws.Cells[currentRow, column++].Value = item.ContractorInn ?? "";
                ws.Cells[currentRow, column++].Value = item.Contractor ?? "";
                ws.Cells[currentRow, column++].Value = item.Region;
                ws.Cells[currentRow, column++].Value = item.District;
                ws.Cells[currentRow, column++].Value = item.OkedCode + " - " + item.Oked;
                ws.Cells[currentRow, column++].Value = "";
                ws.Cells[currentRow, column++].Value = (item.Isimporter == true) ? "Importyor" : (item.Isexporter == true) ? "Eksportyor" : null;
                ws.Cells[currentRow, column++].Value = item.AppealDescription ?? "";
                ws.Cells[currentRow, column++].Value = item.AppealTypeArrive ?? "";
                ws.Cells[currentRow, column++].Value = item.AppealType ?? "";
                ws.Cells[currentRow, column++].Value = item.AppealFormatType ?? "";
                ws.Cells[currentRow, column++].Value = item.Details ?? "";
                ws.Cells[currentRow, column++].Value = item.PhoneNumber ?? "";
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
    private async Task<IEnumerable<FileResultResponse>> UploadFile(byte[] file)
    {
        try
        {
            var res = await _edocRegistrateService.UploadFile(file);
            return (IEnumerable<FileResultResponse>)res;
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
            return null;
        }
    }
    private async Task<IEnumerable<FileResultResponse>> UploadAttachment(StorageFile file)
    {
        try
        {
            var res = await _edocRegistrateService.UploadAttachment(file);
            return (IEnumerable<FileResultResponse>)res;
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
            return null;
        }
    }
    private async Task UpdateEdoc(UpdateCallCenterAppealDlDto dto)
    {
        var log = new CreateApiRequestLogDlDto
        {
            DocumentId = dto.Id,
            TableId = TableIdConst.DOC_CALL_CENTER_APPLICATION,
            UserId = (int)_authService.UserId,
            UserInfo = _authService.User?.ToString() ?? "",
            RequestAt = DateTime.Now,
        };

        try
        {
            var doc = Get(dto.Id);
            int externalDocumentTypeId = (doc.Contractor != null) ? 1 : 2;
            var employeId = _unitOfWork.Context.Set<Organization>()
                .FirstOrDefault(a => a.Id == OrganizationIdConst.SSP)?.IncomingDocReceiverEmployeeId;

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

            var edocCallCenterAppeal = new EdocRegisterRequestDto
            {
                SenderName = doc.ContractorFulName ?? doc.PersonFullName ?? doc.Person.FullName,
                DocNumber = doc.DocNumber,
                SpecialNotes = string.Empty,
                ExternalDocumentTypeId = externalDocumentTypeId,
                Document = new Document
                {
                    DocumentTypeId = 12,
                    Summary = doc.Details,
                    TermExecution = null,
                    ActivityFieldId = null,
                    RegDate = DateTime.Now,
                    RegNumber = doc.DocNumber,
                    ImportanceId = null,
                    ExternalDocumentId = null,
                    CallCenterAppealId = doc.Id,
                    EmployeeManageId = empManage?.Id
                },
                Files = uploadedFiles?.Select(fileId => new FileForEdoc { Id = fileId.FileId }).ToList() ?? new List<FileForEdoc>(),
                Attachments = uploadedAttachments?.Select(attachmentId => new AttachmentForEdoc { Id = attachmentId.FileId }).ToList() ?? new List<AttachmentForEdoc>()
            };
            await _edocRegistrateService.UpdateEdoc(edocCallCenterAppeal);
            CombineStatuses(_edocRegistrateService);
        }
        catch (Exception ex)
        {
            // Rollback transaction on exception
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

    #endregion
}
