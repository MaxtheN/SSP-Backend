using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ServiceContractRepository
        : BaseEntityRepository<long, ServiceContract, CreateServiceContractDlDto, UpdateServiceContractDlDto, UpdateStatusServiceContractDlDto>
        , IServiceContractRepository
    {
        private readonly IAuthService _authService;
        private readonly ICrudServices _crudService;
        private readonly IUnitOfWork _unitOfWork;
        public ServiceContractRepository(
            ICrudServices crudServices,
            IAuthService authService,
            IUnitOfWork unitOfWork)
            : base(crudServices)
        {
            _authService = authService;
            _crudService = crudServices;
            _unitOfWork = unitOfWork;
        }

        protected override void OnCreate(ServiceContract entity, CreateServiceContractDlDto dto)
        {
            base.OnCreate(entity, dto);

            var application = _unitOfWork.Context
                .Set<Application>()
                .FirstOrDefault(x => x.Id == entity.ApplicationId);

            if (application == null)
            {
                AddError("Ariza topilmadi");
                return;
            }

            var organization = _unitOfWork.OrganizationRepository.AllAsQueryable
                .FirstOrDefault(x =>
                x.RegionId == application.RegionId
                && (x.OrganizationGroupId == OrganizationGroupIdConst.SSP || x.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH));

            entity.OrganizationId = organization.Id;

            SetEntityProperties(dto, entity);
        }

        protected override void OnUpdate(ServiceContract entity, UpdateServiceContractDlDto dto)
        {
            base.OnUpdate(entity, dto);
            SetEntityProperties(dto, entity);
        }

        private void SetEntityProperties<TDto>(ServiceContractDlDto<TDto> dto, ServiceContract entity)
              where TDto : ServiceContractDlDto<TDto>
        {
            if (dto is UpdateServiceContractDlDto)
            {

            }
            else
            {
                if (HasErrors)
                    return;

                var application = _unitOfWork.Context.Set<Application>()
                    .Include(x => x.ServiceApplication).ThenInclude(x => x.Groups).ThenInclude(x => x.Tables)
                    .FirstOrDefault(x => x.Id == dto.ApplicationId && x.StatusId == StatusIdConst.ACCEPTED);

                var lastPrice = Context.Set<ServicePrice>()
                    .Include(x => x.Groups).ThenInclude(x => x.Tables)
                    .OrderByDescending(x => x.DocOn)
                    .ThenByDescending(x => x.Id)
                    .FirstOrDefault(x => x.StatusId != StatusIdConst.DELETED || x.StatusId != StatusIdConst.CANCELED);

                if (application is null || lastPrice is null)
                {
                    AddError("Ariza yoki oxirgi narx yo`q !");
                    return;
                }

                var fixedMinimumValue = Context.Set<FixedMinimumValue>()
                    .OrderByDescending(f => f.DateOn)
                    .ThenByDescending(f => f.Id)
                    .FirstOrDefault(a => a.MinimumValueTypeId == MinimumValueTypeIdConst.BRV);

                foreach (var group in entity.Groups)
                {
                    var priceGroup = lastPrice.Groups.SelectMany(x => x.Tables);
                    var applicationGroup = application.ServiceApplication.Groups.SelectMany(x => x.Tables);

                    foreach (var table in group.Tables)
                    {
                        var applicationTable = applicationGroup.FirstOrDefault(t => t.NeedChamberServiceId == table.NeedChamberServiceId);
                        var priceTable = priceGroup.FirstOrDefault(t => t.NeedChamberServiceId == table.NeedChamberServiceId);

                        table.ServicePriceId = lastPrice != null ? lastPrice.Id : null;
                        table.ServicePriceTableId = priceTable != null ? priceTable.Id : null;
                        table.OfferServiceText = applicationTable != null ? applicationTable.OfferServiceText : null;

                        if (table.RealCoef != 0)
                            table.Price = table.RealCoef * (fixedMinimumValue != null ? fixedMinimumValue.FixedValue : 1);
                    }
                }
            }
        }

        protected override IQueryable<ServiceContract> InjectFilter(IQueryable<ServiceContract> query)
        {
            query = query.Where(a => a.StatusId != StatusIdConst.DELETED);

            query = _authService.Contractor != null
                ? query.Where(a => a.ContractorId == _authService.Contractor.Id)
                : query.Where(a => a.OrganizationId == _authService.Organization.Id || _authService.Organization.Id == OrganizationIdConst.SSP);

            return query;
        }

        protected override IQueryable<ServiceContract> ByIdQuery()
        {
            return AllAsQueryable
                .Include(a => a.Signs)
                .Include(c => c.Application).ThenInclude(x => x.ServiceApplication)
                .Include(a => a.Groups).ThenInclude(x => x.Tables);
        }
    }
}