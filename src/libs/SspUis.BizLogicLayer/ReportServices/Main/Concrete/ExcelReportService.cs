using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SspUis.BizLogicLayer.Doc.MonoApplicationServices;
using SspUis.BizLogicLayer.ReportServices.Main;
using SspUis.BizLogicLayer.ReportServices.Main.ReportDtos.Memship;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Report.Func;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using WEBASE.Storage;
using WEBASE.Utility;

namespace SspUis.BizLogicLayer.ReportServices;

public partial class ReportService : StatusGenericHandler, IReportService
{
    #region PRTN
    public Stream SaveAsExcelPrtnEmploymentGraph(PrtnEmploymentGraphPageOption dto)
    {
        var data = this.GetPrtnEmploymentGraphReport(dto, true);

        MemoryStream result = new();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate
                .GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_PARTNER_EMPLOYMENT_GRAPH));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["Date"];
            var ws = namerange.Worksheet;

            #region INITIAL COLUMN
            namerange.Value = (_cultureHelper.CurrentCulture.Id == LanguageIdConst.UZ_CYRL)
                ? $"{DateTime.Now.Year} - Йил ҳолатига кўра" : (_cultureHelper.CurrentCulture.Id == LanguageIdConst.UZ_LATN)
                ? $"{DateTime.Now.Year} - Yil holatiga ko'ra" : $"{DateTime.Now.Year} - По состоянию на год";

            var ic = excelPackage.Workbook.Names["TotalMonthlyData"];

            var keyValuePair = (DateTime.Now.Year == 2023)
                ? data.Columns.Where(a => a.Key > 5).OrderByDescending(a => a.Key)
                : data.Columns.OrderByDescending(a => a.Key);

            foreach (var month in keyValuePair)
            {
                int currectcol = ic.Start.Column + 3;
                ws.InsertColumn(currectcol, 3);

                // MONTH NAME
                ws.Cells[ic.Start.Row + 1, currectcol, ic.Start.Row + 1, currectcol + 2].Merge = true;
                ws.Cells[ic.Start.Row + 1, currectcol].Value = month.Value;
                ws.Cells[ic.Start.Row + 1, currectcol, ic.Start.Row + 1, currectcol + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[ic.Start.Row + 1, currectcol, ic.Start.Row + 1, currectcol + 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[ic.Start.Row + 1, currectcol, ic.Start.Row + 1, currectcol + 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[ic.Start.Row + 1, currectcol, ic.Start.Row + 1, currectcol + 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[ic.Start.Row + 1, currectcol, ic.Start.Row + 1, currectcol + 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[ic.Start.Row + 1, currectcol, ic.Start.Row + 1, currectcol + 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[ic.Start.Row + 1, currectcol].Style.Font.Bold = true;
                ws.Cells[ic.Start.Row + 1, currectcol].Style.Font.Size = 12;

                // GRAPH REPORT
                ws.Cells[ic.Start.Row + 2, currectcol].Value = ws.Cells[ic.Start.Row + 2, ic.Start.Column].Value;
                ws.Cells[ic.Start.Row + 2, currectcol, ic.Start.Row + 2, currectcol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[ic.Start.Row + 2, currectcol, ic.Start.Row + 2, currectcol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[ic.Start.Row + 2, currectcol, ic.Start.Row + 2, currectcol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[ic.Start.Row + 2, currectcol, ic.Start.Row + 2, currectcol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[ic.Start.Row + 2, currectcol, ic.Start.Row + 2, currectcol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[ic.Start.Row + 2, currectcol, ic.Start.Row + 2, currectcol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[ic.Start.Row + 2, currectcol].Style.Font.Bold = true;
                ws.Cells[ic.Start.Row + 2, currectcol].Style.Font.Size = 10;
                ws.Cells[ic.Start.Row + 2, currectcol].Style.WrapText = true;

                // BY REPORT TAX
                ws.Cells[ic.Start.Row + 2, currectcol + 1].Value = ws.Cells[ic.Start.Row + 2, ic.Start.Column + 1].Value;
                ws.Cells[ic.Start.Row + 2, currectcol, ic.Start.Row + 2, currectcol + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[ic.Start.Row + 2, currectcol, ic.Start.Row + 2, currectcol + 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[ic.Start.Row + 2, currectcol, ic.Start.Row + 2, currectcol + 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[ic.Start.Row + 2, currectcol, ic.Start.Row + 2, currectcol + 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[ic.Start.Row + 2, currectcol, ic.Start.Row + 2, currectcol + 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[ic.Start.Row + 2, currectcol, ic.Start.Row + 2, currectcol + 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[ic.Start.Row + 2, currectcol + 1].Style.Font.Bold = true;
                ws.Cells[ic.Start.Row + 2, currectcol + 1].Style.Font.Size = 10;
                ws.Cells[ic.Start.Row + 2, currectcol + 1].Style.WrapText = true;

                // DIFFERENCE
                ws.Cells[ic.Start.Row + 2, currectcol + 2].Value = ws.Cells[ic.Start.Row + 2, ic.Start.Column + 2].Value;
                ws.Cells[ic.Start.Row + 2, currectcol, ic.Start.Row + 2, currectcol + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[ic.Start.Row + 2, currectcol, ic.Start.Row + 2, currectcol + 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[ic.Start.Row + 2, currectcol, ic.Start.Row + 2, currectcol + 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[ic.Start.Row + 2, currectcol, ic.Start.Row + 2, currectcol + 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[ic.Start.Row + 2, currectcol, ic.Start.Row + 2, currectcol + 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[ic.Start.Row + 2, currectcol, ic.Start.Row + 2, currectcol + 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[ic.Start.Row + 2, currectcol + 2].Style.Font.Bold = true;
                ws.Cells[ic.Start.Row + 2, currectcol + 2].Style.Font.Size = 10;
                ws.Cells[ic.Start.Row + 2, currectcol + 2].Style.WrapText = true;
            }

            ws.Cells[ic.Start.Row, ic.Start.Column + 3].Value = (_cultureHelper.CurrentCulture.Id == LanguageIdConst.UZ_CYRL) ? "шундан" :
                    (_cultureHelper.CurrentCulture.Id == LanguageIdConst.UZ_LATN) ? "shundan" : "от этого";

            ws.Cells[ic.Start.Row, ic.Start.Column + 3, ic.Start.Row, ic.Start.Column + 2 + (keyValuePair.Count() * 3)].Merge = true;
            ws.Cells[ic.Start.Row, ic.Start.Column + 3, ic.Start.Row, ic.Start.Column + 2 + (keyValuePair.Count() * 3)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[ic.Start.Row, ic.Start.Column + 3, ic.Start.Row, ic.Start.Column + 2 + (keyValuePair.Count() * 3)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells[ic.Start.Row, ic.Start.Column + 3, ic.Start.Row, ic.Start.Column + 2 + (keyValuePair.Count() * 3)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[ic.Start.Row, ic.Start.Column + 3, ic.Start.Row, ic.Start.Column + 2 + (keyValuePair.Count() * 3)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[ic.Start.Row, ic.Start.Column + 3, ic.Start.Row, ic.Start.Column + 2 + (keyValuePair.Count() * 3)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[ic.Start.Row, ic.Start.Column + 3, ic.Start.Row, ic.Start.Column + 2 + (keyValuePair.Count() * 3)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[ic.Start.Row, ic.Start.Column + 3].Style.Font.Bold = true;
            ws.Cells[ic.Start.Row, ic.Start.Column + 3].Style.Font.Size = 12;
            #endregion

            #region ADD VALUES
            var importRow = excelPackage.Workbook.Names["ImportRow"];

            int currentRow = importRow.Start.Row, index = 1;
            foreach (var row in data.Rows)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                ws.Cells[currentRow, column++].Value = row.Region;
                ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                ws.Cells[currentRow, column++].Value = row.District;
                ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                ws.Cells[currentRow, column++].Value = row.Mfy;
                ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                ws.Cells[currentRow, column++].Value = row.ContractorName;
                ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                ws.Cells[currentRow, column++].Value = row.ContractorInn;
                ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                ws.Cells[currentRow, column++].Value = row.EmployeesCountUntilFounded;
                ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                ws.Cells[currentRow, column++].Value = row.RowsTotal.TotalPlanGraph;
                ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                ws.Cells[currentRow, column++].Value = row.RowsTotal.TotalDifferenceGraphAndReport;
                ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                ws.Cells[currentRow, column++].Value = row.RowsTotal.TotalDifference;
                ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                foreach (var subItem in row.RowsMonthly.Where(a => a.Key != 0).OrderBy(a => a.Key))
                {
                    /// PLAN GRAPH COUNT
                    ws.Cells[currentRow, column++].Value = subItem.Value.PlanGraphCount;
                    ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[currentRow, column - 1].Style.Font.Size = 11;
                    ws.Cells[currentRow, column - 1].Style.Font.Name = "Arial";

                    /// DIFFERCE GRAPH AND REPORT COUNT
                    ws.Cells[currentRow, column++].Value = subItem.Value.DifferenceGraphAndReportCount;
                    ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[currentRow, column - 1].Style.Font.Size = 11;
                    ws.Cells[currentRow, column - 1].Style.Font.Name = "Arial";

                    /// DIFFERCE COUNT
                    ws.Cells[currentRow, column++].Value = subItem.Value.DifferenceCount;
                    ws.Cells[currentRow, column - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[currentRow, column - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[currentRow, column - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[currentRow, column - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[currentRow, column - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[currentRow, column - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[currentRow, column - 1].Style.Font.Size = 11;
                    ws.Cells[currentRow, column - 1].Style.Font.Name = "Arial";
                }
                currentRow++;
            }

            // TOTAL DATA
            var totalColumn = ic.Start.Column;
            int col = 7;
            ws.Cells[currentRow, 1, currentRow, col - 4].Value = "TOTAL";
            ws.Cells[currentRow, 1, currentRow, col - 4].Merge = true;
            ws.Cells[currentRow, 1, currentRow, col - 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[currentRow, 1, currentRow, col - 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[currentRow, 1, currentRow, col - 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[currentRow, 1, currentRow, col - 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[currentRow, 1, currentRow, col - 4].Style.Font.Bold = true;
            ws.Cells[currentRow, 1, currentRow, col - 4].Style.Font.Size = 13;

            ws.Cells[currentRow, col++].Value = data.TotalEmployeesCountUntilFounded;
            ws.Cells[currentRow, col - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[currentRow, col - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[currentRow, col - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[currentRow, col - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[currentRow, col - 1].Style.Font.Bold = true;

            ws.Cells[currentRow, col++].Value = data.Total.TotalPlanGraph;
            ws.Cells[currentRow, col - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[currentRow, col - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[currentRow, col - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[currentRow, col - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[currentRow, col - 1].Style.Font.Bold = true;

            ws.Cells[currentRow, col++].Value = data.Total.TotalDifferenceGraphAndReport;
            ws.Cells[currentRow, col - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[currentRow, col - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[currentRow, col - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[currentRow, col - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[currentRow, col - 1].Style.Font.Bold = true;

            ws.Cells[currentRow, col++].Value = data.Total.TotalDifference;
            ws.Cells[currentRow, col - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            ws.Cells[currentRow, col - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[currentRow, col - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            ws.Cells[currentRow, col - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[currentRow, col - 1].Style.Font.Bold = true;

            foreach (var monthly in data.TotalMonthly.Where(a => a.Key != 0).OrderBy(a => a.Key))
            {
                /// PLAN GRAPH
                ws.Cells[currentRow, col++].Value = monthly.Value.TotalPlanGraphCount;
                ws.Cells[currentRow, col - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[currentRow, col - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[currentRow, col - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, col - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, col - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, col - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, col - 1].Style.Font.Bold = true;
                ws.Cells[currentRow, col - 1].Style.Font.Size = 11;
                ws.Cells[currentRow, col - 1].Style.Font.Name = "Arial";

                /// TOTAL DIFFERENT GRAPH AND REPORT
                ws.Cells[currentRow, col++].Value = monthly.Value.TotalDifferenceGraphAndReportCount;
                ws.Cells[currentRow, col - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[currentRow, col - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[currentRow, col - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, col - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, col - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, col - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, col - 1].Style.Font.Bold = true;
                ws.Cells[currentRow, col - 1].Style.Font.Size = 11;
                ws.Cells[currentRow, col - 1].Style.Font.Name = "Arial";

                /// DIFFERENT
                ws.Cells[currentRow, col++].Value = monthly.Value.TotalDifferenceCount;
                ws.Cells[currentRow, col - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[currentRow, col - 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[currentRow, col - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, col - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, col - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, col - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[currentRow, col - 1].Style.Font.Bold = true;
                ws.Cells[currentRow, col - 1].Style.Font.Size = 11;
                ws.Cells[currentRow, col - 1].Style.Font.Name = "Arial";
            }

            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
            #endregion
        }

        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelPrtnBojxonaContracts(PrtnApplicationByContractTypeDtoFilter dto)
    {
        var data = GetPrtnApplicationByContractType(dto);

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_BOJXONA_IMTIYOZ));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;



            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            foreach (var item in data.Rows)
            {
                //var totalSum = item.TotalPrtnApplicationSentCount + item.TotalPrtnApplicationSentForReviewCount + item.TotalPrtnApplicationSentRejectedCount + item.TotalPrtnContractCount;
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                if (dto.ByDistrict == true)
                    ws.Cells[currentRow, column++].Value = item.District;
                else if (dto.ByContractor == true)
                    ws.Cells[currentRow, column++].Value = item.ContractorInn + " - " + item.Contractor;
                else
                    ws.Cells[currentRow, column++].Value = item.Region;
                ws.Cells[currentRow, column++].Value = item.TotalApplication.TotalCount;
                ws.Cells[currentRow, column++].Value = item.TotalApplication.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = item.TotalCertificate.TotalCount;
                ws.Cells[currentRow, column++].Value = item.TotalCertificate.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = item.CountContractsSigned.FirstOrDefault(a => a.Key == 1).Value.Count;
                ws.Cells[currentRow, column++].Value = item.CountContractsSigned.FirstOrDefault(a => a.Key == 2).Value.Count;
                ws.Cells[currentRow, column++].Value = item.CountContractsSigned.FirstOrDefault(a => a.Key == 3).Value.Count;
                ws.Cells[currentRow, column++].Value = item.TotalCertificate.TotalCount;
                ;
                currentRow++;
            }

            var columnTotal = 3;
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.TotalApplication.TotalCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.TotalApplication.TotalNewVacanciesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.TotalCertificate.TotalCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.TotalCertificate.TotalNewVacanciesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.CountContractsSigned.FirstOrDefault(a => a.Key == 1).Value.Count);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.CountContractsSigned.FirstOrDefault(a => a.Key == 2).Value.Count);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.CountContractsSigned.FirstOrDefault(a => a.Key == 3).Value.Count);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.TotalCertificate.TotalCount);
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;


    }
    public Stream SaveAsExcelPrtnSoliqImtiyozContracts(PrtnApplicationByContractTypeDtoFilter dto)
    {
        var data = GetPrtnApplicationByContractType(dto);


        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_SOLIQ_IMTIYOZ));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;



            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            foreach (var item in data.Rows)
            {
                //var totalSum = item.TotalPrtnApplicationSentCount + item.TotalPrtnApplicationSentForReviewCount + item.TotalPrtnApplicationSentRejectedCount + item.TotalPrtnContractCount;
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                if (dto.ByDistrict == true)
                    ws.Cells[currentRow, column++].Value = item.District;
                else if (dto.ByContractor == true)
                    ws.Cells[currentRow, column++].Value = item.ContractorInn + " - " + item.Contractor;
                else
                    ws.Cells[currentRow, column++].Value = item.Region;
                ws.Cells[currentRow, column++].Value = item.TotalApplication.TotalCount;
                ws.Cells[currentRow, column++].Value = item.TotalApplication.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = item.TotalCertificate.TotalCount;
                ws.Cells[currentRow, column++].Value = item.TotalCertificate.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = item.CountCertificates.FirstOrDefault(a => a.Key == 1).Value.Count;
                ws.Cells[currentRow, column++].Value = item.CountCertificates.FirstOrDefault(a => a.Key == 2).Value.Count;
                ws.Cells[currentRow, column++].Value = item.CountCertificates.FirstOrDefault(a => a.Key == 3).Value.Count;
                ws.Cells[currentRow, column++].Value = item.CountCertificates.FirstOrDefault(a => a.Key == 1).Value.Count +
                  item.CountCertificates.FirstOrDefault(a => a.Key == 2).Value.Count;
                ws.Cells[currentRow, column++].Value = item.CountCertificates.FirstOrDefault(a => a.Key == 3).Value.Count;
                ws.Cells[currentRow, column++].Value = item.CountCertificates.FirstOrDefault(a => a.Key == 1).Value.Count +
                  item.CountCertificates.FirstOrDefault(a => a.Key == 2).Value.Count;
                ws.Cells[currentRow, column++].Value = item.CountCertificates.FirstOrDefault(a => a.Key == 3).Value.Count;
                ws.Cells[currentRow, column++].Value = item.CountCertificates.FirstOrDefault(a => a.Key == 1).Value.Count +
                 item.CountCertificates.FirstOrDefault(a => a.Key == 2).Value.Count + item.CountCertificates.FirstOrDefault(a => a.Key == 3).Value.Count;
                ws.Cells[currentRow, column++].Value = item.CountCertificates.FirstOrDefault(a => a.Key == 1).Value.Count +
                 item.CountCertificates.FirstOrDefault(a => a.Key == 2).Value.Count + item.CountCertificates.FirstOrDefault(a => a.Key == 3).Value.Count;
                currentRow++;
            }

            var columnTotal = 3;
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.TotalApplication.TotalCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.TotalApplication.TotalNewVacanciesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.TotalCertificate.TotalCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.TotalCertificate.TotalNewVacanciesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.CountCertificates.FirstOrDefault(a => a.Key == 1).Value.Count);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.CountCertificates.FirstOrDefault(a => a.Key == 2).Value.Count);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.CountCertificates.FirstOrDefault(a => a.Key == 3).Value.Count);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item =>
            item.CountCertificates.FirstOrDefault(a => a.Key == 1).Value.Count +
                  item.CountCertificates.FirstOrDefault(a => a.Key == 2).Value.Count
            );

            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.CountCertificates.FirstOrDefault(a => a.Key == 3).Value.Count);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item =>
            item.CountCertificates.FirstOrDefault(a => a.Key == 1).Value.Count +
                 item.CountCertificates.FirstOrDefault(a => a.Key == 2).Value.Count
            );
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.CountCertificates.FirstOrDefault(a => a.Key == 3).Value.Count);
            //ws.Cells[currentRow, columnTotal++].Value = data.ClaimThemeCount.Sum(item => item.TotalApplication.TotalCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item =>
            item.CountCertificates.FirstOrDefault(a => a.Key == 1).Value.Count +
                 item.CountCertificates.FirstOrDefault(a => a.Key == 2).Value.Count + item.CountCertificates.FirstOrDefault(a => a.Key == 3).Value.Count
            );
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item =>
              item.CountCertificates.FirstOrDefault(a => a.Key == 1).Value.Count +
                   item.CountCertificates.FirstOrDefault(a => a.Key == 2).Value.Count + item.CountCertificates.FirstOrDefault(a => a.Key == 3).Value.Count
              );
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelSummaryReportByOkedTypes(PrtnApplicationByContractTypeDtoFilter filter)
    {

        var data = GetPrtnApplicationByContractType(new()
        {
            ByContractor = filter.ByContractor,
            ByDistrict = filter.ByDistrict,
            DistrictId = filter.DistrictId,
            ByRegion = filter.ByRegion,
            EndDate = filter.EndDate,
            ContractorId = filter.ContractorId,
            ContractorInn = filter.ContractorInn,
            StartDate = filter.StartDate,
            ByMfy = filter.ByMfy,
            MfyId = filter.MfyId,
            PrtnContractTypeId = filter.PrtnContractTypeId,
            RegionId = filter.RegionId,
            OkedTypeId = null
        });
        var dataByOkedId1 = GetPrtnApplicationByContractType(new()
        {
            ByContractor = filter.ByContractor,
            ByDistrict = filter.ByDistrict,
            DistrictId = filter.DistrictId,
            ByRegion = filter.ByRegion,
            EndDate = filter.EndDate,
            ContractorId = filter.ContractorId,
            ContractorInn = filter.ContractorInn,
            StartDate = filter.StartDate,
            ByMfy = filter.ByMfy,
            MfyId = filter.MfyId,
            PrtnContractTypeId = filter.PrtnContractTypeId,
            RegionId = filter.RegionId,
            OkedTypeId = 1
        });
        var dataByOkedId2 = GetPrtnApplicationByContractType(new()
        {
            ByContractor = filter.ByContractor,
            ByDistrict = filter.ByDistrict,
            DistrictId = filter.DistrictId,
            ByRegion = filter.ByRegion,
            EndDate = filter.EndDate,
            ContractorId = filter.ContractorId,
            ContractorInn = filter.ContractorInn,
            StartDate = filter.StartDate,
            ByMfy = filter.ByMfy,
            MfyId = filter.MfyId,
            PrtnContractTypeId = filter.PrtnContractTypeId,
            RegionId = filter.RegionId,
            OkedTypeId = 2
        });
        var dataByOkedId3 = GetPrtnApplicationByContractType(new()
        {
            ByContractor = filter.ByContractor,
            ByDistrict = filter.ByDistrict,
            DistrictId = filter.DistrictId,
            ByRegion = filter.ByRegion,
            EndDate = filter.EndDate,
            ContractorId = filter.ContractorId,
            ContractorInn = filter.ContractorInn,
            StartDate = filter.StartDate,
            ByMfy = filter.ByMfy,
            MfyId = filter.MfyId,
            PrtnContractTypeId = filter.PrtnContractTypeId,
            RegionId = filter.RegionId,
            OkedTypeId = 3
        });
        var dataByOkedId4 = GetPrtnApplicationByContractType(new()
        {
            ByContractor = filter.ByContractor,
            ByDistrict = filter.ByDistrict,
            DistrictId = filter.DistrictId,
            ByRegion = filter.ByRegion,
            EndDate = filter.EndDate,
            ContractorId = filter.ContractorId,
            ContractorInn = filter.ContractorInn,
            StartDate = filter.StartDate,
            ByMfy = filter.ByMfy,
            MfyId = filter.MfyId,
            PrtnContractTypeId = filter.PrtnContractTypeId,
            RegionId = filter.RegionId,
            OkedTypeId = 4
        });
        var dataByOkedId5 = GetPrtnApplicationByContractType(new()
        {
            ByContractor = filter.ByContractor,
            ByDistrict = filter.ByDistrict,
            DistrictId = filter.DistrictId,
            ByRegion = filter.ByRegion,
            EndDate = filter.EndDate,
            ContractorId = filter.ContractorId,
            ContractorInn = filter.ContractorInn,
            StartDate = filter.StartDate,
            ByMfy = filter.ByMfy,
            MfyId = filter.MfyId,
            PrtnContractTypeId = filter.PrtnContractTypeId,
            RegionId = filter.RegionId,
            OkedTypeId = 5
        });
        var dataByOkedId6 = GetPrtnApplicationByContractType(new()
        {
            ByContractor = filter.ByContractor,
            ByDistrict = filter.ByDistrict,
            DistrictId = filter.DistrictId,
            ByRegion = filter.ByRegion,
            EndDate = filter.EndDate,
            ContractorId = filter.ContractorId,
            ContractorInn = filter.ContractorInn,
            StartDate = filter.StartDate,
            ByMfy = filter.ByMfy,
            MfyId = filter.MfyId,
            PrtnContractTypeId = filter.PrtnContractTypeId,
            RegionId = filter.RegionId,
            OkedTypeId = 6
        });
        var dataByOkedId7 = GetPrtnApplicationByContractType(new()
        {
            ByContractor = filter.ByContractor,
            ByDistrict = filter.ByDistrict,
            DistrictId = filter.DistrictId,
            ByRegion = filter.ByRegion,
            EndDate = filter.EndDate,
            ContractorId = filter.ContractorId,
            ContractorInn = filter.ContractorInn,
            StartDate = filter.StartDate,
            ByMfy = filter.ByMfy,
            MfyId = filter.MfyId,
            PrtnContractTypeId = filter.PrtnContractTypeId,
            RegionId = filter.RegionId,
            OkedTypeId = 7
        });
        var dataByOkedId8 = GetPrtnApplicationByContractType(new()
        {
            ByContractor = filter.ByContractor,
            ByDistrict = filter.ByDistrict,
            DistrictId = filter.DistrictId,
            ByRegion = filter.ByRegion,
            EndDate = filter.EndDate,
            ContractorId = filter.ContractorId,
            ContractorInn = filter.ContractorInn,
            StartDate = filter.StartDate,
            ByMfy = filter.ByMfy,
            MfyId = filter.MfyId,
            PrtnContractTypeId = filter.PrtnContractTypeId,
            RegionId = filter.RegionId,
            OkedTypeId = 8
        });
        var dataByOkedId9 = GetPrtnApplicationByContractType(new()
        {
            ByContractor = filter.ByContractor,
            ByDistrict = filter.ByDistrict,
            DistrictId = filter.DistrictId,
            ByRegion = filter.ByRegion,
            EndDate = filter.EndDate,
            ContractorId = filter.ContractorId,
            ContractorInn = filter.ContractorInn,
            StartDate = filter.StartDate,
            ByMfy = filter.ByMfy,
            MfyId = filter.MfyId,
            PrtnContractTypeId = filter.PrtnContractTypeId,
            RegionId = filter.RegionId,
            OkedTypeId = 9
        });




        MemoryStream result = new MemoryStream();

        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate
            .GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.SUMMARY_REPORT_BY_OKED_TYPES));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRowTotal = importRow.Start.Row + 1;
            int currentRow = importRow.Start.Row + 2;
            int index = 1;
            var now = DateTime.Now;

            ws.Cells[importRow.Start.Row - 4, importRow.Start.Column].Value = now.ToString("dd.MM.yyyy");
            #region Total
            var columnTotal = 2;
            ws.Cells[currentRowTotal, columnTotal++].Value = "Жами";
            ws.Cells[currentRowTotal, columnTotal++].Value = data.Rows.Sum(item => item.TotalApplication.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = data.Rows.Sum(item => item.TotalApplication.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId1.Rows.Sum(item => item.TotalApplication.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId1.Rows.Sum(item => item.TotalApplication.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId2.Rows.Sum(item => item.TotalApplication.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId2.Rows.Sum(item => item.TotalApplication.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId3.Rows.Sum(item => item.TotalApplication.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId3.Rows.Sum(item => item.TotalApplication.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId4.Rows.Sum(item => item.TotalApplication.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId4.Rows.Sum(item => item.TotalApplication.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId5.Rows.Sum(item => item.TotalApplication.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId5.Rows.Sum(item => item.TotalApplication.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId6.Rows.Sum(item => item.TotalApplication.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId6.Rows.Sum(item => item.TotalApplication.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId7.Rows.Sum(item => item.TotalApplication.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId7.Rows.Sum(item => item.TotalApplication.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId8.Rows.Sum(item => item.TotalApplication.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId8.Rows.Sum(item => item.TotalApplication.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId9.Rows.Sum(item => item.TotalApplication.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId9.Rows.Sum(item => item.TotalApplication.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = data.Rows.Sum(item => item.TotalContract.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = data.Rows.Sum(item => item.TotalContract.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId1.Rows.Sum(item => item.TotalContract.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId1.Rows.Sum(item => item.TotalContract.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId2.Rows.Sum(item => item.TotalContract.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId2.Rows.Sum(item => item.TotalContract.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId3.Rows.Sum(item => item.TotalContract.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId3.Rows.Sum(item => item.TotalContract.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId4.Rows.Sum(item => item.TotalContract.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId4.Rows.Sum(item => item.TotalContract.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId5.Rows.Sum(item => item.TotalContract.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId5.Rows.Sum(item => item.TotalContract.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId6.Rows.Sum(item => item.TotalContract.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId6.Rows.Sum(item => item.TotalContract.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId7.Rows.Sum(item => item.TotalContract.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId7.Rows.Sum(item => item.TotalContract.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId8.Rows.Sum(item => item.TotalContract.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId8.Rows.Sum(item => item.TotalContract.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId9.Rows.Sum(item => item.TotalContract.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId9.Rows.Sum(item => item.TotalContract.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = data.Rows.Sum(item => item.TotalContractSigned.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = data.Rows.Sum(item => item.TotalContractSigned.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId1.Rows.Sum(item => item.TotalContractSigned.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId1.Rows.Sum(item => item.TotalContractSigned.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId2.Rows.Sum(item => item.TotalContractSigned.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId2.Rows.Sum(item => item.TotalContractSigned.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId3.Rows.Sum(item => item.TotalContractSigned.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId3.Rows.Sum(item => item.TotalContractSigned.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId4.Rows.Sum(item => item.TotalContractSigned.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId4.Rows.Sum(item => item.TotalContractSigned.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId5.Rows.Sum(item => item.TotalContractSigned.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId5.Rows.Sum(item => item.TotalContractSigned.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId6.Rows.Sum(item => item.TotalContractSigned.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId6.Rows.Sum(item => item.TotalContractSigned.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId7.Rows.Sum(item => item.TotalContractSigned.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId7.Rows.Sum(item => item.TotalContractSigned.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId8.Rows.Sum(item => item.TotalContractSigned.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId8.Rows.Sum(item => item.TotalContractSigned.TotalNewVacanciesCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId9.Rows.Sum(item => item.TotalContractSigned.TotalCount);
            ws.Cells[currentRowTotal, columnTotal++].Value = dataByOkedId9.Rows.Sum(item => item.TotalContractSigned.TotalNewVacanciesCount);

            #endregion
            for (int i = 0; i < data.Rows.Count; i++)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                if (filter.ByDistrict == true)
                    ws.Cells[currentRow, column++].Value = data.Rows[i].District;
                else if (filter.ByContractor == true)
                    ws.Cells[currentRow, column++].Value = data.Rows[i].ContractorInn + " - " + data.Rows[i].Contractor;
                else
                    ws.Cells[currentRow, column++].Value = data.Rows[i].Region;
                ws.Cells[currentRow, column++].Value = data.Rows[i].TotalApplication.TotalCount;
                ws.Cells[currentRow, column++].Value = data.Rows[i].TotalApplication.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId1.Rows[i].TotalApplication.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId1.Rows[i].TotalApplication.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId2.Rows[i].TotalApplication.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId2.Rows[i].TotalApplication.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId3.Rows[i].TotalApplication.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId3.Rows[i].TotalApplication.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId4.Rows[i].TotalApplication.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId4.Rows[i].TotalApplication.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId5.Rows[i].TotalApplication.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId5.Rows[i].TotalApplication.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId6.Rows[i].TotalApplication.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId6.Rows[i].TotalApplication.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId7.Rows[i].TotalApplication.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId7.Rows[i].TotalApplication.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId8.Rows[i].TotalApplication.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId8.Rows[i].TotalApplication.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId9.Rows[i].TotalApplication.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId9.Rows[i].TotalApplication.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = data.Rows[i].TotalContract.TotalCount;
                ws.Cells[currentRow, column++].Value = data.Rows[i].TotalContract.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId1.Rows[i].TotalContract.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId1.Rows[i].TotalContract.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId2.Rows[i].TotalContract.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId2.Rows[i].TotalContract.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId3.Rows[i].TotalContract.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId3.Rows[i].TotalContract.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId4.Rows[i].TotalContract.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId4.Rows[i].TotalContract.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId5.Rows[i].TotalContract.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId5.Rows[i].TotalContract.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId6.Rows[i].TotalContract.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId6.Rows[i].TotalContract.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId7.Rows[i].TotalContract.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId7.Rows[i].TotalContract.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId8.Rows[i].TotalContract.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId8.Rows[i].TotalContract.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId9.Rows[i].TotalContract.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId9.Rows[i].TotalContract.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = data.Rows[i].TotalContractSigned.TotalCount;
                ws.Cells[currentRow, column++].Value = data.Rows[i].TotalContractSigned.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId1.Rows[i].TotalContractSigned.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId1.Rows[i].TotalContractSigned.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId2.Rows[i].TotalContractSigned.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId2.Rows[i].TotalContractSigned.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId3.Rows[i].TotalContractSigned.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId3.Rows[i].TotalContractSigned.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId4.Rows[i].TotalContractSigned.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId4.Rows[i].TotalContractSigned.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId5.Rows[i].TotalContractSigned.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId5.Rows[i].TotalContractSigned.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId6.Rows[i].TotalContractSigned.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId6.Rows[i].TotalContractSigned.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId7.Rows[i].TotalContractSigned.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId7.Rows[i].TotalContractSigned.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId8.Rows[i].TotalContractSigned.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId8.Rows[i].TotalContractSigned.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId9.Rows[i].TotalContractSigned.TotalCount;
                ws.Cells[currentRow, column++].Value = dataByOkedId9.Rows[i].TotalContractSigned.TotalNewVacanciesCount;
                currentRow++;
            }

            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public Stream SaveAsExecelForPrtnCreditDemand(PrtnCreditDemandInfoByBankDtoFilter dto)
    {
        var data = GetPrtnCreditDemandInfoByBank(dto).ToList();

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate
                .GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.PRNT_CREDIT_DEMAND_LIST));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            namerange.Value = "";

            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            foreach (var item in data)
            {
                //var totalSum = item.TotalPrtnApplicationSentCount + item.TotalPrtnApplicationSentForReviewCount + item.TotalPrtnApplicationSentRejectedCount + item.TotalPrtnContractCount;
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                if (dto.ByDistrict == true)
                    ws.Cells[currentRow, column++].Value = item.District;
                else if (dto.ByContractor == true)
                    ws.Cells[currentRow, column++].Value = item.ContractorInn + " - " + item.Contractor;
                else
                    ws.Cells[currentRow, column++].Value = item.Region;
                ws.Cells[currentRow, column++].Value = item.ContractorCount.ToString();
                ws.Cells[currentRow, column++].Value = item.ProjectCost.ToString();
                ws.Cells[currentRow, column++].Value = item.OwnInvestment.ToString();
                ws.Cells[currentRow, column++].Value = item.ForeignInvestment.ToString();
                ws.Cells[currentRow, column++].Value = item.PrivilegeBankCredit.ToString();
                currentRow++;
            }

            //var importRowTotal = excelPackage.Workbook.Names["ImportRowTotal"];
            //int currentRowTotal = importRowTotal.Start.Row + 1;
            var columnTotal = 3;
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.ContractorCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.ProjectCost);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.OwnInvestment);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.ForeignInvestment);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.PrivilegeBankCredit);
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public Stream SaveAsExecelForPrtnCreditDemandPaged(PrtnCreditDemandInfoDtoFilterPaged dto)
    {
        var data = GetPrtnCreditDemandInfoMethod(dto).ToList();

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate
                .GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.PRNT_CREDIT_DEMAND_LIST_PAGED));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["insertrow"];
            namerange.Value = "";

            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["insertrow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value = item.Region;
                ws.Cells[currentRow, column++].Value = item.District.ToString();
                ws.Cells[currentRow, column++].Value = $"{item.ContractorInn} - {item.Contractor}";
                ws.Cells[currentRow, column++].Value = item.ContractorPhoneNumber != null ? item.ContractorPhoneNumber.ToString() : string.Empty;
                ws.Cells[currentRow, column++].Value = item.ContractType.ToString();
                ws.Cells[currentRow, column++].Value = item.TotalDocCount.ToString();
                ws.Cells[currentRow, column++].Value = item.TotalProjectCost.ToString();
                ws.Cells[currentRow, column++].Value = item.TotalOwnInvestment.ToString();
                ws.Cells[currentRow, column++].Value = item.TotalForeignInvestment.ToString();
                ws.Cells[currentRow, column++].Value = item.TotalPrivillageBankCredit.ToString();
                currentRow++;
            }

            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelPrtnDavAkticContract(PrtnApplicationByContractTypeDtoFilter dto)
    {
        var data = GetPrtnApplicationByContractType(dto);

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_DAVAKTIV_IMTIYOZ));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            foreach (var item in data.Rows)
            {
                //var totalSum = item.TotalPrtnApplicationSentCount + item.TotalPrtnApplicationSentForReviewCount + item.TotalPrtnApplicationSentRejectedCount + item.TotalPrtnContractCount;
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                if (dto.ByDistrict == true)
                    ws.Cells[currentRow, column++].Value = item.District;
                else if (dto.ByContractor == true)
                    ws.Cells[currentRow, column++].Value = item.ContractorInn + " - " + item.Contractor;
                else
                    ws.Cells[currentRow, column++].Value = item.Region;
                ws.Cells[currentRow, column++].Value = item.TotalApplication.TotalCount;
                ws.Cells[currentRow, column++].Value = item.TotalApplication.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = item.TotalCertificate.TotalCount;
                ws.Cells[currentRow, column++].Value = item.TotalCertificate.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = item.CountCertificates.FirstOrDefault(a => a.Key == 1).Value.Count;
                ws.Cells[currentRow, column++].Value = item.CountCertificates.FirstOrDefault(a => a.Key == 2).Value.Count;
                ws.Cells[currentRow, column++].Value = item.CountCertificates.FirstOrDefault(a => a.Key == 3).Value.Count;
                currentRow++;
            }

            var columnTotal = 3;
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.TotalApplication.TotalCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.TotalApplication.TotalNewVacanciesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.TotalCertificate.TotalCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.TotalCertificate.TotalNewVacanciesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.CountCertificates.FirstOrDefault(a => a.Key == 1).Value.Count);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.CountCertificates.FirstOrDefault(a => a.Key == 2).Value.Count);
            ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(item => item.CountCertificates.FirstOrDefault(a => a.Key == 3).Value.Count);
            //ws.Cells[currentRow, columnTotal++].Value = contracts.ClaimThemeCount.Sum(item => item.TotalApplication.TotalCount);
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }

    public Stream PrintPrtnApplicationByFullInfo(PrtnDocumentSortFilterOptions filter)
    {
        var data = GetPrtnApplicationByFullInfo(filter).ToList();
        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.CONTRACTS_PROCESS_INFO));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;


            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value = item.ContractorRegion;
                ws.Cells[currentRow, column++].Value = item.IsRegion;
                ws.Cells[currentRow, column++].Value = item.Region;
                ws.Cells[currentRow, column++].Value = item.District;
                ws.Cells[currentRow, column++].Value = item.Organization;
                ws.Cells[currentRow, column++].Value = item.MFY;
                ws.Cells[currentRow, column++].Value = item.INN;
                ws.Cells[currentRow, column++].Value = item.DateSendedOfApplication;
                ws.Cells[currentRow, column++].Value = item.DateApplicationSigned;
                ws.Cells[currentRow, column++].Value = item.DaysLateToApplicationSign;
                ws.Cells[currentRow, column++].Value = item.DateOfExpertOpinion;
                ws.Cells[currentRow, column++].Value = item.DateLateToExpertOpinion;
                ws.Cells[currentRow, column++].Value = item.DateSignedByBusinessman;
                ws.Cells[currentRow, column++].Value = item.DateOfOrganization1;
                ws.Cells[currentRow, column++].Value = item.DateOfOrganization2;
                ws.Cells[currentRow, column++].Value = item.DateLateToOrganization2;
                ws.Cells[currentRow, column++].Value = item.DateOfCertificate;

                currentRow++;
            }
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;

        return result;
    }

    public Stream PrintPrtnApplicationByContractNewInfo(PrtnDocumentSortFilterOptions filter)
    {
        var data = GetPrtnApplicationByContractNewInfo(filter).ToList();
        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.CONTRACTS_INFO));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;


            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value = item.Region;
                ws.Cells[currentRow, column++].Value = item.District;
                ws.Cells[currentRow, column++].Value = item.MFY;
                ws.Cells[currentRow, column++].Value = item.Organization;
                ws.Cells[currentRow, column++].Value = item.INN;
                ws.Cells[currentRow, column++].Value = item.DateSendedOfApplication;
                ws.Cells[currentRow, column++].Value = item.DateBusinessmanApplicationSigned;
                ws.Cells[currentRow, column++].Value = item.DateBusinessmanApplicationSigned;
                ws.Cells[currentRow, column++].Value = item.BusinessmanStatus;
                ws.Cells[currentRow, column++].Value = item.DateEmploymentApplicationSigned;
                ws.Cells[currentRow, column++].Value = item.DaysEmploymentApplicationSigned;
                ws.Cells[currentRow, column++].Value = item.EmploymentStatus;
                ws.Cells[currentRow, column++].Value = item.DateEconomyApplicationSigned;
                ws.Cells[currentRow, column++].Value = item.DaysEconomyApplicationSigned;
                ws.Cells[currentRow, column++].Value = item.EconomyStatus;
                ws.Cells[currentRow, column++].Value = item.DateOfCertificate;
                currentRow++;
            }
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;

        return result;
    }

    public Stream PrintPrtnApplicationByPetitionInfo(PrtnDocumentSortFilterOptions filter)
    {
        var data = GetPrtnApplicationByPetitionInfo(filter).ToList();
        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.APPLICATION_PROCESSINFO));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value = item.Region;
                ws.Cells[currentRow, column++].Value = item.District;
                ws.Cells[currentRow, column++].Value = item.MFY;
                ws.Cells[currentRow, column++].Value = item.Organization;
                ws.Cells[currentRow, column++].Value = item.INN;
                ws.Cells[currentRow, column++].Value = item.DateSendedOfApplication;
                ws.Cells[currentRow, column++].Value = item.DateApplicationSigned;
                ws.Cells[currentRow, column++].Value = item.DaysLateToApplicationSign;
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
    public Stream PrintPrtnEmploymentGraphNewReport(PrtnDocumentSortFilterOptions filter)
    {
        var data = GetPrtnEmploymentGraphNewReport(filter).ToList();
        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.APPLICATION_STATUS));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;


            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value = item.Region;
                ws.Cells[currentRow, column++].Value = item.District;
                ws.Cells[currentRow, column++].Value = item.Mfy;
                ws.Cells[currentRow, column++].Value = item.OrganizationName;
                ws.Cells[currentRow, column++].Value = item.ContractorInn;
                ws.Cells[currentRow, column++].Value = item.SentForExamination;
                ws.Cells[currentRow, column++].Value = item.DateOfConclusionByJustice;
                ws.Cells[currentRow, column++].Value = item.DaysLate;
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

    public Stream SaveAsExecel(PrtnApplicationAndContractInfoDtoFilter dto)
    {
        var data = GetPrtnApplicationAndContractInfo(dto).ToList();

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.PRNT_APPLICATION_AND_CONTRACT_INFO_LIST));

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
                //var totalSum = item.TotalPrtnApplicationSentCount + item.TotalPrtnApplicationSentForReviewCount + item.TotalPrtnApplicationSentRejectedCount + item.TotalPrtnContractCount;
                var item1 = item.TotalPrtnApplicationCount + item.TotalPrtnApplicationSentRejectedCount;
                var item3 = item.TotalPrtnApplicationSentForReviewCount + item.TotalPrtnApplicationSentRejectedCount + item.TotalPrtnApplicationSentAcceptedCount;
                var item9 = item.TotalPrtnApplicationPassExpertisesCount + item.TotalPrtnApplicationSignningCount + item.TotalPrtnApplicationSignedCount;
                var item12 = item.TotalPrtnApplicationSignningCount + item.TotalPrtnApplicationPassExpertisesCount;
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                if (dto.ByDistrict == true)
                    ws.Cells[currentRow, column++].Value = item.District;
                else if (dto.ByContractor == true)
                    ws.Cells[currentRow, column++].Value = item.ContractorInn + " - " + item.Contractor;
                else
                    ws.Cells[currentRow, column++].Value = item.Region;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnApplicationCount + item.TotalPrtnApplicationSentRejectedCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnApplicationCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnApplicationSentCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnApplicationSentForReviewCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnApplicationSentRejectedCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnContractCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnApplicationNotPassExpertisesCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnApplicationPassExpertisesCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnApplicationSignningCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnApplicationSignedCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnCertificateCount;
                ws.Cells[currentRow, column++].Value = item.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnApplicationIsOffersCount;
                currentRow++;
            }


            //var importRowTotal = excelPackage.Workbook.Names["ImportRowTotal"];
            //int currentRowTotal = importRowTotal.Start.Row + 1;
            var columnTotal = 3;
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnApplicationCount + a.TotalPrtnApplicationSentRejectedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnApplicationCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnApplicationSentCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnApplicationSentForReviewCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnApplicationSentRejectedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnContractCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnApplicationNotPassExpertisesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnApplicationPassExpertisesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnApplicationSignningCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnApplicationSignedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnCertificateCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalNewVacanciesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnApplicationIsOffersCount);
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public Stream SaveAsExecelForSum(PrtnApplicationAndContractInfoDtoFilter dto)
    {
        var data = GetPrtnApplicationAndContractInfo(dto).ToList();

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.PRNT_APPLICATION_AND_CONTRACT_FOR_SUM_LIST));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["Organization"];
            namerange.Value = DateTime.Now;

            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            var cs = namerange.Worksheet;
            var importTotalRow = excelPackage.Workbook.Names["ImportTotalRow"];
            int currentTotalRow = importTotalRow.Start.Row + 1;
            var columnTotal = 3;
            cs.InsertRow(currentTotalRow, 1, importTotalRow.Start.Row);
            cs.Cells[currentTotalRow, 1].Value = "Жами";
            cs.Cells[currentTotalRow, 1, currentTotalRow, 2].Merge = true;

            cs.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalApplication.TotalCount);
            cs.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalApplication.TotalNewVacanciesCount);
            cs.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnApplicationSentForReviewCount + a.TotalPrtnApplicationSentRejectedCount + a.TotalPrtnApplicationSentAcceptedCount);
            cs.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnContractCount);
            cs.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnApplicationSentForReviewCount);
            cs.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnApplicationSentRejectedCount);
            cs.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnContractRejectedCount);
            cs.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnCertificateCanceledApplicationCount);
            cs.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnContractCount);
            cs.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnContractCount);
            cs.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnApplicationPassExpertisesCount + a.TotalPrtnApplicationSignningCount + a.TotalPrtnApplicationSignedCount);
            cs.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnApplicationSentForExpertisesCount);
            cs.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnApplicationNotPassExpertisesCount);
            cs.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnApplicationPassExpertisesCount + a.TotalPrtnApplicationSignningCount);
            cs.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnApplicationSignedCount);
            cs.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnContractRejectWhithOutCertificateCount);
            cs.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnCertificateCanceledCount);
            cs.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnCertificateCount);
            cs.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalNewVacanciesCount);
            cs.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalPrtnCertificateCanceledCount);
            
            cs.DeleteRow(importTotalRow.Start.Row);
            currentTotalRow++;

            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                if (dto.ByDistrict == true)
                    ws.Cells[currentRow, column++].Value = item.District;
                else if (dto.ByContractor == true)
                    ws.Cells[currentRow, column++].Value = item.ContractorInn + " - " + item.Contractor;
                else
                    ws.Cells[currentRow, column++].Value = item.Region;

                ws.Cells[currentRow, column++].Value = item.TotalApplication.TotalCount;
                ws.Cells[currentRow, column++].Value = item.TotalApplication.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnApplicationSentForReviewCount + item.TotalPrtnApplicationSentRejectedCount + item.TotalPrtnApplicationSentAcceptedCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnContractCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnApplicationSentForReviewCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnApplicationSentRejectedCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnContractRejectedCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnCertificateCanceledApplicationCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnContractCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnContractCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnApplicationPassExpertisesCount + item.TotalPrtnApplicationSignningCount + item.TotalPrtnApplicationSignedCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnApplicationSentForExpertisesCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnApplicationNotPassExpertisesCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnApplicationPassExpertisesCount + item.TotalPrtnApplicationSignningCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnApplicationSignedCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnContractRejectWhithOutCertificateCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnCertificateCanceledCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnCertificateCount;
                ws.Cells[currentRow, column++].Value = item.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = item.TotalPrtnCertificateCanceledCount;
                currentRow++;
            }
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelPrtnContracts(PrtnDocumentSortFilterOptions dto)
    {
        var needStatusArray = new[] { StatusIdConst.SENT_FOR_EXPERTISE, StatusIdConst.SIGNING, StatusIdConst.PASS_EXPERTISE, StatusIdConst.NOT_PASS_EXPERTISE, StatusIdConst.SIGNED };

        var res = _unitOfWork.Context.Set<PrtnContract>()
            .Include(x => x.Application).ThenInclude(x => x.PrtnApplication).ThenInclude(x => x.ChoosedRegion)
            .Include(x => x.Application.PrtnApplication.ChoosedDistrict)
            .Include(x => x.Contractor).ThenInclude(x => x.Region)
            .Include(x => x.Contractor.District)
            .Include(x => x.PrtnContractType)
            .Include(x => x.Organization)
            .Include(x => x.Signs).ThenInclude(x => x.PrtnContractTypeTable).ThenInclude(x => x.SignOrganizationType)
            .Include(x => x.Signs).ThenInclude(x => x.OrganizationSign).ThenInclude(x => x.PrtnContractTypeTable).ThenInclude(x => x.Position)
            .Include(x => x.Status).ThenInclude(x => x.Translates)
            .AsSplitQuery()
            .Where(x => needStatusArray.Contains(x.StatusId)
                && (dto.ContractorInn.IsNullOrEmpty() || dto.ContractorInn == x.Contractor.Inn)
                && (!dto.PrtnContractTypeId.HasValue || dto.PrtnContractTypeId == x.PrtnContractTypeId)
                && (!dto.RegionId.HasValue || dto.RegionId ==
                    (x.Application.PrtnApplication.ChooseLocation ? x.Application.PrtnApplication.ChoosedRegionId : x.Application.RegionId))
                && (!dto.DistrictId.HasValue || dto.DistrictId ==
                    (x.Application.PrtnApplication.ChooseLocation ? x.Application.PrtnApplication.ChoosedDistrictId : x.Application.DistrictId))
                && (!(dto.StatusIds != null && dto.StatusIds.Any()) || dto.StatusIds.Any(a => a == x.StatusId))
                && (!dto.FromDocDate.HasValue || dto.FromDocDate <= x.DocOn)
                && (!dto.ToDocDate.HasValue || dto.ToDocDate >= x.DocOn));

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate
            .GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_PRTN_CONTRACT));

        if (IsValid && res != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);
            for (int i = 0; i <= 4; i++)
            {
                ExcelNamedRange namerange = excelPackage.Workbook.Worksheets[i].Names["ImportRow"];
                ExcelWorksheet ws = namerange.Worksheet;
                int currentRow = namerange.Start.Row, index = 1;
                var rows = i switch
                {
                    0 => res.Where(x => x.StatusId == StatusIdConst.SENT_FOR_EXPERTISE).ToList(),
                    1 => res.Where(x => x.StatusId == StatusIdConst.SIGNING).ToList(),
                    2 => res.Where(x => x.Signs != null && x.Signs.Any() && x.Signs.Any(d => d.OrganizationId == 176)).ToList(),
                    3 => res.Where(x => x.StatusId == StatusIdConst.PASS_EXPERTISE).ToList(),
                    4 => res.Where(x => x.StatusId == StatusIdConst.NOT_PASS_EXPERTISE).ToList(),
                    _ => null
                };

                foreach (var row in rows)
                {
                    var column = 1;
                    if (i == 2)
                        row.Signs = row.Signs.Where(x => x.OrganizationId == 176).ToList();

                    ws.Cells[currentRow, column++].Value = index++;
                    ws.Cells[currentRow, column++].Value = row.DocNumber;
                    ws.Cells[currentRow, column++].Value = row.DocOn.ToString();

                    ws.Cells[currentRow, column++].Value = row.Application.PrtnApplication.ChooseLocation
                        ? row.Application.PrtnApplication.ChoosedRegion.FullName
                        : row.Contractor.Region.FullName;

                    ws.Cells[currentRow, column++].Value = row.Application.PrtnApplication.ChooseLocation
                        ? row.Application.PrtnApplication.ChoosedDistrict.FullName
                        : row.Contractor.District.FullName;

                    ws.Cells[currentRow, column++].Value = row.PrtnContractType.FullName;

                    ws.Cells[currentRow, column++].Value = row.Contractor.Inn;
                    ws.Cells[currentRow, column++].Value = row.Contractor.FullName;
                    ws.Cells[currentRow, column++].Value = row.Contractor.RegistrationDate.ToString();
                    ws.Cells[currentRow, column++].Value = row.Organization.FullName;

                    ws.Cells[currentRow, column++].Value = string.Join(", ", row.Signs.Where(x => x.IsSigned).Select(x =>
                        x.PrtnContractTypeTable.SignOrganizationTypeId == SignOrganizationTypeIdConst.BUSINESSMAN
                            ? x.PrtnContractTypeTable.SignOrganizationType.FullName
                            : x.OrganizationSign.PrtnContractTypeTable.Position.ShortName + $" - {x.OrganizationSign.FullName}"));

                    ws.Cells[currentRow, column++].Value = string.Join(", ", row.Signs.Where(x => !x.IsSigned).Select(x =>
                        x.PrtnContractTypeTable.SignOrganizationTypeId == SignOrganizationTypeIdConst.BUSINESSMAN
                            ? x.PrtnContractTypeTable.SignOrganizationType.FullName
                            : x.OrganizationSign.PrtnContractTypeTable.Position.ShortName + $" - {x.OrganizationSign.FullName}"));

                    ws.Cells[currentRow, column++].Value = row.Status.Translates.AsQueryable()
                        .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText
                        ?? row.Status.FullName;

                    ws.Cells[currentRow, column++].Value = i == 0 ? row.CreatedAt.ToString() : i == 2
                        ? string.Join(", ", row.Signs.Select(x => x.SignedAt)) : row.ModifiedAt.ToString();

                    currentRow++;
                }
            }

            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }

        result.Position = 0;
        return result;
    }
    public Stream SaveAsExecelForPrtnCreditBank(PrtnCreditDemandInfoByBankDtoFilter dto)
    {
        var data = GetPrtnCreditDemandInfoByBank(dto).ToList();

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.PRNT_CREDIT_BANK_LIST));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            namerange.Value = "";

            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            foreach (var item in data)
            {
                //var totalSum = item.TotalPrtnApplicationSentCount + item.TotalPrtnApplicationSentForReviewCount + item.TotalPrtnApplicationSentRejectedCount + item.TotalPrtnContractCount;
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;

                ws.Cells[currentRow, column++].Value = item.MainBank;
                ws.Cells[currentRow, column++].Value = item.ContractorCount;
                ws.Cells[currentRow, column++].Value = item.NewVacanciesCount;
                ws.Cells[currentRow, column++].Value = item.PrivilegeBankCredit;
                ws.Cells[currentRow, column++].Value = item.ProjectCost;
                ws.Cells[currentRow, column++].Value = item.OwnInvestment;
                currentRow++;
            }

            //var importRowTotal = excelPackage.Workbook.Names["ImportRowTotal"];
            //int currentRowTotal = importRowTotal.Start.Row + 1;
            var columnTotal = 3;
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.ContractorCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.NewVacanciesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.PrivilegeBankCredit);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.ProjectCost);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.OwnInvestment);
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    #endregion

    #region MEMSHIP
    public Stream SaveAsExcelGetMemshipDocsInfo(MemshipDocsInfoDtoFilter option)
    {
        var data = GetMemshipDocsInfo(option).ToList();

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_MEMSHIP));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var ws = excelPackage.Workbook.Worksheets[0];

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row;
            int index = 1;

            var importTotalRow = excelPackage.Workbook.Names["ImportTotalRow"];
            int currentTotalRow = importTotalRow.Start.Row;
            var columnTotal = 3;
            //ws.InsertRow(currentTotalRow, 1, importTotalRow.Start.Row);
            //ws.Cells[currentTotalRow, 1].Value = "Jami";
            //ws.Cells[currentTotalRow, 1, currentTotalRow, 2].Merge = true;
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalMemshipApplicationCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipApplicationAcceptedCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipApplicationReviewCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipApplicationRejectedCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalMemshipContractCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipContractReviewCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipContractAcceptedCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipContractRejectedCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipCertificateReviewCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipCertificateFormedCount).ToString();
            //ws.DeleteRow(importTotalRow.Start.Row);
            currentTotalRow++;

            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                //if (item.ContractorId.HasValue)
                //    ws.Cells[currentRow, column++].Value = item.Contractor;
                //else
                if (item.DistrictId.HasValue)
                    ws.Cells[currentRow, column++].Value = item.District;
                else
                    ws.Cells[currentRow, column++].Value = item.Region;

                ws.Cells[currentRow, column++].Value = item.TotalMemshipApplicationCount.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipApplicationAcceptedCount.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipApplicationReviewCount.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipApplicationRejectedCount.ToString();
                ws.Cells[currentRow, column++].Value = item.TotalMemshipContractCount.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipContractReviewCount.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipContractAcceptedCount.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipContractRejectedCount.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipCertificateReviewCount.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipCertificateFormedCount.ToString();
                currentRow++;
            }
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }

        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelGetMemshipReport(MemshipReportDtoFilter option)
    {
        var data = GetMemshipReports(option).ToList();

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_MEMSHIPBYMONTH));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var ws = excelPackage.Workbook.Worksheets[0];

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row;
            int index = 1;

            ws.Cells[3, 3].Value = DateTime.Now.Month.ToString();

            var importTotalRow = excelPackage.Workbook.Names["ImportTotalRow"];
            int currentTotalRow = importTotalRow.Start.Row;
            var columnTotal = 3;
            //ws.InsertRow(currentTotalRow, 1, importTotalRow.Start.Row);
            //ws.Cells[currentTotalRow, 1].Value = "Jami";
            //ws.Cells[currentTotalRow, 1, currentTotalRow, 2].Merge = true;
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipGeneralPlan).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipFactByMonth).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = (data.Sum(a => a.MemshipFactByMonthPercentage) / data.Count()).ToString() + " %";
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipApplicationLegal).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipApplicationYtt).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipCertificateLegal).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipCertificateYtt).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipGeneralPlanByYear).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipFactByYear).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipApplicationCountByYear).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipContractCountByYear).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipCertificateAcceptedCountByear).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipCertificateProgressCountByear).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipCertificateNotIncludedCountByear).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipGeneralIndebtednessByYear).ToString();
            ;
            ws.Cells[currentTotalRow, columnTotal++].Value = (data.Sum(a => a.MemshipGeneralIndebtednessPercentageByYear) / data.Count()).ToString() + " %";
            ;
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipGeneralIndebtednessCoefficientByYear).ToString();
            //ws.DeleteRow(importTotalRow.Start.Row);
            //currentTotalRow++;

            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                if (item.DistrictId.HasValue)
                    ws.Cells[currentRow, column++].Value = item.District;
                else
                    ws.Cells[currentRow, column++].Value = item.Region;

                ws.Cells[currentRow, column++].Value = item.MemshipGeneralPlan.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipFactByMonth.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipFactByMonthPercentage.ToString() + " %";
                ws.Cells[currentRow, column++].Value = item.MemshipApplicationLegal.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipApplicationYtt.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipCertificateLegal.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipCertificateYtt.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipGeneralPlanByYear.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipFactByYear.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipApplicationCountByYear.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipContractCountByYear.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipCertificateAcceptedCountByear.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipCertificateProgressCountByear.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipCertificateNotIncludedCountByear.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipGeneralIndebtednessByYear.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipGeneralIndebtednessPercentageByYear.ToString() + " %";
                ws.Cells[currentRow, column++].Value = item.MemshipGeneralIndebtednessCoefficientByYear.ToString();
                currentRow++;
            }
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }

        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelGetPaidMemshipReport(MemshipPaidReportDtoFilter option)
    {
        var data = GetPaidMemshipReport(option).ToList();

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_PAID_MEMSHIP));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var ws = excelPackage.Workbook.Worksheets[0];

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row;
            int index = 1;

            var importTotalRow = excelPackage.Workbook.Names["ImportTotalRow"];
            int currentTotalRow = importTotalRow.Start.Row;
            var columnTotal = 3;
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.SentMemshipApplicationCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.AcceptedMemshipApplicationCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.CreatedMemshipContractCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.SigningMemshipContractCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.SignedMemshipContractCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipCertificateCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipCertificateContributionBXM).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipCertificateContributionAmount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipCertificateRevenueAmount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.MemshipCertificateIndebtednessAmount).ToString();

            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                if (item.DistrictId.HasValue)
                    ws.Cells[currentRow, column++].Value = item.District;
                else
                    ws.Cells[currentRow, column++].Value = item.Region;

                ws.Cells[currentRow, column++].Value = item.SentMemshipApplicationCount.ToString();
                ws.Cells[currentRow, column++].Value = item.AcceptedMemshipApplicationCount.ToString();
                ws.Cells[currentRow, column++].Value = item.CreatedMemshipContractCount.ToString();
                ws.Cells[currentRow, column++].Value = item.SigningMemshipContractCount.ToString();
                ws.Cells[currentRow, column++].Value = item.SignedMemshipContractCount.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipCertificateCount.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipCertificateContributionBXM.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipCertificateContributionAmount.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipCertificateRevenueAmount.ToString();
                ws.Cells[currentRow, column++].Value = item.MemshipCertificateIndebtednessAmount.ToString();
                currentRow++;
            }
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }

        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelGetMemshipReportByOrganization(MemshipReportByOrganizationDtoFilter dto)
    {
        var data = GetMemshipReportByOrganization(dto);

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_MEMSHIP_REPORT_BY_ORGANIZATION));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;



            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            foreach (var item in data)
            {

                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                if (dto.ByRegion)
                    ws.Cells[currentRow, column++].Value = item.Region;
                else if (dto.ByDistrict == true)
                    ws.Cells[currentRow, column++].Value = item.District;
                else
                    ws.Cells[currentRow, column++].Value = item.Contractor;
                ws.Cells[currentRow, column++].Value = item.ApprovedApplication.TotalCount;
                ws.Cells[currentRow, column++].Value = item.ApprovedApplication.MicroOrgCount;
                ws.Cells[currentRow, column++].Value = item.ApprovedApplication.SmallOrgCount;
                ws.Cells[currentRow, column++].Value = item.ApprovedApplication.MiddleOrgCount;
                ws.Cells[currentRow, column++].Value = item.ApprovedApplication.BigOrgCount;

                ws.Cells[currentRow, column++].Value = item.SignedApplication.TotalCount;
                ws.Cells[currentRow, column++].Value = item.SignedApplication.MicroOrgCount;
                ws.Cells[currentRow, column++].Value = item.SignedApplication.SmallOrgCount;
                ws.Cells[currentRow, column++].Value = item.SignedApplication.MiddleOrgCount;
                ws.Cells[currentRow, column++].Value = item.SignedApplication.BigOrgCount;

                ws.Cells[currentRow, column++].Value = item.CertificateGivenApplication.TotalCount;
                ws.Cells[currentRow, column++].Value = item.CertificateGivenApplication.MicroOrgCount;
                ws.Cells[currentRow, column++].Value = item.CertificateGivenApplication.SmallOrgCount;
                ws.Cells[currentRow, column++].Value = item.CertificateGivenApplication.MiddleOrgCount;
                ws.Cells[currentRow, column++].Value = item.CertificateGivenApplication.BigOrgCount;

                currentRow++;
            }

            var columnTotal = 3;
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ApprovedApplication.TotalCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ApprovedApplication.MicroOrgCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ApprovedApplication.SmallOrgCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ApprovedApplication.MiddleOrgCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ApprovedApplication.BigOrgCount);

            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.SignedApplication.TotalCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.SignedApplication.MicroOrgCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.SignedApplication.SmallOrgCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.SignedApplication.MiddleOrgCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.SignedApplication.BigOrgCount);

            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.CertificateGivenApplication.TotalCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.CertificateGivenApplication.MicroOrgCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.CertificateGivenApplication.SmallOrgCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.CertificateGivenApplication.MiddleOrgCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.CertificateGivenApplication.BigOrgCount);
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelGetMemshipReportByPersonType(MemshipReportByPersonTypeFilter dto)
    {
        var data = GetMemshipReportByPersonTypes(dto);

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_MEMSHIP_REPORT_BY_PERSON_TYPE));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;



            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            foreach (var item in data)
            {

                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                if (dto.ByRegion)
                    ws.Cells[currentRow, column++].Value = item.Region;
                else if (dto.ByDistrict == true)
                    ws.Cells[currentRow, column++].Value = item.District;
                else
                    ws.Cells[currentRow, column++].Value = item.Contractor;
                ws.Cells[currentRow, column++].Value = item.TotalAcceptedApplication.LegalPersonCount;
                ws.Cells[currentRow, column++].Value = item.TotalAcceptedApplication.PhysicalPersonCount;

                ws.Cells[currentRow, column++].Value = item.TotalSignedApplication.LegalPersonCount;
                ws.Cells[currentRow, column++].Value = item.TotalSignedApplication.PhysicalPersonCount;

                ws.Cells[currentRow, column++].Value = item.TotalGivenCertificateCount.LegalPersonCount;
                ws.Cells[currentRow, column++].Value = item.TotalGivenCertificateCount.PhysicalPersonCount;

                currentRow++;
            }

            var columnTotal = 3;
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.TotalAcceptedApplication.LegalPersonCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.TotalAcceptedApplication.PhysicalPersonCount);

            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.TotalSignedApplication.LegalPersonCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.TotalSignedApplication.PhysicalPersonCount);

            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.TotalGivenCertificateCount.LegalPersonCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.TotalGivenCertificateCount.PhysicalPersonCount);

            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    #endregion

    #region INTEGRATION

    public Stream SaveAsExcelAllIntegrationReportByContractor()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.ALL_INTEGRATION_REPORT_BY_CONTRACTOR));

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        ExcelPackage excelPackage = new ExcelPackage(template);

        var ws = excelPackage.Workbook.Worksheets[0];

        //var importRow = excelPackage.Workbook.Names["ImportRow"];
        int currentRow = 5;
        int index = 1;


        var contractors = _unitOfWork.Context.Contractors.Select(a => new
        {
            a.Id,
            a.Inn,
            a.FullName,
            a.RegionId,
            a.DistrictId,
            Oked = a.Oked.FullName
        })
        //.Take(100)
        .ToArray();
        Console.WriteLine($"After contractors {stopwatch.ElapsedMilliseconds}");

        var contractTypes = _unitOfWork.Context.PrtnContractTypes.ToArray();


        var contractData = _unitOfWork.Context.PrtnContracts
            .Select(a => new { a.ContractorId, a.ApplicationId, a.DocOn, a.Id, a.PrtnContractTypeId, a.StatusId })
            .Where(a => a.StatusId != StatusIdConst.CANCELED && a.StatusId != StatusIdConst.DELETED)
            .ToArray();
        var applicationData = _unitOfWork.Context.Applications
            .Select(a => new { a.StatusId, a.Id, a.DocOn, a.RegionId, a.DistrictId })
            .Where(a => a.StatusId != StatusIdConst.CANCELED)
            .ToArray();
        var prtnApplicationData = _unitOfWork.Context.PrtnApplications
            .Select(a => new { a.ApplicationId, a.ChoosedDistrictId, a.ChoosedRegionId, a.MfyName, a.ChooseLocation, a.NewVacanciesCount })
            .ToArray();
        var prtnCertificateData = _unitOfWork.Context.PrtnCertificates
            .Select(a => new { a.StatusId, a.ContractorId, a.PrtnContractId, a.DocOn })
            .Where(a => a.StatusId == StatusIdConst.FORMED)
            .ToArray();

        Console.WriteLine($" After 4 query {stopwatch.ElapsedMilliseconds}");

        var regionData = _unitOfWork.Context.Regions
           .IsActive()
           .ToDictionary(
                   a => a.Id,
                   a => a.Translates.AsQueryable().FirstOrDefault(RegionTranslate
                           .GetExpr(TranslateColumn.short_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
                           ?? a.ShortName);

        var districtData = _unitOfWork.Context.Districts
        .IsActive()
        .ToDictionary(
                       ent => ent.Id,
                       ent => new
                       {
                           DistrictName = ent.Translates.AsQueryable().FirstOrDefault(DistrictTranslate
                            .GetExpr(TranslateColumn.short_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
                            ?? ent.ShortName,
                           ent.RegionId
                       });

        var statusData = _unitOfWork.Context.Statuses
        .ToDictionary(
                       ent => ent.Id,
                        a => a.Translates.AsQueryable().FirstOrDefault(StatusTranslate
                           .GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
                           ?? a.FullName);

        Console.WriteLine($" After enum data {stopwatch.ElapsedMilliseconds}");

        var bankCreditReportData = GetBankCreditReport(new ContractorBankCreditReportDtoFilter
        {
            ByContractor = true,
        }).Result.Select(a => new { a.ContractorInn, a.Application }).ToArray();

        Console.WriteLine($" After bankcredit {stopwatch.ElapsedMilliseconds}");

        var projectInfoData = GetPrtnCreditDemandInfoByBank(new PrtnCreditDemandInfoByBankDtoFilter
        {
            ByContractor = true,
        }).Select(a => new { a.ContractorId, a.OwnInvestment, a.ForeignInvestment, a.PrivilegeBankCredit, a.ProjectCost, a.Bank }).ToArray();


        var bojxonaInfoData = GetBojxonaImtiyozReportByContractor(new BojxonaImtiyozReportByContractorDtoFilter
        {
            ByContractor = true,
        }).Select(a => new { a.ContractorId, a.GrChanContractorCount }).ToArray();

        var soliqInfoData = GetTaxCreditReport(new TaxCreditReportDtoFilter
        {
            ByContractor = true,
        }).Select(a => new { a.ContractorId, a.SocialTaxSum, a.LandTaxSum, a.PropertyTaxSum, a.IncomeTaxSum }).ToArray();

        var kafillikAmounts = _unitOfWork.Context.FundTadbirkors
                    .Include(a => a.Credits)
                    .Where(a => a.AidAmount != 0)
                    .Select(a => new
                    {
                        a.AidAmount,
                        a.TinPinfl
                    }).ToArray();


        Console.WriteLine($" After projectInfo {stopwatch.ElapsedMilliseconds}");

        List<SaveAsExcelAllIntegrationReportByContractorDto> datas = new List<SaveAsExcelAllIntegrationReportByContractorDto>();

        Parallel.ForEach(contractors, contractor =>
        {
            // Console.WriteLine($" After {i} th iteraion  {stopwatch.ElapsedMilliseconds}");

            var contract = contractData
                .Where(a => a.ContractorId == contractor.Id)
                .OrderByDescending(a => a.DocOn)
                .FirstOrDefault();

            var application = contract != null ? applicationData.Where(a => a.Id == contract.ApplicationId)
                .OrderByDescending(a => a.DocOn)
                .FirstOrDefault() : null;

            var prtnApplication = application != null ? prtnApplicationData.FirstOrDefault(a => a.ApplicationId == application.Id)
                : null;

            var prtnCertificate = contract != null ? prtnCertificateData
                .Where(a => a.ContractorId == contractor.Id && a.PrtnContractId == contract.Id)
                .OrderByDescending(a => a.DocOn)
                .FirstOrDefault() : null;

            if (prtnCertificate != null)
            {
                SaveAsExcelAllIntegrationReportByContractorDto data = new SaveAsExcelAllIntegrationReportByContractorDto();
                int regionId = (prtnApplication != null && prtnApplication.ChooseLocation && prtnApplication.ChoosedRegionId != null)
                    ? prtnApplication.ChoosedRegionId.Value
                    : (application != null
                        ? application.RegionId
                        : contractor.RegionId);

                int districtId = (prtnApplication != null && prtnApplication.ChooseLocation && prtnApplication.ChoosedDistrictId != null)
                   ? prtnApplication.ChoosedDistrictId.Value
                   : (application != null
                       ? application.DistrictId
                       : contractor.DistrictId);

                string? mfy = prtnApplication != null
                   ? prtnApplication.MfyName
                   : null;

                string contractType = contract != null ? contractTypes.FirstOrDefault(a => a.Id == contract.PrtnContractTypeId)?.FullName : null;

                var projectInfo = projectInfoData.Where(a => a.ContractorId == contractor.Id).FirstOrDefault();

                var creditInfo = bankCreditReportData.FirstOrDefault(a => a.ContractorInn == contractor.Inn)?.Application;

                var kafillikAmount = kafillikAmounts
                    .Where(a => a.TinPinfl == contractor.Inn)
                    .Sum(a => a.AidAmount);

                var bojxonaInfo = bojxonaInfoData.FirstOrDefault(a => a.ContractorId == contractor.Id);

                var soliqInfo = soliqInfoData.FirstOrDefault(a => a.ContractorId == contract.Id);


                //Korxona nomi
                data.Contractor = contractor.FullName;
                //Viloyat
                data.Region = regionData[regionId];
                //Tuman
                data.District = districtData[districtId]?.DistrictName;
                //Mfy nomi
                data.Mfy = mfy;
                //Inn
                data.Inn = contractor.Inn;
                //Shartnome turi
                data.ContractType = contractType;
                //Jami talab etiladigan mablag'
                data.ProjectCost = projectInfo?.ProjectCost;
                //o'z mablag'i
                data.OwnInvestment = projectInfo?.OwnInvestment;
                //bank krediti
                data.PrivilegeBankCredit = projectInfo?.PrivilegeBankCredit;
                //xorijiy investitsiya
                data.ForeignInvestment = projectInfo?.ForeignInvestment;
                //Tijorat bank nomi
                data.Bank = projectInfo?.Bank;
                //Yaratilgan ish o'rinlari soni
                data.NewVacanciesCount = prtnApplication != null ? prtnApplication.NewVacanciesCount : null;
                //Ariza holati
                data.ApplicationStatus = application != null ? statusData[application.StatusId] : string.Empty;
                //Shartnoma holati
                data.ContractStatus = contract != null ? statusData[contract.StatusId] : string.Empty;
                //Sertifikat holati
                data.CertificateStatus = prtnCertificate != null ? statusData[prtnCertificate.StatusId] : string.Empty;
                //Faoliyat turi
                data.Oked = contractor.Oked;
                //Kerditlar: Ajratildi
                data.ApprovedSum = creditInfo?.ApprovedSum;
                //Kreditlar: Rad etildi
                data.RejectedSum = creditInfo?.RejectedSum;
                //Kreditlar: ko'rib chiqilmoqda
                data.OtherSum = creditInfo?.SubmittedSum - (creditInfo?.ApprovedSum + creditInfo?.RejectedSum);
                //Berilgan kafillik
                data.KafillikAmount = kafillikAmount;
                //Imtiyoz miqdori
                data.SoliqSum = soliqInfo?.LandTaxSum + soliqInfo?.PropertyTaxSum + soliqInfo?.IncomeTaxSum + soliqInfo?.SocialTaxSum;

                //Mol mulk solig'i
                data.LandPropertySum = soliqInfo?.LandTaxSum + soliqInfo?.PropertyTaxSum;
                //Daromat 
                data.IncomeSum = soliqInfo?.IncomeTaxSum;
                //Soliq stafkasining 50 foiz miqdorida..
                data.SocialSum = soliqInfo?.SocialTaxSum;

                //Soliqdan bo'lib to'lash huquqini olganlar soni
                //

                //Bojxona yashil yo'lak qo'llanilgan holatlar
                data.GrChanContractorCount = bojxonaInfo?.GrChanContractorCount;

                //Bojxona to'lovlarini foizsiz bo'lib to'lash huquqini olganlar soni
                //


                datas.Add(data);
            }
        });


        if (datas.Count() != 0)
        {
            foreach (var data in datas)
            {
                var column = 1;
                ws.InsertRow(currentRow + 1, 1, 5);

                ws.Cells[currentRow, column++].Value = index++;
                //Korxona nomi
                ws.Cells[currentRow, column++].Value = data.Contractor;
                //Viloyat
                ws.Cells[currentRow, column++].Value = data.Region;
                //Tuman
                ws.Cells[currentRow, column++].Value = data.District;
                //Mfy nomi
                ws.Cells[currentRow, column++].Value = data.Mfy;
                //Inn
                ws.Cells[currentRow, column++].Value = data.Inn;
                //Shartnome turi
                ws.Cells[currentRow, column++].Value = data.ContractType;
                //Jami talab etiladigan mablag'
                ws.Cells[currentRow, column++].Value = data.ProjectCost;
                //o'z mablag'i
                ws.Cells[currentRow, column++].Value = data.OwnInvestment;
                //bank krediti
                ws.Cells[currentRow, column++].Value = data.PrivilegeBankCredit;
                //xorijiy investitsiya
                ws.Cells[currentRow, column++].Value = data.ForeignInvestment;
                //Tijorat bank nomi
                ws.Cells[currentRow, column++].Value = data.Bank;
                //Yaratilgan ish o'rinlari soni
                ws.Cells[currentRow, column++].Value = data.NewVacanciesCount;
                //Ariza holati
                ws.Cells[currentRow, column++].Value = data.ApplicationStatus;
                //Shartnoma holati
                ws.Cells[currentRow, column++].Value = data.ContractStatus;
                //Sertifikat holati
                ws.Cells[currentRow, column++].Value = data.CertificateStatus;
                //Faoliyat turi
                ws.Cells[currentRow, column++].Value = data.Oked;
                //Kerditlar: Ajratildi
                ws.Cells[currentRow, column++].Value = data.ApprovedSum;
                //Kreditlar: Rad etildi
                ws.Cells[currentRow, column++].Value = data.RejectedSum;
                //Kreditlar: ko'rib chiqilmoqda
                ws.Cells[currentRow, column++].Value = data.OtherSum;
                //Berilgan kafillik
                ws.Cells[currentRow, column++].Value = data.KafillikAmount;
                //Imtiyoz miqdori
                ws.Cells[currentRow, column++].Value = data.SoliqSum;

                //Mol mulk solig'i
                ws.Cells[currentRow, column++].Value = data.LandPropertySum;
                //Daromat 
                ws.Cells[currentRow, column++].Value = data.IncomeSum;
                //Soliq stafkasining 50 foiz miqdorida..
                ws.Cells[currentRow, column++].Value = data.SocialSum;

                //Soliqdan bo'lib to'lash huquqini olganlar soni
                ws.Cells[currentRow, column++].Value = string.Empty;
                //Bojxona yashil yo'lak qo'llanilgan holatlar
                ws.Cells[currentRow, column++].Value = data.GrChanContractorCount;
                //Bojxona to'lovlarini foizsiz bo'lib to'lash huquqini olganlar soni
                ws.Cells[currentRow, column++].Value = string.Empty;

                currentRow++;
            }
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
            result.Position = 0;
        }



        stopwatch.Stop();
        Console.WriteLine($"{contractors.Count()}Time taken: {stopwatch.ElapsedMilliseconds} ms");
        return result;

    }
    public Stream SaveAsExcelAllIntegrationReportByRegion()
    {
        var applications = _unitOfWork.Context.Set<Application>()
                             .Include(a => a.PrtnContract)
                             .ThenInclude(b => b.PrtnCertificate)
                             .Include(a => a.Region)
                             .Include(a => a.PrtnApplication)
                             .ThenInclude(b => b.Graphs)
                             .Where(a => a.StatusId == StatusIdConst.ACCEPTED && a.ApplicationTypeId == ApplicationTypeIdConst.PARTNER)
                             .OrderBy(a => a.Region.OrderCode).Distinct().ToArray();

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.EMPLOYEE_COUNT_BY_REGION_LIST));

        if (IsValid && applications != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var ws = excelPackage.Workbook.Worksheets[0];

            var namerange = excelPackage.Workbook.Names["Organization"];
            namerange.Value = "";

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            var groupedApplications = applications.OrderBy(a => a.Region.OrderCode).GroupBy(a => a.Region.FullName).Select(a => new
            {
                ApplicationCount = a.Count(),

                PrtnApplicationId = a.Select(a => a.PrtnApplication.Id).First(),
                RegionId = a.Select(a => a.RegionId).First(),
                RegionName = a.Select(a => a.Region.FullName).First(),
            });

            foreach (var item in groupedApplications)
            {
                var applicationGraph = _unitOfWork.Context.Set<PrtnApplicationGraph>()
                                     .Where(a => a.OwnerId == item.PrtnApplicationId)
                                     .ToList();

                var yearCounts = applicationGraph.GroupBy(g => g.YearIn)
                                 .ToDictionary(g => g.Key, g => g.Sum(x => x.NewVacanciesCount));

                var prtnCertificate = _unitOfWork.Context.Set<PrtnCertificate>().Include(a => a.PrtnContract).ThenInclude(b => b.Application).Where(a => a.PrtnContract.Application.RegionId == item.RegionId && a.StatusId == StatusIdConst.FORMED).Count();

                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value = item.RegionName;
                ws.Cells[currentRow, column++].Value = item.ApplicationCount;
                ws.Cells[currentRow, column++].Value = prtnCertificate;
                ws.Cells[currentRow, column++].Value = yearCounts.Sum(a => a.Value);
                for (int i = 2023; i <= 2026; i++)
                {
                    int countForYear = yearCounts.ContainsKey(i) ? yearCounts[i] : 0;
                    ws.Cells[currentRow, column++].Value = countForYear;
                }
                for (int year = 2023; year <= 2026; year++)
                {
                    var columnForTotal = column;
                    var data = GetSoliqReportByContractor(new GetSoliqReportByContractorDtoFilter
                    {
                        Year = year,
                        RegionId = item.RegionId
                    });

                    foreach (var item2 in data)
                    {
                        if (year == 2023)
                        {
                            ws.Cells[currentRow, columnForTotal++].Value = item2.TotalNumberEmployee;
                            ws.Cells[currentRow, columnForTotal++].Value = item2.NumberEmployee12 - item2.NumberEmployee5;
                        }
                        else
                        {
                            ws.Cells[currentRow, columnForTotal++].Value = item2.TotalNumberEmployee;
                            ws.Cells[currentRow, columnForTotal++].Value = item2.NumberEmployee12 - item2.NumberEmployee1;
                        }
                    }
                }
                currentRow++;
            }
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelAllEmployeeCountReport(AllEmployeeCountReportDto dto)
    {
        if ((dto.Year.IsNullOrEmptyObject() ^ dto.Month.IsNullOrEmptyObject()) || (dto.Year == 2023 && dto.Month <= 5) || dto.Year < 2023)
        {
            AddError("Yil va oyni to'g'ri tanlang");
        }

        if (!dto.ByOrganization)
        {
            var contractors = _unitOfWork.Context.Set<Contractor>()
                .Where(a => a.Applications.Any(a => a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED && a.StatusId == StatusIdConst.ACCEPTED))
                .AsNoTracking()
                .Select(a => new
                {
                    Id = a.Id,
                    Inn = a.Inn,
                    RegionId = a.RegionId,
                    DistrictId = a.DistrictId,
                    Application = a.Applications
                                    .FirstOrDefault(a => a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED && a.StatusId == StatusIdConst.ACCEPTED)
                })
                .Select(a => new
                {
                    Id = a.Id,
                    RegionId = a.Application
                                           .PrtnApplication.ChoosedRegionId
                                                ?? a.RegionId,
                    DistrictId = a.Application.PrtnApplication.ChoosedDistrictId
                                                ?? a.DistrictId,
                    ContractorInn = a.Inn,
                    ContractTypeId = a.Application.PrtnContract.PrtnCertificate.PrtnContractTypeId,
                    PlanEmpCount_2023 = a.Application.PrtnApplication.Graphs.Where(a => a.YearIn == 2023).Sum(b => b.NewVacanciesCount),
                    PlanEmpCount_2024 = a.Application.PrtnApplication.Graphs.Where(a => a.YearIn == 2024).Sum(b => b.NewVacanciesCount),
                    PlanEmpCount_2025 = a.Application.PrtnApplication.Graphs.Where(a => a.YearIn == 2025).Sum(b => b.NewVacanciesCount),
                    PlanEmpCount_2026 = a.Application.PrtnApplication.Graphs.Where(a => a.YearIn == 2026).Sum(b => b.NewVacanciesCount),
                }).ToArray();

            if (dto.RegionId.HasValue)
                contractors = contractors.Where(a => a.RegionId == dto.RegionId).ToArray();


            var totalData = contractors.GroupBy(a => new { a.RegionId, a.ContractTypeId, a.DistrictId }).Select(a => new
            {
                RegionId = a.Key.RegionId,
                DistrictId = a.Key.DistrictId,
                ContractTypeId = a.Key.ContractTypeId,

                PlanEmpCount_2023 = a.Sum(b => b.PlanEmpCount_2023),
                PlanEmpCount_2024 = a.Sum(b => b.PlanEmpCount_2024),
                PlanEmpCount_2025 = a.Sum(b => b.PlanEmpCount_2025),
                PlanEmpCount_2026 = a.Sum(b => b.PlanEmpCount_2026),
                TotalPlanEmpCount = a.Sum(b => b.PlanEmpCount_2023 + b.PlanEmpCount_2024 + b.PlanEmpCount_2025 + b.PlanEmpCount_2026),

                Count_2023 = a.Where(b => b.PlanEmpCount_2023 > 0).Count(),
                Count_2024 = a.Where(b => b.PlanEmpCount_2024 > 0).Count(),
                Count_2025 = a.Where(b => b.PlanEmpCount_2025 > 0).Count(),
                Count_2026 = a.Where(b => b.PlanEmpCount_2026 > 0).Count(),
                TotalCount = a.Where(b => (b.PlanEmpCount_2023 + b.PlanEmpCount_2024 + b.PlanEmpCount_2025 + b.PlanEmpCount_2026) > 0).Count()
            }).ToArray();
            EmployeeCountDataHelper[] allEmployeeCountData = new EmployeeCountDataHelper[] { };
            var employeeCountInMay = new List<EmployeeCountInMay>();

            if (!dto.Month.IsNullOrEmptyObject() && !dto.Year.IsNullOrEmptyObject())
            {
                var empCounts = _unitOfWork.Context.EmployeeCounts
                    .GroupBy(a => new { a.Tin, a.Year, a.Month })
                    .Select(a => new { Tin = a.Key.Tin, Year = a.Key.Year, EmployeeCount = a.Sum(a => a.MonthlyNumberEmployees), a.Key.Month })
                    .ToArray();

                var allEmployeeCountDataBefore = from con in contractors
                                                 join empCount in empCounts
                                                 on con.ContractorInn equals empCount.Tin
                                                 into grouping
                                                 from empCount in grouping.DefaultIfEmpty()
                                                 select new
                                                 {
                                                     Inn = con.ContractorInn,
                                                     Month = empCount?.Month ?? 0,
                                                     RegionId = con.RegionId,
                                                     DistrictId = con.DistrictId,
                                                     Year = empCount?.Year ?? 2023,
                                                     EmployeeCount = empCount?.EmployeeCount ?? 0,
                                                     ContractTypeId = con.ContractTypeId
                                                 };


                allEmployeeCountData = allEmployeeCountDataBefore.GroupBy(a => new { a.RegionId, a.ContractTypeId, a.Inn, a.DistrictId })
                    .Select(a => new
                    {
                        Inn = a.Key.Inn,
                        RegionId = a.Key.RegionId,
                        DistrictId = a.Key.DistrictId,
                        ContractTypeId = a.Key.ContractTypeId,
                        EmployeeCount_2023 = (
                                dto.Year == 2023
                                    ? (a.FirstOrDefault(b => b.Month == dto.Month && b.Year == 2023)?.EmployeeCount ?? 0)
                                    : (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2023)?.EmployeeCount ?? 0)
                                - (a.FirstOrDefault(b => b.Month == 5 && b.Year == 2023)?.EmployeeCount ?? 0)) > 0
                                                                                ? (dto.Year == 2023
                                    ? (a.FirstOrDefault(b => b.Month == dto.Month && b.Year == 2023)?.EmployeeCount ?? 0)
                                    : (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2023)?.EmployeeCount ?? 0) - (a.FirstOrDefault(b => b.Month == 5 && b.Year == 2023)?.EmployeeCount ?? 0))
                                                                                : 0,
                        EmployeeCount_2024 = (
                                dto.Year == 2024
                                    ? (a.FirstOrDefault(b => b.Month == dto.Month && b.Year == 2024)?.EmployeeCount ?? 0)
                                    : (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2024)?.EmployeeCount ?? 0)
                                - (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2023)?.EmployeeCount ?? 0)) > 0
                                                                                ? (dto.Year == 2024
                                        ? (a.FirstOrDefault(b => b.Month == dto.Month && b.Year == 2024)?.EmployeeCount ?? 0)
                                        : (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2024)?.EmployeeCount ?? 0)
                                    - (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2023)?.EmployeeCount ?? 0))
                                                                                : 0,
                        EmployeeCount_2025 = (dto.Year == 2025
                                    ? (a.FirstOrDefault(b => b.Month == dto.Month && b.Year == 2025)?.EmployeeCount ?? 0)
                                    : (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2025)?.EmployeeCount ?? 0) - (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2024)?.EmployeeCount ?? 0)) > 0
                                                                                ? (dto.Year == 2025
                                    ? (a.FirstOrDefault(b => b.Month == dto.Month && b.Year == 2025)?.EmployeeCount ?? 0)
                                    : (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2025)?.EmployeeCount ?? 0) - (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2024)?.EmployeeCount ?? 0))
                                                                                : 0,
                        EmployeeCount_2026 = (dto.Year == 2026
                                    ? (a.FirstOrDefault(b => b.Month == dto.Month && b.Year == 2026)?.EmployeeCount ?? 0)
                                    : (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2026)?.EmployeeCount ?? 0)
                                   - (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2025)?.EmployeeCount ?? 0)) > 0
                                                                                ? (dto.Year == 2026
                                    ? (a.FirstOrDefault(b => b.Month == dto.Month && b.Year == 2026)?.EmployeeCount ?? 0)
                                    : (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2026)?.EmployeeCount ?? 0) - (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2025)?.EmployeeCount ?? 0))
                                                                                : 0,
                    })
                    .GroupBy(a => new { a.RegionId, a.ContractTypeId, a.DistrictId })
                    .Select(a => new EmployeeCountDataHelper()
                    {
                        RegionId = a.Key.RegionId,
                        DistrictId = a.Key.DistrictId,
                        ContractTypeId = a.Key.ContractTypeId,
                        EmployeeCount_2023 = a.Sum(b => b.EmployeeCount_2023),
                        EmployeeCount_2024 = a.Sum(b => b.EmployeeCount_2024),
                        EmployeeCount_2025 = a.Sum(b => b.EmployeeCount_2025),
                        EmployeeCount_2026 = a.Sum(b => b.EmployeeCount_2026),
                        TotalEmployeeCount = a.Sum(b => b.EmployeeCount_2023 + b.EmployeeCount_2024 + b.EmployeeCount_2025 + b.EmployeeCount_2026),

                        Count_2023 = a.Where(b => b.EmployeeCount_2023 > 0).Count(),
                        Count_2024 = a.Where(b => b.EmployeeCount_2024 > 0).Count(),
                        Count_2025 = a.Where(b => b.EmployeeCount_2025 > 0).Count(),
                        Count_2026 = a.Where(b => b.EmployeeCount_2026 > 0).Count(),
                        TotalCount = a.Where(b => (b.EmployeeCount_2023 + b.EmployeeCount_2024 + b.EmployeeCount_2025 + b.EmployeeCount_2026) > 0).Count()
                    })
                    .ToArray();


                employeeCountInMay = allEmployeeCountDataBefore.Where(x => x.Month == 5).Select(a => new EmployeeCountInMay()
                {
                    RegionId = a.RegionId,
                    DistrictId = a.DistrictId,
                    Sum = a.EmployeeCount
                }).ToList();

            }

            PrtnApplicationByContractTypeDto appCert = new PrtnApplicationByContractTypeDto();

            if (!dto.RegionId.HasValue)
            {
                appCert = GetPrtnApplicationByContractType(new PrtnApplicationByContractTypeDtoFilter()
                {
                    ByRegion = true,
                    StartDate = dto.StartDate.HasValue ? dto.StartDate.Value : null,
                    EndDate = dto.EndDate.HasValue ? dto.EndDate.Value : null
                });
            }
            else
            {
                appCert = GetPrtnApplicationByContractType(new PrtnApplicationByContractTypeDtoFilter()
                {
                    ByRegion = false,
                    ByDistrict = true,
                    RegionId = dto.RegionId,
                    StartDate = dto.StartDate.HasValue ? dto.StartDate.Value : null,
                    EndDate = dto.EndDate.HasValue ? dto.EndDate.Value : null
                });
            }


            MemoryStream result = new MemoryStream();
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_EMPLOYEE_COUNT));

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);




            var importRow2 = excelPackage.Workbook.Names["ImportRow1"];
            int currentRow2 = importRow2.Start.Row;


            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;

            var namerange2 = excelPackage.Workbook.Names["ImportRow1"];
            var ws2 = namerange2.Worksheet;



            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row;
            int index = 1;
            int index2 = 1;

            if (dto.RegionId.HasValue)
            {
                var districts = _unitOfWork.Context.Districts.Where(a => a.RegionId == dto.RegionId).ToArray();
                foreach (var reg in districts)
                {
                    var column = 1;
                    var column2 = 1;
                    ws.InsertRow(currentRow, 1, importRow.Start.Row);
                    ws2.InsertRow(currentRow2, 1, importRow2.Start.Row);

                    ws.Cells[currentRow, column++].Value = index++;
                    ws2.Cells[currentRow2, column2++].Value = index2++;

                    ws.Cells[currentRow, column++].Value = reg.FullName;
                    ws2.Cells[currentRow2, column2++].Value = reg.FullName;

                    ws2.Cells[currentRow2, column2++].Value = appCert.Rows.FirstOrDefault(x => x.DistrictId == reg.Id).TotalApplication.TotalCount;
                    ws2.Cells[currentRow2, column2++].Value = appCert.Rows.FirstOrDefault(x => x.DistrictId == reg.Id).TotalCertificate.TotalCount;
                    ws2.Cells[currentRow2, column2++].Value = totalData.Where(a => a.DistrictId == reg.Id).Sum(a => a.TotalPlanEmpCount);
                    ws2.Cells[currentRow2, column2++].Value = totalData.Where(a => a.DistrictId == reg.Id).Sum(a => a.PlanEmpCount_2023);
                    ws2.Cells[currentRow2, column2++].Value = totalData.Where(a => a.DistrictId == reg.Id).Sum(a => a.PlanEmpCount_2024);
                    ws2.Cells[currentRow2, column2++].Value = totalData.Where(a => a.DistrictId == reg.Id).Sum(a => a.PlanEmpCount_2025);
                    ws2.Cells[currentRow2, column2++].Value = totalData.Where(a => a.DistrictId == reg.Id).Sum(a => a.PlanEmpCount_2026);

                    ws2.Cells[currentRow2, column2++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id).Sum(a => a.TotalEmployeeCount);
                    ws2.Cells[currentRow2, column2++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id).Sum(a => a.EmployeeCount_2023);
                    ws2.Cells[currentRow2, column2++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id).Sum(a => a.EmployeeCount_2024);
                    ws2.Cells[currentRow2, column2++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id).Sum(a => a.EmployeeCount_2025);
                    ws2.Cells[currentRow2, column2++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id).Sum(a => a.EmployeeCount_2026);


                    //1- bo'lim
                    ws.Cells[currentRow, column++].Value = employeeCountInMay.Where(a => a.DistrictId == reg.Id).Sum(a => a.Sum);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id).Sum(a => a.TotalPlanEmpCount);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalCount);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalPlanEmpCount);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalCount);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalPlanEmpCount);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalCount);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalPlanEmpCount);


                    //2-bo'lim
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id).Sum(a => a.TotalEmployeeCount);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalCount);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalEmployeeCount);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalCount);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalEmployeeCount);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalCount);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalEmployeeCount);


                    //3-bo'lim
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id).Sum(a => a.PlanEmpCount_2023);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2023);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2023);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2023);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2023);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2023);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2023);


                    //4-bo'lim
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id).Sum(a => a.EmployeeCount_2023);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2023);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2023);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2023);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2023);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2023);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2023);


                    //5-bo'lim
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id).Sum(a => a.PlanEmpCount_2024);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2024);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2024);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2024);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2024);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2024);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2024);


                    //6-bo'lim
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id).Sum(a => a.EmployeeCount_2024);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2024);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2024);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2024);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2024);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2024);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2024);


                    //7-bo'lim
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id).Sum(a => a.PlanEmpCount_2025);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2025);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2025);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2025);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2025);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2025);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2025);


                    ////8-bo'lim
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id).Sum(a => a.EmployeeCount_2025);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2025);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2025);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2025);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2025);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2025);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2025);


                    //9-bo'lim
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id).Sum(a => a.PlanEmpCount_2026);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2026);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2026);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2026);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2026);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2026);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2026);

                    //10-bo'lim
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id).Sum(a => a.EmployeeCount_2026);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2026);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2026);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2026);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2026);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2026);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.DistrictId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2026);

                    currentRow++;
                    currentRow2++;
                }
                ws.DeleteRow(importRow.Start.Row);
                ws2.DeleteRow(importRow2.Start.Row);
                var columnTotal = 3;
                var columnTotal2 = 3;
                currentRow = 5 + districts.Count();

                ws2.Cells[currentRow2, columnTotal2++].Value = appCert.Rows.Sum(x => x.TotalApplication.TotalCount);
                ws2.Cells[currentRow2, columnTotal2++].Value = appCert.Rows.Sum(x => x.TotalCertificate.TotalCount);
                ws2.Cells[currentRow2, columnTotal2++].Value = totalData.Sum(a => a.TotalPlanEmpCount);
                ws2.Cells[currentRow2, columnTotal2++].Value = totalData.Sum(a => a.PlanEmpCount_2023);
                ws2.Cells[currentRow2, columnTotal2++].Value = totalData.Sum(a => a.PlanEmpCount_2024);
                ws2.Cells[currentRow2, columnTotal2++].Value = totalData.Sum(a => a.PlanEmpCount_2025);
                ws2.Cells[currentRow2, columnTotal2++].Value = totalData.Sum(a => a.PlanEmpCount_2026);

                ws2.Cells[currentRow2, columnTotal2++].Value = allEmployeeCountData.Sum(a => a.TotalEmployeeCount);
                ws2.Cells[currentRow2, columnTotal2++].Value = allEmployeeCountData.Sum(a => a.EmployeeCount_2023);
                ws2.Cells[currentRow2, columnTotal2++].Value = allEmployeeCountData.Sum(a => a.EmployeeCount_2024);
                ws2.Cells[currentRow2, columnTotal2++].Value = allEmployeeCountData.Sum(a => a.EmployeeCount_2025);
                ws2.Cells[currentRow2, columnTotal2++].Value = allEmployeeCountData.Sum(a => a.EmployeeCount_2026);

                //1
                ws.Cells[currentRow, columnTotal++].Value = employeeCountInMay.Sum(a => a.Sum);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Sum(a => a.TotalPlanEmpCount);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(item => item.TotalCount);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalPlanEmpCount);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(item => item.TotalCount);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalPlanEmpCount);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(item => item.TotalCount);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalPlanEmpCount);

                //2
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Sum(a => a.TotalEmployeeCount);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalCount);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalEmployeeCount);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalCount);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalEmployeeCount);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalCount);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalEmployeeCount);

                //3
                ws.Cells[currentRow, columnTotal++].Value = totalData.Sum(a => a.PlanEmpCount_2023);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(item => item.Count_2023);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2023);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(item => item.Count_2023);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2023);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(item => item.Count_2023);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2023);

                //4
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Sum(a => a.EmployeeCount_2023);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2023);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2023);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2023);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2023);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2023);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2023);

                //5
                ws.Cells[currentRow, columnTotal++].Value = totalData.Sum(a => a.PlanEmpCount_2024);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(item => item.Count_2024);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2024);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(item => item.Count_2024);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2024);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(item => item.Count_2024);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2024);

                //6
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Sum(a => a.EmployeeCount_2024);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2024);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2024);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2024);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2024);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2024);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2024);

                //7
                ws.Cells[currentRow, columnTotal++].Value = totalData.Sum(a => a.PlanEmpCount_2025);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(item => item.Count_2025);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2025);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(item => item.Count_2025);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2025);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(item => item.Count_2025);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2025);

                //8
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Sum(a => a.EmployeeCount_2025);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2025);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2025);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2025);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2025);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2025);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2025);

                //9
                ws.Cells[currentRow, columnTotal++].Value = totalData.Sum(a => a.PlanEmpCount_2026);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(item => item.Count_2026);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2026);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(item => item.Count_2026);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2026);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(item => item.Count_2026);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2026);

                //10
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Sum(a => a.EmployeeCount_2026);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2026);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2026);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2026);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2026);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2026);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2026);

                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
                result.Position = 0;
                return result;
            }
            else
            {
                foreach (var reg in _unitOfWork.Context.Regions)
                {
                    var column = 1;
                    var column2 = 1;
                    ws.InsertRow(currentRow, 1, importRow.Start.Row);
                    ws2.InsertRow(currentRow2, 1, importRow2.Start.Row);

                    ws.Cells[currentRow, column++].Value = index++;
                    ws2.Cells[currentRow2, column2++].Value = index2++;

                    ws.Cells[currentRow, column++].Value = reg.FullName;
                    ws2.Cells[currentRow2, column2++].Value = reg.FullName;

                    ws2.Cells[currentRow2, column2++].Value = appCert.Rows.FirstOrDefault(x => x.RegionId == reg.Id).TotalApplication.TotalCount;
                    ws2.Cells[currentRow2, column2++].Value = appCert.Rows.FirstOrDefault(x => x.RegionId == reg.Id).TotalCertificate.TotalCount;
                    ws2.Cells[currentRow2, column2++].Value = totalData.Where(a => a.RegionId == reg.Id).Sum(a => a.TotalPlanEmpCount);
                    ws2.Cells[currentRow2, column2++].Value = totalData.Where(a => a.RegionId == reg.Id).Sum(a => a.PlanEmpCount_2023);
                    ws2.Cells[currentRow2, column2++].Value = totalData.Where(a => a.RegionId == reg.Id).Sum(a => a.PlanEmpCount_2024);
                    ws2.Cells[currentRow2, column2++].Value = totalData.Where(a => a.RegionId == reg.Id).Sum(a => a.PlanEmpCount_2025);
                    ws2.Cells[currentRow2, column2++].Value = totalData.Where(a => a.RegionId == reg.Id).Sum(a => a.PlanEmpCount_2026);

                    ws2.Cells[currentRow2, column2++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id).Sum(a => a.TotalEmployeeCount);
                    ws2.Cells[currentRow2, column2++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id).Sum(a => a.EmployeeCount_2023);
                    ws2.Cells[currentRow2, column2++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id).Sum(a => a.EmployeeCount_2024);
                    ws2.Cells[currentRow2, column2++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id).Sum(a => a.EmployeeCount_2025);
                    ws2.Cells[currentRow2, column2++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id).Sum(a => a.EmployeeCount_2026);


                    //1- bo'lim
                    ws.Cells[currentRow, column++].Value = employeeCountInMay.Where(a => a.RegionId == reg.Id).Sum(a => a.Sum);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id).Sum(a => a.TotalPlanEmpCount);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalCount);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalPlanEmpCount);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalCount);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalPlanEmpCount);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalCount);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalPlanEmpCount);


                    //2-bo'lim
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id).Sum(a => a.TotalEmployeeCount);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalCount);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalEmployeeCount);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalCount);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalEmployeeCount);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalCount);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalEmployeeCount);


                    //3-bo'lim
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id).Sum(a => a.PlanEmpCount_2023);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2023);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2023);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2023);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2023);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2023);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2023);


                    //4-bo'lim
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id).Sum(a => a.EmployeeCount_2023);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2023);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2023);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2023);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2023);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2023);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2023);


                    //5-bo'lim
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id).Sum(a => a.PlanEmpCount_2024);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2024);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2024);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2024);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2024);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2024);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2024);


                    //6-bo'lim
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id).Sum(a => a.EmployeeCount_2024);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2024);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2024);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2024);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2024);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2024);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2024);


                    //7-bo'lim
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id).Sum(a => a.PlanEmpCount_2025);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2025);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2025);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2025);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2025);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2025);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2025);


                    ////8-bo'lim
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id).Sum(a => a.EmployeeCount_2025);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2025);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2025);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2025);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2025);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2025);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2025);


                    //9-bo'lim
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id).Sum(a => a.PlanEmpCount_2026);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2026);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2026);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2026);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2026);

                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2026);
                    ws.Cells[currentRow, column++].Value = totalData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2026);

                    //10-bo'lim
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id).Sum(a => a.EmployeeCount_2026);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2026);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2026);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2026);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2026);

                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2026);
                    ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.RegionId == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2026);

                    currentRow++;
                    currentRow2++;
                }
                ws.DeleteRow(importRow.Start.Row);
                ws2.DeleteRow(importRow2.Start.Row);
                var columnTotal = 3;
                var columnTotal2 = 3;
                currentRow = 19;

                ws2.Cells[currentRow2, columnTotal2++].Value = appCert.Rows.Sum(x => x.TotalApplication.TotalCount);
                ws2.Cells[currentRow2, columnTotal2++].Value = appCert.Rows.Sum(x => x.TotalCertificate.TotalCount);
                ws2.Cells[currentRow2, columnTotal2++].Value = totalData.Sum(a => a.TotalPlanEmpCount);
                ws2.Cells[currentRow2, columnTotal2++].Value = totalData.Sum(a => a.PlanEmpCount_2023);
                ws2.Cells[currentRow2, columnTotal2++].Value = totalData.Sum(a => a.PlanEmpCount_2024);
                ws2.Cells[currentRow2, columnTotal2++].Value = totalData.Sum(a => a.PlanEmpCount_2025);
                ws2.Cells[currentRow2, columnTotal2++].Value = totalData.Sum(a => a.PlanEmpCount_2026);

                ws2.Cells[currentRow2, columnTotal2++].Value = allEmployeeCountData.Sum(a => a.TotalEmployeeCount);
                ws2.Cells[currentRow2, columnTotal2++].Value = allEmployeeCountData.Sum(a => a.EmployeeCount_2023);
                ws2.Cells[currentRow2, columnTotal2++].Value = allEmployeeCountData.Sum(a => a.EmployeeCount_2024);
                ws2.Cells[currentRow2, columnTotal2++].Value = allEmployeeCountData.Sum(a => a.EmployeeCount_2025);
                ws2.Cells[currentRow2, columnTotal2++].Value = allEmployeeCountData.Sum(a => a.EmployeeCount_2026);

                //1
                ws.Cells[currentRow, columnTotal++].Value = employeeCountInMay.Sum(a => a.Sum);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Sum(a => a.TotalPlanEmpCount);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(item => item.TotalCount);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalPlanEmpCount);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(item => item.TotalCount);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalPlanEmpCount);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(item => item.TotalCount);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalPlanEmpCount);

                //2
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Sum(a => a.TotalEmployeeCount);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalCount);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalEmployeeCount);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalCount);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalEmployeeCount);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalCount);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalEmployeeCount);

                //3
                ws.Cells[currentRow, columnTotal++].Value = totalData.Sum(a => a.PlanEmpCount_2023);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(item => item.Count_2023);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2023);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(item => item.Count_2023);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2023);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(item => item.Count_2023);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2023);

                //4
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Sum(a => a.EmployeeCount_2023);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2023);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2023);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2023);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2023);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2023);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2023);

                //5
                ws.Cells[currentRow, columnTotal++].Value = totalData.Sum(a => a.PlanEmpCount_2024);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(item => item.Count_2024);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2024);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(item => item.Count_2024);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2024);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(item => item.Count_2024);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2024);

                //6
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Sum(a => a.EmployeeCount_2024);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2024);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2024);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2024);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2024);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2024);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2024);

                //7
                ws.Cells[currentRow, columnTotal++].Value = totalData.Sum(a => a.PlanEmpCount_2025);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(item => item.Count_2025);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2025);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(item => item.Count_2025);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2025);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(item => item.Count_2025);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2025);

                //8
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Sum(a => a.EmployeeCount_2025);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2025);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2025);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2025);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2025);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2025);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2025);

                //9
                ws.Cells[currentRow, columnTotal++].Value = totalData.Sum(a => a.PlanEmpCount_2026);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(item => item.Count_2026);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2026);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(item => item.Count_2026);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2026);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(item => item.Count_2026);
                ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2026);

                //10
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Sum(a => a.EmployeeCount_2026);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2026);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2026);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2026);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2026);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2026);
                ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2026);
            }

            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
            result.Position = 0;
            return result;
        }
        else
        {
            var contractors = _unitOfWork.Context.Set<Contractor>()
               .Where(a => a.Applications.Any(a => a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED && a.StatusId == StatusIdConst.ACCEPTED))
               .AsNoTracking()
               .Select(a => new
               {
                   Id = a.Id,
                   Inn = a.Inn,
                   RegionId = a.RegionId,
                   ContractorName = a.Inn + "-" + a.FullName,
                   Application = a.Applications
                                   .FirstOrDefault(a => a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED && a.StatusId == StatusIdConst.ACCEPTED)
               })
               .Select(a => new
               {
                   Id = a.Id,
                   RegionId = a.Application
                                           .PrtnApplication.ChoosedRegionId
                                                ?? a.RegionId,
                   ContractorName = a.ContractorName,
                   ContractorInn = a.Inn,
                   ContractTypeId = a.Application.PrtnContract.PrtnCertificate.PrtnContractTypeId,
                   PlanEmpCount_2023 = a.Application.PrtnApplication.Graphs.Where(a => a.YearIn == 2023).Sum(b => b.NewVacanciesCount),
                   PlanEmpCount_2024 = a.Application.PrtnApplication.Graphs.Where(a => a.YearIn == 2024).Sum(b => b.NewVacanciesCount),
                   PlanEmpCount_2025 = a.Application.PrtnApplication.Graphs.Where(a => a.YearIn == 2025).Sum(b => b.NewVacanciesCount),
                   PlanEmpCount_2026 = a.Application.PrtnApplication.Graphs.Where(a => a.YearIn == 2026).Sum(b => b.NewVacanciesCount),
               }).ToArray();

            if (dto.ContractorTypeId.HasValue)
                contractors = contractors.Where(a => a.ContractTypeId == dto.ContractorTypeId).ToArray();

            if (dto.RegionId.HasValue)
                contractors = contractors.Where(a => a.RegionId == dto.RegionId).ToArray();


            var totalData = contractors.GroupBy(a => new { a.ContractTypeId, a.ContractorName, a.Id }).Select(a => new
            {
                ContractTypeId = a.Key.ContractTypeId,
                ContractorName = a.Key.ContractorName,
                Id = a.Key.Id,
                PlanEmpCount_2023 = a.Sum(b => b.PlanEmpCount_2023),
                PlanEmpCount_2024 = a.Sum(b => b.PlanEmpCount_2024),
                PlanEmpCount_2025 = a.Sum(b => b.PlanEmpCount_2025),
                PlanEmpCount_2026 = a.Sum(b => b.PlanEmpCount_2026),
                TotalPlanEmpCount = a.Sum(b => b.PlanEmpCount_2023 + b.PlanEmpCount_2024 + b.PlanEmpCount_2025 + b.PlanEmpCount_2026),

                Count_2023 = a.Where(b => b.PlanEmpCount_2023 > 0).Count(),
                Count_2024 = a.Where(b => b.PlanEmpCount_2024 > 0).Count(),
                Count_2025 = a.Where(b => b.PlanEmpCount_2025 > 0).Count(),
                Count_2026 = a.Where(b => b.PlanEmpCount_2026 > 0).Count(),
                TotalCount = a.Where(b => (b.PlanEmpCount_2023 + b.PlanEmpCount_2024 + b.PlanEmpCount_2025 + b.PlanEmpCount_2026) > 0).Count()
            }).ToArray();
            EmployeeCountDataHelper[] allEmployeeCountData = new EmployeeCountDataHelper[] { };
            var employeeCountInMay = new List<EmployeeCountInMay>();

            if (!dto.Month.IsNullOrEmptyObject() && !dto.Year.IsNullOrEmptyObject())
            {
                var empCounts = _unitOfWork.Context.EmployeeCounts
                    .GroupBy(a => new { a.Tin, a.Year, a.Month })
                    .Select(a => new { Tin = a.Key.Tin, Year = a.Key.Year, EmployeeCount = a.Sum(a => a.MonthlyNumberEmployees), a.Key.Month })
                    .ToArray();

                var allEmployeeCountDataBefore = from con in contractors
                                                 join empCount in empCounts
                                                 on con.ContractorInn equals empCount.Tin
                                                 into grouping
                                                 from empCount in grouping.DefaultIfEmpty()
                                                 select new
                                                 {
                                                     Inn = con.ContractorInn,
                                                     Id = con.Id,
                                                     Month = empCount?.Month ?? 0,
                                                     Year = empCount?.Year ?? 2023,
                                                     EmployeeCount = empCount?.EmployeeCount ?? 0,
                                                     ContractTypeId = con.ContractTypeId
                                                 };


                allEmployeeCountData = allEmployeeCountDataBefore.GroupBy(a => new { a.ContractTypeId, a.Inn, a.Id })
                    .Select(a => new
                    {
                        Id = a.Key.Id,
                        Inn = a.Key.Inn,
                        ContractTypeId = a.Key.ContractTypeId,
                        EmployeeCount_2023 = (
                                dto.Year == 2023
                                    ? (a.FirstOrDefault(b => b.Month == dto.Month && b.Year == 2023)?.EmployeeCount ?? 0)
                                    : (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2023)?.EmployeeCount ?? 0)
                                - (a.FirstOrDefault(b => b.Month == 5 && b.Year == 2023)?.EmployeeCount ?? 0)) > 0
                                                                                ? (dto.Year == 2023
                                    ? (a.FirstOrDefault(b => b.Month == dto.Month && b.Year == 2023)?.EmployeeCount ?? 0)
                                    : (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2023)?.EmployeeCount ?? 0) - (a.FirstOrDefault(b => b.Month == 5 && b.Year == 2023)?.EmployeeCount ?? 0))
                                                                                : 0,
                        EmployeeCount_2024 = (
                                dto.Year == 2024
                                    ? (a.FirstOrDefault(b => b.Month == dto.Month && b.Year == 2024)?.EmployeeCount ?? 0)
                                    : (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2024)?.EmployeeCount ?? 0)
                                - (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2023)?.EmployeeCount ?? 0)) > 0
                                                                                ? (dto.Year == 2024
                                        ? (a.FirstOrDefault(b => b.Month == dto.Month && b.Year == 2024)?.EmployeeCount ?? 0)
                                        : (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2024)?.EmployeeCount ?? 0)
                                    - (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2023)?.EmployeeCount ?? 0))
                                                                                : 0,
                        EmployeeCount_2025 = (dto.Year == 2025
                                    ? (a.FirstOrDefault(b => b.Month == dto.Month && b.Year == 2025)?.EmployeeCount ?? 0)
                                    : (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2025)?.EmployeeCount ?? 0) - (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2024)?.EmployeeCount ?? 0)) > 0
                                                                                ? (dto.Year == 2025
                                    ? (a.FirstOrDefault(b => b.Month == dto.Month && b.Year == 2025)?.EmployeeCount ?? 0)
                                    : (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2025)?.EmployeeCount ?? 0) - (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2024)?.EmployeeCount ?? 0))
                                                                                : 0,
                        EmployeeCount_2026 = (dto.Year == 2026
                                    ? (a.FirstOrDefault(b => b.Month == dto.Month && b.Year == 2026)?.EmployeeCount ?? 0)
                                    : (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2026)?.EmployeeCount ?? 0)
                                   - (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2025)?.EmployeeCount ?? 0)) > 0
                                                                                ? (dto.Year == 2026
                                    ? (a.FirstOrDefault(b => b.Month == dto.Month && b.Year == 2026)?.EmployeeCount ?? 0)
                                    : (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2026)?.EmployeeCount ?? 0) - (a.FirstOrDefault(b => b.Month == 12 && b.Year == 2025)?.EmployeeCount ?? 0))
                                                                                : 0,
                    })
                    .GroupBy(a => new { a.ContractTypeId, a.Id })
                    .Select(a => new EmployeeCountDataHelper()
                    {
                        Id = a.Key.Id,
                        ContractTypeId = a.Key.ContractTypeId,
                        EmployeeCount_2023 = a.Sum(b => b.EmployeeCount_2023),
                        EmployeeCount_2024 = a.Sum(b => b.EmployeeCount_2024),
                        EmployeeCount_2025 = a.Sum(b => b.EmployeeCount_2025),
                        EmployeeCount_2026 = a.Sum(b => b.EmployeeCount_2026),
                        TotalEmployeeCount = a.Sum(b => b.EmployeeCount_2023 + b.EmployeeCount_2024 + b.EmployeeCount_2025 + b.EmployeeCount_2026),

                        Count_2023 = a.Where(b => b.EmployeeCount_2023 > 0).Count(),
                        Count_2024 = a.Where(b => b.EmployeeCount_2024 > 0).Count(),
                        Count_2025 = a.Where(b => b.EmployeeCount_2025 > 0).Count(),
                        Count_2026 = a.Where(b => b.EmployeeCount_2026 > 0).Count(),
                        TotalCount = a.Where(b => (b.EmployeeCount_2023 + b.EmployeeCount_2024 + b.EmployeeCount_2025 + b.EmployeeCount_2026) > 0).Count()
                    })
                    .ToArray();


                employeeCountInMay = allEmployeeCountDataBefore.Where(x => x.Month == 5).Select(a => new EmployeeCountInMay()
                {
                    Sum = a.EmployeeCount
                }).ToList();

            }

            PrtnApplicationByContractTypeDto appCert = new PrtnApplicationByContractTypeDto();

            appCert = GetPrtnApplicationByContractType(new PrtnApplicationByContractTypeDtoFilter()
            {
                ByContractor = true,
                StartDate = dto.StartDate.HasValue ? dto.StartDate.Value : null,
                EndDate = dto.EndDate.HasValue ? dto.EndDate.Value : null
            });


            MemoryStream result = new MemoryStream();
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_EMPLOYEE_COUNT));

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);




            var importRow2 = excelPackage.Workbook.Names["ImportRow1"];
            int currentRow2 = importRow2.Start.Row;


            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;

            var namerange2 = excelPackage.Workbook.Names["ImportRow1"];
            var ws2 = namerange2.Worksheet;



            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row;
            int index = 1;
            int index2 = 1;

            foreach (var reg in contractors)
            {
                var column = 1;
                var column2 = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);

                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value = reg.ContractorName;


                //1- bo'lim
                ws.Cells[currentRow, column++].Value = employeeCountInMay.Where(a => a.DistrictId == reg.Id).Sum(a => a.Sum);
                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id).Sum(a => a.TotalPlanEmpCount);

                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalCount);
                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalPlanEmpCount);

                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalCount);
                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalPlanEmpCount);

                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalCount);
                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalPlanEmpCount);


                //2-bo'lim
                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id).Sum(a => a.TotalEmployeeCount);

                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalCount);
                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalEmployeeCount);

                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalCount);
                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalEmployeeCount);

                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalCount);
                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalEmployeeCount);


                //3-bo'lim
                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id).Sum(a => a.PlanEmpCount_2023);

                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2023);
                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2023);

                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2023);
                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2023);

                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2023);
                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2023);


                //4-bo'lim
                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id).Sum(a => a.EmployeeCount_2023);

                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2023);
                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2023);

                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2023);
                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2023);

                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2023);
                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2023);


                //5-bo'lim
                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id).Sum(a => a.PlanEmpCount_2024);

                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2024);
                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2024);

                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2024);
                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2024);

                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2024);
                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2024);


                //6-bo'lim
                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id).Sum(a => a.EmployeeCount_2024);

                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2024);
                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2024);

                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2024);
                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2024);

                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2024);
                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2024);


                //7-bo'lim
                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id).Sum(a => a.PlanEmpCount_2025);

                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2025);
                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2025);

                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2025);
                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2025);

                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2025);
                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2025);


                ////8-bo'lim
                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id).Sum(a => a.EmployeeCount_2025);

                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2025);
                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2025);

                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2025);
                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2025);

                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2025);
                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2025);


                //9-bo'lim
                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id).Sum(a => a.PlanEmpCount_2026);

                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2026);
                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2026);

                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2026);
                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2026);

                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2026);
                ws.Cells[currentRow, column++].Value = totalData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2026);

                //10-bo'lim
                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id).Sum(a => a.EmployeeCount_2026);

                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2026);
                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2026);

                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2026);
                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2026);

                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2026);
                ws.Cells[currentRow, column++].Value = allEmployeeCountData.Where(a => a.Id == reg.Id && a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2026);

                currentRow++;
                currentRow2++;
            }
            ws.DeleteRow(importRow.Start.Row);
            var columnTotal = 3;
            currentRow = 5 + contractors.Count();

            //1
            ws.Cells[currentRow, columnTotal++].Value = employeeCountInMay.Sum(a => a.Sum);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Sum(a => a.TotalPlanEmpCount);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(item => item.TotalCount);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalPlanEmpCount);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(item => item.TotalCount);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalPlanEmpCount);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(item => item.TotalCount);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalPlanEmpCount);

            //2
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Sum(a => a.TotalEmployeeCount);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalCount);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.TotalEmployeeCount);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalCount);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.TotalEmployeeCount);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalCount);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.TotalEmployeeCount);

            //3
            ws.Cells[currentRow, columnTotal++].Value = totalData.Sum(a => a.PlanEmpCount_2023);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(item => item.Count_2023);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2023);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(item => item.Count_2023);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2023);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(item => item.Count_2023);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2023);

            //4
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Sum(a => a.EmployeeCount_2023);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2023);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2023);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2023);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2023);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2023);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2023);

            //5
            ws.Cells[currentRow, columnTotal++].Value = totalData.Sum(a => a.PlanEmpCount_2024);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(item => item.Count_2024);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2024);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(item => item.Count_2024);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2024);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(item => item.Count_2024);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2024);

            //6
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Sum(a => a.EmployeeCount_2024);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2024);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2024);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2024);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2024);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2024);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2024);

            //7
            ws.Cells[currentRow, columnTotal++].Value = totalData.Sum(a => a.PlanEmpCount_2025);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(item => item.Count_2025);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2025);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(item => item.Count_2025);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2025);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(item => item.Count_2025);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2025);

            //8
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Sum(a => a.EmployeeCount_2025);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2025);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2025);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2025);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2025);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2025);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2025);

            //9
            ws.Cells[currentRow, columnTotal++].Value = totalData.Sum(a => a.PlanEmpCount_2026);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(item => item.Count_2026);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.PlanEmpCount_2026);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(item => item.Count_2026);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.PlanEmpCount_2026);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(item => item.Count_2026);
            ws.Cells[currentRow, columnTotal++].Value = totalData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.PlanEmpCount_2026);

            //10
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Sum(a => a.EmployeeCount_2026);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.Count_2026);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._50_100).Sum(a => a.EmployeeCount_2026);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.Count_2026);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._101_200).Sum(a => a.EmployeeCount_2026);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.Count_2026);
            ws.Cells[currentRow, columnTotal++].Value = allEmployeeCountData.Where(a => a.ContractTypeId == PrtnContractTypeIdConst._201__).Sum(a => a.EmployeeCount_2026);

            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
            result.Position = 0;
            return result;

        }

    }
    public Stream SaveAsExcelTaxReport(TaxCreditReportDtoFilter dto)
    {
        int? curContractType = dto.ContarctTypeId;
        var dtoContractTtypeId = dto.ContarctTypeId;
        dto.ContarctTypeId = PrtnContractTypeIdConst._50_100;
        var dataType_1 = GetTaxCreditReport(dto).ToList();
        dto.ContarctTypeId = PrtnContractTypeIdConst._101_200;
        var dataType_2 = GetTaxCreditReport(dto).ToList();
        dto.ContarctTypeId = PrtnContractTypeIdConst._201__;
        var dataType_3 = GetTaxCreditReport(dto).ToList();

        MemoryStream result = new MemoryStream();
        if (dto.Tab == 1)
        {
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_TAX_MOLMULK_SOLIQ));

            if (IsValid && dataType_1 != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excelPackage = new ExcelPackage(template);

                var ws = excelPackage.Workbook.Worksheets[0];

                var importRow = excelPackage.Workbook.Names["ImportRow"];
                int currentRow = importRow.Start.Row;
                int index = 1;

                var importTotalRow = excelPackage.Workbook.Names["ImportTotalRow"];
                int currentTotalRow = importTotalRow.Start.Row;
                var columnTotal = 3;
                ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.PropertyTaxCount) + dataType_2.Sum(a => a.PropertyTaxCount) + dataType_3.Sum(a => a.PropertyTaxCount)).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.ContractorPropertyTaxCount) + dataType_2.Sum(a => a.ContractorPropertyTaxCount) + dataType_3.Sum(a => a.ContractorPropertyTaxCount)).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.PropertyNewVacanciesCount) + dataType_2.Sum(a => a.PropertyNewVacanciesCount) + dataType_3.Sum(a => a.PropertyNewVacanciesCount)).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = 0.ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.PropertyTaxSum) + dataType_2.Sum(a => a.PropertyTaxSum) + dataType_3.Sum(a => a.PropertyTaxSum)).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.PropertyTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.ContractorPropertyTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.PropertyNewVacanciesCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = 0.ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.PropertyTaxSum).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.PropertyTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.ContractorPropertyTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.PropertyNewVacanciesCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = 0.ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.PropertyTaxSum).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.PropertyTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.ContractorPropertyTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.PropertyNewVacanciesCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = 0.ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.PropertyTaxSum).ToString();

                for (int i = 0; i < dataType_1.Count; i++)
                {
                    var column = 1;
                    ws.InsertRow(currentRow, 1, importRow.Start.Row);
                    ws.Cells[currentRow, column++].Value = index++;
                    if (dto.ByDistrict == true)
                        ws.Cells[currentRow, column++].Value = dataType_1[i].District;
                    else if (dto.ByContractor == true)
                        ws.Cells[currentRow, column++].Value = dataType_1[i].ContractorInn + " - " + dataType_1[i].ContractorFullName;
                    else
                        ws.Cells[currentRow, column++].Value = dataType_1[i].Region;

                    ws.Cells[currentRow, column++].Value = (dataType_1[i].PropertyTaxCount + dataType_2[i].PropertyTaxCount + dataType_3[i].PropertyTaxCount).ToString();
                    ws.Cells[currentRow, column++].Value = (dataType_1[i].ContractorPropertyTaxCount + dataType_2[i].ContractorPropertyTaxCount + dataType_3[i].ContractorPropertyTaxCount).ToString();
                    ws.Cells[currentRow, column++].Value = (dataType_1[i].PropertyNewVacanciesCount + dataType_2[i].PropertyNewVacanciesCount + dataType_3[i].PropertyNewVacanciesCount).ToString();
                    ws.Cells[currentRow, column++].Value = 0;
                    ws.Cells[currentRow, column++].Value = (dataType_1[i].PropertyTaxSum + dataType_2[i].PropertyTaxSum + dataType_3[i].PropertyTaxSum).ToString();

                    ws.Cells[currentRow, column++].Value = dataType_1[i].PropertyTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_1[i].ContractorPropertyTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_1[i].PropertyNewVacanciesCount;
                    ws.Cells[currentRow, column++].Value = 0;
                    ws.Cells[currentRow, column++].Value = dataType_1[i].PropertyTaxSum;
                    ws.Cells[currentRow, column++].Value = dataType_2[i].PropertyTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_2[i].ContractorPropertyTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_2[i].PropertyNewVacanciesCount;
                    ws.Cells[currentRow, column++].Value = 0;
                    ws.Cells[currentRow, column++].Value = dataType_2[i].PropertyTaxSum;
                    ws.Cells[currentRow, column++].Value = dataType_3[i].PropertyTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_3[i].ContractorPropertyTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_3[i].PropertyNewVacanciesCount;
                    ws.Cells[currentRow, column++].Value = 0;
                    ws.Cells[currentRow, column++].Value = dataType_3[i].PropertyTaxSum;
                    currentRow++;
                }
                ws.DeleteRow(importRow.Start.Row);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }
        }
        else if (dto.Tab == 2)
        {
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_TAX_YER_SOLIQ));

            if (IsValid && dataType_1 != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excelPackage = new ExcelPackage(template);

                var ws = excelPackage.Workbook.Worksheets[0];

                var importRow = excelPackage.Workbook.Names["ImportRow"];
                int currentRow = importRow.Start.Row;
                int index = 1;

                var importTotalRow = excelPackage.Workbook.Names["ImportTotalRow"];
                int currentTotalRow = importTotalRow.Start.Row;
                var columnTotal = 3;
                ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.LandTaxCount) + dataType_2.Sum(a => a.LandTaxCount) + dataType_3.Sum(a => a.LandTaxCount)).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.LandNewVacanciesCount) + dataType_2.Sum(a => a.LandNewVacanciesCount) + dataType_3.Sum(a => a.LandNewVacanciesCount)).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = 0.ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.LandTaxSum) + dataType_2.Sum(a => a.LandTaxSum) + dataType_3.Sum(a => a.LandTaxSum)).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.LandTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.LandNewVacanciesCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = 0.ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.LandTaxSum).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.LandTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.LandNewVacanciesCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = 0.ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.LandTaxSum).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.LandTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.LandNewVacanciesCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = 0.ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.LandTaxSum).ToString();

                for (int i = 0; i < dataType_1.Count; i++)
                {
                    var column = 1;
                    ws.InsertRow(currentRow, 1, importRow.Start.Row);
                    ws.Cells[currentRow, column++].Value = index++;
                    if (dto.ByDistrict == true)
                        ws.Cells[currentRow, column++].Value = dataType_1[i].District;
                    else if (dto.ByContractor == true)
                        ws.Cells[currentRow, column++].Value = dataType_1[i].ContractorInn + " - " + dataType_1[i].ContractorFullName;
                    else
                        ws.Cells[currentRow, column++].Value = dataType_1[i].Region;

                    ws.Cells[currentRow, column++].Value = (dataType_1[i].LandTaxCount + dataType_2[i].LandTaxCount + dataType_3[i].LandTaxCount).ToString();
                    ws.Cells[currentRow, column++].Value = (dataType_1[i].LandNewVacanciesCount + dataType_2[i].LandNewVacanciesCount + dataType_3[i].LandNewVacanciesCount).ToString();
                    ws.Cells[currentRow, column++].Value = 0;
                    ws.Cells[currentRow, column++].Value = (dataType_1[i].LandTaxSum + dataType_2[i].LandTaxSum + dataType_3[i].LandTaxSum).ToString();

                    ws.Cells[currentRow, column++].Value = dataType_1[i].LandTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_1[i].LandNewVacanciesCount;
                    ws.Cells[currentRow, column++].Value = 0;
                    ws.Cells[currentRow, column++].Value = dataType_1[i].LandTaxSum;

                    ws.Cells[currentRow, column++].Value = dataType_2[i].LandTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_2[i].LandNewVacanciesCount;
                    ws.Cells[currentRow, column++].Value = 0;
                    ws.Cells[currentRow, column++].Value = dataType_2[i].LandTaxSum;

                    ws.Cells[currentRow, column++].Value = dataType_3[i].LandTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_3[i].LandNewVacanciesCount;
                    ws.Cells[currentRow, column++].Value = 0;
                    ws.Cells[currentRow, column++].Value = dataType_3[i].LandTaxSum;
                    currentRow++;
                }
                ws.DeleteRow(importRow.Start.Row);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }
        }
        else if (dto.Tab == 3)
        {
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_TAX_DAROMAD_SOLIQ));

            if (IsValid && dataType_1 != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excelPackage = new ExcelPackage(template);

                var ws = excelPackage.Workbook.Worksheets[0];

                var importRow = excelPackage.Workbook.Names["ImportRow"];
                int currentRow = importRow.Start.Row;
                int index = 1;

                var importTotalRow = excelPackage.Workbook.Names["ImportTotalRow"];
                int currentTotalRow = importTotalRow.Start.Row;
                var columnTotal = 3;
                ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.IncomeTaxCount) + dataType_2.Sum(a => a.IncomeTaxCount) + dataType_3.Sum(a => a.IncomeTaxCount)).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = 0.ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.IncomeTaxSum) + dataType_2.Sum(a => a.IncomeTaxSum) + dataType_3.Sum(a => a.IncomeTaxSum)).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = 0.ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.IncomeTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = 0.ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.IncomeTaxSum).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = 0.ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.IncomeTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = 0.ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.IncomeTaxSum).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = 0.ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.IncomeTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = 0.ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.IncomeTaxSum).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = 0.ToString();

                for (int i = 0; i < dataType_1.Count; i++)
                {
                    var column = 1;
                    ws.InsertRow(currentRow, 1, importRow.Start.Row);
                    ws.Cells[currentRow, column++].Value = index++;
                    if (dto.ByDistrict == true)
                        ws.Cells[currentRow, column++].Value = dataType_1[i].District;
                    else if (dto.ByContractor == true)
                        ws.Cells[currentRow, column++].Value = dataType_1[i].ContractorInn + " - " + dataType_1[i].ContractorFullName;
                    else
                        ws.Cells[currentRow, column++].Value = dataType_1[i].Region;

                    ws.Cells[currentRow, column++].Value = (dataType_1[i].IncomeTaxCount + dataType_2[i].IncomeTaxCount + dataType_3[i].IncomeTaxCount).ToString();
                    ws.Cells[currentRow, column++].Value = 0;
                    ws.Cells[currentRow, column++].Value = (dataType_1[i].IncomeTaxSum + dataType_2[i].IncomeTaxSum + dataType_3[i].IncomeTaxSum).ToString();
                    ws.Cells[currentRow, column++].Value = 0;

                    ws.Cells[currentRow, column++].Value = dataType_1[i].IncomeTaxCount;
                    ws.Cells[currentRow, column++].Value = 0;
                    ws.Cells[currentRow, column++].Value = dataType_1[i].IncomeTaxSum;
                    ws.Cells[currentRow, column++].Value = 0;

                    ws.Cells[currentRow, column++].Value = dataType_1[i].IncomeTaxCount;
                    ws.Cells[currentRow, column++].Value = 0;
                    ws.Cells[currentRow, column++].Value = dataType_1[i].IncomeTaxSum;
                    ws.Cells[currentRow, column++].Value = 0;

                    ws.Cells[currentRow, column++].Value = dataType_2[i].IncomeTaxCount;
                    ws.Cells[currentRow, column++].Value = 0;
                    ws.Cells[currentRow, column++].Value = dataType_2[i].IncomeTaxSum;
                    ws.Cells[currentRow, column++].Value = 0;

                    ws.Cells[currentRow, column++].Value = dataType_3[i].IncomeTaxCount;
                    ws.Cells[currentRow, column++].Value = 0;
                    ws.Cells[currentRow, column++].Value = dataType_3[i].IncomeTaxSum;
                    ws.Cells[currentRow, column++].Value = dataType_3[i].IncomeNewVacanciesCount;
                    currentRow++;
                }
                ws.DeleteRow(importRow.Start.Row);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }
        }
        else if (dto.Tab == 4)
        {
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_TAX_IJTIMOIY_SOLIQ));

            if (IsValid && dataType_1 != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excelPackage = new ExcelPackage(template);

                var ws = excelPackage.Workbook.Worksheets[0];

                var importRow = excelPackage.Workbook.Names["ImportRow"];
                int currentRow = importRow.Start.Row;
                int index = 1;

                var importTotalRow = excelPackage.Workbook.Names["ImportTotalRow"];
                int currentTotalRow = importTotalRow.Start.Row;
                var columnTotal = 3;
                ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.SocialTaxCount) + dataType_2.Sum(a => a.SocialTaxCount) + dataType_3.Sum(a => a.SocialTaxCount)).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.SocialNewVacanciesCount) + dataType_2.Sum(a => a.SocialNewVacanciesCount) + dataType_3.Sum(a => a.SocialNewVacanciesCount)).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = 0.ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.SocialTaxSum) + dataType_2.Sum(a => a.SocialTaxSum) + dataType_3.Sum(a => a.SocialTaxSum)).ToString();

                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.SocialTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.SocialNewVacanciesCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = 0.ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.SocialTaxSum).ToString();

                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.SocialTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.SocialNewVacanciesCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = 0.ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.SocialTaxSum).ToString();

                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.SocialTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.SocialNewVacanciesCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = 0.ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.SocialTaxSum).ToString();

                for (int i = 0; i < dataType_1.Count; i++)
                {
                    var column = 1;
                    ws.InsertRow(currentRow, 1, importRow.Start.Row);
                    ws.Cells[currentRow, column++].Value = index++;
                    if (dto.ByDistrict == true)
                        ws.Cells[currentRow, column++].Value = dataType_1[i].District;
                    else if (dto.ByContractor == true)
                        ws.Cells[currentRow, column++].Value = dataType_1[i].ContractorInn + " - " + dataType_1[i].ContractorFullName;
                    else
                        ws.Cells[currentRow, column++].Value = dataType_1[i].Region;

                    ws.Cells[currentRow, column++].Value = (dataType_1[i].SocialTaxCount + dataType_2[i].SocialTaxCount + dataType_3[i].SocialTaxCount).ToString();
                    ws.Cells[currentRow, column++].Value = (dataType_1[i].SocialNewVacanciesCount + dataType_2[i].SocialNewVacanciesCount + dataType_3[i].SocialNewVacanciesCount).ToString();
                    ws.Cells[currentRow, column++].Value = 0;
                    ws.Cells[currentRow, column++].Value = (dataType_1[i].SocialTaxSum + dataType_2[i].SocialTaxSum + dataType_3[i].SocialTaxSum).ToString();

                    ws.Cells[currentRow, column++].Value = dataType_1[i].SocialTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_1[i].SocialNewVacanciesCount;
                    ws.Cells[currentRow, column++].Value = 0;
                    ws.Cells[currentRow, column++].Value = dataType_1[i].SocialTaxSum;

                    ws.Cells[currentRow, column++].Value = dataType_2[i].SocialTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_2[i].SocialNewVacanciesCount;
                    ws.Cells[currentRow, column++].Value = 0;
                    ws.Cells[currentRow, column++].Value = dataType_2[i].SocialTaxSum;

                    ws.Cells[currentRow, column++].Value = dataType_3[i].SocialTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_3[i].SocialNewVacanciesCount;
                    ws.Cells[currentRow, column++].Value = 0;
                    ws.Cells[currentRow, column++].Value = dataType_3[i].SocialTaxSum;
                    currentRow++;
                }
                ws.DeleteRow(importRow.Start.Row);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }
        }
        else
        {
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_TAX_IMTIYOZ));

            if (IsValid && dataType_1 != null && dataType_2 != null && dataType_3 != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excelPackage = new ExcelPackage(template);

                var ws = excelPackage.Workbook.Worksheets[0];

                var importRow = excelPackage.Workbook.Names["ImportRow"];
                int currentRow = importRow.Start.Row;
                int index = 1;

                var importTotalRow = excelPackage.Workbook.Names["ImpotTotalRow"];
                int currentTotalRow = importTotalRow.Start.Row;
                var columnTotal = 3;

                if (curContractType == null)
                {
                    ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.CertificateCount) + dataType_2.Sum(a => a.CertificateCount) + dataType_3.Sum(a => a.CertificateCount)).ToString();
                    ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.NewVacanciesCount) + dataType_2.Sum(a => a.NewVacanciesCount) + dataType_3.Sum(a => a.NewVacanciesCount)).ToString();
                    ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.TotalContractApplicationCount) + dataType_2.Sum(a => a.TotalContractApplicationCount) + dataType_3.Sum(a => a.TotalContractApplicationCount)).ToString();
                }

                else if (curContractType == 1)
                {
                    ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.CertificateCount)).ToString();
                    ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.NewVacanciesCount)).ToString();
                    ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.TotalContractApplicationCount)).ToString();
                }

                else if (curContractType == 2)
                {
                    ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_2.Sum(a => a.CertificateCount)).ToString();
                    ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_2.Sum(a => a.NewVacanciesCount)).ToString();
                    ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_2.Sum(a => a.TotalContractApplicationCount)).ToString();
                }

                else if (curContractType == 3)
                {
                    ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_3.Sum(a => a.CertificateCount)).ToString();
                    ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_3.Sum(a => a.NewVacanciesCount)).ToString();
                    ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_3.Sum(a => a.TotalContractApplicationCount)).ToString();
                }


                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.IncomeTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.IncomeTaxSum).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.ContractorSocialTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.SocialTaxSum).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.LandTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.LandTaxSum).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.PropertyTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.PropertyTaxSum).ToString();

                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.IncomeTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.IncomeTaxSum).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.SocialTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.SocialTaxSum).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.LandTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.LandTaxSum).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.PropertyTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.PropertyTaxSum).ToString();

                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.IncomeTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.IncomeTaxSum).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.SocialTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.SocialTaxSum).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.LandTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.LandTaxSum).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.PropertyTaxCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.PropertyTaxSum).ToString();

                for (int i = 0; i < dataType_1.Count; i++)
                {
                    var column = 1;
                    ws.InsertRow(currentRow, 1, importRow.Start.Row);
                    ws.Cells[currentRow, column++].Value = index++;
                    if (dto.ByDistrict == true)
                        ws.Cells[currentRow, column++].Value = dataType_1[i].District;
                    else if (dto.ByContractor == true)
                        ws.Cells[currentRow, column++].Value = dataType_1[i].ContractorInn + " - " + dataType_1[i].ContractorFullName;
                    else
                        ws.Cells[currentRow, column++].Value = dataType_1[i].Region;

                    if (curContractType == null)
                    {
                        ws.Cells[currentRow, column++].Value = (dataType_1[i].CertificateCount + dataType_2[i].CertificateCount + dataType_3[i].CertificateCount).ToString();
                        ws.Cells[currentRow, column++].Value = (dataType_1[i].NewVacanciesCount + dataType_2[i].NewVacanciesCount + dataType_3[i].NewVacanciesCount).ToString();
                        ws.Cells[currentRow, column++].Value = (dataType_1[i].TotalContractApplicationCount + dataType_2[i].TotalContractApplicationCount + dataType_3[i].TotalContractApplicationCount).ToString();
                    }
                    else if (curContractType == 1)
                    {
                        ws.Cells[currentRow, column++].Value = (dataType_1[i].CertificateCount).ToString();
                        ws.Cells[currentRow, column++].Value = (dataType_1[i].NewVacanciesCount).ToString();
                        ws.Cells[currentRow, column++].Value = (dataType_1[i].TotalContractApplicationCount).ToString();
                    }
                    else if (curContractType == 2)
                    {
                        ws.Cells[currentRow, column++].Value = (dataType_2[i].CertificateCount).ToString();
                        ws.Cells[currentRow, column++].Value = (dataType_2[i].NewVacanciesCount).ToString();
                        ws.Cells[currentRow, column++].Value = (dataType_2[i].TotalContractApplicationCount).ToString();
                    }
                    else if (curContractType == 3)
                    {
                        ws.Cells[currentRow, column++].Value = (dataType_3[i].CertificateCount).ToString();
                        ws.Cells[currentRow, column++].Value = (dataType_3[i].NewVacanciesCount).ToString();
                        ws.Cells[currentRow, column++].Value = (dataType_3[i].TotalContractApplicationCount).ToString();
                    }
                    Console.WriteLine(dataType_1[i].CertificateCount);
                    Console.WriteLine();
                    Console.WriteLine(dataType_1[i].NewVacanciesCount);
                    Console.WriteLine();
                    Console.WriteLine(dataType_1[i].TotalContractApplicationCount);
                    Console.WriteLine();

                    ws.Cells[currentRow, column++].Value = dataType_1[i].IncomeTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_1[i].IncomeTaxSum;
                    ws.Cells[currentRow, column++].Value = dataType_1[i].ContractorSocialTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_1[i].SocialTaxSum;
                    ws.Cells[currentRow, column++].Value = dataType_1[i].LandTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_1[i].LandTaxSum;
                    ws.Cells[currentRow, column++].Value = dataType_1[i].PropertyTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_1[i].PropertyTaxSum;

                    ws.Cells[currentRow, column++].Value = dataType_2[i].IncomeTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_2[i].IncomeTaxSum;
                    ws.Cells[currentRow, column++].Value = dataType_2[i].ContractorSocialTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_2[i].SocialTaxSum;
                    ws.Cells[currentRow, column++].Value = dataType_2[i].LandTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_2[i].LandTaxSum;
                    ws.Cells[currentRow, column++].Value = dataType_2[i].PropertyTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_2[i].PropertyTaxSum;

                    ws.Cells[currentRow, column++].Value = dataType_3[i].IncomeTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_3[i].IncomeTaxSum;
                    ws.Cells[currentRow, column++].Value = dataType_3[i].ContractorSocialTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_3[i].SocialTaxSum;
                    ws.Cells[currentRow, column++].Value = dataType_3[i].LandTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_3[i].LandTaxSum;
                    ws.Cells[currentRow, column++].Value = dataType_3[i].PropertyTaxCount;
                    ws.Cells[currentRow, column++].Value = dataType_3[i].PropertyTaxSum;
                    currentRow++;
                }
                ws.DeleteRow(importRow.Start.Row);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }
            result.Position = 0;
            return result;
        }

        result.Position = 0;
        return result;
    }
    public Stream SaveAsExecelBojxonaImtiyozReportByContractor(BojxonaImtiyozReportByContractorDtoFilter dto)
    {
        var data = GetBojxonaImtiyozReportByContractor(new BojxonaImtiyozReportByContractorDtoFilter
        {
            ByContractor = true,
            ByDistrict = true,
            ByRegion = true,
            RegionId = dto.RegionId,
            DistrictId = dto.DistrictId,
            HasCertificate = dto.HasCertificate
        }).ToList();

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.BOJXONA_IMTIYOZ_REPORT_BY_CONTRACTOR));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;



            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value = item.Region;
                ws.Cells[currentRow, column++].Value = item.District;
                ws.Cells[currentRow, column++].Value = item.ContractorFullName;
                ws.Cells[currentRow, column++].Value = item.ContractorInn;
                ws.Cells[currentRow, column++].Value = item.AppCount;
                ws.Cells[currentRow, column++].Value = item.DevCount;
                ws.Cells[currentRow, column++].Value = item.RejCount;
                ws.Cells[currentRow, column++].Value = item.GrChanCount;
                ws.Cells[currentRow, column++].Value = item.Sum;
                currentRow++;
            }

            var columnTotal = 6;
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.AppCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.DevCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.RejCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.GrChanCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.Sum);
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelBankCreditReport(ContractorBankCreditReportDtoFilter dto)
    {
        var data = GetBankCreditReport(new ContractorBankCreditReportDtoFilter
        {
            ByBank = true,
            ByDistrict = true,
            ByRegion = true,
            RegionId = dto.RegionId,
            DistrictId = dto.DistrictId,
        }).Result.ToList();

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_BANK_CREDIT));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;



            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            foreach (var item in data)
            {
                //var totalSum = item.TotalPrtnApplicationSentCount + item.TotalPrtnApplicationSentForReviewCount + item.TotalPrtnApplicationSentRejectedCount + item.TotalPrtnContractCount;
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value = item.RegionName;
                ws.Cells[currentRow, column++].Value = item.DistrictName;
                ws.Cells[currentRow, column++].Value = item.BankName;
                ws.Cells[currentRow, column++].Value = item.Application?.IssuanceCount;
                ws.Cells[currentRow, column++].Value = item.Application?.IssuanceSum;
                ws.Cells[currentRow, column++].Value = item.Application?.CanceledCount;
                ws.Cells[currentRow, column++].Value = item.Application?.CanceledSum;
                ws.Cells[currentRow, column++].Value = item.Application?.RejectedCount;
                ws.Cells[currentRow, column++].Value = item.Application?.RejectedSum;
                ws.Cells[currentRow, column++].Value = item.Application?.SubmittedCount;
                ws.Cells[currentRow, column++].Value = item.Application?.SubmittedSum;
                ws.Cells[currentRow, column++].Value = item.Application?.ApprovedCount;
                ws.Cells[currentRow, column++].Value = item.Application?.ApprovedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.IssuanceCount;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.IssuanceSum;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.CanceledCount;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.CanceledSum;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.RejectedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.RejectedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.SubmittedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.SubmittedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.ApprovedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.ApprovedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.IssuanceCount;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.IssuanceSum;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.CanceledCount;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.CanceledSum;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.RejectedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.RejectedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.SubmittedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.SubmittedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.ApprovedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.ApprovedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.IssuanceCount;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.IssuanceSum;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.CanceledCount;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.CanceledSum;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.RejectedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.RejectedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.SubmittedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.SubmittedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.ApprovedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.ApprovedSum;
                currentRow++;
            }

            var columnTotal = 6;
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.IssuanceCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.IssuanceSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.CanceledCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.CanceledSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.RejectedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.RejectedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.SubmittedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.SubmittedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.ApprovedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.ApprovedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.IssuanceCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.IssuanceSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.CanceledCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.CanceledSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.RejectedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.RejectedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.SubmittedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.SubmittedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.ApprovedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.ApprovedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.IssuanceCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.IssuanceSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.CanceledCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.CanceledSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.RejectedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.RejectedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.SubmittedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.SubmittedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.ApprovedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.ApprovedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.IssuanceCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.IssuanceSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.CanceledCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.CanceledSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.RejectedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.RejectedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.SubmittedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.SubmittedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.ApprovedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.ApprovedSum);
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }

    public Stream SaveAsExcelTaxQqsAylanmaReport(GetTaxQqsAylanmaDtoFilter option)
    {
        var data = GetTaxQqsAylanmaReport(new GetTaxQqsAylanmaDtoFilter
        {
            Year = option.Year,
            Month = option.Month,
            RegionId = option.RegionId,
            DistrictId = option.DistrictId,
            ByRegion = option.ByRegion,
            ByDistrict = option.ByDistrict,
            ByContractor = option.ByContractor
        });

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate
            .GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_TAX_QQS_AYLANMA));
        try
        {
            if (IsValid && data != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excelPackage = new ExcelPackage(template);
                //var year1 = excelPackage.Workbook.Names["Year1"];
                //year1.Value = option.Year;
                //var year2 = excelPackage.Workbook.Names["Year2"];
                //year2.Value = option.Year;

                var importRow = excelPackage.Workbook.Names["ImportRow"];
                var ws = importRow.Worksheet;
                int currentRow = importRow.Start.Row + 1;

                int index = 1;
                var list = data.Rows.ToList();

                foreach (var item in list)
                {
                    var column = 1;
                    ws.InsertRow(currentRow, 1, importRow.Start.Row);
                    ws.Cells[currentRow, column++].Value = index++;
                    if (option.ByRegion)
                        ws.Cells[currentRow, column++].Value = item.Region != null ? item.Region : "";
                    else if (option.ByDistrict)
                        ws.Cells[currentRow, column++].Value = item.District != null ? item.District : "";
                    else
                        ws.Cells[currentRow, column++].Value = item.ContractorFullName != null ? item.ContractorFullName : "";

                    ws.Cells[currentRow, column++].Value = item.ContractorCount;
                    ws.Cells[currentRow, column++].Value = item.NetIncomeWithOutVat1;
                    ws.Cells[currentRow, column++].Value = item.VatSum1;
                    ws.Cells[currentRow, column++].Value = item.NetIncomeWithOutVat2;
                    ws.Cells[currentRow, column++].Value = item.VatSum2;
                    ws.Cells[currentRow, column++].Value = item.NetIncomeWithOutVat3;
                    ws.Cells[currentRow, column++].Value = item.VatSum3;
                    ws.Cells[currentRow, column++].Value = item.PNetIncomeWithoutvat1;
                    ws.Cells[currentRow, column++].Value = item.PVatSum1;



                    ws.Cells[currentRow, column++].Value = item.NetIncomeWithOutVat4;
                    ws.Cells[currentRow, column++].Value = item.VatSum4;
                    ws.Cells[currentRow, column++].Value = item.NetIncomeWithOutVat5;
                    ws.Cells[currentRow, column++].Value = item.VatSum5;
                    ws.Cells[currentRow, column++].Value = item.NetIncomeWithOutVat6;
                    ws.Cells[currentRow, column++].Value = item.VatSum6;
                    ws.Cells[currentRow, column++].Value = item.PNetIncomeWithoutvat2;
                    ws.Cells[currentRow, column++].Value = item.PVatSum2;


                    ws.Cells[currentRow, column++].Value = item.NetIncomeWithOutVat7;
                    ws.Cells[currentRow, column++].Value = item.VatSum7;
                    ws.Cells[currentRow, column++].Value = item.NetIncomeWithOutVat8;
                    ws.Cells[currentRow, column++].Value = item.VatSum8;
                    ws.Cells[currentRow, column++].Value = item.NetIncomeWithOutVat9;
                    ws.Cells[currentRow, column++].Value = item.VatSum9;
                    ws.Cells[currentRow, column++].Value = item.PNetIncomeWithoutvat3;
                    ws.Cells[currentRow, column++].Value = item.PVatSum3;



                    ws.Cells[currentRow, column++].Value = item.NetIncomeWithOutVat10;
                    ws.Cells[currentRow, column++].Value = item.VatSum10;
                    ws.Cells[currentRow, column++].Value = item.NetIncomeWithOutVat11;
                    ws.Cells[currentRow, column++].Value = item.VatSum11;
                    ws.Cells[currentRow, column++].Value = item.NetIncomeWithOutVat12;
                    ws.Cells[currentRow, column++].Value = item.VatSum12;
                    ws.Cells[currentRow, column++].Value = item.P4NetIncomeWithoutvat;
                    ws.Cells[currentRow, column++].Value = item.P4VatSum;


                    ws.Cells[currentRow, column++].Value = item.TotalNetIncomeWithoutvat;
                    ws.Cells[currentRow, column++].Value = item.TotalVatSum;
                    currentRow++;

                }

                var columnTotal = 3;
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.ContractorCount);


                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.NetIncomeWithOutVat1);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.VatSum1);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.NetIncomeWithOutVat2);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.VatSum2);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.NetIncomeWithOutVat3);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.VatSum3);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.PNetIncomeWithoutvat1);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.PVatSum1);



                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.NetIncomeWithOutVat4);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.VatSum4);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.NetIncomeWithOutVat5);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.VatSum5);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.NetIncomeWithOutVat6);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.VatSum6);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.PNetIncomeWithoutvat2);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.PVatSum2);



                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.NetIncomeWithOutVat7);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.VatSum7);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.NetIncomeWithOutVat8);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.VatSum8);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.NetIncomeWithOutVat9);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.VatSum9);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.PNetIncomeWithoutvat3);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.PVatSum3);



                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.NetIncomeWithOutVat10);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.VatSum10);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.NetIncomeWithOutVat11);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.VatSum11);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.NetIncomeWithOutVat12);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.VatSum12);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.P4NetIncomeWithoutvat);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.P4VatSum);

                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.TotalNetIncomeWithoutvat);
                ws.Cells[currentRow, columnTotal++].Value = data.Rows.Sum(a => a.TotalVatSum);

                ws.DeleteRow(importRow.Start.Row);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }

        }
        catch (Exception ex)
        {
            AddError($"{ex.Message}: {ex.InnerException}");
        }

        return result;


    }
    public Stream SaveAsExcelSoliqReportByContractor(GetSoliqReportByContractorDtoFilter options)
    {
        var data = GetSoliqReportByContractor(new GetSoliqReportByContractorDtoFilter
        {
            Year = options.Year,
            RegionId = options.RegionId,
            DistrictId = options.DistrictId,
            HasCertificate = options.HasCertificate,
            ByRegion = options.ByRegion,
            ByDistrict = options.ByDistrict,
            ByContractor = options.ByContractor
        });
        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate
            .GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.SOLIQ_REPORT_BY_CONTRACTOR));

        try
        {
            if (IsValid && data != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excelPackage = new ExcelPackage(template);
                var year1 = excelPackage.Workbook.Names["Year1"];
                year1.Value = options.Year;
                var year2 = excelPackage.Workbook.Names["Year2"];
                year2.Value = options.Year;

                var importRow = excelPackage.Workbook.Names["ImportRow"];
                var ws = importRow.Worksheet;
                int currentRow = importRow.Start.Row + 1;


                ws.Column(7).Width = 35;
                ws.Column(9).Width = 35;
                ws.Column(11).Width = 35;
                ws.Column(13).Width = 35;
                ws.Column(15).Width = 35;
                ws.Column(17).Width = 35;
                ws.Column(19).Width = 35;
                ws.Column(21).Width = 35;
                ws.Column(23).Width = 35;
                ws.Column(25).Width = 35;
                ws.Column(27).Width = 35;
                ws.Column(29).Width = 35;
                ws.Column(31).Width = 35;
                ws.Column(32).Width = 32;
                ws.Column(33).Width = 32;
                ws.Column(34).Width = 32;
                ws.Column(35).Width = 32;
                ws.Column(36).Width = 32;
                ws.Column(37).Width = 35;

                int index = 1;

                foreach (var item in data)
                {
                    var column = 1;
                    ws.InsertRow(currentRow, 1, importRow.Start.Row);
                    ws.Cells[currentRow, column++].Value = index++;
                    if (options.ByRegion)
                        ws.Cells[currentRow, column++].Value = item.Region != null ? item.Region : "";
                    else if (options.ByDistrict)
                        ws.Cells[currentRow, column++].Value = item.District != null ? item.District : "";
                    else
                        ws.Cells[currentRow, column++].Value = item.ContractorName != null ? item.ContractorName : "";
                    ws.Cells[currentRow, column++].Value = item.ContractorCount;
                    ws.Cells[currentRow, column++].Value = item.TotalPaymentTax;
                    ws.Cells[currentRow, column++].Value = item.NumberEmployee1;
                    ws.Cells[currentRow, column++].Value = item.PaymentTax1;
                    ws.Cells[currentRow, column++].Value = item.NumberEmployee2;
                    ws.Cells[currentRow, column++].Value = item.PaymentTax2;
                    ws.Cells[currentRow, column++].Value = item.NumberEmployee3;
                    ws.Cells[currentRow, column++].Value = item.PaymentTax3;
                    ws.Cells[currentRow, column++].Value = item.NumberEmployee4;
                    ws.Cells[currentRow, column++].Value = item.PaymentTax4;
                    ws.Cells[currentRow, column++].Value = item.NumberEmployee5;
                    ws.Cells[currentRow, column++].Value = item.PaymentTax5;
                    ws.Cells[currentRow, column++].Value = item.NumberEmployee6;
                    ws.Cells[currentRow, column++].Value = item.PaymentTax6;
                    ws.Cells[currentRow, column++].Value = item.NumberEmployee7;
                    ws.Cells[currentRow, column++].Value = item.PaymentTax7;
                    ws.Cells[currentRow, column++].Value = item.NumberEmployee8;
                    ws.Cells[currentRow, column++].Value = item.PaymentTax8;
                    ws.Cells[currentRow, column++].Value = item.NumberEmployee9;
                    ws.Cells[currentRow, column++].Value = item.PaymentTax9;
                    ws.Cells[currentRow, column++].Value = item.NumberEmployee10;
                    ws.Cells[currentRow, column++].Value = item.PaymentTax10;
                    ws.Cells[currentRow, column++].Value = item.NumberEmployee11;
                    ws.Cells[currentRow, column++].Value = item.PaymentTax11;
                    ws.Cells[currentRow, column++].Value = item.NumberEmployee12;
                    ws.Cells[currentRow, column++].Value = item.PaymentTax12;
                    //ws.Cells[currentRow, column++].Value = item.TotalNetIncome;
                    ws.Cells[currentRow, column++].Value = item.NetIncome1;
                    ws.Cells[currentRow, column++].Value = item.NetIncome2;
                    ws.Cells[currentRow, column++].Value = item.NetIncome3;
                    ws.Cells[currentRow, column++].Value = item.NetIncome4;
                    ws.Cells[currentRow, column++].Value = item.TotalTaxDebt;
                    currentRow++;
                }
                //  ws.DeleteRow(currentRow);

                var columnTotal = 3;
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.ContractorCount);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalPaymentTax);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.NumberEmployee1);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.PaymentTax1);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.NumberEmployee2);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.PaymentTax2);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.NumberEmployee3);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.PaymentTax3);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.NumberEmployee4);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.PaymentTax4);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.NumberEmployee5);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.PaymentTax5);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.NumberEmployee6);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.PaymentTax6);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.NumberEmployee7);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.PaymentTax7);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.NumberEmployee8);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.PaymentTax8);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.NumberEmployee9);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.PaymentTax9);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.NumberEmployee10);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.PaymentTax10);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.NumberEmployee11);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.PaymentTax11);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.NumberEmployee12);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.PaymentTax12);
                // ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalNetIncome);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.NetIncome1);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.NetIncome2);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.NetIncome3);
                // columnTotalws.Cells[currentRow++,columnTotal ].Value = data.Sum(a => a.NetIncome3);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.NetIncome4);
                ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalTaxDebt);

                //result = new MemorySteam(excelPackage.GetAsByteArray());
                //excelPackage.Dispose(;
                //result.Position = 0;

                ws.DeleteRow(importRow.Start.Row);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }
            result.Position = 0;
            return result;
        }
        catch (Exception ex)
        {
            AddError($"{ex.Message}: {ex.InnerException}");
        }
        return result;
    }
    public async Task<Stream> SaveAsExcelBankCreditReportAsync(ContractorBankCreditReportDtoFilter dto)
    {
        var data = await GetBankCreditReport(dto).ConfigureAwait(false);
        var result = new MemoryStream();
        var template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_BANK_TEZKOR_HISOBOT));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var excelPackage = new ExcelPackage(template);
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
                if (dto.ByContractor == true)
                    ws.Cells[currentRow, column++].Value = $"{item.ContractorInn} {item.Contractor}";
                else
                    ws.Cells[currentRow, column++].Value = dto.ByBank ? item.BankName : item.RegionName;
                ws.Cells[currentRow, column++].Value = item.Application?.SubmittedCount;
                ws.Cells[currentRow, column++].Value = item.Application?.SubmittedSum;
                ws.Cells[currentRow, column++].Value = item.Application?.CanceledCount;
                ws.Cells[currentRow, column++].Value = item.Application?.CanceledSum;
                ws.Cells[currentRow, column++].Value = item.Application?.RejectedCount;
                ws.Cells[currentRow, column++].Value = item.Application?.RejectedSum;
                ws.Cells[currentRow, column++].Value = item.Application?.ApprovedCount;
                ws.Cells[currentRow, column++].Value = item.Application?.ApprovedSum;
                ws.Cells[currentRow, column++].Value = item.Application?.IssuanceCount;
                ws.Cells[currentRow, column++].Value = item.Application?.IssuanceSum;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.SubmittedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.SubmittedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.CanceledCount;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.CanceledSum;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.RejectedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.RejectedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.ApprovedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.ApprovedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.IssuanceCount;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.IssuanceSum;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.SubmittedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.SubmittedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.CanceledCount;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.CanceledSum;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.RejectedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.RejectedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.ApprovedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.ApprovedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.IssuanceCount;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.IssuanceSum;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.SubmittedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.SubmittedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.CanceledCount;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.CanceledSum;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.RejectedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.RejectedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.ApprovedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.ApprovedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.IssuanceCount;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.IssuanceSum;
                currentRow++;
            }

            var columnTotal = 3;
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.SubmittedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.SubmittedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.CanceledCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.CanceledSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.RejectedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.RejectedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.ApprovedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.ApprovedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.IssuanceCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.IssuanceSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.SubmittedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.SubmittedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.CanceledCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.CanceledSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.RejectedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.RejectedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.ApprovedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.ApprovedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.IssuanceCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.IssuanceSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.SubmittedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.SubmittedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.CanceledCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.CanceledSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.RejectedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.RejectedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.ApprovedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.ApprovedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.IssuanceCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.IssuanceSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.SubmittedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.SubmittedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.CanceledCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.CanceledSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.RejectedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.RejectedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.ApprovedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.ApprovedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.IssuanceCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.IssuanceSum);
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public async Task<Stream> SaveAsExcelSecondBankCreditReportAsync(ContractorBankCreditReportDtoFilter dto)
    {
        var data = await GetBankCreditReport(dto).ConfigureAwait(false);
        var result = new MemoryStream();
        var template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_TABDIRKOR_DASTUR_HISOBOT));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var excelPackage = new ExcelPackage(template);
            var namerange = excelPackage.Workbook.Names["Organization"];
            namerange.Value = "";
            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;
            if (dto.ByDistrict)
            {
                ws.Cells[8, 2].Value = "Tuman";
            }
            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                if (dto.ByDistrict == true)
                    ws.Cells[currentRow, column++].Value = item.DistrictName;
                else if (dto.ByContractor == true)
                    ws.Cells[currentRow, column++].Value = item.ContractorInn + " - " + item.Contractor;
                else
                    ws.Cells[currentRow, column++].Value = item.RegionName;
                ws.Cells[currentRow, column++].Value = item.Application?.ApprovedCount;
                ws.Cells[currentRow, column++].Value = item.Application?.ApprovedSum;
                ws.Cells[currentRow, column++].Value = item.Application?.CanceledCount + item.Application?.RejectedCount;
                ws.Cells[currentRow, column++].Value = 0;
                ws.Cells[currentRow, column++].Value = item.Application?.IssuanceCount;
                ws.Cells[currentRow, column++].Value = item.Application?.IssuanceSum;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.ApprovedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.ApprovedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.CanceledCount + item.ContractType1?.RejectedCount;
                ws.Cells[currentRow, column++].Value = 0;
                ws.Cells[currentRow, column++].Value = item.ContractType1?.IssuanceCount;
                ws.Cells[currentRow, column++].Value = item.ContractType1.IssuanceSum;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.ApprovedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.ApprovedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.CanceledCount + item.ContractType2?.RejectedCount;
                ws.Cells[currentRow, column++].Value = 0;
                ws.Cells[currentRow, column++].Value = item.ContractType2?.IssuanceCount;
                ws.Cells[currentRow, column++].Value = item.ContractType2.IssuanceSum;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.ApprovedCount;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.ApprovedSum;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.CanceledCount + item.ContractType3?.RejectedCount;
                ws.Cells[currentRow, column++].Value = 0;
                ws.Cells[currentRow, column++].Value = item.ContractType3?.IssuanceCount;
                ws.Cells[currentRow, column++].Value = item.ContractType3.IssuanceSum;
                currentRow++;
            }

            var columnTotal = 3;
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.ApprovedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.ApprovedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.CanceledCount + item.Application?.RejectedCount);
            ws.Cells[currentRow, columnTotal++].Value = 0;
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.IssuanceCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.Application?.IssuanceSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.ApprovedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.ApprovedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.CanceledCount + item.ContractType1?.RejectedCount);
            ws.Cells[currentRow, columnTotal++].Value = 0;
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.IssuanceCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType1?.IssuanceSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.ApprovedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.ApprovedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.CanceledCount + item.ContractType2?.RejectedCount);
            ws.Cells[currentRow, columnTotal++].Value = 0;
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.IssuanceCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType2?.IssuanceSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.ApprovedCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.ApprovedSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.CanceledCount + item.ContractType3?.RejectedCount);
            ws.Cells[currentRow, columnTotal++].Value = 0;
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.IssuanceCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.ContractType3?.IssuanceSum);
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelContractorFundByBank(BusinessActivityTypeReprotFilter dto)
    {
        var data = GetBusinessActivityTypeReport(dto);

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_CONTRACTOR_FUND_BYBANK));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow1"];
            var ws = namerange.Worksheet;



            var importRow = excelPackage.Workbook.Names["ImportRow1"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            foreach (var item in data)
            {
                if (item != null)
                {
                    var column = 1;
                    ws.InsertRow(currentRow, 1, importRow.Start.Row);
                    ws.Cells[currentRow, column++].Value = index++;
                    ws.Cells[currentRow, column++].Value = item.BankName != null ? item.BankName : " ";
                    ws.Cells[currentRow, column++].Value = item.BusinessActivity.UserPrivilegeCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessActivity.CreatedVacanciesCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessActivity.FinancialHelpAmount != null ? item.BusinessActivity.FinancialHelpAmount : 0;
                    ws.Cells[currentRow, column++].Value = item.BusinessActivity.UserPrivilegeCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessActivity.FinancialHelpAmount != null ? item.BusinessActivity.FinancialHelpAmount : 0;

                    ws.Cells[currentRow, column++].Value = item.BusinessType1.UserPrivilegeCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessType1.CreatedVacanciesCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessType1.FinancialHelpAmount != null ? item.BusinessActivity.FinancialHelpAmount : 0;
                    ws.Cells[currentRow, column++].Value = item.BusinessType1.UserPrivilegeCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessType1.FinancialHelpAmount != null ? item.BusinessActivity.FinancialHelpAmount : 0;

                    ws.Cells[currentRow, column++].Value = item.BusinessType2.UserPrivilegeCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessType2.CreatedVacanciesCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessType2.FinancialHelpAmount != null ? item.BusinessActivity.FinancialHelpAmount : 0;
                    ws.Cells[currentRow, column++].Value = item.BusinessType2.UserPrivilegeCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessType2.FinancialHelpAmount != null ? item.BusinessActivity.FinancialHelpAmount : 0;

                    ws.Cells[currentRow, column++].Value = item.BusinessType3.UserPrivilegeCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessType3.CreatedVacanciesCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessType3.FinancialHelpAmount != null ? item.BusinessActivity.FinancialHelpAmount : 0;
                    ws.Cells[currentRow, column++].Value = item.BusinessType3.UserPrivilegeCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessType3.FinancialHelpAmount != null ? item.BusinessActivity.FinancialHelpAmount : 0;

                    currentRow++;
                }
            }

            var columnTotal = 3;
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessActivity.UserPrivilegeCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessActivity.CreatedVacanciesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessActivity.FinancialHelpAmount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessActivity.UserPrivilegeCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessActivity.FinancialHelpAmount);

            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType1.UserPrivilegeCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType1.CreatedVacanciesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType1.FinancialHelpAmount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType1.UserPrivilegeCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType1.FinancialHelpAmount);

            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType2.UserPrivilegeCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType2.CreatedVacanciesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType2.FinancialHelpAmount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType2.UserPrivilegeCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType2.FinancialHelpAmount);

            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType3.UserPrivilegeCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType3.CreatedVacanciesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType3.FinancialHelpAmount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType3.UserPrivilegeCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType3.FinancialHelpAmount != null ? item.BusinessActivity.FinancialHelpAmount : 0);

            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelContractorFundByRegion(BusinessActivityTypeReportByRegionFilter dto)
    {
        var data = GetBusinessActivityTypeReportByRegion(dto);

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_CONTRACTOR_FUND_BYREGION));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            foreach (var item in data)
            {
                if (item != null)
                {
                    var column = 1;
                    ws.InsertRow(currentRow, 1, importRow.Start.Row);
                    ws.Cells[currentRow, column++].Value = index++;
                    if (dto.ByRegion == true)
                        ws.Cells[currentRow, column++].Value = item.RegionName;
                    else if (dto.ByDistrict == true)
                        ws.Cells[currentRow, column++].Value = item.DistrictName;
                    ws.Cells[currentRow, column++].Value = item.BusinessActivity.UserPrivilegeCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessActivity.CreatedVacanciesCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessActivity.FinancialHelpAmount != null ? item.BusinessActivity.FinancialHelpAmount : 0;
                    ws.Cells[currentRow, column++].Value = item.BusinessActivity.UserPrivilegeCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessActivity.FinancialHelpAmount != null ? item.BusinessActivity.FinancialHelpAmount : 0;

                    ws.Cells[currentRow, column++].Value = item.BusinessType1.UserPrivilegeCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessType1.CreatedVacanciesCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessType1.FinancialHelpAmount != null ? item.BusinessActivity.FinancialHelpAmount : 0;
                    ws.Cells[currentRow, column++].Value = item.BusinessType1.UserPrivilegeCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessType1.FinancialHelpAmount != null ? item.BusinessActivity.FinancialHelpAmount : 0;

                    ws.Cells[currentRow, column++].Value = item.BusinessType2.UserPrivilegeCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessType2.CreatedVacanciesCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessType2.FinancialHelpAmount != null ? item.BusinessActivity.FinancialHelpAmount : 0;
                    ws.Cells[currentRow, column++].Value = item.BusinessType2.UserPrivilegeCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessType2.FinancialHelpAmount != null ? item.BusinessActivity.FinancialHelpAmount : 0;

                    ws.Cells[currentRow, column++].Value = item.BusinessType3.UserPrivilegeCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessType3.CreatedVacanciesCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessType3.FinancialHelpAmount != null ? item.BusinessActivity.FinancialHelpAmount : 0;
                    ws.Cells[currentRow, column++].Value = item.BusinessType3.UserPrivilegeCount;
                    ws.Cells[currentRow, column++].Value = item.BusinessType3.FinancialHelpAmount != null ? item.BusinessActivity.FinancialHelpAmount : 0;

                    currentRow++;
                }
            }

            var columnTotal = 3;
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessActivity.UserPrivilegeCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessActivity.CreatedVacanciesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessActivity.FinancialHelpAmount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessActivity.UserPrivilegeCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessActivity.FinancialHelpAmount);

            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType1.UserPrivilegeCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType1.CreatedVacanciesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType1.FinancialHelpAmount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType1.UserPrivilegeCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType1.FinancialHelpAmount);

            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType2.UserPrivilegeCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType2.CreatedVacanciesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType2.FinancialHelpAmount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType2.UserPrivilegeCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType2.FinancialHelpAmount);

            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType3.UserPrivilegeCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType3.CreatedVacanciesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType3.FinancialHelpAmount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType3.UserPrivilegeCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.BusinessType3.FinancialHelpAmount != null ? item.BusinessActivity.FinancialHelpAmount : 0);

            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelBankCreditePrivilegeReport(BankCreditApplicationReportByRegionAndDistrictDtoFilter dto)
    {
        dto.ByContactType = true;
        dto.ContractTypeId = 1;
        var dataType_1 = GetBankCreditApplicationReportByRegionAndDistrict(dto).ToList();
        dto.ContractTypeId = 2;
        var dataType_2 = GetBankCreditApplicationReportByRegionAndDistrict(dto).ToList();
        dto.ContractTypeId = 3;
        var dataType_3 = GetBankCreditApplicationReportByRegionAndDistrict(dto).ToList();
        dto.ContractTypeId = null;
        dto.ByContactType = false;
        var data = GetBankCreditApplicationReportByRegionAndDistrict(dto).ToList();
        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_BANK_CREDITE_PRIVILEGE));

        if (IsValid && dataType_1 != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var ws = excelPackage.Workbook.Worksheets[0];

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row;
            int index = 1;

            var importTotalRow = excelPackage.Workbook.Names["ImportTotalRow"];
            int currentTotalRow = importTotalRow.Start.Row;
            var columnTotal = 3;
            ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.ApprovedCount) + dataType_2.Sum(a => a.ApprovedCount) + dataType_3.Sum(a => a.ApprovedCount)).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.ApprovedNewVacanciesCount) + dataType_2.Sum(a => a.ApprovedNewVacanciesCount) + dataType_3.Sum(a => a.ApprovedNewVacanciesCount)).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.ApprovedSum) + dataType_2.Sum(a => a.ApprovedSum) + dataType_3.Sum(a => a.ApprovedSum)).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.IssuanceContractorCount) + dataType_2.Sum(a => a.IssuanceContractorCount) + dataType_3.Sum(a => a.IssuanceContractorCount)).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.IssuanceSum) + dataType_2.Sum(a => a.IssuanceSum) + dataType_3.Sum(a => a.IssuanceSum)).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.ApprovedCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.ApprovedNewVacanciesCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.ApprovedSum).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.IssuanceContractorCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.IssuanceSum).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.ApprovedCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.ApprovedNewVacanciesCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.ApprovedSum).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.IssuanceContractorCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.IssuanceSum).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.ApprovedCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.ApprovedNewVacanciesCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.ApprovedSum).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.IssuanceContractorCount).ToString();
            ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.IssuanceSum).ToString();

            for (int i = 0; i < dataType_1.Count; i++)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                if (dto.ByDistrict == true)
                    ws.Cells[currentRow, column++].Value = dataType_1[i].District;
                else if (dto.ByContractor == true)
                    ws.Cells[currentRow, column++].Value = dataType_1[i].ContractorInn + " - " + dataType_1[i].ContractorFullName;
                else
                    ws.Cells[currentRow, column++].Value = dataType_1[i].Region;

                ws.Cells[currentRow, column++].Value = dataType_1[i].ApprovedCount;
                ws.Cells[currentRow, column++].Value = dataType_1[i].ApprovedNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataType_1[i].ApprovedSum;
                ws.Cells[currentRow, column++].Value = dataType_1[i].IssuanceContractorCount;
                ws.Cells[currentRow, column++].Value = dataType_1[i].IssuanceSum;

                ws.Cells[currentRow, column++].Value = dataType_2[i].ApprovedCount;
                ws.Cells[currentRow, column++].Value = dataType_2[i].ApprovedNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataType_2[i].ApprovedSum;
                ws.Cells[currentRow, column++].Value = dataType_2[i].IssuanceContractorCount;
                ws.Cells[currentRow, column++].Value = dataType_2[i].IssuanceSum;

                ws.Cells[currentRow, column++].Value = dataType_3[i].ApprovedCount;
                ws.Cells[currentRow, column++].Value = dataType_3[i].ApprovedNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = dataType_3[i].ApprovedSum;
                ws.Cells[currentRow, column++].Value = dataType_3[i].IssuanceContractorCount;
                ws.Cells[currentRow, column++].Value = dataType_3[i].IssuanceSum;
                currentRow++;
            }
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelStateAssetApplications(StateAssetApplicationFilter dto)
    {
        var data = GetStateAssetApplicationReport(dto);

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_DAVAKTIV));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow1"];
            var ws = namerange.Worksheet;



            var importRow = excelPackage.Workbook.Names["ImportRow1"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            foreach (var item in data)
            {
                //var totalSum = item.TotalPrtnApplicationSentCount + item.TotalPrtnApplicationSentForReviewCount + item.TotalPrtnApplicationSentRejectedCount + item.TotalPrtnContractCount;
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                if (dto.ByDistrict == true)
                    ws.Cells[currentRow, column++].Value = item.DistrictName;
                else
                    ws.Cells[currentRow, column++].Value = item.RegionName;
                ws.Cells[currentRow, column++].Value = item.StateAssetType.CertificateCount;
                ws.Cells[currentRow, column++].Value = item.StateAssetType.NewVacanciesCount;
                ws.Cells[currentRow, column++].Value = item.StateAssetType.StateAssetApplicationCount;
                ws.Cells[currentRow, column++].Value = "";
                ws.Cells[currentRow, column++].Value = item.StateAssetType1.CertificateCount;
                ws.Cells[currentRow, column++].Value = item.StateAssetType1.NewVacanciesCount;
                ws.Cells[currentRow, column++].Value = item.StateAssetType1.StateAssetApplicationCount;
                ws.Cells[currentRow, column++].Value = "";
                ws.Cells[currentRow, column++].Value = item.StateAssetType2.CertificateCount;
                ws.Cells[currentRow, column++].Value = item.StateAssetType2.NewVacanciesCount;
                ws.Cells[currentRow, column++].Value = item.StateAssetType2.StateAssetApplicationCount;
                ws.Cells[currentRow, column++].Value = "";
                ws.Cells[currentRow, column++].Value = item.StateAssetType2.CertificateCount;
                ws.Cells[currentRow, column++].Value = item.StateAssetType2.NewVacanciesCount;
                ws.Cells[currentRow, column++].Value = item.StateAssetType2.StateAssetApplicationCount;
                ws.Cells[currentRow, column++].Value = "";
                currentRow++;
            }

            var columnTotal = 3;
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.StateAssetType.CertificateCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.StateAssetType.NewVacanciesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.StateAssetType.StateAssetApplicationCount);
            ws.Cells[currentRow, columnTotal++].Value = "";
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.StateAssetType1.CertificateCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.StateAssetType1.NewVacanciesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.StateAssetType1.StateAssetApplicationCount);
            ws.Cells[currentRow, columnTotal++].Value = "";
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.StateAssetType2.CertificateCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.StateAssetType2.NewVacanciesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.StateAssetType2.StateAssetApplicationCount);
            ws.Cells[currentRow, columnTotal++].Value = "";
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.StateAssetType3.CertificateCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.StateAssetType3.NewVacanciesCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.StateAssetType3.StateAssetApplicationCount);
            ws.Cells[currentRow, columnTotal++].Value = "";
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelCustomsRelief(BojxonaImtiyozReportByContractorDtoFilter option)
    {
        option.ContarctTypeId = 1;
        var dataType_1 = GetBojxonaImtiyozReportByContractor(option).ToList();
        option.ContarctTypeId = 2;
        var dataType_2 = GetBojxonaImtiyozReportByContractor(option).ToList();
        option.ContarctTypeId = 3;
        var dataType_3 = GetBojxonaImtiyozReportByContractor(option).ToList();

        option.ContarctTypeId = null;
        option.ByContactType = false;
        var data = GetBojxonaImtiyozReportByContractor(option).ToList();

        MemoryStream result = new MemoryStream();

        if (option.Tab == 1)
        {
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.BOJXONE_IMTIYOZITAP2));

            if (IsValid && dataType_1 != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excelPackage = new ExcelPackage(template);

                var ws = excelPackage.Workbook.Worksheets[0];

                var importRow = excelPackage.Workbook.Names["ImportRow"];
                int currentRow = importRow.Start.Row;
                int index = 1;

                var importTotalRow = excelPackage.Workbook.Names["ImportTotalRow"];
                int currentTotalRow = importTotalRow.Start.Row;
                var columnTotal = 3;
                ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.TotalContractorCount) + dataType_2.Sum(a => a.TotalContractorCount) + dataType_3.Sum(a => a.TotalContractorCount)).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.TotalCount) + dataType_2.Sum(a => a.TotalCount) + dataType_3.Sum(a => a.TotalCount)).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.GrChanNewVacanciesCount) + dataType_2.Sum(a => a.GrChanNewVacanciesCount) + dataType_3.Sum(a => a.GrChanNewVacanciesCount)).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.TotalContractorCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.TotalCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.GrChanNewVacanciesCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.TotalContractorCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.TotalCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.GrChanNewVacanciesCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.TotalContractorCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.TotalCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.GrChanNewVacanciesCount).ToString();

                for (int i = 0; i < dataType_1.Count(); i++)
                {
                    var column = 1;
                    ws.InsertRow(currentRow, 1, importRow.Start.Row);
                    ws.Cells[currentRow, column++].Value = index++;
                    if (dataType_1[i].DistrictId.HasValue)
                        ws.Cells[currentRow, column++].Value = dataType_1[i].District;
                    else
                        ws.Cells[currentRow, column++].Value = dataType_1[i].Region;
                    ws.Cells[currentRow, column++].Value = (dataType_1[i].TotalContractorCount + dataType_2[i].TotalContractorCount + dataType_3[i].TotalContractorCount).ToString();
                    ws.Cells[currentRow, column++].Value = (dataType_1[i].TotalCount + dataType_2[i].TotalCount + dataType_3[i].TotalCount).ToString();
                    ws.Cells[currentRow, column++].Value = (dataType_1[i].GrChanNewVacanciesCount + dataType_2[i].GrChanNewVacanciesCount + dataType_3[i].GrChanNewVacanciesCount).ToString();
                    ws.Cells[currentRow, column++].Value = dataType_1[i].TotalContractorCount.ToString();
                    ws.Cells[currentRow, column++].Value = dataType_1[i].TotalCount.ToString();
                    ws.Cells[currentRow, column++].Value = dataType_1[i].GrChanNewVacanciesCount.ToString();
                    ws.Cells[currentRow, column++].Value = dataType_2[i].TotalContractorCount.ToString();
                    ws.Cells[currentRow, column++].Value = dataType_2[i].TotalCount.ToString();
                    ws.Cells[currentRow, column++].Value = dataType_2[i].GrChanNewVacanciesCount.ToString();
                    ws.Cells[currentRow, column++].Value = dataType_3[i].TotalContractorCount.ToString();
                    ws.Cells[currentRow, column++].Value = dataType_3[i].TotalCount.ToString();
                    ws.Cells[currentRow, column++].Value = dataType_3[i].GrChanNewVacanciesCount.ToString();
                    currentRow++;
                }
                ws.DeleteRow(importRow.Start.Row);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }
        }
        else if (option.Tab == 2)
        {
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.BOJXONE_IMTIYOZITAP3));

            if (IsValid && dataType_1 != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excelPackage = new ExcelPackage(template);

                var ws = excelPackage.Workbook.Worksheets[0];

                var importRow = excelPackage.Workbook.Names["ImportRow"];
                int currentRow = importRow.Start.Row;
                int index = 1;

                var importTotalRow = excelPackage.Workbook.Names["ImportTotalRow"];
                int currentTotalRow = importTotalRow.Start.Row;
                var columnTotal = 3;
                ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.AppContractorCount) + dataType_2.Sum(a => a.AppContractorCount) + dataType_3.Sum(a => a.AppContractorCount)).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.DevContractorCount) + dataType_2.Sum(a => a.DevContractorCount) + dataType_3.Sum(a => a.DevContractorCount)).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = (dataType_1.Sum(a => a.Sum) + dataType_2.Sum(a => a.Sum) + dataType_3.Sum(a => a.Sum)).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.AppContractorCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.DevContractorCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_1.Sum(a => a.Sum).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.AppContractorCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.DevContractorCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_2.Sum(a => a.Sum).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.AppContractorCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.DevContractorCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = dataType_3.Sum(a => a.Sum).ToString();

                for (int i = 0; i < dataType_1.Count; i++)
                {
                    var column = 1;
                    ws.InsertRow(currentRow, 1, importRow.Start.Row);
                    ws.Cells[currentRow, column++].Value = index++;
                    if (dataType_1[i].DistrictId.HasValue)
                        ws.Cells[currentRow, column++].Value = dataType_1[i].District;
                    else
                        ws.Cells[currentRow, column++].Value = dataType_1[i].Region;

                    ws.Cells[currentRow, column++].Value = (dataType_1[i].AppContractorCount + dataType_2[i].AppContractorCount + dataType_3[i].AppContractorCount).ToString();
                    ws.Cells[currentRow, column++].Value = (dataType_1[i].DevContractorCount + dataType_2[i].DevContractorCount + dataType_3[i].DevContractorCount).ToString();
                    ws.Cells[currentRow, column++].Value = (dataType_1[i].Sum + dataType_2[i].Sum + dataType_3[i].Sum).ToString();
                    ws.Cells[currentRow, column++].Value = dataType_1[i].AppContractorCount.ToString();
                    ws.Cells[currentRow, column++].Value = dataType_1[i].DevContractorCount.ToString();
                    ws.Cells[currentRow, column++].Value = dataType_1[i].Sum.ToString();
                    ws.Cells[currentRow, column++].Value = dataType_2[i].AppContractorCount.ToString();
                    ws.Cells[currentRow, column++].Value = dataType_2[i].DevContractorCount.ToString();
                    ws.Cells[currentRow, column++].Value = dataType_2[i].Sum.ToString();
                    ws.Cells[currentRow, column++].Value = dataType_3[i].AppContractorCount.ToString();
                    ws.Cells[currentRow, column++].Value = dataType_3[i].DevContractorCount.ToString();
                    ws.Cells[currentRow, column++].Value = dataType_3[i].Sum.ToString();
                    currentRow++;
                }
                ws.DeleteRow(importRow.Start.Row);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }
        }
        else
        {
            MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.BOJXONE_IMTIYOZI));

            if (IsValid && data != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excelPackage = new ExcelPackage(template);

                var ws = excelPackage.Workbook.Worksheets[0];

                var importRow = excelPackage.Workbook.Names["ImportRow"];
                int currentRow = importRow.Start.Row;
                int index = 1;

                var importTotalRow = excelPackage.Workbook.Names["ImportTotalRow"];
                int currentTotalRow = importTotalRow.Start.Row;
                var columnTotal = 3;
                //ws.InsertRow(currentTotalRow, 1, importTotalRow.Start.Row);
                //ws.Cells[currentTotalRow, 1].Value = "Jami";
                //ws.Cells[currentTotalRow, 1, currentTotalRow, 2].Merge = true;
                ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.CertificateCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.NewVacanciesCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalContractorCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.TotalCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.AppContractorCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.AppCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.RejContractorCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.RejCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.DevContractorCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.DevCount).ToString();
                ws.Cells[currentTotalRow, columnTotal++].Value = data.Sum(a => a.Sum).ToString();
                //ws.DeleteRow(importTotalRow.Start.Row);
                //currentTotalRow++;

                foreach (var item in data)
                {
                    var column = 1;
                    ws.InsertRow(currentRow, 1, importRow.Start.Row);
                    ws.Cells[currentRow, column++].Value = index++;
                    if (item.DistrictId.HasValue)
                        ws.Cells[currentRow, column++].Value = item.District;
                    else
                        ws.Cells[currentRow, column++].Value = item.Region;

                    ws.Cells[currentRow, column++].Value = item.CertificateCount.ToString();
                    ws.Cells[currentRow, column++].Value = item.NewVacanciesCount.ToString();
                    ws.Cells[currentRow, column++].Value = item.TotalContractorCount.ToString();
                    ws.Cells[currentRow, column++].Value = item.TotalCount.ToString();
                    ws.Cells[currentRow, column++].Value = item.AppContractorCount.ToString();
                    ws.Cells[currentRow, column++].Value = item.AppCount.ToString();
                    ws.Cells[currentRow, column++].Value = item.RejContractorCount.ToString();
                    ws.Cells[currentRow, column++].Value = item.RejCount.ToString();
                    ws.Cells[currentRow, column++].Value = item.DevContractorCount.ToString();
                    ws.Cells[currentRow, column++].Value = item.DevCount.ToString();
                    ws.Cells[currentRow, column++].Value = item.Sum.ToString();
                    currentRow++;
                }
                ws.DeleteRow(importRow.Start.Row);
                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }
        }

        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelGreenCorridor(BojxonaImtiyozReportByContractorDtoFilter dto)
    {
        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate
            .GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.BOJXONA_IMTIYOZ_REPORT_BY_CONTRACTOR));

        var data = GetBojxonaImtiyozReportByContractor(new BojxonaImtiyozReportByContractorDtoFilter
        {
            ByContractor = true,
            ByDistrict = true,
            ByRegion = true,
            RegionId = dto.RegionId,
            DistrictId = dto.DistrictId,
            HasCertificate = dto.HasCertificate
        }).ToList();

        var dataa = data.Where(x => x.ContractTypeId == 1);

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);
            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value = item.Region;
                ws.Cells[currentRow, column++].Value = item.District;
                ws.Cells[currentRow, column++].Value = item.ContractorFullName;
                ws.Cells[currentRow, column++].Value = item.ContractorInn;
                ws.Cells[currentRow, column++].Value = item.AppCount;
                ws.Cells[currentRow, column++].Value = item.DevCount;
                ws.Cells[currentRow, column++].Value = item.RejCount;
                ws.Cells[currentRow, column++].Value = item.GrChanCount;
                ws.Cells[currentRow, column++].Value = item.Sum;
                currentRow++;
            }

            var columnTotal = 6;
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.AppCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.DevCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.RejCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.GrChanCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.Sum);

            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }

        result.Position = 0;
        return result;
    }
    #endregion

    #region CLAIM
    public Stream SaveAsExcelAppealsSentToClaimApplication(ClaimApplicationReportsDtoFilter dto)
    {
        var data = GetAppealsSentToClaimApplication(dto);

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate
            .GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.APPEALS_SENT_TO_CLAIM_APPLICATION));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row;
            int index = 1;

            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value =
                    dto.ByRegion ? item.Region :
                    dto.ByDistrict ? item.District :
                    dto.ByContractor ? item.Contractor
                    : null;
                ws.Cells[currentRow, column++].Value = item.TotalApplicationAmountInArea.ToString();

                ws.Cells[currentRow, column++].Value = item.ApplicationCount.LegalPersonCount.ToString();
                ws.Cells[currentRow, column++].Value = item.ApplicationCount.PhysicalPersonCount.ToString();

                ws.Cells[currentRow, column++].Value = item.MediationPlanCount.LegalPersonCount.ToString();
                ws.Cells[currentRow, column++].Value = item.MediationPlanCount.PhysicalPersonCount.ToString();

                ws.Cells[currentRow, column++].Value = item.MediationCount.LegalPersonCount.ToString();
                ws.Cells[currentRow, column++].Value = item.MediationCount.PhysicalPersonCount.ToString();

                ws.Cells[currentRow, column++].Value = item.ClaimApplicationForCourtCount.LegalPersonCount.ToString();
                ws.Cells[currentRow, column++].Value = item.ClaimApplicationForCourtCount.PhysicalPersonCount.ToString();

                ws.Cells[currentRow, column++].Value = item.RejectCancelApplicationCancel.RejectedCount.ToString();
                ws.Cells[currentRow, column++].Value = item.RejectCancelApplicationCancel.CanceledCount.ToString();
                currentRow++;
            }

            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelReceivedClaimApplication(ClaimApplicationReportsDtoFilter dto)
    {
        var data = GetReceivedClaimApplication(dto).Rows;

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate
            .GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.RECEIVED_CLAIM_APPLICATION));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int index = 1;

            int currentRow = importRow.Start.Row;
            foreach (var item in data)
            {
                int column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value =
                    dto.ByRegion ? item.Region :
                    dto.ByDistrict ? item.District :
                    dto.ByContractor ? item.Contractor
                    : null;
                ws.Cells[currentRow, column++].Value = item.TotalApplicationAmountInArea.ToString();

                ws.Cells[currentRow, column++].Value = item.ClaimThemeCount[1].ToString();
                ws.Cells[currentRow, column++].Value = item.ClaimThemeCount[2].ToString();
                ws.Cells[currentRow, column++].Value = item.ClaimThemeCount[3].ToString();
                ws.Cells[currentRow, column++].Value = item.ClaimThemeCount[4].ToString();
                ws.Cells[currentRow, column++].Value = item.ClaimThemeCount[5].ToString();
                ws.Cells[currentRow, column++].Value = item.ClaimThemeCount[6].ToString();
                ws.Cells[currentRow, column++].Value = item.ClaimThemeCount[7].ToString();
                ws.Cells[currentRow, column++].Value = item.ClaimThemeCount[8].ToString();
                ws.Cells[currentRow, column++].Value = item.ClaimThemeCount[9].ToString();
                currentRow++;
            }

            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelSummaOfClaimApplication(ClaimApplicationReportsDtoFilter dto)
    {
        var data = GetSummaOfClaimApplication(dto);

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate
            .GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.SUMMA_OF_CLAIM_APPLICATION));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int index = 1;

            int currentRow = importRow.Start.Row;
            foreach (var item in data)
            {
                int column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;

                ws.Cells[currentRow, column++].Value =
                    dto.ByRegion ? item.Region :
                    dto.ByDistrict ? item.District :
                    dto.ByContractor ? item.Contractor
                    : null;

                ws.Cells[currentRow, column++].Value = item.TotalSummaInArea.ToString();

                ws.Cells[currentRow, column++].Value = item.TreatedSum.TotalSumma.ToString();
                ws.Cells[currentRow, column++].Value = item.TreatedSum.LegalSumma.ToString();
                ws.Cells[currentRow, column++].Value = item.TreatedSum.YATTSumma.ToString();
                ws.Cells[currentRow, column++].Value = item.TreatedSum.IndividualsSumma.ToString();
                ws.Cells[currentRow, column++].Value = item.TreatedSum.StateOrganizationSumma.ToString();
                ws.Cells[currentRow, column++].Value = item.TreatedSum.ForeignCitizenSumma.ToString();

                ws.Cells[currentRow, column++].Value = item.UnidirectionalSum.TotalSumma.ToString();
                ws.Cells[currentRow, column++].Value = item.UnidirectionalSum.LegalSumma.ToString();
                ws.Cells[currentRow, column++].Value = item.UnidirectionalSum.YATTSumma.ToString();
                ws.Cells[currentRow, column++].Value = item.UnidirectionalSum.IndividualsSumma.ToString();
                ws.Cells[currentRow, column++].Value = item.UnidirectionalSum.StateOrganizationSumma.ToString();
                ws.Cells[currentRow, column++].Value = item.UnidirectionalSum.ForeignCitizenSumma.ToString();

                currentRow++;
            }

            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    #endregion

    #region OTHERS
    public Stream SaveAsExcelGetSmsLog(SmsLogReportFilterDto dto)
    {
        var data = GetSmsLogReportList(dto);

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate
            .GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.SEND_SMS_REPORT));

        if (IsValid && data.Item1 != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row;
            int index = 1;

            foreach (var item in data.Item1)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value = item.CreatedUserId.ToString();
                ws.Cells[currentRow, column++].Value = item.CreatedAt.ToString();
                ws.Cells[currentRow, column++].Value = item.Inn;
                ws.Cells[currentRow, column++].Value = item.PhoneNumer;
                ws.Cells[currentRow, column++].Value = item.Table;
                ws.Cells[currentRow, column++].Value = item.SmsText;
                ws.Cells[currentRow, column++].Value = item.ErrorText;
                ws.Cells[currentRow, column++].Value = item.Table;
                ws.Cells[currentRow, column++].Value = item.FromStatus;
                ws.Cells[currentRow, column++].Value = item.ToStatus;
                currentRow++;
            }

            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    #endregion

    #region CallCeterAppeal
    public Stream SaveAsExcelCallCenterAppealReportByOkedType(CallCenterAppealReportByOkedTypeFilter dto)
    {
        var data = CallCenterAppealReportByOkedType(dto);
        var regions = _unitOfWork.RegionRepository.DbSet.Include(x => x.Translates).OrderBy(a => a.OrderCode).ToArray();
        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate
             .GetFileName(_cultureHelper.CurrentCulture.Code,
             StaticFileConst.Report.SAVE_AS_EXCEL_CALL_CENTER_APPEAL_REPORT_BY_OKED_TYPE));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            var importRowDetails = excelPackage.Workbook.Names["Details"];
            var importRowRegion = excelPackage.Workbook.Names["region"];
            var TotalVal = excelPackage.Workbook.Names["TotalVal"];
            int currentRow = importRow.Start.Row;
            int index = 1;
            int j = importRowRegion.Start.Column;
            int TotalValColumn = TotalVal.Start.Column;
            if (dto.ByOkedType)
                ws.Cells[importRowDetails.Start.Row, importRowDetails.Start.Column].Value = "Murojaat kilgan tadbirkorlarning faoliyat turi";
            else
                ws.Cells[importRowDetails.Start.Row, importRowDetails.Start.Column].Value = "Murojaatlar yunalishi";
            for (int a = 0; a < regions.Length; a++)
            {
                ws.Cells[importRowRegion.Start.Row, j++].Value = regions[a].Translates.AsQueryable()
                                   .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText
                                   ?? regions[a].FullName;
            }
            for (int i = 0; i < data.Count; i++)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);

                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value = data[i]?.fullName;
                ws.Cells[currentRow, column++].Value = data[i].document_count;
                ws.Cells[currentRow, column++].Value = data[i].document_percentage;
                ws.Cells[currentRow, column++].Value = data[i].document_count_region_6;
                ws.Cells[currentRow, column++].Value = data[i].document_count_region_3;
                ws.Cells[currentRow, column++].Value = data[i].document_count_region_4;
                ws.Cells[currentRow, column++].Value = data[i].document_count_region_5;
                ws.Cells[currentRow, column++].Value = data[i].document_count_region_7;
                ws.Cells[currentRow, column++].Value = data[i].document_count_region_8;
                ws.Cells[currentRow, column++].Value = data[i].document_count_region_9;
                ws.Cells[currentRow, column++].Value = data[i].document_count_region_10;
                ws.Cells[currentRow, column++].Value = data[i].document_count_region_11;
                ws.Cells[currentRow, column++].Value = data[i].document_count_region_12;
                ws.Cells[currentRow, column++].Value = data[i].document_count_region_2;
                ws.Cells[currentRow, column++].Value = data[i].document_count_region_13;
                ws.Cells[currentRow, column++].Value = data[i].document_count_region_14;
                ws.Cells[currentRow, column++].Value = data[i].document_count_region_1;

                currentRow++;
            }

            ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data.Sum(x => x.document_count);
            ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = "100%";
            ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data.Sum(x => x.document_count_region_6);
            ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data.Sum(x => x.document_count_region_3);
            ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data.Sum(x => x.document_count_region_4);
            ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data.Sum(x => x.document_count_region_5);
            ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data.Sum(x => x.document_count_region_7);
            ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data.Sum(x => x.document_count_region_8);
            ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data.Sum(x => x.document_count_region_9);
            ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data.Sum(x => x.document_count_region_10);
            ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data.Sum(x => x.document_count_region_11);
            ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data.Sum(x => x.document_count_region_12);
            ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data.Sum(x => x.document_count_region_2);
            ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data.Sum(x => x.document_count_region_13);
            ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data.Sum(x => x.document_count_region_14);
            ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data.Sum(x => x.document_count_region_1);


            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelGetCallCenterReportByWeek(CallCenterReportByWeekFilterDto dto)
    {
        try
        {
            var data = GetCallCenterReportByWeek(dto);

            MemoryStream result = new MemoryStream();
            MemoryStream template = null;

            template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate
                   .GetFileName(_cultureHelper.CurrentCulture.Code,
                   StaticFileConst.Report.SAVE_AS_EXCEL_CALL_CENTER_REPORT_BY_WEEK));

            if (IsValid && data != null)
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excelPackage = new ExcelPackage(template);

                var namerange = excelPackage.Workbook.Names["ImportRow"];
                var ws = namerange.Worksheet;

                var importRow = excelPackage.Workbook.Names["ImportRow"];

                var importRowWeek = excelPackage.Workbook.Names["WeekOrUser"];
                int currentRow = importRow.Start.Row;
                int index = 0;

                if (dto.ByWeek)
                    ws.Cells[importRowWeek.Start.Row, importRowWeek.Start.Column].Value = "Xafta kunlari";
                else
                    ws.Cells[importRowWeek.Start.Row, importRowWeek.Start.Column].Value = "Hodimlar kesimida";

                for (int i = 0; i < data.Count; i++)
                {
                    var column = 1;
                    ws.InsertRow(currentRow, 1, importRow.Start.Row);

                    ws.Cells[currentRow, column++].Value = index++;
                    if (dto.ByWeek == false)
                        ws.Cells[currentRow, column++].Value = data[i]?.UserFullName;
                    else
                        ws.Cells[currentRow, column++].Value = (i == 1) ? "Dushanba" : (i == 2) ? "Seshanba" : (i == 3) ? "Chorshanba" : (i == 4) ? "Payshanba" : (i == 5) ? "Juma" : (i == 6) ? "Shanba" : (i == 0) ? "Jami" : "";
                    ws.Cells[currentRow, column++].Value = data[i].Total;
                    ws.Cells[currentRow, column++].Value = data[i].TotalPercentage.ToString() + "%";
                    ws.Cells[currentRow, column++].Value = data[i].InWorkTime;
                    ws.Cells[currentRow, column++].Value = data[i].InWorkTimePercentage.ToString() + "%";
                    ws.Cells[currentRow, column++].Value = data[i].OutWorkTime;
                    ws.Cells[currentRow, column++].Value = data[i].OutWorkTimePercentage.ToString() + "%";

                    currentRow++;
                }

                result = new MemoryStream(excelPackage.GetAsByteArray());
                excelPackage.Dispose();
            }
            result.Position = 0;
            return result;
        }
        catch (Exception e)
        {
            AddError(e.Message + e.InnerException.Message);
            throw e.InnerException;
        }
    }
    public Stream SaveAsExcelGetCallCenterByRegion(CallCenterDtoFilter dto)
    {
        var data = GetCallCenterByRegion(dto);


        MemoryStream result = new MemoryStream();
        MemoryStream template = null;

        if (dto.ReportType == true)
            template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate
                    .GetFileName(_cultureHelper.CurrentCulture.Code,
                      StaticFileConst.Report.SAVE_AS_EXCEL_GET_CALL_CENTER_BY_REGION));
        else
            template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate
                 .GetFileName(_cultureHelper.CurrentCulture.Code,
                   StaticFileConst.Report.SAVE_AS_EXCEL_GET_CALL_CENTER_BY_PHYSICAL_TYPE));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];

            var TotalVal = excelPackage.Workbook.Names["TotalVal"];
            int currentRow = importRow.Start.Row;
            int index = 1;
            int TotalValColumn = TotalVal.Start.Column;


            for (int i = 0; i < data.Count; i++)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                if (dto.ByRegion == true)
                    ws.Cells[currentRow, column++].Value = data[i].Region;
                else
                    ws.Cells[currentRow, column++].Value = data[i].District;
                ws.Cells[currentRow, column++].Value = data[i]?.TotalCallCenterAppeal;
                ws.Cells[currentRow, column++].Value = data[i]?.TotalCallCenterAppealPercent;
                if (dto.ReportType == true)
                {
                    ws.Cells[currentRow, column++].Value = data[i]?.TotalLegalCount;
                    ws.Cells[currentRow, column++].Value = data[i]?.TotalLegalCountPercent;
                    ws.Cells[currentRow, column++].Value = data[i]?.TotalPhysicalCount;
                    ws.Cells[currentRow, column++].Value = data[i]?.TotalPhysicalCountPercent;
                    ws.Cells[currentRow, column++].Value = data[i]?.TotalInvestorCount;
                    ws.Cells[currentRow, column++].Value = data[i]?.TotalInvestorPercent;
                    ws.Cells[currentRow, column++].Value = data[i]?.TotalPhysicalContractorCount;
                    ws.Cells[currentRow, column++].Value = data[i]?.TotalPhysicalContractorPercent;

                }
                else
                {
                    ws.Cells[currentRow, column++].Value = data[i]?.TotalMikroContractorCount;
                    ws.Cells[currentRow, column++].Value = data[i]?.TotalMikroContractorCountPercent;
                    ws.Cells[currentRow, column++].Value = data[i]?.TotalLitteContractorCount;
                    ws.Cells[currentRow, column++].Value = data[i]?.TotalLittleContractorCountPercent;
                    ws.Cells[currentRow, column++].Value = data[i]?.TotalMiddleContractorCount;
                    ws.Cells[currentRow, column++].Value = data[i]?.TotalMikroContractorCountPercent;
                    ws.Cells[currentRow, column++].Value = data[i]?.TotalHigheContractorCount;
                    ws.Cells[currentRow, column++].Value = data[i]?.TotalHigheContractorCountPercent;
                }

                currentRow++;
            }

            ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data?.Sum(x => x?.TotalCallCenterAppeal);
            ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = "100%";

            if (dto.ReportType == true)
            {
                ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data?.Sum(x => x?.TotalLegalCount);
                ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data?.Sum(x => x?.TotalLegalCountPercent);
                ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data?.Sum(x => x?.TotalPhysicalCount);
                ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data?.Sum(x => x?.TotalPhysicalCountPercent);
                ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data?.Sum(x => x?.TotalInvestorCount);
                ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data?.Sum(x => x?.TotalInvestorPercent);
                ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data?.Sum(x => x?.TotalPhysicalContractorCount);
                ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data?.Sum(x => x?.TotalPhysicalContractorPercent);

            }
            else
            {
                ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data?.Sum(x => x?.TotalMikroContractorCount);
                ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data?.Sum(x => x?.TotalMikroContractorCount);
                ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data?.Sum(x => x?.TotalLitteContractorCount);
                ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data?.Sum(x => x?.TotalLittleContractorCountPercent);
                ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data?.Sum(x => x?.TotalMiddleContractorCount);
                ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data?.Sum(x => x?.TotalMiddleContractorCountPercent);
                ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data?.Sum(x => x?.TotalHigheContractorCount);
                ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = data?.Sum(x => x?.TotalHigheContractorCountPercent);
            }


            //ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = $"{(100 * (data.Sum(x => x.InWorkTime))) / (data.Sum(x => x.Total))} .{(100 * (data.Sum(x => x.InWorkTime))) % (data.Sum(x => x.Total))} %";
            //ws.Cells[TotalVal.Start.Row, TotalValColumn++].Value = $"{(100 * (data.Sum(x => x.OutWorkTime))) / (data.Sum(x => x.Total))} .{(100 * (data.Sum(x => x.OutWorkTime))) % (data.Sum(x => x.Total))} %";

            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelContractorCategoryType(ContractorCategoryDtoFilter options)
    {
        var data = GetContractorCategoryTypeMethod(options);


        MemoryStream result = new MemoryStream();
        MemoryStream template = null;


        template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.SAVE_AS_EXCEL_GET_CONTRACTOR_CATEGORY_TYPE));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);


            var importRow = excelPackage.Workbook.Names["insertrow"];
            var ws = importRow.Worksheet;
            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            var namerange = excelPackage.Workbook.Names["centerrow"];
            var wss = namerange.Worksheet;
            var curentRow = namerange.Start.Row;
            wss.Cells[curentRow, 1].Value = data.FirstOrDefault().DistrictId != null ? data.FirstOrDefault().Region.ToString() : string.Empty;


            foreach (var item in data)
            {

                var column = 1;
                ws.Cells[currentRow, 1, currentRow, 14].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[currentRow, 1, currentRow, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value = item.District == null ? item.Region : item.District;
                ws.Cells[currentRow, column++].Value = item.TotalNewCreatedContractorLegalCount;
                ws.Cells[currentRow, column++].Value = item.TotalNewCreatedContractorPhysicalCount;
                ws.Cells[currentRow, column++].Value = !string.IsNullOrWhiteSpace(item.TotalFreeAddedMemshipLegalCount.ToString()) ? item.TotalFreeAddedMemshipLegalCount.ToString() : "0";
                ws.Cells[currentRow, column++].Value = !string.IsNullOrWhiteSpace(item.TotalFreeAddedMemshipPhysicalCount.ToString()) ? item.TotalFreeAddedMemshipPhysicalCount : "0";
                ws.Cells[currentRow, column++].Value = !string.IsNullOrWhiteSpace(item.Total.ToString()) ? item.Total.ToString() : 0.ToString();
                ws.Cells[currentRow, column++].Value = !string.IsNullOrWhiteSpace(item.TotalRatingFromNormaLegalCount.ToString()) ? item.TotalRatingFromNormaLegalCount.ToString() : "0";
                ws.Cells[currentRow, column++].Value = !string.IsNullOrWhiteSpace(item.TotalRatingFromNormaPhysicalCount.ToString()) ? item.TotalRatingFromNormaPhysicalCount.ToString() : "0";
                ws.Cells[currentRow, column++].Value = !string.IsNullOrWhiteSpace(item.TotalEvaluationRatingLegalCount.ToString()) ? item.TotalEvaluationRatingLegalCount : "0";
                ws.Cells[currentRow, column++].Value = !string.IsNullOrWhiteSpace(item.TotalEvaluationRatingPhysicalCount.ToString()) ? item.TotalEvaluationRatingPhysicalCount : "0";
                ws.Cells[currentRow, column++].Value = !string.IsNullOrWhiteSpace(item.AverageRating.ToString()) ? item.AverageRating : "0";
                ws.Cells[currentRow, column++].Value = !string.IsNullOrWhiteSpace(item.Evaluation) ? item.Evaluation : "-";
                currentRow++;
            }

            //int currentRowTotal = importRowTotal.Start.Row + 1;
            var columnTotal = 2;
            ws.Cells[currentRow, columnTotal++].Value = "Jami";
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalNewCreatedContractorLegalCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalNewCreatedContractorPhysicalCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalFreeAddedMemshipLegalCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalFreeAddedMemshipPhysicalCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.Total);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalRatingFromNormaLegalCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalRatingFromNormaPhysicalCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalEvaluationRatingLegalCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.TotalEvaluationRatingPhysicalCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(a => a.AverageRating);
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    #endregion

    #region CORRUPTION
    public Stream SaveAsExcelCharterMembersRegisterReport(CharterMembersRegisterFilterDto dto)
    {
        var data = GetCharterMembersRegisterReportList(dto);

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate
            .GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.CHARTER_MEMBERS_REGISTER));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row;
            int index = 1;

            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value = item.ContractorFullName;
                ws.Cells[currentRow, column++].Value = item.ContractorOpf;
                ws.Cells[currentRow, column++].Value = $"{item.ContractorRegion} - {item.ContractorDistrict}";
                ws.Cells[currentRow, column++].Value = item.ContractorInn;
                ws.Cells[currentRow, column++].Value = item.DocNumber;
                ws.Cells[currentRow, column++].Value = item.DocOn.ToString();
                ws.Cells[currentRow, column++].Value = item.DocOn.ToString();
                ws.Cells[currentRow, column++].Value = item.ContractorPhoneNumber;

                currentRow++;
            }

            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    #endregion

    #region SRV
    public Stream SaveAsExcelGetSrvServiceInfo(GetSrvServiceInfoRequestDto dto)
    {
        var data = GetSrvServicesInfo(dto);
        MemoryStream result = new MemoryStream();

        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.REPORT_SRV_SERVICE_GET_INFO));
        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var namerange = excelPackage.Workbook.Names["ImportRow"];
            var ws = namerange.Worksheet;

            var importRow = excelPackage.Workbook.Names["ImportRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            foreach (var item in data)
            {

                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                if (dto.ByRegion)
                    ws.Cells[currentRow, column++].Value = item.Region;
                else if (dto.ByDistrict)
                    ws.Cells[currentRow, column++].Value = item.District;

                ws.Cells[currentRow, column++].Value = item.PlanPaidCount;
                ws.Cells[currentRow, column++].Value = item.PlanPaidSum;
                ws.Cells[currentRow, column++].Value = item.LegalAmount;
                ws.Cells[currentRow, column++].Value = item.EconomomyAmount;
                ws.Cells[currentRow, column++].Value = item.TotalMemshipPaymentOrderCount.TotalCount;
                ws.Cells[currentRow, column++].Value = item.TotalMemshipPaymentOrderCount.TotalSum;
                ws.Cells[currentRow, column++].Value = item.TotalMemshipPaymentOrderCount.LegalAmount;
                ws.Cells[currentRow, column++].Value = item.TotalMemshipPaymentOrderCount.EconomomyAmount;
                ws.Cells[currentRow, column++].Value = item.TotalMemshipPaymentOrderCount.BirjaAmount;

                ws.Cells[currentRow, column++].Value = item.AcceptedPaidCountPercentage;
                ws.Cells[currentRow, column++].Value = item.AcceptedPaidSumPercentage;

                ws.Cells[currentRow, column++].Value = item.PaidCoef;

                ws.Cells[currentRow, column++].Value = item.PlanFreeCount;
                ws.Cells[currentRow, column++].Value = item.AcceptedFreeCount;
                ws.Cells[currentRow, column++].Value = item.AcceptedFreePercentage;
                ws.Cells[currentRow, column++].Value = item.FreeCoef;

                currentRow++;
            }

            var columnTotal = 3;
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.PlanPaidCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.PlanPaidSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.LegalAmount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.EconomomyAmount);

            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.TotalMemshipPaymentOrderCount.TotalCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.TotalMemshipPaymentOrderCount.TotalSum);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.TotalMemshipPaymentOrderCount.LegalAmount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.TotalMemshipPaymentOrderCount.EconomomyAmount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.TotalMemshipPaymentOrderCount.BirjaAmount);

            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.AcceptedPaidCountPercentage);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.AcceptedPaidSumPercentage);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.PaidCoef);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.PlanFreeCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.AcceptedFreeCount);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.AcceptedFreePercentage);
            ws.Cells[currentRow, columnTotal++].Value = data.Sum(item => item.FreeCoef);
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;

    }

	public Stream MonoApplicationReportSaveExcel(MonoApplicationReportSortFilter filter)
    {
        var data = MonoApplicationReport(filter);
		MemoryStream result = new MemoryStream();
		MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.MONO_APPLICATION_REPORT));
        if(IsValid && data != null)
        {

			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			ExcelPackage excelPackage = new ExcelPackage(template);

			var importRow = excelPackage.Workbook.Names["ImportRow"];
			int currentRow = importRow.Start.Row + 1;
			var ws = importRow.Worksheet;
            int index = data.Count();
            data.Reverse();
            foreach (var item in data)
            {
                var column = 1;
				ws.InsertRow(currentRow, 1, importRow.Start.Row);
				ws.Cells[currentRow, column++].Value = index--;

                if(!string.IsNullOrEmpty(item.ContractorName))
				    ws.Cells[currentRow, column++].Value = item.ContractorName;

                else if(!string.IsNullOrEmpty(item.DistricName))
					ws.Cells[currentRow, column++].Value = item.DistricName;

                else
					ws.Cells[currentRow, column++].Value = item.RegionName;

				ws.Cells[currentRow, column++].Value = item.AllApplicaions.Count;
				ws.Cells[currentRow, column++].Value = item.AllApplicaions.TotalAmount;
				ws.Cells[currentRow, column++].Value = item.AllApplicaions.TotalCost;

				ws.Cells[currentRow, column++].Value = item.ReviewApplications.Count;
				ws.Cells[currentRow, column++].Value = item.ReviewApplications.TotalAmount;
				ws.Cells[currentRow, column++].Value = item.ReviewApplications.TotalCost;

				ws.Cells[currentRow, column++].Value = item.AskApplications.RejectApplicaions.Count;
				ws.Cells[currentRow, column++].Value = item.AskApplications.RejectApplicaions.TotalAmount;
				ws.Cells[currentRow, column++].Value = item.AskApplications.RejectApplicaions.TotalCost;

				ws.Cells[currentRow, column++].Value = item.AskApplications.AcceptApplicaions.Count;
				ws.Cells[currentRow, column++].Value = item.AskApplications.AcceptApplicaions.TotalAmount;
				ws.Cells[currentRow, column++].Value = item.AskApplications.AcceptApplicaions.TotalCost;
				ws.Cells[currentRow, column++].Value = item.AskApplications.AcceptApplicaions.SubsidyAmount;

			}

			string[] columnsToSum = Enumerable.Range('C', 'O' - 'C' + 1)
								  .Select(c => ((char)c).ToString())
								  .ToArray();

			foreach (string column in columnsToSum)
			{
				// Находим последнюю заполненную строку в столбце
				int lastRow = ws.Cells[$"{column}:{column}"]
					.LastOrDefault(c => !string.IsNullOrEmpty(c.Text))?.Start.Row ?? 2;

				// Создаем формулу суммы
				string formula = $"=SUM({column}2:{column}{lastRow})";

				// Применяем формулу к ячейке под последней строкой данных
				ws.Cells[$"{column}{lastRow + 1}"].Formula = formula;

			}

			ws.DeleteRow(importRow.Start.Row);
			result = new MemoryStream(excelPackage.GetAsByteArray());
			excelPackage.Dispose();
		}
		result.Position = 0;
		return result;
	}
	#endregion

	#region HRM
	public Stream SaveAsExcelGetHrmCommands(HrmEmployeeDocumentDtoFilter dto)
    {
        var data = GetReportDocumentsForHrm(dto);

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst
                            .ExcelTemplate
                            .GetFileName(_cultureHelper.CurrentCulture.Code,
                                                StaticFileConst.Report.HRM_COMMANDS_REPORT));

        if (data != null)
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
                if (dto.ByOrganization)
                    ws.Cells[currentRow, column++].Value = item.OrganizationName;
                if (dto.ByDepartment)
                    ws.Cells[currentRow, column++].Value = item.Department;
                if (dto.ByPosition)
                    ws.Cells[currentRow, column++].Value = item.Position;
                if (dto.ByEmployee)
                    ws.Cells[currentRow, column++].Value = item.Employee;
                ws.Cells[currentRow, column++].Value = item.TotalAppoimtEmployeesHireCount;
                ws.Cells[currentRow, column++].Value = item.TotalAppoimtEmployeesTransferCount;
                ws.Cells[currentRow, column++].Value = item.TotalAppoimtEmployeesDismissilCount;
                ws.Cells[currentRow, column++].Value = item.TotalChastisementPenaltyCount;
                ws.Cells[currentRow, column++].Value = item.TotalChastisementReprimandCount;
                ws.Cells[currentRow, column++].Value = item.TotalTempCalcKindFinancialCount;
                ws.Cells[currentRow, column++].Value = item.TotalTempCalcKindIncentiveCount;
                ws.Cells[currentRow, column++].Value = item.TotalOrderToSendBusinessTripCount;
                ws.Cells[currentRow, column++].Value = item.TotalOrderToSendBusinessTripCountryCount;
                ws.Cells[currentRow, column++].Value = item.TotalOrderToSendBusinessTripAnotherOrgCount;
                ws.Cells[currentRow, column++].Value = item.TotalEmployeeLeaveOrderCount;
                ws.Cells[currentRow, column++].Value = item.TotalEmployeeSendTrainCount;
                ws.Cells[currentRow, column++].Value = item.TotalEmployeeSickLeaveHomladorlikCount;
                ws.Cells[currentRow, column++].Value = item.TotalEmployeeSickLeaveBolaParvarishiCount;
                ws.Cells[currentRow, column++].Value = item.TotalRecallLeaveCount;
                currentRow++;
            }
            ws.Cells[currentRow, 2].Value = "Jami:";
            ws.Cells[currentRow, 3].Value = data.Sum(item => item.TotalAppoimtEmployeesHireCount);
            ws.Cells[currentRow, 4].Value = data.Sum(item => item.TotalAppoimtEmployeesTransferCount);
            ws.Cells[currentRow, 5].Value = data.Sum(item => item.TotalAppoimtEmployeesDismissilCount);
            ws.Cells[currentRow, 6].Value = data.Sum(item => item.TotalChastisementPenaltyCount);
            ws.Cells[currentRow, 7].Value = data.Sum(item => item.TotalChastisementReprimandCount);
            ws.Cells[currentRow, 8].Value = data.Sum(item => item.TotalTempCalcKindFinancialCount);
            ws.Cells[currentRow, 9].Value = data.Sum(item => item.TotalTempCalcKindIncentiveCount);
            ws.Cells[currentRow, 10].Value = data.Sum(item => item.TotalOrderToSendBusinessTripCount);
            ws.Cells[currentRow, 11].Value = data.Sum(item => item.TotalOrderToSendBusinessTripCountryCount);
            ws.Cells[currentRow, 12].Value = data.Sum(item => item.TotalOrderToSendBusinessTripAnotherOrgCount);
            ws.Cells[currentRow, 13].Value = data.Sum(item => item.TotalEmployeeLeaveOrderCount);
            ws.Cells[currentRow, 14].Value = data.Sum(item => item.TotalEmployeeSendTrainCount);
            ws.Cells[currentRow, 15].Value = data.Sum(item => item.TotalEmployeeSickLeaveHomladorlikCount);
            ws.Cells[currentRow, 16].Value = data.Sum(item => item.TotalEmployeeSickLeaveBolaParvarishiCount);
            ws.Cells[currentRow, 17].Value = data.Sum(item => item.TotalRecallLeaveCount);
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelGetStaffCountByGenderReport(StaffCountByGenderDtoFilter dto)
    {
        var data = GetStaffCountByGenderReport(dto).ToArray();


        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst
                            .ExcelTemplate
                            .GetFileName(_cultureHelper.CurrentCulture.Code,
                                                StaticFileConst.Report.HRM_GET_STAFF_COUNT_BY_GENDER_REPORT));


        if (data != null)
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
                if (dto.ByOrganization)
                    ws.Cells[currentRow, column++].Value = item.Organization;
                if (dto.ByDepartment)
                    ws.Cells[currentRow, column++].Value = item.Department;
                if (dto.ByPosition)
                    ws.Cells[currentRow, column++].Value = item.Position;
                if (dto.ByEmployee)
                    ws.Cells[currentRow, column++].Value = item.Employee;
                ws.Cells[currentRow, column++].Value = item.TotalStaffingRate;
                ws.Cells[currentRow, column++].Value = item.TotalEmployeeManageRate;
                ws.Cells[currentRow, column++].Value = item.TotalCount;
                ws.Cells[currentRow, column++].Value = item.TotalEmployee;
                ws.Cells[currentRow, column++].Value = item.TotalEmployeeMen;
                ws.Cells[currentRow, column++].Value = item.TotalEmployeeWomen;
                if (item.PositionCategorys == null)
                {
                    currentRow++;
                    continue;
                }

                foreach (var temp in item.PositionCategorys)
                {

                    ws.Cells[currentRow, column++].Value = temp.Count;
                    ws.Cells[currentRow, column++].Value = temp.Men;
                    ws.Cells[currentRow, column++].Value = temp.Women;
                }
                currentRow++;
            }
            ws.Cells[currentRow, 2].Value = "Jami:";
            ws.Cells[currentRow, 3].Value = data.Sum(item => item.TotalStaffingRate);
            ws.Cells[currentRow, 4].Value = data.Sum(item => item.TotalEmployeeManageRate);
            ws.Cells[currentRow, 5].Value = data.Sum(item => item.TotalCount);
            ws.Cells[currentRow, 6].Value = data.Sum(item => item.TotalEmployee);
            ws.Cells[currentRow, 7].Value = data.Sum(item => item.TotalEmployeeMen);
            ws.Cells[currentRow, 8].Value = data.Sum(item => item.TotalEmployeeWomen);
            ws.Cells[currentRow, 9].Value = data.Sum(item => item.PositionCategorys?.Where(a => a.PositionCategoryId == 1).Sum(B => B.Count));
            ws.Cells[currentRow, 10].Value = data.Sum(item => item.PositionCategorys?.Where(a => a.PositionCategoryId == 1).Sum(B => B.Men));
            ws.Cells[currentRow, 11].Value = data.Sum(item => item.PositionCategorys?.Where(a => a.PositionCategoryId == 1).Sum(B => B.Women));
            ws.Cells[currentRow, 12].Value = data.Sum(item => item.PositionCategorys?.Where(a => a.PositionCategoryId == 2).Sum(B => B.Count));
            ws.Cells[currentRow, 13].Value = data.Sum(item => item.PositionCategorys?.Where(a => a.PositionCategoryId == 2).Sum(B => B.Men));
            ws.Cells[currentRow, 14].Value = data.Sum(item => item.PositionCategorys?.Where(a => a.PositionCategoryId == 2).Sum(B => B.Women));
            ws.Cells[currentRow, 15].Value = data.Sum(item => item.PositionCategorys?.Where(a => a.PositionCategoryId == 3).Sum(B => B.Count));
            ws.Cells[currentRow, 16].Value = data.Sum(item => item.PositionCategorys?.Where(a => a.PositionCategoryId == 3).Sum(B => B.Men));
            ws.Cells[currentRow, 17].Value = data.Sum(item => item.PositionCategorys?.Where(a => a.PositionCategoryId == 3).Sum(B => B.Women));
            ws.Cells[currentRow, 18].Value = data.Sum(item => item.PositionCategorys?.Where(a => a.PositionCategoryId == 4).Sum(B => B.Count));
            ws.Cells[currentRow, 19].Value = data.Sum(item => item.PositionCategorys?.Where(a => a.PositionCategoryId == 4).Sum(B => B.Men));
            ws.Cells[currentRow, 20].Value = data.Sum(item => item.PositionCategorys?.Where(a => a.PositionCategoryId == 4).Sum(B => B.Women));


            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public Stream SaveAsExcelGetStateEmploymentReport(StaffCountDtoFilter dto)
    {
        var data = GetStaffCountReport(dto);

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst
                            .ExcelTemplate
                            .GetFileName(_cultureHelper.CurrentCulture.Code,
                                                StaticFileConst.Report.HRM_STATE_EMPLOYMENT_REPORT));

        if (data != null)
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


                if (dto.ByRegion == true)
                    ws.Cells[currentRow, column++].Value = item.Region;
                else if (dto.ByDepartment == true)
                    ws.Cells[currentRow, column++].Value = item.Department;
                else if (dto.ByPosition == true)
                    ws.Cells[currentRow, column++].Value = item.Position;
                else
                    ws.Cells[currentRow, column++].Value = item.Organization;
                ws.Cells[currentRow, column++].Value = item.TotalStaffingRate;
                ws.Cells[currentRow, column++].Value = item.TotalEmployeeManageRate;
                ws.Cells[currentRow, column++].Value = item.TotalCount;
                currentRow++;
            }
            ws.Cells[currentRow, 2].Value = "Jami:";
            ws.Cells[currentRow, 3].Value = data.Sum(item => item.TotalStaffingRate);
            ws.Cells[currentRow, 4].Value = data.Sum(item => item.TotalEmployeeManageRate);
            ws.Cells[currentRow, 5].Value = data.Sum(item => item.TotalCount);

            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }
        result.Position = 0;
        return result;
    }
    public async Task<Stream> SaveAsExecelForCollectedReport(PrtnApplicationByContractTypeDtoFilter filter)
    {
        var data = GetPrtnApplicationByContractTypeMethod(filter);

        var bankCreditReport = await GetBankCreditReport(new ContractorBankCreditReportDtoFilter
        {
            ByRegion = filter.ByRegion
        });


        var bankCreditReportByBank = await GetBankCreditReport(new ContractorBankCreditReportDtoFilter
        {
            ByRegion = filter.ByRegion,
            ByBank = true
        });
        var businessActivityReport = GetBusinessActivityTypeReportByRegion(new BusinessActivityTypeReportByRegionFilter
        {
            ByRegion = filter.ByRegion
        });

        var BojxconaImtiyozReport = GetBojxonaImtiyozReportByContractor(new BojxonaImtiyozReportByContractorDtoFilter
        {
            ByRegion = filter.ByRegion,
            HasCertificate = true
        });

        var taxCreditReport = GetTaxCreditReport(new TaxCreditReportDtoFilter
        {
            Year = DateTime.Now.Year,
            ByRegion = filter.ByRegion

        });
        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.COLLECTED_REPORTS));

        if (IsValid && data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);

            var ws = excelPackage.Workbook.Worksheets[0];



            var importRow = excelPackage.Workbook.Names["InsertRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;

            double summSubmittedCount = 0, issuanceCount = 0, issuanceSum = 0, rejectedCount = 0, rejectedSum = 0, otherCount = 0, otherSum = 0, submittedCount = 0;

            for (int i = 0; i < data.Rows.Count; i++)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;

                //jamlanma hisobot
                ws.Cells[currentRow, column++].Value = data.Rows[i].Region;
                ws.Cells[currentRow, column++].Value = data.Rows[i].TotalApplication.TotalCount;
                ws.Cells[currentRow, column++].Value = data.Rows[i].TotalApplication.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = data.Rows[i].TotalCertificate.TotalCount;
                ws.Cells[currentRow, column++].Value = data.Rows[i].TotalCertificate.TotalNewVacanciesCount;
                ws.Cells[currentRow, column++].Value = data.Rows[i].CountApplication.FirstOrDefault().Value.Count;
                ws.Cells[currentRow, column++].Value = data.Rows[i].CountApplication.FirstOrDefault().Value.NewVacanciesCount;

                ws.Cells[currentRow, column++].Value = data.Rows[i].CountApplication.Skip(1).Take(2).FirstOrDefault().Value.Count;
                ws.Cells[currentRow, column++].Value = data.Rows[i].CountApplication.Skip(1).Take(2).FirstOrDefault().Value.NewVacanciesCount;

                ws.Cells[currentRow, column++].Value = data.Rows[i].CountApplication.Skip(2).Take(3).FirstOrDefault().Value.Count;
                ws.Cells[currentRow, column++].Value = data.Rows[i].CountApplication.Skip(2).Take(3).FirstOrDefault().Value.NewVacanciesCount;

                //bank credit
                var regions = _unitOfWork.Context.Regions.FirstOrDefault(a => a.Id == data.Rows[i].RegionId);


                var bankRegion = bankCreditReport.FirstOrDefault(b => b.RegionId == regions.BankRegionId);

                if (bankRegion != null)
                {
                    ws.Cells[currentRow, column++].Value = bankRegion.Application.SubmittedCount;
                    summSubmittedCount += bankRegion.Application.SubmittedCount;

                    ws.Cells[currentRow, column++].Value = bankRegion.Application.IssuanceCount;
                    issuanceCount += bankRegion.Application.IssuanceCount;
                    ws.Cells[currentRow, column++].Value = bankRegion.Application.IssuanceSum;
                    issuanceSum += bankRegion.Application.IssuanceSum;

                    ws.Cells[currentRow, column++].Value = bankRegion.Application.RejectedCount;
                    rejectedCount += bankRegion.Application.RejectedCount;

                    ws.Cells[currentRow, column++].Value = bankRegion.Application.RejectedSum;
                    rejectedSum += bankRegion.Application.RejectedSum;

                    ws.Cells[currentRow, column++].Value = bankRegion.Application.SubmittedCount - bankRegion.Application.IssuanceCount - bankRegion.Application.RejectedCount;
                    otherCount += bankRegion.Application.SubmittedCount - bankRegion.Application.IssuanceCount - bankRegion.Application.RejectedCount;

                    ws.Cells[currentRow, column++].Value = bankRegion.Application.SubmittedSum - bankRegion.Application.IssuanceSum - bankRegion.Application.RejectedSum;
                    otherSum += bankRegion.Application.SubmittedSum - bankRegion.Application.IssuanceSum - bankRegion.Application.RejectedSum;
                }




                //kafillik

                var businessActivity = businessActivityReport.FirstOrDefault(b => b.RegionId == data.Rows[i].RegionId);
                ws.Cells[currentRow, column++].Value = businessActivity != null ? businessActivity.BusinessActivity.UserPrivilegeCount : 0;

                ws.Cells[currentRow, column++].Value = businessActivity != null ? businessActivity.BusinessActivity.UserPrivilegeCount : 0;

                ws.Cells[currentRow, column++].Value = businessActivity != null ? businessActivity.BusinessActivity.ApprovedFinancialHelpAmount : 0;

                //markazlar malumot yoq
                ws.Cells[currentRow, column++].Value = string.Empty;
                ws.Cells[currentRow, column++].Value = string.Empty;
                ws.Cells[currentRow, column++].Value = string.Empty;

                //tax soliq
                var taxCredit = taxCreditReport.FirstOrDefault(t => t.RegionId == data.Rows[i].RegionId);
                ws.Cells[currentRow, column++].Value = taxCredit != null ? taxCredit.TotalContractApplicationCount : 0;
                ws.Cells[currentRow, column++].Value = taxCredit != null ? taxCredit.SocialTaxSum + taxCredit.LandTaxSum + taxCredit.PropertyTaxSum : 0;
                ws.Cells[currentRow, column++].Value = taxCredit != null ? taxCredit.ContractorPropertyTaxCount + taxCredit.ContractorLandTaxCount : 0;
                ws.Cells[currentRow, column++].Value = taxCredit != null ? taxCredit.IncomeTaxCount : 0;
                ws.Cells[currentRow, column++].Value = taxCredit != null ? taxCredit.ContractorSocialTaxCount : 0;
                ws.Cells[currentRow, column++].Value = string.Empty;   //TEMP

                //bojhona tolovlari
                var BojxconaImtiyoz = BojxconaImtiyozReport.FirstOrDefault(b => b.RegionId == data.Rows[i].RegionId);
                ws.Cells[currentRow, column++].Value = BojxconaImtiyoz != null ? BojxconaImtiyoz.CertificateCount : 0;
                ws.Cells[currentRow, column++].Value = BojxconaImtiyoz != null ? BojxconaImtiyoz.TotalContractorCount : 0;
                ws.Cells[currentRow, column++].Value = BojxconaImtiyoz != null ? BojxconaImtiyoz.AppContractorCount : 0;

                currentRow++;
            }

            ws.Cells["M9"].Value = summSubmittedCount;
            ws.Cells["N9"].Value = issuanceCount;
            ws.Cells["O9"].Value = issuanceSum;
            ws.Cells["P9"].Value = rejectedCount;
            ws.Cells["Q9"].Value = rejectedSum;
            ws.Cells["R9"].Value = otherCount;
            ws.Cells["S9"].Value = otherSum;

            var importBankRow = excelPackage.Workbook.Names["InsertBankRows"];
            currentRow = importBankRow.Start.Row;
            index = 1;

            otherCount = 0;
            otherSum = 0;
            if (IsValid && bankCreditReportByBank != null)
            {
                var bankCredit = bankCreditReportByBank.Where(b => b.Application.SubmittedCount > 0).ToList();
                for (int i = 0; i < bankCredit.Count; i++)
                {
                    var column = 1;
                    ws.InsertRow(currentRow, 1, importRow.Start.Row);
                    ws.Cells[currentRow, column++].Value = index++;
                    ws.Cells[currentRow, column++].Value = bankCredit[i].BankName;
                    column = 13;
                    ws.Cells[currentRow, column++].Value = bankCredit[i].Application.SubmittedCount;
                    ws.Cells[currentRow, column++].Value = bankCredit[i].Application.IssuanceCount;
                    ws.Cells[currentRow, column++].Value = bankCredit[i].Application.IssuanceSum;
                    ws.Cells[currentRow, column++].Value = bankCredit[i].Application.RejectedCount;
                    ws.Cells[currentRow, column++].Value = bankCredit[i].Application.RejectedSum;
                    ws.Cells[currentRow, column++].Value = bankCredit[i].Application.SubmittedCount - bankCredit[i].Application.IssuanceCount - bankCredit[i].Application.RejectedCount;
                    ws.Cells[currentRow, column++].Value = bankCredit[i].Application.SubmittedSum - bankCredit[i].Application.IssuanceSum - bankCredit[i].Application.RejectedSum;
                    ws.Cells[currentRow, column++].Value = bankCredit[i].Application.SubmittedCount;
                    submittedCount += bankCredit[i].Application.SubmittedCount;
                    otherCount += bankCredit[i].Application.SubmittedCount - bankCredit[i].Application.IssuanceCount - bankCredit[i].Application.RejectedCount;
                    otherSum += bankCredit[i].Application.SubmittedSum - bankCredit[i].Application.IssuanceSum - bankCredit[i].Application.RejectedSum;

                    currentRow++;
                }

                ws.Cells["M28"].Value = bankCredit.Sum(s => s.Application.SubmittedCount);
                ws.Cells["N28"].Value = bankCredit.Sum(s => s.Application.IssuanceCount);
                ws.Cells["O28"].Value = bankCredit.Sum(s => s.Application.IssuanceSum);
                ws.Cells["P28"].Value = bankCredit.Sum(s => s.Application.RejectedCount);
                ws.Cells["Q28"].Value = bankCredit.Sum(s => s.Application.RejectedSum);
                ws.Cells["R28"].Value = otherCount;
                ws.Cells["S28"].Value = otherSum;
                ws.Cells["T28"].Value = submittedCount;
            }

            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();

        }


        result.Position = 0;
        return result;
    }



    public WEBASE.Models.PagedResult<PrtnApplicationAndContractInfoDto> GetPrtnApplicationAndContractInfoPaged(PrtnDocumentSortFilterOptions filter)
    {
        var result = new List<PrtnApplicationAndContractInfoDto>();

        var query = _unitOfWork.Context.Set<Application>()
            .Include(a => a.PrtnApplication)
            .Include(a => a.PrtnContract).ThenInclude(a => a.PrtnCertificate)
            .Include(a => a.Contractor).ThenInclude(c => c.Oked).ThenInclude(a => a.OkedType)
            .Where(a => new int[]
                {
                        StatusIdConst.SENT_FOR_REVIEW, StatusIdConst.SENT,
                        StatusIdConst.ACCEPTED, StatusIdConst.EXECUTING,
                        StatusIdConst.REJECTED, StatusIdConst.PASS_EXPERTISE,
                        StatusIdConst.NOT_PASS_EXPERTISE, StatusIdConst.SIGNED,
                        StatusIdConst.SIGNING, StatusIdConst.SENT_FOR_EXPERTISE,
                        StatusIdConst.CANCELED, StatusIdConst.REVOKED
                }.Contains(a.StatusId)
                && a.ApplicationTypeId == ApplicationTypeIdConst.PARTNER
            )
            .Where(a => !filter.PrtnContractTypeId.HasValue || filter.PrtnContractTypeId == a.PrtnApplication.PrtnContractTypeId);

        query = query.Where(a => (filter.StartDate.HasValue ? a.DocOn >= filter.StartDate.Value : true)
              && (filter.EndDate.HasValue ? a.DocOn <= filter.EndDate.Value : true));

        query = query.Where(a => (!filter.RegionId.HasValue || filter.RegionId == (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId : a.RegionId))
            && (!filter.DistrictId.HasValue || filter.DistrictId == (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId))
            && (!filter.ContractorId.HasValue || filter.ContractorId == a.ContractorId) && (!filter.OkedTypeId.HasValue || filter.OkedTypeId == a.Contractor.Oked.OkedTypeId));

        var query2 = query.ToList();

        result = query
            .Select(a => new PrtnApplicationAndContractInfoDto
            {
                PrtnContractTypeId = filter.PrtnContractTypeId.HasValue ? a.PrtnApplication.PrtnContractTypeId : 0,
                PrtnContractType = filter.PrtnContractTypeId.HasValue ? (a.PrtnApplication.PrtnContractType.Translates.AsQueryable()
                    .FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.PrtnContractType.FullName) : "",

                RegionId = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId : a.RegionId) : null,
                RegionOrderCode = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegion.OrderCode : a.Region.OrderCode) : null,
                Region = filter.ByRegion ? (a.PrtnApplication.ChooseLocation ? (a.PrtnApplication.ChoosedRegion.Translates.AsQueryable()
                            .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.ChoosedRegion.FullName)
                        : (a.Region.Translates.AsQueryable()
                            .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.Region.FullName))
                    : null,

                DistrictId = filter.ByDistrict ? (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId : a.DistrictId) : null,
                District = filter.ByDistrict
                    ? (a.PrtnApplication.ChooseLocation
                        ? (a.PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
                            .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.PrtnApplication.ChoosedDistrict.FullName)
                        : (a.District.Translates.AsQueryable()
                            .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.District.FullName))
                    : null,

                ContractorId = filter.ByContractor ? a.ContractorId : null,
                Contractor = filter.ByContractor ? a.Contractor.FullName : null,
                ContractorInn = filter.ByContractor ? a.Contractor.Inn : null,
                ContractorPhoneNumber = filter.ByContractor ? a.Contractor.PhoneNumber : null,
                NewVacanciesCount = a.PrtnApplication.NewVacanciesCount,

                TotalPrtnApplicationSentCount = a.StatusId == StatusIdConst.SENT ? 1 : 0,
                TotalPrtnApplicationPassExpertisesCount = a.PrtnContract.StatusId == StatusIdConst.PASS_EXPERTISE ? 1 : 0,
                TotalPrtnApplicationSentForExpertisesCount = a.PrtnContract.StatusId == StatusIdConst.SENT_FOR_EXPERTISE ? 1 : 0,
                TotalPrtnApplicationNotPassExpertisesCount = a.PrtnContract.StatusId == StatusIdConst.NOT_PASS_EXPERTISE ? 1 : 0,
                TotalPrtnApplicationSignedCount = a.PrtnContract.StatusId == StatusIdConst.SIGNED ? 1 : 0,
                TotalPrtnApplicationSignningCount = a.PrtnContract.StatusId == StatusIdConst.SIGNING ? 1 : 0,
                TotalPrtnApplicationSentForReviewCount = a.StatusId == StatusIdConst.SENT_FOR_REVIEW ? 1 : 0,
                TotalPrtnApplicationSentAcceptedCount = a.StatusId == StatusIdConst.ACCEPTED ? 1 : 0,
                TotalPrtnApplicationSentRejectedCount = a.StatusId == StatusIdConst.REJECTED ? 1 : 0,
                TotalPrtnApplicationSentRevokedCount = a.StatusId == StatusIdConst.REVOKED ? 1 : 0,
                TotalPrtnApplicationCanceledCount = a.StatusId == StatusIdConst.CANCELED ? 1 : 0,

                TotalPrtnApplicationCanceledWhithOutContractCount = a.PrtnContract == null && a.StatusId == StatusIdConst.CANCELED ? 1 : 0,
                TotalPrtnApplicationCanceledWhithOutRejectCount = a.PrtnContract == null && a.StatusId == StatusIdConst.REJECTED ? 1 : 0,

                TotalPrtnContractCanceledWhithOutCertificateCount = a.PrtnContract.PrtnCertificate == null && a.PrtnContract.StatusId == StatusIdConst.CANCELED ? 1 : 0,
                TotalPrtnContractRejectWhithOutCertificateCount = a.PrtnContract.PrtnCertificate == null && a.PrtnContract.StatusId == StatusIdConst.REJECTED ? 1 : 0,

                TotalPrtnCertificateCanceledApplicationCount = (a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.CANCELED) ? 1 : 0,
                TotalPrtnCertificateRejectApplicationCount = (a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.REJECTED) ? 1 : 0,

                TotalPrtnContractCount = a.PrtnContract != null && a.PrtnContract.StatusId != StatusIdConst.REJECTED ? 1 : 0,
                TotalPrtnContractCancelCount = a.PrtnContract != null && a.PrtnContract.StatusId == StatusIdConst.CANCELED ? 1 : 0,
                TotalPrtnContractRejectedCount = a.PrtnContract != null && a.PrtnContract.StatusId == StatusIdConst.REJECTED ? 1 : 0,
                TotalPrtnCertificateCount = a.PrtnContract.PrtnCertificate != null && a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED ? 1 : 0,
                TotalPrtnCertificateCanceledCount = a.PrtnContract.PrtnCertificate != null && a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.CANCELED ? 1 : 0,
                TotalNewVacanciesCount = a.PrtnContract.PrtnCertificate.StatusId == StatusIdConst.FORMED ? a.PrtnApplication.NewVacanciesCount : 0,
                TotalPrtnApplicationIsOffersCount = a.Contractor.IsLastOffer ? 1 : 0,
            }).AsNoTracking().ToList()

            .GroupBy(a => new
            {
                a.PrtnContractTypeId,
                a.PrtnContractType,
                a.DistrictId,
                a.District,
                a.RegionId,
                a.RegionOrderCode,
                a.Region,
                a.ContractorId,
                a.Contractor,
                a.ContractorInn,
                a.ContractorPhoneNumber,
            })
            .Select(a => new PrtnApplicationAndContractInfoDto
            {
                PrtnContractTypeId = a.Key.PrtnContractTypeId,
                PrtnContractType = a.Key.PrtnContractType,
                DistrictId = a.Key.DistrictId,
                District = a.Key.District,
                RegionId = a.Key.RegionId,
                RegionOrderCode = a.Key.RegionOrderCode,
                Region = a.Key.Region,
                ContractorId = a.Key.ContractorId,
                Contractor = a.Key.Contractor,
                ContractorInn = a.Key.ContractorInn,
                ContractorPhoneNumber = a.Key.ContractorPhoneNumber,
                TotalPrtnApplicationSentCount = a.Sum(b => b.TotalPrtnApplicationSentCount),

                TotalApplication = ((long)a.Count(), a.Sum(c => c.NewVacanciesCount)),

                TotalPrtnApplicationCanceledWhithOutContractCount = a.Sum(b => b.TotalPrtnApplicationCanceledWhithOutContractCount),
                TotalPrtnApplicationCanceledWhithOutRejectCount = a.Sum(b => b.TotalPrtnApplicationCanceledWhithOutRejectCount),


                TotalPrtnContractCanceledWhithOutCertificateCount = a.Sum(b => b.TotalPrtnContractCanceledWhithOutCertificateCount),
                TotalPrtnContractRejectWhithOutCertificateCount = a.Sum(b => b.TotalPrtnContractRejectWhithOutCertificateCount),

                TotalPrtnCertificateCanceledApplicationCount = a.Sum(b => b.TotalPrtnCertificateCanceledApplicationCount),
                TotalPrtnCertificateRejectApplicationCount = a.Sum(b => b.TotalPrtnCertificateRejectApplicationCount),

                TotalPrtnApplicationSentForReviewCount = a.Sum(b => b.TotalPrtnApplicationSentForReviewCount),
                TotalPrtnApplicationSentAcceptedCount = a.Sum(b => b.TotalPrtnApplicationSentAcceptedCount),
                TotalPrtnApplicationPassExpertisesCount = a.Sum(b => b.TotalPrtnApplicationPassExpertisesCount),
                TotalPrtnApplicationSentForExpertisesCount = a.Sum(b => b.TotalPrtnApplicationSentForExpertisesCount),
                TotalPrtnApplicationNotPassExpertisesCount = a.Sum(b => b.TotalPrtnApplicationNotPassExpertisesCount),
                TotalPrtnApplicationSignedCount = a.Sum(b => b.TotalPrtnApplicationSignedCount),
                TotalPrtnApplicationSignningCount = a.Sum(b => b.TotalPrtnApplicationSignningCount),
                TotalPrtnApplicationCanceledCount = a.Sum(b => b.TotalPrtnApplicationCanceledCount),
                TotalPrtnApplicationSentRejectedCount = a.Sum(b => b.TotalPrtnApplicationSentRejectedCount),
                TotalPrtnApplicationSentRevokedCount = a.Sum(b => b.TotalPrtnApplicationSentRevokedCount),
                TotalPrtnContractCount = a.Sum(b => b.TotalPrtnContractCount),
                TotalPrtnContractCancelCount = a.Sum(b => b.TotalPrtnContractCancelCount),
                TotalPrtnContractRejectedCount = a.Sum(b => b.TotalPrtnContractRejectedCount),
                TotalPrtnCertificateCount = a.Sum(b => b.TotalPrtnCertificateCount),
                TotalPrtnCertificateCanceledCount = a.Sum(b => b.TotalPrtnCertificateCanceledCount),
                TotalNewVacanciesCount = a.Sum(b => b.TotalNewVacanciesCount),
                TotalPrtnApplicationCount = a.Sum(b => b.TotalPrtnApplicationSentCount) + a.Sum(b => b.TotalPrtnApplicationSentForReviewCount) + a.Sum(b => b.TotalPrtnApplicationSentAcceptedCount),
                TotalPrtnApplicationIsOffersCount = a.Sum(b => b.TotalPrtnApplicationIsOffersCount),
            })
            .ToList();

        // Hamma viloyatlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
        if (filter.ByRegion)
        {
            var regions = _unitOfWork.RegionRepository.AllAsQueryable
                .Include(a => a.Translates)
                .IsActive()
                .Where(a => !filter.RegionId.HasValue || filter.RegionId == a.Id)
                .ToDictionary(
                    a => a.Id,
                    a => new
                    {
                        OrderCode = a.OrderCode,
                        FullName = a.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))?.TranslateText ?? a.FullName
                    }
                );

            foreach (var region in regions)
            {
                if (result.Select(a => a.RegionId).Contains(region.Key))
                    continue;

                result.Add(new PrtnApplicationAndContractInfoDto
                {
                    Region = region.Value.FullName,
                    RegionOrderCode = region.Value.OrderCode,
                    RegionId = region.Key
                });
            }

            result = result.OrderBy(a => a.RegionOrderCode).ToList();
        }

        // Viloyatdagi hamma tumanlar boyicha korishi uchun. Ma'lumot bo'masa sonini 0 qilib ekranda ko'rsatamiz
        if (filter.ByDistrict)
        {
            var districts = _unitOfWork.DistrictRepository.AllAsQueryable
                .Include(a => a.Region).ThenInclude(a => a.Translates)
                .Include(a => a.Translates)
                .IsActive()
                .Where(a => !filter.RegionId.HasValue || filter.RegionId == a.RegionId
                    && !filter.DistrictId.HasValue || filter.DistrictId == a.Id
                )
                .ToDictionary(
                    a => a.Id,
                    a => new
                    {
                        RegionId = a.RegionId,
                        Region = a.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                            ?.TranslateText
                        ?? a.Region.FullName,
                        District = a.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                            ?.TranslateText
                        ?? a.FullName,
                        DistrictOrderCode = a.OrderCode,
                    }
                );

            foreach (var district in districts)
            {
                if (result.Select(a => a.DistrictId).Contains(district.Key))
                    continue;

                result.Add(new PrtnApplicationAndContractInfoDto
                {
                    Region = district.Value.Region,
                    RegionId = district.Value.RegionId,
                    District = district.Value.District,
                    RegionOrderCode = district.Value.DistrictOrderCode,
                    DistrictId = district.Key
                });
            }

            result = result.OrderBy(a => a.RegionOrderCode).ToList();
        }

        return result.AsQueryable().AsPagedResult(filter);
    }

    #endregion

    #region ijro kechikishi hisoboti
    public Stream SaveAsExcelExpiredContractorsReport(PrtnApplicationAndContractInfoDtoFilter filter)
    {
        List<PrtnApplicationAndContractInfoDto>? data = GetExpiredContractorsReport(filter);

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.Expired_Execution_Report));

        if(data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);
            var ws = excelPackage.Workbook.Worksheets[0];
            var importRow = excelPackage.Workbook.Names["insertrow"];
            int currentRow = importRow.Start.Row + 1;
            int index = data.Count;

            data.Reverse();
            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index--;

                if(filter.RegionId == null && filter.DistrictId == null)
                         ws.Cells[currentRow, column++].Value = item.Region;

                else if (filter.RegionId != null && filter.DistrictId == null)
                    ws.Cells[currentRow, column++].Value = item.District;

                else  ws.Cells[currentRow, column++].Value = $"{item.ContractorInn} - {item.Contractor}";

                ws.Cells[currentRow, column++].Value = item.IsBeingCosideredApplicationCount;

                ws.Cells[currentRow, column++].Value = item.IsBeingCosideredApplicationCount;
                ws.Cells[currentRow, column++].Value = item.ExpiredIsBeingCosideredApplicationCount;
                ws.Cells[currentRow, column++].Value = item.SendToExpertiseCount;
                ws.Cells[currentRow, column++].Value = item.ExpiredSendToExpertiseCount;
                ws.Cells[currentRow, column++].Value = item.NotPassCount;
                ws.Cells[currentRow, column++].Value = item.ExpiredResentToExpiredCount;
                ws.Cells[currentRow, column++].Value = item.PassCount1;
                ws.Cells[currentRow, column++].Value = item.SignExpireOnCount1;
                ws.Cells[currentRow, column++].Value = item.PassCount2;
                ws.Cells[currentRow, column++].Value = item.SigningExpireOnCount;
                ws.Cells[currentRow, column++].Value = item.SigningCount;
                ws.Cells[currentRow, column++].Value = item.SignExpireOnCount2;
                ws.Cells[currentRow, column++].Value = item.NotGeneratedCertificatesCount;
                ws.Cells[currentRow, column++].Value = item.GeneratedCertificatesCount;
            }
            string[] columnsToSum = Enumerable.Range('C', 'Q' - 'C' + 1)
                                    .Select(c => ((char)c).ToString())
                                    .ToArray();

            foreach (string column in columnsToSum)
            {
                // Находим последнюю заполненную строку в столбце
                int lastRow = ws.Cells[$"{column}:{column}"]
                    .LastOrDefault(c => !string.IsNullOrEmpty(c.Text))?.Start.Row ?? 2;

                // Создаем формулу суммы
                string formula = $"=SUM({column}2:{column}{lastRow})";

                // Применяем формулу к ячейке под последней строкой данных
                ws.Cells[$"{column}{lastRow + 1}"].Formula = formula;
            }
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }

        result.Position = 0;
        return result;
    }

    public  Stream SaveAsExcelAllExpiredContractorsReport(PrtnDocumentSortFilterOptions filter)
    {
        IQueryable<PrtnApplicationAndContractInfoDto>? data =  GetExpiredContractorsReportMethod(filter);

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.Expired_Execution_Report));

        if (data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);
            var ws = excelPackage.Workbook.Worksheets[0];
            var importRow = excelPackage.Workbook.Names["insertrow"];
            int currentRow = importRow.Start.Row + 1;
            int index = data.Count();

            data.Reverse();
            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index--;

                ws.Cells[currentRow, column++].Value = $"{item.ContractorInn} - {item.Contractor}";

                ws.Cells[currentRow, column++].Value = item.IsBeingCosideredApplicationCount;

                ws.Cells[currentRow, column++].Value = item.IsBeingCosideredApplicationCount;
                ws.Cells[currentRow, column++].Value = item.ExpiredIsBeingCosideredApplicationCount;
                ws.Cells[currentRow, column++].Value = item.SendToExpertiseCount;
                ws.Cells[currentRow, column++].Value = item.ExpiredSendToExpertiseCount;
                ws.Cells[currentRow, column++].Value = item.NotPassCount;
                ws.Cells[currentRow, column++].Value = item.ExpiredResentToExpiredCount;
                ws.Cells[currentRow, column++].Value = item.PassCount1;
                ws.Cells[currentRow, column++].Value = item.SignExpireOnCount1;
                ws.Cells[currentRow, column++].Value = item.PassCount2;
                ws.Cells[currentRow, column++].Value = item.SigningExpireOnCount;
                ws.Cells[currentRow, column++].Value = item.SigningCount;
                ws.Cells[currentRow, column++].Value = item.SignExpireOnCount2;
                ws.Cells[currentRow, column++].Value = item.NotGeneratedCertificatesCount;
                ws.Cells[currentRow, column++].Value = item.GeneratedCertificatesCount;
            }
            ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();

        }

        result.Position = 0;
        return result;
    }


    public Stream SaveAsExcelClaimApplicationReport(ClaimApplicationDtoFilter filter)
    {
        var data = ClaimApplicationReport(filter);
        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.CLAIM_APPLICATION_REPORT));

        if(data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template); 

            var ws = excelPackage.Workbook.Worksheets[0];
            var importRow = excelPackage.Workbook.Names["InsertRow"];
            int currentRow = importRow.Start.Row + 1;
            int index = 1;
            foreach (var item in data)
            {
                var column = 1;
                ws.InsertRow(currentRow, 1, importRow.Start.Row);
                ws.Cells[currentRow, column++].Value = index++;
                ws.Cells[currentRow, column++].Value = item.Region;
                ws.Cells[currentRow, column++].Value = item.TotalClaimApplicationCount;
                ws.Cells[currentRow, column++].Value = item.TotalClaimApplicationAmount.Uzs;
                ws.Cells[currentRow, column++].Value = item.TotalClaimApplicationAmount.Usd;
                ws.Cells[currentRow, column++].Value = item.TotalClaimApplicationAmount.Euro;
                ws.Cells[currentRow, column++].Value = item.TotalEconomicCourt;
                ws.Cells[currentRow, column++].Value = item.TotalCivilCourt;
                ws.Cells[currentRow, column++].Value = item.TotalAdministrativeCourt;
                //ws.Cells[currentRow, column++].Value = item.LeganClaims;
                //ws.Cells[currentRow, column++].Value = item.SatisfiedClaims;
                //ws.Cells[currentRow, column++].Value = item.CanceledClaims;
                //ws.Cells[currentRow, column++].Value = item.RejectedClaims;
                ws.Cells[currentRow, column++].Value = item.TotalAppilationCount;
                ws.Cells[currentRow, column++].Value = item.TotalAppilationAmount;
                ws.Cells[currentRow, column++].Value = item.TotalAppilationAcceptedCount;
                //ws.Cells[currentRow, column++].Value = item.TotalAppilationRejectedCount;
                ws.Cells[currentRow, column++].Value = item.TotalMediationCount;
            }
			

			ws.DeleteRow(importRow.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }

        result.Position = 0;
        return result;
    }

    public Stream SaveAsExcelGetSrvDeedReport(SrvDeedListReportSortFilter options)
    {
        var data = GetSrvDeedReport(options);
        CollectGroupedServices(options);

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.DEED_SWOT_REPORT));

        if (data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);
            var ws = excelPackage.Workbook.Worksheets[0];
            var importColumn = excelPackage.Workbook.Names["regionInsert"];
            var allSumAndCount = excelPackage.Workbook.Names["allSumAndCount"];
            //var groupInsert = excelPackage.Workbook.Names["groupInsert"]; 
            var serviceInsert = excelPackage.Workbook.Names["ServiceInsert"]; 


            int regionColumn = importColumn.Start.Column;
            int allSumAndCountColumn = allSumAndCount.Start.Column;
            //int groupInsertColumn = groupInsert.Start.Column;

            foreach (var item in data)
            {
             

                ws.Cells[importColumn.Start.Row, regionColumn].Value = item.Name;
                ws.Cells[importColumn.Start.Row, regionColumn, importColumn.Start.Row, regionColumn + 1].Merge = true;

                ws.Cells[allSumAndCount.Start.Row, allSumAndCountColumn++].Value = item.Count;
                ws.Cells[allSumAndCount.Start.Row, allSumAndCountColumn++].Value = item.Sum == null ? 0 : item.Sum;
                ws.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                regionColumn += 2;

            }


            int index = 1;
            var column = 1;
            // var Services = data.SelectMany(a => a.DeedGroupCollection.DeedGroup).SelectMany(g => g.ServiceNames.Select(a => a.ServiceNames)).Distinct().ToList();
            var fixedElemenets = CollectGroupedServices(options);

            var serviceInserRow = serviceInsert.Start.Row;

            foreach (var item in fixedElemenets)
            {
                ws.InsertRow(serviceInsert.Start.Row, 1, serviceInsert.Start.Row);
                foreach (var innerItem in item.Services)
                {
                    ws.InsertRow(serviceInsert.Start.Row, 1, serviceInsert.Start.Row);
                }

            }

            foreach (var item in fixedElemenets)
            {
                ws.Cells[serviceInserRow, column].Value = item.GroupName;
                ws.Cells[serviceInserRow, column, serviceInserRow, column + regionColumn - 1].Merge = true;
                ws.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                foreach (var innerItem in item.Services)
                {
                    serviceInserRow++;
                    int innerColumn = 1;
                    ws.Cells[serviceInserRow, innerColumn++].Value = index;
                    ws.Cells[serviceInserRow, innerColumn++].Value = innerItem.ServiceName;
                    ws.Cells[serviceInserRow, innerColumn++].Value = innerItem.TotalCount;
                    ws.Cells[serviceInserRow, innerColumn++].Value = innerItem.TotalSum;

                foreach (var InnerItemMain in data)
                {
                        var innerData = InnerItemMain.DeedGroupCollection.DeedGroup .Where(a => a.GroupName == item.GroupName && a.ServiceNames.Any(s => s.ServiceNames == innerItem.ServiceName));
                        if (innerData.Count() != 0)
                        {
                            ws.Cells[serviceInserRow, innerColumn++].Value = innerData.FirstOrDefault().ServiceNames.FirstOrDefault().Count;
                            ws.Cells[serviceInserRow, innerColumn++].Value = innerData.FirstOrDefault().ServiceNames.FirstOrDefault().Sum;
                        }
                        else
                        {
                            ws.Cells[serviceInserRow, innerColumn++].Value = 0;
                            ws.Cells[serviceInserRow, innerColumn++].Value = 0;
                        }
                    }

                index++;

                }


                serviceInserRow++;
            }

            ws.DeleteRow(serviceInsert.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }

        result.Position = 0;
        return result;
    }

    public Stream SaveAsExcelGetSrvFreeDeedReport(SrvDeedListReportSortFilter options)
    {

        var data = GetSrvFreeDeedReport(options);

        MemoryStream result = new MemoryStream();
        MemoryStream template = _storageService.GetStaticFile(StaticFileConst.ExcelTemplate.GetFileName(_cultureHelper.CurrentCulture.Code, StaticFileConst.Report.DEED_FREE_SWOT_REPORT));

        if (data != null)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(template);
            var ws = excelPackage.Workbook.Worksheets[0];
            var importColumn = excelPackage.Workbook.Names["regionInsert"];
            var allSumAndCount = excelPackage.Workbook.Names["allSumAndCount"];
            //var groupInsert = excelPackage.Workbook.Names["groupInsert"]; 
            var serviceInsert = excelPackage.Workbook.Names["ServiceInsert"];


            int regionColumn = importColumn.Start.Column;
            int allSumAndCountColumn = allSumAndCount.Start.Column;
            //int groupInsertColumn = groupInsert.Start.Column;

            foreach (SrvDeedDto item in data)
            {

                ws.Cells[importColumn.Start.Row, regionColumn].Value = item.Name;
                //ws.Cells[importColumn.Start.Row, regionColumn, importColumn.Start.Row, regionColumn + 1].Merge = true;

                ws.Cells[allSumAndCount.Start.Row, allSumAndCountColumn++].Value = item.Count;
                //ws.Cells[allSumAndCount.Start.Row, allSumAndCountColumn++].Value = item.Sum == null ? 0 : item.Sum;
                ws.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                regionColumn ++;

            }


            int index = 1;
            var column = 1;
            var fixedElemenets = CollectFreeGroupedServices(options, data);

            var serviceInserRow = serviceInsert.Start.Row;

            foreach (var item in fixedElemenets)
            {
                ws.InsertRow(serviceInsert.Start.Row, 1, serviceInsert.Start.Row);
                foreach (var innerItem in item.Services)
                {
                    ws.InsertRow(serviceInsert.Start.Row, 1, serviceInsert.Start.Row);
                }

            }

            foreach (var item in fixedElemenets)
            {
                ws.Cells[serviceInserRow, column].Value = item.GroupName == null ? string.Empty : item.GroupName;
                ws.Cells[serviceInserRow, column, serviceInserRow, column + regionColumn - 1].Merge = true;
                ws.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                foreach (var innerItem in item.Services)
                {
                    serviceInserRow++;
                    int innerColumn = 1;
                    ws.Cells[serviceInserRow, innerColumn++].Value = index;
                    ws.Cells[serviceInserRow, innerColumn++].Value = innerItem.ServiceName == null ? string.Empty : innerItem.ServiceName;
                    ws.Cells[serviceInserRow, innerColumn++].Value = innerItem.TotalCount;

                    foreach (var InnerItemMain in data)
                    {
                        var innerData = InnerItemMain.DeedGroup.Where(a => a.GroupName == item.GroupName && a.ServiceNames.Any(s => s.ServiceNames == innerItem.ServiceName));
                        if (innerData.Count() != 0)
                        {
                            var test = innerData.FirstOrDefault().ServiceNames.GroupBy(a => a.ServiceNames).Select(s => new { name = s.Key, Count = s.Sum(q => q.Count) }).ToList();
                            //var ba = innerData.FirstOrDefault().ServiceNames.GroupBy(a => a.ServiceNames).Select(s => new {Count = s.Sum(q=>q.Count)}).ToList();
                            var c  = test.FirstOrDefault(a => a.name == innerItem.ServiceName).Count;
                            ws.Cells[serviceInserRow, innerColumn++].Value = test.FirstOrDefault(a => a.name == innerItem.ServiceName).Count;
                            //ws.Cells[serviceInserRow, innerColumn++].Value = 0;
                            //ws.Cells[serviceInserRow, innerColumn++].Value = innerData.FirstOrDefault().ServiceNames.FirstOrDefault().Count;


                        }
                        else
                        {
                            
                            ws.Cells[serviceInserRow, innerColumn++].Value = 0;
                        }
                    }

                    index++;

                }


                serviceInserRow++;
            }

            ws.DeleteRow(serviceInsert.Start.Row);
            result = new MemoryStream(excelPackage.GetAsByteArray());
            excelPackage.Dispose();
        }

        result.Position = 0;
        return result;
    }
  

    public List<GroupedService> CollectGroupedServices(SrvDeedListReportSortFilter options)
    {
        var data = GetSrvDeedReport(options);
        var groups = new Dictionary<string, GroupedService>();

        foreach (var region in data)
        {
            if (region.DeedGroupCollection?.DeedGroup != null)
            {
                foreach (var group in region.DeedGroupCollection.DeedGroup)
                {
                    if (!groups.ContainsKey(group.GroupName))
                    {
                        groups[group.GroupName] = new GroupedService
                        {
                            GroupName = group.GroupName,
                            Services = new List<Service>()
                        };
                    }

                    foreach (var service in group.ServiceNames)
                    {
                        var existingService = groups[group.GroupName].Services
                            .FirstOrDefault(s => s.ServiceName == service.ServiceNames);

                        if (existingService == null)
                        {
                            existingService = new Service
                            {
                               
                                ServiceName = service.ServiceNames,
                                TotalCount = 0,
                                TotalSum = 0
                            };
                            groups[group.GroupName].Services.Add(existingService);
                        }

                        existingService.TotalCount += service.Count;
                        existingService.TotalSum += service.Sum;

                     //   var regionOrDistrictId = region.RegionId;
                     //   var dataDict = existingService.Regions;

                     //   //if (!dataDict.ContainsKey(regionOrDistrictId))
                     //  // {
                     //       dataDict[regionOrDistrictId.ToString()] = new RegionOrDistrictData();
                     ////   }

                     //   dataDict[regionOrDistrictId.ToString()].Count += service.Count;
                     //   dataDict[regionOrDistrictId.ToString()].Sum += service.Sum;
                    }
                }
            }
        }

        return groups.Values.ToList();
    }

    public List<GroupedService> CollectFreeGroupedServices(SrvDeedListReportSortFilter options, List<SrvDeedDto> data)
    {
        var groups = new Dictionary<string, GroupedService>();

        foreach (var region in data)
        {
            
                foreach (var group in region.DeedGroup)
                {
                    if (group.GroupName != null && !groups.ContainsKey(group.GroupName))
                    {
                        groups[group.GroupName] = new GroupedService
                        {
                            GroupName = group.GroupName,
                            Services = new List<Service>()
                        };
                    }

                    foreach (var service in group.ServiceNames)
                    {
                        if (group.GroupName == null) continue;
                        var existingService = groups[group.GroupName].Services
                            .FirstOrDefault(s => s.ServiceName == service.ServiceNames);

                        if (existingService == null)
                        {
                            existingService = new Service
                            {

                                ServiceName = service.ServiceNames,
                                TotalCount = 0,
                                TotalSum = 0
                            };
                            groups[group.GroupName].Services.Add(existingService);
                        }

                        existingService.TotalCount += service.Count;
                        existingService.TotalSum += service.Sum;

                        //   var regionOrDistrictId = region.RegionId;
                        //   var dataDict = existingService.Regions;

                        //   //if (!dataDict.ContainsKey(regionOrDistrictId))
                        //  // {
                        //       dataDict[regionOrDistrictId.ToString()] = new RegionOrDistrictData();
                        ////   }

                        //   dataDict[regionOrDistrictId.ToString()].Count += service.Count;
                        //   dataDict[regionOrDistrictId.ToString()].Sum += service.Sum;
                    }
                }
            
        }

        return groups.Values.ToList();
    }

    #endregion
}
public class EmployeeCountDataHelper
{
    public long? Id { get; set; }
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
    public int ContractTypeId { get; set; }
    public decimal EmployeeCount_2023 { get; set; }
    public decimal EmployeeCount_2024 { get; set; }
    public decimal EmployeeCount_2025 { get; set; }
    public decimal EmployeeCount_2026 { get; set; }
    public decimal TotalEmployeeCount { get; set; }
    public int Count_2023 { get; set; }
    public int Count_2024 { get; set; }
    public int Count_2025 { get; set; }
    public int Count_2026 { get; set; }
    public int TotalCount { get; set; }
}
public class EmployeeCountInMay
{
    public int RegionId { get; set; }
    public int DistrictId { get; set; }
    public decimal Sum { get; set; }
}

public class GroupedService
{

    public string GroupName { get; set; }
    public List<Service> Services { get; set; }
}

public class Service
{
    
    public string ServiceName { get; set; }
    public int TotalCount { get; set; }
    public decimal TotalSum { get; set; }
    public Dictionary<string, RegionOrDistrictData> Regions { get; set; }
    public Dictionary<string, RegionOrDistrictData> Districts { get; set; }
}

public class RegionOrDistrictData
{
    public int Count { get; set; }
    public decimal Sum { get; set; }
}