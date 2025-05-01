using System;
using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ContractorRepository : BaseEntityRepository<long, Contractor, CreateContractorDlDto, UpdateContractorDlDto>, IContractorRepository
    {
        public ContractorRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }
        protected override IQueryable<Contractor> ByIdQuery()
        {
            return base.ByIdQuery()
                .Include(a => a.Okeds)
                .Include(a => a.Oked)
                .Include(a => a.Contacts)
                .Include(a => a.SettlementAccounts)
                .Include(a => a.BusinessmanUserInContractors)
                    .ThenInclude(buc => buc.BusinessmanUser);
        }

        public Contractor ByInn(string inn)
        {
            return ByIdQuery().FirstOrDefault(a => a.Inn == inn);
        }

        public Contractor ByPinfl(string pinfl)
        {
            return ByIdQuery().FirstOrDefault(a => a.Pinfl == pinfl);
        }

        public Contractor Update(MyUpdateContractorDlDto updateDto, Action<Contractor> validation = null)
        {
            var val = ById(updateDto.Id);

            if (IsValid)
            {
                validation?.Invoke(val);
            }

            if (HasErrors)
            {
                return null;
            }

            updateDto.UpdateEntity(val);
            Context.Entry(val).State = EntityState.Modified;
            return val;
        }

        public void UpdateContractorSettlementAccount(UpdateContractorSettlementAccountDlDto dto)
        {
            var entity = Context.Set<ContractorSettlementAccount>()
                .FirstOrDefault(x => x.Id == dto.Id && x.StateId != StateIdConst.PASSIVE);

            if (entity == null)
            { AddError("Такого предпринимателя не существует !"); return; }

            if (IsValid)
            {
                dto.UpdateEntity(entity);
                Context.Entry(entity).State = EntityState.Modified;
            }
        }

        public ContractorContact AddContactInfo(long ownerId, int contactTypeId, string contact, Action<ContractorContact> validation)
        {
            if (base.HasErrors)
                return null;

            ContractorContact val = new ContractorContact
            {
                OwnerId = ownerId,
                ContactTypeId = contactTypeId,
                Contact = contact,
            };
            if (base.IsValid)
            {
                if (validation != null)
                    validation(val);
            }
            if (base.HasErrors)
                return null;

            base.Context.Add(val);
            base.Context.Entry(val).State = EntityState.Added;
            return val;
        }

        protected override void CreateValidate(CreateContractorDlDto dto)
        {
            Validate(null, dto);
        }

        protected override void UpdateValidate(Contractor entity, UpdateContractorDlDto dto)
        {
            Validate(entity, dto);
        }

        public ContractorOffers CreateContractorOffers(ContractorOfferDto dto, ContractorOffers entity)
        {
            entity.SignData = dto.SignData;
            entity.OfferId = dto.OfferId;
            entity.ContractorId = dto.ContractorId;
            entity.CreatedAt = DateTime.Now;

            base.Context.Set<ContractorOffers>().Add(entity);
            base.Context.Entry(entity).State = EntityState.Added;
            //base.Context.SaveChanges();

            return entity;
        }

        public void ChangeMainSettlementAccounting(long contartorId, long settlementAccountId)
        {
            var con = ById(contartorId);
            if (con is null)
            { AddError("Not found !"); return; }

            var basicSett = Context.Set<ContractorSettlementAccount>()
                .FirstOrDefault(x => x.Id == settlementAccountId && x.StateId == StateIdConst.ACTIVE);
            if (basicSett.OwnerId != con.Id || basicSett is null)
            {
                AddError("This account does not belong to this contractor !");
                return;
            }

            foreach (var sett in con.SettlementAccounts)
            {
                sett.IsMain = false;
                if (sett.Id == basicSett.Id)
                    sett.IsMain = true;
            }
        }
        protected override IQueryable<Contractor> ByIdQuery(bool applyFilter)
        {
            return AllAsQueryable.Include(x => x.SettlementAccounts).Include(x => x.Contacts);
        }
        private void Validate<TDto>(Contractor entity, ContractorDlDto<TDto> dto)
            where TDto : ContractorDlDto<TDto>
        {
            var query = DbSet.AsQueryable();

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

            if (!dto.Inn.NullOrEmpty() && query.ByInn(dto.Inn, isIncludePassive: true).Any())
                AddError($"Контрагент с этим ИНН ({dto.Inn}) уже существует.", nameof(dto.Inn));
            if (!dto.Pinfl.NullOrEmpty() && query.ByPinfl(dto.Pinfl, isIncludePassive: true).Any())
                AddError($"Контрагент с этим ИНПС ({dto.Pinfl}) уже существует.", nameof(dto.Pinfl));
        }
    }
}
