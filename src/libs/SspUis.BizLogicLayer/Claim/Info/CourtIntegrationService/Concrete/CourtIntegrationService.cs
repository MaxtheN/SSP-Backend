using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Claim.Info.CourtIntegrationService
{
    internal class CourtIntegrationService : StatusGenericHandler, ICourtIntegrationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICourtntegrationRepository _repository;
        private readonly IAuthService _authService;

        public CourtIntegrationService(IUnitOfWork unitOfWork, ICourtntegrationRepository repository, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _authService = authService;
        }

        public IReadOnlyList<ErrorGeneric> Errors => throw new NotImplementedException();

        public bool IsValid => throw new NotImplementedException();

        public bool HasErrors => throw new NotImplementedException();

        public string Message { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IStatusGeneric CombineStatuses(IStatusGeneric status)
        {
            throw new NotImplementedException();
        }

        public HaveId<int> Create(CreateCourtntegrationDlDto dto)
        {
            var entity = _repository.Create(dto, ent => Validation(dto, ent));
            return null;
            
        }

        public string GetAllErrors(string separator = null)
        {
            throw new NotImplementedException();
        }

        private void Validation<TDto>(CourtntegrationDlDto<TDto> dto, Courtntegration entity)
            where TDto : CourtntegrationDlDto<TDto>
        {
            var query = _repository.AllAsQueryable;

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

        }
    }
}

