using DocumentFormat.OpenXml.Spreadsheet;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using System.Linq;

namespace SspUis.DataLayer.Repositories
{
    public class MonoApplicationRepository
        : BaseApplicationRepository<MonoApplication, CreateMonoApplicationDlDto, UpdateMonoApplicationDlDto, UpdateStatusMonoApplicationDlDto>,
        IMonoApplicationRepository
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        public MonoApplicationRepository(ICrudServices crudServices, IAuthService authService, IUnitOfWork unitOfWork)
            : base(crudServices, authService, unitOfWork)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
        }
        protected override void OnCreate(MonoApplication entity, CreateMonoApplicationDlDto dto)
        {
            base.OnCreate(entity, dto);
            SetEntityProperties(entity, dto);
        }

        protected override void OnUpdate(MonoApplication entity, UpdateMonoApplicationDlDto dto)
        {
            base.OnUpdate(entity, dto);
            SetEntityProperties(entity, dto);
        }

        protected override IQueryable<MonoApplication> InjectFilter(IQueryable<MonoApplication> query)
        {
            query = base.InjectFilter(query);

            if (_authService.Contractor == null)
                query = query.Where(a => a.Application.StatusId != StatusIdConst.DELETED && a.Application.StatusId != StatusIdConst.MODIFIED);

            return query;
        }

        private void SetEntityProperties<TDto>(MonoApplication entity, MonoApplicationDlDto<TDto> dto)
            where TDto : MonoApplicationDlDto<TDto>
        {
            decimal total = 0;
            decimal sum = 0;
            foreach (var tables in dto.ItemTables)
            {
                total += tables.EducationItemCount;
                sum += tables.EducationItemAmount;

            }
			entity.TotalAmount = total;
			entity.TotalCost = sum;
			entity.CurrencyId = CurrencyIdConst.UZS;
            entity.ItemTables.FirstOrDefault().EducationItemCurrencyId = CurrencyIdConst.UZS;
        }

        protected override IQueryable<MonoApplication> ByIdQuery()
        {
            return AllAsQueryable
                .Include(c => c.StudentTables)
                .Include(c => c.ItemTables)
                .Include(c => c.Files)
                .Include(c => c.Application)
                .Include(c => c.Application.Contractor)
                .Include(c => c.Application.Region)
                .Include(c => c.Application.District);
        }
    }
}
