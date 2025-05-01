using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class MemshipPaymentOrderRepository : BaseEntityRepository<long, MemshipPaymentOrder, CreateMemshipPaymentOrderDlDto, UpdateMemshipPaymentOrderDlDto,UpdateStatusMemshipPaymentOrderDlDto>, IMemshipPaymentOrderRepository
    {
        private readonly IAuthService _authService;
        public MemshipPaymentOrderRepository(ICrudServices crudServices, IAuthService authService)
            : base(crudServices)
        {
            this._authService = authService;
        }

        protected override void OnCreate(MemshipPaymentOrder entity, CreateMemshipPaymentOrderDlDto dto)
        {
            base.OnCreate(entity, dto);
            SetEntityProperties(entity);
        }

        protected override void OnUpdate(MemshipPaymentOrder entity, UpdateMemshipPaymentOrderDlDto dto)
        {
            base.OnUpdate(entity, dto);
            SetEntityProperties(entity);
        }

        private void SetEntityProperties(MemshipPaymentOrder entity)
        {
            entity.OrganizationId = _authService.User.OrganizationId;
            entity.CurrencyId = CurrencyIdConst.UZS;
        }
        protected override IQueryable<MemshipPaymentOrder> InjectFilter(IQueryable<MemshipPaymentOrder> query)
        {
            if (_authService.HasPermission(ModuleCode.ServicePaymentOrderViewAll))
                return query.Where(a => a.StatusId != StatusIdConst.DELETED);
            else if (_authService.User != null)
            {
                return query = query.Where(x => x.OrganizationId == _authService.User.OrganizationId && x.StatusId != StatusIdConst.DELETED);
            }
            else
            return query;
        }
        protected override IQueryable<MemshipPaymentOrder> ByIdQuery()
        {
            return AllAsQueryable.Include(a => a.Files);
        }
    }
}
