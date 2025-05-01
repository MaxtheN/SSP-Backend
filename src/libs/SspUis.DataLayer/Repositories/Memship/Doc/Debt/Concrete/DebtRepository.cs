using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class DebtRepository : BaseEntityRepository<long, Debt, CreateDebtDlDto, UpdateDebtDlDto, UpdateStatusDebtDlDto>, IDebtRepository
    {
        private readonly IAuthService _authService;
        public DebtRepository(ICrudServices crudServices, IAuthService authService)
            : base(crudServices)
        {
            this._authService = authService;
        }

        protected override void OnCreate(Debt entity, CreateDebtDlDto dto)
        {
            base.OnCreate(entity, dto);
            SetEntityProperties(entity);
        }

        protected override void OnUpdate(Debt entity, UpdateDebtDlDto dto)
        {
            base.OnUpdate(entity, dto);
            SetEntityProperties(entity);
        }
        protected override IQueryable<Debt> ByIdQuery()
        {
            return base.ByIdQuery()
                .Include(x => x.Tables)
                ;
        }
        private void SetEntityProperties(Debt entity)
        {
            entity.OrganizationId = _authService.User.OrganizationId;
        }
        protected override IQueryable<Debt> InjectFilter(IQueryable<Debt> query)
        {
            if (_authService.User.OrganizationId != OrganizationIdConst.SSP)
                return query.Where(x => x.OrganizationId == _authService.User.OrganizationId);

            return query.Where(x => x.StatusId != StatusIdConst.DELETED);
        }
    }
}
