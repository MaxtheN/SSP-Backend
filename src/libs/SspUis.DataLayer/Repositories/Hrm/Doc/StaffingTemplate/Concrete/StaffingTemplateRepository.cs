using GenericServices;
using Microsoft.EntityFrameworkCore;
using WEBASE.EF;
using WEBASE.i18n;
using GenericServices.PublicButHidden;
using System.Runtime.CompilerServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.Core.Security;
using System.Linq;
using SspUis.Core;

namespace SspUis.DataLayer.Repositories.Hrm
{
    public class StaffingTemplateRepository : BaseEntityRepository<long, StaffingTemplate, CreateStaffingTemplateDlDto, UpdateStaffingTemplateDlDto, UpdateStatusStaffingTemplateDlDto>, IStaffingTemplateRepository
    {
        private readonly ICultureHelper _cultureHelper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;


        public StaffingTemplateRepository(ICrudServices crudServices,
            ICultureHelper cultureHelper,
            IUnitOfWork unitOfWork,
            IAuthService authService)
            : base(crudServices)
        {
            _cultureHelper = cultureHelper;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        protected override void OnCreate(StaffingTemplate entity, CreateStaffingTemplateDlDto dto)
        {
            if (_authService.HasPermission(ModuleCode.AllStaffingTemplateCreate))
            {
                entity.OrganizationId = dto.OrganizationId.Value;
            }
            else
                entity.OrganizationId = _authService.Organization.Id;

            SetEntityProperties(entity, dto);
        }

        protected override void OnUpdate(StaffingTemplate entity, UpdateStaffingTemplateDlDto dto)
        {
            SetEntityProperties(entity, dto);
        }

        private void SetEntityProperties<TDto>(StaffingTemplate entity, StaffingTemplateDlDto<TDto> dto)
           where TDto : StaffingTemplateDlDto<TDto>
        {
            foreach (var item in dto.Tables)
            {
                if (item.TariffScaleTypeId == TariffScaleTypeIdConst.BY_BASE_SALARY)
                {
                    item.TariffScaleId = null;
                    item.TariffScaleTableId = null;
                }
            }
        }

        protected override IQueryable<StaffingTemplate> ByIdQuery()
            => AllAsQueryable.Include(a => a.Tables);

        protected override IQueryable<StaffingTemplate> InjectFilter(IQueryable<StaffingTemplate> query)
           => query.Where(a => a.OrganizationId == _authService.Organization.Id && a.StatusId != StatusIdConst.DELETED);
    }
}
