using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class ArbitrationDelayRepository
    : BaseEntityRepository<long, ArbitrationDelay,
        CreateArbitrationDelayDlDto,
        UpdateArbitrationDelayDlDto,
        UpdateStatusArbitrationDelayDlDto>,
    IArbitrationDelayRepository
{
    private readonly ICrudServices _crudServices;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public ArbitrationDelayRepository(
        ICrudServices crudServices,
        IAuthService authService,
        IUnitOfWork unitOfWork) : base(crudServices)
    {
        this._crudServices = crudServices;
        this._authService = authService;
        this._unitOfWork = unitOfWork;
    }

    protected override void OnUpdate(ArbitrationDelay entity, UpdateArbitrationDelayDlDto dto)
    {
        base.OnUpdate(entity, dto);
        SetEntityProperties(dto, entity);
    }
    protected override void OnCreate(ArbitrationDelay entity, CreateArbitrationDelayDlDto dto)
    {
        base.OnCreate(entity, dto);
        SetEntityProperties(dto, entity);
    }

    private void SetEntityProperties<TDto>(ArbitrationDelayDlDto<TDto> dto, ArbitrationDelay entity)
          where TDto : ArbitrationDelayDlDto<TDto>
    {
        var m = _crudServices.Context.Set<ArbitrationCourtApplication>()
            .Include(x => x.Application)
            .FirstOrDefault(m => m.Id == dto.ArbitrationCourtApplicationId);

        entity.ContractorId = m.Application.ContractorId;
        entity.ResponsibleContractorId = m.ResponsibleContractorId;
    }

    protected override IQueryable<ArbitrationDelay> InjectFilter(IQueryable<ArbitrationDelay> query)
    {
        query = query.Include(m => m.ArbitrationCourtApplication)
            .ThenInclude(m => m.ArbitrationCourtResult);

        query = query.Where(a => a.StatusId != StatusIdConst.DELETED);

        if (_authService.Contractor != null)
            return query.Where(m => m.ContractorId == _authService.Contractor.Id);

        if (_unitOfWork.ArbitrationJudgeRepository.AllAsQueryable.Any(x => x.PersonId == _authService.User.PersonId))
        {
            return query.Where(doc => doc.Signs.Any(s => s.ArbitrationJudge.PersonId == _authService.User.PersonId));
        }

        if (_authService.User.OrganizationId == OrganizationIdConst.SSP
            || _authService.User.OrganizationId == OrganizationIdConst.COURT_OF_ARBITRATION)
            return query;
        else
            return query.Where(x => x.ArbitrationCourtApplication.OrganizationId == _authService.User.OrganizationId);
    }
    protected override IQueryable<ArbitrationDelay> ByIdQuery()
    {
        return AllAsQueryable.Include(x => x.Signs).Include(x => x.Files);
    }
}
