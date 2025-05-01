using SspUis.BizLogicLayer.Hrm;
using SspUis.BizLogicLayer.Models;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Integration.MSPD.GSP;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public interface IEmployeeService : IStatusGeneric
{
    PagedResult<EmployeeListDto> GetList(EmployeeSortFilterPageOptions dto);
    IQueryable<EmployeeListDto> GetListMethod(EmployeeSortFilterPageOptions dto);
    EmployeeDto Get();
    EmployeeDto Get(int id);
    PagedSelectList<int> AsSelectList(EmployeeSortFilterPageOptions options);
    HaveId<int> Create(CreateEmployeeDto dto);
    Task Update(UpdateEmployeeDlDto dto, bool isJob = false);
    void Delete(int id);
    Task<EmployeeDto> GetByPassportData(GSPPersonInfoRequestDto dto);
    public Task<EmployeeRelativeDto> GetRelativesByPassportData(GSPPersonInfoRequestDto dto);
    HaveId<int> Create(CreateEmployeeByUserDlDto dto);
    Task<byte[]> DownloadCV(string pinfl, string? lang);
}
