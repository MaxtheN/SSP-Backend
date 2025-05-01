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
using SspUis.Core.Security;
using SspUis.Core;
using Minio.DataModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace SspUis.DataLayer.Repositories
{
    public class PrtnContractRepository : BaseEntityRepository<long, PrtnContract, CreatePrtnContractDlDto, UpdatePrtnContractDlDto, UpdateStatusPrtnContractDlDto>, IPrtnContractRepository
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public PrtnContractRepository(ICrudServices crudServices, IAuthService authService, IUnitOfWork unitOfWork)
            : base(crudServices)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public PrtnContract ChangeDate(long id, DateOnly newDocOn, Action<PrtnContract> validation = null)
        {
            PrtnContract val = ById(id);
            if (base.IsValid)
            {
                if (validation != null)
                {
                    validation(val);
                }
            }

            if (base.HasErrors)
            {
                return null;
            }

            val.DocOn = newDocOn;
            base.Context.Entry(val).State = EntityState.Modified;
            return val;
        }

        public PrtnContractSign ChangeSigner(long prtnContractSignId, int newOrganizationSignId, Action<PrtnContractSign> validation = null)
        {
            PrtnContractSign val = _unitOfWork.Context.Set<PrtnContractSign>()
                .FirstOrDefault(a => a.Id == prtnContractSignId);

            if (base.IsValid)
            {
                if (validation != null)
                { 
                    validation(val);
                }
            }

            if (base.HasErrors)
            {
                return null;
            }

            val.OrganizationSignId = newOrganizationSignId;
            base.Context.Entry(val).State = EntityState.Modified;
            return val;
        }

        protected override IQueryable<PrtnContract> ByIdQuery()
        {
            return base.ByIdQuery().Include(a => a.Signs).Include(a => a.Files);
        }

        protected override IQueryable<PrtnContract> ByIdQuery(bool applyFilter)
        {
            return base.ByIdQuery(applyFilter).Include(a => a.Signs).Include(a => a.Files);
        }

        protected override IQueryable<PrtnContract> InjectFilter(IQueryable<PrtnContract> query)
        {
            query = query.Where(a => a.StatusId != StatusIdConst.DELETED);

            if (_authService.Contractor != null)
                return query.Where(a => a.ContractorId == _authService.Contractor.Id);

            if (_authService.HasPermission(ModuleCode.PrtnContractViewAll))
                return query;

            if (_authService.HasPermission(ModuleCode.PrtnContractViewByRegion))
                return query.Where(a => a.Organization.RegionId == _authService.Organization.RegionId);

            return query.Where(a => 
                // Viloyat va tumanda imzolidigan tashkilot bitta bogani uchun
                (a.OrganizationId == _authService.Organization.Id && a.PrtnContractTypeId != PrtnContractTypeIdConst._201__)

                // Vazirliklardan bir nechtasi shartnomani imzolagani uchun
                || (a.PrtnContractTypeId == PrtnContractTypeIdConst._201__
                    && PrtnContractTypeIdConst.GetBySignOrganizationTypeId(_authService.Organization.SignOrganizationTypeId ?? 0) == PrtnContractTypeIdConst._201__)
            );
        }
    }
}
