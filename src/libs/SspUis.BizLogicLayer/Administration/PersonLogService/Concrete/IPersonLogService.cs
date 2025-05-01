using SspUis.BizLogicLayer.Administration.PersonLogService;
using SspUis.DataLayer.EfClasses.Public.Hl;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public interface IPersonLogService : IBaseEntityService<int,PersonLog,
                    PersonLogListDto,
                    PersonLogDto,
                    CreatePersonLogDlDto,
                    UpdatePersonLogDlDto,
                    PersonLogSortFilterOption>
{
    HaveId<int> Create(CreatePersonLogDlDto dto);
    PersonLogListDto Get();
    PersonLogListDto GetByEmployeeId(int employeeId);
    PagedResult<PersonLogListDto> GetList(PersonLogSortFilterOption dto);
}
