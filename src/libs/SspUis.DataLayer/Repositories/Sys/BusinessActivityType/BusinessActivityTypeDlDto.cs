using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;
using WEBASE.Attributes;
using AutoMapper;

namespace SspUis.DataLayer.Repositories
{
    public class BusinessActivityTypeDlDto<TDto> : EntityDto<TDto, BusinessActivityType>
        where TDto : BusinessActivityTypeDlDto<TDto>
    {
        [LocalizedRequired]
        public long ContractorId { get; set; }
        [LocalizedRequired]
        public int BankId { get; set; }
        public decimal? HelpAmount { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(255)]
        public string RealEmployeesCount { get; set; }
        [LocalizedRequired]
        public int BusinessCtorId { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(255)]
        public string BusinessCtorName { get; set; }
        public int FinancialHelpId { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(255)]
        public string FinancialHelp { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(255)]
        public string NewEmployeesCount { get; set; }

        public List<BusinessActivityTypeTableDlDto> Tables { get; set; } = new List<BusinessActivityTypeTableDlDto>();

        protected override Action<IMappingExpression<TDto, BusinessActivityType>> AlterMapping =>
            cfg => cfg
                .ForMember(x => x.Tables, x => x.Ignore());

        public override BusinessActivityType CreateEntity()
        {
            var entity = base.CreateEntity();
            Tables.AddTo(entity.Tables);
            return entity;
        }
        public override void UpdateEntity(BusinessActivityType entity)
        {
            base.UpdateEntity(entity);
            Tables.ApplyChangesTo<long, BusinessActivityTypeTableDlDto, BusinessActivityTypeTable>(entity.Tables);
        }
    }
}
