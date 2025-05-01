using System.Collections.Generic;
using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using WEBASE.EF;
using WEBASE.Utility;

namespace SspUis.DataLayer.Repositories
{
    public class EmployeeRepository : BaseEntityRepository<int, Employee, CreateEmployeeDlDto, UpdateEmployeeDlDto>, IEmployeeRepository
    {
        private readonly IAuthService _authService;
        public EmployeeRepository(ICrudServices crudServices, IAuthService authService)
            : base(crudServices)
        {
            _authService = authService;
        }
        public Employee Create(CreateEmployeeByUserDlDto dto)
        {
            if(HasErrors)
                return null;
            var user = CrudServices.Context.Set<User>()
                                           .Include(a => a.Person)
                                           .Include(a => a.Organization)
                                           .FirstOrDefault(a => a.Id == dto.UserId);
            if(user == null)
            {
                AddError("User not found");
                return null;
            }
            var entity = dto.CreateEntity();
            entity.PersonId = user.PersonId;
            if (!_authService.HasPermission(ModuleCode.AllEmployeeCreate))
            {
                entity.OrganizationId = dto.OrganizationId.Value;
            }
            else
            entity.OrganizationId = user.OrganizationId;
            DbSet.Add(entity);
            Context.Entry(entity).State = EntityState.Added;
            return entity;
        }
        protected override void OnCreate(Employee entity, CreateEmployeeDlDto dto)
        {
            if (_authService.HasPermission(ModuleCode.AllEmployeeCreate))
            {
                entity.OrganizationId = dto.OrganizationId.Value;
            }
            else
            entity.OrganizationId = _authService.Organization.Id;
        }
        protected override IQueryable<Employee> ByIdQuery()
        {
            return base.ByIdQuery()
                                   .Include(x => x.PlaceOfWorks)
                                   .Include(x => x.Relatives)
                                   .Include(x => x.HigherEdu)
                                   .Include(x => x.AcademicDegrees)
                                   .Include(x => x.DegreeTitles)
                                   .Include(x => x.ElectionMembers)
                                   .Include(x => x.LanguageProficiencys)
                                   .Include(x => x.Partisanships)
                                   .Include(x => x.ScientificDegrees)
                                   .Include(x => x.StateAwards)
                                   .Include(x => x.MilitaryRanks)
                                   .Include(x=>x.Person);
        }

        protected override void CreateValidate(CreateEmployeeDlDto dto)
        {
            Validate(null, dto);
        }
    
        protected override void UpdateValidate(Employee entity, UpdateEmployeeDlDto dto)
        {
            Validate(entity, dto, true);
        }
        private void Validate<TDto>(Employee entity, EmployeeDlDto<TDto> dto, bool isUpdated = false)
            where TDto : EmployeeDlDto<TDto>
        {
            var query = DbSet.Include(a => a.Person).AsQueryable();

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

            Employee emp = null;

            if(isUpdated == false)
            {
                emp = query.IsActive().FirstOrDefault(a => a.PersonId == dto.PersonId && a.OrganizationId == _authService.Organization.Id);
            }

            else
            {
                emp = query.IsActive().FirstOrDefault(a => a.PersonId == dto.PersonId);
            }

            if(emp != null)
                AddError($"Это сотрудник уже существует.", $"{emp.Person.PassportSeria}{emp.Person.PassportNumber}");
        }
        public void UpdateRelatives(List<EmployeeRelativeDlDto> relatives, int employeeId)
        {
            var employee = DbSet
                .Include(p => p.Relatives)
                .FirstOrDefault(p => p.Id == employeeId)
                ;
            relatives.ForEach(a =>
            {
                a.FullName = StringUtility.GetFullFIO(a.LastName, a.FirstName, a.FamilyName);
                a.ShortName = StringUtility.GetFIO(a.FamilyName);
            });
            relatives.ApplyChangesTo<int, EmployeeRelativeDlDto, EmployeeRelative>(employee.Relatives);
        }
    }
}
