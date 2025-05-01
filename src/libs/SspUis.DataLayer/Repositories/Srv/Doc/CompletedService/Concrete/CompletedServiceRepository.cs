using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class CompletedServiceRepository
        : BaseEntityRepository<long, CompletedService, CreateCompletedServiceDlDto, UpdateCompletedServiceDlDto, UpdateStatusCompletedServiceDlDto>
        , ICompletedServiceRepository
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        public CompletedServiceRepository(
            ICrudServices crudServices,
            IAuthService authService,
            IUnitOfWork unitOfWork)
            : base(crudServices)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        protected override void OnCreate(CompletedService entity, CreateCompletedServiceDlDto dto)
        {
            entity.OrganizationId = _authService.User.OrganizationId;
            SetEntityProperties(entity, dto);
        }

        protected override void OnUpdate(CompletedService entity, UpdateCompletedServiceDlDto dto)
        {
            SetEntityProperties(entity, dto);
        }

        private void SetEntityProperties<TDto>(CompletedService entity, CompletedServiceDlDto<TDto> dto)
               where TDto : CompletedServiceDlDto<TDto>
        {

            var application = _unitOfWork.Context
                .Set<ServiceApplication>()
                .FirstOrDefault(x => x.Id == dto.ServiceApplicationId);

            var organization = _unitOfWork.OrganizationRepository.AllAsQueryable
                .FirstOrDefault(x =>
                x.RegionId == application.RegionId
                && (x.OrganizationGroupId == OrganizationGroupIdConst.SSP || x.OrganizationGroupId == OrganizationGroupIdConst.REGIONAL_BRANCH));

            entity.OrganizationId = organization.Id;
        }

        protected override IQueryable<CompletedService> InjectFilter(IQueryable<CompletedService> query)
        {
            if (_authService.Contractor != null)
            {
                query = query.Where(c => c.ContractorId == _authService.Contractor.Id);
            }

            return query.Where(x => x.OrganizationId == _authService.User.OrganizationId && x.StatusId != StatusIdConst.DELETED);
        }

        protected override IQueryable<CompletedService> ByIdQuery()
             => AllAsQueryable.Include(x => x.Contractor);
    }
}
