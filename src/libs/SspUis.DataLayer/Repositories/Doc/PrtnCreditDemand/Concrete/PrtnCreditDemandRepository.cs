using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class PrtnCreditDemandRepository : BaseEntityRepository<long, PrtnCreditDemand, CreatePrtnCreditDemandDlDto, UpdatePrtnCreditDemandDlDto, UpdateStatusPrtnCreditDemandDlDto>, IPrtnCreditDemandRepository
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public PrtnCreditDemandRepository(ICrudServices crudServices, IAuthService authService, IUnitOfWork unitOfWork)
            : base(crudServices)
        {
            _authService = authService;
            this._unitOfWork = unitOfWork;
        }
        protected override void OnCreate(PrtnCreditDemand entity, CreatePrtnCreditDemandDlDto dto)
        {
            SetEntityProperties(entity, dto);
        }

        protected override void OnUpdate(PrtnCreditDemand entity, UpdatePrtnCreditDemandDlDto dto)
        {
            SetEntityProperties(entity, dto);
        }

        private void SetEntityProperties<TDto>(PrtnCreditDemand entity, PrtnCreditDemandDlDto<TDto> dto)
            where TDto : PrtnCreditDemandDlDto<TDto>
        {
            var certificate = _unitOfWork.Context.Set<PrtnCertificate>()
                .Include(c => c.PrtnContract)
                .ThenInclude(c => c.Application)
                .ThenInclude(c => c.PrtnApplication)
                .FirstOrDefault(a => a.ContractorId == _authService.Contractor.Id
                    && a.StatusId == StatusIdConst.FORMED);
            entity.ContractorId = certificate.ContractorId;
            entity.CertificateId = certificate.Id;
            if (certificate.PrtnContract.Application.PrtnApplication.ChooseLocation)
            {
                entity.RegionId = (int)certificate.PrtnContract.Application.PrtnApplication.ChoosedRegionId;
                entity.DistrictId = (int)certificate.PrtnContract.Application.PrtnApplication.ChoosedDistrictId;
            }
            else
            {
                entity.RegionId = certificate.PrtnContract.Application.RegionId;
                entity.DistrictId = certificate.PrtnContract.Application.RegionId;
            }
            entity.ApplicationId = certificate.PrtnContract.Application.PrtnApplication.Id;
        }

        protected override IQueryable<PrtnCreditDemand> InjectFilter(IQueryable<PrtnCreditDemand> query)
        {
            query = query.Where(a => a.StatusId != StatusIdConst.DELETED);

            if (_authService.Contractor != null)
                query = query.Where(a => a.ContractorId == _authService.Contractor.Id);

            return query;
        }
    }
}
