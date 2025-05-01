using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OfficeOpenXml;
using OpenXmlPowerTools;
using Spire.Doc;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.Srv;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.DocxToPdf;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WbImzo.Models;
using WbImzo.Proxy.Sdk;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.OfficeTools;
using WEBASE.OfficeTools.Factory;
using WEBASE.OfficeTools.Handlers;
using WEBASE.QRCode;
using WEBASE.Storage;
using WEBASE.Utility;

namespace SspUis.BizLogicLayer
{
    public class SrvDeedService
        : BaseEntityService<long, ServiceDeed, SrvDeedListDto, SrvDeedDto, CreateServiceDeedDlDto, UpdateServiceDeedDlDto,
          IServiceDeedRepository, SrvDeedSortFilterOption>
         ,ISrvDeedService
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentChangeLogService _documentChangeLogService;
        private readonly IEImzoService _eImzoService;
        private readonly INumberService _numberService;
        private readonly IStorageService _storageService;
        private readonly ISrvApplicationService _applicationService;
        private readonly ISrvCompleteService _completeService;
        private readonly IConvertService _pdfConverter;
        private readonly SystemConf _systemConf;
        private readonly ICultureHelper _cultureHelper;
        private readonly IWbImzoService _wbImzoService;
        private readonly WbImzoConfig _wbImzoConfig;
        private readonly LinkConfig _linkConfig;

        public SrvDeedService(
            IUnitOfWork unitOfWork,
            INumberService numberService,
            IDocumentChangeLogService documentChangeLogService,
            IAuthService authService,
            IEImzoService eImzoService,
            IStorageService storageService,
            ISrvApplicationService appService,
            ISrvCompleteService completeService,
            IConvertService pdfConverter,
            ICultureHelper cultureHelper,
            WbImzoConfig wbImzoConfig,
            List<LinkConfig> linkConfigs,
            IWbImzoService wbImzoService,
            SystemConf systemConf)
            : base(unitOfWork)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
            _documentChangeLogService = documentChangeLogService;
            _numberService = numberService;
            _eImzoService = eImzoService;
            _storageService = storageService;
            _applicationService = appService;
            _completeService = completeService;
            _pdfConverter = pdfConverter;
            _systemConf = systemConf;
            _cultureHelper = cultureHelper;
            _wbImzoService = wbImzoService;
            _wbImzoConfig = wbImzoConfig;
            _linkConfig = linkConfigs.FirstOrDefault(config => config.Name == "ServiceDeed");
        }
        #region C O R E
        public override PagedResult<SrvDeedListDto> GetList(SrvDeedSortFilterOption options)
        {
            var data = Repository.ReadAsNoTracked<SrvDeedListDto>()
                .SortFilter(options)
                .AsPagedResult(options);

            return data;
        }
		public int GetCount()
		{
            SrvDeedSortFilterOption options = new SrvDeedSortFilterOption();
            var data = Repository.ReadAsNoTracked<SrvDeedListDto>()
              .SortFilter(options);

            return data.Count();
		}
		public override SrvDeedDto Get()
        {
            return new SrvDeedDto
            {
                Id2 = Guid.NewGuid(),
                DocOn = DateOnly.FromDateTime(DateTime.Now),
                DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_SERVICE_DEED, 1).Item2
            };
        }
        public override SrvDeedDto Get(long id)
        {
            var dto = Repository.ById<SrvDeedDto>(id);
            CombineStatuses(Repository);

            if (HasErrors)
            { AddError("xato !"); return null; }

            return dto;
        }
        public SrvContractForDeedDto GetBySrvContractId(long srvContractId)
        {
            var contract = _unitOfWork.Context
                .Set<ServiceContract>()
                .Include(x => x.Groups).ThenInclude(x => x.Tables).ThenInclude(x => x.NeedChamberService).ThenInclude(x => x.Translates)
                .Include(x => x.Groups).ThenInclude(x => x.Tables).ThenInclude(x => x.NeedChamberService.ServicePriceType).ThenInclude(x => x.Translates)
                .Include(x => x.Groups).ThenInclude(x => x.NeedChamberServiceGroup).ThenInclude(x => x.Translates)
                .Include(x => x.Contractor)
                .Include(x => x.Contractor.District)
                .Include(x => x.Contractor.Region)
                .Include(a => a.Application.Status).ThenInclude(c => c.Translates)
                .AsSplitQuery()
                .FirstOrDefault(a => a.Id == srvContractId && a.Application.StatusId != StatusIdConst.DELETED && a.Application.StatusId != StatusIdConst.REJECTED);

            if (contract == null)
            { AddError("Shartnomasi topilmadi !"); return null; }

            var orgByRegion = UnitOfWork.Context.Set<DataLayer.EfClasses.Organization>()
                .FirstOrDefault(org => org.RegionId == contract.Application.RegionId
                                    && (org.OrganizationGroupId == OrganizationGroupIdConst.SSP
                                    || org.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH));

            var contractor = _unitOfWork.Context.Set<Contractor>().FirstOrDefault(a => a.Id == contract.ContractorId);

            if (orgByRegion == null)
            { AddError("Tashkilot topilmadi !"); return null; }

            var dict = LastServicePrices();

            return new SrvContractForDeedDto()
            {
                SrvContractDocNumber = contract.DocNumber,
                SrvContractDocOn = contract.DocOn,
                Details = null/*application.Application.Message*/,
                ApplicationId = contract.ApplicationId,
                ContractorId = contract.ContractorId,
                SrvContractorId = contract.Id,
                StatusId = contract.Application.StatusId,
                Status = contract.Application.Status.Translates.AsQueryable()
                            .FirstOrDefault(StatusTranslate.GetExpr(
                                TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?
                                .TranslateText ?? contract.Application.Status.FullName,
                ContractorInn = contractor.Inn,
                ContractorFullName = contractor.FullName,
                ContractorRegion = contractor.Region.FullName,
                ContractorDistrict = contractor.District.FullName,
                ContractorAddress = contractor.Address,
                ContractorPhoneNumber = contractor.PhoneNumber,
                ContractorDirector = contractor.Director,
                Organization = orgByRegion.FullName,
                Groups = contract.Groups.Select(x => new SrvDeedGroupDto
                {
                    Id = x.Id,
                    GroupId = x.GroupId,
                    CreatedAt = x.CreatedAt,
                    Group = x.NeedChamberServiceGroup != null
                        ? x.NeedChamberServiceGroup.Translates.AsQueryable()
                            .FirstOrDefault(NeedChamberServiceGroupTranslate.GetExpr(
                                TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?
                                .TranslateText ?? x.NeedChamberServiceGroup.FullName
                        : string.Empty,
                    Tables = x.Tables.Select(y => new SrvDeedTableDto
                    {
                        NeedChamberService = y.NeedChamberService.Translates.AsQueryable()
                            .FirstOrDefault(NeedChamberServiceTranslate.GetExpr(
                                TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?
                                .TranslateText ?? y.NeedChamberService.FullName,
                        NeedChamberServiceId = y.NeedChamberServiceId,
                        ServicePriceTypeId = y.NeedChamberService.ServicePriceTypeId,
                        ServicePriceType = y.NeedChamberService.ServicePriceType.Translates.AsQueryable()
                            .FirstOrDefault(ServicePriceTypeTranslate.GetExpr(
                                TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?
                                .TranslateText ?? y.NeedChamberService.ServicePriceType.FullName,
                        ServicePriceId = y.ServicePriceId,
                        ServicePriceTableId = y.ServicePriceTableId,
                        OfferServiceText = y.OfferServiceText,
                        RealCoef = y.RealCoef,
                        CanPayDivided = y.CanPayDivided,
                        Price = y.Price,
                        BeginCoef = dict.ContainsKey(y.NeedChamberServiceId) ? dict[y.NeedChamberServiceId].BeginCoef : null,
                        EndCoef = dict.ContainsKey(y.NeedChamberServiceId) ? dict[y.NeedChamberServiceId].EndCoef : null,
                        ConcreteCoef = dict.ContainsKey(y.NeedChamberServiceId) ? dict[y.NeedChamberServiceId].ConcreteCoef : null,
                        IsConcrete = dict.ContainsKey(y.NeedChamberServiceId) ? dict[y.NeedChamberServiceId].IsConcrete : null,
                    }).ToList(),
                }).ToList(),
            };
        }
        public override HaveId<long> Create(CreateServiceDeedDlDto dto)
        {
            bool isCommit = UnitOfWork.CurrentTransaction == null;
            using var transaction = isCommit ? UnitOfWork.BeginTransaction() : UnitOfWork.CurrentTransaction;
            try
            {
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
                AddError($"{e.Message} - {e.InnerException}");
                transaction.Rollback();
            }
            finally
            {
                transaction.Dispose();
            }

            return null;
        }
        public override void Update(UpdateServiceDeedDlDto dto)
        {
            using var transaction = UnitOfWork.BeginTransaction();
            try
            {
                var entity = base.Repository.Update(dto, ent => Validation(dto, ent));
                UnitOfWork.Save();

                CombineStatuses(Repository);

                if (IsValid)
                    transaction.Commit();
            }
            catch (DbUpdateException e)
            {
                AddError(e.Message + " " + e.InnerException);
                transaction.Rollback();
            }
        }
        public override void Delete(long id)
        {
            using (var transaction = UnitOfWork.BeginTransaction())
            {
                try
                {
                    var statusDto = new DeletetatusSrvDeedDto()
                    {
                        Id = id,
                        StatusId = StatusIdConst.DELETED
                    };

                    var contract = Repository.UpdateStatus(statusDto, ent =>
                    {
                        if (!StatusIdConst.CanApplySrvDocStatus(ent.StatusId, statusDto.StatusId))
                            AddError("Нет доступа");
                    });

                    UnitOfWork.Save();

                    if (IsValid)
                        transaction.Commit();
                }
                catch (DbUpdateException ex)
                {
                    AddError(ex.Message);
                    transaction.Rollback();
                }
            }
        }
        #endregion
        #region S T A T U S
        public async Task Signed(SignStatusSrvDeedDto dto)
        {
            var doc = this.Get(dto.Id);

            if (doc is null)
            { AddError("По вашему запросу запись не найдено"); return; }

            if (_authService.Contractor is not null)
                if (doc.ContractorId != _authService.Contractor.Id)
                { AddError("Нет доступа"); return; }

            var timeStamp = await _eImzoService.TimeStamp(new EImzoTimeStampDto
            {
                SignData = dto.SignedData,
                Inn = dto.IsPinfl ? null : _authService.Contractor?.Inn,
                Pinfl = dto.IsPinfl ? _authService.User.Pinfl : null
            });

            CombineStatuses(_eImzoService);
            if (HasErrors)
                return;

            if (_authService.Contractor != null)
            {
                var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
                {
                    SignData = timeStamp.Pkcs7b64,
                    Inn = dto.IsPinfl ? null : _authService.Contractor.Inn,
                    Pinfl = dto.IsPinfl ? _authService.Contractor.Pinfl : null
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
            dto.SignedUserInfo = _authService.User.ToTextForDocumentLog();

            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                Repository.AllAsQueryable.Lock(dto.Id);

                var deed = Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplySrvDeedStatus(ent.StatusId, dto.StatusId))
                        AddError("Нет доступа");
                });
                deed.Signs.Add(new()
                {
                    OwnerId = doc.Id,
                    SignFile = dto.SignFile,
                    DataFile = dto.DataFile,
                    SignedUserInfo = dto.SignedUserInfo,
                    SignedAt = DateTime.Now,
                    StatusId = dto.StatusId,
                });

                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                foreach (var group in deed.Groups)
                {
                    foreach (var table in group.Tables)
                    {
                        var complete = _completeService.Create(new CreateCompletedServiceDlDto
                        {
                            DocNumber = deed.DocNumber,
                            DocOn = DateOnly.FromDateTime(DateTime.Now),
                            ContractorId = deed.Id,
                            ServiceContractId = deed.SrvContractorId,
                            EmployeeManageId = _authService.User.EmployeeManageId,
                            ServiceApplicationId = deed.Application.ServiceApplication.Id,
                        });
                    }
                }

                UnitOfWork.Save();

                var res = CreateDocumentChangeLog(dto.Id);
                if (HasErrors)
                { transaction.Rollback(); return; }

                if (IsValid)
                    transaction.Commit();
            }
            catch (Exception ex)
            {
                AddError($"{ex.Message} // {ex.InnerException}");
                transaction.Rollback();
            }
            finally
            {
                transaction.Dispose();
            }
        }
        public async ValueTask Cancel(CancelStatusSrvDeedDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    Repository.AllAsQueryable.Lock(dto.Id);
                    var contract = Repository.UpdateStatus(dto, ent =>
                    {
                        if (!StatusIdConst.CanApplySrvDeedStatus(ent.StatusId, dto.StatusId))
                            AddError("Имкони йўқ / Нет доступа");
                    });

                    CombineStatuses(Repository);
                    if (HasErrors)
                        return;

                    UnitOfWork.Save();

                    _applicationService.Cancel(new CancelStatusSrvApplicationDto { Id = contract.Application.ServiceApplication.Id, Message = contract.Details });
                    CombineStatuses(_applicationService);
                    if (HasErrors)
                        return;

                    var log = CreateDocumentChangeLog(dto.Id);
                    if (IsValid)
                    {
                        transaction.Commit();
                        await UpdateSignRequestStateAsync(contract);
                    }
                }
                catch (Exception ex)
                {
                    AddError($"{ex.Message} - {ex.InnerException}");
                    return;
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }
        public async Task Signing(SigningStatusSrvDeedDto dto)
        {
            var doc = this.Get(dto.Id);

            if (doc is null)
            { AddError("По вашему запросу запись не найдено"); return; }

            var timeStamp = await _eImzoService.TimeStamp(new EImzoTimeStampDto
            {
                SignData = dto.SignedData,
                Inn = dto.IsPinfl ? null : _authService.Contractor?.Inn,
                Pinfl = dto.IsPinfl
                 ? (_authService.Contractor == null ? _authService.User.Pinfl : _authService.Contractor.Pinfl)
                 : null
            });

            CombineStatuses(_eImzoService);
            if (HasErrors)
                return;

            if (_authService.Contractor != null)
            {
                var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
                {
                    SignData = timeStamp.Pkcs7b64,
                    Inn = dto.IsPinfl ? null : _authService.Contractor.Inn,
                    Pinfl = dto.IsPinfl ? _authService.Contractor.Pinfl : null
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
            dto.SignedUserInfo = _authService.User.ToTextForDocumentLog();

            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                Repository.AllAsQueryable.Lock(dto.Id);

                var contract = Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplySrvDeedStatus(ent.StatusId, dto.StatusId))
                        AddError("Нет доступа");
                });

                contract.Signs.Add(new()
                {
                    OwnerId = doc.Id,
                    SignFile = dto.SignFile,
                    DataFile = dto.DataFile,
                    SignedUserInfo = dto.SignedUserInfo,
                    SignedAt = DateTime.Now,
                    StatusId = dto.StatusId,
                });

                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                UnitOfWork.Save();

                var res = CreateDocumentChangeLog(dto.Id);
                if (HasErrors)
                { transaction.Rollback(); return; }

                if (IsValid)
                    transaction.Commit();
            }
            catch (Exception ex)
            {
                AddError($"{ex.Message} // {ex.InnerException}");
                transaction.Rollback();
            }
            finally
            {
                transaction.Dispose();
            }
        }
        public async ValueTask Reject(RejectStatusSrvDeedDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    Repository.AllAsQueryable.Lock(dto.Id);

                    var doc = Repository.UpdateStatus(dto, ent =>
                    {
                        if (!StatusIdConst.CanApplySrvDeedStatus(ent.StatusId, dto.StatusId))
                            AddError("Имкони йўқ / Нет доступа");
                    });

                    CombineStatuses(Repository);
                    if (HasErrors)
                        return;

                    _unitOfWork.Save();

                    if (doc?.ApplicationId is not null)
                        _applicationService.Reject(new RejectStatusSrvApplicationDto { Id = (long)doc.Application.ServiceApplication.Id, Message = doc.Details });
                    CombineStatuses(_applicationService);
                    if (HasErrors)
                        return;

                    var log = CreateDocumentChangeLog(dto.Id);

                    if (IsValid)
                    {
                        transaction.Commit();
                        await UpdateSignRequestStateAsync(doc);
                    }
                        
                }
                catch (Exception e)
                {
                    AddError($"{e.Message} -- {e.InnerException}");
                    transaction.Rollback();
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }
        #endregion
        #region F I L E
        public byte[] DownloadPdf(Guid id2, string? lang)
        {
            var language = lang ?? "uz-latn";
            SrvDeedDto dto = Repository.ReadAsNoTracked<SrvDeedDto>(applyFilter: false)
                                .FirstOrDefault(x => x.Id2 == id2);
            Placeholders plh = WordFactory.MakePlaceholders(dto);
            if (dto == null)
            {
                AddError("file not found");
            }

            MemoryStream wordFile = null;

            wordFile = _storageService.GetStaticFile(
               StaticFileConst.WordTemplate.GetFileName(
            language,
            StaticFileConst.WordTemplate.SRV_DEED_WITH_PAY));

            //var placeholder = new Placeholders();
            var link = _systemConf.QrImagePrintMy + "/srv/ServiceDeed/DownloadPdf?id2=" + dto.Id2.ToString();
            using var qrcode = new MemoryStream(QRCodeHelper.GeneratePng(link, 256, 256));
			var qrDownload = new MemoryStream(QRCodeHelper.GeneratePng(link));
			plh.ImagePlaceholders.Add("QrDownload", new() { Dpi = 768, MemStream = qrDownload });

			var serviceDeed = _unitOfWork.Context.Set<ServiceDeed>().Include(a => a.Signs).FirstOrDefault(a => a.Id2 == id2);
            var signDeedor = serviceDeed.Signs.FirstOrDefault(a => a.StatusId == StatusIdConst.SIGNED);
            var signSsp = serviceDeed.Signs.FirstOrDefault(a => a.StatusId == StatusIdConst.SIGNING);
            var linkssp = _systemConf.QrImagePrintMy+" " + dto.OrganizationDirector + " "+ dto.OrganizationInn;
			var qrssp = new MemoryStream(QRCodeHelper.GeneratePng(linkssp));

			var linkContractor = _systemConf.QrImagePrintMy + " " + dto.ContractorDirector + " " + dto.ContractorInn;
			var qrContractor = new MemoryStream(QRCodeHelper.GeneratePng(linkContractor));
			if (signSsp != null)
            {
				
				plh.ImagePlaceholders.Add("QrCodeSsp", new()
                {
                    Dpi = 512,
                    MemStream = qrssp,
                });
			}
            else
				plh.ImagePlaceholders.Add("QrCodeSsp", new()
				{
					Dpi = 512,
					MemStream = qrssp,
				});

            if (signDeedor != null)
            {
                var QrCodeDeedor = new MemoryStream(QRCodeHelper.GeneratePng(serviceDeed.Id2.ToString()
                       + "  " + serviceDeed.DocNumber
                       + "  " + serviceDeed.DocOn.ToString(Constants.DATE_FORMAT)
                       + "  " + signDeedor.SignedUserInfo));

				plh.ImagePlaceholders.Add("QrCode", new()
				{
					Dpi = 512,
					MemStream = qrContractor,
				});
			}
            else
				plh.ImagePlaceholders.Add("QrCode", new()
				{
					Dpi = 512,
					MemStream = qrContractor,
				});

			/*if(res == null)
            {
                Document document = new Document();
                document.LoadFromStream(wordFile, FileFormat.Docx);
                var pdfStream = new MemoryStream();
                document.SaveToStream(pdfStream, FileFormat.PDF);
                return pdfStream.ToArray();
            }*/

			wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();
			var res = _pdfConverter.DocxToPdfAsync(wordFile, new()).Result;
			CombineStatuses(_pdfConverter);

			return res;
        }
        public byte[] DownloadPdfOld(Guid id2, string? lange)
        {
            var lang = lange ?? "uz-latn";
            var wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                    ServiceProvider.CultureHelper.CurrentCulture.Code,
                    StaticFileConst.WordTemplate.SRV_DEED_WITH_PAY));

            var lan = UnitOfWork.Context.Set<Language>()
                .FirstOrDefault(l => l.Code == ServiceProvider.CultureHelper.CurrentCulture.Code)?.Id ?? 1;

            var serviceDeed = UnitOfWork.Context.Set<ServiceDeed>()
                .Include(x => x.Groups).ThenInclude(x => x.NeedChamberServiceGroup).ThenInclude(x => x.Translates)
                .Include(x => x.Groups).ThenInclude(x => x.Tables).ThenInclude(x => x.NeedChamberService).ThenInclude(x => x.Translates)
                .Include(x => x.Organization).ThenInclude(x => x.Translates)
                .Include(x => x.Organization.Oked)
                .Include(x => x.Organization.SettlementAccounts).ThenInclude(x => x.Bank).ThenInclude(x => x.Translates)
                .Include(x => x.Signs)
                .Include(x => x.Application).ThenInclude(x => x.Region).ThenInclude(x => x.Translates)
                .AsSplitQuery()
                .FirstOrDefault(x => x.Id2 == id2);

            if (serviceDeed is null)
            {
                AddError("Deed not found");
                return null;
            }

            var contractor = UnitOfWork.Context.Set<DataLayer.EfClasses.Contractor>()
               .Include(c => c.Oked)
               .Include(c => c.SettlementAccounts)
               .Include(c => c.Bank)
               .Include(c => c.Bank.Translates)
               .AsSplitQuery()
               .FirstOrDefault(c => c.Id == serviceDeed.ContractorId);



            var signDeedor = serviceDeed.Signs.FirstOrDefault(s => s.StatusId == StatusIdConst.SIGNED);
            var signSsp = serviceDeed.Signs.FirstOrDefault(s => s.StatusId == StatusIdConst.SIGNING);

            var placeholder = new Placeholders();
            var link = _systemConf.QrImagePrintMy + "/Service/ServiceDeed/DownloadPdf?id2=" + serviceDeed.Id2.ToString();
            var qrDownload = new MemoryStream(QRCodeHelper.GeneratePng(link));
            placeholder.ImagePlaceholders.Add("QrDownload", new() { Dpi = 512, MemStream = qrDownload });

            #region P L A C E   H O L D E R

            placeholder.TextPlaceholders.Add(nameof(serviceDeed.DocNumber), serviceDeed.DocNumber);
            placeholder.TextPlaceholders.Add(nameof(serviceDeed.DocOn), serviceDeed.DocOn.ToString(Constants.DATE_FORMAT));

            var signUser = signSsp != null
                ? UnitOfWork.Context.Set<DataLayer.EfClasses.User>().Include(x => x.Person).FirstOrDefault(x => x.Id == signSsp.CreatedUserId)
                : null;

            placeholder.TextPlaceholders.Add(nameof(serviceDeed.Organization.Director), signUser?.Person?.FullName ?? "-");
            placeholder.TextPlaceholders.Add("DeedorFullName", contractor.FullName);
            placeholder.TextPlaceholders.Add("DeedorDirector", contractor.Director);
            var totalPrice = serviceDeed.Groups.Sum(x => x.Tables.Sum(s => s.RealCoef));
            if (totalPrice - ((int)totalPrice) == 0)
                placeholder.TextPlaceholders.Add("TotalPrice", $"{((int)totalPrice)}");
            else
                placeholder.TextPlaceholders.Add("TotalPrice", $"{totalPrice}");
            placeholder.TextPlaceholders.Add("TotalPriceName", $"({serviceDeed.Groups.Sum(x => x.Tables.Sum(s => s.Price))/*.DecimalInWord(lang)*/})"); //DecimalInWord hato ishlavotti

            /// БАЖАРУВЧИ 
            placeholder.TextPlaceholders.Add(nameof(serviceDeed.Organization.FullName),
                serviceDeed.Organization.Translates.FirstOrDefault(x =>
                    x.LanguageId == lan && x.ColumnName == TranslateColumn.full_name.ToString())?.TranslateText
                    ?? serviceDeed.Organization.FullName);
            placeholder.TextPlaceholders.Add(nameof(serviceDeed.Organization.Address), serviceDeed.Organization.Address);

            var orgAccount = serviceDeed.Organization.SettlementAccounts.FirstOrDefault();
            placeholder.TextPlaceholders.Add(nameof(orgAccount.AccountCode), orgAccount?.AccountCode ?? "-");
            placeholder.TextPlaceholders.Add(nameof(orgAccount.Bank.BankName),
                orgAccount?.Bank.Translates.FirstOrDefault(c =>
                        c.LanguageId == lan && c.ColumnName == BankTranslateColumn.bank_name.ToString())?.TranslateText
                        ?? orgAccount?.Bank.BankName ?? "-");
            placeholder.TextPlaceholders.Add(nameof(orgAccount.Bank.Code), orgAccount?.Bank.Code ?? "-");
            placeholder.TextPlaceholders.Add(nameof(serviceDeed.Organization.Inn), serviceDeed.Organization.Inn);
            placeholder.TextPlaceholders.Add(nameof(serviceDeed.Organization.Oked), serviceDeed.Organization.Oked.Code);
            placeholder.TextPlaceholders.Add(nameof(serviceDeed.Organization.PhoneNumber), serviceDeed.Organization.PhoneNumber ?? "");

            if (signSsp != null)
            {
                var QrCodeSsp = new MemoryStream(QRCodeHelper.GeneratePng(serviceDeed.Id2.ToString()
                        + "  " + serviceDeed.DocNumber
                        + "  " + serviceDeed.DocOn.ToString(Constants.DATE_FORMAT)
                        + "  " + signSsp.SignedUserInfo));
                placeholder.ImagePlaceholders.Add("QrCodeSsp", new() { Dpi = 512, MemStream = QrCodeSsp });
                placeholder.TextPlaceholders.Add("QrCodeSsp", "++QrCodeSsp++");
            }
            else
                placeholder.TextPlaceholders.Add("QrCodeSsp", "");

            /// БУЮРТМАЧИ
            var account = contractor.SettlementAccounts.FirstOrDefault(a => a.IsMain);
            placeholder.TextPlaceholders.Add("DeedorAccountCode", account?.AccountCode ?? "-");
            placeholder.TextPlaceholders.Add("DeedorAddress", contractor.Address);
            placeholder.TextPlaceholders.Add("DeedorBankName",
                contractor.Bank?.Translates.FirstOrDefault(c =>
                c.LanguageId == lan && c.ColumnName == BankTranslateColumn.bank_name.ToString())?.TranslateText
                ?? contractor.Bank?.BankName ?? "");
            placeholder.TextPlaceholders.Add("DeedorBankCode", contractor.Bank?.Code ?? "-");
            placeholder.TextPlaceholders.Add("DeedorInn", contractor?.Inn ?? "-");
            placeholder.TextPlaceholders.Add("DeedorOked", contractor.Oked?.Code ?? "-");
            placeholder.TextPlaceholders.Add("DeedorPhoneNumber", contractor?.PhoneNumber ?? "-");

            if (signDeedor != null)
            {
                var QrCodeDeedor = new MemoryStream(QRCodeHelper.GeneratePng(serviceDeed.Id2.ToString()
                       + "  " + serviceDeed.DocNumber
                       + "  " + serviceDeed.DocOn.ToString(Constants.DATE_FORMAT)
                       + "  " + signDeedor.SignedUserInfo));

                placeholder.ImagePlaceholders.Add("QrCode", new() { Dpi = 512, MemStream = QrCodeDeedor });
                placeholder.TextPlaceholders.Add("QrCode", "++QrCode++");
            }
            else
                placeholder.TextPlaceholders.Add("QrCode", "");

            Region region;
            var toshkent = UnitOfWork.Context.Set<Region>()
                .Include(x => x.Translates)
                .FirstOrDefault(x => x.Id == RegionIdConst.TASHKENT);

            region = serviceDeed?.Application?.MemshipApplication?.ChooseLocation ?? false
                ? (serviceDeed?.Application?.MemshipApplication?.ChoosedRegion) ?? null
                : (serviceDeed?.Application?.Region) ?? toshkent;

            placeholder.TextPlaceholders.Add(nameof(serviceDeed.Application.Region),
                (region.Translates.FirstOrDefault(r =>
                        r.LanguageId == lan && r.ColumnName == TranslateColumn.full_name.ToString())?.TranslateText ??
                        region.FullName ?? "-") + ((region.Id != RegionIdConst.KARAKALPAKSTAN && region.Id != RegionIdConst.TASHKENT) ? " вилояти" : ""));
            #endregion

            var dict = serviceDeed.Groups
                .SelectMany(group => group.Tables)
                .Select(table => new { name = table.NeedChamberService.FullName, price = table.Price.ToString() })
                .ToList();

            var items = new List<Placeholders>();
            foreach (var group in dict)
            {
                Placeholders lplh = new();
                lplh.TextPlaceholders.Add("service", $"{group.name}");
                items.Add(lplh);
            }
        placeholder.TemplateListPlaceholders.Add("ChamberServices", items);

            var a = new DocXHandler(wordFile, placeholder).ReplaceAll();
            var res = _pdfConverter.DocxToPdfAsync(wordFile, new()).Result;
            if (res == null)
            {
                Document document = new Document();
                document.LoadFromStream(wordFile, FileFormat.Docx);
                var pdfStream = new MemoryStream();
                document.SaveToStream(pdfStream, FileFormat.PDF);
                return pdfStream.ToArray();
            }

            return res;
        }
        public Stream SaveAsExcel(SrvDeedSortFilterOption options)
        {
            var data = Repository.ReadAsNoTracked<SrvDeedListDto>()
                .SortFilter(options).ToList();

            MemoryStream result = new MemoryStream();
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.SRV_DEED));

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
                    ws.Cells[currentRow, column++].Value = item.OrganizationRegion;
                    ws.Cells[currentRow, column++].Value = item.OrganizationDistrict;
                    ws.Cells[currentRow, column++].Value = data.Count(x => x.SrvContractStatusId == StatusIdConst.SIGNED
                                                                            && x.OrganizationRegion == item.OrganizationRegion
                                                                              && x.OrganizationDistrict == item.OrganizationDistrict);
                    ws.Cells[currentRow, column++].Value = item.Price;
                    ws.Cells[currentRow, column++].Value = data.Count(x => x.StatusId == StatusIdConst.SIGNED
                                                                            && x.OrganizationRegion == item.OrganizationRegion
                                                                              && x.OrganizationDistrict == item.OrganizationDistrict);

                    ws.Cells[currentRow, column++].Value = data.Count(x => x.StatusId == StatusIdConst.SIGNING
                                                                            && x.OrganizationRegion == item.OrganizationRegion
                                                                              && x.OrganizationDistrict == item.OrganizationDistrict);
                    ws.Cells[currentRow, column++].Value = data.Count(x => x.StatusId == StatusIdConst.REJECTED
                                                                            && x.OrganizationRegion == item.OrganizationRegion
                                                                              && x.OrganizationDistrict == item.OrganizationDistrict);
                    currentRow++;
                }
                ws.Cells[currentRow, 4].Value = data.Count(x => x.SrvContractStatusId == StatusIdConst.SIGNED);
                ws.Cells[currentRow, 5].Value = data.Select(x => x.Price).Sum();
                ws.Cells[currentRow, 6].Value = data.Count(x => x.StatusId == StatusIdConst.SIGNED);
                ws.Cells[currentRow, 7].Value = data.Count(x => x.StatusId == StatusIdConst.SIGNING);
                ws.Cells[currentRow, 8].Value = data.Count(x => x.StatusId == StatusIdConst.REJECTED);

                ws.DeleteRow(importRow.Start.Row);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }
            result.Position = 0;
            return result;
        }
        #endregion
        #region H E L P E R
        public Guid SaveFile(long docId, string data, string fileName)
        {
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(data);
            writer.Flush();
            ms.Position = 0;
            var fileInfo = _storageService.Save(
                $"{nameof(TableIdConst.DOC_SERVICE_CONTRACT)}_SIGN_DATA",
                docId.ToString(),
                new StorageFile(Guid.NewGuid(),
                fileName,
                ms));

            return fileInfo.FirstOrDefault().FileId;
        }
        private void Validation<TDto>(ServiceDeedDlDto<TDto> dto, ServiceDeed entity)
            where TDto : ServiceDeedDlDto<TDto>
        {
            if (entity is null)
            { AddError("Хизмат шартномаси яратилишда хато !"); return; };

            var application = _unitOfWork.Context.Set<Application>()
                .Include(x => x.ServiceApplication)
                .FirstOrDefault(x => x.Id == dto.ApplicationId);

            if (application is null || application.StatusId != StatusIdConst.ACCEPTED)
            {
                Repository.AddError("Бу документ ҳали тасдиқланмаган !");
                return;
            }

            var srvApplication = _applicationService.Get(application.ServiceApplication.Id);
            if (srvApplication is null || srvApplication.Application.StatusId != StatusIdConst.ACCEPTED)
            {
                Repository.AddError("Бу документ ҳали тасдиқланмаган !");
                return;
            }

            if (entity.Id != 0)
            {
                if (!StatusIdConst.CanEditStatuses.Contains(entity.StatusId))
                    Repository.AddError("Tahrirlash mumkin emas / Невозможно редактировать");
            }
            else
            {
                // 1 doc numberga validation
                if (string.IsNullOrEmpty(dto.DocNumber))
                    throw new ArgumentException($"{nameof(dto.DocNumber)} cannot be null or empty string", nameof(dto.DocNumber));

                if (Repository.AllAsQueryable.Where(x => x.DocNumber == dto.DocNumber).Any())
                {
                    Repository.AddError("Бу документ номерли шартнома аллақачон мавжуд !");
                    return;
                }

                // 2 srvApplication va contract servislari solishtirish
                srvApplication.Groups.ForEach(group =>
                {
                    var ids = NeedChamberServiceIdsInDto(dto);
                    if (group.Tables.Any(x => !ids.Contains(x.NeedChamberServiceId)))
                        Repository.AddError("Шартномада кўрсатилган хизматлар аризадаги каби емас !");
                });
                if (HasErrors)
                    return;

                // 3 Tekin va pullikka narx kiritish va kiritmasligigayam validation
                var paid = this.PaidServices(dto);
                var prices = this.LastServicePrices();
                var n = 0;
                dto.Groups.ForEach(group =>
                {
                    group.Tables.ForEach(table =>
                    {
                        if (paid[table.NeedChamberServiceId] == ServicePriceTypeIdConst.BXM
                            || paid[table.NeedChamberServiceId] == ServicePriceTypeIdConst.PERCENTAGE_CONTRACT_SIZE)
                        {
                            if (table.RealCoef == null || table.RealCoef == 0)
                            {
                                Repository.AddError("Сиз пулли хизматга нарх киритишга мажбурсиз !");
                                return;
                            }

                            if (!prices.ContainsKey(table.NeedChamberServiceId))
                            {
                                Repository.AddError($"{table.NeedChamberServiceId} Бу хизматга ҳали нарх киритилмаган !");
                                return;
                            }
                            else
                            {
                                var lastPrice = prices[table.NeedChamberServiceId];

                                if (lastPrice.IsConcrete)
                                {
                                    if (lastPrice.ConcreteCoef != table.RealCoef)
                                    {
                                        Repository.AddError("Киритилган нарх хизмат нархида емас. // Илтимос аввал хизмат нархи билан танишинг !");
                                        return;
                                    }
                                }
                                else
                                {
                                    if (!(lastPrice.BeginCoef <= table.RealCoef && lastPrice.EndCoef >= table.RealCoef))
                                    {
                                        Repository.AddError("Киритилган нарх хизмат нархида емас. // Илтимос аввал хизмат нархи билан танишинг !");
                                        return;
                                    }
                                }
                            }
                        }
                        if (paid[table.NeedChamberServiceId] == ServicePriceTypeIdConst.BY_AGREEMENT)
                        {
                            if (table.RealCoef == null && table.RealCoef == 0)
                            {
                                Repository.AddError("Narx belgilanishi kerak!");
                                return;
                            }

                            //if (table.Price == null)
                            //{
                            //    Repository.AddError("Текин хизматга нарх {Price} белгилашингиз шарт !");
                            //    return;
                            //}
                        }
                    });
                });
            }
        }
        private Dictionary<int, ServicePriceTable> LastServicePrices()
        {
            var doc = _unitOfWork.Context.Set<ServicePrice>()
                .Include(sp => sp.Groups).ThenInclude(spg => spg.Tables)
                .OrderByDescending(x => x.DocOn).ThenByDescending(x => x.Id)
                .FirstOrDefault(x => x.StatusId != StatusIdConst.DELETED && x.StatusId != StatusIdConst.CANCELED);

            return doc.Groups.SelectMany(spg => spg.Tables.Select(st => new { st.NeedChamberServiceId, table = st }))
                .ToDictionary(i => i.NeedChamberServiceId, i => i.table);
        }
        private int[] NeedChamberServiceIdsInDto<TDto>(ServiceDeedDlDto<TDto> dto)
            where TDto : ServiceDeedDlDto<TDto>
        {
            return dto.Groups
                .SelectMany(x => x.Tables.Select(y => y.NeedChamberServiceId))
                .ToArray();
        }
        private Dictionary<int, int> PaidServices<TDto>(ServiceDeedDlDto<TDto> dto)
            where TDto : ServiceDeedDlDto<TDto>
        {
            var needChamberServiceIds = dto.Groups
                .SelectMany(t => t.Tables)
                .Select(r => r.NeedChamberServiceId)
                .Distinct()
                .ToArray();

            return _unitOfWork.Context.Set<NeedChamberService>()
                .Where(x => needChamberServiceIds.Contains(x.Id))
                .ToDictionary(g => g.Id, g => g.ServicePriceTypeId);
        }
        public HaveId<long> CreateDocumentChangeLog(long id, string message = null, string userIp = null, string userAgent = null)
        {
            var entityDto = base.Repository.ById<SrvDeedDto>(id, applyFilter: false);
            _documentChangeLogService.Create(
                dto: entityDto,
                tableId: TableIdConst.DOC_SERVICE_CONTRACT,
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
        #endregion
        public async ValueTask<ApiResult<bool>> UpdateSignRequestStateAsync(ServiceDeed contract)
        {
            WbImzoUpdateSignRequestStateDto updateState = new()
            {
                ApiKey = _wbImzoConfig.ApiKey,
                DocumentId = contract.Id,
                DocumentType = "Xizmatlar dalolatnomasi",
                DocumentIdAsString = JsonConvert.SerializeObject(contract),
                TableId = TableIdConst.DOC_SERVICE_CONTRACT,
            };

            return await _wbImzoService.UpdateSignRequestStateAsync(updateState);
        }
        public async ValueTask<(string? Url, bool Result)> WebImzoSign(WebImzoSignedFilter filter)
        {
            var doc = _unitOfWork.Context.Set<ServiceDeed>().Include(a => a.Contractor)
                                .Include(x => x.Signs)
                                .FirstOrDefault(x => x.Id == filter.Id && x.StatusId != StatusIdConst.DELETED);

            if (doc == null)
            {
                AddError("Xizmatlar dalolatnomasi topilmadi");
                return (Url: null, Result: false);
            }

            if (doc.StatusId == StatusIdConst.SIGNED)
            {
                AddError("Dalolatnoma allaqachon imzolangan");
                return (Url: null, Result: false);
            }

            if (_authService.Contractor != null)
            {
                if (doc.StatusId != StatusIdConst.SIGNING)
                {
                    AddError("Hakamlik sudi tomondan imozlanmagan ");
                    return (Url: null, Result: false);
                }

                var url = await PostToIMZOAndSentUrl(doc);

                if (url == null)
                    return (Url: null, Result: false);

                return (Url: url, Result: true);
            }
            var Url = await PostToIMZOAndSentUrl(doc);

            if (Url == null)
                return (Url: null, Result: false);

            return (Url: Url, Result: true);

        }
        public async ValueTask<string> PostToIMZOAndSentUrl(ServiceDeed contract)
        {
            if (!contract.WebImzoSecretKey.IsNullOrEmptyObject())
                await SendUrl(contract.Id);

            var canDispose = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            WbImzoCreateSignRequestDto signRequestCreateDto = CreatDtoForRequestSign(contract);


            #region Signers
            {
                #region Boshqarma boshliqi o'rinbosari
                int signPriority = 1;
                var debutyHeadOfDepartment = _unitOfWork.Context.Set<EmployeeManage>().Include(a => a.Employee).ThenInclude(a => a.Person).Include(a => a.Department)
                                                         .Where(a => a.OrganizationId == contract.OrganizationId && a.Department.OrganizationId == contract.OrganizationId)
                                                         .FirstOrDefault(a => a.PositionId == SignerPositionId.DebutyHeadOfDepartment);
                if (debutyHeadOfDepartment != null)
                {
                    signRequestCreateDto.SignRequestUsers.Add(
                                            new WbImzoCreateSignRequestUserDto
                                            {
                                                UserKey = debutyHeadOfDepartment.Employee.Person.Inn ?? debutyHeadOfDepartment.Employee.Person.Pinfl,
                                                UserInfo = debutyHeadOfDepartment.Employee.Person.FullName,
                                                UserId = (int)debutyHeadOfDepartment.Employee.PersonId,
                                                DocStatusId = StatusIdConst.SIGNING,
                                                SignPriority = signPriority,
                                                IpAddress = _authService.UserIp,
                                                UserAgent = _authService.UserAgent,
                                                UserPhoneNumber = debutyHeadOfDepartment.Employee.PhoneNumber,
                                            }
                                        );
                }
                #endregion
                #region Boshqarma boshliq birinchi o'rinbosari
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
                                                DocStatusId = StatusIdConst.SIGNING,
                                                SignPriority = signPriority,
                                                IpAddress = _authService.UserIp,
                                                UserAgent = _authService.UserAgent,
                                                UserPhoneNumber = debutyHeadOfDepartment1.Employee.PhoneNumber,
                                            }
                                        );
                }

                #endregion
                #region Boshqarma boshliqi
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
                                                                DocStatusId = StatusIdConst.SIGNING,
                                                                SignPriority = signPriority,
                                                                IpAddress = _authService.UserIp,
                                                                UserAgent = _authService.UserAgent,
                                                                UserPhoneNumber = departmentdHead.Employee.PhoneNumber,
                                                            }
                                                        );
                }
                #endregion
                #region Contractor
                signPriority = 2;

                signRequestCreateDto.SignRequestUsers.Add(
                                                new WbImzoCreateSignRequestUserDto
                                                {
                                                    UserKey = !string.IsNullOrEmpty(contract.Contractor.Inn) ? contract.Contractor.Inn : contract.Contractor.Pinfl,
                                                    UserInfo = contract.Contractor.FullName,
                                                    UserId = (int)contract.ContractorId,
                                                    DocStatusId = StatusIdConst.SIGNED,
                                                    SignPriority = signPriority,
                                                    IpAddress = _authService.UserIp,
                                                    UserAgent = _authService.UserAgent,
                                                    UserPhoneNumber = contract.Contractor.PhoneNumber,
                                                }
                                            );
                #endregion
            }
            #endregion

            var wbImzoResult = await _wbImzoService.UpsertStringFilePrintableLinkRequestAsync(signRequestCreateDto);
            if (!wbImzoResult.IsSuccess && wbImzoResult.Response is null)
                CombineStatuses(wbImzoResult.GetStatusGeneric());

            if (HasErrors || wbImzoResult.Response is null)
            {
                AddError("Dalolatnoma imzolash uchun yuborilayotgan jarayonda xatolik yuz berdi");
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

            var contract = _unitOfWork.Context.Set<ServiceDeed>().FirstOrDefault(a => a.Id == contractId);

            if (contract.WebImzoSecretKey.IsNullOrEmpty())
            {
                await PostToIMZOAndSentUrl(contract);
            }

            var url = (_wbImzoConfig.Api.Contains("api/") ? _wbImzoConfig.Api.Replace("api/", null) : _wbImzoConfig.Api) + $"app?requestId={contract.WebImzoRequestId}&secretKey={contract.WebImzoSecretKey}";

            return url;

        }
        public WbImzoCreateSignRequestDto CreatDtoForRequestSign(ServiceDeed contract)
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
                DocumentType = "Xizmatlar dalolatnomasi",
                DocumentId = contract.Id,
                ApiKey = _wbImzoConfig.ApiKey,
                Title = contract.DocNumber,
                IsForceCreate = true,
                TableId = TableIdConst.DOC_SERVICE_DEED,
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
}