using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;
using System.Linq;

namespace SspUis.DataLayer.Repositories
{
    public class ClaimApplicationRepository
        : BaseApplicationRepository<ClaimApplication, CreateClaimApplicationDlDto, UpdateClaimApplicationDlDto, UpdateStatusClaimApplicationDlDto>,
        IClaimApplicationRepository
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        public ClaimApplicationRepository(ICrudServices crudServices, IAuthService authService, IUnitOfWork unitOfWork)
            : base(crudServices, authService, unitOfWork)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        protected override void OnCreate(ClaimApplication entity, CreateClaimApplicationDlDto dto)
        {
            base.OnCreate(entity, dto);
            SetEntityProperties(entity, dto);
        }

        protected override void OnUpdate(ClaimApplication entity, UpdateClaimApplicationDlDto dto)
        {
            base.OnUpdate(entity, dto);
            SetEntityProperties(entity, dto);
        }

        protected override IQueryable<ClaimApplication> InjectFilter(IQueryable<ClaimApplication> query)
        {
            query = base.InjectFilter(query);
            if (_authService.User.IsAdmin)
            {
                return query;
            }
            else if (_authService.Contractor == null)
            {
                query = query.Where(a => a.Application.StatusId != StatusIdConst.MODIFIED && a.Application.StatusId != StatusIdConst.CREATED);

                if (_authService.User.OrganizationId != OrganizationIdConst.SSP)
                    query = query.Where(a => a.OrganizationId.Value == _authService.User.OrganizationId || a.Application.RegionId == _authService.Organization.RegionId);
            }
            else
            {
                query = query.Where(x => x.Application.ContractorId == _authService.Contractor.Id);
            }

            return query;
        }

        public void UpdateEmployeeAttachment(UpdateEmployeeAttechmentDlDto dto)
        {
            var entity = ById(dto.Id);

            if (entity == null)
                AddError("Such a user does not exist !");

            if (IsValid)
            {
                dto.UpdateEntity(entity);
                Context.Entry(entity).State = EntityState.Modified;
            }
        }
        public void UpdateStep(UpdateStepDlDto dto)
        {
            var app = Context.Set<Application>()
                .FirstOrDefault(x => x.Id == dto.Id && x.StatusId != StatusIdConst.DELETED);

            if (app == null) AddError("Such a user does not exist !");

            if (IsValid)
            {
                dto.UpdateEntity(app);
                Context.Entry(app).State = EntityState.Modified;
            }
        }
        private void SetEntityProperties<TDto>(ClaimApplication entity, ClaimApplicationDlDto<TDto> dto)
            where TDto : ClaimApplicationDlDto<TDto>
        {
            dto.MainDebt = dto.MainDebt == null ? 0 : dto.MainDebt.Value;
            dto.CalculedPenalty = dto.CalculedPenalty == null ? 0 : dto.CalculedPenalty.Value;
            dto.Penalty = dto.Penalty == null ? 0 : dto.Penalty.Value;
            dto.Percent = dto.Percent == null ? 0 : dto.Percent.Value;
            dto.CurrentPrincipalInterest = dto.CurrentPrincipalInterest == null ? 0 : dto.CurrentPrincipalInterest.Value;
            dto.CurrentInterestRate = dto.CurrentInterestRate == null ? 0 : dto.CurrentInterestRate.Value;
            dto.OtherDebtRepayment = dto.OtherDebtRepayment == null ? 0 : dto.OtherDebtRepayment.Value;

            entity.TotalAmount =
                dto.MainDebt +
                dto.CalculedPenalty +
                dto.Penalty +
                dto.Percent +
                dto.CurrentPrincipalInterest +
                dto.CurrentInterestRate +
                dto.OtherDebtRepayment;
            entity.OrganizationId = dto.OrganizationId;
        }

        protected override IQueryable<ClaimApplication> ByIdQuery()
        {
            return AllAsQueryable
                .Include(c => c.MemshipContract)
                .Include(c => c.MemshipCertificate)
                .Include(c => c.Files)
                .Include(c => c.Tables)
                .Include(c => c.Application)
                .Include(c => c.Application.Contractor)
                .Include(c => c.Application.Region)
                .Include(c => c.Application.District);
        }
    }
}
