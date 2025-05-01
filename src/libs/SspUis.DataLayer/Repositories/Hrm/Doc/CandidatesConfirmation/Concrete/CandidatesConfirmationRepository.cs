using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class CandidatesConfirmationRepository : BaseEntityRepository
        <long,
         CandidatesConfirmation,
         CreateCandidatesConfirmationDlDto,
         UpdateCandidatesConfirmationDlDto,
         UpdateStatusCandidatesConfirmationDlDto>
        , ICandidatesConfirmationRepository
    {
        private readonly IAuthService _authService;
        public CandidatesConfirmationRepository(
            ICrudServices crudServices,
            IAuthService authService)
            : base(crudServices)
        {
            this._authService = authService;
        }

        protected override void OnCreate(CandidatesConfirmation entity, CreateCandidatesConfirmationDlDto dto)
        {
            base.OnCreate(entity, dto);
            if (_authService.HasPermission(ModuleCode.AllCandidatesConfirmationCreate))
            {
                entity.OrganizationId = dto.OrganizationId.Value;
            }
            else
                entity.OrganizationId = _authService.User.OrganizationId;
        }

        protected override IQueryable<CandidatesConfirmation> InjectFilter(IQueryable<CandidatesConfirmation> query)
        {
            if (_authService.HasPermission(ModuleCode.CandidatesConfirmationViewHeader) && !_authService.User.IsAdmin)
                query = query.Where(a => a.StatusId == StatusIdConst.SENT_FOR_REVIEW);

            if (_authService.User.IsAdmin)
            {
                return query.Where(a => a.StatusId != StatusIdConst.DELETED);
            }
            else if (_authService.User != null)
            {
                return query = query.Where(x => x.OrganizationId == _authService.User.OrganizationId && x.StatusId != StatusIdConst.DELETED);
            }
            else
                return query;
        }

        protected override IQueryable<CandidatesConfirmation> ByIdQuery()
            => AllAsQueryable.Include(x => x.Tables).ThenInclude(x => x.Files);
    }
}