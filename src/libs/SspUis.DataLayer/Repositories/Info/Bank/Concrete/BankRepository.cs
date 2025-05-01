using SspUis.DataLayer.EfClasses;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class BankRepository : BaseEntityRepository<int, Bank, CreateBankDlDto, UpdateBankDlDto>, IBankRepository
    {
        public BankRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }

        public Bank ByCode(string code)
        {
            return ByIdQuery().FirstOrDefault(a => a.Code == code);
        }

        protected override IQueryable<Bank> ByIdQuery()
        {
            return AllAsQueryable.Include(a => a.Translates);
        }

        protected override void CreateValidate(CreateBankDlDto dto)
        {
            Validate(null, dto);
        }

        protected override void UpdateValidate(Bank entity, UpdateBankDlDto dto)
        {
            Validate(entity, dto);
        }

        private void Validate<TDto>(Bank entity, BankDlDto<TDto> dto)
            where TDto : BankDlDto<TDto>
        {
            var query = DbSet.AsQueryable();

            if (entity != null)
                query = query.Where(a => a.Id != entity.Id);

            int bankCode = (dto.BankCodeId.HasValue) ? dto.BankCodeId.Value : 0;

            if (query.ByBankCode(dto.Code, bankCode, isIncludePassive: true).Any())
                AddError($"Банк с этим кодом ({dto.Code}) уже существует.", nameof(dto.Code));
            if (query.ByBankName(dto.BankName, bankCode, isIncludePassive: true).Any())
                AddError($"Банк с этим кодом ({dto.BankName}) уже существует.", nameof(dto.BankName));
        }

    }
}
