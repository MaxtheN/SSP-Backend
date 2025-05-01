using System;
using System.Linq;
using GenericServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class AdditionalAgreementRepository
        : BaseEntityRepository<long, AdditionalAgreement, CreateAdditionalAgreementDlDto, UpdateAdditionalAgreementDlDto, UpdateStatusAdditionalAgreementDlDto>
        , IAdditionalAgreementRepository
    {
        private readonly IAuthService _authService;
        public AdditionalAgreementRepository(ICrudServices crudServices, IAuthService authService)
            : base(crudServices)
        {
            _authService = authService;
        }

        protected override void OnCreate(AdditionalAgreement entity, CreateAdditionalAgreementDlDto dto)
        {
            entity.OrganizationId = _authService.User.OrganizationId;
            entity.Id2 = Guid.NewGuid();
            SetEntityProperties(entity, dto);
            Validate(entity, dto);
        }

        protected override void OnUpdate(AdditionalAgreement entity, UpdateAdditionalAgreementDlDto dto)
        {
            SetEntityProperties(entity, dto);
            Validate(entity, dto);
        }
        private void SetEntityProperties<TDto>(AdditionalAgreement entity, AdditionalAgreementDlDto<TDto> dto)
               where TDto : AdditionalAgreementDlDto<TDto>
        {
        }
        protected override IQueryable<AdditionalAgreement> InjectFilter(IQueryable<AdditionalAgreement> query)
        {
            query = query.Where(x => x.StatusId != StatusIdConst.DELETED);
            if (_authService.Contractor != null)
                return query.Where(x => x.ContractorId == _authService.Contractor.Id);
            if (_authService.User != null)
                return query.Where(x => x.OrganizationId == _authService.User.OrganizationId || _authService.Organization.Id == OrganizationIdConst.SSP);
            return query;
        }

        protected override IQueryable<AdditionalAgreement> ByIdQuery()
        {
            return AllAsQueryable;
        }

        //protected override void CreateValidate(CreateAdditionalAgreementDlDto dto)
        //{
        //    dto.OrganizationId = _authService.User.OrganizationId;
        //    Validate(null, dto);
        //}
        //protected override void UpdateValidate(AdditionalAgreement entity, UpdateAdditionalAgreementDlDto dto)
        //{
        //    Validate(entity, dto);
        //}
        private void Validate<TDto>(AdditionalAgreement entity, AdditionalAgreementDlDto<TDto> dto)
            where TDto : AdditionalAgreementDlDto<TDto>
        {
            var query = DbSet.AsQueryable();

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

            if (query.ByNumber(dto.DocNumber, isIncludePassive: true).Any())
                AddError($"Дополнительный договор с этим номер ({dto.DocNumber}) уже существует.", nameof(dto.DocNumber));
        }
    }
}
