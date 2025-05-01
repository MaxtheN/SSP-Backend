using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.EfClasses.Report.Func;
using StatusGeneric;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.Info.EmployeeTurnstileReportServices;

public interface IEmployeeTurnstileReportService : IStatusGeneric
{
    Task<List<EmployeeTurnstileReportDto>> GetEmployeeTurnstileReportAsync(EmployeeTurnstileReportDtoFilter filter);
    List<EmployeeTurnstileTimeReportByIdDto> GetEmployeeTurnstileReportById(EmployeeTurnstileReportByIdDtoFilter filter);
    PagedResult<EmployeeTurnstileTimeReportDto> GetEmployeeTurnstileTimeReport(EmployeeTurnstileTimeReportDtoFilter filter);
    Task<Stream> SaveEmployeeTurnstileReportAsExcel(EmployeeTurnstileReportDtoFilter filter);
    Task<Stream> SaveEmployeeTurnstileReportById(EmployeeTurnstileReportByIdDtoFilter filter);
}
