using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.ItemOfExpenseServices
{
    public class ItemOfExpenseTranslateDto : ItemOfExpenseTranslateDlDto, ILinkToEntity<ItemOfExpenseTranslate>
    {
        public string Language { get; set; }
    }
    public class ItemOfExpenseTranslateDtoConfig : PerDtoConfig<ItemOfExpenseTranslateDto, ItemOfExpenseTranslate>
    {
        public override Action<IMappingExpression<ItemOfExpenseTranslate, ItemOfExpenseTranslateDto>> AlterReadMapping => cfg => cfg
                .IncludeBase<ItemOfExpenseTranslate, ItemOfExpenseTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
