using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Hrm;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class QualificationCategoryRepository : BaseEntityRepository<int, QualificationCategory, CreateQualificationCategoryDlDto, UpdateQualificationCategoryDlDto>, IQualificationCategoryRepository
    {
        private readonly IAuthService _authService;

        public QualificationCategoryRepository(
            ICrudServices crudServices,
            IAuthService authService)
            : base(crudServices)
        {
            _authService = authService;
        }

        protected override IQueryable<QualificationCategory> ByIdQuery()
            => AllAsQueryable.Include(a => a.Translates);

    }
}
