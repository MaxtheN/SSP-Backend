using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Hrm;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm
{
    public class StaffingRepository : BaseEntityRepository<long, Staffing, CreateStaffingDlDto, UpdateStaffingDlDto, UpdateStatusStaffingDlDto>, IStaffingRepository
    {
        private readonly ICrudServices _crudServices;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public StaffingRepository(
            ICrudServices crudServices,
            IAuthService authService,
            IUnitOfWork unitOfWork
            ) : base(crudServices)
        {
            _crudServices = crudServices;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        protected override IQueryable<Staffing> ByIdQuery()
        {
            return AllAsQueryable.Include(a => a.Positions).ThenInclude(a => a.CalcKinds)
                                 .Include(a => a.IndicatorValues);
        }

        protected override void OnCreate(Staffing entity, CreateStaffingDlDto dto)
        {
            if (_authService.HasPermission(ModuleCode.AllStaffingCreate))
            {
                entity.OrganizationId = dto.OrganizationId.Value;
            }
            else
                entity.OrganizationId = _authService.Organization.Id;

            SetEntityProperties(entity, dto);
        }

        protected override void OnUpdate(Staffing entity, UpdateStaffingDlDto dto)
        {
            SetEntityProperties(entity, dto);
        }

        private void SetEntityProperties<TDto>(Staffing entity, StaffingDlDto<TDto> dto)
            where TDto : StaffingDlDto<TDto>
        {
            entity.DocSum = 0;
            foreach (var table in dto.Positions)
            {
                if (table.Fot != null)
                    entity.DocSum += table.Fot.Value;

                //if (table.PositionClassificationId == 0)
                //    table.PositionClassificationId = null;
            }
        }

        protected override IQueryable<Staffing> InjectFilter(IQueryable<Staffing> query) 
            => query.Where(a => a.OrganizationId == _authService.Organization.Id && a.StatusId != StatusIdConst.DELETED);
    }
}
