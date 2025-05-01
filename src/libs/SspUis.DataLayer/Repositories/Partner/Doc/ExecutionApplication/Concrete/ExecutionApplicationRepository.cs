using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ExecutionApplicationRepository :
    BaseEntityRepository<long,
        ExecutionApplication,
        CreateExecutionApplicationDlDto,
        UpdateExecutionApplicationDlDto,
        UpdateStatusExecutionApplicationDlDto>
    , IExecutionApplicationRepository
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICrudServices _crudServices;
        public ExecutionApplicationRepository(
            ICrudServices crudServices,
            IAuthService authService,
            IUnitOfWork unitOfWork)
            : base(crudServices)
        {
            this._authService = authService;
            this._crudServices = crudServices;
            this._unitOfWork = unitOfWork;
        }

        protected override void OnCreate(ExecutionApplication entity, CreateExecutionApplicationDlDto dto)
        {
            base.OnCreate(entity, dto);
            SetEntityProperties(entity, dto);
        }

        protected override void OnUpdate(ExecutionApplication entity, UpdateExecutionApplicationDlDto dto)
        {
            base.OnUpdate(entity, dto);
            SetEntityProperties(entity, dto);
        }

        protected override IQueryable<ExecutionApplication> InjectFilter(IQueryable<ExecutionApplication> query)
        {
            query = base.InjectFilter(query);
            if (_authService.User.IsAdmin)
            {
                return query;
            }
            if (_authService.User.OrganizationId != OrganizationIdConst.SSP)
                query = query.Where(a => a.OrganizationId == _authService.User.OrganizationId || a.RegionId == _authService.Organization.RegionId);

            return query;
        }
        private void SetEntityProperties<TDto>(ExecutionApplication entity, ExecutionApplicationDlDto<TDto> dto)
            where TDto : ExecutionApplicationDlDto<TDto>
        {
            var organizaiton = _unitOfWork.Context.Set<Organization>().FirstOrDefault(a => a.Id == _authService.User.OrganizationId);

            if (organizaiton != null)
            {
                entity.RegionId = organizaiton.RegionId;
                entity.DistrictId = organizaiton.DistrictId.Value;
                entity.OrganizationId = _authService.User.OrganizationId;
            }
        }

        protected override IQueryable<ExecutionApplication> ByIdQuery()
        {
            return AllAsQueryable
                .Include(a => a.Signs)
                .Include(c => c.Tables);
        }
    }
}
