using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ServicePriceRepository :
    BaseEntityRepository<long, ServicePrice, CreateServicePriceDlDto, UpdateServicePriceDlDto, UpdateStatusServicePriceDlDto>
    , IServicePriceRepository
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        public ServicePriceRepository(
            ICrudServices crudServices,
            IAuthService authService,
            IUnitOfWork unitOfWork)
            : base(crudServices)
        {
            this._authService = authService;
            this._unitOfWork = unitOfWork;
        }

        public override ServicePrice UpdateStatus(UpdateStatusServicePriceDlDto updateStatusDto, Action<ServicePrice> validation = null)
        {
            return base.UpdateStatus(updateStatusDto, validation);
        }

        protected override void OnCreate(ServicePrice entity, CreateServicePriceDlDto dto)
        {
            entity.OrganizationId = _authService.User.OrganizationId;
            SetEntityProperties(entity, dto);
        }

        protected override void OnUpdate(ServicePrice entity, UpdateServicePriceDlDto dto)
        {
            SetEntityProperties(entity, dto);
        }

        private void SetEntityProperties<TDto>(ServicePrice entity, ServicePriceDlDtoo<TDto> dto)
               where TDto : ServicePriceDlDtoo<TDto>
        {
            entity.Groups.SelectMany(g => g.Tables)
                .Where(t => t.ConcreteCoef != 0)
                .ToList()
                .ForEach(t => t.IsConcrete = true);
        }

        protected override IQueryable<ServicePrice> InjectFilter(IQueryable<ServicePrice> query)
        {
            if(_authService.Contractor == null)
                query = query.Where(x => x.OrganizationId == _authService.User.OrganizationId && x.StatusId != StatusIdConst.DELETED);
            else
                query = query.Where(x => x.StatusId != StatusIdConst.DELETED);

            return query;
        }

        protected override IQueryable<ServicePrice> ByIdQuery()
             => AllAsQueryable.Include(x => x.Groups).ThenInclude(c => c.Tables);
    }
}