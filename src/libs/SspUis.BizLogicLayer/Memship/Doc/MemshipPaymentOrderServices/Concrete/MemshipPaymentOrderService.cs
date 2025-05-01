using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using SspUis.ServiceLayer.NumberServices;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Memship;

public class MemshipPaymentOrderService : BaseEntityService<long, MemshipPaymentOrder, MemshipPaymentOrderListDto, MemshipPaymentOrderDto, CreateMemshipPaymentOrderDlDto, UpdateMemshipPaymentOrderDlDto, IMemshipPaymentOrderRepository, MemshipPaymentOrderSortFilterOption>,
    IMemshipPaymentOrderService
{
    IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly INumberService _numberService;
    private readonly IStorageService _storageService;
    private readonly IMemshipCertificateService _memshipCertificateService;
    private readonly IDocumentChangeLogService _documentChangeLogService;
    private readonly ICultureHelper _cultureHelper;

    public MemshipPaymentOrderService(IUnitOfWork unitOfWork, 
        IAuthService authService, 
        ICultureHelper cultureHelper, 
        INumberService numberService,
        IStorageService storageService, 
        IMemshipCertificateService memshipCertificateService,
        IDocumentChangeLogService documentChangeLogService)
        : base(unitOfWork)
    {
        _authService = authService;
        _cultureHelper = cultureHelper;
        _numberService = numberService;
        _storageService = storageService;
        this._unitOfWork = unitOfWork;
        this._memshipCertificateService = memshipCertificateService;
        _documentChangeLogService = documentChangeLogService;
    }

    public override PagedResult<MemshipPaymentOrderListDto> GetList(MemshipPaymentOrderSortFilterOption options)
    {
        var result = Repository.ReadAsNoTracked<MemshipPaymentOrderListDto>()
            .SortFilter(options)
            .AsPagedResult(options);
        return result;

    }
    public SelectList<long> AsSelectList()
    {
        return Repository.AllAsQueryable
            .AsSelectList();
    }
    public override MemshipPaymentOrderDto Get()
    {
        return
            new()
            {
                DocOn = DateOnly.FromDateTime(DateTime.Now),
                DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_MEMSHIP_PAYMENT_ORDER, _authService.User.OrganizationId).Item2
            };
    }
    public override MemshipPaymentOrderDto Get(long id)
    {
        var dto = Repository.ById<MemshipPaymentOrderDto>(id);
        if (dto.MemshipContractId.HasValue)
        {
            var contract = UnitOfWork.Context.Set<MemshipContract>()
            .FirstOrDefault(c => c.Id == dto.MemshipContractId);

            var minimum = UnitOfWork.FixedMinimumValueRepository.AllAsQueryable
                .FirstOrDefault(v => v.MinimumValueTypeId == MinimumValueTypeIdConst.BRV);

            var old = UnitOfWork.Context.Set<MemshipPaymentOrder>()
                .Where(p => p.MemshipContractId == dto.MemshipContractId)
                .Sum(p => p.Amount);

            dto.TotalAmount = contract.BaseFixedMinimumValue * minimum.FixedValue;
            dto.RestAmount = contract.BaseFixedMinimumValue * minimum.FixedValue - old;
            dto.PayedAmount = old;
        }
        if (IsValid)
        {
            dto.CanAccept = StatusIdConst.CanApplySrvDocStatus(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.ServicePaymentOrderAccept);
            dto.CanCancel = StatusIdConst.CanApplySrvDocStatus(dto.StatusId, StatusIdConst.CANCELED)
                    && _authService.HasPermission(ModuleCode.ServicePaymentOrderCancel);
        }
        return dto;
    }
    public MemshipPaymentOrderDto GetByMemshipContractId(long memshipContractId)
    {
        var contract = UnitOfWork.Context.Set<MemshipContract>()
            .FirstOrDefault(c => c.Id == memshipContractId);
        if (contract == null)
        {
            AddError("Contract not found");
            return null;
        }
        var minimum = UnitOfWork.Context.Set<FixedMinimumValue>()
            .OrderByDescending(v => v.CreatedAt)
            .FirstOrDefault(v => v.MinimumValueTypeId == MinimumValueTypeIdConst.BRV);

        var old = UnitOfWork.Context.Set<MemshipPaymentOrder>()
            .Where(p => p.MemshipContractId == memshipContractId)
            .Sum(p => p.Amount);

        var dto = new MemshipPaymentOrderDto()
        {
            DocOn = DateOnly.FromDateTime(DateTime.Now),
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_MEMSHIP_PAYMENT_ORDER, _authService.User.OrganizationId).Item2,
            ContractorId = contract.ContractorId,
            MemshipContractId = contract.Id,
            MemshipContractDocOn = contract.DocOn,
            MemshipContractNumber = contract.DocNumber,
            TotalAmount = contract.BaseFixedMinimumValue * minimum.FixedValue,
            PayedAmount = old,
            RestAmount = contract.BaseFixedMinimumValue * minimum.FixedValue - old
        };

        return dto;

    }
    public HaveId<long> Create(CreateMemshipPaymentOrderDlDto dto)
    {
        var entity = Repository.Create(dto, ent => Validation(dto, ent));
        CombineStatuses(Repository);
        _unitOfWork.Save();
        _storageService.MoveToPersistent(DocumentStorageConst.DOC_MEMSHIP_PAYMENT_ORDER_FILES, $"{entity.Id}", dto.Files.Select(a => a.Id).ToArray());
        CombineStatuses(_storageService);

        if (IsValid)
        {
            UnitOfWork.Save();
            return HaveId.Create(entity.Id);
        }
        return null;
    }

    public HaveId<long> Accept(long id)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.ById(id);
                var statusDto = new UpdateStatusMemshipPaymentOrderDlDto()
                {
                    Id = id,
                    StatusId = StatusIdConst.ACCEPTED
                };
                Repository.UpdateStatus(statusDto, ent =>
                {
                    if (!StatusIdConst.CanApplySrvDocStatus(ent.StatusId, statusDto.StatusId))
                        AddError("Нет доступа");
                });
                UnitOfWork.Save();
                if (entity.MemshipContractId != null)
                {
                    var certificate = UnitOfWork.MemshipCertificateRepository.AllAsQueryable
                        .FirstOrDefault(x =>
                            x.MemshipContractId == entity.MemshipContractId
                            && x.StatusId == StatusIdConst.FORMED);
                    if (certificate != null)
                    {
                        _memshipCertificateService.ProlongExpireOnForPaid(
                            certificate.Id,
                            entity.Id,
                            $" {entity.DocNumber} - Raqamli to'lov hujjatiga asosan guvohnoma muddati avtomatik uzaytirildi. ");
                        CombineStatuses(_memshipCertificateService);
                    }
                }
                if (IsValid)
                {
                    transaction.Commit();
                }
                return HaveId.Create(id);
            }
            catch (DbUpdateException ex)
            {
                AddError(ex.Message);
                transaction.Rollback();
            }
            return null;
        }
    }

    public HaveId<long> Cancel(long id, string message)
    {
        var entity = UnitOfWork.Context.Set<MemshipPaymentOrder>()
            .Include(x => x.MemshipContract)
            .ThenInclude(x => x.Certificates)
            .FirstOrDefault(p => p.Id == id);

        if (entity.MemshipContractId.HasValue)
        {
            var certificate = entity?.MemshipContract?.Certificates?.FirstOrDefault(c => c.StatusId == StatusIdConst.FORMED);
            if (certificate != null)
            {
                AddError($"Uchbu to'lov hujjatiga asosan pullik a'zolik bo'yicha {certificate.DocNumber} raqamli guvohnoma mavjud. Bekor qilish imkoni yoq!");
                return null;
            }

            var completeService = UnitOfWork.Context.Set<CompletedService>()
                .Where(x => x.ServiceContractId == entity.ServiceContractId
                    && x.StatusId == StatusIdConst.ACCEPTED);

            if (completeService.Any())
            {
                AddError("Document has already been approved !");
                return null;
            }
        }
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var statusDto = new UpdateStatusMemshipPaymentOrderDlDto()
                {
                    Id = id,
                    StatusId = StatusIdConst.CANCELED
                };
                Repository.UpdateStatus(statusDto, ent =>
                {
                    if (!StatusIdConst.CanApplySrvDocStatus(ent.StatusId, statusDto.StatusId))
                        AddError("Нет доступа");
                });
                UnitOfWork.Save();

                if (IsValid)
                {
                    CreateDocumentChangeLog(id, message: message, _authService.UserIp);
                    transaction.Commit();
                }
                return HaveId.Create(id);
            }
            catch (DbUpdateException ex)
            {
                AddError(ex.Message);
                if(ex.InnerException != null)
                    AddError($"Inner Exception: {ex.InnerException.Message}");

                transaction.Rollback();
            }
            return null;
        }
    }
    public void Update(UpdateMemshipPaymentOrderDlDto dto)
    {
        Repository.Update(dto, ent => Validation(dto, ent));
        CombineStatuses(Repository);

        _storageService.ResolveMarkedFiles(DocumentStorageConst.DOC_MEMSHIP_PAYMENT_ORDER_FILES, $"{dto.Id}");

        if (IsValid)
            UnitOfWork.Save();
    }

    //public void Delete(long id)
    //{
    //    try
    //    {
    //        Repository.Delete(id);
    //        CombineStatuses(Repository);
    //        if (IsValid)
    //            UnitOfWork.Save();
    //    }
    //    catch (DbUpdateException)
    //    {
    //        AddError("Запись не может быть удален");
    //    }
    //}
    public override void Delete(long id)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.ById(id);
                var statusDto = new UpdateStatusMemshipPaymentOrderDlDto()
                {
                    Id = id,
                    StatusId = StatusIdConst.DELETED
                };
                Repository.UpdateStatus(statusDto, ent =>
                {
                    if (!StatusIdConst.CanApplySrvDocStatus(ent.StatusId, statusDto.StatusId))
                        AddError("Нет доступа");
                });
                UnitOfWork.Save();

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
    private void Validation<TDto>(MemshipPaymentOrderDlDto<TDto> dto, MemshipPaymentOrder entity)
        where TDto : MemshipPaymentOrderDlDto<TDto>
    {
        var query = Repository.AllAsQueryable;

        if (!StatusIdConst.CanApplySrvDocStatus(entity.StatusId, StatusIdConst.MODIFIED))
            AddError("Нет доступа");

        if (entity != null)
            query = query.Where(a => a.Id != entity.Id);

    }
    public IEnumerable<MemshipPaymentOrderFileDto> UploadFiles(params StorageFile[] files)
    {
        var result = _storageService.SaveTemp(DocumentStorageConst.DOC_MEMSHIP_PAYMENT_ORDER_FILES, files).Select(a => new MemshipPaymentOrderFileDto
        {
            Id = a.FileId,
            FileName = a.FileName
        });
        CombineStatuses(_storageService);
        return IsValid ? result : null;
    }
    public StorageFile DownloadFile(Guid fileId)
    {
        var entity = _unitOfWork.Context.Set<MemshipPaymentOrderFile>().FirstOrDefault(a => a.Id == fileId);
        return Download(fileId, entity, DocumentStorageConst.DOC_MEMSHIP_PAYMENT_ORDER_FILES);
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
            .Set<MemshipPaymentOrderFile>()
            .FirstOrDefault(a => a.Id == fileId);

        Delete(fileId, entity, DocumentStorageConst.DOC_MEMSHIP_PAYMENT_ORDER_FILES);
    }
    private void Delete(Guid fileId, FileEntity<long> entity, string storageDocument)
    {
        if (entity != null)
        {
            _storageService.DeleteTemp(storageDocument, fileId);
            CombineStatuses(_storageService);
        }
    }
    public Stream SaveAsExcelPrtnBojxonaContracts(MemshipPaymentOrderSortFilterOption options)
    {
        var data = Repository.ReadAsNoTracked<MemshipPaymentOrderListDto>()
            .SortFilter(options);

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate
            .GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.FOR_GET_LIST_MEMSHIP_PAYMENT_ORDER));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;

            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = item.Id;
                ws.Cells[currentRow, column++].Value = item.DocNumber;
                ws.Cells[currentRow, column++].Value = item.DocOn.ToString("dd.MM.yyyy");
                ws.Cells[currentRow, column++].Value = item.RegionalOrganization;
                ws.Cells[currentRow, column++].Value = item.MemshipContractNumber;
                ws.Cells[currentRow, column++].Value = item.MemshipContractDocOn;
                ws.Cells[currentRow, column++].Value = item.ContractorInn + "-"+ item.Contractor;
                ws.Cells[currentRow, column++].Value = item.Amount;
                ws.Cells[currentRow, column++].Value = item.BankName;
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
    public HaveId<long> CreateDocumentChangeLog(long id, string message = null, string userIp = null, string userAgent = null)
    {
        var entityDto = Repository.ById<MemshipPaymentOrderDto>(id, applyFilter: false);
        
        _documentChangeLogService.Create(
            dto: entityDto,
            tableId: TableIdConst.DOC_MEMSHIP_PAYMENT_ORDER,
            organizationId: _authService.Contractor == null ? _authService?.Organization?.Id ?? null : null,
            statusId: entityDto.StatusId,
            message: message,
            userIp: _authService?.UserIp ?? userIp,
            userAgent: _authService?.UserAgent);
        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }
}
