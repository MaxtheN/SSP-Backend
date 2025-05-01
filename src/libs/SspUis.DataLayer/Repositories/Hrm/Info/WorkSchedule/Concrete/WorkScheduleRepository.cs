using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class WorkScheduleRepository : BaseEntityRepository<int, WorkSchedule, CreateWorkScheduleDlDto, UpdateWorkScheduleDlDto>, IWorkScheduleRepository
    {
        public WorkScheduleRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }

        protected override IQueryable<WorkSchedule> ByIdQuery()
            => AllAsQueryable
            .Include(x => x.DayHours)
            .Include(x => x.WorkHours);
    }
}
