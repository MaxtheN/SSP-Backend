using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
//using RestSharp.Extensions;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Proposal;
using SspUis.DataLayer.Repositories;
using SspUis.ServiceLayer.NumberServices;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using WEBASE.i18n;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Propos
{
    public class ProposalService : StatusGenericHandler, IProposalService
    {
        private readonly IProposalRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IStorageService _storageService;
        private readonly INumberService _numberService;
        private readonly ICultureHelper _cultureHelper;
        private readonly IContractorService _contractorService;

        public ProposalService(
            IAuthService authService,
            IUnitOfWork unitOfWork,
            IProposalRepository repository,
            IStorageService storageServic,
            INumberService numberService,
            ICultureHelper cultureHelper,
            IContractorService contractorService)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _storageService = storageServic;
            _numberService = numberService;
            _cultureHelper = cultureHelper;
            _contractorService = contractorService;
        }

        public PagedResult<ProposalListDto> GetList(ProposalSortFilterOptions options)
        {
            var result = _repository.ReadAsNoTracked<ProposalListDto>()
                        .SortFilter(options)
                        .ToTableData(options);

            return result;
        }

        public ProposalDto Get()
        {
            return new ProposalDto();
        }

        public ProposalDto Get(long id)
        {
            var dto = _repository.ById<ProposalDto>(id);
            CombineStatuses(_repository);
            return dto;
        }

        public Task<ContractorDto> GetfromSoliqByInn(string inn)
        {
            var contractor = _contractorService.GetByInnFromSoliq(inn);

            return contractor;
        }

        public HaveId<long> Create(CreateProposalDlDto dto)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    dto.DocNumber = GetDocumentNumber(dto);
                    var entity = CreateProposalEntity(dto);

                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return null;
                    }

                    SaveFiles(entity, dto.Files);
                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return null;
                    }

                    transaction.Commit();
                    return HaveId.Create(entity.Id);
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Update(UpdateProposalDlDto dto)
        {
            _repository.Update(dto);
            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();
        }

        public void Delete(long id)
        {
            try
            {
                _repository.Delete(id);
                CombineStatuses(_repository);
                if (IsValid)
                    _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                AddError("Запись не может быть удален");
            }
        }

        public Stream SaveAsExcel(ProposalSortFilterOptions dto)
        {
            var data = _repository.ReadAsNoTracked<ProposalListDto>()
                            .SortFilter(dto)
                            .Take(10000000)
                            .ToList();

            MemoryStream result = new MemoryStream();
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.PROPOSAL_LIST));

            if (IsValid && data != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excelPackage = new ExcelPackage(template);

                var namerange = excelPackage.Workbook.Names["ImportRow1"];
                var currentRow = namerange.Start.Row;
                var ws = namerange.Worksheet;

                foreach (var item in data)
                {
                    var column = 1;
                    ws.InsertRow(currentRow, 1, namerange.Start.Row);
                    ws.Cells[currentRow, column++].Value = item.Id;
                    ws.Cells[currentRow, column++].Value = item.DocNumber;
                    ws.Cells[currentRow, column++].Value = item.DocOn?.ToString(Constants.DATE_FORMAT);
                    ws.Cells[currentRow, column++].Value = item.ExternalSourceTypeName;
                    ws.Cells[currentRow, column++].Value = item.ProposalTypeName;
                    ws.Cells[currentRow, column++].Value = item.CompanyTypeName;
                    ws.Cells[currentRow, column++].Value = item.BusinessSectorName;
                    ws.Cells[currentRow, column++].Value = item.PhoneNumber;
                    ws.Cells[currentRow, column++].Value = $"{RemoveInvalidXmlChars(item.NameLatin)} {item.SurnameLatin}";
                    ws.Cells[currentRow, column++].Value = item.CompanyInn;
                    ws.Cells[currentRow, column++].Value = item.CompanyName;
                    ws.Cells[currentRow, column++].Value = item.RegionName;
                    ws.Cells[currentRow, column++].Value = item.DistrictName;
                    ws.Cells[currentRow, column++].Value = item.MfyName;
                    ws.Cells[currentRow, column++].Value = item.AddressName;
                    ws.Cells[currentRow, column++].Value = item.ProposalSubjectName;
                    ws.Cells[currentRow, column++].Value = RemoveInvalidXmlChars(item.AppealText);
                    ws.Cells[currentRow, column++].Value = RemoveInvalidXmlChars(item.ProposalText);

                    currentRow++;
                }
                ws.DeleteRow(namerange.Start.Row);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }
            result.Position = 0;
            return result;
        }
        static string RemoveInvalidXmlChars(string text)
        {
            if (text == null)
            {
                return string.Empty;
            }

            var validXmlChars = text.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray();
            return new string(validXmlChars);
        }

        static bool IsValidXmlString(string text)
        {
            try
            {
                XmlConvert.VerifyXmlChars(text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<IStorageFileInfo> UploadFiles(params StorageFile[] files)
        {
            var result = _storageService.SaveTemp(DocumentStorageConst.ATTESTATION_FILES, files);

            CombineStatuses(_storageService);
            return IsValid ? result : null;
        }

        private string GetDocumentNumber(CreateProposalDlDto dto)
        {
            return _numberService.GetNext(NumberTemplateDocumentConst.DOC_PROPOSAL, organizationId: _authService.IsAuthenticated ? _authService.Organization.Id : 1, dto.ExternalSourceTypeId).Item2;
        }

        private Proposal CreateProposalEntity(CreateProposalDlDto dto)
        {
            var entity = _repository.Create(dto);
            CombineStatuses(_repository);
            _unitOfWork.Save();
            return entity;
        }

        private void SaveFiles(Proposal entity, List<ProposalFileDlDto> files)
        {
            if (files != null)
            {
                _storageService.MoveToPersistent(DocumentStorageConst.DOC_PROPOSAL_FILE, $"{entity.Id}", files.Select(a => a.Id).ToArray());
                CombineStatuses(_storageService);
            }
        }
    }
}
