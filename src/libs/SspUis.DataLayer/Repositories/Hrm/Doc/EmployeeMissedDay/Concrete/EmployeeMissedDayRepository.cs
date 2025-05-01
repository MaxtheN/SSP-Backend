using System;
using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class EmployeeMissedDayRepository : BaseEntityRepository<long, EmployeeMissedDay, CreateEmployeeMissedDayDlDto, UpdateEmployeeMissedDayDlDto>, IEmployeeMissedDayRepository
{
    private readonly IAuthService _authService;

    public EmployeeMissedDayRepository(
        ICrudServices crudServices,
        IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
    }

    public EmployeeMissedDay Create(CreateEmployeeMissedDayDlDto createEmployeeMissedDayDlDto, int organizationId)
    {
        var entity = base.Create(createEmployeeMissedDayDlDto);
        if (entity == null) return null;

        entity.OrganizationId = organizationId;
        return entity;
    }

    public override EmployeeMissedDay Update(UpdateEmployeeMissedDayDlDto updateDto, Action<EmployeeMissedDay> validation = null)
    {
        var entity = base.Update(updateDto);
        if (entity == null) return null;

        Context.Entry(entity).State = EntityState.Modified;

        return entity;
    }

    public EmployeeMissedDay UpdateStatus(UpdateStatusEmployeeMissedDayDlDto dto)
    {
        var entity = ById(dto.Id);
        //if (IsValid)
        //    UpdateStatusValidate(entity, dto);

        if (HasErrors)
            return null;

        dto.UpdateEntity(entity);
        Context.Entry(entity).State = EntityState.Modified;

        return entity;
    }

    protected override IQueryable<EmployeeMissedDay> ByIdQuery() =>
        AllAsQueryable.Include(a => a.Tables);

    protected override void CreateValidate(EmployeeMissedDay entity, CreateEmployeeMissedDayDlDto dto)
    {
        Validate(null, dto);
    }

    protected override void UpdateValidate(EmployeeMissedDay entity, UpdateEmployeeMissedDayDlDto dto)
    {
        Validate(entity, dto);
    }

    private void Validate<TDto>(EmployeeMissedDay entity, EmployeeMissedDayDlDto<TDto> dto)
        where TDto : EmployeeMissedDayDlDto<TDto>
    {
        var query = DbSet.AsQueryable();

        if (entity != null)
            query = query.Where(a => a.Id != entity.Id);
    }

    protected override IQueryable<EmployeeMissedDay> InjectFilter(IQueryable<EmployeeMissedDay> query)
        => query.Where(a => a.StatusId != StatusIdConst.DELETED && a.OrganizationId == _authService.User.OrganizationId);
}
