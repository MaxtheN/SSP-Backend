using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using System.Linq;

namespace SspUis.DataLayer.Repositories;

public class JoinAntiCorruptionApplicationRepository
    : BaseApplicationRepository
        <JoinAntiCorruptionApplication,
        CreateJoinAntiCorruptionApplicationDlDto,
        UpdateJoinAntiCorruptionApplicationDlDto,
        UpdateStatusJoinAntiCorruptionApplicationDlDto>,
    IJoinAntiCorruptionApplicationRepository
{
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    public JoinAntiCorruptionApplicationRepository(ICrudServices crudServices, IAuthService authService, IUnitOfWork unitOfWork) 
        : base(crudServices, authService, unitOfWork)
    {
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    protected override void OnCreate(JoinAntiCorruptionApplication entity, CreateJoinAntiCorruptionApplicationDlDto dto)
    {
        base.OnCreate(entity, dto);
    }

    protected override void OnUpdate(JoinAntiCorruptionApplication entity, UpdateJoinAntiCorruptionApplicationDlDto dto)
    {
        base.OnUpdate(entity, dto);
    }

    protected override IQueryable<JoinAntiCorruptionApplication> InjectFilter(IQueryable<JoinAntiCorruptionApplication> query)
    {
        query = base.InjectFilter(query);

        if (_authService.Contractor == null)
        {
            query = query.Where(a =>
            new[]
            {
                StatusIdConst.ACCEPTED,
                StatusIdConst.CANCELED ,
                StatusIdConst.SENT,
                StatusIdConst.REJECTED,
                StatusIdConst.ACCEPTED_SSP,
                StatusIdConst.ACCEPTED_OMBUDSMAN
            }.Contains(a.Application.StatusId));
        }

        return query;
    }

    protected override IQueryable<JoinAntiCorruptionApplication> ByIdQuery()
    {
        return AllAsQueryable
            .Include(j => j.Files)
            .Include(j => j.Employees)
            .Include(j => j.Tables)
            .Include(j => j.Participates)
            .Include(j => j.Application)
            .Include(j => j.Application.Contractor)
            .Include(j => j.Application.Region)
            .Include(j => j.Application.District);
    }

    public void UpdateStep(UpdateStepDlDto dto)
    {
        var app = Context.Set<Application>()
                .FirstOrDefault(x => x.Id == dto.Id && x.StatusId != StatusIdConst.DELETED);

        if (app == null) AddError("Such a user does not exist !");

        if (IsValid)
        {
            dto.UpdateEntity(app);
            Context.Entry(app).State = EntityState.Modified;
        }
    }
}
