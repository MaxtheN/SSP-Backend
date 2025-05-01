using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class DepartmentRepository : BaseEntityRepository<int, Department, CreateDepartmentDlDto, UpdateDepartmentDlDto>, IDepartmentRepository
{
    private readonly IAuthService _authService;
    public DepartmentRepository(ICrudServices crudServices, IAuthService authService)
        : base(crudServices)
    {
        _authService = authService;
    }
    protected override void OnCreate(
         Department entity,
         CreateDepartmentDlDto dto)
             => entity.OrganizationId = _authService.User.OrganizationId;

    protected override void CreateValidate(CreateDepartmentDlDto dto)
        => Validate(null, dto);

    protected override void UpdateValidate(
        Department entity,
        UpdateDepartmentDlDto dto)
            => Validate(entity, dto);

    private void Validate<TDto>(
        Department entity,
        DepartmentDlDto<TDto> dto)
        where TDto : DepartmentDlDto<TDto>
    {
        var query = InjectFilter(DbSet.AsQueryable());

        if (entity != null)
            query = query.Where(a => a.Id != entity.Id);

        //if (query.Any(department => department.Code == dto.Code))
        //    AddError($"Запись с этим кодом ({dto.Code}) уже существует.",
        //            nameof(dto.Code));


    }
    protected override IQueryable<Department> ByIdQuery()
          => AllAsQueryable.Include(x => x.Translates);
    protected override IQueryable<Department> InjectFilter(IQueryable<Department> query)
        => query.Where(a => a.StateId != StateIdConst.PASSIVE && a.OrganizationId == _authService.Organization.Id);
}
