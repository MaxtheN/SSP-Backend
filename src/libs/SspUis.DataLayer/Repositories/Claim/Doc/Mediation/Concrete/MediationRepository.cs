using System;
using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories.Claim;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class MediationRepository : BaseEntityRepository<long, Mediation, CreateMediationDlDto, UpdateMediationDlDto, UpdateStatusMediationDlDto>, IMediationRepository
    {
        private readonly IAuthService _authService;
        public MediationRepository(ICrudServices crudServices, IAuthService authService)
            : base(crudServices)
        {
            this._authService = authService;
        }

        protected override void OnCreate(Mediation entity, CreateMediationDlDto dto)
        {
            entity.Id2 = Guid.NewGuid();
            entity.OrganizationId = _authService.User.OrganizationId;
        }

        protected override IQueryable<Mediation> InjectFilter(IQueryable<Mediation> query)
        {
            if (_authService.Contractor != null)
                query = query.Where(p => p.ContractorId == _authService.Contractor.Id);
            else
                query = query.Where(x => x.OrganizationId == _authService.User.OrganizationId && x.StatusId != StatusIdConst.DELETED);
            return query;
        }

        protected override IQueryable<Mediation> ByIdQuery()
        {
            return AllAsQueryable
                .Include(x => x.MediationPlan)
                .ThenInclude(x => x.Application);
        }
    }
}
