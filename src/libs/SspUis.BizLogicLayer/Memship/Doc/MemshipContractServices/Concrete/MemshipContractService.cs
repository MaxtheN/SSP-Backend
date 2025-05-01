using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OfficeOpenXml;
using Spire.Doc;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.MemshipApplicationServices;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Extensions;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Memship;
using SspUis.Integration.DocxToPdf;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WbImzo.Models;
using WbImzo.Proxy.Sdk;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.Notify.Sms;
using WEBASE.OfficeTools;
using WEBASE.OfficeTools.Handlers;
using WEBASE.QRCode;
using WEBASE.Storage;
using FileFormat = Spire.Doc.FileFormat;

namespace SspUis.BizLogicLayer.Memship;
public class MemshipContractService
    : BaseEntityService<long, MemshipContract, MemshipContractListDto, MemshipContractDto, CreateMemshipContractDlDto, UpdateMemshipContractDlDto,
      IMemshipContractRepository, MemshipContractSortFilterOptions>
    , IMemshipContractService
{
    #region ctor
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDocumentChangeLogService _documentChangeLogService;
    private readonly IEImzoService _eImzoService;
    private readonly SystemConf _systemConf;
    private readonly INumberService _numberService;
    private readonly IContractorService _contractorService;
    private readonly IConvertService _pdfConverter;
    private readonly ICultureHelper _cultureHelper;
    private readonly IWbImzoService _wbImzoService;
    private readonly WbImzoConfig _wbImzoConfig;
    private readonly ISmsService _smsService;
    private readonly LinkConfig _linkConfig;
    private readonly IStorageService _storageService;
    private readonly IMemshipApplicationService _applicationService;

    public MemshipContractService(
        INumberService numberService,
        IDocumentChangeLogService documentChangeLogService,
        IAuthService authService,
        WbImzoConfig wbImzoConfig,
        List<LinkConfig> linkConfigs,
        IWbImzoService wbImzoService,
        IContractorService contractorService,
        IEImzoService eImzoService,
        IStorageService storageService,
        IMemshipApplicationService applicationService,
        IConvertService pdfConverter,
        ICultureHelper cultureHelper,
        IUnitOfWork unitOfWork,
        ISmsService smsService,
        SystemConf systemConf)
        : base(unitOfWork)
    {
        _authService = authService;
        _unitOfWork = unitOfWork;
        _documentChangeLogService = documentChangeLogService;
        _numberService = numberService;
        _contractorService = contractorService;
        _eImzoService = eImzoService;
        _storageService = storageService;
        _applicationService = applicationService;
        _pdfConverter = pdfConverter;
        _cultureHelper = cultureHelper;
        _smsService = smsService;
        _wbImzoService = wbImzoService;
        _wbImzoConfig = wbImzoConfig;
        _linkConfig = linkConfigs.FirstOrDefault(config => config.Name == "MemshipContract");

        this._systemConf = systemConf;
    }
    #endregion

    #region Get Function
    public PagedResult<MemshipContractListDto> GetList(MemshipContractSortFilterOptions options)
    {
        var lang = _cultureHelper.CurrentCulture.Id;
        var result = Repository.ReadAsNoTracked<MemshipContractListDto>()
            .SortFilter(options)
            .AsPagedResult(options);

        //var listDto = result.Rows.ToList();       200144908

        //result.Rows = listDto;
        return result;
    }
    public int GetCount()
    {
        MemshipContractSortFilterOptions memshipContractSortFilterOptions =
            new MemshipContractSortFilterOptions();

        return GetList(memshipContractSortFilterOptions).Rows.Count();
    }
    public SelectList<long> AsSelectList()
    {
        return Repository.AllAsQueryable.Where(a => a.StatusId == StatusIdConst.ACCEPTED)
                        .AsSelectList();
    }
    public MemshipContractDto Get()
    {
        return new MemshipContractDto
        {
            DocOn = DateOnly.FromDateTime(DateTime.Now),
            //DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_MEMSHIP_CONTRACT, 1).Item2
        };
    }
    public override MemshipContractDto Get(long id)
    {
        var dto = Repository.ById<MemshipContractDto>(id);
        CombineStatuses(Repository);
        if (IsValid)
        {
            dto.CanModify = StatusIdConst.CanMemshipContractApplyStatus(dto.StatusId, StatusIdConst.MODIFIED);
            dto.CanAccept = StatusIdConst.CanMemshipContractApplyStatus(dto.StatusId, StatusIdConst.ACCEPTED);
            dto.CanCancel = StatusIdConst.CanMemshipContractApplyStatus(dto.StatusId, StatusIdConst.REJECTED);
            dto.CanDelete = StatusIdConst.CanMemshipContractApplyStatus(dto.StatusId, StatusIdConst.DELETED);
            #region Edit File
            bool canEdit = dto.StatusId == StatusIdConst.SENT_FOR_REVIEW ||
                           dto.StatusId == StatusIdConst.SIGNING ||
                           dto.StatusId == StatusIdConst.CREATED;
            dto.CanEditFile = canEdit;
            #endregion
            dto.CanConfirm = _authService.User.OrganizationId == OrganizationIdConst.SSP
                && dto.ApplicationId != null
                && dto.MemshipContractTypeId == MemshipContractTypeIdConst.FREE
                && dto.StatusId == StatusIdConst.SENT_FOR_REVIEW;

            dto.CanChangeToPaid = dto.MemshipContractTypeId == MemshipContractTypeIdConst.FREE && dto.StatusId == StatusIdConst.CREATED;

            if (_authService.Contractor == null)
                dto.CanSign = StatusIdConst.CanMemshipContractApplyStatus(dto.StatusId, StatusIdConst.SIGNED);
            else
                dto.CanSign = StatusIdConst.CanMemshipContractApplyStatus(dto.StatusId, StatusIdConst.SENT_FOR_REVIEW);
        }
        return dto;
    }
    public MemshipContractDto GetByApplicationId(int applicationId)
    {
        var application = _unitOfWork.Context.Set<MemshipApplication>()
            .Include(a => a.Application)
            .Include(a => a.ChoosedRegion)
            .Include(a => a.Application.Contractor)
            .Include(a => a.Application.Contractor.SettlementAccounts)
            .Include(a => a.Application.Contractor.Bank)
            .FirstOrDefault(a => a.ApplicationId == applicationId);

        if (application == null)
        { AddError("Ariza topilmadi."); return null; }

        var orgByRegion = UnitOfWork.Context.Set<Organization>()
            .FirstOrDefault(org =>
            IsPayed(application.ContractorCategoryId, application.Application.Contractor.OpfId ?? 0) ?
            org.Id == OrganizationIdConst.SSP
            : ((application.ChooseLocation ?
                org.RegionId == application.ChoosedRegionId
                : org.RegionId == application.Application.RegionId)
                && (org.OrganizationGroupId == OrganizationGroupIdConst.SSP || org.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH)));

        var districtId = application.ChooseLocation ? application.ChoosedDistrictId.Value : application.Application.DistrictId;

        if (orgByRegion == null)
        {
            AddError("Ushbu hududda tashkilot hududiy boshqarmasi topilmadi: "
                + (application.ChooseLocation ? application.ChoosedRegion.FullName : application.Application.RegionName));
            return null;
        }
        var dto = new MemshipContractDto()
        {
            DocOn = DateOnly.FromDateTime(DateTime.Now),
            DocNumber = this._numberService.GetNext(
                NumberTemplateDocumentConst.DOC_MEMSHIP_CONTRACT,
                organizationId: orgByRegion.Id,
                regionId: IsPayed(application.ContractorCategoryId, application.Application.Contractor.OpfId ?? 0) ? 0 : orgByRegion.RegionId,
                districtId: IsPayed(application.ContractorCategoryId, application.Application.Contractor.OpfId ?? 0) ? 0 : districtId).Item2,
            ApplicationId = application.ApplicationId,
            ContractorSettlementAccountId = application.Application.Contractor.SettlementAccounts
            .FirstOrDefault(ac => ac.IsMain)?.Id ?? null,
            Contractor = application.Application.Contractor.FullName,
            ContractorSettlementAccount = application.Application.Contractor.SettlementAccounts
            .FirstOrDefault(a => a.IsMain)?.AccountCode ?? null,
            Director = application.Application.Contractor.Director,
            Bank = application.Application.Contractor.Bank?.BankName,
            BankId = application.Application.Contractor.BankId ?? 0,
            ContractorCategoryId = application.ContractorCategoryId,
            CanSelectOrganization = IsPayed(application.ContractorCategoryId, application.Application.Contractor.OpfId ?? 0) ? true : false,

            OrganizationId = IsPayed(application.ContractorCategoryId, application.Application.Contractor.OpfId ?? 0) ? OrganizationIdConst.SSP : orgByRegion.Id,

            MemshipContractTypeId =
            IsPayed(application.ContractorCategoryId, application.Application.Contractor.OpfId ?? 0)
            ? MemshipContractTypeIdConst.PAID
            : MemshipContractTypeIdConst.FREE
        };

        if (IsValid)
        {
            dto.CanModify = StatusIdConst.CanApplyStatus(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.MemshipContractEdit);
            dto.CanAccept = StatusIdConst.CanApplyStatus(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.MemshipContractSign);
            dto.CanCancel = StatusIdConst.CanApplyStatus(dto.StatusId, StatusIdConst.REJECTED) && _authService.HasPermission(ModuleCode.MemshipContractReject);
            dto.CanDelete = StatusIdConst.CanApplyStatus(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.MemshipContractDelete);
        }

        return dto;
    }

    #endregion

    #region CRUD
    private static bool IsPayed(int contractorCategoryId, int opfId)
    {
        return (contractorCategoryId == ContractorCategoryIdConst.YIRIK_KORXONA
                    || opfId == OpfIdConst.UYUSHMA);
    }
    public async ValueTask<HaveId<long>> Create(CreateMemshipContractDlDto dto)
    {
        if (dto.ContractorCategoryId == null)
        {
            AddError("ContractorCategoryId can not be null");
            return null;
        }
        var application = UnitOfWork.Context.Set<MemshipApplication>()
            .Include(x => x.Application).ThenInclude(x => x.Contractor)
            .FirstOrDefault(a => a.ApplicationId == dto.ApplicationId);
        
        dto.MemshipContractTypeId = IsPayed(dto.ContractorCategoryId.Value, application.Application.Contractor.OpfId ?? 0) ? MemshipContractTypeIdConst.PAID : MemshipContractTypeIdConst.FREE;

        if (application == null)
        {
            AddError("Ariza topilmadi.");
            return null;
        }

        var orgByRegion = UnitOfWork.Context.Set<Organization>()
            .FirstOrDefault(org =>
           IsPayed(dto.ContractorCategoryId.Value, application.Application.Contractor.OpfId ?? 0) ?
            org.Id == OrganizationIdConst.SSP
            : ((application.ChooseLocation ?
            org.RegionId == application.ChoosedRegionId
            : org.RegionId == application.Application.RegionId)
            && org.OrganizationGroupId == OrganizationGroupIdConst.SSP));

        bool isCommit = UnitOfWork.CurrentTransaction == null;
        using var transaction = isCommit ? UnitOfWork.BeginTransaction()
            : UnitOfWork.CurrentTransaction;

        try
        {
            dto.OrganizationId = (IsPayed(dto.ContractorCategoryId.Value, application.Application.Contractor.OpfId ?? 0)
                && dto.OrganizationId == null)
                ? OrganizationIdConst.SSP
                : orgByRegion.Id;
            
            var entity = base.Repository.Create(dto, ent => Validation(dto, ent));

            CombineStatuses(Repository);
            if (HasErrors)
            {
                transaction.Rollback();
                return null;
            }
            UnitOfWork.Save();

            await _applicationService.Accept(new()
            {
                Id = application.Id,
                Message = "Application accepted in creating contract"
            });
            CombineStatuses(_applicationService);
            if (HasErrors)
            {
                transaction.Rollback();
                return null;
            }
            _unitOfWork.Save();

            CreateDocumentChangeLog(entity.Id, "Create");

            if (IsValid && isCommit)
            {
                transaction.Commit();

              //  await PostToIMZOAndSentUrl(entity);
            }

            return HaveId.Create(entity.Id);
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " InnerException: " + e.InnerException);
            transaction.Rollback();
        }
        return null;
    }
    public override void Update(UpdateMemshipContractDlDto dto)
    {
        var entityModel = Repository.ById(dto.Id);
        dto.OrganizationId = entityModel.OrganizationId;
        using var transaction = UnitOfWork.BeginTransaction();
        try
        {
            var entity = base.Repository.Update(dto, ent => Validation(dto, ent));
            UnitOfWork.Save();
            _storageService.ResolveMarkedFiles(DocumentStorageConst.DOC_MEMSHIP_CONTRACT_FILE, dto.Id.ToString());
            CombineStatuses(Repository);
            if (IsValid)
                transaction.Commit();
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " Inner: " + e.InnerException);
            transaction.Rollback();
        }
    }
    public void Comfirm(long id)
    {
        var entity = Repository.AllAsQueryable.FirstOrDefault(x => x.Id == id);
        if (entity == null)
        {
            AddError("Shartnoma topilmadi");
            return;
        }
        entity.IsRead = false;
        entity.StatusId = StatusIdConst.SIGNING;
        UnitOfWork.Context.SaveChanges();
        CreateDocumentChangeLog(entity.Id, "Comfirm clicked");
    }
    public async ValueTask<(string? Url, bool Result)> WebImzoSign(SignWebImzoContractFilter filter)
    {
        var doc = _unitOfWork.Context.Set<MemshipContract>().Include(a => a.Contractor)
                                        .Include(x => x.Signs)
                                        .FirstOrDefault(x => x.Id == filter.ContractId);

        if (doc == null)
        {
            AddError("По вашему запросу запись не найдено");
            return (Url: null, Result: false);
        }

        if (doc.StatusId == StatusIdConst.SIGNED)
        {
            AddError("Shartnoma allaqachon imzolangan");
            return (Url: null, Result: false);
        }

        if (_authService.Contractor != null)
        {
            if (doc.StatusId != StatusIdConst.SIGNING || doc.StatusId != StatusIdConst.SENT_FOR_REVIEW)
            {
                var Url = await PostToIMZOAndSentUrl(doc);
                if (Url != null)
                    return (Url: Url, Result: true);
            }
        }

        else
        {
            #region Bu HRM tayyor bo'ganda commentdan ochamiz o'chirmanglar
            if (!_systemConf.IsTest)
            {
                var signer = UnitOfWork.Context.Set<SignCriterion>()
                    .Include(x => x.Position)
                    .Where(x => x.ApplicationTypeId == ApplicationTypeIdConst.MEMSHIP);
                if (signer == null)
                {
                    AddError("Imzo chekuvchi belgilanmagan. Bu hujjat imzolash uchun lavozim belgilashni talab qiladi.");
                    return (Url: null, Result: false);
                }
                var employeeManage = UnitOfWork.Context.EmployeeManages.FirstOrDefault(x => x.Id == _authService.User.EmployeeManageId);

                if (!signer.Any(x => x.PositionId == employeeManage.PositionId))
                {
                    AddError("Ushbu hujjatni imzolash uchun lavozim belgilangan lavozimga to'g'ri kelmaydi.");
                    return (Url: null, Result: false);
                }
            }
        }

        var url = await PostToIMZOAndSentUrl(doc);
        if (url != null)
            return (Url: url, Result: true);

        return (Url: null, Result: false);
    }
    public void Sign(SignStatusMemshipContractDto dto)
    {
        var doc = Repository.Context.Set<MemshipContract>()
            .Include(x => x.Signs)
            .FirstOrDefault(x => x.Id == dto.Id);

        if (doc == null)
        {
            AddError("По вашему запросу запись не найдено");
            return;
        }
        if (_authService.Contractor != null)
        {
            dto.StatusId = doc.MemshipContractTypeId == MemshipContractTypeIdConst.FREE ? StatusIdConst.SENT_FOR_REVIEW : StatusIdConst.SIGNING;
            if (!(doc.ContractorId != _authService.User.Id))
            {
                AddError("Нет доступа");
                return;
            }
        }
        else
        {
            if (doc.StatusId != StatusIdConst.SIGNING)
            {
                AddError("Bu shartnoma tadbirkor tomonidan imzolanmagan. :( ");
                return;
            }

            #region Bu HRM tayyor bo'ganda commentdan ochamiz o'chirmanglar
            if (!_systemConf.IsTest)
            {
                var signer = UnitOfWork.Context.Set<SignCriterion>()
                    .Include(x => x.Position)
                    .Where(x => x.ApplicationTypeId == ApplicationTypeIdConst.MEMSHIP);
                if (signer == null)
                {
                    AddError("Imzo chekuvchi belgilanmagan. Bu hujjat imzolash uchun lavozim belgilashni talab qiladi.");
                    return;
                }
                EmployeeManage employeeManage = UnitOfWork.Context.EmployeeManages.FirstOrDefault(x => x.Id == _authService.User.EmployeeManageId);

                if (!signer.Any(x => x.PositionId == employeeManage.PositionId))
                {
                    AddError("Ushbu hujjatni imzolash uchun lavozim belgilangan lavozimga to'g'ri kelmaydi.");
                    return;
                }
            }
            #endregion

            dto.StatusId = StatusIdConst.SIGNED;
            dto.IsRead = false;
        }
        var timeStamp = _eImzoService.TimeStamp(new EImzoTimeStampDto
        {
            SignData = dto.SignedData,
            Inn = dto.IsPinfl ? null : _authService.Contractor?.Inn,
            Pinfl = dto.IsPinfl
                 ? (_authService.Contractor == null ? _authService.User.Pinfl : _authService.Contractor.Pinfl)
                 : null
        }).Result;

        CombineStatuses(_eImzoService);
        if (HasErrors)
            return;

        if (_authService.Contractor != null)
        {
            var eImzoVerifyAttached = _eImzoService.VerifyAttached(new EImzoVerifyDto
            {
                SignData = timeStamp.Pkcs7b64,
                Inn = dto.IsPinfl ? null : _authService.Contractor.Inn,
                Pinfl = dto.IsPinfl ? _authService.Contractor.Pinfl : null
            }).Result;
        }
        else
        {
            var eImzoVerifyAttached = _eImzoService.VerifyAttached(new EImzoVerifyDto
            {
                SignData = timeStamp.Pkcs7b64,
                Inn = null,
                Pinfl = _authService.User.Pinfl
            }).Result;
        }
        CombineStatuses(_eImzoService);
        if (HasErrors)
            return;

        dto.SignFile = SaveFile(doc.Id, timeStamp.Pkcs7b64, "sign.txt");
        dto.DataFile = SaveFile(doc.Id, JsonConvert.SerializeObject(doc), "data.txt");
        dto.SignedUserInfo = _authService.UserName + " - " + _authService.User.FullName;

        using var transaction = _unitOfWork.BeginTransaction();
        try
        {
            UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanMemshipContractApplyStatus(ent.StatusId, dto.StatusId))
                    Repository.AddError("Нет доступа");
            });
            doc.Signs.Add(new()
            {
                OwnerId = doc.Id,
                SignFile = dto.SignFile,
                SignedAt = DateTime.Now,
                DataFile = dto.DataFile,
                StatusId = dto.StatusId,
                SignedUserInfo = dto.SignedUserInfo,
            });
            UnitOfWork.Save();

            CreateDocumentChangeLog(doc.Id);

            //Repository.Context.SaveChanges();
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
    //public async ValueTask Reject(RejectStatusMemshipContractDto dto)
    //{
    //    var doc = Repository.ById<MemshipContractDto>(dto.Id, applyFilter: false);

    //    var memeshipContract = _unitOfWork.Context.Set<MemshipContract>()
    //                                                          .Include(x => x.Files)
    //                                                          .FirstOrDefault(x => x.Id == dto.Id);

    //    using (var transaction = _unitOfWork.BeginTransaction())
    //    {
    //        try
    //        {
    //            UpdateStatus(dto, ent => { });

    //            if (doc?.MemshipApplicationId is not null)
    //            {
    //                _applicationService.Reject(
    //                    new RejectStatusMemshipApplicationDto
    //                    {
    //                        Id = (long)doc.MemshipApplicationId
    //                    });
    //            }

    //            foreach (var fileDto in dto.Files)
    //            {
    //                var file = memeshipContract.Files.FirstOrDefault(f => f.Id == fileDto.Id);
    //                if (file != null)
    //                {
    //                    file.IsReject = false;
    //                    _unitOfWork.Context.Entry(file).State = EntityState.Modified;
    //                }
    //            }
    //            CombineStatuses(_applicationService);

    //            _unitOfWork.Save();
    //            dto.UpdateEntity(memeshipContract);
    //            _storageService.MoveToPersistent(DocumentStorageConst.DOC_MEMSHIP_CONTRACT_FILE, dto.Id.ToString(), dto.Files.Select(a => a.Id).ToArray());
    //            CombineStatuses(_storageService);

    //            if (IsValid)
    //            {
    //                transaction.Commit();

    //                WbImzoUpdateSignRequestStateDto updateState = new()
    //                {
    //                    ApiKey = _wbImzoConfig.ApiKey,
    //                    DocumentId = dto.Id,
    //                    DocumentType = "A'zolik shartnomasi",
    //                    DocumentIdAsString = JsonConvert.SerializeObject(memeshipContract),
    //                    TableId = TableIdConst.MEMSHIP__DOC_MEMSHIP_CONTRACT,
    //                };

    //                var log = CreateDocumentChangeLog(dto.Id, dto.RejectMessage, _authService.UserIp, _authService.UserAgent);
    //                await _wbImzoService.UpdateSignRequestStateAsync(updateState);
    //            }
    //            else
    //            {
    //                transaction.Rollback();
    //            }
    //        }
    //        catch (DbUpdateException e)
    //        {
    //            AddError(e.Message);
    //            transaction.Rollback();
    //        }
    //    }
    //}
    public async ValueTask Cancel(CancelStatusMemshipContractDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        var doc = Repository.ById<MemshipContractDto>(dto.Id, applyFilter: false);
        var memshipContract = _unitOfWork.Context.Set<MemshipContract>().FirstOrDefault(a => a.Id == dto.Id);
        if (doc == null)
        {
            AddError("Entity not found");
            return;
        }

        var claimApplication = _unitOfWork.ApplicationRepository.AllAsQueryable.Where(a => a.ApplicationTypeId == ApplicationTypeIdConst.CLAIM && a.ClaimApplication.MemshipContractId == doc.Id);

        if (claimApplication.Any())
        {
            AddError("Имкони йўқ / Нет доступа");
            return;
        }

        try
        {
            Repository.UpdateStatus(dto, ent => { });
            if (doc.MemshipApplicationId != null)
                _applicationService.Cancel(new CancelStatusMemshipApplicationDto { Id = (long)doc.MemshipApplicationId, IsRead = false });

            CombineStatuses(_applicationService);

            if (IsValid)
            {
                _unitOfWork.Save();
                if (canCommit)
                {
                    transaction.Commit();
                    WbImzoUpdateSignRequestStateDto updateState = new()
                    {
                        ApiKey = _wbImzoConfig.ApiKey,
                        DocumentId = dto.Id,
                        DocumentType = "A'zolik shartnomasi",
                        DocumentIdAsString = JsonConvert.SerializeObject(memshipContract),
                        TableId = TableIdConst.MEMSHIP__DOC_MEMSHIP_CONTRACT,
                    };

                    var log = CreateDocumentChangeLog(dto.Id, message: dto.Message, _authService.UserIp, _authService.UserAgent);
                    await _wbImzoService.UpdateSignRequestStateAsync(updateState);
                }
            }
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
        finally
        {
            if (canCommit)
                transaction.Dispose();
        }
    }
    public async ValueTask<ApiResult<bool>> UpdateSignRequestStateAsync(MemshipContract contract)
    {
        WbImzoUpdateSignRequestStateDto updateState = new()
        {
            ApiKey = _wbImzoConfig.ApiKey,
            DocumentId = contract.Id,
            DocumentType = "A'zolik shartnomasi",
            DocumentIdAsString = JsonConvert.SerializeObject(contract),
            TableId = TableIdConst.MEMSHIP__DOC_MEMSHIP_CONTRACT,
        };

       return await _wbImzoService.UpdateSignRequestStateAsync(updateState);
    }
    public override void Delete(long id)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.ById(id);
                var statusDto = new UpdateStatusMemshipContractDlDto()
                {
                    Id = id,
                    StatusId = StatusIdConst.DELETED
                };
                Repository.UpdateStatus(statusDto, ent =>
                {
                    if (!StatusIdConst.CanMemshipContractApplyStatus(ent.StatusId, statusDto.StatusId))
                        AddError("Нет доступа");


                    ent.IsRead = false;
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
    #region FILE
    public async Task<byte[]> DownloadTemplate(bool isFree = false)
    {
        try
        {
            string templateName;

            if (isFree)
            {
                templateName = StaticFileConst.WordTemplate.MEMSHIP_CONTRACT_FREE;
            }
            else
            {
                templateName = StaticFileConst.WordTemplate.MEMSHIP_CONTRACT;
            }

            var wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName("ru", templateName)
            );

            return wordFile.ToArray();
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
            return null; // or throw ex; if you want to propagate the exception
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
    public async Task<byte[]> GetWordTemplate(Guid id2)
    {
        try
        {
            var lang = "uz-cyrl";
            var templateName = StaticFileConst.WordTemplate.MEMSHIP_CONTRACT;

            var memshipContract = UnitOfWork.Context.Set<MemshipContract>().FirstOrDefault(a => a.Id2 == id2);

            if (memshipContract != null && memshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.FREE)
            {
                templateName = StaticFileConst.WordTemplate.MEMSHIP_CONTRACT_FREE;
            }

            var wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(lang, templateName));

            return wordFile.ToArray();
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
        }
        return null;
    }
    public byte[] DownloadFile(Guid? id2, Guid? fileId)
    {
        if (id2 == null && fileId == null)
        {
            AddError("Both id2 and fileId are null. At least one must be provided.");
            return null;
        }

        byte[] fileBytes = null;

        if (id2 != null)
        {
            MemshipContract entity = null;

            entity = UnitOfWork.Context.Set<MemshipContract>().Include(x => x.Files).FirstOrDefault(a => a.Id2 == id2);
            fileId = entity.Files.OrderByDescending(a => a.Id).FirstOrDefault()?.Id;
            if (fileId == null)
                return null;
            if (entity == null)
            {
                AddError($"File with id2 '{id2}'{(fileId != null ? $" and fileId '{fileId}'" : "")} not found.");
                return null;
            }

            //fileBytes = DownloadPdf(id2.Value);
            //}
            //else if (fileId != null)
            //{
            var fileEntity = UnitOfWork.Context.Set<MemshipContractFile>().FirstOrDefault(a => a.Id == fileId);

            //    if (entity == null)
            //    {
            //        AddError($"File with fileId '{fileId}' not found.");
            //        return null;
            //    }

            var storageFile = Download(fileId.Value, fileEntity, DocumentStorageConst.DOC_MEMSHIP_CONTRACT_FILE);

            if (storageFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    storageFile.GetStream().CopyTo(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }
            }
        }

        return fileBytes;
    }
    public object UploadFiles(params StorageFile[] files)
    {
        if (files == null || files.Length <= 0)
        {
            AddError("Empty file");
            return null;
        }
        var result = _storageService.SaveTemp(DocumentStorageConst.DOC_MEMSHIP_CONTRACT_FILE, files)
            .Select(a => new MemshipContractFileDto
            {
                Id = a.FileId,
                FileName = a.FileName,
                CreatedAt = DateTime.Now,
            });

        CombineStatuses(_storageService);
        return IsValid ? result : null;
    }
    public StorageFile DownloadByIdFile(Guid fileId)
    {
        var entity = _unitOfWork.Context
            .Set<MemshipContractFile>()
            .FirstOrDefault(a => a.Id == fileId);

        return Download(fileId, entity, DocumentStorageConst.DOC_MEMSHIP_CONTRACT_FILE);
    }
    public byte[] DownloadPdf(Guid id2, string? lange)
    {
        string language = lange ?? "uz-latn";
        #region collecting data

        var wordFile = _storageService.GetStaticFile(
            StaticFileConst.WordTemplate.GetFileName(
              language,
                StaticFileConst.WordTemplate.MEMSHIP_CONTRACT)
            );
        //if (id2 == Guid.Empty)
        //{
        //    wordFile = _storageService.GetStaticFile(
        //        StaticFileConst.WordTemplate.GetFileName(
        //            lang, StaticFileConst.WordTemplate.MEMSHIP_CONTRACT_EMPTY));
        //    return _pdfConverter.DocxToPdfAsync(wordFile, new object()).Result;
        //}

        //var lan = UnitOfWork.Context.Set<Language>()
        //    .FirstOrDefault(l => l.Code == ServiceProvider.CultureHelper.CurrentCulture.Code)?.Id ?? 1;
        //var lang = "";
        //switch (lan)
        //{
        //    case 1:
        //        {
        //            lang = "uz-latn";
        //            break;
        //        }
        //    case 3:
        //        {
        //            lang = "ru";
        //            break;
        //        }
        //    case 2:
        //        {
        //            lang = "uz-cyrl";
        //            break;
        //        }



        var memshipContract = UnitOfWork.Context.Set<MemshipContract>()
            .Include(c => c.Organization)
            .Include(c => c.Signs)
            .Include(c => c.Organization.Oked)
            .Include(c => c.Organization.Translates)
            .Include(c => c.Organization.SettlementAccounts)
            .ThenInclude(c => c.Bank)
            .ThenInclude(c => c.Translates)
            .Include(c => c.Application)
            .ThenInclude(c => c.Region)
            .ThenInclude(c => c.Translates)
            .Include(c => c.Application)
            .ThenInclude(c => c.MemshipApplication)
            .ThenInclude(c => c.ChoosedRegion)
            .ThenInclude(c => c.Translates)
            .FirstOrDefault(c => c.Id2 == id2);

        if (memshipContract == null)
        {
            AddError("Contract not found");
            return null;
        }
        if (memshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.FREE)
        {
            wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                    ServiceProvider.CultureHelper.CurrentCulture.Code, StaticFileConst.WordTemplate.MEMSHIP_CONTRACT_FREE));
        }

        var entity = UnitOfWork.Context.Set<MemshipContract>().Include(x => x.Files).FirstOrDefault(a => a.Id2 == id2);
        var fileId = entity.Files.OrderByDescending(a => a.Id).FirstOrDefault()?.Id;
        var fileEntity = UnitOfWork.Context.Set<MemshipContractFile>().FirstOrDefault(a => a.Id == fileId);

        var storageFile = DownloadFile(id2: memshipContract.Id2, Guid.Empty);
        //var stream = storageFile.GetStream();
        //MemoryStream wordFile = new();
        if (storageFile != null && (!fileEntity.IsReject.HasValue || fileEntity.IsReject == true))
        {
            var tempPath = "TempWordFileMemshipContract.docx";
            File.WriteAllBytes(tempPath, storageFile);

            using (FileStream file = new FileStream(tempPath, FileMode.Open, FileAccess.Read))
            {
                wordFile = new MemoryStream();
                file.CopyTo(wordFile);
                wordFile.Position = 0;
                //var tempFile = File.OpenRead(tempPath);

            }
            //tempFile.Close();
            File.Delete(tempPath);
        }
        var contractor = UnitOfWork.Context.Set<Contractor>()
               .Include(c => c.Oked)
               .Include(c => c.SettlementAccounts)
               .ThenInclude(c => c.Bank).ThenInclude(b => b.Translates)
               .FirstOrDefault(c => c.Id == memshipContract.ContractorId);
        var signSsp = memshipContract.Signs.FirstOrDefault(s => s.StatusId == StatusIdConst.SIGNED);
        var signContractor = memshipContract.Signs.FirstOrDefault(s =>
            s.StatusId == StatusIdConst.SENT_FOR_REVIEW
            || s.StatusId == StatusIdConst.SIGNING);

        var placeholder = new Placeholders();

        var link = _systemConf.QrImagePrintMy + "/Memship/MemshipContract/DownloadPdf?id2=" + memshipContract.Id2.ToString();

        var qrDownload = new MemoryStream(QRCodeHelper.GeneratePng(link));
        placeholder.ImagePlaceholders.Add("QrDownload", new() { Dpi = 512, MemStream = qrDownload });

        string rejectMessage = $"Bekor qilish sababi : {memshipContract.RejectMessage}";
        string rejetDate = $"Bekor qilish sanasi : {memshipContract.RejectDate?.ToString("dd-MM-yyyy")}";

        if (signSsp != null)
        {
            var QrCodeSsp = new MemoryStream(QRCodeHelper.GeneratePng(memshipContract.Id2.ToString()
                    + "  " + memshipContract.DocNumber
                    + "  " + memshipContract.DocOn.ToString(Constants.DATE_FORMAT)
                    //+ "  " + sign.Pinfl
                    + "  " + signSsp.SignedUserInfo));
            if (memshipContract.StatusId == StatusIdConst.REJECTED)
            {
                placeholder.TextPlaceholders.Add("RejectMessage", rejectMessage);
                placeholder.TextPlaceholders.Add("RejectDate", rejetDate);
                placeholder.TextPlaceholders.Add("QrCodeSsp", "");
            }
            else
            {
                placeholder.ImagePlaceholders.Add("QrCodeSsp",
                new() { Dpi = 512, MemStream = QrCodeSsp });
                placeholder.TextPlaceholders.Add("QrCodeSsp", "++QrCodeSsp++");
                placeholder.TextPlaceholders.Add("RejectMessage", "");
                placeholder.TextPlaceholders.Add("RejectDate", "");
            }
        }
        else if (memshipContract.StatusId == StatusIdConst.REJECTED)
        {
            placeholder.TextPlaceholders.Add("RejectMessage", rejectMessage);
            placeholder.TextPlaceholders.Add("RejectDate", rejetDate);
            placeholder.TextPlaceholders.Add("QrCodeSsp", "");
        }
        else
        {
            placeholder.TextPlaceholders.Add("QrCodeSsp", "");
            placeholder.TextPlaceholders.Add("RejectMessage", "");
            placeholder.TextPlaceholders.Add("RejectDate", "");
        }

        if (signContractor != null)
        {
            var QrCodeContractor = new MemoryStream(QRCodeHelper.GeneratePng(memshipContract.Id2.ToString()
                   + "  " + memshipContract.DocNumber
                   + "  " + memshipContract.DocOn.ToString(Constants.DATE_FORMAT)
                   //+ "  " + sign.Pinfl
                   + "  " + signContractor.SignedUserInfo));
            if (memshipContract.StatusId == StatusIdConst.REJECTED)
            {
                placeholder.TextPlaceholders.Add("QrCode", "");
            }
            else
            {
                placeholder.ImagePlaceholders.Add("QrCode",
                new() { Dpi = 512, MemStream = QrCodeContractor });
                placeholder.TextPlaceholders.Add("QrCode", "++QrCode++");
            }

        }
        else
            placeholder.TextPlaceholders.Add("QrCode", "");


        #endregion

        #region plh
        placeholder.TextPlaceholders.Add(nameof(memshipContract.DocNumber), memshipContract.DocNumber);
        DateTimeFormatInfo info = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat;
        var month = info.MonthNames[memshipContract.DocOn.Month - 1];
        if (month.Last() == 'ь')
            month = month.Replace("ь", "");
        placeholder.TextPlaceholders.Add("DocOnText", "\"" + memshipContract.DocOn.Day + "\"" + month);

        //if (memshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.FREE)
        placeholder.TextPlaceholders.Add(nameof(memshipContract.DocOn), memshipContract.DocOn.ToString(Constants.DATE_FORMAT));
        //else
        //    placeholder.TextPlaceholders.Add(nameof(memshipContract.DocOn), signSsp?.SignedAt?.AsDateOnly().ToString(Constants.DATE_FORMAT) ?? "-");

        placeholder.TextPlaceholders.Add(nameof(memshipContract.BaseFixedMinimumValue)
            , memshipContract.BaseFixedMinimumValue.ToString() + " ("
            + memshipContract.BaseFixedMinimumValue.DecimalInWord(language) + ") ");

        placeholder.TextPlaceholders.Add(nameof(memshipContract.Organization.Director),
            memshipContract.Organization.Director);

        var signUser = signSsp != null ? UnitOfWork.Context.Set<User>()
            .Include(x => x.Person)
            .FirstOrDefault(x => x.Id == signSsp.CreatedUserId) : null;

        string position = "";
        if (signUser != null)
            position = UnitOfWork.Context.EmployeeManages
                .Include(x => x.Position)
                .FirstOrDefault(x => x.Employee.PersonId == signUser.PersonId)
                ?.Position?.FullName ?? "";

        placeholder.TextPlaceholders.Add(nameof(signUser),
            position + "  " +
            signUser?.Person?.FullName ?? "-");

        var languageId = UnitOfWork.Context.Set<Language>()
    .FirstOrDefault(l => l.Code == language)?.Id ?? 1;
        placeholder.TextPlaceholders.Add(nameof(memshipContract.Organization.FullName),
            memshipContract.Organization.Translates.FirstOrDefault(x =>
                x.LanguageId == languageId
                && x.ColumnName == TranslateColumn.full_name.ToString())
            ?.TranslateText ?? memshipContract.Organization.FullName);
        placeholder.TextPlaceholders.Add(nameof(memshipContract.Organization.Address), memshipContract.Organization.Address);
        var orgAccount = memshipContract.OrganizationSettlementAccount;
        placeholder.TextPlaceholders.Add(nameof(orgAccount.AccountCode), orgAccount?.AccountCode ?? "-");
        placeholder.TextPlaceholders.Add(
            nameof(orgAccount.Bank.BankName),
            orgAccount?.Bank.Translates.FirstOrDefault(c =>
                    c.LanguageId == languageId
                    && c.ColumnName == BankTranslateColumn.bank_name.ToString()
                    )?.TranslateText ?? orgAccount?.Bank.BankName ?? "-");
        placeholder.TextPlaceholders.Add(nameof(orgAccount.Bank.Code), orgAccount?.Bank.Code ?? "-");
        placeholder.TextPlaceholders.Add(nameof(memshipContract.Organization.Inn), memshipContract.Organization.Inn);
        placeholder.TextPlaceholders.Add(nameof(memshipContract.Details), memshipContract.Details);
        placeholder.TextPlaceholders.Add(nameof(memshipContract.Organization.Oked), memshipContract.Organization.Oked.Code);
        placeholder.TextPlaceholders.Add(nameof(memshipContract.Organization.PhoneNumber), memshipContract.Organization.PhoneNumber ?? "");

        placeholder.TextPlaceholders.Add("ContractorFullName", contractor.FullName);
        placeholder.TextPlaceholders.Add("ContractorDirector", contractor.Director);
        placeholder.TextPlaceholders.Add("ContractorAddress", contractor.Address);
        var account = contractor.SettlementAccounts.FirstOrDefault(a => a.IsMain);
        placeholder.TextPlaceholders.Add("ContractorAccountCode", account?.AccountCode ?? "-");

        placeholder.TextPlaceholders.Add("ContractorBankName",
            contractor.Bank?.Translates.FirstOrDefault(c =>
                    c.LanguageId == languageId
                    && c.ColumnName == BankTranslateColumn.bank_name.ToString()
                    )?.TranslateText ?? contractor.SettlementAccounts.FirstOrDefault(a => a.IsMain)?.Bank.BankName ?? "");
        placeholder.TextPlaceholders.Add("ContractorBankCode", contractor.Bank?.Code ?? "-");
        placeholder.TextPlaceholders.Add("ContractorOked", contractor.Oked?.Code ?? "-");
        placeholder.TextPlaceholders.Add("ContractorPhoneNumber", contractor?.PhoneNumber ?? "-");
        placeholder.TextPlaceholders.Add("ContractorInn", contractor?.Inn ?? "-");
        Region region;
        var toshkent = UnitOfWork.Context.Set<Region>()
            .Include(x => x.Translates)
            .FirstOrDefault(x => x.Id == RegionIdConst.TASHKENT);

        if (memshipContract?.Application?.MemshipApplication?.ChooseLocation ?? false)
        {
            region = (memshipContract?.Application?.MemshipApplication?.ChoosedRegion) ?? null;
        }
        else
        {
            region = (memshipContract?.Application?.Region) ?? toshkent;
        };

        placeholder.TextPlaceholders.Add(nameof(memshipContract.Application.Region),
            memshipContract.MemshipContractTypeId == MemshipContractTypeIdConst.PAID ?
            " " :
            (region.Translates.FirstOrDefault(r =>
                    r.LanguageId == languageId
                    && r.ColumnName == TranslateColumn.full_name.ToString())?.TranslateText ??
                    region.FullName ?? "-") + ((region.Id != RegionIdConst.KARAKALPAKSTAN && region.Id != RegionIdConst.TASHKENT && region.Id != RegionIdConst.TASHKENT_CITY) ? " вилойат" : ""));
        #endregion
        var a = new DocXHandler(wordFile, placeholder);
        a.ReplaceTexts();
        a.ReplaceImages();

        //_storageService.SaveTemp("azolikTest", new StorageFile("TempFile.docx",wordFile));

        var res = _pdfConverter.DocxToPdfAsync(wordFile, new()).Result;
        if (res == null)
        {
            Document document = new Document();
            document.LoadFromStream(wordFile, FileFormat.Docx);
            var pdfStream = new MemoryStream();
            document.SaveToStream(pdfStream, FileFormat.PDF);
            return pdfStream.ToArray();
        }
        CombineStatuses(_pdfConverter);
        return res;
    }
    public byte[] DownloadPdf(MemshipContractDto model)
    {
        var lang = "uz-cyrl";
        var wordFile = _storageService.GetStaticFile(
           StaticFileConst.WordTemplate.GetFileName(
               ServiceProvider.CultureHelper.CurrentCulture.Code,
               StaticFileConst.WordTemplate.MEMSHIP_CONTRACT)
           );

        var lan = UnitOfWork.Context.Set<Language>()
            .FirstOrDefault(l => l.Code == ServiceProvider.CultureHelper.CurrentCulture.Code)?.Id ?? 1;


        if (model.MemshipContractTypeId == MemshipContractTypeIdConst.FREE)
        {
            wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                    lang, StaticFileConst.WordTemplate.MEMSHIP_CONTRACT_FREE));
        }
        var contractor = UnitOfWork.Context.Set<Contractor>()
               .Include(c => c.Oked)
               .Include(c => c.SettlementAccounts)
               .Include(c => c.Bank)
               .Include(c => c.Bank.Translates)
               .FirstOrDefault(c => c.Id == model.ContractorId);

        var placeholder = new Placeholders();
        var link = "FAKE FAKE FAKE FAKE FAKE FAKE FAKE" +
            " FAKE FAKE FAKE FAKE FAKE FAKE FAKE FAKE" +
            " FAKE FAKE FAKE FAKE FAKE FAKE FAKE FAKE" +
            " FAKE FAKE FAKE FAKE FAKE FAKE FAKE FAKE ";

        var qrDownload = new MemoryStream(QRCodeHelper.GeneratePng(link));
        placeholder.ImagePlaceholders.Add("QrDownload", new() { Dpi = 512, MemStream = qrDownload });
        placeholder.TextPlaceholders.Add("QrCode", "");
        placeholder.TextPlaceholders.Add("QrCodeSsp", "");
        var memshipApplication = UnitOfWork.Context.Set<MemshipApplication>()
            .Include(c => c.ChoosedRegion)
            .ThenInclude(c => c.Translates)
            .Include(c => c.Application)
            .ThenInclude(c => c.Region)
            .ThenInclude(c => c.Translates)
            .FirstOrDefault(x => x.ApplicationId == model.ApplicationId);

        if (memshipApplication == null)
        {
            AddError("Ariza topilmadi.");
            return null;
        }

        var ssp = UnitOfWork.Context.Set<Organization>()
            .Include(x => x.SettlementAccounts)
            .ThenInclude(x => x.Bank)
            .Include(x => x.Oked)
            .Where(x => x.OrganizationGroupId == OrganizationGroupIdConst.SSP)
            .FirstOrDefault(a =>
            memshipApplication.ChooseLocation ?
            a.RegionId == memshipApplication.ChoosedRegionId
            : a.RegionId == memshipApplication.Application.RegionId);

        placeholder.TextPlaceholders.Add(nameof(model.DocNumber), model.DocNumber);
        placeholder.TextPlaceholders.Add(nameof(model.DocOn), model.DocOn.ToString(Constants.DATE_FORMAT));
        placeholder.TextPlaceholders.Add(nameof(ssp.Director), ssp.Director);
        placeholder.TextPlaceholders.Add(nameof(ssp.FullName), ssp.FullName);
        placeholder.TextPlaceholders.Add(nameof(model.Details), model.Details);
        placeholder.TextPlaceholders.Add(nameof(model.BaseFixedMinimumValue)
             , model?.BaseFixedMinimumValue.ToString() ?? 0 + " ("
             + model?.BaseFixedMinimumValue.ToString() ?? 0 + ") ");
        placeholder.TextPlaceholders.Add(nameof(ssp.Address), ssp.Address);
        var orgAccount = ssp.SettlementAccounts.FirstOrDefault();
        placeholder.TextPlaceholders.Add(nameof(orgAccount.AccountCode), orgAccount?.AccountCode ?? "-");
        placeholder.TextPlaceholders.Add(nameof(orgAccount.Bank.BankName), orgAccount?.Bank?.BankName ?? "-");
        placeholder.TextPlaceholders.Add(nameof(orgAccount.Bank.Code), orgAccount?.Bank?.Code ?? "-");
        placeholder.TextPlaceholders.Add(nameof(ssp.Inn), ssp.Inn);
        placeholder.TextPlaceholders.Add(nameof(ssp.Oked), ssp.Oked.Code);
        placeholder.TextPlaceholders.Add(nameof(ssp.PhoneNumber), ssp.PhoneNumber);

        placeholder.TextPlaceholders.Add("ContractorFullName", contractor.FullName);
        placeholder.TextPlaceholders.Add("ContractorDirector", contractor.Director);
        placeholder.TextPlaceholders.Add("ContractorAddress", contractor.Address);
        var account = contractor.SettlementAccounts.FirstOrDefault(a => a.IsMain);
        placeholder.TextPlaceholders.Add("ContractorAccountCode", account?.AccountCode ?? "-");

        placeholder.TextPlaceholders.Add("ContractorBankName",
            contractor.Bank?.Translates.FirstOrDefault(c =>
            c.LanguageId == lan
            && c.ColumnName == BankTranslateColumn.bank_name.ToString()
            )?.TranslateText ?? contractor.Bank?.BankName ?? "");

        placeholder.TextPlaceholders.Add("ContractorBankCode", contractor.Bank?.Code ?? "-");
        placeholder.TextPlaceholders.Add("ContractorOked", contractor.Oked?.Code ?? "-");
        placeholder.TextPlaceholders.Add("ContractorPhoneNumber", contractor?.PhoneNumber ?? "-");
        placeholder.TextPlaceholders.Add("ContractorInn", contractor?.Inn ?? "-");
        placeholder.TextPlaceholders.Add(nameof(memshipApplication.Application.Region),
            ((memshipApplication?.ChooseLocation ?? false) ?
            memshipApplication?.ChoosedRegion?.Translates.FirstOrDefault(r =>
                    r.LanguageId == lan
                    && r.ColumnName == TranslateColumn.full_name.ToString())?.TranslateText ??
                    memshipApplication?.ChoosedRegion?.FullName
            : memshipApplication?.Application?.RegionName) ?? "-");

        wordFile = (new DocXHandler(wordFile, placeholder)).ReplaceAll();
        var modelForWord = new
        {
            BaseFixedMinimumValue = model.BaseFixedMinimumValue.ToString() + " ("
          + model.BaseFixedMinimumValue.DecimalInWord(lang) + ") "
        };
        var res = _pdfConverter.DocxToPdfAsync(wordFile, model).Result;
        if (res == null)
        {
            Document document = new Document();
            document.LoadFromStream(wordFile, FileFormat.Docx);
            var pdfStream = new MemoryStream();
            document.SaveToStream(pdfStream, FileFormat.PDF);
            return pdfStream.ToArray();
        }
        //CombineStatuses(_pdfConverter);
        return res;
    }
    public Stream SaveAsExcel(MemshipContractSortFilterOptions options)
    {
        var data = Repository.ReadAsNoTracked<MemshipContractListDto>()
            .SortFilter(options).ToList();

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.MEMSHIP_CONTRACT));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var ws = excelPackage.Workbook.Worksheets[0];

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;
            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value = item.Id;
                ws.Cells[currentRow, column++].Value = item.DocNumber;
                ws.Cells[currentRow, column++].Value = item.DocOn.ToString(Constants.DATE_FORMAT);
                ws.Cells[currentRow, column++].Value = item.Region;
                ws.Cells[currentRow, column++].Value = item.District;
                ws.Cells[currentRow, column++].Value = item.ContractorInn;
                ws.Cells[currentRow, column++].Value = item.Contractor;
                ws.Cells[currentRow, column++].Value = item.Director;
                ws.Cells[currentRow, column++].Value = item.ContractorCategory;
                ws.Cells[currentRow, column++].Value = item.ContractorOked;
                ws.Cells[currentRow, column++].Value = item.MemshipContractType;
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
    #region Private Function
    public HaveId<long> UpdateStatus(UpdateStatusMemshipContractDlDto dto, Action<MemshipContract> validation)
    {
        var isCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        validation += Validation(dto);
        try
        {
            CombineStatuses(Repository);
            if (HasErrors)
                return null;
            var entity = Repository.UpdateStatus(dto, validation);
            CombineStatuses(Repository);
            if (HasErrors)
                return null;
            _unitOfWork.Save();
            var res = CreateDocumentChangeLog(entity.Id, message: dto.Message);
            if (IsValid && isCommit)
                transaction.Commit();
            return res;
        }
        finally
        {
            if (isCommit)
                transaction?.Dispose();
        }
    }
    private Action<MemshipContract> Validation(UpdateStatusMemshipContractDlDto dto)
    {
        return ent =>
        {
            if (!StatusIdConst.CanMemshipContractApplyStatus(ent.StatusId, dto.StatusId))
                AddError("Имкони йўқ / Нет доступа");
        };
    }
    public HaveId<long> CreateDocumentChangeLog(long id, string message = null, string userIp = null, string userAgent = null)
    {
        var entityDto = Repository.ById<MemshipContractDto>(id, applyFilter: false);
        _documentChangeLogService.Create(
            dto: entityDto,
            tableId: TableIdConst.MEMSHIP__DOC_MEMSHIP_CONTRACT,
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
    private void Validation<TDto>(MemshipContractDlDto<TDto> dto, MemshipContract entity)
          where TDto : MemshipContractDlDto<TDto>
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
        var fileInfo = _storageService.Save($"{nameof(TableIdConst.MEMSHIP__DOC_MEMSHIP_CONTRACT)}_SIGN_DATA", docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

        return fileInfo.FirstOrDefault().FileId;
    }

    #endregion

    #region Imports
    public async Task Import(List<ImportMemshipContractDlDto> listDto)
    {
        int step = 0;
        int insertCount = 0;
        string lastInn = "";
        listDto = listDto.Where(a => !string.IsNullOrEmpty(a.ContractorInn)).ToList();

        var contractorIds = _unitOfWork.Context.Set<Contractor>()
            .Where(a => listDto.Select(b => b.ContractorInn).Contains(a.Inn))
            .AsEnumerable()
            .ToDictionary(
                a => a.Inn,
                a => a.Id
            );

        var memshipContracts = _unitOfWork.Context.Set<MemshipContract>()
            .Where(a => listDto.Select(b => b.DocNumber).Contains(a.DocNumber))
            .Select(a => a.DocNumber)
            .ToList();

        try
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                foreach (var dto in listDto.Where(a => !memshipContracts.Contains(a.DocNumber)))
                {
                    step++;
                    lastInn = dto.ContractorInn;
                    if (!contractorIds.ContainsKey(dto.ContractorInn))
                    {
                        var contractor = await _contractorService.GetByInn(dto.ContractorInn);
                        //CombineStatuses(_contractorService);
                        //if (HasErrors)
                        //    return;
                        if (contractor == null)
                            continue;

                        var mc = new AutoMapper.MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<ContractorDto, CreateContractorDlDto>();
                        });
                        var createContractorDlDto = mc.CreateMapper().Map<CreateContractorDlDto>(contractor);
                        createContractorDlDto.Contacts = new List<ContractorContactDlDto>();
                        if (!string.IsNullOrEmpty(dto.ContractorEmail))
                            createContractorDlDto.Contacts.Add(new ContractorContactDlDto { Contact = dto.ContractorEmail, ContactTypeId = ContactTypeIdConst.EMAIL });
                        if (!string.IsNullOrEmpty(dto.ContractorPhoneNumber))
                        {
                            string phoneNumber = Regex.Replace(dto.ContractorPhoneNumber, @"[^\d]", "");
                            if (phoneNumber.Length == 12)
                                createContractorDlDto.Contacts.Add(new ContractorContactDlDto { Contact = phoneNumber, ContactTypeId = ContactTypeIdConst.MOBILE_PHONE });
                        }
                        if (!string.IsNullOrEmpty(dto.ContractorAdditionalPhoneNumber))
                        {
                            string phoneNumber = Regex.Replace(dto.ContractorAdditionalPhoneNumber, @"[^\d]", "");
                            if (phoneNumber.Length == 12)
                                createContractorDlDto.Contacts.Add(new ContractorContactDlDto { Contact = phoneNumber, ContactTypeId = ContactTypeIdConst.ADDITIONAL_PHONE });
                        }

                        var contractorEntity = _contractorService.Create(createContractorDlDto);
                        if (contractorEntity == null)
                        {
                            continue;
                        }

                        contractorIds.Add(dto.ContractorInn, contractorEntity.Id);
                    }
                    else
                    {
                        var contractor = _contractorService.Get(contractorIds[dto.ContractorInn]);
                        if (!contractor.Contacts.Any(a => a.ContactTypeId == ContactTypeIdConst.EMAIL && a.Contact == dto.ContractorEmail))
                            contractor.Contacts.Add(new ContractorContactDto { Contact = dto.ContractorEmail, ContactTypeId = ContactTypeIdConst.EMAIL });

                        string phoneNumber = Regex.Replace(dto.ContractorPhoneNumber, @"[^\d]", "");
                        if (!contractor.Contacts.Any(a => a.ContactTypeId == ContactTypeIdConst.MOBILE_PHONE && a.Contact == phoneNumber))
                            contractor.Contacts.Add(new ContractorContactDto { Contact = phoneNumber, ContactTypeId = ContactTypeIdConst.MOBILE_PHONE });

                        phoneNumber = Regex.Replace(dto.ContractorAdditionalPhoneNumber, @"[^\d]", "");
                        if (!contractor.Contacts.Any(a => a.ContactTypeId == ContactTypeIdConst.ADDITIONAL_PHONE && a.Contact == phoneNumber))
                            contractor.Contacts.Add(new ContractorContactDto { Contact = phoneNumber, ContactTypeId = ContactTypeIdConst.ADDITIONAL_PHONE });

                        var mc = new AutoMapper.MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<ContractorDto, UpdateContractorDlDto>();
                        });
                        var updateContractorDlDto = mc.CreateMapper().Map<UpdateContractorDlDto>(contractor);
                        _contractorService.Update(updateContractorDlDto);
                        //CombineStatuses(_contractorService);
                        //if (HasErrors)
                        //    return;

                        // keremas chunki UPDATE qvomiz
                        // contractorIds.Add(dto.ContractorInn, contractorId);
                    }

                    var createMemshipContractDlDto = new CreateMemshipContractDlDto
                    {
                        //ContractorId = contractorIds[dto.ContractorInn],
                        BaseFixedMinimumValue = 0,
                        MemshipContractTypeId = MemshipContractTypeIdConst.PAID,
                        DocNumber = dto.DocNumber,
                        DocOn = new DateOnly(dto.Year, dto.Month, dto.Day),
                        ApplicationId = null,
                        ContractorSettlementAccountId = null,
                        OrganizationSettlementAccountId = null
                    };
                    var entity = Repository.Create(createMemshipContractDlDto);
                    entity.StatusId = StatusIdConst.ACCEPTED;
                    CombineStatuses(Repository);
                    if (HasErrors)
                        return;

                    insertCount++;
                }

                if (IsValid)
                    transaction.Commit();
            }
        }
        catch (Exception ex)
        {
            AddError($"{ex.Message}: {ex.InnerException.Message}");
        }
    }
    //public async ValueTask<byte[]> GenerateWord()
    //{
    //    //var plh = new Placeholders();
    //    var dto = new MemshipContractDto();
    //    var file = WordFactory.GenerateWordTemplate(dto);
    //    return file;
    //}
    //public async ValueTask<byte[]> Print(long id)
    //{
    //    var dto = Repository.ById<MemshipContractDto>(id);
    //    var file = WordFactory.PrintWordDocument(dto);
    //    var res = await _pdfConverter.DocxToPdfAsync(file, new());
    //    CombineStatuses(_pdfConverter);
    //    return res;
    //}

    #region FOR IMPORT VAQTINCHALIKKA
    public async Task<string> ImportYuridik(List<ImportYuridikMemshipContractDlDto> listDto)
    {
        int step = 0, insertCount = 0;
        string lastInn = "";
        listDto = listDto.Where(a => a.ContractorInn != 0).ToList();

        var contractorIds = _unitOfWork.Context.Set<Contractor>()
            .Where(a => listDto.Select(b => b.ContractorInn.ToString()).Contains(a.Inn))
            .IsActive()
            .AsEnumerable()
            .ToDictionary(
                a => a.Inn,
                a => a.Id
            );

        var memshipContracts = _unitOfWork.Context.Set<MemshipContract>()
            .Where(a => listDto.Select(b => b.DocNumber).Contains(a.DocNumber))
            .Select(a => a.DocNumber)
            .ToList();

        StringBuilder errors = new();
        ///// settlementId
        long settlementId = 0;

        foreach (var dto in listDto)
        {
            try
            {
                if (memshipContracts.Any(a => a == dto.DocNumber))
                    continue;

                step++;
                lastInn = dto.ContractorInn.ToString();
                if (!contractorIds.ContainsKey(dto.ContractorInn.ToString()))
                {
                    var contractor = await _contractorService.GetByInn(dto.ContractorInn.ToString());

                    if (contractor == null)
                        throw new Exception($"INN: {dto.ContractorInn} soliq tizimida yo'q");

                    if (contractor.CountryId == 0 || contractor.RegionId == 0 || contractor.DistrictId == 0)
                    {
                        throw new Exception($"INN: {contractor.Inn} contractorni // " +
                                            $"CId: {contractor.CountryId} // " +
                                            $"RId: {contractor.RegionId} // " +
                                            $"DId: {contractor.DistrictId}");
                    }

                    var mc = new AutoMapper.MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<ContractorDto, CreateContractorDlDto>();
                    });

                    var createContractorDlDto = mc.CreateMapper().Map<CreateContractorDlDto>(contractor);
                    createContractorDlDto.Contacts = new List<ContractorContactDlDto>();

                    ////// settlementAccount
                    var settlementAccount = new List<ContractorSettlementAccountDlDto>();
                    if (contractor.SettlementAccounts.Count() > 0)
                    {
                        foreach (var item in contractor.SettlementAccounts)
                        {
                            settlementAccount.Add(new ContractorSettlementAccountDlDto
                            {
                                AccountCode = item.AccountCode,
                                AccountName = item.AccountName,
                                BankId = item.BankId,
                                StateId = item.StateId,
                                IsMain = item.IsMain
                            });
                        }

                        createContractorDlDto.SettlementAccounts = settlementAccount;
                    }

                    if (!string.IsNullOrEmpty(dto.Email))
                        createContractorDlDto.Contacts.Add(new ContractorContactDlDto { Contact = dto.Email, ContactTypeId = ContactTypeIdConst.EMAIL });

                    if (!string.IsNullOrEmpty(dto.Phone))
                    {
                        string phoneNumber = Regex.Replace(dto.Phone, @"[^\d]", "");
                        if (phoneNumber.Length == 12)
                            createContractorDlDto.Contacts.Add(new ContractorContactDlDto { Contact = phoneNumber, ContactTypeId = ContactTypeIdConst.MOBILE_PHONE });
                    }

                    if (!string.IsNullOrEmpty(dto.Telegram))
                    {
                        string telegram = Regex.Replace(dto.Telegram, @"[^\d]", "");
                        if (telegram.Length == 12)
                            createContractorDlDto.Contacts.Add(new ContractorContactDlDto { Contact = telegram, ContactTypeId = ContactTypeIdConst.TELEGRAM });
                    }

                    var contractorEntity = _contractorService.Create(createContractorDlDto);
                    if (contractorEntity == null)
                        continue;

                    /// settlementId
                    var csa = UnitOfWork.Context
                        .Set<ContractorSettlementAccount>()
                        .FirstOrDefault(csa => csa.OwnerId == contractor.Id);
                    if (csa != null)
                        settlementId = csa.Id;

                    contractorIds.Add(dto.ContractorInn.ToString(), contractorEntity.Id);
                }
                else
                {
                    var contractor = _contractorService.Get(contractorIds[dto.ContractorInn.ToString()]);

                    CombineStatuses(_contractorService);

                    if (HasErrors)
                        errors.AppendLine($"{dto.ContractorInn} // XATO SERVICEDA");

                    if (!string.IsNullOrEmpty(dto.Email))
                    {
                        if (!contractor.Contacts.Any(a => a.ContactTypeId == ContactTypeIdConst.EMAIL && dto.Email != null && a.Contact == dto.Email))
                            contractor.Contacts.Add(new ContractorContactDto { Contact = dto.Email, ContactTypeId = ContactTypeIdConst.EMAIL });
                    }
                    if (!string.IsNullOrEmpty(dto.Phone))
                    {
                        string phoneNumber = Regex.Replace(dto.Phone, @"[^\d]", "");
                        if (!contractor.Contacts.Any(a => a.ContactTypeId == ContactTypeIdConst.MOBILE_PHONE && a.Contact == phoneNumber))
                            contractor.Contacts.Add(new ContractorContactDto { Contact = phoneNumber, ContactTypeId = ContactTypeIdConst.MOBILE_PHONE });
                    }
                    if (!string.IsNullOrEmpty(dto.Telegram))
                    {
                        string telegram = Regex.Replace(dto.Telegram, @"[^\d]", "");
                        if (!contractor.Contacts.Any(a => a.ContactTypeId == ContactTypeIdConst.TELEGRAM && a.Contact == telegram))
                            contractor.Contacts.Add(new ContractorContactDto { Contact = telegram, ContactTypeId = ContactTypeIdConst.TELEGRAM });
                    }

                    var mc = new AutoMapper.MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<ContractorDto, UpdateContractorDlDto>();
                    });
                    var updateContractorDlDto = mc.CreateMapper().Map<UpdateContractorDlDto>(contractor);
                    //updateContractorDlDto.StateId=contractor.st
                    _contractorService.Update(updateContractorDlDto);

                    /// settlementId
                    var csa = UnitOfWork.Context
                        .Set<ContractorSettlementAccount>()
                        .FirstOrDefault(csa => csa.OwnerId == contractor.Id);
                    if (csa != null)
                        settlementId = csa.Id;
                }

                var createMemshipContractDlDto = new CreateMemshipContractDlDto
                {
                    BaseFixedMinimumValue = 0,
                    MemshipContractTypeId = MemshipContractTypeIdConst.PAID,
                    DocNumber = dto.DocNumber,
                    DocOn = new DateOnly(int.Parse(dto.Year), int.Parse(dto.Month), int.Parse(dto.Day)),
                    ApplicationId = null,
                    ContractorSettlementAccountId = settlementId != 0 ? settlementId : null,
                    OrganizationSettlementAccountId = null,
                    OrganizationId = 1,
                };

                var entity = Repository.Create(createMemshipContractDlDto);
                entity.StatusId = StatusIdConst.SIGNED;
                UnitOfWork.Save();
                CombineStatuses(Repository);

                if (HasErrors)
                    errors.AppendLine($"{dto.ContractorInn} // XATO REPOSITORYDA");

                insertCount++;
            }
            catch (DbUpdateException ex)
            {
                errors.AppendLine($"{dto.ContractorInn} // {ex.Message}");
            }
            catch (Exception ex)
            {
                errors.AppendLine($"{dto.ContractorInn} // {ex.Message}");
            }
        }

        return errors.ToString();
    }
    public async Task<string> ImportJismoniy(List<ImportJismoniyMemshipContractDlDto> listDto)
    {
        int insertCount = 0;
        Dictionary<string, long> contractorIds = new();
        listDto = listDto.Where(a => !string.IsNullOrEmpty(a.Pinfl)).ToList();

        contractorIds = _unitOfWork.Context.Set<Contractor>()
            .Where(a => listDto.Select(b => b.Pinfl.ToString())
            .Contains(a.Pinfl)).IsActive().AsEnumerable()
            .ToDictionary(a => a.Pinfl, a => a.Id);

        StringBuilder sb = new();
        foreach (var dto in listDto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    if (!contractorIds.ContainsKey(dto.Pinfl))
                    {
                        var contractor = await _contractorService.GetByPinfl(dto.Pinfl);

                        if (contractor is null)
                        {
                            sb.AppendLine($"Pnfl: {dto.Pinfl} soliq tizimida yo'q");
                            continue;
                        }


                        if (contractor.DistrictId == 0)
                        {
                            sb.AppendLine($"Bu pinflli:{dto.Pinfl} district yuq");
                            continue;
                        }
                        if (contractor.RegionId == 0)
                        {
                            sb.AppendLine($"Bu pinflli:{dto.Pinfl} region yuq");
                            continue;
                        }
                        if (contractor.CountryId == 0)
                        {
                            sb.AppendLine($"Bu pinflli:{dto.Pinfl} country yuq");
                            continue;
                        }

                        var mc = new AutoMapper.MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<ContractorDto, CreateContractorDlDto>();
                        });

                        var createContractorDlDto = mc.CreateMapper().Map<CreateContractorDlDto>(contractor);
                        createContractorDlDto.Contacts = new List<ContractorContactDlDto>();

                        if (!string.IsNullOrEmpty(dto.Email))
                            createContractorDlDto.Contacts.Add(new ContractorContactDlDto { Contact = dto.Email, ContactTypeId = ContactTypeIdConst.EMAIL });

                        if (!string.IsNullOrEmpty(dto.PhoneNumber))
                        {
                            string phoneNumber = Regex.Replace(dto.PhoneNumber, @"[^\d]", "");
                            if (phoneNumber.Length == 12)
                                createContractorDlDto.Contacts.Add(new ContractorContactDlDto { Contact = phoneNumber, ContactTypeId = ContactTypeIdConst.MOBILE_PHONE });
                        }

                        if (!string.IsNullOrEmpty(dto.Telegram))
                        {
                            string telegram = Regex.Replace(dto.Telegram, @"[^\d]", "");
                            if (telegram.Length == 12)
                                createContractorDlDto.Contacts.Add(new ContractorContactDlDto { Contact = telegram, ContactTypeId = ContactTypeIdConst.TELEGRAM });
                        }

                        var contractorEntity = _contractorService.Create(createContractorDlDto);
                        if (contractorEntity is null)
                            continue;
                        contractorIds.Add(dto.Pinfl, contractorEntity.Id);

                        var createMemshipContractDlDto = new CreateMemshipContractDlDto
                        {
                            BaseFixedMinimumValue = 0,
                            MemshipContractTypeId = MemshipContractTypeIdConst.FREE,
                            DocNumber = dto.DocNumber,
                            DocOn = new DateOnly(int.Parse(dto.Year), int.Parse(dto.Month), int.Parse(dto.Day)),
                            ApplicationId = null,
                            ContractorSettlementAccountId = null,
                            OrganizationSettlementAccountId = null,
                            OrganizationId = 1
                        };

                        var entity = Repository.Create(createMemshipContractDlDto);
                        entity.StatusId = StatusIdConst.SIGNED;
                        _unitOfWork.Context.SaveChanges();

                        CombineStatuses(Repository);
                        if (HasErrors)
                            return null;

                        if (IsValid)
                        {
                            transaction.Commit();
                            insertCount++;
                        }
                        else
                            transaction.Rollback();
                    }
                }
                catch (DbUpdateException ex)
                {
                    sb.AppendLine($"{dto.Pinfl}\n\nEX:{ex.Message}\n\nInnerEX:{ex.InnerException.Message}");
                    transaction.Rollback();
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"{dto.Pinfl}\n\nEX:{ex.Message}\n\nInnerEX:{ex.InnerException.Message}");
                    transaction.Rollback();
                }
            }
        }

        return $"Errors: {sb}\n\n\nInsert Count: {insertCount}";
    }
    #endregion
    public async ValueTask ChangeContractorToPaid(ChangeContractToPayedDto dTo)
    {
        var entity = _unitOfWork.Context.Set<MemshipContract>().FirstOrDefault(a => a.Id == dTo.Id);

        if (entity == null)
        {
            AddError("Malumot topilmadi");
        }

        if (entity.MemshipContractTypeId == MemshipContractTypeIdConst.FREE)
        {
            await UpdateSignRequestStateAsync(entity);
            UpdateSecretKeyAndRequestId(entity.Id);

            entity.BaseFixedMinimumValue = dTo.BaseFixedMinimumValue;
            entity.OrganizationSettlementAccountId = dTo.OrganizationSettlementAccountId;
            entity.RegionalOrganizationId = dTo.RegionalOrganizationId;
            entity.OrganizationId = OrganizationIdConst.SSP;
            entity.MemshipContractTypeId = MemshipContractTypeIdConst.PAID;
            entity.DocNumber = dTo.DocNumber;
            entity.Details = dTo.Details;

            _unitOfWork.Save();

            await PostToIMZOAndSentUrl(entity);
            var res = CreateDocumentChangeLog(entity.Id, "Tekin shartnomani pullika o'zgartirish", _authService.User.FullName);

        }
        else
        {
            AddError("Pullik shartnoma olgan tadbirkorlik yana pullik qilish imkoni yoq");
        }
    }
    public void UpdateSecretKeyAndRequestId(long contractId)
    {
        var dto = Repository.ById(contractId);
        dto.WebImzoRequestId = null;
        dto.WebImzoSecretKey = null;
        _unitOfWork.Save();
    }
    public void ChangeContractorDocnumber(ChangeContractParametirsDto dTo)
    {
        var entity = _unitOfWork.Context.Set<MemshipContract>().FirstOrDefault(a => a.Id == dTo.Id);

        if (entity == null)
        {
            AddError("Malumot topilmadi");
        }

        if (entity.MemshipContractTypeId == MemshipContractTypeIdConst.PAID)
        {
            var res = CreateDocumentChangeLog(entity.Id, $"Shartnomani o'zgartirish - eskisi - {entity.DocNumber} yangisi - {dTo.DocNumber} - eskisi -{entity.DocOn} yangisi-{dTo.DocOn}  eskisi-{entity.Details}  yangisi-{dTo.Details}", _authService.User.FullName);

            entity.DocNumber = dTo.DocNumber;
            entity.DocOn = dTo.DocOn;
            entity.Details = dTo.Details;

            _unitOfWork.Save();
        }
        else
        {
            AddError("Tekin Shartnomalarni hujjat raqami va sanasini ozgartira olmaysiz");
        }
    }
    #endregion
    public void Update(UpdatingBirdMemshipContractDlDto dto)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.ById(dto.Id);
                if (entity == null)
                    AddError("Not Found");

                entity.Details = dto.Detail;
                UnitOfWork.Save();
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
    #endregion
    #region WebImzo
    public async ValueTask<string?> PostToIMZOAndSentUrl(MemshipContract contract)
    {
        if (contract.WebImzoSecretKey != null)
            await SendUrl(contract.Id);

        var canDispose = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        WbImzoCreateSignRequestDto signRequestCreateDto = CreatDtoForRequestSign(contract);
        List<Contractor> contractor = new();


        if (contract.MemshipContractTypeId == MemshipContractTypeIdConst.PAID)
        {

            var rais = _unitOfWork.Context.Set<EmployeeManage>().Include(a => a.Employee).ThenInclude(a => a.Person)
                                                                                                   .FirstOrDefault(a => a.PositionId == SignerPositionId.Chairman);
            signRequestCreateDto.SignRequestUsers.Add(
                                            new WbImzoCreateSignRequestUserDto
                                            {
                                                UserKey = !string.IsNullOrEmpty(contract.Contractor.Inn) ? contract.Contractor.Inn : contract.Contractor.Pinfl,
                                                UserInfo = contract.Contractor.FullName,
                                                UserId = (int)contract.ContractorId,
                                                DocStatusId = StatusIdConst.SIGNING,
                                                SignPriority = 1,
                                                IpAddress = _authService.UserIp,
                                                UserAgent = _authService.UserAgent,
                                                UserPhoneNumber = contract.Contractor.PhoneNumber,
                                            }
                                        );

            signRequestCreateDto.SignRequestUsers.Add(
                                new WbImzoCreateSignRequestUserDto
                                {
                                    UserKey = rais.Employee.Person.Pinfl,
                                    UserInfo = rais.Employee.Person.FullName,
                                    UserId = (int)rais.Employee.PersonId,
                                    DocStatusId = StatusIdConst.SIGNED,
                                    SignPriority = 2,
                                    IpAddress = _authService.UserIp,
                                    UserAgent = _authService.UserAgent,
                                    UserPhoneNumber = rais.Employee.PhoneNumber,
                                }
                            );
        }
        else
        {
            int signPriority = 1;

            signRequestCreateDto.SignRequestUsers.Add(
                                            new WbImzoCreateSignRequestUserDto
                                            {
                                                UserKey = !string.IsNullOrEmpty(contract.Contractor.Inn) ? contract.Contractor.Inn : contract.Contractor.Pinfl,
                                                UserInfo = contract.Contractor.FullName,
                                                UserId = (int)contract.ContractorId,
                                                DocStatusId = StatusIdConst.SENT_FOR_REVIEW,
                                                SignPriority = signPriority,
                                                IpAddress = _authService.UserIp,
                                                UserAgent = _authService.UserAgent,
                                                UserPhoneNumber = contract.Contractor.PhoneNumber,
                                            }
                                        );

            var debutyHeadOfDepartment = _unitOfWork.Context.Set<EmployeeManage>().Include(a => a.Employee).ThenInclude(a => a.Person).Include(a => a.Department)
                                                     .Where(a => a.OrganizationId == contract.OrganizationId && a.Department.OrganizationId == contract.OrganizationId)
                                                     .FirstOrDefault(a => a.PositionId == SignerPositionId.DebutyHeadOfDepartment);

            signPriority = 2;
            if (debutyHeadOfDepartment != null)
            {
                signRequestCreateDto.SignRequestUsers.Add(
                                        new WbImzoCreateSignRequestUserDto
                                        {
                                            UserKey = debutyHeadOfDepartment.Employee.Person.Inn ?? debutyHeadOfDepartment.Employee.Person.Pinfl,
                                            UserInfo = debutyHeadOfDepartment.Employee.Person.FullName,
                                            UserId = (int)debutyHeadOfDepartment.Employee.PersonId,
                                            DocStatusId = StatusIdConst.SIGNED,
                                            SignPriority = signPriority,
                                            IpAddress = _authService.UserIp,
                                            UserAgent = _authService.UserAgent,
                                            UserPhoneNumber = debutyHeadOfDepartment.Employee.PhoneNumber,
                                        }
                                    );
            }

            var debutyHeadOfDepartment1 = _unitOfWork.Context.Set<EmployeeManage>().Include(a => a.Employee).ThenInclude(a => a.Person).Include(a => a.Department).Include(a => a.Position)
                                                                 .Where(a => a.OrganizationId == contract.OrganizationId && a.Department.OrganizationId == contract.OrganizationId)
                                                                 .FirstOrDefault(a => a.PositionId == SignerPositionId.DebutyHeadOfDepartment1);
            if (debutyHeadOfDepartment1 != null)
            {
                signRequestCreateDto.SignRequestUsers.Add(
                                        new WbImzoCreateSignRequestUserDto
                                        {
                                            UserKey = debutyHeadOfDepartment1.Employee.Person.Inn ?? debutyHeadOfDepartment1.Employee.Person.Pinfl,
                                            UserInfo = debutyHeadOfDepartment1.Employee.Person.FullName,
                                            UserId = (int)debutyHeadOfDepartment1.Employee.PersonId,
                                            DocStatusId = StatusIdConst.SIGNED,
                                            SignPriority = signPriority,
                                            IpAddress = _authService.UserIp,
                                            UserAgent = _authService.UserAgent,
                                            UserPhoneNumber = debutyHeadOfDepartment1.Employee.PhoneNumber,
                                        }
                                    );
            }


            var departmentdHead = _unitOfWork.Context.Set<EmployeeManage>().Include(a => a.Employee).ThenInclude(a => a.Person).Include(a => a.Department).Include(a => a.Position)
                                                                                         .Where(a => a.OrganizationId == contract.OrganizationId && a.Department.OrganizationId == contract.OrganizationId)
                                                                                         .FirstOrDefault(a => a.PositionId == SignerPositionId.DepartmentHead);
            if (departmentdHead != null)
            {

                signRequestCreateDto.SignRequestUsers.Add(
                                                        new WbImzoCreateSignRequestUserDto
                                                        {
                                                            UserKey = departmentdHead.Employee.Person.Inn ?? departmentdHead.Employee.Person.Pinfl,
                                                            UserInfo = departmentdHead.Employee.Person.FullName,
                                                            UserId = (int)departmentdHead.Employee.PersonId,
                                                            DocStatusId = StatusIdConst.SIGNED,
                                                            SignPriority = signPriority,
                                                            IpAddress = _authService.UserIp,
                                                            UserAgent = _authService.UserAgent,
                                                            UserPhoneNumber = departmentdHead.Employee.PhoneNumber,
                                                        }
                                                    );
            }
        }

        var wbImzoResult = await _wbImzoService.UpsertStringFilePrintableLinkRequestAsync(signRequestCreateDto);
        if (!wbImzoResult.IsSuccess && wbImzoResult.Response is null)
            CombineStatuses(wbImzoResult.GetStatusGeneric());

        if (HasErrors || wbImzoResult.Response is null)
        {
            AddError("Contract imzolash uchun yuborilayotgan jarayonda xatolik yuz berdi");
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

    private async ValueTask<string?> SendUrl(long contractId)
    {

        var canDispose = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        var contract = _unitOfWork.Context.Set<MemshipContract>().FirstOrDefault(a => a.Id == contractId);

        if (contract.WebImzoSecretKey.IsNullOrEmpty())
        {
            await PostToIMZOAndSentUrl(contract);
        }

        var url = (_wbImzoConfig.Api.Contains("api/") ? _wbImzoConfig.Api.Replace("api/", null) : _wbImzoConfig.Api) + $"app?requestId={contract.WebImzoRequestId}&secretKey={contract.WebImzoSecretKey}";

        return url;

    }
    public WbImzoCreateSignRequestDto CreatDtoForRequestSign(MemshipContract contract)
    {
        var organization = _unitOfWork.OrganizationRepository.AllAsQueryable.FirstOrDefault(a => a.Id == contract.OrganizationId);

        if (contract == null)
        {
            AddError("Organization topilmadi!");
        }

        string documentDataAsString = JsonConvert.SerializeObject(contract, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        var filePrintableLink = $"{_linkConfig.Api}{_linkConfig.DownloadRoute}?id2={contract.Id2}&lang=uz-cyrl&__lang=uz-cyrl&langId=2";

        return new WbImzoCreateSignRequestDto
        {
            DocumentId = contract.Id,
            DocumentType = "A'zolik shartnomasi",
            ApiKey = _wbImzoConfig.ApiKey,
            Title = contract.DocNumber,
            IsForceCreate = true,
            TableId = TableIdConst.MEMSHIP__DOC_MEMSHIP_CONTRACT,
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
    #endregion
}