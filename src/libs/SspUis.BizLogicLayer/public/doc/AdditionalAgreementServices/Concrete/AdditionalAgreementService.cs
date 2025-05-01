using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Spire.Doc;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.DocxToPdf;
using SspUis.ServiceLayer.NumberServices;
using WEBASE;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.OfficeTools.Factory;
using WEBASE.OfficeTools.Handlers;
using WEBASE.QRCode;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Hrm.AdditionalAgreementService;

public class AdditionalAgreementService
    : BaseEntityService<long, AdditionalAgreement, AdditionalAgreementListDto, AdditionalAgreementDto, CreateAdditionalAgreementDlDto, UpdateAdditionalAgreementDlDto, IAdditionalAgreementRepository>
    , IAdditionalAgreementService
{
    private readonly IAuthService _authService;
    private readonly INumberService _numberService;
    private readonly IStorageService _storageService;
    private readonly IEImzoService _eImzoService;
    private readonly IConvertService _pdfConverter;
    private readonly SystemConf _systemConf;
    private readonly IDocumentPrintService _printService;

    #region ctor
    public AdditionalAgreementService(
        IAuthService authService,
        IUnitOfWork unitOfWork,
        IEImzoService eImzoService,
        IStorageService storageService,
        INumberService numberService,
        IConvertService pdfConverter,
        IDocumentPrintService printService,
        SystemConf systemConf)
        : base(unitOfWork)
    {
        _authService = authService;
        _eImzoService = eImzoService;
        _storageService = storageService;
        _numberService = numberService;
        _pdfConverter = pdfConverter;
        _printService = printService;
        _systemConf = systemConf;
    }
    #endregion

    public SelectList<long> AsSelectList()
    {
        return Repository.AllAsQueryable.AsSelectList();
    }

    public PagedResult<AdditionalAgreementListDto> GetList(AdditionalAgreementSortFilterOption dto)
    {
        var result = Repository.ReadAsNoTracked<AdditionalAgreementListDto>().SortFilter(dto).AsPagedResult(dto);
        return result;
    }

    public override AdditionalAgreementDto Get()
    {
        return new AdditionalAgreementDto()
        {
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.ADDITIONAL_AGREEMENT).Item2,
            DocOn = DateOnly.FromDateTime(DateTime.Now)
        };
    }

    public MemshipAdditionalAgreementDto GetByMemshipContractId(long memshipContractId)
    {
        var contract = UnitOfWork.MemshipContractRepository.ById(memshipContractId);
        if (contract == null)
        {
            AddError("Azolik shartnomasi topilmadi :( ");
            return null;
        }
        return new MemshipAdditionalAgreementDto()
        {
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.ADDITIONAL_AGREEMENT).Item2,
            DocOn = DateOnly.FromDateTime(DateTime.Now),
            MemshipContractId = memshipContractId,
            ApplicationTypeId = ApplicationTypeIdConst.MEMSHIP,
            MemshipContractDocNumber = contract.DocNumber,
            MemshipContractDocOn = contract.DocOn
        };
    }

    public override AdditionalAgreementDto Get(long id)
    {
        var dto = Repository.ById<AdditionalAgreementDto>(id);
        CombineStatuses(Repository);
        return dto;
    }

    public async Task Sign(SignStatusAdditionalAgreementDto dto)
    {
        var doc = Repository.Context.Set<AdditionalAgreement>()
          .Include(x => x.Signs)
          .FirstOrDefault(x => x.Id == dto.Id);

        if (doc == null)
        {
            AddError("По вашему запросу запись не найдено");
            return;
        }

        if (_authService.Contractor == null)
            dto.StatusId = StatusIdConst.SIGNING;
        else
        {
            if (doc.StatusId != StatusIdConst.SIGNING)
            {
                AddError("Bu shartnoma Palata tomonidan imzolanmagan. :( ");
                return;
            }

            #region Bu HRM tayyor bo'ganda commentdan ochamiz o'chirmanglar
            var signer = UnitOfWork.Context.Set<SignCriterion>()
              .Include(x => x.Position)
              .Where(x => x.ApplicationTypeId == ApplicationTypeIdConst.MEMSHIP);
            if (signer == null)
            {
                AddError("Imzo chekuvchi belgilanmagan. Bu hujjat imzolash uchun lavozim belgilashni talab qiladi.");
                return;
            }

            if (signer.Any(x => x.PositionId == _authService.User.PositionId))
            {
                AddError("Ushbu hujjatni imzolash uchun lavozim belgilangan lavozimga to'g'ri kelmaydi.");
                return;
            }
            #endregion

            dto.StatusId = StatusIdConst.SIGNED;
        }

        var eImzoTimstampDto = new EImzoTimeStampDto
        {
            SignData = dto.SignedData,
            Inn = dto.IsPinfl ? null : _authService.Contractor?.Inn,
            Pinfl = dto.IsPinfl ? _authService.User.Pinfl : null
        };
        var timeStamp = await _eImzoService.TimeStamp(eImzoTimstampDto);

        CombineStatuses(_eImzoService);
        if (HasErrors)
            return;

        if (_authService.Contractor != null)
        {
            if (doc.ContractorId != _authService.Contractor.Id)
            {
                AddError("Нет доступа");
                return;
            }
            var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
            {
                SignData = timeStamp.Pkcs7b64,
                Inn = dto.IsPinfl ? null : _authService.Contractor.Inn,
                Pinfl = _authService.Contractor.Pinfl
            });
        }
        else
        {
            var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
            {
                SignData = timeStamp.Pkcs7b64,
                Inn = null,
                Pinfl = _authService.User.Pinfl
            });
        }

        CombineStatuses(_eImzoService);
        if (HasErrors)
            return;

        dto.SignFile = SaveFile(doc.Id, timeStamp.Pkcs7b64, "sign.txt");
        dto.DataFile = SaveFile(doc.Id, JsonConvert.SerializeObject(doc), "data.txt");
        dto.SignedUserInfo = _authService.UserName + " - " + _authService.User.FullName;

        using var transaction = UnitOfWork.BeginTransaction();
        try
        {
            Repository.UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanAdditionalAgreementApplyStatus(ent.StatusId, dto.StatusId))
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
            await Repository.Context.SaveChangesAsync();
            CombineStatuses(Repository);
            if (IsValid)
                transaction.Commit();
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " - " + e.InnerException);
            transaction.Rollback();
        }
    }

    public async Task Reject(RejectStatusAdditionalAgreementDto dto)
    {
        var doc = Repository.Context.Set<AdditionalAgreement>()
          .Include(x => x.Signs)
          .FirstOrDefault(x => x.Id == dto.Id);

        if (doc == null)
        {
            AddError("По вашему запросу запись не найдено");
            return;
        }

        //if (doc.StatusId == StatusIdConst.SIGNED)
        //{
        //    AddError("Bu shartnoma Tadbirkor tomonidan imzolangan. :( ");
        //    return;
        //}

        #region Bu HRM tayyor bo'ganda commentdan ochamiz o'chirmanglar
        var signer = UnitOfWork.Context.Set<SignCriterion>()
          .Include(x => x.Position)
          .Where(x => x.ApplicationTypeId == ApplicationTypeIdConst.MEMSHIP);
        if (signer == null)
        {
            AddError("Imzo chekuvchi belgilanmagan. Bu hujjat imzolash uchun lavozim belgilashni talab qiladi.");
            return;
        }

        if (signer.Any(x => x.PositionId == _authService.User.PositionId))
        {
            AddError("Ushbu hujjatni imzolash uchun lavozim belgilangan lavozimga to'g'ri kelmaydi.");
            return;
        }
        #endregion

        var eImzoTimstampDto = new EImzoTimeStampDto
        {
            SignData = dto.SignedData,
            Inn = dto.IsPinfl ? null : _authService.Contractor?.Inn,
            Pinfl = dto.IsPinfl ? _authService.User.Pinfl : null
        };
        var timeStamp = await _eImzoService.TimeStamp(eImzoTimstampDto);

        CombineStatuses(_eImzoService);
        if (HasErrors)
            return;

        if (_authService.Contractor != null)
        {
            if (doc.ContractorId != _authService.Contractor.Id)
            {
                AddError("Нет доступа");
                return;
            }
            var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
            {
                SignData = timeStamp.Pkcs7b64,
                Inn = dto.IsPinfl ? null : _authService.Contractor.Inn,
                Pinfl = _authService.Contractor.Pinfl
            });
        }
        else
        {
            var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
            {
                SignData = timeStamp.Pkcs7b64,
                Inn = null,
                Pinfl = _authService.User.Pinfl
            });
        }

        CombineStatuses(_eImzoService);
        if (HasErrors)
            return;

        dto.SignFile = SaveFile(doc.Id, timeStamp.Pkcs7b64, "sign.txt");
        dto.DataFile = SaveFile(doc.Id, JsonConvert.SerializeObject(doc), "data.txt");
        dto.SignedUserInfo = _authService.UserName + " - " + _authService.User.FullName;

        using var transaction = UnitOfWork.BeginTransaction();
        try
        {
            Repository.UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanAdditionalAgreementApplyStatus(ent.StatusId, dto.StatusId))
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
            await Repository.Context.SaveChangesAsync();
            CombineStatuses(Repository);
            if (IsValid)
                await transaction.CommitAsync();
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " - " + e.InnerException);
            await transaction.RollbackAsync();
        }
    }

    public override HaveId<long> Create(CreateAdditionalAgreementDlDto dto)
    {
        var memshipContract = UnitOfWork.MemshipContractRepository.ById(dto.MemshipContractId);
        if ((!dto.BaseFixedMinimumValue.HasValue) && memshipContract != null)
        {
            dto.BaseFixedMinimumValue = memshipContract.BaseFixedMinimumValue;
        }

        var bhm = UnitOfWork.FixedMinimumValueRepository.AllAsQueryable
            .FirstOrDefault(x => x.MinimumValueTypeId == MinimumValueTypeIdConst.BRV).FixedValue;

        dto.Amount = (dto.BaseFixedMinimumValue ?? 0) * bhm;

        if (memshipContract != null &&
            UnitOfWork.AdditionalAgreementRepository.AllAsQueryable.Any(x =>
            x.StatusId != StatusIdConst.REJECTED &&
            x.MemshipContractId == memshipContract.Id))
        {

        }
        bool canCommit = UnitOfWork.CurrentTransaction == null;
        var transaction = canCommit ? UnitOfWork.BeginTransaction() : UnitOfWork.CurrentTransaction;
        try
        {
            var entity = Repository.Create(dto, ent =>
            {

            });
            CombineStatuses(Repository);
            if (IsValid)
            {
                UnitOfWork.Save();
                transaction.Commit();
                return HaveId.Create(entity.Id);
            }
        }
        catch (Exception ex)
        {
            if (canCommit)
                transaction.Rollback();
        }
        return null;
    }

    public override void Update(UpdateAdditionalAgreementDlDto dto)
    {
        bool canCommit = UnitOfWork.CurrentTransaction == null;
        var transaction = canCommit ? UnitOfWork.BeginTransaction() : UnitOfWork.CurrentTransaction;
        try
        {
            Repository.Update(dto, ent =>
            {
                if (!StatusIdConst.CanAdditionalAgreementApplyStatus(ent.StatusId, StatusIdConst.MODIFIED))
                {
                    AddError("Нет доступа!!!");
                    return;
                }
            });
            CombineStatuses(Repository);
            if (IsValid)
            {
                UnitOfWork.Save();
                transaction.Commit();
            }
        }
        catch (Exception ex)
        {
            AddError($"Error: {ex.Message}, - {ex.InnerException}");
            if (canCommit)
                transaction.Rollback();
        }
        finally
        {
            if (canCommit)
                transaction.Dispose();
        }
        //return null;
    }
    public async Task<byte[]> DownloadPdf(Guid id2, string? lang)
    {
        var language  = lang?? "uz-cyrl";
        var wordFile = _storageService.GetStaticFile(
           StaticFileConst.WordTemplate.GetFileName(language, StaticFileConst.WordTemplate.ADDITIONAL_AGREEMENT));
        
        var lan = UnitOfWork.Context.Set<Language>()
          .FirstOrDefault(l => l.Code == language)?.Id ?? 1;

        var dto = Repository.ReadAsNoTracked<MemshipAdditionalAgreementDto>()
            .FirstOrDefault(x => x.Id2 == id2);
        
        if (dto == null)
        {
            AddError("Hujjat topilmadi.");
            return null;
        }
        var plh = WordFactory.MakePlaceholders(dto);

        #region QrCodes
        var signSsp = UnitOfWork.Context.Set<AdditionalAgreementSign>()
            .FirstOrDefault(s => s.OwnerId == dto.Id && s.StatusId == StatusIdConst.SIGNING);
        
        var signContractor = UnitOfWork.Context.Set<AdditionalAgreementSign>()
            .FirstOrDefault(s => s.OwnerId == dto.Id && s.StatusId == StatusIdConst.SIGNED);

        var link = _systemConf.QrImagePrintMy + "/Memship/AdditionalAgreement/DownloadPdf?id2=" + dto.Id2.ToString();

        var qrDownload = new MemoryStream(QRCodeHelper.GeneratePng(link));
        plh.ImagePlaceholders.Add("QrDownload", new() { Dpi = 512, MemStream = qrDownload });

        if (signSsp != null)
        {
            var QrCodeSsp = new MemoryStream(QRCodeHelper.GeneratePng(dto.Id2.ToString()
                    + "  " + dto.DocNumber
                    + "  " + dto.DocOn.ToString(Constants.DATE_FORMAT)
                    + "  " + signSsp.SignedUserInfo));
            plh.ImagePlaceholders.Add("QrCodeSsp",
                new() { Dpi = 512, MemStream = QrCodeSsp });
            plh.TextPlaceholders.Add("QrCodeSsp", "++QrCodeSsp++");
        }
        else
            plh.TextPlaceholders.Add("QrCodeSsp", "");
        if (signContractor != null)
        {
            var QrCodeContractor = new MemoryStream(QRCodeHelper.GeneratePng(dto.Id2.ToString()
                   + "  " + dto.DocNumber
                   + "  " + dto.DocOn.ToString(Constants.DATE_FORMAT)
                   + "  " + signContractor.SignedUserInfo));

            plh.ImagePlaceholders.Add("QrCode",
                new() { Dpi = 512, MemStream = QrCodeContractor });
            plh.TextPlaceholders.Add("QrCode", "++QrCode++");
        }
        else
            plh.TextPlaceholders.Add("QrCode", "");
        #endregion
        DateTimeFormatInfo info = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat;

        //DateOnly memshipContractDate = dto.MemshipContractSignedAt.HasValue ? dto.MemshipContractSignedAt.Value.AsDateOnly() : dto.MemshipContractDocOn;
        DateOnly memshipContractDate = dto.MemshipContractDocOn;

        plh.TextPlaceholders.Add(nameof(memshipContractDate.Year), memshipContractDate.Year.ToString());
        plh.TextPlaceholders.Add(nameof(memshipContractDate.Day), memshipContractDate.Day.ToString());
        
        var month = info.MonthNames[memshipContractDate.Month - 1];
        if (month.Last() == 'ь')
            month = month.Replace("ь", "");
        plh.TextPlaceholders.Add(nameof(memshipContractDate.Month), month);

        wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();
        var res = await _pdfConverter.DocxToPdfAsync(wordFile, new());
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
    public override void Delete(long id)
    {
        bool canCommit = UnitOfWork.CurrentTransaction == null;
        var transaction = canCommit ? UnitOfWork.BeginTransaction() : UnitOfWork.CurrentTransaction;
        try
        {
            Repository.UpdateStatus(new()
            {
                Id = id,
                StatusId = StatusIdConst.DELETED
            }, ent =>
            {
                if (!StatusIdConst.CanAdditionalAgreementApplyStatus(ent.StatusId, StatusIdConst.DELETED))
                {
                    AddError("Нет доступа.");
                    return;
                }
            });
            CombineStatuses(Repository);
            if (IsValid)
            {
                UnitOfWork.Save();
                transaction.Commit();
            }
        }
        catch (Exception ex)
        {
            AddError($"Error: {ex.Message}, - {ex.InnerException}");
            if (canCommit)
                transaction.Rollback();
        }
        finally
        {
            if (canCommit)
                transaction.Dispose();
        }
    }

    private Guid SaveFile(long docId, string data, string fileName)
    {
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(data);
        writer.Flush();
        ms.Position = 0;
        var fileInfo = _storageService.Save($"{nameof(TableIdConst.ADDITIONAL_AGREEMENT)}_SIGN_DATA", docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

        return fileInfo.FirstOrDefault().FileId;
    }
}
