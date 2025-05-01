using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Spire.Doc.Documents.Rendering;
using Ssp.DataLayer.EFClasses.Edoc;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.DualApplicationServices;
using SspUis.BizLogicLayer.DualContractServices;
using SspUis.BizLogicLayer.DualEdu;
using SspUis.BizLogicLayer.Hrm;
using SspUis.BizLogicLayer.Srv;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Extensions;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.Billing.Services;
using SspUis.Integration.DocxToPdf;
using SspUis.Integration.Dual.Models;
using SspUis.Integration.Dual.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WbImzo.Models;
using WbImzo.Proxy.Sdk;
using WEBASE;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.OfficeTools;
using WEBASE.OfficeTools.Factory;
using WEBASE.OfficeTools.Handlers;
using WEBASE.QRCode;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public class DualContractService : BaseEntityService<long, DualContract, DualContractListDto, DualContractDto, CreateDualContractDlDto, UpdateDualContractDlDto,
        IDualContractRepository, DualContractDtoSortFilterOptions>
        , IDualContractService
{
    private readonly IBillingService _billingService;
    private readonly IDualContractRepository _repository;
    private readonly IDocumentChangeLogService _documentChangeLogService;
    private readonly IEImzoService _eImzoService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SystemConf _systemConf;
    private readonly IWbImzoService _wbImzoService;
    private readonly WbImzoConfig _wbImzoConfig;
    private readonly LinkConfig _linkConfig;
    private readonly IAuthService _authService;
    private readonly IStorageService _storageService;
    private readonly IDualService _integrationDualService;
    private readonly IConvertService _pdfConverter;
    public DualContractService(IUnitOfWork unitOfWork, 
                               IAuthService authService, 
                               IBillingService service, 
                               IDualContractRepository repository, 
                               IDocumentChangeLogService documentChangeLog,
                               IEImzoService eImzoService,
                               SystemConf systemConf,
                               IStorageService storageService,
                               IDualService integrationDualService,
                               WbImzoConfig wbImzoConfig,
                               List<LinkConfig> linkConfigs,
                               IWbImzoService wbImzoService,
                               IConvertService pdfConverter) 
        : base(unitOfWork)
    {
        _billingService = service;
        _unitOfWork = unitOfWork;
        _authService = authService;
        _repository = repository;
        _documentChangeLogService = documentChangeLog;
        _eImzoService = eImzoService;
        _storageService = storageService;
        _integrationDualService = integrationDualService;
        this._pdfConverter = pdfConverter;
        _systemConf = systemConf;
        _wbImzoService = wbImzoService;
        _wbImzoConfig = wbImzoConfig;
        _linkConfig = linkConfigs.FirstOrDefault(config => config.Name == "DualContract");
    }

    public void Delete(long id)
    {
        throw new NotImplementedException();
    }
    public override HaveId<long> Create(CreateDualContractDlDto dto)
    {
        bool isCommit = UnitOfWork.CurrentTransaction == null;
        using var transaction = isCommit ? UnitOfWork.BeginTransaction() : UnitOfWork.CurrentTransaction;
        try
        {
            var speciality = _unitOfWork.SpecialtyBillingRepository.AllAsQueryable
                .Where(x => x.Id == dto.SpecialityId)
                .FirstOrDefault();
            if (speciality == null)
            {
                AddError("erp tizimida bunday yo'nalish mavjud emas adminga murojat qiling!");
                transaction.Rollback();
            }
            dto.SpecialityId = speciality.Id;
            var entity = base.Repository.Create(dto, ent => Validation(dto, ent));
            CombineStatuses(Repository);
            if (HasErrors)
                return null;

            if (IsValid)
                UnitOfWork.Save();

            if (IsValid && isCommit)
                transaction.Commit();

            return HaveId.Create(entity.Id);
        }
        catch (DbUpdateException e)
        {
            AddError($"{e.Message} InnerException: {e.InnerException}");
            transaction.Rollback();
        }
        finally
        {
            transaction.Dispose();
        }

        return null;
    }
    public async Task<byte[]> DownloadPdf(Guid id2)
    {
        var result = await _billingService.DownloadContract(id2);
        return result;
    }
    public override PagedResult<DualContractListDto> GetList(DualContractDtoSortFilterOptions options)
    {
        var data = Repository.ReadAsNoTracked<DualContractListDto>()
            .SortFilter(options)
            .AsPagedResult(options);

        return data;
    }
    public int GetCount()
    {
		DualContractDtoSortFilterOptions options = new DualContractDtoSortFilterOptions();
        var data = Repository.ReadAsNoTracked<DualContractListDto>().SortFilter(options).Count();
        return data;

	}
    public override DualContractDto Get(long id)
    {
        var dto = Repository.ById<DualContractDto>(id, applyFilter: false);
        CombineStatuses(Repository);
        if (dto == null || HasErrors)
        {
            AddError("Not found");
            return null;
        }
        if (_authService.Contractor != null)
        {
            dto.CanReject = false; /*StatusIdConst.CanDualContractApplyStatus(dto.StatusId, StatusIdConst.REJECTED);*/
            dto.CanSign = StatusIdConst.CanDualContractApplyStatus(dto.StatusId, StatusIdConst.SIGNED);
        }
        if (HasErrors)
        { AddError("xato !"); return null; }

        return dto;
    }
    private HaveId<long> UpdateStatus(UpdateStatusDualContractDlDto dto, Action<ServiceContract> validation)
    {
        var canCommit = UnitOfWork.CurrentTransaction == null;
        var transaction = UnitOfWork.CurrentTransaction ?? UnitOfWork.BeginTransaction();

        try
        {
            var entity = _repository.UpdateStatus(dto);
            CombineStatuses(_repository);
            if (HasErrors)
                return null;

            _unitOfWork.Save();

            var res = CreateDocumentChangeLog(entity.Id, dto.StatusId);
            if (IsValid && canCommit)
                transaction.Commit();
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
            transaction.Rollback();
        }
        finally
        {
            if (canCommit)
                transaction.Dispose();
        }
        return null;
    }
    public async Task Sign(UpdateDualContract dto)
    {
        try
        {
            var doc = await _unitOfWork.Context.Set<DualContract>()
                            .Include(a => a.Application)
                            .FirstOrDefaultAsync(a => a.Id == dto.Id);

            if (doc == null)
            {
                AddError("Record not found");
                return;
            }
            else if (doc.Application.ContractorId != _authService.Contractor.Id)
            {
                AddError("Access denied");
                return;
            }

            var timeStamp = await _eImzoService.TimeStamp(new EImzoTimeStampDto
            {
                SignData = dto.SignedData,
                Inn = _authService.Contractor.Inn,
                Pinfl = _authService.Contractor.Pinfl
            });

            CombineStatuses(_eImzoService);
            if (HasErrors) return;

            var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
            {
                SignData = timeStamp.Pkcs7b64,
                Inn = null,
                Pinfl = _authService.Contractor.Pinfl
            });

            CombineStatuses(_eImzoService);
            if (HasErrors) return;

            dto.SignFile = SaveFile(doc.Id, timeStamp.Pkcs7b64, "sign.txt");
            dto.DataFile = SaveFile(doc.Id, JsonConvert.SerializeObject(doc), "data.txt");
            dto.SignedUserInfo = _authService.User.ToTextForDocumentLog();

            UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanDualContractApplyStatus(ent.StatusId, StatusIdConst.SIGNED))
                    AddError("No permission");
            });

            await _unitOfWork.Context.SaveChangesAsync();

            if (IsValid)
            {
                if (doc.ExternalId != null)
                { await _billingService.HeldByContractor((int)doc.ExternalId); }
            }
        }
        catch (DbUpdateException ex)
        {
            AddError(ex.Message);
        }
    }
    public async void Reject(RejectStatusDualContractDto dto)
    {
        var canCommit = UnitOfWork.CurrentTransaction == null;
        var transaction = UnitOfWork.CurrentTransaction ?? UnitOfWork.BeginTransaction();

        var doc = _unitOfWork.Context.Set<DualContract>().Include(a => a.Application).FirstOrDefault(a => a.Id == dto.Id);
        if (doc == null)
        {
            AddError("По вашему запросу запись не найдено");
            return;
        }
        else if (doc.Application.ContractorId != _authService.Contractor.Id)
        {
            AddError("Ruxsat yo'q / Нет доступа");
            return;
        }

        var eImzoTimstampDto = new EImzoTimeStampDto
        {
            SignData = dto.SignedData,
            Inn = _authService.User.Inn,
            Pinfl = _authService.User.Pinfl
        };

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
        dto.SignedUserInfo = _authService.User.ToTextForDocumentLog();

        try
        {
            var result = _integrationDualService.RejectDualContract(new DualContractRejectDto()
            {
                Id = dto.Id,
                Message = "Shartnomani bekor qilish"
            });

            var res = UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanDualContractApplyStatus(ent.StatusId, StatusIdConst.REJECTED))
                    AddError("Имкони йўқ / Нет доступа");
            });

            UnitOfWork.Save();
            CombineStatuses(Repository);
            if (IsValid && canCommit)
                transaction.Commit();
        }
        catch (DbUpdateException ex)
        {
            AddError(ex.Message);
            transaction.Rollback();
        }
        finally
        {
            if (canCommit)
                transaction.Dispose();
        }
    }
    public Guid SaveFile(long docId, string data, string fileName)
    {
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        
        writer.Write(data);
        writer.Flush();
        
        ms.Position = 0;
        var fileInfo = _storageService.Save($"{nameof(TableIdConst.DOC_DUAL_CONTRACT)}_SIGN_DATA", docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

        return fileInfo.FirstOrDefault().FileId;
    }
    public async Task<byte[]> DownloadPdf(Guid id2, string langu)
    {
        var language = langu ?? "uz-latn";

        var wordFile = _storageService.GetStaticFile(StaticFileConst.WordTemplate.GetFileName(language, StaticFileConst.WordTemplate.DUAL_CONTRACT));
        try
        {
            var dto = Repository.ReadAsNoTracked<DualContractDto>(applyFilter: false)
                .FirstOrDefault(x => x.Id2 == id2);

            if (dto == null)
            {
                AddError("Hujjat topilmadi!");
                return null;
            }

            var plh = WordFactory.MakePlaceholders(dto);
            var handler = new DocXHandler(wordFile, plh);
            handler.ReplaceLists();
            handler.ReplaceTexts();
            handler.ReplaceImages();
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
    public HaveId<long> CreateDocumentChangeLog(long id, int statusId)
    {
        var entityDto = _repository.ById<DualContractDto>(id, applyFilter: false);
        _documentChangeLogService.Create(
            dto: entityDto,
            tableId: TableIdConst.DOC_DUAL_CONTRACT,
            organizationId: OrganizationIdConst.SSP,
            statusId: statusId,
            message: _authService.Contractor.FullName);
        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }
    private void Validation<TDto>(DualContractDlDto<TDto> dto, DualContract entity)
           where TDto : DualContractDlDto<TDto>
    {
        var query = _repository.AllAsQueryable;

        if (entity != null)
        {
            query = query.Where(a => a.Id != entity.Id);

            //if (!StatusIdConst.CanModifyStatus.Contains(entity.StatusId))
            //    AddError("Таҳрирлаш мумкин эмас / Невозможно редактировать");
        }
    }
    public bool GetStatus(string pinfl)
    {
        var dto = _repository.Context
                       .Set<DualContract>()
                       .Where(x => x.Pinfl == pinfl && x.StatusId == StatusIdConst.SIGNED)
                       .FirstOrDefault();

        if (dto == null || HasErrors)
        {
            AddError("Imzolanmagan yoki topilmadi!");
            return false;
        }

        return true;
    }

    public async ValueTask<string> WebImzoSign(WebImzoSignedFilter filter)
    {
        var doc = _unitOfWork.Context.Set<DualContract>()
                            .Include(x => x.Application).ThenInclude(x => x.Contractor)
                            .FirstOrDefault(x => x.Id == filter.Id && x.StatusId != StatusIdConst.DELETED);

        if (doc == null)
        {
            AddError("По вашему запросу запись не найдено");
            return null;
        }

        return await PostToIMZOAndSentUrl(doc);
    }

    public async ValueTask<string> PostToIMZOAndSentUrl(DualContract contract)
    {
        var canDispose = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        WbImzoCreateSignRequestDto signRequestCreateDto = CreatDtoForRequestSign(contract);
        signRequestCreateDto.SignRequestUsers = new List<WbImzoCreateSignRequestUserDto>
                                                    {
                                                        new WbImzoCreateSignRequestUserDto
                                                        {
                                                            UserKey = !string.IsNullOrEmpty(contract.Application.Contractor.Inn) ? contract.Application.Contractor.Inn : contract.Application.Contractor.Pinfl,
                                                            UserInfo = _authService.User.ToTextForDocumentLog(),
                                                            UserId = (int)contract.Application.ContractorId,
                                                            DocStatusId = StatusIdConst.SIGNED,
                                                            SignPriority = 1,
                                                            IpAddress = _authService.UserIp,
                                                            UserAgent = _authService.UserAgent,
                                                            UserPhoneNumber = contract.Application.Contractor.PhoneNumber
                                                        }
                                                    };

        var wbImzoResult = await _wbImzoService.UpsertStringFilePrintableLinkRequestAsync(signRequestCreateDto);
        if (!wbImzoResult.IsSuccess && wbImzoResult.Response is null)
            CombineStatuses(wbImzoResult.GetStatusGeneric());

        if (HasErrors || wbImzoResult.Response is null)
        {
            AddError("Dual shartnoma imzolash uchun yuborilayotgan jarayonda xatolik yuz berdi");
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

        var contract = _unitOfWork.Context.Set<DualContract>().FirstOrDefault(a => a.Id == contractId);

        if (contract.WebImzoRequestId == null || contract.WebImzoSecretKey.IsNullOrEmpty())
        {
            AddError("Dual Contract imzolash uchun yuborilayotgan jarayonda qaytgan keylani saqlashda xatolik yuz berdi");
            return null;
        }

        var url = (_wbImzoConfig.Api.Contains("api/") ? _wbImzoConfig.Api.Replace("api/", null) : _wbImzoConfig.Api) + $"app?requestId={contract.WebImzoRequestId}&secretKey={contract.WebImzoSecretKey}";

        return url;

    }

    public WbImzoCreateSignRequestDto CreatDtoForRequestSign(DualContract contract)
    {
        //var organization = _unitOfWork.OrganizationRepository.AllAsQueryable.FirstOrDefault(a => a.Id == contract.o);

        //if (contract == null)
        //{
        //    AddError("tashkilot topilmadi!");
        //}
        string documentDataAsString = JsonConvert.SerializeObject(ConvertToDto(contract));
        var filePrintableLink = $"{_linkConfig.Api}{_linkConfig.DownloadRoute}?id2={contract.Id2}&lang=uz-cyrl&__lang=uz-cyrl&langId=2";

        return new WbImzoCreateSignRequestDto
        {
            DocumentId = contract.Id,
            ApiKey = _wbImzoConfig.ApiKey,
            Title = contract.DocNumber,
            IsForceCreate = true,
            TableId = TableIdConst.DOC_DUAL_CONTRACT,
            SignData = documentDataAsString,
            //OrganizationInn = organization != null ? organization.Inn : null,
            //OrganizationName = organization != null ? organization.FullName : null,
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
    public WebImzoDto ConvertToDto(DualContract dto) 
    => new()
    {
        DocOn = dto.DocDate,
        StatusId = dto.StatusId,
        DocNumber = dto.DocNumber,
        Id = dto.Id,
    };
}