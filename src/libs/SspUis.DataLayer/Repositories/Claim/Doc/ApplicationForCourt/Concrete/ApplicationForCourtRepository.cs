using System;
using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Claim;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ApplicationForCourtRepository : BaseEntityRepository<long, ApplicationForCourt, CreateApplicationForCourtDlDto, UpdateApplicationForCourtDlDto, UpdateStatusApplicationForCourtDlDto>, IApplicationForCourtRepository
    {
        private readonly IAuthService _authService;
        private readonly ICrudServices _crudServices;
        private readonly IUnitOfWork _unitOfWork;
        public ApplicationForCourtRepository(ICrudServices crudServices, IAuthService authService, IUnitOfWork unitOfWork)
            : base(crudServices)
        {
            this._authService = authService;
            _crudServices = crudServices;
            _unitOfWork = unitOfWork;
        }

        protected override void OnCreate(ApplicationForCourt entity, CreateApplicationForCourtDlDto dto)
        {
            entity.Id2 = Guid.NewGuid();
            entity.OrganizationId = _authService.User.OrganizationId;

            SetEntityProperties(entity);

        }

        protected override IQueryable<ApplicationForCourt> InjectFilter(IQueryable<ApplicationForCourt> query)
        {
            query = query.Where(x => x.StatusId != StatusIdConst.DELETED);

            query = _authService.Contractor != null
                ? query.Where(x => x.ContractorId == _authService.User.OrganizationId)
                : query.Where(x => x.OrganizationId == _authService.User.OrganizationId);

            return query;
        }

        public void UpdateStep(ApplicationForCourt entity, UpdateStepApplicationForCourtDlDto dto)
        {
            if (entity == null)
                AddError("Such a user does not exist !");

            if (IsValid)
            {
                dto.UpdateEntity(entity);
                Context.Entry(entity).State = EntityState.Modified;
            }
        }

        private void SetEntityProperties(ApplicationForCourt entity)
        {
            if (entity.MediationId.HasValue)
            {
                var mediation = _crudServices.Context.Set<Mediation>()
                .FirstOrDefault(m => m.Id == entity.MediationId);

                entity.ContractorId = mediation.ContractorId;
            }
            else if(entity.ApplicationId.HasValue)
            {
                var application = _unitOfWork.Context.Applications.FirstOrDefault(a => a.Id == entity.ApplicationId);

                entity.ContractorId = application.ContractorId.Value;
            }
            
        }

        protected override IQueryable<ApplicationForCourt> ByIdQuery()
        {
            return base.ByIdQuery()
                .Include(x => x.Files)
                .Include(x => x.Mediation)
                    .ThenInclude(x => x.MediationPlan)
                    .ThenInclude(x => x.Application)
                .AsSplitQuery();
        }
    }
}
