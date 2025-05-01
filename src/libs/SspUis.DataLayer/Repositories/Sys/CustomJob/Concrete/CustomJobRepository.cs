using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Linq;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Mmv;

public class CustomJobRepository : BaseEntityRepository<long, CustomJob, CreateCustomJobDlDto, UpdateCustomJobDlDto>, ICustomJobRepository
{
   

    public CustomJobRepository(ICrudServices crudServices,
        IUnitOfWork unitOfWork)
    : base(crudServices)
    {
    }

   
    public override CustomJob Create(CreateCustomJobDlDto createDto, Action<CustomJob> validation = null)
    {

        CreateValidate(createDto);
        if (HasErrors)
            return null;

        var entity = base.Create(createDto);
        
        if (entity == null)
            return null;
        SetEntityProperties(entity);

        return entity;
    }
    private void SetEntityProperties(CustomJob entity)
    {
       
    }

    public override CustomJob Update(UpdateCustomJobDlDto updateDto, Action<CustomJob> validation = null)
    {
        var entity = base.Update(updateDto);
        

        if (entity == null)
            return null;
        SetEntityProperties(entity);

        UpdateValidate(entity, updateDto);
        if (HasErrors)
            return null;

        return entity;
    }

    


    protected override void CreateValidate(CreateCustomJobDlDto dto)
    {
        Validate(null, dto);
    }

    protected override void UpdateValidate(CustomJob entity, UpdateCustomJobDlDto dto)
    {
        Validate(entity, dto);
    }
    public CustomJob UpdateStatus(UpdateStatusCustomJobDlDto dto)
    {
        var entity = ById(dto.Id);
       

        if (HasErrors)
            return null;

        dto.UpdateEntity(entity);
        Context.Entry(entity).State = EntityState.Modified;

        return entity;
    }

    private void Validate<TDto>(CustomJob entity, CustomJobDlDto<TDto> dto)
        where TDto : CustomJobDlDto<TDto>
    {
        var query = DbSet.AsQueryable();

    }

    protected override IQueryable<CustomJob> InjectFilter(IQueryable<CustomJob> query)
        => query.Where(a=> a.StatusId!=StatusIdConst.DELETED);
}
