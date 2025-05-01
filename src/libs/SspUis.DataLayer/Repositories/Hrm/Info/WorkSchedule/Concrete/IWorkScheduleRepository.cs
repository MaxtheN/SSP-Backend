using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.DataLayer.Repositories
{
    public interface IWorkScheduleRepository : IBaseEntityRepository<int, WorkSchedule, CreateWorkScheduleDlDto, UpdateWorkScheduleDlDto>
    {
    }
}
