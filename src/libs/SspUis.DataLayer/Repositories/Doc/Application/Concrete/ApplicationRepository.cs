using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Memship;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ApplicationRepository : BaseEntityRepository<long, Application, CreateApplicationDlDto, UpdateApplicationDlDto, UpdateStatusApplicationDlDto>, IApplicationRepository
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemshipContractRepository _memshipContractRepository;

        public ApplicationRepository(
            ICrudServices crudServices,
            IAuthService authService,
            IUnitOfWork unitOfWork,
            IMemshipContractRepository memshipContractRepository)
            : base(crudServices)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
            _memshipContractRepository = memshipContractRepository;
        }

        public Application SetSend(long id, long? externalId = null)
        {
            Application val = ById(id);
            if (base.HasErrors)
            {
                return null;
            }
            if (val.PrtnApplication.IsSent == false)
            {
                val.PrtnApplication.IsSent = true;
                if (externalId.HasValue)
                    val.PrtnApplication.MahallaExternalId = externalId.Value;
            }
            else
                AddError($"Ariza jo'natilgan {id}");

            base.Context.Entry(val).State = EntityState.Modified;
            return val;
        }

        protected override void OnCreate(Application entity, CreateApplicationDlDto dto)
        {
            SetEntityProperties(entity, dto);
        }

        protected override void OnUpdate(Application entity, UpdateApplicationDlDto dto)
        {
            SetEntityProperties(entity, dto);
        }

        private void SetEntityProperties<TDto>(Application entity, ApplicationDlDto<TDto> dto)
            where TDto : ApplicationDlDto<TDto>
        {
            var region = _unitOfWork.RegionRepository.ById(_authService.Contractor.RegionId);
            var district = _unitOfWork.DistrictRepository.ById(_authService.Contractor.DistrictId);
            entity.DistrictId = _authService.Contractor.DistrictId;
            entity.RegionId = _authService.Contractor.RegionId;
            entity.DistrictName = district.FullName;
            entity.RegionName = region.FullName;
            entity.ContractorId = _authService.Contractor.Id;
            

            if (dto is IPrtnApplicationDlDto _dto)
            {
                var mfy = _unitOfWork.MfyRepository.ById(_dto.MfyId);
                entity.PrtnApplication.MfyName = mfy.FullName;
            }
            else if (dto is IClaimApplicationDlDto _dtoClaim)
            {

                entity.ClaimApplication.TotalAmount = _dtoClaim.MainDebt + _dtoClaim.CalculedPenalty + _dtoClaim.Penalty + _dtoClaim.Percent;
            };
        }

        protected override IQueryable<Application> ByIdQuery()
            => AllAsQueryable
                .Include(a => a.PrtnApplication).ThenInclude(a => a.Graphs)
                .Include(a => a.MemshipApplication)
                .Include(a => a.ClaimApplication).ThenInclude(a => a.Tables)
                .Include(a => a.ClaimApplication).ThenInclude(a => a.MemshipContract)
                .Include(a => a.ClaimApplication).ThenInclude(a => a.MemshipCertificate)
                .Include(a => a.ClaimApplication).ThenInclude(a => a.Files)
                .Include(a => a.DualApplication).ThenInclude(a => a.Tables)
                .Include(a => a.StateAssetApplication)
            ;

        protected override IQueryable<Application> InjectFilter(IQueryable<Application> query)
        {
            query = query.Where(a => a.StatusId != StatusIdConst.DELETED);

            if (_authService.Contractor != null)
                return query.Where(a => a.ContractorId == _authService.Contractor.Id);
            else
                query = query.Where(a => (a.ApplicationTypeId == ApplicationTypeIdConst.CLAIM) ?  a.StatusId != StatusIdConst.CREATED && a.StatusId != StatusIdConst.MODIFIED : true);

            if (_authService.HasPermission(ModuleCode.ApplicationViewAll) || _authService.UserName == "mahalla")
                return query;

            if (_authService.HasPermission(ModuleCode.ApplicationViewByRegion))
                return query.Where(a => (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId.Value : a.RegionId) == _authService.Organization.RegionId);

            query =
                query.Where(a => a.ApplicationTypeId != ApplicationTypeIdConst.PARTNER ||
                                (a.ApplicationTypeId == ApplicationTypeIdConst.PARTNER &&
                                (a.PrtnApplication.PrtnContractTypeId == PrtnContractTypeIdConst._50_100 &&
                                PrtnContractTypeIdConst.GetBySignOrganizationTypeId(_authService.Organization.SignOrganizationTypeId ?? 0) == PrtnContractTypeIdConst._50_100 &&
                                (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId.Value : a.RegionId) == _authService.Organization.RegionId &&
                                (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedDistrictId.Value : a.DistrictId) == _authService.Organization.DistrictId
                                ) ||
                                (a.PrtnApplication.PrtnContractTypeId == PrtnContractTypeIdConst._101_200 &&
                                PrtnContractTypeIdConst.GetBySignOrganizationTypeId(_authService.Organization.SignOrganizationTypeId ?? 0) == PrtnContractTypeIdConst._101_200 &&
                                (a.PrtnApplication.ChooseLocation ? a.PrtnApplication.ChoosedRegionId.Value : a.RegionId) == _authService.Organization.RegionId
                                ) ||
                                (a.PrtnApplication.PrtnContractTypeId == PrtnContractTypeIdConst._201__ &&
                                PrtnContractTypeIdConst.GetBySignOrganizationTypeId(_authService.Organization.SignOrganizationTypeId ?? 0) == PrtnContractTypeIdConst._201__
                                )
                                )
                            );

            return query;
        }
    }
}
