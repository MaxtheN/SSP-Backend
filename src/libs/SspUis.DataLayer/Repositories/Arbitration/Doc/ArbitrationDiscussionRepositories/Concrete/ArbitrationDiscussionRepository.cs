using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class ArbitrationDiscussionRepository
    : BaseEntityRepository<long, ArbitrationDiscussion,
        CreateArbitrationDiscussionDlDto,
        UpdateArbitrationDiscussionDlDto,
        UpdateStatusArbitrationDiscussionDlDto>,
    IArbitrationDiscussionRepository
{
    private readonly ICrudServices _crudServices;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public ArbitrationDiscussionRepository(
        ICrudServices crudServices,
        IAuthService authService,
        IUnitOfWork unitOfWork) : base(crudServices)
    {
        this._crudServices = crudServices;
        this._authService = authService;
        this._unitOfWork = unitOfWork;
    }

    protected override void OnUpdate(ArbitrationDiscussion entity, UpdateArbitrationDiscussionDlDto dto)
    {
        base.OnUpdate(entity, dto);
        SetEntityProperties(dto, entity);
    }
    protected override void OnCreate(ArbitrationDiscussion entity, CreateArbitrationDiscussionDlDto dto)
    {
        base.OnCreate(entity, dto);
        SetEntityProperties(dto, entity);
    }

    private void SetEntityProperties<TDto>(ArbitrationDiscussionDlDto<TDto> dto, ArbitrationDiscussion entity)
          where TDto : ArbitrationDiscussionDlDto<TDto>
    {
        var m = _crudServices.Context.Set<ArbitrationCourtApplication>()
            .Include(x => x.Application)
            .FirstOrDefault(m => m.Id == dto.ArbitrationCourtApplicationId);

        entity.ContractorId = m.Application.ContractorId;
        entity.ResponsibleContractorId = m.ResponsibleContractorId;
    }

    protected override IQueryable<ArbitrationDiscussion> InjectFilter(IQueryable<ArbitrationDiscussion> query)
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
    protected override IQueryable<ArbitrationDiscussion> ByIdQuery()
    {
        return AllAsQueryable.Include(x => x.Signs).Include(x => x.Files);
    }
}
