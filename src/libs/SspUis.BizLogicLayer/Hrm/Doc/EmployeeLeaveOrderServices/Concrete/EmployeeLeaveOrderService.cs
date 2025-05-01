using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Http;
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

public class EmployeeLeaveOrderService
    : BaseEntityService<long, EmployeeLeaveOrder, EmployeeLeaveOrderListDto, EmployeeLeaveOrderDto, CreateEmployeeLeaveOrderDlDto, UpdateEmployeeLeaveOrderDlDto, IEmployeeLeaveOrderRepository, EmployeeLeaveOrderSortFilterOptions>
    , IEmployeeLeaveOrderService
{
    private readonly IDocumentChangeLogService _documentChangeLogService;
    private readonly IEmployeeLeaveOrderRepository _repository;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INumberService _numberService;
    private readonly IEImzoService _eImzoService;
    private readonly IStorageService _storageService;
    private readonly IConvertService _pdfConverter;
    private readonly LinkConfig _linkConfig;
    private readonly WbImzoConfig _wbImzoConfig;
    private readonly IWbImzoService _wbImzoService;

    public EmployeeLeaveOrderService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        INumberService numberService,
        IEImzoService eImzoService,
        IStorageService storageService,
        IDocumentChangeLogService documentChangeLogService,
        IConvertService pdfConverter,
        List<LinkConfig> linkConfigs,
        WbImzoConfig wbImzoConfig,
        IWbImzoService wbImzoService) : base(unitOfWork)
    {
        this._repository = unitOfWork.EmployeeLeaveOrderRepository;
        this._unitOfWork = unitOfWork;
        this._documentChangeLogService = documentChangeLogService;
        this._numberService = numberService;
        _authService = authService;
        _eImzoService = eImzoService;
        _storageService = storageService;
        this._pdfConverter = pdfConverter;
        _linkConfig = linkConfigs.FirstOrDefault(config => config.Name == "EmployeeLeaveOrder");
        _wbImzoConfig = wbImzoConfig;
        _wbImzoService = wbImzoService;
    }

    #region CRUD
    //public PagedResult<EmployeeLeaveOrderListDto> GetListAll(EmployeeLeaveOrderSortFilter dto)
    //{
    //    var result = _repository.CrudServices.ProjectFromEntityToDto<EmployeeLeaveOrder, EmployeeLeaveOrderListDto>(
    //                            query => query.Where(a => ((dto.IsSelectList.HasValue &&
    //                                                        dto.IsSelectList.Value == true &&
    //                                                        dto.EmployeeId.HasValue) ? (a.Tables.Any(t => t.EmployeeId == dto.EmployeeId) &&
    //                                                      a.StatusId == StatusIdConst.ACCEPTED) : true))
    //                                            .Where(a =>
    //                                                      (a.OrganizationId == _authService.Organization.Id &&
    //                                                      a.StatusId != StatusIdConst.DELETED))
    //        );

    //    return result.SortFilter(dto)
    //                            .AsPagedResult(dto);
    //}
    public PagedResult<EmployeeLeaveOrderListDto> GetList(EmployeeLeaveOrderSortFilterOptions options)
    {
        var result = _repository.ReadAsNoTracked<EmployeeLeaveOrderListDto>().Where(a =>
            new int[]
            {
                StatusIdConst.CREATED,
                StatusIdConst.CANCELED,
                StatusIdConst.ACCEPTED,
                StatusIdConst.MODIFIED,
                StatusIdConst.SIGNING,
                StatusIdConst.SIGNED,
            }
            .Contains(a.StatusId))
            .SortFilter(options)
            .AsPagedResult(options);

        return result;
    }
    //public PagedResult<UnpaidEmployeeLeaveOrderListDto> GetUnPaidList(UnpaidEmployeeLeaveOrderSortFilterPageOptions dto)
    //{
    //    if (!dto.StartDate.HasValue)
    //        dto.StartDate = DateTimeUtility.FirstDayOfYear(DateTime.Today);
    //    if (!dto.EndDate.HasValue)
    //        dto.EndDate = DateTimeUtility.LastDayOfYear(DateTime.Today);

    //    //var calcLeavePayDocumentTableIds = _repository.Context.Set<CalcLeavePay>()
    //    //        .Where(a => a.OrganizationId == _authService.Organization.Id &&
    //    //                    a.StatusId != StatusIdConst.DELETED &&
    //    //                    a.DocumentTableId.HasValue &&
    //    //                    a.DocumentSysTableId == TableIdConst.DOC_EMPLOYEE_LEAVE_ORDER)
    //    //        .Select(a => a.DocumentTableId);

    //    var result = _repository.CrudServices.ProjectFromEntityToDto<EmployeeLeaveOrderTable, UnpaidEmployeeLeaveOrderListDto>(a => a.Where(b => b.IsWithOutPay == false))
    //                                      .Where(a => a.DocumentStatusId == StatusIdConst.ACCEPTED && !calcLeavePayDocumentTableIds.Contains(a.Id))
    //                                      .SortFilter(dto)
    //                                      .AsPagedResult(dto);
    //    return result;
    //}

    public SelectList<long> AsSelectList(int? employeeId = null)
    {
        var res = _repository.AllAsQueryable
            .Include(a => a.Tables)
            .Where(a => employeeId.HasValue ? a.Tables.Any(a => a.EmployeeId == employeeId.Value) : true && a.StatusId == StatusIdConst.ACCEPTED)
                        .AsSelectList();
        return res;
    }
    public PagedResult<EmployeeLeaveOrderListDto> GetListForSigner(EmployeeLeaveOrderSortFilterOptions dto)
    {
        var result = _repository.ReadAsNoTracked<EmployeeLeaveOrderListDto>(q =>
                    q.Signer.Any(a => a.EmployeeManageId == _authService.User.EmployeeManageId)
                && (new int[] { StatusIdConst.ACCEPTED, StatusIdConst.SIGNED, StatusIdConst.SIGNING }).Contains(q.StatusId))
                                .SortFilter(dto)
                                .AsPagedResult(dto);
        return result;
    }
    public SelectList<long> GetTableAsSelectList(long ownerId, long? employeeId)
    {
        return _repository.Context.Set<EmployeeLeaveOrderTable>()
            .Include(a => a.Owner)
            .Include(a => a.Employee).ThenInclude(a => a.Person)
            .GetTableAsSelectList(ownerId, employeeId);
    }
    public EmployeeLeaveOrderDto Get()
    {
        return new EmployeeLeaveOrderDto
        {
            DocOn = DateOnly.FromDateTime(DateTime.Now),
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_EMPLOYEE_lEAVE_ORDER, 1).Item2
        };
    }

    public EmployeeLeaveOrderDto Get(long id)
    {
        var dto = _repository.ById<EmployeeLeaveOrderDto>(id);
        CombineStatuses(_repository);
        if (IsValid)
        {
            var nextSigner = _unitOfWork.Context.Set<EmployeeLeaveOrderSigner>()
                    .Where(a => a.OwnerId == dto.Id && !a.SignedAt.HasValue)
                    .OrderBy(a => a.SignOrder).FirstOrDefault();

            dto.CanSign = StatusIdConst.CanApplyEmployeeLeaveOrder(dto.StatusId, StatusIdConst.SIGNING) && _authService.HasPermission(ModuleCode.EmployeeLeaveOrderSign) && (nextSigner == null || (nextSigner != null && nextSigner.EmployeeManageId == _authService.User.EmployeeManageId));
            dto.CanModify = StatusIdConst.CanApplyEmployeeLeaveOrder(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.EmployeeLeaveOrderEdit);
            dto.CanAccept = StatusIdConst.CanApplyEmployeeLeaveOrder(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.EmployeeLeaveOrderAccept);
            dto.CanCancel = StatusIdConst.CanApplyEmployeeLeaveOrder(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(ModuleCode.EmployeeLeaveOrderCancel);
            dto.CanDelete = StatusIdConst.CanApplyEmployeeLeaveOrder(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.EmployeeLeaveOrderDelete);
        }
        return dto;
    }

    public EmployeeLeaveOrderTableDto GetByEmployeeId(int employeeId, DateOnly? startOn, DateOnly? endOn)
    {
        if (startOn == null || endOn == null) return null;

        var dto = _repository.ReadAsNoTracked<EmployeeLeaveOrderDto>().Where(a => a.Tables.Any(t => t.EmployeeId == employeeId) && a.StatusId == 2).FirstOrDefault();

        var tableItem = dto?.Tables.FirstOrDefault(t => t.EmployeeId == employeeId && t.StartOn >= startOn && t.EndOn <= endOn);
        CombineStatuses(_repository);
        return tableItem;
    }

    public override HaveId<long> Create(CreateEmployeeLeaveOrderDlDto dto)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.Create(dto, ent => Validation(dto, ent));
                CombineStatuses(Repository);
                if (IsValid)
                {
                    UnitOfWork.Save();
                    var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "CreateEmployeeLeaveOrder");
                    transaction.Commit();
                    return HaveId.Create(entity.Id);
                }
            }
            catch (DbUpdateException e)
            {
                AddError(e.Message);
                if (e.InnerException != null)
                    AddError(e.InnerException.Message);
                transaction.Rollback();
            }
            return null;
        }
    }

    public override void Update(UpdateEmployeeLeaveOrderDlDto dto)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.Update(dto, ent => Validation(dto, ent));
                CombineStatuses(Repository);
                if (IsValid)
                {
                    UnitOfWork.Save();
                    var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "UpdateEmployeeLeaveOrder");
                    transaction.Commit();
                }
            }
            catch (DbUpdateException e)
            {
                AddError(e.Message);
                if (e.InnerException != null)
                    AddError(e.InnerException.Message);
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
                entity.StatusId = StatusIdConst.DELETED;
                CombineStatuses(Repository);
                var dto = new UpdateStatusEmployeeLeaveOrderDlDto { Id = id, StatusId = StatusIdConst.DELETED };
                var ent = _repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, dto.StatusId))
                        _repository.AddError("Нет доступа");
                });
                if (IsValid)
                {
                    UnitOfWork.Save();
                    var res = CreateDocumentChangeLog(entity.Id, StatusIdConst.DELETED, "DeleteEmployeeLeaveOrder");
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
    #endregion

    #region Accept Cancel
    public void Accept(UpdateStatusEmployeeLeaveOrderDto dTo, bool isLoged = false)
    {
        try
        {
            var canCommit = _unitOfWork.Context.Database.CurrentTransaction == null;
            var transaction = canCommit ? _unitOfWork.BeginTransaction() : _unitOfWork.CurrentTransaction;

            if (isLoged)
            {
                Repository.AllAsQueryable.Lock(dTo.Id);
            }

            var dto = new UpdateStatusEmployeeLeaveOrderDlDto { Id = dTo.Id, StatusId = StatusIdConst.ACCEPTED };

            var entity = Repository.ById(dTo.Id);

            #region Set EmployeeManage to 0 until emplotee comeback
            foreach (var table in entity.Tables)
            {
                var employeeManage = _unitOfWork.Context.EmployeeManages.FirstOrDefault(m => m.Id == table.EmployeeManageId);
                employeeManage.EmploymentRate = 0;
                _unitOfWork.Save();
            }
            #endregion

            UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, dto.StatusId))
                {
                    _repository.AddError("Нет доступа");
                }

                bool isDeleted = _unitOfWork.Context.Set<EmployeeManage>()
                    .Where(a => ent.Tables.Select(a => a.EmployeeManageId).Contains(a.Id))
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

    public void Cancel(UpdateStatusEmployeeLeaveOrderDto dTo)
    {
        var dto = new UpdateStatusEmployeeLeaveOrderDlDto { Id = dTo.Id, StatusId = StatusIdConst.NOT_ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
            var res = CreateDocumentChangeLog(dto.Id, dto.StatusId);
            //_hrmPackageContext.DocControlPackage.EmployeeLeaveOrderCancelValidation(
            //    organizationId: _authService.CurrentOrganizationId,
            //    id: dto.Id,
            //    userId: (int)_authService.UserId
            //);
            CombineStatuses(_repository);
        });
    }

    public async Task Sign(SignStatusEmployeeLeaveOrdeDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = _repository.ById(dto.Id);

                //if (!_authService.HasPermission(ModuleCode.AppointEmployeeWithoutSigner) || entity.Signer.Count() == 0)
                //{
                //    AddError("Siz Imzolovchilarsiz hujjatni qabul qilish vakolati yo'q");
                //}
                if (!_authService.HasPermission(ModuleCode.AppointEmployeeWithoutSigner) && entity.Signer.Count() == 0)
                {
                    AddError("Siz Imzolovchilarsiz hujjatni qabul qilish vakolati yo'q");
                }
                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                };

                var nextSigner = _unitOfWork.Context.Set<EmployeeLeaveOrderSigner>()
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

                //var dto = new UpdateStatusAppointEmployeeDlDto { Id = dto.Id, StatusId = StatusIdConst.SIGNING };

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
                        Accept(new UpdateStatusEmployeeLeaveOrderDto
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

    private HaveId<long> UpdateStatus(UpdateStatusEmployeeLeaveOrderDlDto dto, Action<EmployeeLeaveOrder> validation)
    {
        var canCommit = _unitOfWork.Context.Database.CurrentTransaction == null;
        var transaction = canCommit ? _unitOfWork.BeginTransaction() : _unitOfWork.CurrentTransaction;

        try
        {
            var entity = _repository.UpdateStatus(dto, validation);
            CombineStatuses(_repository);
            if (HasErrors)
                return null;
            _unitOfWork.Save();
            var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "EmployeeLeaveOrder");
            if (HasErrors)
            {
                if (canCommit)
                    transaction.Rollback();
                return null;
            }
            if (IsValid)
            {
                if (canCommit)
                    transaction.Commit();
            }
            return res;
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
            if (canCommit && transaction != null)
                transaction.Rollback();
        }
        finally
        {
            if (canCommit && transaction != null)
                transaction.Dispose();
        }
        return null;
    }


    #endregion

    public async Task<byte[]> DownloadPdf(Guid id2, string? lang)
    {
        var language = lang ?? "uz-latn";
        var wordFile = _storageService.GetStaticFile(StaticFileConst.WordTemplate.GetFileName(language,
                                StaticFileConst.WordTemplate.EMPLOYEE_LEAVE_ORDER_IS_WITH_OUT_PAY));
        try
        {
            var dto = Repository.ReadAsNoTracked<EmployeeLeaveOrderDto>(applyFilter: false)
            .FirstOrDefault(x => x.Id2 == id2);

            //if (!dto.Tables.Where(a => a.index == 0).Select(a => a.IsWithOutPay).FirstOrDefault())
            //{
            //     wordFile = _storageService.GetStaticFile(StaticFileConst.WordTemplate.GetFileName(lang,
            //       StaticFileConst.WordTemplate.EMPLOYEE_LEAVE_ORDER_IS_WITH_PAY));
            //}

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
        catch (Exception ex)
        {
            AddError(ex.Message);
        }
        return null;
    }

    public HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
    {
        var moveDto = Repository.ById<EmployeeLeaveOrderDto>(id, applyFilter: false);
        _documentChangeLogService.Create(
            dto: moveDto,
            tableId: TableIdConst.DOC_EMPLOYEE_LEAVE_ORDER,
            organizationId: null,
            statusId: statusId,
            message: message);
        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }

    private void Validation<TDto>(EmployeeLeaveOrderDlDto<TDto> dto, EmployeeLeaveOrder entity)
          where TDto : EmployeeLeaveOrderDlDto<TDto>
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
    private Guid SaveFile(long docId, string data, string fileName)
    {
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(data);
        writer.Flush();
        ms.Position = 0;
        var fileInfo = _storageService.Save($"{nameof(TableIdConst.DOC_EMPLOYEE_LEAVE_ORDER)}_SIGN_DATA", docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

        return fileInfo.FirstOrDefault().FileId;
    }
    public int GetCalculatedDays(int employeeManageId,
                               DateTime startDate,
                               DateTime endDate)
    {
        //var workingScheduleWorkHours = _unitOfWork.Context.Set<EmployeeManage>()
        //    .Include(manage => manage.WorkSchedule).ThenInclude(schedule => schedule.WorkHours)
        //        .FirstOrDefault(manage => manage.Id == employeeManageId)
        //            .WorkSchedule
        //                .WorkHours;

        //return workingScheduleWorkHours.Where(schedule => schedule.OwnerId == 14 && schedule.DateOn.AsDateTime() >= startDate &&
        //                                                  schedule.DateOn.AsDateTime() <= endDate)
        //                                .Count(schedule => schedule.Days == 1);

        var workingScheduleWorkHours = _unitOfWork.Context.Set<WorkSchedule>().IsActive()
                .Where(a => a.Code == "000").Select(a => a.WorkHours).FirstOrDefault();

        return workingScheduleWorkHours.Where(schedule => schedule.DateOn.AsDateTime() >= startDate &&
                                                          schedule.DateOn.AsDateTime() <= endDate &&
                                                          schedule.Days == 1)
                                        .Count();
    }

    public byte[]? GetWordTemplate()
    {
        var lang = "uz-cyrl";

        var wordFile = _storageService.GetStaticFile(
            StaticFileConst.WordTemplate.GetFileName(lang, StaticFileConst.WordTemplate.EMPLOYEE_LEAVE_ORDER_IS_WITH_OUT_PAY));

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

        var result = _storageService.SaveTemp(DocumentStorageConst.DOC_EMPLOYEE_LEAVE_ORDER_IS_WITH_OUT_PAY, files)
            .Select(a => new EmployeeLeaveOrderFileDto
            {
                Id = a.FileId,
                FileName = a.FileName,
                CreatedAt = DateTime.Now,
            });

        CombineStatuses(_storageService);
        return IsValid ? result : null;
    }

    public async ValueTask<string> PostToIMZOAndSentUrl(EmployeeLeaveOrder contract)
    {
        var canDispose = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        WbImzoCreateSignRequestDto signRequestDto = CreatDtoForRequestSign(contract);

        foreach (var signer in contract.Signer)
        {
            if(contract.Signer.Count == signer.SignOrder)
            {
                signRequestDto.SignRequestUsers.Add(new WbImzoCreateSignRequestUserDto
                {
                    UserKey = signer.EmployeeManage.Employee.Organization.Inn ?? signer.EmployeeManage.Employee.Person.Pinfl,
                    UserInfo = signer.EmployeeManage.Employee.Person.FullName,
                    UserId = contract.CreatedUserId,
                    DocStatusId = StatusIdConst.SIGNED,
                    SignPriority = signer.SignOrder,
                    IpAddress = _authService.UserIp,
                    UserAgent = _authService.UserAgent,
                    UserPhoneNumber = signer.EmployeeManage.Employee.PhoneNumber,
                });
            }
            else
            {
                signRequestDto.SignRequestUsers.Add(new WbImzoCreateSignRequestUserDto
                {
                    UserKey = signer.EmployeeManage.Employee.Person.Inn ?? signer.EmployeeManage.Employee.Person.Pinfl,
                    UserInfo = signer.EmployeeManage.Employee.Person.FullName,
                    UserId = contract.CreatedUserId,
                    DocStatusId = StatusIdConst.SIGNING,
                    SignPriority = signer.SignOrder,
                    IpAddress = _authService.UserIp,
                    UserAgent = _authService.UserAgent,
                    UserPhoneNumber = signer.EmployeeManage.Employee.PhoneNumber,
                });
            }
        }

        if (contract.WebImzoSecretKey != null)
            return await SendUrl(contract.Id);

        var wbImzoResult = await _wbImzoService.UpsertStringFilePrintableLinkRequestAsync(signRequestDto);
        if (!wbImzoResult.IsSuccess && wbImzoResult.Response is null)
            CombineStatuses(wbImzoResult.GetStatusGeneric());

        if(HasErrors || wbImzoResult.Response is null)
        {
            AddError("Contract imzolash uchun yuborilayotgan jarayonda xatolik yuz berdi!");
            return null;
        }

        try
        {
            contract.WebImzoRequestId = wbImzoResult.Response.RequestId;
            contract.WebImzoSecretKey = wbImzoResult.Response.SecretKey;

            _unitOfWork.Save();
            CombineStatuses(this);
            if(canDispose)
                transaction.Commit();
        }
        catch (Exception ex)
        {
            AddError("Document did not update" + ex.Message);
        }

        return await SendUrl(contract.Id);
    }

    public async ValueTask<string> SendUrl(long contractId)
    {

        var employeeLeaveOrder = _unitOfWork.Context.Set<EmployeeLeaveOrder>()
                                                  .Include(a => a.Signer)
                                                      .ThenInclude(a => a.EmployeeManage)
                                                          .ThenInclude(em => em.Employee)
                                                              .ThenInclude(e => e.Organization)
                                                  .Include(a => a.Signer)
                                                      .ThenInclude(a => a.EmployeeManage)
                                                          .ThenInclude(em => em.Employee)
                                                              .ThenInclude(e => e.Person)
                                                  .FirstOrDefault(a => a.Id == contractId);



        if (!_authService.HasPermission(ModuleCode.EmployeeLeaveOrderSign) && employeeLeaveOrder.Signer.Count() == 0)
        {
            _repository.AddError("Sizda Imzolovchilarsiz hujjatni qabul qilish vakolati yo'q");
            CombineStatuses(_repository);

            if (HasErrors)
                return null;
        }
        if(employeeLeaveOrder.Signer.Count() > 0)
        {
            var nextSigner = _unitOfWork.Context.Set<EmployeeLeaveOrderSigner>()
                                                                                  .Where(x => x.OwnerId == contractId)
                                                                                  .OrderBy(x => x.SignOrder);

            if (nextSigner == null)
                AddError("Imzolovchi topilmadi!");
            else if (!nextSigner.Any(a => a.EmployeeManageId != _authService.User.EmployeeManageId))
                AddError("Sizda imzolash huquqi yo'q!");

            if(HasErrors)
                return null;

            if (employeeLeaveOrder.WebImzoRequestId == null || employeeLeaveOrder.WebImzoSecretKey.IsNullOrEmpty())
                await PostToIMZOAndSentUrl(employeeLeaveOrder);

            var url = (_wbImzoConfig.Api.Contains("api/") ? _wbImzoConfig.Api.Replace("api/", null) : _wbImzoConfig.Api) + $"app?requestId={employeeLeaveOrder.WebImzoRequestId}&secretKey={employeeLeaveOrder.WebImzoSecretKey}";
            return url;
        }
        return null;
    }

    private WbImzoCreateSignRequestDto CreatDtoForRequestSign(EmployeeLeaveOrder contract)
    {
        string documentDataString = JsonConvert.SerializeObject(new WebImzoDto
        {
            Id = contract.Id,
            DocOn = contract.DocOn,
            StatusId = contract.StatusId,
            DocNumber = contract.DocNumber,
            OrganizationId = contract.OrganizationId,
        });

        var filePrintTableLink = $"{_linkConfig.Api}{_linkConfig.DownloadRoute}?id2={contract.Id2}&lang=uz-cyrl&__lang=uz-cyrl&langId=2";

        return new WbImzoCreateSignRequestDto
        {
            ApiKey = _wbImzoConfig.ApiKey,
            DocumentId = contract.Id,
            Title = contract.DocNumber,
            IsForceCreate = false,
            TableId = TableIdConst.DOC_EMPLOYEE_LEAVE_ORDER,
            SignData = documentDataString,
            PrintableLink = filePrintTableLink,
            SignatureMethodIds = new List<int> { SignatureMethodIdConst.E_IMZO },
            TemplateId = 28,
            SignRequestActionTypes = new()
            {
                new WbImzoCreateSignRequestActionTypeDto
                {
                    ActionTypeId = StatusIdConst.SIGNED,
                    ActionTypeName = "SIGNED",
                    Translates = new()
                    {
                        new ActionTranslateTypeDto
                        {
                            LanguageCode = LanguageCodeConst.RU,
                            Name = "Я согласен"
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
                        }
                    }
                }
            }
        };
    }
}
