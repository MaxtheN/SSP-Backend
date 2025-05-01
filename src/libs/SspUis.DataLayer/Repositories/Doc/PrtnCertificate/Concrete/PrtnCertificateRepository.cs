using SspUis.DataLayer.EfClasses;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.EF;
using SspUis.Core.Security;
using SspUis.Core;

namespace SspUis.DataLayer.Repositories
{
    public class PrtnCertificateRepository : BaseEntityRepository<long, PrtnCertificate, CreatePrtnCertificateDlDto, UpdatePrtnCertificateDlDto, UpdateStatusPrtnCertificateDlDto>, IPrtnCertificateRepository
    {
        private readonly IAuthService _authService;

        public PrtnCertificateRepository(ICrudServices crudServices, IAuthService authService)
            : base(crudServices)
        {
            _authService = authService;
        }

        protected override IQueryable<PrtnCertificate> InjectFilter(IQueryable<PrtnCertificate> query)
        {
            query = query.Where(a => a.StatusId != StatusIdConst.DELETED);

            if (_authService.Contractor != null)
                return query.Where(a => a.ContractorId == _authService.Contractor.Id);

            if (_authService.HasPermission(ModuleCode.PrtnCertificateViewAll) || _authService.UserName == "certificate")
                return query;

            if (_authService.HasPermission(ModuleCode.PrtnCertificateViewByRegion))
                return query.Where(a => (a.PrtnContract.Application.PrtnApplication.ChooseLocation
                    ? a.PrtnContract.Application.PrtnApplication.ChoosedRegionId 
                    : a.Contractor.RegionId) == _authService.Organization.RegionId);

            if (_authService.HasPermission(ModuleCode.PrtnCertificateViewByRegion))
                return query.Where(a => a.Organization.RegionId == _authService.Organization.RegionId);

            return query.Where(a => a.OrganizationId == _authService.Organization.Id || _authService.UserName == "certificate");
        }
    }
}
