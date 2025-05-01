using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.DualEdu;
using SspUis.DataLayer.EfClasses.DualEdu.Doc;
using SspUis.DataLayer.Repositories;
using SspUis.ServiceLayer.NumberServices;
using WEBASE;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public class SubsidyRequestService : BaseEntityService<long,
    SubsidyRequest,
    SubsidyRequestListDto,
    SubsidyRequestDto,
    CreateSubsidyRequestDlDto,
    UpdateSubsidyRequestDlDto,
    ISubsidyRequestRepository,
    SubsidyRequestSortFilterOption>,
    ISubsidyRequestService
{
    private readonly IAuthService _authService;
    private readonly INumberService _numberService;
    private readonly IDocumentChangeLogService _documentChangeLogService;
    private readonly IStorageService _storageService;
    private readonly IPersonService _personService;
    private readonly IEImzoService _eImzoService;

    public SubsidyRequestService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        INumberService numberService,
        IDocumentChangeLogService documentChangeLogService,
        IStorageService storageService,
        IEImzoService eImzoService,
        IPersonService personService)
        : base(unitOfWork)
    {
        _authService = authService;
        _numberService = numberService;
        _documentChangeLogService = documentChangeLogService;
        _storageService = storageService;
        _personService = personService;
        _eImzoService = eImzoService;
    }

    #region CRUD
    public override PagedResult<SubsidyRequestListDto> GetList(SubsidyRequestSortFilterOption options)
    {
        var result = Repository.ReadAsNoTracked<SubsidyRequestListDto>()
            .SortFilter(options)
            .AsPagedResult(options);
        return result;
    }

    public int GetCount()
    {
        SubsidyRequestSortFilterOption options = 
                            new SubsidyRequestSortFilterOption();
		var result = Repository.ReadAsNoTracked<SubsidyRequestListDto>().SortFilter(options).Count();
        return result;

	}
    public SelectList<long> AsSelectList()
    {
        return Repository.AllAsQueryable
            .AsSelectList();
    }
    public override SubsidyRequestDto Get()
    {
        var contractor = UnitOfWork.ContractorRepository.AllAsQueryable
            .Include(x => x.SettlementAccounts)
            .ThenInclude(x => x.Bank)
            .Include(x => x.Region)
            .Include(x => x.District)
            .Include(x => x.Contacts)
            .FirstOrDefault(x => x.Id == _authService.Contractor.Id);
        var settlementAccounts = contractor.SettlementAccounts
                    .FirstOrDefault(x => x.IsMain);

        var orgByRegion = UnitOfWork.OrganizationRepository.AllAsQueryable
            .FirstOrDefault(x =>
           (x.OrganizationGroupId == OrganizationGroupIdConst.SSP ||
            x.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)
            && x.RegionId == contractor.RegionId);

        return new()
        {
            DocOn = DateOnly.FromDateTime(DateTime.Now),
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_APPLICATION,
                    orgByRegion.RegionId).Item2,
            Contractor = contractor.FullName,
            ContractorInn = contractor.Inn,
            ContractorPinfl = contractor.Pinfl,
            RegionId = contractor.RegionId,
            Region = contractor.Region.FullName,
            DistrictId = contractor.DistrictId,
            District = contractor.District.FullName,
            Phone = contractor.PhoneNumber,
            Email = contractor.Contacts
                    .FirstOrDefault(x => x.ContactTypeId == ContactTypeIdConst.EMAIL)
                    .Contact,
            Address = contractor.Address,
            ContractorSettlementAccountId = settlementAccounts?.Id ?? 0,
            ContractorSettlementAccount = settlementAccounts?.AccountCode,
            Bank = settlementAccounts?.Bank?.Code + ", " + settlementAccounts?.Bank?.BankName
        };
    }
    public override SubsidyRequestDto Get(long id)
    {
        var dto = base.Get(id);
        dto.Files = dto.Files.Where(x => x.SubsidyRequestTableId == null).ToList();
        if (dto == null)
            return null;
        if (_authService.Contractor != null)
        {
            dto.CanEdit = StatusIdConst.CanSubsidyRequestApplyStatus(dto.StatusId, StatusIdConst.MODIFIED);
            dto.CanDelete = StatusIdConst.CanSubsidyRequestApplyStatus(dto.StatusId, StatusIdConst.DELETED);
            dto.CanSend = StatusIdConst.CanSubsidyRequestApplyStatus(dto.StatusId, StatusIdConst.SENT_FOR_REVIEW);
            dto.CanRevoke = StatusIdConst.CanSubsidyRequestApplyStatus(dto.StatusId, StatusIdConst.REVOKED);
        }
        else
        {
            dto.CanAccept = StatusIdConst.CanSubsidyRequestApplyStatus(dto.StatusId, StatusIdConst.ACCEPTED);
            dto.CanCancel = StatusIdConst.CanSubsidyRequestApplyStatus(dto.StatusId, StatusIdConst.CANCELED);
            dto.CanReject = StatusIdConst.CanSubsidyRequestApplyStatus(dto.StatusId, StatusIdConst.REJECTED);
        }
        return dto;
    }
    public async Task<HaveId<long>> Create(CreateSubsidyRequestDlDto dto)
    {
        var canCommit = UnitOfWork.CurrentTransaction == null;
        var transaction = canCommit ? UnitOfWork.BeginTransaction() : UnitOfWork.CurrentTransaction;
        try
        {
            var persons = new HashSet<int>();
            foreach (var tableDto in dto.Tables)
            {
                tableDto.PersonId = await SetPersonId(tableDto.Number, tableDto.Seria, tableDto.DateOfBirth);

                if (persons.Contains(tableDto.PersonId))
                    AddError("Bitta odam bir martadan ortiq bitta hujjatda qo'shilmoqda.");
                else
                    persons.Add(tableDto.PersonId);

                if (tableDto.Files == null || tableDto.Files.Count == 0)
                    AddError("File biriktirilmagan hodim bor: " + tableDto.Seria + " " + tableDto.Number);
            }

            var entity = Repository.Create(dto, ent => Validation(dto, ent));
            CombineStatuses(Repository);
            if (HasErrors)
                throw new("");
            UnitOfWork.Save();
            _storageService.MoveToPersistent(
                       DocumentStorageConst.DOC_DUAL_SUBSIDY_REQUEST_FILES
                       , entity.Id.ToString()
                       , dto.Files.Select(x => x.Id).ToArray()
                   );
             
            _storageService.MoveToPersistent(
                       DocumentStorageConst.DOC_DUAL_SUBSIDY_REQUEST_FILES
                       , entity.Id.ToString()
                       , dto.Tables.SelectMany(x=>x.Files).Select(x => x.Id).ToArray()
                   );

            CreateDocumentChangeLog(entity.Id, "SubsidyRequestCreated");
            if (IsValid && canCommit)
                transaction.Commit();
            return HaveId.Create(entity.Id);
        }
        catch (Exception ex)
        {
            AddError(ex.Message + "InnerException: " + ex.InnerException);
            if (canCommit)
                transaction.Rollback();
        }
        return null;
    }
    public async Task Update(UpdateSubsidyRequestDlDto dto)
    {
        var canCommit = UnitOfWork.CurrentTransaction == null;
        var transaction = canCommit ? UnitOfWork.BeginTransaction() : UnitOfWork.CurrentTransaction;
        try
        {

            foreach (var tableDto in dto.Tables)
            {
                tableDto.PersonId = await SetPersonId(tableDto.Number, tableDto.Seria, tableDto.DateOfBirth);
            }

            var entity = Repository.Update(dto, ent => Validation(dto, ent));
            CombineStatuses(Repository);
            if (IsValid)
                UnitOfWork.Save();
            _storageService.ResolveMarkedFiles(DocumentStorageConst.DOC_DUAL_SUBSIDY_REQUEST_FILES, dto.Id.ToString());
            CreateDocumentChangeLog(entity.Id, "SubsidyRequestUpdated");
            if (IsValid && canCommit)
                transaction.Commit();
        }
        catch (Exception ex)
        {
            AddError(ex.Message + "InnerException: " + ex.InnerException);
            if (canCommit)
                transaction.Rollback();
        }
        return;
    }
    private async ValueTask<int> SetPersonId(string passportNumber, string passportSeria, DateOnly birthDate)
    {
        var gspPerson = await _personService.GetByPassportData(new()
        {
            Seria = passportSeria,
            Number = passportNumber,
            DateOfBirth = birthDate.AsDateTime(),
        }) ?? throw new($"Ushbu passport malumoti bo'yicha inson topilmadi: {passportNumber}-{passportSeria}  {birthDate}");
        if (gspPerson.Id == 0)
        {
            var mc = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PersonDto, CreatePersonDlDto>();
            });

            var createPersonDlDto = mc.CreateMapper().Map<CreatePersonDlDto>(gspPerson);

            var personId = _personService.Create(createPersonDlDto);
            CombineStatuses(_personService);
            if (HasErrors)
                return 0;
            UnitOfWork.Save();
            return personId.Id;
        }
        else
        {
            return gspPerson.Id;
        }
    }
    public override void Delete(long id)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.ById(id);
                var statusDto = new UpdateStatusSubsidyRequestDlDto()
                {
                    Id = id,
                    StatusId = StatusIdConst.DELETED
                };
                Repository.UpdateStatus(statusDto, ent =>
                {
                    if (!StatusIdConst.CanApplyStatus(ent.StatusId, statusDto.StatusId))
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
    #endregion

    #region Status control
    public async Task Send(SendStatusSubsidyRequestDto dto)
    {
        var canCommit = UnitOfWork.CurrentTransaction == null;
        var transaction = UnitOfWork.CurrentTransaction ?? await UnitOfWork
                                                .Context.Database.BeginTransactionAsync();
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

            var signer = new SubsidyRequestSign()
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

            var ent = Repository.UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanSubsidyRequestApplyStatus(ent.StatusId, StatusIdConst.SENT_FOR_REVIEW))
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
    private Guid SaveFile(long docId, string data, string fileName)
    {
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(data);
        writer.Flush();
        ms.Position = 0;
        var fileInfo = _storageService.Save(
            $"{nameof(TableIdConst.DUAL_EDU_SUBSIDY_REQUEST)}_SIGN_DATA",
            docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

        return fileInfo.FirstOrDefault().FileId;
    }
    public void Revoke(UpdateStatusSubsidyRequestDlDto dto)
    {
        dto.StatusId = StatusIdConst.REVOKED;
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                UpdateStatus(dto, ent => { });

                if (IsValid)
                    transaction.Commit();
                else
                {
                    transaction.Rollback();
                }
            }
            catch (DbUpdateException e)
            {
                AddError(e.Message);
                transaction.Rollback();
            }
        }
    }
    public void Accept(UpdateStatusSubsidyRequestDlDto dto)
    {
        dto.StatusId = StatusIdConst.ACCEPTED;
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                UpdateStatus(dto, ent => { });

                if (IsValid)
                    transaction.Commit();
                else
                {
                    transaction.Rollback();
                }
            }
            catch (DbUpdateException e)
            {
                AddError(e.Message);
                transaction.Rollback();
            }
        }
    }
    public void Reject(UpdateStatusSubsidyRequestDlDto dto)
    {
        dto.StatusId = StatusIdConst.REJECTED;
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                UpdateStatus(dto, ent => { });

                if (IsValid)
                    transaction.Commit();
                else
                {
                    transaction.Rollback();
                }
            }
            catch (DbUpdateException e)
            {
                AddError(e.Message);
                transaction.Rollback();
            }
        }
    }
    public void Cancel(UpdateStatusSubsidyRequestDlDto dto)
    {
        dto.StatusId = StatusIdConst.CANCELED;
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                UpdateStatus(dto, ent => { });

                if (IsValid)
                    transaction.Commit();
                else
                {
                    transaction.Rollback();
                }
            }
            catch (DbUpdateException e)
            {
                AddError(e.Message);
                transaction.Rollback();
            }
        }
    }
    private void Validation<TDto>(SubsidyRequestDlDto<TDto> dto, SubsidyRequest entity)
        where TDto : SubsidyRequestDlDto<TDto>
    {
        var query = Repository.AllAsQueryable;

        if (!StatusIdConst.CanSubsidyRequestApplyStatus(entity.StatusId, StatusIdConst.MODIFIED))
            AddError("Имкони йўқ / Нет доступа");

        if (entity != null)
            query = query.Where(a => a.Id != entity.Id);

        if (dto is CreateSubsidyRequestDlDto)
        {
            foreach (var item in dto.Tables)
            {
                if (query.Any(a => a.Month == dto.Month && a.Year == dto.Year && a.Tables.Any(a => a.PersonId == item.PersonId)))
                {
                    var personName = UnitOfWork.Context.Set<Person>().FirstOrDefault(a => a.Id == item.PersonId);

                    AddError($"{personName.FullName} uchun subsidiya arizasi boshqa korxona tomonidan berilgan");
                }
            }
        }
    }
    private HaveId<long> UpdateStatus(UpdateStatusSubsidyRequestDlDto dto, Action<SubsidyRequest> validation)
    {
        var isCommit = UnitOfWork.CurrentTransaction == null;
        var transaction = isCommit ? UnitOfWork.BeginTransaction()
        : null;
        validation += Validation(dto);
        try
        {
            var entity = Repository.UpdateStatus(dto, validation);
            CombineStatuses(Repository);
            if (HasErrors)
                return null;
            UnitOfWork.Save();
            var res = CreateDocumentChangeLog(entity.Id, message: $"SubsidyRequestUpdateStatus: {dto.StatusId}");
            if (IsValid && isCommit)
                transaction.Commit();
            return res;
        }
        finally
        {
            transaction?.Dispose(); 
        }
    }
    private Action<SubsidyRequest> Validation(UpdateStatusSubsidyRequestDlDto dto)
    {
        return ent =>
        {
            if (!StatusIdConst.CanSubsidyRequestApplyStatus(ent.StatusId, dto.StatusId))
                AddError("Имкони йўқ / Нет доступа");
        };
    }
    #endregion
    
    private HaveId<long> CreateDocumentChangeLog(long id, string message = null, string userIp = null, string userAgent = null)
    {
        var entityDto = Repository.ById<SubsidyRequestDto>(id, applyFilter: false);
        _documentChangeLogService.Create(
            dto: entityDto,
            tableId: TableIdConst.DUAL_EDU_SUBSIDY_REQUEST,
            organizationId: null,
            statusId: entityDto.StatusId,
            message: message,
            userIp: userIp,
            userAgent: userAgent);
        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }

    #region F I L E S
    public IEnumerable<SubsidyRequestFileDto> UploadFiles(params StorageFile[] files)
    {
        if (files == null || files.Length <= 0)
        {
            AddError("files can not be null");
            return null;
        }
        var result = _storageService.SaveTemp(DocumentStorageConst.DOC_DUAL_SUBSIDY_REQUEST_FILES, files)
            .Select(a => new SubsidyRequestFileDto
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
        var entity = UnitOfWork.Context.Set<SubsidyRequestFile>()
            .Include(a => a.Owner)
            .Include(a => a.SubsidyRequestTable)
            .FirstOrDefault(a => a.Id == fileId);

        return Download(fileId, entity, DocumentStorageConst.DOC_DUAL_SUBSIDY_REQUEST_FILES);
    }
    public void DeleteFile(Guid fileId)
    {
        var entity = UnitOfWork.Context
            .Set<SubsidyRequestFile>()
            .FirstOrDefault(a => a.Id == fileId);

        Delete(fileId, entity, DocumentStorageConst.DOC_DUAL_SUBSIDY_REQUEST_FILES);
    }
    private void Delete(Guid fileId, FileEntity<long> entity, string storageDocument)
    {
        if (entity != null)
        {
            _storageService.DeleteTemp(storageDocument, fileId);
            CombineStatuses(_storageService);
        }
    }
    private StorageFile Download(Guid fileId, SubsidyRequestFile entity, string storageDocument)
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
                file = _storageService.GetFile(storageDocument, entity.SubsidyRequestTableId.ToString(), fileId);
            CombineStatuses(_storageService);

            if (IsValid)
                file.FileName = entity.FileName;
        }

        return file;
    }
    #endregion
}
