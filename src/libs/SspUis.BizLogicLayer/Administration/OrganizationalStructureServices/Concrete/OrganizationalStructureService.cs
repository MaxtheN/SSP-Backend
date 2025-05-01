using GenericServices;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet;
using WEBASE.Models;
using System.IO;
using Microsoft.EntityFrameworkCore;
using WEBASE.Storage;
using OfficeOpenXml;
using System.Linq.Dynamic.Core;
using iText.Html2pdf.Attach;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using iText.Kernel.Pdf;
//using RestSharp.Extensions;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer;
using SspUis.Core.Security;
using SspUis.BizLogicLayer.Info.OrganizationalStructureServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.OrganizationalStructureServices
{
    public class OrganizationalStructureService : BaseEntityService<int, OrganizationalStructure, OrganizationalStructureListDto, OrganizationalStructureDto, CreateOrganizationalStructureDlDto, UpdateOrganizationalStructureDlDto, IOrganizationalStructureRepository, SortFilterPageOptions>
        , IOrganizationalStructureService
    {
        private readonly IOrganizationalStructureRepository _repository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageService _storageService;

        public OrganizationalStructureService(
            IUnitOfWork unitOfWork,
            IStorageService storageService,
            IAuthService authService)
            : base(unitOfWork)
        {

            _repository = unitOfWork.OrganizationalStructureRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
            _storageService = storageService;
        }

        //protected override IQueryable<OrganizationalStructureListDto> SortFilter(IQueryable<OrganizationalStructureListDto> query, SortFilterPageOptions options)
        //{
        //    return base.SortFilter(query, options).SortFilter(options);
        //}     

        public override OrganizationalStructureDto Get()
        {
            return new OrganizationalStructureDto
            {
                CorrCoef = 1
            };
        }

        public decimal GetCorrCoef()
        {
            if (_authService.User.OrganizationId != null && _authService.Organization.OrganizationalStructureId.HasValue)
            {
                var entity = _repository.ById(_authService.Organization.OrganizationalStructureId.Value);
                return entity?.CorrCoef ?? 0;
            }

            return 0;
        }

        public WEBASE.Models.PagedResult<OrganizationalStructureListDto> GetList(OrganizationalStructureFilterDto dto)
        {
            var result = _repository.ReadAsNoTracked<OrganizationalStructureListDto>()
                           .SortFilter(dto)
                           .AsPagedResult(dto);

            // To change the elements, need to convert to a list
            result.Rows = result.Rows.ToList();

            foreach (var row in result.Rows.Where(x => x.IsParent.HasValue && x.IsParent.Value))
                row.OrganizationCount = result.Rows.Where(a => a.CodeSymbol == row.Code && (!a.IsParent.HasValue || !a.IsParent.Value)).Sum(a => a.OrganizationCount);

            foreach (var row in result.Rows.Where(x => x.IsParent.HasValue && x.IsParent.Value))
                row.PositionCount = result.Rows.Where(a => a.CodeSymbol == row.Code && (!a.IsParent.HasValue || !a.IsParent.Value)).Sum(a => a.PositionCount);

            //if(!dto.HasSort() || dto.SortBy=="code")
            // {
            //     if (dto.OrderType.ToLower() == "desc") result.ClaimThemeCount = result.ClaimThemeCount.OrderByDescending(x => x.CodeSymbol).ThenBy(x => x.CodeNumber); 
            //     else result.ClaimThemeCount = result.ClaimThemeCount.OrderBy(x => x.CodeSymbol).ThenBy(x => x.CodeNumber);
            // }


            return result;
        }
        public List<OrganizationalStructureListDto> GetListDashboard(string parentCode, int? regionId)
        {

            var organizationStructure = _repository.ReadAsNoTracked<OrganizationalStructureListDto>()
                       .Where(x => string.IsNullOrEmpty(parentCode) ? (x.IsParent.HasValue && x.IsParent.Value) : (x.Code.StartsWith(parentCode) && !(x.IsParent.HasValue && x.IsParent.Value)))
                       .ToList();


            if (string.IsNullOrEmpty(parentCode))
                organizationStructure.ForEach(x =>
                {
                    x.OrganizationCount = _unitOfWork.Context.Set<Organization>().Where(org => (regionId.HasValue && regionId.Value > 0 ? org.RegionId == regionId.Value : true) && org.OrganizationalStructure.Code.StartsWith(x.Code)).Count();
                });
            else
                organizationStructure.ForEach(x =>
                {
                    x.OrganizationCount = _unitOfWork.Context.Set<Organization>().Where(org => (regionId.HasValue ? org.RegionId == regionId.Value : true) && org.OrganizationalStructure.Code.Equals(x.Code)).Count();
                });

            return organizationStructure;

        }
        public List<OrganizationalStructureDashboardDto> GetListDashboard2()
        {
           var data =  _repository.ReadAsNoTracked<OrganizationalStructureDashboardDto>();

            var parents = data.Where(x => x.IsParent.HasValue && x.IsParent.Value).ToList();
            parents.ForEach(x =>
            {
                x.OrganizationCount = data.Where(tab => tab.CodeSymbol == x.Code).Sum(tab => tab.OrganizationCount);
                x.PositionCount = data.Where(tab => tab.CodeSymbol == x.Code).Sum(tab => tab.PositionCount);
                x.ManagementPositionCount = data.Where(tab => tab.CodeSymbol == x.Code).Sum(tab => tab.ManagementPositionCount);
                x.AssistantPositionCount = data.Where(tab => tab.CodeSymbol == x.Code).Sum(tab => tab.AssistantPositionCount);
                x.TechnicalPositionCount = data.Where(tab => tab.CodeSymbol == x.Code).Sum(tab => tab.TechnicalPositionCount);
                x.ProductionPositionCount = data.Where(tab => tab.CodeSymbol == x.Code).Sum(tab => tab.ProductionPositionCount);
            });
            return parents.OrderBy(x=> x.Code).ToList();
        }
        public dynamic GetOrganizationCount()
        {
            return new
            {
                organizationalStructureCount = _unitOfWork.OrganizationRepository.AllAsQueryable.Where(a => a.OrganizationalStructureId.HasValue).Count(),
                organizationCount = _unitOfWork.OrganizationRepository.AllAsQueryable.Where(a => a.StateId == StateIdConst.ACTIVE).Count()
            };

        }
        //public Stream PrintOrganizationStructure(int id, bool isFullOrganization)
        //{
        //    List<Organization> organization = _unitOfWork.Context.Set<Organization>()
        //                                            .Include(x => x.OrganizationalStructure)
        //                                            .Where(x => (isFullOrganization || (x.OrganizationalStructureId.HasValue && x.OrganizationalStructureId.Value == id)) && x.StateId != 2)
        //                                            .OrderBy(x => x.OrganizationalStructure != null ? x.OrganizationalStructure.Code.Substring(0, 1) : "").ThenBy(x => x.OrganizationalStructure != null ? Convert.ToInt16(x.OrganizationalStructure.Code.Substring(1)) : 0)
        //                                            .ToList();

        //    // var organizationStructure = _repository.ById<OrganizationalStructureDto>(id);

        //    MemoryStream template = _storageService.GetStaticFile(WEBASE.Storage.StaticFileConst.Report.CONTROLFUNCTION);
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    ExcelPackage pck = new ExcelPackage(template);

        //    var importrow = pck.Workbook.Names["ImportRow"];
        //    int currentrow = importrow.Start.Row;
        //    var ws = importrow.Worksheet;

        //    for (int i = 0; i < organization.Count; i++)
        //    {
        //        ws.InsertRow(currentrow, 1, importrow.Start.Row);
        //        ws.Cells[currentrow, 1].Value = (i + 1);
        //        ws.Cells[currentrow, 2].Value = organization[i].OrganizationalStructure?.FullName;

        //        // ws.Cells[currentrow, 3].Merge = true;
        //        ws.Cells[currentrow, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        ws.Cells[currentrow, 3].Value = organization[i].OrganizationalStructure?.Code;

        //        ws.Cells[currentrow, 4].Value = organization[i].FullName;

        //        // ws.Cells[currentrow, 5].Merge = true;
        //        ws.Cells[currentrow, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        ws.Cells[currentrow, 5].Value = organization[i].Inn;
        //        currentrow++;
        //    }

        //    //AutoFit column width and row height
        //   /* for (int i = 0; i < ws.Workbook.Worksheets.Count; i++)
        //    {
        //        ExcelWorksheet worksheet = ws.Workbook.Worksheets[i];
        //        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns(15);
        //    }*/
        //    MemoryStream result = new MemoryStream(pck.GetAsByteArray());
        //    pck.Dispose();
        //    return result;
        //}
        //public Stream PrintOrganizationalStructureList(OrganizationalStructureFilterDto dto)
        //{
        //    var structure = _repository.ReadAsNoTracked<OrganizationalStructureListDto>()
        //                               .OrderBy(x => x.Code)
        //                               .ToList();

        //    foreach (var row in structure.Where(x => x.IsParent.HasValue && x.IsParent.Value))
        //        row.OrganizationCount = structure.Where(a => a.CodeSymbol == row.Code && (!a.IsParent.HasValue || !a.IsParent.Value)).Sum(a => a.OrganizationCount);


        //    MemoryStream template = _storageService.GetStaticFile(WEBASE.Storage.StaticFileConst.Report.CONTROLFUNCTION);
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    ExcelPackage pck = new ExcelPackage(template);

        //    var importrow = pck.Workbook.Names["ImportRow"];
        //    int currentrow = importrow.Start.Row;
        //    var ws = importrow.Worksheet;

        //    for (int i = 0; i < structure.Count; i++)
        //    {
        //        ws.InsertRow(currentrow, 1, importrow.Start.Row);
        //        ws.Cells[currentrow, 1].Value = (i + 1);
        //        ws.Cells[currentrow, 2].Value = structure[i].FullName;

        //        ws.Cells[currentrow, 3].Merge = true;
        //        ws.Cells[currentrow, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        ws.Cells[currentrow, 3].Value = structure[i].Code;

        //        ws.Cells[currentrow, 4].Merge = true;
        //        ws.Cells[currentrow, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        ws.Cells[currentrow, 4].Value = structure[i].OrganizationCount;
        //        ws.Cells[currentrow, 5].Merge = true;
        //        ws.Cells[currentrow, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        ws.Cells[currentrow, 5].Value = structure[i].CalculationKindCount;
        //        ws.Cells[currentrow, 6].Merge = true;
        //        ws.Cells[currentrow, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        ws.Cells[currentrow, 6].Value = structure[i].PositionCount;
        //        currentrow++;
        //    }
        //    var totalrow = pck.Workbook.Names["TotalRow"];
        //    //currentrow = totalrow.Start.Row;
        //    //  ws = totalrow.Worksheet;
        //    ws.Cells[currentrow, 2].Merge = true;
        //    ws.Cells[currentrow, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    ws.Cells[currentrow, 2].Value = "Жами";
        //    ws.Cells[currentrow, 3].Merge = true;
        //    ws.Cells[currentrow, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    ws.Cells[currentrow, 3].Value = structure.Count;
        //    ws.Cells[currentrow, 4].Merge = true;
        //    ws.Cells[currentrow, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    ws.Cells[currentrow, 4].Value = structure.Sum(x => x.OrganizationCount);

        //    ws.Cells[currentrow, 5].Merge = true;
        //    ws.Cells[currentrow, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    ws.Cells[currentrow, 5].Value = structure.Sum(x => x.CalculationKindCount);
           
        //    ws.Cells[currentrow, 6].Merge = true;
        //    ws.Cells[currentrow, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    ws.Cells[currentrow, 6].Value = structure.Sum(x => x.PositionCount);

        //    //AutoFit column width and row height
        //    /* for (int i = 0; i < ws.Workbook.Worksheets.Count; i++)
        //     {
        //         ExcelWorksheet worksheet = ws.Workbook.Worksheets[i];
        //         worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns(15);
        //     }*/
        //    MemoryStream result = new MemoryStream(pck.GetAsByteArray());
        //    pck.Dispose();
        //    return result;
        //}

        //public Stream PrintOrganizationalStructureDashboard()
        //{
        //    var structure = GetListDashboard2();
        //    MemoryStream template = _storageService.GetStaticFile(WEBASE.Storage.StaticFileConst.Report.CONTROLFUNCTION);
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    ExcelPackage pck = new ExcelPackage(template);

        //    var importrow = pck.Workbook.Names["InsertRows"];
        //    int currentrow = importrow.Start.Row;
        //    var ws = importrow.Worksheet;

        //    for (int i = 0; i < structure.Count; i++)
        //    {
        //        ws.InsertRow(currentrow, 1, importrow.Start.Row);
        //        ws.Cells[currentrow, 1].Value = (i + 1);
        //        ws.Cells[currentrow, 2].Value = structure[i].FullName;

        //        ws.Cells[currentrow, 3].Merge = true;
        //        ws.Cells[currentrow, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        ws.Cells[currentrow, 3].Value = structure[i].Code;

        //        ws.Cells[currentrow, 4].Merge = true;
        //        ws.Cells[currentrow, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        ws.Cells[currentrow, 4].Value = structure[i].OrganizationCount;
               
        //        ws.Cells[currentrow, 5].Merge = true;
        //        ws.Cells[currentrow, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        ws.Cells[currentrow, 5].Value = 0;
               
        //        ws.Cells[currentrow, 6].Merge = true;
        //        ws.Cells[currentrow, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        ws.Cells[currentrow, 6].Value = 0;
               
        //        ws.Cells[currentrow, 7].Merge = true;
        //        ws.Cells[currentrow, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        ws.Cells[currentrow, 7].Value = 0;
               
        //        ws.Cells[currentrow, 8].Merge = true;
        //        ws.Cells[currentrow, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        ws.Cells[currentrow, 8].Value = 0;
               
        //        ws.Cells[currentrow, 9].Merge = true;
        //        ws.Cells[currentrow, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        ws.Cells[currentrow, 9].Value = 0;
        //        currentrow++;
        //    }

           
        //    ws.Cells[currentrow, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    ws.Cells[currentrow, 2].Value = "Жами";
            
        //    //ws.Cells[currentrow, 3].Merge = true;
        //    ws.Cells[currentrow, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    ws.Cells[currentrow, 3].Value = structure.Count;
          
        //    //ws.Cells[currentrow, 4].Merge = true;
        //    ws.Cells[currentrow, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    ws.Cells[currentrow, 4].Value = structure.Sum(x => x.OrganizationCount);

        //   // ws.Cells[currentrow, 5].Merge = true;
        //    ws.Cells[currentrow, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    ws.Cells[currentrow, 5].Value = 0;

        //   // ws.Cells[currentrow, 6].Merge = true;
        //    ws.Cells[currentrow, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    ws.Cells[currentrow, 6].Value = 0;

        //   // ws.Cells[currentrow, 7].Merge = true;
        //    ws.Cells[currentrow, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    ws.Cells[currentrow, 7].Value = 0;

        //   // ws.Cells[currentrow, 8].Merge = true;
        //    ws.Cells[currentrow, 8].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    ws.Cells[currentrow, 8].Value = 0;

        //   // ws.Cells[currentrow, 9].Merge = true;
        //    ws.Cells[currentrow, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //    ws.Cells[currentrow, 9].Value = 0;



        //    MemoryStream result = new MemoryStream(pck.GetAsByteArray());
        //    pck.Dispose();
        //    return result;

        //}
        public SelectList<int> AsSelectList()
        {
            return _repository.AllAsQueryable.AsSelectList();
        }

        public void UploadEcxel(IFormFile file)
        {

            using ExcelPackage package = new ExcelPackage(file.OpenReadStream());
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var worksheet = package.Workbook.Worksheets.FirstOrDefault().Cells;

            List<UploadOrganizationStructureDto> uploadData = new List<UploadOrganizationStructureDto>();
            int rowIndex = 2;

            int outData = 0;
            while (worksheet[rowIndex, 1].Value != null)
            {
                UploadOrganizationStructureDto dto = new UploadOrganizationStructureDto();
                dto.OrganizationStructureId = (worksheet[rowIndex, 1].Value != null && worksheet[rowIndex, 1].Value.ToString().IndexOf("-") > 0) ? int.TryParse(worksheet[rowIndex, 1].Value.ToString().Substring(0, worksheet[rowIndex, 1].Value.ToString().IndexOf("-")), out outData) ? outData : 0 : 0;
                dto.PositionClassificationId = worksheet[rowIndex, 2].Value != null ? int.TryParse(worksheet[rowIndex, 2].Value.ToString().Trim(), out outData) ? outData : 0 : 0;
                dto.PositionName = worksheet[rowIndex, 3].Value?.ToString()?.Trim();
                dto.PositionTypeId = (worksheet[rowIndex, 4].Value != null && worksheet[rowIndex, 4].Value.ToString().IndexOf("-") > 0) ? int.TryParse(worksheet[rowIndex, 4].Value.ToString().Substring(0, worksheet[rowIndex, 4].Value.ToString().IndexOf("-")), out outData) ? outData : 0 : 0;
                dto.PositionCategoryId = (worksheet[rowIndex, 5].Value != null && worksheet[rowIndex, 5].Value.ToString().IndexOf("-") > 0) ? int.TryParse(worksheet[rowIndex, 5].Value.ToString().Substring(0, worksheet[rowIndex, 5].Value.ToString().IndexOf("-")), out outData) ? outData : 0 : 0;
                dto.TarifScaleTypeId = (worksheet[rowIndex, 6].Value != null && worksheet[rowIndex, 6].Value.ToString().IndexOf("-") > 0) ? int.TryParse(worksheet[rowIndex, 6].Value.ToString().Substring(0, worksheet[rowIndex, 6].Value.ToString().IndexOf("-")), out outData) ? outData : 0 : 0;
                dto.TarifScaleId = (worksheet[rowIndex, 7].Value != null && worksheet[rowIndex, 7].Value.ToString().IndexOf("-") > 0) ? int.TryParse(worksheet[rowIndex, 7].Value.ToString().Substring(0, worksheet[rowIndex, 7].Value.ToString().IndexOf("-")), out outData) ? outData : null : null;
                dto.RankCode = worksheet[rowIndex, 8].Value?.ToString()?.Trim();
                dto.SchoolGroupId = (worksheet[rowIndex, 9].Value != null && worksheet[rowIndex, 9].Value.ToString().IndexOf("-") > 0) ? int.TryParse(worksheet[rowIndex, 9].Value.ToString().Substring(0, worksheet[rowIndex, 9].Value.ToString().IndexOf("-")), out outData) ? outData : 0 : 0;;
                dto.Sum = decimal.TryParse(worksheet[rowIndex, 10].Value?.ToString()?.Trim(), out decimal sum ) ? sum : 0m;

                uploadData.Add(dto);
                rowIndex++;
            }
            if (uploadData.Any(x => x.OrganizationStructureId == 0))
                AddError("Класс киритилмаган ёки хато малумот киритилган", "OrganizationStructureId");

            if (HasErrors)
                return;
            if (uploadData.Any(x => x.PositionClassificationId == 0))
                AddError("Лавозим классификатори ИД рақами (ХАЛИКК) киритилмаган ёки хато малумот киритилган", "PositionClassificationId");

            if (HasErrors)
                return;
            if (uploadData.Any(x => string.IsNullOrEmpty(x.PositionName?.Trim())))
                AddError("Лавозим номи  киритилмаган ёки хато малумот киритилган", "PositionName");

            if (HasErrors)
                return;
            if (uploadData.Any(x => x.PositionTypeId == 0))
                AddError("Лавозим тури  киритилмаган ёки хато малумот киритилган", "PositionTypeId");

            if (HasErrors)
                return;
            if (uploadData.Any(x => x.PositionCategoryId == 0))
                AddError("Лавозим категорияси  киритилмаган ёки хато малумот киритилган", "PositionCategoryId");

            if (HasErrors)
                return;
            if (uploadData.Any(x => x.TarifScaleTypeId == 0))
                AddError("Тариф сетка тури  киритилмаган ёки хато малумот киритилган", "TarifScaleTypeId");


          

            if (uploadData.Any(x => x.TarifScaleTypeId == 2 &&  x.Sum == 0))
                AddError("Базавий оклад асосида тариф сеткасида Сумма кўрсатилмаган ", "Amount");
          
            if (HasErrors)
                return;

            var updateData = uploadData.GroupBy(a => a.OrganizationStructureId);
            
            foreach (var item in updateData)
            {
                foreach (var subItem in item.GroupByMany(a => new { a.PositionName,a.SchoolGroupId }).ToList())
                {
                    if (subItem.Count > 1)
                    {
                        AddError($"{item.Key} ИД ли  Классга  {subItem.Key.PositionName} лавозим 1 мартадан ортиқ биктирилган\n ", "PositionName");
                    }
                }
            }

            if (HasErrors)
                return;

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                foreach (var groupedData in updateData)
                {
                    var organizationalStructure = _repository.ById(groupedData.Key);
                                                         

                    foreach (var subItem in groupedData)
                    {
                        var postionId = _unitOfWork.Context
                                                   .Set<Position>()
                                                   .FirstOrDefault(x => x.FullName.ToLower() == subItem.PositionName.ToLower() && x.PositionClassificationId == subItem.PositionClassificationId)?.Id;
                        if (postionId.HasValue)
                            subItem.PositionId = postionId.Value;

                        else
                        {
                            var localPostionId = _unitOfWork.Context
                                               .Set<Position>()
                                               .Local
                                               .FirstOrDefault(x => x.FullName.ToLower() == subItem.PositionName.ToLower() && x.PositionClassificationId == subItem.PositionClassificationId)?.Id;
                            if (localPostionId.HasValue)
                                subItem.PositionId = localPostionId.Value;
                            else
                            {
                               if( !_unitOfWork.Context.Set<PositionClassification>().Any(x => x.Id == subItem.PositionClassificationId))
                                {
                                    AddError($"{subItem.PositionClassificationId} Лавозим классификатори   (ХАЛИКК) ИД топилмади");
                                    return;
                                }
                               
                                
                                var positionEntry = _unitOfWork.Context.Set<Position>().Add(new Position()
                                {
                                    FullName = subItem.PositionName,
                                    ShortName = subItem.PositionName,
                                    PositionClassificationId = subItem.PositionClassificationId,
                                });

                                _unitOfWork.Save();

                                if (HasErrors)
                                    return;
                                
                                subItem.PositionId = positionEntry.Entity.Id;

                            }
                        }

                        if (subItem.TarifScaleTypeId == 1 && !string.IsNullOrEmpty(subItem.RankCode))
                        {
                            int? rankId = _unitOfWork.Context.Set<TariffScaleTable>().FirstOrDefault(x => x.RankCode == subItem.RankCode)?.Id;
                            if (!rankId.HasValue)
                            {
                                AddError($"{subItem.RankCode} Разряд хато киритилган", nameof(subItem.RankCode));
                                return;
                            }

                            subItem.RankId = rankId.Value;
                        }
                        if (!organizationalStructure.StructurePosition.Any(x => x.PositionId == subItem.PositionId && subItem.SchoolGroupId != 0))
                        {
                            var pos = new SspUis.DataLayer.EfClasses.OrganizationalStructurePosition
                            {
                                // OwnerId = groupedData.Key,
                                PositionId = subItem.PositionId,
                                PositionTypeId = subItem.PositionTypeId,
                                PositionCategoryId = subItem.PositionCategoryId,
                                TariffScaleTypeId = subItem.TarifScaleTypeId,
                                TariffScaleId = subItem.TarifScaleId,
                                RankId = subItem.TarifScaleTypeId == 1 ? subItem.RankId : null,
                            };
                              if(subItem.TarifScaleTypeId == 2)
                            {
                                pos.Amount = subItem.Sum;  
                               
                            }
                            organizationalStructure.StructurePosition.Add(pos);


                            try
                            {
                                _unitOfWork.Save();
                            } catch(Exception ex)
                            {
                              var t =  ex.Message;
                            }
                        }
                       
                    }

                    //Update(organizationalStructure);
                }
                if (IsValid)
                    transaction.Commit();
            }
        }

        public override void Update(UpdateOrganizationalStructureDlDto dto)
        {
            var canCommit = Repository.Context.Database.CurrentTransaction == null;
            var transaction = Repository.Context.Database.CurrentTransaction ?? Repository.Context.Database.BeginTransaction();
            try
            {
                var entiry = _repository.Update(dto, ent => Validation(dto, ent));
                CombineStatuses(_repository);
                if (IsValid)
                    Repository.Context.SaveChanges();

                if (HasErrors)
                {
                    if (canCommit)
                        transaction.Rollback();
                    return;
                }

                // CreateChangeLog(dto.Id, StatusIdConst.MODIFIED);

                if (HasErrors)
                {
                    if (canCommit)
                        transaction.Rollback();
                    return;
                }

                if (canCommit)
                    transaction!.Commit();
            }
            finally
            {
                if (canCommit)
                    transaction!.Dispose();
            }
        }

        public void Validation<TDto>(OrganizationalStructureDlDto<TDto> dto, OrganizationalStructure entity)
            where TDto : OrganizationalStructureDlDto<TDto>
        {
            var query = Repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

            if (query.ByNumberCode(dto.Code, isIncludePassive: true).Any())
                AddError($"Тип организации по штат. расписанию с этим кодом ({dto.Code}) уже существует.", nameof(dto.Code));

            foreach (var item in dto.StructureStaffingIndicator.OrderBy(a => a.CalcOrderCode))
            {
                var allReferencedIndicatorIds = item.IndicatorTables.ToHashSet();

                if (item.IndicatorTables.Any())
                {
                    checkingForRecursiveReferencing(item.IndicatorTables, allReferencedIndicatorIds);

                    if (allReferencedIndicatorIds.Contains(item.StaffingIndicatorId))
                    {
                        var indicator = _unitOfWork.Context.Set<StaffingIndicator>().FirstOrDefault(a => a.Id == item.StaffingIndicatorId);

                        if (indicator != null)
                        {
                            AddError($"Есть взаимосвязанные показатели ({indicator.FullName})");
                            break;
                        }
                    }
                }
            }

            // local recursive function to fill referenced staffing indicator ids recursively
            void checkingForRecursiveReferencing(List<int> referencedIndicatorIds, HashSet<int> allReferencedIndicatorIds)
            {
                if (HasErrors)
                    return;

                foreach (var indicatorId in referencedIndicatorIds)
                {
                    var referencedIndicator = dto.StructureStaffingIndicator.FirstOrDefault(a => a.StaffingIndicatorId == indicatorId);

                    if (referencedIndicator != null && referencedIndicator.IndicatorTables.Any())

                        foreach (var item in referencedIndicator.IndicatorTables)
                        {
                            if (!allReferencedIndicatorIds.Contains(item))
                            {
                                allReferencedIndicatorIds.Add(item);
                                checkingForRecursiveReferencing(referencedIndicator.IndicatorTables.ToList(), allReferencedIndicatorIds);
                            }
                        }
                }
            }
        }
    }
}

