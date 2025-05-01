using AutoMapper;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ItemOfExpenseDlDto<TDto> : EntityDto<TDto, ItemOfExpense>
        where TDto : ItemOfExpenseDlDto<TDto>
    {
        [LocalizedRequired]
        public int NumberOfGroup { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string ShortName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string FullName { get; set; }
        [LocalizedRequired]
        public bool IsGroup { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(7)]
        public string Code { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(2)]
        public string Code1 { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(3)]
        public string Code2 { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(3)]
        public string Code3 { get; set; }
        public int? ParentId { get; set; }
        [LocalizedRequired]
        public bool AgeingAllowed { get; set; }

        public List<ItemOfExpenseTranslateDlDto> Translates { get; set; } = new List<ItemOfExpenseTranslateDlDto>();

        protected override Action<IMappingExpression<TDto, ItemOfExpense>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

        public override ItemOfExpense CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Translates.AddByUniqueFKTo(entity.Translates);
            return entity;
        }

        public override void UpdateEntity(ItemOfExpense entity)
        {
            base.UpdateEntity(entity);
            Translates.AddByUniqueFKTo(entity.Translates);
        }
    }
}
