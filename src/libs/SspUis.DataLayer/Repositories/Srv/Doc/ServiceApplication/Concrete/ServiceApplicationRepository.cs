using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.Repositories
{
    public class ServiceApplicationRepository
        : BaseApplicationRepository<ServiceApplication, CreateServiceApplicationDlDto, UpdateServiceApplicationDlDto, UpdateStatusServiceApplicationDlDto>,
        IServiceApplicationRepository
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        public ServiceApplicationRepository(
            ICrudServices crudServices,
            IAuthService authService,
            IUnitOfWork unitOfWork)
            : base(crudServices, authService, unitOfWork)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        protected override void OnCreate(ServiceApplication entity, CreateServiceApplicationDlDto dto)
        {

            //base.OnCreate(entity, dto);
            entity.Application.ContractorId = _authService.Contractor.Id;
            var region = _unitOfWork.Context
                .Set<Region>()
                .FirstOrDefault(x => x.Id == dto.RegionId);

            if (dto.ToRegionalOffice)
            {
                 var Organisation = _unitOfWork.Context.Set<Organization>().FirstOrDefault(f=>f.RegionId == region.Id && f.OrganizationGroupId == 3);
                 entity.OrganisationId = Organisation?.Id;
                 entity.ToRegionalOffice = true;
 
            }

            var district = _unitOfWork.Context
                            .Set<District>()
                            .FirstOrDefault(x => x.Id == (dto.DistrictId ?? _authService.Contractor.DistrictId));

         
            entity.Application.RegionName = region.FullName;
            entity.Application.DistrictName = district.FullName;
            entity.Application.RegionId = region.Id;
            entity.Application.DistrictId = district?.Id ?? _authService.Contractor.DistrictId;
            entity.RegionId = region.Id;
            entity.DistrictId = dto.DistrictId;

            if (dto.IsFree)
            {
                foreach (var group in entity.Groups)
                {
                    foreach (var table in group.Tables)
                    {
                        if (!table.OfferServiceText.IsNullOrEmpty())
                            continue;
                        table.IsCompleted = false;
                    }
                }
            }
            else
            {
                var srvPrice = PaidServicesQuery();
                foreach (var group in entity.Groups)
                {
                    foreach (var table in group.Tables)
                    {
                        if (!table.OfferServiceText.IsNullOrEmpty())
                            continue;

                        var servicePrice = srvPrice.FirstOrDefault(x => x.NeedChamberServiceId == table.NeedChamberServiceId);
                        if (servicePrice != null)
                        {
                            table.ServicePriceId = servicePrice.Owner.OwnerId;
                            table.ServicePriceTableId = servicePrice.Id;
                        }
                        else
                        {
                            AddError("Бу хизмат мавжуд емас !");
                            return;
                        }
                    }
                }
            }
        }
        private IQueryable<ServicePriceTable> PaidServicesQuery()
        {
            return _unitOfWork.Context.Set<ServicePriceTable>()
                .Include(x => x.Owner)
                .OrderByDescending(x => x.OwnerId);
        }
        protected override IQueryable<ServiceApplication> InjectFilter(IQueryable<ServiceApplication> query)
        {
            //if(_authService.Contractor!=null)
            //    return query.
            query = query.Where(x => x.Application.StatusId != StatusIdConst.DELETED);

            query = _authService.Contractor != null
                ? query.Where(a => a.Application.ContractorId == _authService.Contractor.Id)
                : query.Where(a => _authService.User.OrganizationId == OrganizationIdConst.SSP ? true : _authService.Organization.RegionId == a.Application.RegionId);

            return query;
        }
        protected override IQueryable<ServiceApplication> ByIdQuery()
        {
            return AllAsQueryable
                .Include(m => m.Application)
                .Include(m => m.Application.Contractor)
                .Include(m => m.Application.Region)
                .Include(m => m.Application.District)
                .Include(i => i.Groups)
                .ThenInclude(i => i.Tables);
        }
    }
}