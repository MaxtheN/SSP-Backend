using SspUis.DataLayer.EfClasses;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.EF;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.i18n;
using SspUis.Core.Security;

namespace SspUis.DataLayer.Repositories.Hrm
{
    public class EmployeeManageRepository : BaseEntityRepository<long, EmployeeManage, CreateEmployeeManageDlDto, UpdateEmployeeManageDlDto>, IEmployeeManageRepository
    {
         
        private readonly ICultureHelper _cultureHelper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;


        public EmployeeManageRepository(ICrudServices crudServices,
            ICultureHelper cultureHelper,
            IUnitOfWork unitOfWork,
            IAuthService authService)
            : base(crudServices)
        {
            _cultureHelper = cultureHelper;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        //protected override void OnCreate(EmployeeManage entity, CreateEmployeeManageDlDto dto)
        //{
        //    entity.OrganizationId = _authService.Organization.Id;
        //}

        public override EmployeeManage Update(UpdateEmployeeManageDlDto updateDto, Action<EmployeeManage> validation = null)
        {
            EmployeeManage val = ById(updateDto.Id, false);
            if (base.IsValid)
            {
                if (validation == null)
                {
                    UpdateValidate(val, updateDto);
                }
                else
                {
                    validation(val);
                }
            }

            if (base.HasErrors)
            {
                return null;
            }

            updateDto.IsDeleted = false;
            updateDto.UpdateEntity(val);
            OnUpdate(val, updateDto);
            if (base.HasErrors)
            {
                return null;
            }

            base.Context.Entry(val).State = EntityState.Modified;
            return val;
        }

        public EmployeeManage SetEnd(long id, DateOnly endOn, long endDocumentId, int endTableId)
        {
            var entity = ById(id);

            if (entity == null)
                AddError("Ишга қабул қилиш ҳужжати топилмади.");
            else if (entity.EndDocId != null)
                AddError("Ушбу ҳужжат билан аввал амалиёт бажарилган:" + entity.EndDocId);

            if (HasErrors)
                return null;

            entity.EndOn = endOn;
            entity.EndDocId = endDocumentId;
            entity.EndTableId = endTableId;
            Context.Entry(entity).State = EntityState.Modified;
            _unitOfWork.Save();
            return entity;
        }
        public EmployeeManage SetRate(long id, decimal empRate, long endDocumentId, int endTableId)
        {
            var entity = ById(id);


            if (entity == null)
                AddError("Ишга қабул қилиш ҳужжати топилмади.");
            else if (entity.EndDocId != null)
                AddError("Ушбу ҳужжат билан аввал амалиёт бажарилган:" + entity.EndDocId);

            if (HasErrors)
                return null;

            entity.EmploymentRate = empRate;
            entity.EndDocId = endDocumentId;
            entity.EndTableId = endTableId;
            Context.Entry(entity).State = EntityState.Modified;

            return entity;
        }
        public override EmployeeManage Delete(long id, Action<EmployeeManage> validation = null)
        {

            EmployeeManage val = ById(id);
            if (IsValid)
            {
                validation?.Invoke(val);
            }

            if (HasErrors)
            {
                return null;
            }

            OnDelete(val);
            if (HasErrors)
            {
                return null;
            }

            DbSet.Remove(val);
            Context.Entry(val).State = EntityState.Deleted;
            return val;
        }

        public EmployeeManage SetIsDeleted(long id, bool isDeleted = true, Action<EmployeeManage> validation = null)
        {
            var entity = ById(id);
            if (IsValid)
            {
                validation?.Invoke(entity);
            }

            entity.IsDeleted = isDeleted;

            if (!isDeleted)
            {
                entity.EndOn = null;
                entity.EndDocId = null;
                entity.EndTableId = null;
            }
            Context.Entry(entity).State = EntityState.Modified;

            return entity;
        }
        protected override IQueryable<EmployeeManage> ByIdQuery()
            => AllAsQueryable;

        protected override IQueryable<EmployeeManage> InjectFilter(IQueryable<EmployeeManage> query)
        {
            query = query.Where(a => !a.IsDeleted);
            if (_authService.User.IsAdmin)
                return query;
            else if (_authService.HasPermission(ModuleCode.AllEmployeeManageView))
            {
                return query;
            }
            else
                return query.Where(a => a.OrganizationId == _authService.Organization.Id);
        }
    }
}

