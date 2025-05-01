using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.ServiceLayer.NumberServices;
using WEBASE;
using WEBASE.i18n;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.PrtnCreditDemandServices
{
    public class PrtnCreditDemandService
        : BaseEntityService<long, PrtnCreditDemand, PrtnCreditDemandListDto, PrtnCreditDemandDto, CreatePrtnCreditDemandDlDto, UpdatePrtnCreditDemandDlDto, IPrtnCreditDemandRepository, PrtnCreditDemandSortFilterOptions>
        , IPrtnCreditDemandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly INumberService _numberService;
        private readonly IStorageService _storageService;
        private readonly ICultureHelper _cultureHelper;
        private readonly IDocumentChangeLogService _documentChangeLogService;

        public PrtnCreditDemandService(IUnitOfWork unitOfWork,
            IAuthService authService,
            IStorageService storageService,
            ICultureHelper cultureHelper,
            IDocumentChangeLogService documentChangeLogService,
            INumberService numberService)
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _cultureHelper = cultureHelper;
            _storageService = storageService;
            _documentChangeLogService = documentChangeLogService;
            this._numberService = numberService;
        }

        //protected override IQueryable<PrtnCreditDemandListDto> SortFilter(IQueryable<PrtnCreditDemandListDto> query, PrtnCreditDemandSortFilterOptions options)
        //{
        //    return base.SortFilter(query, options);
        //}
        public SelectList<long> AsSelectList()
        {
            return Repository.ReadAsNoTracked<PrtnCreditDemandListDto>().AsSelectList();
        }

        public override PagedResult<PrtnCreditDemandListDto> GetList(PrtnCreditDemandSortFilterOptions options)
        {
            return Repository.ReadAsNoTracked<PrtnCreditDemandListDto>()
                .SortFilter(options)
                .AsPagedResult(options);
        }
        public override PrtnCreditDemandDto Get(long id)
        {
            return Repository.ById<PrtnCreditDemandDto>(id);
        }

        public int GetCount()
        {
			PrtnCreditDemandSortFilterOptions options = 
                new PrtnCreditDemandSortFilterOptions();
            var data = Repository.ReadAsNoTracked<PrtnCreditDemandListDto>()
                .SortFilter(options).Count();

            return data;

		}
        public PrtnCreditDemandDto GetCheckCreditDemandWithCertificate(Guid? id2, string contractorInn)
        {

            if (id2 == null && contractorInn == null)
            {
                AddError("Sertifikatni olish uchun yetarli parametr kiritilmagan!");
                return null;
            }

            var creditDemand = _unitOfWork.Context.Set<PrtnCreditDemand>()
                                                        .FirstOrDefault(a => a.PrtnCertificate.Id2 == id2 || a.Contractor.Inn == contractorInn);

            if (creditDemand == null)
            {
                AddError("Sertifikat topilmadi!");
                return null;
            }

            return Get(creditDemand.Id);
        }

        public override PrtnCreditDemandDto Get()
        {
            var contractor = UnitOfWork.Context.Set<Contractor>()
                .Include(c => c.Region)
                .Include(c => c.District)
                .Include(c => c.Bank)
                .FirstOrDefault(c => c.Id == _authService.Contractor.Id);
            var certificate = UnitOfWork.Context.Set<PrtnCertificate>()
                .Include(c => c.PrtnContractType)
                .Include(c => c.PrtnContract)
                .ThenInclude(pc => pc.Application)
                .FirstOrDefault(c => c.ContractorId == _authService.Contractor.Id
                && c.StatusId == StatusIdConst.FORMED);
            //var a = contractor.Address;
            if (contractor == null)
            {
                AddError("Нет доступа");
                return null;
            }
            if (certificate == null || certificate.PrtnContract.Application == null)
            {
                AddError("Certificate not found");
                return null;
            }
            return new()
            {
                ContractorId = contractor.Id,
                ContractorInn = contractor.Inn,
                BankId = contractor.BankId,
                //Bank = contractor.Bank?.Translates.AsQueryable()
                //    .FirstOrDefault(
                //        BankTranslate.GetExpr(BankTranslateColumn.bank_name,
                //        ServiceProvider.CultureHelper.CurrentCulture.Id))
                //    ?.TranslateText ?? contractor.Bank.BankName,
                RegionId = contractor.RegionId,
                RegionName = contractor.Region.FullName,
                DistrictId = contractor.DistrictId,
                DistrictName = contractor.District.FullName,
                CertificateId = certificate?.Id,
                PrtnCertificateDocNumber = certificate?.DocNumber,
                PrtnCertificateDocOn = certificate?.DocOn,
                Contractor = contractor.FullName,
                DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_APPLICATION, 1).Item2,
                DocOn = DateOnly.FromDateTime(DateTime.Now),
                PrtnContractType = certificate?.PrtnContractType.FullName,
                PrtnContractTypeId = certificate?.PrtnContractTypeId,
                NewVacanciesCount = certificate?.PrtnContract.NewVacanciesCount,
                Address = contractor.Address,
                CanCreate = certificate != null,
                CanEdit = true
            };
        }
        public override HaveId<long> Create(CreatePrtnCreditDemandDlDto dto)
        {
            if (HasErrors)
                return null;
            if (Repository.AllAsQueryable.Any())
            {
                AddError("Already exist");
                return null;
            }
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var ent = Repository.Create(dto);
                CombineStatuses(Repository);
                if (HasErrors)
                    return null;
                _unitOfWork.Save();

                //var res = CreateDocumentChangeLog(ent.Id, StatusIdConst.CREATED);
                if (IsValid)
                {
                    transaction.Commit();
                }

                return HaveId.Create(ent.Id);
            }
        }
        public Stream SaveAsExecel(PrtnCreditDemandSortFilterOptions dto)
        {
            //var data = GetList(dto);
            var data = Repository.ReadAsNoTracked<PrtnCreditDemandListDto>()
                            .SortFilter(dto)
                            .Take(1000000)
                            .ToList();

            MemoryStream result = new MemoryStream();
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.PRTN_CREDIT_DEMAND_LIST));

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
                    ws.Cells[currentRow, column++].Value = item.Id;
                    ws.Cells[currentRow, column++].Value = item.DocNumber;
                    ws.Cells[currentRow, column++].Value = item.DocOn.ToString(Constants.DATE_FORMAT);
                    ws.Cells[currentRow, column++].Value = item.RegionName;
                    ws.Cells[currentRow, column++].Value = item.DistrictName;
                    ws.Cells[currentRow, column++].Value = item.PrtnContractType;
                    ws.Cells[currentRow, column++].Value = item.NewVacanciesCount;
                    ws.Cells[currentRow, column++].Value = item.ContractorInn;
                    ws.Cells[currentRow, column++].Value = item.Contractor;
                    ws.Cells[currentRow, column++].Value = item.ContractorPhoneNumber;
                    ws.Cells[currentRow, column++].Value = item.ImplementedProjectName;
                    ws.Cells[currentRow, column++].Value = item.ProjectCost;
                    ws.Cells[currentRow, column++].Value = item.OwnInvestment;
                    ws.Cells[currentRow, column++].Value = item.ForeignInvestment;
                    ws.Cells[currentRow, column++].Value = item.PrivillageBankCredit;
                    ws.Cells[currentRow, column++].Value = item.BankCode;
                    ws.Cells[currentRow, column++].Value = item.Bank;
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
        public override void Update(UpdatePrtnCreditDemandDlDto dto)
        {
            if (HasErrors)
                return;
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                Repository.AllAsQueryable.Lock(dto.Id);

                var ent = Repository.Update(dto);
                CombineStatuses(Repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                //var res = CreateDocumentChangeLog(ent.Id, StatusIdConst.MODIFIED);
                if (IsValid)
                {
                    transaction.Commit();
                }
            }
        }
        //private HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
        //{
        //    int? orgId = null;
        //    if (_authService.Organization?.Id != null)
        //        orgId = _authService.Organization.Id;

        //    var moveDto = Repository.ById<PrtnCreditDemandDto>(id, applyFilter: false);

        //    _documentChangeLogService.Create(
        //        dto: moveDto,
        //        tableId: TableIdConst.DOC_PRTN_CERTIFICATE,
        //        organizationId: orgId,
        //        statusId: statusId,
        //        message: message);
        //    CombineStatuses(_documentChangeLogService);

        //    if (HasErrors)
        //        return null;

        //    return HaveId.Create(id);
        //}
        public override void Delete(long id)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                Repository.AllAsQueryable.Lock(id);
                var dto = new UpdateStatusPrtnCreditDemandDlDto { Id = id, StatusId = StatusIdConst.DELETED };

                var ent = Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanDeleteStatuses.Contains(ent.StatusId))
                        AddError("O'chirish mumkin emas / Невозможно удалить");
                });
                CombineStatuses(Repository);
                if (HasErrors)
                    return;
                _unitOfWork.Save();

                //var res = CreateDocumentChangeLog(ent.Id, StatusIdConst.DELETED);
                if (IsValid)
                {
                    transaction.Commit();
                }
            }
        }
    }
}