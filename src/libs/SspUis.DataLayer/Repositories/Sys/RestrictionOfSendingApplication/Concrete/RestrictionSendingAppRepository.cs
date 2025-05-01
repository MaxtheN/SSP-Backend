using GenericServices;
using SspUis.Cor;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class RestrictionSendingAppRepository 
        : BaseEntityRepository<long, RestrictionOfSendingApplication, CreateRestrictionSendingAppDlDto, UpdateRestrictionSendingAppDlDto>, 
        IRestrictionSendingAppRepository
    {
        private readonly IAuthService _authService;
        public RestrictionSendingAppRepository(ICrudServices crudServices, IAuthService authService)
            : base(crudServices)
        {
            _authService = authService;
        }

        public void Passive(RestrictionOfSendingApplication entity)
        {
            entity.StateId = StateIdConst.PASSIVE;
        }

        protected override void CreateValidate(CreateRestrictionSendingAppDlDto dto)
        {
            Validate(null, dto);
        }

        protected override void UpdateValidate(RestrictionOfSendingApplication entity, UpdateRestrictionSendingAppDlDto dto)
        {
            Validate(entity, dto);
        }

        private void Validate<TDto>(RestrictionOfSendingApplication entity, RestrictionSendingAppDlDto<TDto> dto)
            where TDto : RestrictionSendingAppDlDto<TDto>
        {
            var query = DbSet.AsQueryable();

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);
        }

        protected override IQueryable<RestrictionOfSendingApplication> InjectFilter(IQueryable<RestrictionOfSendingApplication> query)
        {
            query = query.Where(a => a.StateId != StateIdConst.PASSIVE);

            if (_authService.Contractor != null)
                query = query.Where(a => a.AppId == AppIdConst.MY);

            return query;
        }
    }
}