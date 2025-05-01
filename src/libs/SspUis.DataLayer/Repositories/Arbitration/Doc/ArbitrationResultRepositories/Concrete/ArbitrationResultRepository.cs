using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class ArbitrationResultRepository
    : BaseEntityRepository<long, ArbitrationResult,
        CreateArbitrationResultDlDto,
        UpdateArbitrationResultDlDto,
        UpdateStatusArbitrationResultDlDto>,
    IArbitrationResultRepository
{
    private readonly ICrudServices _crudServices;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public ArbitrationResultRepository(
        ICrudServices crudServices,
        IAuthService authService,
        IUnitOfWork unitOfWork) : base(crudServices)
    {
        _crudServices = crudServices;
        _authService = authService;
        this._unitOfWork = unitOfWork;
    }

    protected override void OnUpdate(ArbitrationResult entity, UpdateArbitrationResultDlDto dto)
    {
        base.OnUpdate(entity, dto);
        SetEntityProperties(dto, entity);
    }

    protected override void OnCreate(ArbitrationResult entity, CreateArbitrationResultDlDto dto)
    {
        base.OnCreate(entity, dto);
        SetEntityProperties(dto, entity);
    }

    private void SetEntityProperties<TDto>(ArbitrationResultDlDto<TDto> dto, ArbitrationResult entity)
          where TDto : ArbitrationResultDlDto<TDto>
    {
        var arbitrationApplication = _crudServices.Context.Set<ArbitrationCourtApplication>()
            .Include(x => x.Application)
            .FirstOrDefault(m => m.Id == dto.ArbitrationCourtApplicationId);

        entity.ContractorId = arbitrationApplication.Application.ContractorId;
        entity.ResponsibleContractorId = arbitrationApplication.ResponsibleContractorId;
        entity.Amount = arbitrationApplication.Amount;
    }

    protected override IQueryable<ArbitrationResult> InjectFilter(IQueryable<ArbitrationResult> query)
    {

        query = query
            .Include(x => x.Signs)
            .ThenInclude(x => x.ArbitrationJudge)
            .Where(a => a.StatusId != StatusIdConst.DELETED)
            .AsQueryable()
        ;

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
    protected override IQueryable<ArbitrationResult> ByIdQuery()
    {
        return AllAsQueryable.Include(m => m.ArbitrationCourtApplication)
            ;
    }


}
