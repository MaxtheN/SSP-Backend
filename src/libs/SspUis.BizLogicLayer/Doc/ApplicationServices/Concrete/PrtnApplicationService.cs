using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using OpenXmlPowerTools;
using SspUis.BizLogicLayer.DistrictServices;
using SspUis.BizLogicLayer.Doc.PrtnContractServices;
using SspUis.BizLogicLayer.RegionServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.OnlineMahalla;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.ApplicationServices
{
    public partial class ApplicationService
    {
        public async Task<PrtnApplicationDto> GetPrtnApplication()
        {
            var davAktivContractor = await _contractorService.GetByInnFromDavAktiv(_authService.Contractor.Inn);
            var checkContractor = _notBudgetContractorRepository.ByInn(_authService.Contractor.Inn);
            if (checkContractor != null || davAktivContractor != null)
            {
                AddError("Tashkilot ustav kapitalida davlat ulushi mavjud / Имеется государственная доля в уставном капитале организации");
                return null;
            }

            if (_authService.Contractor.RegistrationDate.AddYears(2) > DateTime.Now.AsDateOnly())
            {
                AddError("Tashkilot davlat ro'yxatidan o'tish sanasi 2 yildan kam / Дата государственной регистрации организации менее 2 лет");
                return null;
            }

            if (Repository.AllAsQueryable.Any(a => a.StatusId != StatusIdConst.DELETED && a.StatusId != StatusIdConst.REJECTED && a.StatusId != StatusIdConst.CANCELED && a.StatusId != StatusIdConst.REVOKED && a.ApplicationTypeId == ApplicationTypeIdConst.PARTNER))
            {
                AddError("Ariza allaqachon yaratilgan / Заявка уже создана");
                return null;
            }

            if (Repository.AllAsQueryable.Any(a => new int[] { StatusIdConst.SENT_FOR_REVIEW, StatusIdConst.SENT, StatusIdConst.SENDING, StatusIdConst.ACCEPTED, StatusIdConst.WAITING, StatusIdConst.SENT_FOR_EXPERTISE, StatusIdConst.NOT_PASS_EXPERTISE, StatusIdConst.PASS_EXPERTISE }.Contains(a.StatusId) && a.ApplicationTypeId == ApplicationTypeIdConst.PARTNER))
            {
                AddError("Ariza allaqachon yaratilgan / Заявка уже создана");
                return null;
            }

            var region = _unitOfWork.RegionRepository.ById<RegionListDto>(_authService.Contractor.RegionId);
            var district = _unitOfWork.DistrictRepository.ById<DistrictListDto>(_authService.Contractor.DistrictId);
            return new PrtnApplicationDto
            {
                Contractor = _authService.Contractor.FullName,
                ContractorInn = _authService.Contractor.Inn,
                DocOn = DateTime.Now.AsDateOnly(),
                ContractorId = _authService.Contractor.Id,
                ContractorPositionName = "Директор",
                DistrictId = _authService.Contractor.DistrictId,
                RegionId = _authService.Contractor.RegionId,
                DistrictName = district.FullName,
                RegionName = region.FullName,
                ApplicationTypeId = ApplicationTypeIdConst.PARTNER,
                DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_APPLICATION, 1).Item2,
                CanEdit = true
            };
        }

        public PrtnApplicationDto GetPrtnApplication(long id)
        {
            //var dto = Repository.ById<PrtnApplicationDto>(id,false);
            var dto = Repository.CrudServices.ReadManyNoTracked<PrtnApplicationDto>()
                                             .FirstOrDefault(application => application.Id == id);
            if (dto == null)
                return null;

            dto.Graphs = dto.Graphs.OrderBy(a => a.YearIn)
                .ThenBy(a => a.MonthIn)
                .ToList();

            var applicationMfyLog = _unitOfWork.Context.Set<MfyApplicationLog>()
                .FirstOrDefault(a => a.ApplicationId == id);



            dto.CanAccept = StatusIdConst.CanApplicationApplyStatus(
                dto.StatusId,
                StatusIdConst.ACCEPTED)
                            && _authService.HasPermission(ModuleCode.ApplicationAccept) && dto.StatusId != StatusIdConst.SENT_FOR_REVIEW;

            dto.CanReject = StatusIdConst.CanApplicationApplyStatus(
                dto.StatusId,
                StatusIdConst.REJECTED)
                            && _authService.HasPermission(ModuleCode.ApplicationReject);

            if (applicationMfyLog != null)
            {
                if (applicationMfyLog.IsAccepted)
                    dto.CanReject = false;
                else
                    dto.CanAccept = false;
            }


            dto.CanSendForReview = StatusIdConst.CanApplicationApplyStatus(
                dto.StatusId,
                StatusIdConst.SENT_FOR_REVIEW);

            dto.CanEdit = StatusIdConst.CanApplicationApplyStatus(
                dto.StatusId,
                StatusIdConst.MODIFIED);

            return dto;
        }

        public HaveId<long> CreatePrtnApplication(CreatePrtnApplicationDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var entity = Repository.Create(dto, ent => PrtnValidation(dto, ent));
                  
                    CombineStatuses(Repository);
                    if (HasErrors)
                        return null;
                    entity.PrtnApplication.IsRead = false;
                    UnitOfWork.Save();

                    var res = CreateDocumentChangeLog(entity.Id, "PrtnApplication");
                    if (IsValid)
                    {
                        transaction.Commit();
                        return HaveId.Create(entity.Id);
                    }
                }
                catch (Exception ex)
                {
                    AddError($"{ex.Message}: {ex.InnerException}");
                }
            }
            return null;
        }

        public Boolean CanCreateApplication(string inn)
        {
            //var dto = Repository.CrudServices.ReadManyNoTracked<PrtnApplicationDto>()
            //                                 .Where(a => a.ContractorInn == inn && !new int[] { StatusIdConst.DELETED, StatusIdConst.REJECTED, StatusIdConst.CANCELED, StatusIdConst.REVOKED }.Contains(a.StatusId)).ToList();

            var dto = _unitOfWork.Context.Set<Application>()
                                         .Include(a => a.Contractor)
                                         .Where(a => a.Contractor.Inn == inn && a.ApplicationTypeId == ApplicationTypeIdConst.PARTNER && !new int[] { StatusIdConst.DELETED, StatusIdConst.REJECTED, StatusIdConst.CANCELED, StatusIdConst.REVOKED }.Contains(a.StatusId))
                                         .ToList();

            if (dto.Any())
                return false;
            return true;
        }

        public void UpdatePrtnApplication(UpdatePrtnApplicationDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var entity = Repository.Update(dto, ent => PrtnValidation(dto, ent));
                CombineStatuses(Repository);
                if (IsValid)
                    UnitOfWork.Save();

                var res = CreateDocumentChangeLog(entity.Id, "PrtnApplication");

                if (IsValid)
                {
                    transaction.Commit();
                }
            }
        }

        public void Accept(long id)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();
            try
            {
                Repository.AllAsQueryable.Lock(id);
                var dto = new UpdateStatusApplicationDlDto { Id = id, StatusId = StatusIdConst.ACCEPTED };

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplicationApplyStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");

                    ent.PrtnApplication.IsRead = false;
                });
                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                var res = CreateDocumentChangeLog(id, "PrtnApplication");
                if (IsValid && canCommit)
                {
                    transaction.Commit();
                }
            }
            finally
            {
                if (canCommit) transaction.Dispose();
            }
        }

        public void Accept(AcceptStatusPrtnApplicationDto dlDto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<Application>().Lock(dlDto.Id);

                var entity = Repository.ById(dlDto.Id);
                int? organizationId = null;
                IQueryable<Organization> orgQuery = _unitOfWork.OrganizationRepository.AllAsQueryable.IsActive();
                if (entity.PrtnApplication.PrtnContractTypeId == PrtnContractTypeIdConst._50_100)
                {
                    orgQuery = orgQuery.Where(a => a.DistrictId == (entity.PrtnApplication.ChooseLocation ? entity.PrtnApplication.ChoosedDistrictId : entity.DistrictId)
                        && a.RegionId == (entity.PrtnApplication.ChooseLocation ? entity.PrtnApplication.ChoosedRegionId : entity.RegionId)
                        && a.SignOrganizationTypeId == SignOrganizationTypeIdConst.DISTRICT
                    );
                }
                else if (entity.PrtnApplication.PrtnContractTypeId == PrtnContractTypeIdConst._101_200)
                {
                    orgQuery = orgQuery.Where(a => a.RegionId == (entity.PrtnApplication.ChooseLocation ? entity.PrtnApplication.ChoosedRegionId : entity.RegionId)
                        && a.SignOrganizationTypeId == SignOrganizationTypeIdConst.REGION
                    );
                }
                else if (entity.PrtnApplication.PrtnContractTypeId == PrtnContractTypeIdConst._201__)
                {
                    orgQuery = orgQuery.Where(a => a.SignOrganizationTypeId == SignOrganizationTypeIdConst.MINISTRY);
                }
                organizationId = orgQuery.FirstOrDefault()?.Id;

                var dto = dlDto;

                dto.StatusId = organizationId.HasValue ? StatusIdConst.ACCEPTED : StatusIdConst.FULL_FILLED;


                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplicationApplyStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");

                    ent.PrtnApplication.IsRead = false;
                });
                CombineStatuses(Repository);
                if (HasErrors)
                    return;

                if (organizationId.HasValue)
                {
                    var createPrtnContractDlDto = new CreatePrtnContractDlDto
                    {
                        ApplicationId = entity.Id,
                        ContractorId = entity.ContractorId.Value,
                        DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_PRTN_CONTRACT, 1).Item2,
                        DocOn = DateTime.Now.AsDateOnly(),
                        NewVacanciesCount = entity.PrtnApplication.NewVacanciesCount,
                        OrganizationId = organizationId.Value,
                        PrtnContractTypeId = entity.PrtnApplication.PrtnContractTypeId
                    };

                    var prtnContractEntity = _unitOfWork.PrtnContractRepository.Create(createPrtnContractDlDto);
                    CombineStatuses(_unitOfWork.PrtnContractRepository);
                    if (HasErrors)
                        return;

                    #region Set signers
                    var prtnContractTypeTables = _unitOfWork.PrtnContractTypeRepository.ById(prtnContractEntity.PrtnContractTypeId)?.Tables;

                    List<PrtnContractTypeTable> signers = prtnContractTypeTables.ToList();

                    bool existCurrentRegion = prtnContractTypeTables.Any(a => a.RegionId == _authService.Organization.RegionId);

                    if (existCurrentRegion)
                        signers = prtnContractTypeTables.Where(a => a.SignOrganizationTypeId == SignOrganizationTypeIdConst.BUSINESSMAN
                            || a.RegionId == _authService.Organization.RegionId
                        ).ToList();
                    else
                        signers = prtnContractTypeTables.Where(a => a.SignOrganizationTypeId == SignOrganizationTypeIdConst.BUSINESSMAN
                            || !a.RegionId.HasValue
                        ).ToList();

                    foreach (var signer in signers.OrderBy(a => a.OrderNumber))
                    {
                        if (signer.SignOrganizationTypeId == SignOrganizationTypeIdConst.BUSINESSMAN)
                            prtnContractEntity.Signs.Add(new PrtnContractSign
                            {
                                IsSigned = false,
                                PrtnContractTypeTableId = signer.Id,
                            });
                        else if (signer.SignOrganizationTypeId == SignOrganizationTypeIdConst.MINISTRY)
                        {
                            int signerOrganizationId = organizationId.Value;
                            if (entity.PrtnApplication.PrtnContractTypeId == PrtnContractTypeIdConst._201__)
                            {
                                // Agar vazirlik qaysi tashkilot ekanligi ko'rsatilmagan bo'sa, oshibka beradi
                                if (!signer.OrganizationId.HasValue)
                                {
                                    AddError("Vazirliklar shartnoma turida hali ko'rsatilmagan");
                                    return;
                                }

                                signerOrganizationId = signer.OrganizationId.Value;
                            }

                            var signerOrganization = _unitOfWork.OrganizationRepository.ById(signerOrganizationId);
                            var organizationSignId = signerOrganization.Signs
                                    .FirstOrDefault(a => a.PrtnContractTypeTableId == signer.Id && !a.ExpireOn.HasValue);
                            if (organizationSignId == null)
                            {
                                var orgName = signerOrganization.Translates.AsQueryable().FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? signerOrganization.FullName;
                                AddError($"{orgName} tashkilot sozlamalarida vazir ko'rsatilmagan / Министр не указан в настройках организации {orgName}");
                                return;
                            }

                            prtnContractEntity.Signs.Add(new PrtnContractSign
                            {
                                IsSigned = false,
                                PrtnContractTypeTableId = signer.Id,
                                OrganizationId = signer.OrganizationId,
                                OrganizationSignId = organizationSignId.Id,
                            });
                        }
                        else
                        {
                            int signerOrganizationId = organizationId.Value;
                            if (prtnContractEntity.PrtnContractTypeId == PrtnContractTypeIdConst._201__)
                            {
                                // Agar vazirlik qaysi tashkilot ekanligi ko'rsatilmagan bo'sa, oshibka beradi
                                if (!signer.OrganizationId.HasValue)
                                {
                                    AddError("Vazirliklar shartnoma turida hali ko'rsatilmagan");
                                    return;
                                }

                                signerOrganizationId = signer.OrganizationId.Value;
                            }

                            var signerOrganization = _unitOfWork.OrganizationRepository.ById(signerOrganizationId);
                            var organizationSignId = signerOrganization.Signs
                                .FirstOrDefault(a => a.PrtnContractTypeTableId == signer.Id && !a.ExpireOn.HasValue);
                            if (organizationSignId == null)
                            {
                                var orgName = signerOrganization.Translates.AsQueryable().FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? signerOrganization.FullName;
                                AddError($"{orgName} tashkilot sozlamalarida vazir ko'rsatilmagan / Министр не указан в настройках организации {orgName}");
                                return;
                            }

                            prtnContractEntity.Signs.Add(new PrtnContractSign
                            {
                                IsSigned = false,
                                PrtnContractTypeTableId = signer.Id,
                                OrganizationId = signerOrganizationId,
                                OrganizationSignId = organizationSignId.Id,
                            });
                        }
                    }
                    #endregion
                }

                var res = CreateDocumentChangeLog(dlDto.Id, "PrtnApplication");
                if (IsValid && canCommit)
                    transaction.Commit();
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }

        public async Task Reject(RejectStatusPrtnApplicationDto dto)
        {
            dto.IsRead = false;
            var canCommit = _unitOfWork.CurrentTransaction == null;

            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<Application>().Lock(dto.Id);

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplicationApplyStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");

                    ent.PrtnApplication.IsRead = false;
                });

                CombineStatuses(Repository);

                if (HasErrors)
                    return;

                var application = _unitOfWork.Context.Set<Application>().FirstOrDefault(x => x.Id == dto.Id);

                await _onlineMahallaService.UpdateApplicationStatus(new MahallaApplicationStatusUpdateRequestDto()
                {
                    ApplicationId = application.Id2.ToString(),
                    Status = "rejected"
                });

                CombineStatuses(_onlineMahallaService);

                if (HasErrors)
                    return;

                var res = CreateDocumentChangeLog(id: dto.Id, message: "PrtnApplication");

                if (IsValid && canCommit)
                    transaction.Commit();
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }

        public void Cancel(CancelStatusPrtnApplicationDto dlDto)
        {
            var canCommit = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

            try
            {
                _unitOfWork.Context.Set<Application>().Lock(dlDto.Id);

                var dto = new UpdateStatusApplicationDlDto
                {
                    Id = dlDto.Id,
                    StatusId = StatusIdConst.CANCELED
                };

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplicationApplyStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");


                    ent.PrtnApplication.IsRead = false;
                });

                CombineStatuses(Repository);

                if (HasErrors)
                    return;
                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(id: dlDto.Id, message: "PrtnApplication");

                if (IsValid && canCommit)
                    transaction.Commit();
            }
            finally
            {
                if (canCommit)
                    transaction.Dispose();
            }
        }

        public async Task SentForReview(long id, string userIp = null, string userAgent = null)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    Repository.AllAsQueryable.Lock(id);

                    var dto = new UpdateStatusApplicationDlDto { Id = id, StatusId = StatusIdConst.SENT_FOR_REVIEW};

                    var entity = Repository.UpdateStatus(dto, ent =>
                    {
                        if (!StatusIdConst.CanApplicationApplyStatus(ent.StatusId, dto.StatusId))
                            AddError("Имкони йўқ / Нет доступа");

                        if (!ent.PrtnApplication.MfyId.HasValue)
                            AddError("МФЙ танланг / Выберите СГМ");

                        ent.PrtnApplication.IsRead = false;
                    });

                    CombineStatuses(Repository);
                    if (HasErrors)
                        return;
                    _unitOfWork.Save();

                    var res = CreateDocumentChangeLog(id,
                        userIp: userIp,
                        userAgent: userAgent);
                    if (IsValid)
                    {
                        try
                        {
                            if (!_systemConf.IsTest)
                            {
                                var regionSoato = _unitOfWork.RegionRepository.ById(entity.PrtnApplication.ChooseLocation ? entity.PrtnApplication.ChoosedRegionId.Value : entity.RegionId).Soato;
                                var districtSoato = _unitOfWork.DistrictRepository.ById(entity.PrtnApplication.ChooseLocation ? entity.PrtnApplication.ChoosedDistrictId.Value : entity.DistrictId).SoatoOfMfy;

                                var mfyExternalId = _unitOfWork.MfyRepository.ById(entity.PrtnApplication.MfyId.Value).ExternalId;
                                // _authService dan omaganimiz sababi, RABBITda NULL boladi
                                var contractor = _unitOfWork.ContractorRepository.ById(entity.ContractorId.Value);

                                var prtnApplicationGraphs = entity.PrtnApplication?.Graphs;


                                var responseResult = await PostApplication(new OnlineMahallaRequestDto
                                {
                                    ApplicationDate = entity.DocOn.ToDateTime(TimeOnly.MinValue),
                                    ApplicationNum = entity.DocNumber,
                                    ApplicationId = entity.Id2.ToString(),

                                    OrgTin = long.Parse(contractor.Inn),
                                    OrgName = contractor.FullName,
                                    OrgPhoneNumber = contractor.BusinessmanUserInContractors.FirstOrDefault()?.BusinessmanUser.UserName ?? "",

                                    OblSoato = regionSoato,
                                    AreaSoato = districtSoato,
                                    DistrictId = mfyExternalId,

                                    ContractType = entity.PrtnApplication.PrtnContractTypeId,
                                    StaffCount = entity.PrtnApplication.NewVacanciesCount,
                                    Graphs = prtnApplicationGraphs.Any() ?
                                    prtnApplicationGraphs.Select(pag => new OnlineMahallaRequestGraphDto
                                    {
                                        YearIn = pag.YearIn,
                                        MonthIn = pag.MonthIn,
                                        NewVacanciesCount = pag.NewVacanciesCount
                                    }).ToList() :
                                    new()
                                },
                                entity.Id);
                                
                                if (HasErrors)
                                    return;

                                if (responseResult.Status == 200)
                                {
                                    Repository.SetSend(entity.Id, responseResult.Data.Insert.Id);
                                    CombineStatuses(Repository);
                                    
                                    if (IsValid)
                                        transaction.Commit();
                                }
                            }
                            else
                            {
                                entity.StatusId = StatusIdConst.SENT;
                                Repository.SetSend(entity.Id);
                                CombineStatuses(Repository);
                                if (IsValid)
                                    transaction.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            entity.StatusId = StatusIdConst.SENT;
                            entity.Message = $"{ex.Source}: {ex.Message}. {ex.InnerException}";
                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    AddError($"{ex.Message}: {ex.InnerException}");
                }
            }
        }
        private void FullFill(long id, string? message = null)
        {
           
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                Repository.AllAsQueryable.Lock(id);

                var dto = new UpdateStatusApplicationDlDto
                {
                    Id = id,
                    StatusId = StatusIdConst.FULL_FILLED
                    
                };

                Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplicationApplyStatus(ent.StatusId, dto.StatusId))
                        AddError("Имкони йўқ / Нет доступа");


                    ent.PrtnApplication.IsRead = false;
                });

                CombineStatuses(Repository);

                if (HasErrors)
                    return;

                _unitOfWork.Save();

                var res = CreateDocumentChangeLog(id, message: message);

                if (IsValid)
                    transaction.Commit();
            }

        }

        public Stream SaveAsExecel(PrtnDocumentSortFilterOptions dto)
        {
            //var data = GetList(dto).ClaimThemeCount
            //                .Take(1000000)
            //                .ToList();
            var data = Repository.ReadAsNoTracked<ApplicationListDto>()
                            .Where(a => a.ApplicationTypeId == ApplicationTypeIdConst.PARTNER)
                            .SortFilter(dto)
                            .Filter(dto.Filters)
                            .Take(1000000)
                            .ToList();

            MemoryStream result = new MemoryStream();
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.APPLICATION_LIST));

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
                    ws.Cells[currentRow, column++].Value = item.IsLastOffer ? "Ha" : "Yo'q";
                    ws.Cells[currentRow, column++].Value = item.Id;
                    ws.Cells[currentRow, column++].Value = item.DocNumber;
                    ws.Cells[currentRow, column++].Value = item.DocOn.ToString(Constants.DATE_FORMAT);
                    ws.Cells[currentRow, column++].Value = item.ContractorRegion;
                    ws.Cells[currentRow, column++].Value = item.ContractorDistrict;
                    ws.Cells[currentRow, column++].Value = item.ChooseLocation ? "Ha" : "Yo'q";
                    ws.Cells[currentRow, column++].Value = item.ChoosedRegion;
                    ws.Cells[currentRow, column++].Value = item.ChoosedDistrict;
                    ws.Cells[currentRow, column++].Value = item.ContractorPhoneNumber;
                    ws.Cells[currentRow, column++].Value = item.Director;
                    ws.Cells[currentRow, column++].Value = item.Mfy;
                    ws.Cells[currentRow, column++].Value = item.PrtnApplicationNewVacanciesCount;
                    ws.Cells[currentRow, column++].Value = item.PrtnContractType;
                    ws.Cells[currentRow, column++].Value = item.ContractorInn;
                    ws.Cells[currentRow, column++].Value = item.Contractor;
                    ws.Cells[currentRow, column++].Value = item.OkedCode;
                    ws.Cells[currentRow, column++].Value = item.Oked;
                    ws.Cells[currentRow, column++].Value = item.Organization;
                    ws.Cells[currentRow, column++].Value = item.Status;
                    ws.Cells[currentRow, column++].Value = item.PrtnContractStatus;
                    ws.Cells[currentRow, column++].Value = item.PrtnSertificateStatus;
                    currentRow++;
                }
                ws.DeleteRow(importRow.Start.Row);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }
            result.Position = 0;
            return result;
        }

        private async Task Validation<TDto>(ApplicationDlDto<TDto> dto, Application entity)
            where TDto : ApplicationDlDto<TDto>
        {
            var query = Repository.AllAsQueryable;

            if (entity != null)
            {
                query = query.Where(a => a.Id != entity.Id);

                if (!StatusIdConst.CanEditStatuses.Contains(entity.StatusId))
                    AddError("Таҳрирлаш мумкин эмас / Невозможно редактировать");
            }

            var davAktivContractor = await _contractorService.GetByInnFromDavAktiv(_authService.Contractor.Inn);
            var checkContractor = _notBudgetContractorRepository.ByInn(_authService.Contractor.Inn);
            if (checkContractor != null || davAktivContractor != null)
                AddError("Tashkilot ustav kapitalida davlat ulushi mavjud / Имеется государственная доля в уставном капитале организации");

            if (_authService.Contractor.RegistrationDate.AddYears(2) > DateOnly.FromDateTime(DateTime.Now))
                AddError("Tashkilot davlat ro'yxatidan o'tish sanasi 2 yildan kam / Дата государственной регистрации организации менее 2 лет");
        }

        private void PrtnValidation<TDto>(ApplicationDlDto<TDto> dto, Application entity)
            where TDto : ApplicationDlDto<TDto>
        {
            var query = Repository.AllAsQueryable;

            if (entity != null && entity.Id != 0)
            {
                query = query.Where(a => a.Id != entity.Id);

                if (!StatusIdConst.CanEditStatuses.Contains(entity.StatusId))
                    AddError("Таҳрирлаш мумкин эмас / Невозможно редактировать");
            }

            if (dto is IPrtnApplicationDlDto _dto)
            {
                if (_dto.MfyId == 0)
                    AddError("МФЙ танланг / Выберите СГМ");

                if (_dto.ChooseLocation && (!_dto.ChoosedDistrictId.HasValue || !_dto.ChoosedRegionId.HasValue))
                    AddError("Iltimos, ariza yuborilayotgan viloyat va tumanni tanlang / Пожалуйста, выберите регион и район");

                bool existApplication = query
                    .Any(a => a.ApplicationTypeId == ApplicationTypeIdConst.PARTNER
                        && a.StatusId != StatusIdConst.DELETED
                        && a.StatusId != StatusIdConst.REJECTED
                        && a.StatusId != StatusIdConst.CANCELED
                        && a.StatusId != StatusIdConst.FAILED
                        );
                if (existApplication)
                    AddError("Ariza mavjud / Заявления уже создано");

                var existApplicationWithNewDocNumber = query
                    .Where(a => a.DocNumber == dto.DocNumber);

                if (entity.Id == 0 &&
                    existApplicationWithNewDocNumber.Any())
                    AddError("Bunday ariza raqami bilan ariza mavjud / Заявления с таким номером уже существует");
                else if (entity.Id != 0 && existApplicationWithNewDocNumber.Any(a => a.Id != entity.Id))
                    AddError("Bunday ariza raqami bilan ariza mavjud / Заявления с таким номером уже существует");
                {
                    var prtnContractType = _unitOfWork.PrtnContractTypeRepository.ById(_dto.PrtnContractTypeId);
                    if (prtnContractType.EmployeeRangeFrom > _dto.NewVacanciesCount
                        || prtnContractType.EmployeeRangeTo.HasValue && prtnContractType.EmployeeRangeTo.Value < _dto.NewVacanciesCount)
                    {
                        AddError($"Бўш иш ўринлари сони [{prtnContractType.EmployeeRangeFrom} - {prtnContractType.EmployeeRangeTo?.ToString() ?? ""}] оралиқда бўлиши керак / Количество вакансий должно быть в диапазоне [{prtnContractType.EmployeeRangeFrom} - {prtnContractType.EmployeeRangeTo?.ToString() ?? ""}].");
                    }

                    if (_dto.NewVacanciesCount != _dto.Graphs.Sum(a => a.NewVacanciesCount))
                        AddError("Grafikda yaratilgan ish o'rinlar soni teng emas / Количество созданных рабочих мест не равно на графике ");
                }
            }
        }

        public HaveId<long> ChangeStatusMfyApplication(MfyApplicationStateDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var application = _unitOfWork.Context.Set<Application>().FirstOrDefault(a => a.Id2 == dto.Id);

                if (HasErrors || application == null)
                {
                    AddError($"Заявление с таким id = {dto.Id} не найдено в системе");
                    transaction.Rollback();
                    return null;
                }

                try
                {
                    User user = null;

                    _authService.ResetUserName("mahalla");

                    var applicationId = application.Id;

                    if (_unitOfWork.Context.Set<MfyApplicationLog>().Any(a => a.ApplicationId2 == dto.Id))
                    {
                        AddError("Ответ на этот запрос уже отправлен");
                        return null;
                    }

                    if (dto.IsAccepted)
                    {
                        var acceptDto = new AcceptStatusPrtnApplicationDto()
                        {
                            Id = applicationId,
                            Message = dto.Details,
                            Offer = "Қабул қилинди"
                        };

                        application.PrtnApplication.StatusChangeExpireOn = DateTime.Now;
                        application.PrtnApplication.IsRead = false;
                        Accept(acceptDto);

                        if (HasErrors)
                            FullFill(acceptDto.Id, acceptDto.Message);
                    }
                    else
                    {
                        var acceptDto = new RejectStatusPrtnApplicationDto()
                        {
                            Id = applicationId,
                            Message = dto.Details,
                            Offer = "Рад этилди"
                        };

                        Reject(acceptDto);

                        if (HasErrors)
                            FullFill(acceptDto.Id, acceptDto.Message);
                    }

                    if (IsValid)
                    {
                        var savedLogResult = AddMfyApplicationLog(dto, application.Id);

                        if (IsValid)
                        {
                            transaction.Commit();
                            return savedLogResult;
                        }
                        return null;
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    foreach (var entry in _unitOfWork.Context.ChangeTracker.Entries())
                        entry.State = EntityState.Detached;

                    FullFill(application.Id, dto.Details);

                    var savedLogResult = AddMfyApplicationLog(dto, application.Id);

                    return savedLogResult;
                }
                return null;
            }
        }

        private HaveId<long> AddMfyApplicationLog(MfyApplicationStateDto dto, long applicationId)
        {
            var createEntity = new MfyApplicationLog
            {
                ApplicationId = applicationId,
                ApplicationId2 = dto.Id,
                IsAccepted = dto.IsAccepted,
                Details = dto.Details,
                FileUrl = dto.FileUrl,
                ConclusingPersonFio = dto.ConclusingPersonFio,
                ConclusingPersonInn = dto.ConclusingPersonInn,
                ConclusingPersonPhone = dto.ConclusingPersonPhone,
                ConclusingPersonPosition = dto.ConclusingPersonPosition,
                CreatedAt = DateTime.Now
            };
            //TODO ...Test qilish kerak mahalla post testviy bersa
            if (dto.Data != null)
            {
                var data = dto.Data;
                createEntity.TotalAmount = Convert.ToDecimal(data.TotalAmount);
                createEntity.PersonalAmount = Convert.ToDecimal(data.PersonalAmount);
                createEntity.BankLoanAmount = Convert.ToDecimal(data.BankLoansAmount);
                createEntity.ForeignInvestmentAmount = Convert.ToDecimal(data.ForeignInvestmentAmount);
                createEntity.NewJobCount = data.NewJobsCount;
                createEntity.ProjectStartDate = Convert.ToDateTime(data.ProjectStartDate).AsDateOnly();
                createEntity.ProjectAddress = data.ProjectAddress;

                createEntity.IsProjectFinished = data.IsProjectFinished.HasValue && data.IsProjectFinished.Value == 1 ? true : false;
                createEntity.IsExistingProjectExpanded = data.IsExistingProjectExpanded.HasValue && data.IsExistingProjectExpanded.Value == 1 ? true : false;
                createEntity.IsInFurnishingProccess = data.IsInFurnishingProccess.HasValue && data.IsInFurnishingProccess.Value == 1 ? true : false;
                createEntity.IsConstructionStarted = data.IsConstructionStarted.HasValue && data.IsConstructionStarted.Value == 1 ? true : false;
                createEntity.ThereIsEmptySpaceButNotStarted = data.ThereIsEmptySpaceButNotStarted.HasValue && data.ThereIsEmptySpaceButNotStarted.Value == 1 ? true : false;
                createEntity.ThereIsNoEmptySpaceForProject = data.ThereIsNoEmptySpaceForProject.HasValue && data.ThereIsNoEmptySpaceForProject.Value == 1 ? true : false;
            }

            var result = _unitOfWork.Context.Add(createEntity);

            var appEntity = _unitOfWork.Context.Set<Application>().Include(a => a.PrtnApplication).FirstOrDefault(a => a.Id == applicationId);

            if (appEntity != null)
                appEntity.PrtnApplication.HasBeenAnswered = true;

            _unitOfWork.Context.SaveChanges();

            return HaveId.Create(result.Entity.Id);
        }

        public async Task<OnlineMahallaResponseDto<OnlineMahallaPostDataDto>> PostApplication(OnlineMahallaRequestDto dto, long? id = null)
        {
            var log = new CreateApiRequestLogDlDto
            {
                DocumentId = id,
                TableId = TableIdConst.DOC_APPLICATION,
                UserId = (int)_authService.UserId,
                UserInfo = _authService.User.ToString() ?? "",
                ResponseAt = DateTime.Now
            };

            try
            {
                var result = await _onlineMahallaService.PostApplication(dto);

                CombineStatuses(_onlineMahallaService);

                log.IsSuccess = IsValid;

                if (result != null)
                {
                    log.RequestContent = result?.Message;
                    log.ResponseStatus = result?.Status;
                    log.RequestUrl = result.Path;
                }

                if (IsValid)
                {
                    log.ResponseContent = JsonConvert.SerializeObject(result);
                    return result;
                }
                else
                    log.Exception = _onlineMahallaService.GetAllErrors();
            }
            catch (Exception ex)
            {
                log.Exception = $"{ex.Message}: {ex.InnerException}";
                //throw ex;
            }
            finally
            {
                _apiRequestLogRepository.Create(log);
                _unitOfWork.Save();
            }
            return null;
        }

        public CheckApplicationStatusDto CheckApplicationStatus(string applicationId)
        {
            var application = _unitOfWork.Context.Set<Application>()
                .Include(application => application.PrtnContract)
                    .ThenInclude(contract => contract.PrtnCertificate)
                        .ThenInclude(certificate => certificate.Status)
                .Include(application => application.PrtnContract)
                    .ThenInclude(contract => contract.Status)
                .Select(application => new
                {
                    application.Id2,
                    ApplicationStatus = application.Status.FullName,
                    ApplicationStatusId = application.StatusId,
                    ContractStatus = application.PrtnContract != null ? application.PrtnContract.Status.FullName : null,
                    ContractStatusId = application.PrtnContract != null ? application.PrtnContract.StatusId : 0,
                    CertificateStatus = application.PrtnContract != null && application.PrtnContract.PrtnCertificate != null ? application.PrtnContract.PrtnCertificate.Status.FullName : null,
                    CertificateStatusId = application.PrtnContract != null && application.PrtnContract.PrtnCertificate != null ? application.PrtnContract.PrtnCertificate.StatusId : 0
                }).FirstOrDefault(a => a.Id2.ToString() == applicationId);

            if (application == null)
            {
                AddError("Неверный идентификатор");
                return null;
            }


            var result = new CheckApplicationStatusDto()
            {
                ApplicationStatusId = application.ApplicationStatusId,
                ApplicationStatusName = application.ApplicationStatus,
                ContractStatusId = application.ContractStatusId,
                ContractStatusName = application.ContractStatus,
                CertificateStatusId = application.CertificateStatusId,
                CertificateStatusName = application.CertificateStatus
            };

            return result;
        }

        public MfyApplicationLogDto GetMfyApplication(Guid applicationId2)
        {
            //var mfyApplication = _crudServices.ReadManyNoTracked<MfyApplicationLogDto>().FirstOrDefault(a => a.ApplicationId == applicationId2);

            var query = _unitOfWork.Context.Set<MfyApplicationLog>()
               .Select(a => new MfyApplicationLogDto
               {
                   Id = a.Id,
                   ApplicationId2 = a.ApplicationId2,
                   Details = a.Details,
                   ConclusingPersonInn = a.ConclusingPersonInn,
                   ConclusingPersonPosition = a.ConclusingPersonPosition,
                   ConclusingPersonFio = a.ConclusingPersonFio,
                   ConclusingPersonPhone = a.ConclusingPersonPhone,
                   FileUrl = a.FileUrl,
                   CreatedAt = a.CreatedAt,
                   IsAccepted = a.IsAccepted,
                   TotalAmount = a.TotalAmount,
                   PersonalAmount = a.PersonalAmount,
                   BankLoansAmount = a.BankLoanAmount,
                   ForeignInvestmentAmount = a.ForeignInvestmentAmount,
                   IsConstructionStarted = a.IsConstructionStarted,
                   IsExistingProjectExpanded = a.IsExistingProjectExpanded,
                   IsInFurnishingProccess = a.IsInFurnishingProccess,
                   IsNewProjectDone = a.IsNewProjectDone,
                   IsProjectFinished = a.IsProjectFinished,
                   NewJobsCount = a.NewJobCount,
                   ProjectAddress = a.ProjectAddress,
                   ProjectStartDate = a.ProjectStartDate,
                   ThereIsEmptySpaceButNotStarted = a.ThereIsEmptySpaceButNotStarted,
                   ThereIsNoEmptySpaceForProject = a.ThereIsNoEmptySpaceForProject
               });

            var mfyApplication = query.FirstOrDefault(a => a.ApplicationId2.ToString() == applicationId2.ToString());

            if (mfyApplication == null)
                return null;

            return mfyApplication;
        }

        //This is temporary code for once only
        public List<long> PostAllOldMfyApplications()
        {
            var con = _authService.Contractor;
            return _unitOfWork.Context.Set<Application>()
                .Include(a => a.PrtnApplication)
                .Include(a => a.Contractor)
                .Where(a => a.PrtnApplication.MfyId != null
                    && new int[] { StatusIdConst.SENDING }.Contains(a.StatusId)
                    && a.PrtnContract == null
                )
                .Select(a => a.Id)
                .ToList();
        }
        public async Task SentToMahalla(long id, string userIp = null, string userAgent = null)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    //Repository.AllAsQueryable.Lock(id);

                    var application = _unitOfWork.Context.Set<Application>().Include(c => c.Contractor).ThenInclude(b => b.BusinessmanUserInContractors).ThenInclude(user => user.BusinessmanUser).Include(r => r.Region).ThenInclude(d => d.Districts).Include(prtn => prtn.PrtnApplication).ThenInclude(mfy => mfy.Mfy).Include(prtn => prtn.PrtnApplication).ThenInclude(g => g.Graphs).FirstOrDefault(a => a.Id == id);


                    var dto = new UpdateStatusApplicationDlDto { Id = id, StatusId = StatusIdConst.SENT_FOR_REVIEW};

                    //var entity = Repository.UpdateStatus(dto, ent =>
                    //{
                    //    if (!StatusIdConst.CanApplicationApplyStatus(ent.StatusId, dto.StatusId))
                    //        AddError("Имкони йўқ / Нет доступа");

                       
                    //});

                    if (!application.PrtnApplication.MfyId.HasValue)
                        AddError("МФЙ танланг / Выберите СГМ");

                    CombineStatuses(Repository);
                    if (HasErrors)
                        return;
                    _unitOfWork.Save();

                    var res = CreateDocumentChangeLog(id,
                        userIp: userIp,
                        userAgent: userAgent);
                    if (IsValid)
                    {
                        try
                        {
                            if (!_systemConf.IsTest)
                            {
                                var regionSoato = _unitOfWork.RegionRepository.ById(application.PrtnApplication.ChooseLocation ? application.PrtnApplication.ChoosedRegionId.Value : application.RegionId).Soato;
                                var districtSoato = _unitOfWork.DistrictRepository.ById(application.PrtnApplication.ChooseLocation ? application.PrtnApplication.ChoosedDistrictId.Value : application.DistrictId).SoatoOfMfy;
                                var mfyExternalId = _unitOfWork.MfyRepository.ById(application.PrtnApplication.MfyId.Value).ExternalId;
                                // _authService dan omaganimiz sababi, RABBITda NULL boladi
                                var contractor = _unitOfWork.ContractorRepository.ById(application.ContractorId.Value);

                                var prtnApplicationGraphs = application.PrtnApplication?.Graphs;


                                var responseResult = await PostApplication(new OnlineMahallaRequestDto
                                {
                                    ApplicationDate = application.DocOn.ToDateTime(TimeOnly.MinValue),
                                    ApplicationNum = application.DocNumber,
                                    ApplicationId = application.Id2.ToString(),

                                    OrgTin = long.Parse(contractor.Inn),
                                    OrgName = contractor.FullName,
                                    OrgPhoneNumber = contractor.BusinessmanUserInContractors.FirstOrDefault()?.BusinessmanUser.UserName ?? "",

                                    OblSoato = regionSoato,
                                    AreaSoato = districtSoato,
                                    DistrictId = mfyExternalId,

                                    ContractType = application.PrtnApplication.PrtnContractTypeId,
                                    StaffCount = application.PrtnApplication.NewVacanciesCount,
                                    Graphs = prtnApplicationGraphs.Any() ?
                                    prtnApplicationGraphs.Select(pag => new OnlineMahallaRequestGraphDto
                                    {
                                        YearIn = pag.YearIn,
                                        MonthIn = pag.MonthIn,
                                        NewVacanciesCount = pag.NewVacanciesCount
                                    }).ToList() :
                                    new()
                                },
                                application.Id);
                                if (HasErrors)
                                    return;

                                if (responseResult.Status == 200)
                                {
                                    Repository.SetSend(application.Id, responseResult.Data.Insert.Id);
                                    CombineStatuses(Repository);
                                    if (IsValid)
                                        transaction.Commit();
                                }
                            }
                            else
                            {
                                application.StatusId = StatusIdConst.SENT;
                                Repository.SetSend(application.Id);
                                CombineStatuses(Repository);
                                if (IsValid)
                                    transaction.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            application.StatusId = StatusIdConst.SENT;
                            application.Message = $"{ex.Source}: {ex.Message}. {ex.InnerException}";
                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    AddError($"{ex.Message}: {ex.InnerException}");
                }
            }
        }
        public void Complete(long Id)
        {
            try
            {
                var application = _unitOfWork.Context.Set<Application>().FirstOrDefault(app => app.Id == Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void AddPrtnApplicationStatusChangeExpOn()
        {

            var res = (from prt in _unitOfWork.Context.Set<PrtnApplication>()
                       join mfy in _unitOfWork.Context.Set<MfyApplicationLog>()
                       on prt.ApplicationId equals mfy.ApplicationId into mfyGroup
                       from mfy in mfyGroup.DefaultIfEmpty() 
                       select new
                       {
                           prt,
                           mfy,
                       }).ToList();

            foreach (var item in res)
            {
                item.prt.StatusChangeExpireOn = item.prt.CreatedAt;
                item.prt.PassExpertiseExpireOn = item.mfy?.CreatedAt;
            }

            _unitOfWork.Context.SaveChanges();


            //try
            //{
            //    var result = (from prtApp in _unitOfWork.Context.Set<PrtnApplication>()
            //                  join log in _unitOfWork.Context.Set<DocumentChangeLog>()
            //                  on prtApp.ApplicationId equals log.DocId
            //                  where log.StatusId != 5
            //                  && log.TableId == TableIdConst.DOC_APPLICATION
            //                  && prtApp.Application.ApplicationTypeId == ApplicationTypeIdConst.PARTNER
            //                  select new
            //                  {
            //                      prtApp,
            //                      log
            //                  })
            //        .ToList() // So'rovni xotiraga olib kelish
            //        .GroupBy(x => x.prtApp)
            //        .Select(g => new
            //        {
            //            PrtApp = g.Key,
            //            Logs = g.Select(x => x.log).ToList() // Barcha mos loglarni olish
            //        }).ToList();

            //    int count = 0;
            //    foreach (var item in result)
            //    {
            //        foreach (var log in item.Logs)
            //        {
            //            if (log.DateAt != DateTime.MinValue)
            //            {
            //                if (log.StatusId == StatusIdConst.SENT_FOR_REVIEW  || log.StatusId == StatusIdConst.CREATED)
            //                    item.PrtApp.StatusChangeExpireOn = log.DateAt;
            //                if (log.StatusId == StatusIdConst.ACCEPTED || log.StatusId == StatusIdConst.FULL_FILLED)
            //                    item.PrtApp.PassExpertiseExpireOn = log.DateAt;
            //            }
            //            else if(item.PrtApp.StatusChangeExpireOn == null || item.PrtApp.PassExpertiseExpireOn == null)
            //            {
            //                var result3 = _unitOfWork.Context.Set<MfyApplicationLog>()
            //                    .FirstOrDefault(x => x.ApplicationId == item.PrtApp.ApplicationId);

            //                if (log.StatusId == StatusIdConst.SENT_FOR_REVIEW || log.StatusId == StatusIdConst.CREATED && item.PrtApp.StatusChangeExpireOn == null)
            //                {
            //                    item.PrtApp.StatusChangeExpireOn = item.PrtApp.CreatedAt ;
            //                }
            //                if (log.StatusId == StatusIdConst.ACCEPTED || item.PrtApp.PassExpertiseExpireOn == null)
            //                {
            //                    item.PrtApp.PassExpertiseExpireOn = result3?.CreatedAt;
            //                }
            //            }
            //        }

            //        count++;
            //        if (count == 17)
            //            break;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    AddError($"{ex.StackTrace} ,,,,,,,,,,,,,,,,,,, {ex.InnerException}");
            //    return;
            //}
        }
    }
}