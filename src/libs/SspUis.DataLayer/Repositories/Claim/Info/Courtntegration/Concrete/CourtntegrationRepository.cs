using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Claim;
using StatusGeneric;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class CourtntegrationRepository : BaseEntityRepository<int, Courtntegration, CreateCourtntegrationDlDto, UpdateCourtntegrationDlDto>, ICourtntegrationRepository
    {
        private readonly IAuthService _authService;
        public CourtntegrationRepository(ICrudServices crudServices,
            IAuthService authService)
            : base(crudServices)
        {
            _authService = authService;
        }

    }
}
