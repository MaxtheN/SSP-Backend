using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public class DepartmentService : StatusGenericHandler, IDepartmentService
{
    private readonly IDepartmentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public DepartmentService(
        IDepartmentRepository repository,
        IUnitOfWork unitOfWork,
        IAuthService authService)
    {
        this._repository = repository;
        this._unitOfWork = unitOfWork;
        this._authService = authService;
    }

    public PagedResult<DepartmentListDto> GetList(TableSortFilterPageOptions dto)
    {
        return GetQuery<DepartmentListDto>()
            .SortFilter(dto)
            .ToTableData(dto);
    }

    public DepartmentDto Get()
    {
        return new DepartmentDto();
    }

    public DepartmentDto Get(int id)
    {
        var dto = GetQuery<DepartmentDto>().FirstOrDefault(d => d.Id == id);
        if (dto == null)
            AddError("По вашему запросу запись не найдено");
        return dto;
    }

    public SelectList<int> AsSelectList(int? organizationId = null)
    {
        int selectedOrganizationId = organizationId ?? _authService.User.OrganizationId;

        var data =  _unitOfWork.Context.Set<Department>().Include(a=>a.Translates)
                .AsSelectList(selectedOrganizationId);
        return data;
    }

    public HaveId<int> Create(CreateDepartmentDlDto dto)
    {
        var transaction = _unitOfWork.BeginTransaction();
        try
        {
            var entity = _repository.Create(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            //entity.OrganizationId = _authService.User.OrganizationId;

            if (IsValid)
            {
                _unitOfWork.Save();

                ///Edoc schema uchun Departmenti sync qilish.
                if (!HasErrors)
                    CreateDepartmentForEdocSchema(entity);
                if (HasErrors)
                    return null;

                transaction.Commit();
                return HaveId.Create(entity.Id);
            }

        }
        catch (Exception e)
        {
            AddError(e.Message, "inner:" + e.InnerException, "StackTrace" + e.StackTrace, "data" + e.Data);
            _unitOfWork.Rollback();
        }
        finally
        {
            transaction.Dispose();
        }
        return null;
    }

    public void Update(UpdateDepartmentDlDto dto)
    {
        var transaction = _unitOfWork.BeginTransaction();
        try
        {

            var entity = _repository.Update(dto, ent => Validation(dto, ent));
            CombineStatuses(_repository);
            if (IsValid)
            {
                _unitOfWork.Save();
                ///Edoc schema uchun Departmenti sync qilish.
                if (!HasErrors)
                    UpdateDepartmentForEdocSchema(entity);

                if (HasErrors)
                    return;
                transaction.Commit();
            }

        }
        catch (Exception e)
        {
            AddError(e.Message);
            _unitOfWork.Rollback();
        }
        finally
        {
            transaction.Dispose();
        }
    }

    public void Delete(int id)
    {
        //_repository.Delete(id);
        var entity = _repository.ById(id);
        entity.StateId = StateIdConst.PASSIVE;
        UpdateDepartmentForEdocSchema(entity);
        if (IsValid)
            _unitOfWork.Save();
    }

    private IQueryable<TDto> GetQuery<TDto>()
           where TDto : class
    {
        return _repository.ReadAsNoTracked<TDto>();
    }

    private void Validation<TDto>(DepartmentDlDto<TDto> dto, Department entity)
          where TDto : DepartmentDlDto<TDto>
    {
        var query = _repository.AllAsQueryable;

        if (entity?.Id > 0)
            query = query.Where(a => a.Id != entity.Id);

        var exsisit = query.Any(department => department.Code == dto.Code && department.OrganizationId == _authService.User.OrganizationId);
        if (exsisit)
            AddError($"Запись с этим кодом ({dto?.Code}) уже существует.",
                    nameof(dto.Code));

        exsisit = query.Any(department => department.OrderCode == dto.OrderCode && department.OrganizationId == _authService.User.OrganizationId);
        if (exsisit)
            AddError($"Запись с этим порядковый номером ({dto?.OrderCode}) уже существует.",
                    nameof(dto.Code));
    }


    #region Edoc Department

    public void SyncEdocDepartment(IDbContextTransaction outTransaction = null)
    {
        var departments = _unitOfWork.Context.Set<Department>().ToList();

        var transaction = outTransaction == null ? _unitOfWork.BeginTransaction() : outTransaction;
        try
        {
            foreach (var item in departments)
            {
                CreateDepartmentForEdocSchema(item);
            }
            if (HasErrors)
                return;

            if (outTransaction == null)
                transaction.Commit();
        }
        catch (Exception e)
        {
            AddError(e.Message);
            _unitOfWork.Rollback();
        }
        finally
        {
            transaction.Dispose();
        }
    }

    private void CreateDepartmentForEdocSchema(Department dto)
    {
        try
        {
            var exsist = _unitOfWork.Context.Set<Ssp.DataLayer.EFClasses.Edoc.Department>()
                                    .Any(x => x.Id == dto.Id);
            if (!exsist)
            {
                var department = new Ssp.DataLayer.EFClasses.Edoc.Department()
                {
                    Id = dto.Id,
                    OrderCode = dto.OrderCode,
                    DateOfCreated = DateTime.Now,
                    FullName = dto.FullName,
                    ShortName = dto.ShortName,
                    UniqueId = Guid.NewGuid().ToString(),
                    StateId = dto.StateId,
                    OrganizationId = dto.OrganizationId,
                    Code = dto.Code.ToString(),
                    ParentId = dto.ParentId,
                    IndexCode = dto.IndexCode,
                    CreatedUserId = dto.CreatedUserId.HasValue ? dto.CreatedUserId.Value : 1,
                };
                _unitOfWork.Context.Add(department);
                _unitOfWork.Save();
            }
            else
            {
                UpdateDepartmentForEdocSchema(dto);
            }

        }
        catch (Exception e)
        {
            AddError(e.Message);
        }
    }

    private void UpdateDepartmentForEdocSchema(Department dto)
    {
        try
        {
            var department = _unitOfWork.Context.Set<Ssp.DataLayer.EFClasses.Edoc.Department>()
                                    .FirstOrDefault(x => x.Id == dto.Id);
            if (department is not null)
            {
                department.FullName = dto.FullName;
                department.ShortName = dto.ShortName;
                department.StateId = dto.StateId;
                department.OrganizationId = dto.OrganizationId;
                department.Code = dto.Code.ToString();
                department.ParentId = dto.ParentId;
                department.DateOfModified = DateTime.Now;
                department.ModifiedUserId = _authService.User.Id;
                department.IndexCode = dto.IndexCode;
                _unitOfWork.Context.Update(department);
                _unitOfWork.Save();
            }
            else
            {
                CreateDepartmentForEdocSchema(dto);
            }
        }
        catch (Exception e)
        {
            AddError(e.Message);
        }
    }
    #endregion
}
