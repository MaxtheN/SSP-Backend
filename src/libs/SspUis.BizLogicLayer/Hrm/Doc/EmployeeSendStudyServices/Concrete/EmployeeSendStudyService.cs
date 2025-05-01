using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.Srv;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using SspUis.Integration.DocxToPdf;
using SspUis.ServiceLayer.NumberServices;
using WbImzo.Models;
using WbImzo.Proxy.Sdk;
using WEBASE;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.OfficeTools.Factory;
using WEBASE.OfficeTools.Handlers;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeSendStudyService
    : BaseEntityService<long, EmployeeSendStudy, EmployeeSendStudyListDto, EmployeeSendStudyDto, CreateEmployeeSendStudyDlDto, UpdateEmployeeSendStudyDlDto, IEmployeeSendStudyRepository, EmployeeSendStudySortFilterOptions>
    , IEmployeeSendStudyService
{
    IAuthService _authService;
    private WbImzoConfig _wbImzoConfig;
    private readonly LinkConfig _linkConfig;
    private readonly IDocumentChangeLogService _documentChangeLogService;
    private readonly IEmployeeSendStudyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INumberService _numberService;
    private readonly IEImzoService _eImzoService;
    private readonly IStorageService _storageService;
    private readonly IConvertService _pdfConverter;
    private readonly IWbImzoService _wbImzoService;

    public EmployeeSendStudyService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        INumberService numberService,
        IEImzoService eImzoService,
        IStorageService storageService,
        IDocumentChangeLogService documentChangeLogService,
        WbImzoConfig wbImzoConfig,
        List<LinkConfig> linkConfigs,
        IWbImzoService wbImzoService,
        IConvertService pdfConverter)
        : base(unitOfWork)
    {
        _repository = unitOfWork.EmployeeSendStudyRepository;
        this._authService = authService;
        this._unitOfWork = unitOfWork;
        this._documentChangeLogService = documentChangeLogService;
        this._numberService = numberService;
        this._pdfConverter = pdfConverter;
        _wbImzoService = wbImzoService;
        _wbImzoConfig = wbImzoConfig;
        _linkConfig = linkConfigs.FirstOrDefault(config => config.Name == "EmployeeSendStudy");
        _storageService = storageService;
        _eImzoService = eImzoService;
    }
    public PagedResult<EmployeeSendStudyListDto> GetList(EmployeeSendStudySortFilterOptions options)
    {
        return Repository.ReadAsNoTracked<EmployeeSendStudyListDto>()
            .SortFilter(options)
            .AsPagedResult(options);
    }

    public PagedResult<EmployeeSendStudyListDto> GetListForSigner(EmployeeSendStudySortFilterOptions dto)
    {
        var result = _repository.ReadAsNoTracked<EmployeeSendStudyListDto>(q =>
                    q.Signer.Any(a => a.EmployeeManageId == _authService.User.EmployeeManageId)
                && (new int[] { StatusIdConst.ACCEPTED, StatusIdConst.SIGNED, StatusIdConst.SIGNING }).Contains(q.StatusId))
                                .SortFilter(dto)
                                .AsPagedResult(dto);
        return result;
    }
    public SelectList<long> AsSelectList(int? employeeId = null)
    {
        return _repository.AllAsQueryable
            .Include(a => a.Tables)
            .Where(a => employeeId.HasValue ? a.Tables.Any(a => a.EmployeeId == employeeId.Value) : true && a.StatusId == StatusIdConst.ACCEPTED)
                        .AsSelectList();
    }

    public EmployeeSendStudyDto Get()
    {
        return new EmployeeSendStudyDto
        {
            DocOn = DateOnly.FromDateTime(DateTime.Now),
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_EMPLOYEE_SEND_STUDY, 1).Item2
        };
    }

    public EmployeeSendStudyDto Get(long id)
    {
        var dto = _repository.ById<EmployeeSendStudyDto>(id);
        dto.Tables = dto.Tables.DistinctBy(x => x.Id).ToList();
        CombineStatuses(_repository);
        if (IsValid)
        {
            var nextSigner = _unitOfWork.Context.Set<EmployeeSendStudySigner>()
                   .Where(a => a.OwnerId == dto.Id && !a.SignedAt.HasValue)
                   .OrderBy(a => a.SignOrder).FirstOrDefault();

            dto.CanSign = StatusIdConst.CanApplyEmployeeLeaveOrder(dto.StatusId, StatusIdConst.SIGNING) && _authService.HasPermission(ModuleCode.EmployeeSendStudySign) && (nextSigner == null || (nextSigner != null && nextSigner.EmployeeManageId == _authService.User.EmployeeManageId));
            dto.CanModify = StatusIdConst.CanApplyEmployeeLeaveOrder(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.EmployeeSendStudyEdit);
            dto.CanAccept = StatusIdConst.CanApplyEmployeeLeaveOrder(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.EmployeeSendStudyAccept);
            dto.CanCancel = StatusIdConst.CanApplyEmployeeLeaveOrder(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(ModuleCode.EmployeeSendStudyCancel);
            dto.CanDelete = StatusIdConst.CanApplyEmployeeLeaveOrder(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.EmployeeSendStudyDelete);
        }
        return dto;
    }
    public EmployeeSendStudyTableDto GetByEmployeeId(int employeeId, DateOnly? startOn, DateOnly? endOn)
    {
        if (startOn == null || endOn == null) return null;
        var dto = _repository.ReadAsNoTracked<EmployeeSendStudyDto>().Where(a => a.Tables.Any(t => t.EmployeeId == employeeId) && a.StatusId == 2).FirstOrDefault();
        var tableItem = dto?.Tables.FirstOrDefault(t => t.EmployeeId == employeeId && t.StartOn >= startOn && t.EndOn <= endOn);
        CombineStatuses(_repository);
        return tableItem;
    }
    public async ValueTask<HaveId<long>> Create(CreateEmployeeSendStudyDlDto dto)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                EmployeeSendStudy entity = Repository.Create(dto, ent => Validation(dto, ent));
                UnitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "CreateEmployeeSendStudy");
                CombineStatuses(Repository);
                if (IsValid)
                {
                    transaction.Commit();

                    //var model = _unitOfWork.Context.Set<EmployeeSendStudy>()
                    //                     .Include(a => a.Signer)
                    //                         .ThenInclude(a => a.EmployeeManage)
                    //                             .ThenInclude(em => em.Employee)
                    //                                 .ThenInclude(e => e.Organization)
                    //                     .Include(a => a.Signer)
                    //                         .ThenInclude(a => a.EmployeeManage)
                    //                             .ThenInclude(em => em.Employee)
                    //                                 .ThenInclude(e => e.Person)
                    //                     .FirstOrDefault(a => a.Id == entity.Id);

                    //await PostToIMZOAndSentUrl(model);
                }
                    return HaveId.Create(entity.Id);
            }
            catch (DbUpdateException e)
            {
                AddError(e.Message);
                transaction.Rollback();
            }
            return null;
        }
    }
    public async ValueTask<string> PostToIMZOAndSentUrl(EmployeeSendStudy contract)
    {
        var canDispose = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        WbImzoCreateSignRequestDto signRequestCreateDto = CreatDtoForRequestSign(contract);


        foreach (var signer in contract.Signer)
        {

            if (contract.Signer.Count == signer.SignOrder)
            {

                signRequestCreateDto.SignRequestUsers.Add(
                                                        new WbImzoCreateSignRequestUserDto
                                                        {
                                                            UserKey = !string.IsNullOrEmpty(signer.EmployeeManage.Employee.Organization.Inn) ? signer.EmployeeManage.Employee.Organization.Inn : signer.EmployeeManage.Employee.Person.Pinfl,
                                                            UserInfo = signer.EmployeeManage.Employee.Person.FullName,
                                                            UserId = (int)contract.CreatedUserId,
                                                            DocStatusId = StatusIdConst.SIGNED,
                                                            SignPriority = signer.SignOrder,
                                                            IpAddress = _authService.UserIp,
                                                            UserAgent = _authService.UserAgent,
                                                            UserPhoneNumber = signer.EmployeeManage.Employee.Organization.PhoneNumber,
                                                        }
                                                    );
            }

            else
            {
                signRequestCreateDto.SignRequestUsers.Add(
                                                        new WbImzoCreateSignRequestUserDto
                                                        {
                                                            UserKey = !string.IsNullOrEmpty(signer.EmployeeManage.Employee.Person.Inn) ? signer.EmployeeManage.Employee.Person.Inn : signer.EmployeeManage.Employee.Person.Pinfl,
                                                            UserInfo = signer.EmployeeManage.Employee.Person.FullName,
                                                            UserId = (int)contract.CreatedUserId,
                                                            DocStatusId = StatusIdConst.SIGNING,
                                                            SignPriority = signer.SignOrder,
                                                            IpAddress = _authService.UserIp,
                                                            UserAgent = _authService.UserAgent,
                                                            UserPhoneNumber = signer.EmployeeManage.Employee.PhoneNumber,
                                                        }
                                                    );
            }
        }

        if (contract.WebImzoSecretKey != null)
            return await SendUrl(contract.Id);

        var wbImzoResult = await _wbImzoService.UpsertStringFilePrintableLinkRequestAsync(signRequestCreateDto);
        if (!wbImzoResult.IsSuccess && wbImzoResult.Response is null)
            CombineStatuses(wbImzoResult.GetStatusGeneric());

        if (HasErrors || wbImzoResult.Response is null)
        {
            AddError("Bir martalik tolov berish buyuruqni imzolash uchun yuborilayotgan jarayonda xatolik yuz berdi");
            return null;
        }

        try
        {
            contract.WebImzoRequestId = wbImzoResult.Response.RequestId;
            contract.WebImzoSecretKey = wbImzoResult.Response.SecretKey;

            _unitOfWork.Save();
            CombineStatuses(this);
            if (canDispose)
                transaction.Commit();
        }
        catch (Exception ex)
        {
            AddError("Document did not update" + ex.Message);
        }

        return await SendUrl(contract.Id);
    }

    public async ValueTask<string?> SendUrl(long contractId)
    {

        var contract = _unitOfWork.Context.Set<EmployeeSendStudy>()
                                                          .Include(a => a.Signer)
                                                              .ThenInclude(a => a.EmployeeManage)
                                                                  .ThenInclude(em => em.Employee)
                                                                      .ThenInclude(e => e.Organization)
                                                          .Include(a => a.Signer)
                                                              .ThenInclude(a => a.EmployeeManage)
                                                                  .ThenInclude(em => em.Employee)
                                                                      .ThenInclude(e => e.Person)
                                                          .FirstOrDefault(a => a.Id == contractId);

        if (contract == null)
        {
            AddError("Ma'lumot topilmadi");
            return null;
        }

        if (contract.Signer.Count() > 0)
        {
            var nextSigner = _unitOfWork.Context.Set<EmployeeSendStudySigner>()
                                                                 .Where(a => a.OwnerId == contractId)
                                                                 .OrderBy(a => a.SignOrder);
            if (nextSigner == null)
                AddError("Imzolovchi topilmadi");
            else if (!nextSigner.Any(a => a.EmployeeManageId != _authService.User.EmployeeManageId))
                AddError("Sizda imzolash huquqi yo'q");
            if (HasErrors)
                return null;

            if (contract.WebImzoRequestId == null || contract.WebImzoSecretKey.IsNullOrEmpty())
            {
                await PostToIMZOAndSentUrl(contract);
            }

            var url = (_wbImzoConfig.Api.Contains("api/") ? _wbImzoConfig.Api.Replace("api/", null) : _wbImzoConfig.Api) + $"app?requestId={contract.WebImzoRequestId}&secretKey={contract.WebImzoSecretKey}";

            return url;
        }

        return null;
    }

    public WbImzoCreateSignRequestDto CreatDtoForRequestSign(EmployeeSendStudy contract)
    {
        string documentDataAsString = JsonConvert.SerializeObject(ConvertToDto(contract));
        var filePrintableLink = $"{_linkConfig.Api}{_linkConfig.DownloadRoute}?id2={contract.Id2}";

        return new WbImzoCreateSignRequestDto
        {
            DocumentId = contract.Id,
            ApiKey = _wbImzoConfig.ApiKey,
            Title = contract.DocNumber,
            IsForceCreate = false,
            TableId = TableIdConst.DOC_EMPLOYEE_SEND_STUDY,
            SignData = documentDataAsString,
            OrganizationInn = contract.Organization.Inn,
            OrganizationName = contract.Organization.FullName,
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
    public WebImzoDto ConvertToDto(EmployeeSendStudy dto)
    => new()
    {
        DocOn = dto.DocOn,
        StatusId = dto.StatusId,
        DocNumber = dto.DocNumber,
        Id = dto.Id,
        OrganizationId = dto.OrganizationId,
    };

    public override void Update(UpdateEmployeeSendStudyDlDto dto)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.Update(dto, ent => Validation(dto, ent));
                UnitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "UpdateEmployeeSendStudy");
                CombineStatuses(Repository);
                if (IsValid)
                    transaction.Commit();
            }
            catch (DbUpdateException e)
            {
                AddError(e.Message);
                transaction.Rollback();
            }
        }
    }

    public void Accept(UpdateStatusEmployeeSendStudyDto dTo, bool isLoged = false)
    {
        try
        {
            var canCommit = _unitOfWork.Context.Database.CurrentTransaction == null;
            var transaction = canCommit ? _unitOfWork.BeginTransaction() : _unitOfWork.CurrentTransaction;

            if (isLoged)
            {
                Repository.AllAsQueryable.Lock(dTo.Id);
            }

            var dto = new UpdateStatusEmployeeSendStudyDlDto { Id = dTo.Id, StatusId = StatusIdConst.ACCEPTED };

            UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, dto.StatusId))
                {
                    _repository.AddError("Нет доступа");
                }
                bool isDeleted = _unitOfWork.Context.Set<EmployeeManage>()
                    .Where(a => ent.Tables.Select(a => a.EmployeeId).Contains(a.EmployeeId))
                    .Any(a => a.IsDeleted);

                if (isDeleted)
                {
                    _repository.AddError("Сотрудник был удален");
                }
            });
            if (!IsValid)
            {
                transaction.Rollback();
                return;
            }

            if (canCommit)
            {
                transaction.Commit();
            }
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
        }
    }
    public async Task Sign(SignStatusEmployeeSendStudyDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = _repository.ById(dto.Id);

                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                };


                var nextSigner = _unitOfWork.Context.Set<EmployeeSendStudySigner>()
                .Where(a => a.OwnerId == dto.Id && !a.SignedAt.HasValue)
                .OrderBy(a => a.SignOrder).FirstOrDefault();
                if (nextSigner == null)
                    AddError("Imzolovchi topilmadi");
                else if (nextSigner.EmployeeManageId != _authService.User.EmployeeManageId)
                    AddError("Sizda imzolash huquqi yo'q");
                if (HasErrors)
                    return;


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

                nextSigner.SignFile = SaveFile(dto.Id, timeStamp.Pkcs7b64, "sign.txt");
                nextSigner.DataFile = SaveFile(dto.Id, JsonConvert.SerializeObject(dto), "data.txt");
                nextSigner.SignedAt = DateTime.Now;

                _unitOfWork.Context.Entry(nextSigner).State = EntityState.Modified;
                _unitOfWork.Save();


                var ent = _repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, dto.StatusId))
                        _repository.AddError("Нет доступа");
                });
                CombineStatuses(_repository);
                if (HasErrors)
                    return;

                _unitOfWork.Save();
                if (IsValid)
                {
                    var res = CreateDocumentChangeLog(entity.Id, StatusIdConst.SIGNING);

                    if (nextSigner.IsDirector)
                    {
                        Accept(new UpdateStatusEmployeeSendStudyDto
                        {
                            Id = dto.Id,
                            StatusId = StatusIdConst.ACCEPTED
                        });
                    }
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
        var fileInfo = _storageService.Save($"{nameof(TableIdConst.DOC_EMPLOYEE_SEND_STUDY)}_SIGN_DATA", docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

        return fileInfo.FirstOrDefault().FileId;
    }
    public void Cancel(UpdateStatusEmployeeSendStudyDto dTo)
    {
        var dto = new UpdateStatusEmployeeSendStudyDlDto { Id = dTo.Id, StatusId = StatusIdConst.NOT_ACCEPTED };
        var res = CreateDocumentChangeLog(dto.Id, StatusIdConst.NOT_ACCEPTED);
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
        });
    }
    public async Task<byte[]> DownloadPdf(Guid id2)
    {
        var lang = "ru";

        var wordFile = _storageService.GetStaticFile(
           StaticFileConst.WordTemplate.GetFileName(
               lang,
               StaticFileConst.WordTemplate.EMPLOYEE_LEAVE_ORDER)
           );

        var dto = Repository.ReadAsNoTracked<EmployeeSendStudyDto>(applyFilter: false)
            .FirstOrDefault(x => x.Id2 == id2);

        if (dto == null)
        {
            AddError("Hujjat topilmadi.");
            return null;
        }

        for (int i = 0; i < dto.Tables.Count; i++)
        {
            dto.Tables[i].index = i + 1;
        }
        dto.Signer.OrderByDescending(x => x.SignOrder);

        for (int i = 0; i < dto.Signer.Count; i++)
        {

            if (dto.Signer[i].IsDirector)
                dto.Signer[i].Position = dto.Signer[i].Position + " ";
            else if (dto.Signer[i].IsHr)
                dto.Signer[i].Position = "Kiritildi: <br/>" + dto.Signer[i].Position + " ";
            else
                dto.Signer[i].Position = "Kelishildi: <br/>" + dto.Signer[i].Position + " ";

            if (!dto.Signer[i].SignedAt.HasValue)
            {
                dto.Signer[i].QrSign = null;
                dto.Signer[i].QrSignValue = " ";
            }

        }

        var plh = WordFactory.MakePlaceholders(dto);
        wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();
        var res = await _pdfConverter.DocxToPdfAsync(wordFile, new());
        CombineStatuses(_pdfConverter);
        return res;
    }
    public override void Delete(long id)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.ById(id);
                var statusDto = new UpdateStatusEmployeeSendStudyDlDto()
                {
                    Id = id,
                    StatusId = StatusIdConst.DELETED
                };
                var ent = _repository.UpdateStatus(statusDto, ent =>
                {
                    if (!StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, statusDto.StatusId))
                        _repository.AddError("Нет доступа");
                });
                UnitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, StatusIdConst.DELETED, "DeleteEmployeeSendStudy");

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

    private HaveId<long> UpdateStatus(UpdateStatusEmployeeSendStudyDlDto dto, Action<EmployeeSendStudy> validation)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = _repository.UpdateStatus(dto);
                CombineStatuses(_repository);
                if (HasErrors)
                    return null;
                _unitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "EmployeeSendStudy");
                if (IsValid)
                    transaction.Commit();
                return res;
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                transaction.Rollback();
            }
        }
        return null;
    }

    public HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
    {
        var moveDto = Repository.ById<EmployeeSendStudyDto>(id, applyFilter: false);
        _documentChangeLogService.Create(
            dto: moveDto,
            tableId: TableIdConst.DOC_EMPLOYEE_SEND_STUDY,
            organizationId: null,
            statusId: statusId,
            message: message);
        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }

    private void Validation<TDto>(EmployeeSendStudyDlDto<TDto> dto, EmployeeSendStudy entity)
       where TDto : EmployeeSendStudyDlDto<TDto>
    {
        var query = Repository.AllAsQueryable;

        if (entity != null)
        {
            query = query.Where(a => a.Id != entity.Id);

            if (!StatusIdConst.CanModifyStatus.Contains(entity.StatusId))
                AddError("Таҳрирлаш мумкин эмас / Невозможно редактировать");
        }

        if (query.ByDocNumber(dto.DocNumber).Any())
            _repository.AddError($"Документ с этим кодом {dto.DocNumber} уже существует", nameof(dto.DocNumber));

        if (dto.Signer.Count() > 0)
        {
            if (dto.Signer.Count(a => a.IsHr) == 0)
                AddError("Имзоловчи кадр киритилмаган");
            if (dto.Signer.Count(a => a.IsHr) > 1)
                AddError("Имзоловчи кадр лавозимидаги ходим 1 та болиши керак");
            if (dto.Signer.Count(a => a.IsDirector) == 0)
                AddError("Имзоловчи директор киритилмаган");
            if (dto.Signer.Count(a => a.IsDirector) > 1)
                AddError("Имзоловчи дтректор лавозимидаги ходим 1 та болиши керак");
        }

    }

    public byte[]? GetWordTemplate()
    {
        var lang = "uz-cyrl";

        var wordFile = _storageService.GetStaticFile(
            StaticFileConst.WordTemplate.GetFileName(lang, StaticFileConst.WordTemplate.EMPLOYEE_LEAVE_ORDER));

        CombineStatuses(_storageService);
        if (HasErrors)
            return null;

        return wordFile.ToArray();
    }

    public object UploadFiles(params StorageFile[] files)
    {
        if (files == null || files.Length <= 0)
        {
            AddError("Empty file");
            return null;
        }

        var result = _storageService.SaveTemp(DocumentStorageConst.DOC_EMPLOYEE_LEAVE_ORDER, files)
            .Select(a => new EmployeeSendTrainFileDto
            {
                Id = a.FileId,
                FileName = a.FileName,
                CreatedAt = DateTime.Now,
            });

        CombineStatuses(_storageService);
        return IsValid ? result : null;
    }
}
