using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.DataLayer.Repositories
{
    public class ItemOfExpenseTranslateDlDto : TranslateDto<ItemOfExpenseTranslateDlDto, ItemOfExpenseTranslate, TranslateColumn>, ILinkToEntity<ItemOfExpenseTranslate>
    {

    }

    public class ItemOfExpenseTranslateDlDtoConfig : PerDtoConfig<ItemOfExpenseTranslateDlDto, ItemOfExpenseTranslate>
    {
        public override Action<IMappingExpression<ItemOfExpenseTranslate, ItemOfExpenseTranslateDlDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression<ItemOfExpenseTranslateDlDto, ItemOfExpenseTranslate>> AlterSaveMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
