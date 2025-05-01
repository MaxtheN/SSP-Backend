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
    public class CalculationKindTranslateDlDto : TranslateDto<CalculationKindTranslateDlDto, CalculationKindTranslate, TranslateColumn>, ILinkToEntity<CalculationKindTranslate>
    {

    }

    public class CalculationKindTranslateDlDtoConfig : PerDtoConfig<CalculationKindTranslateDlDto, CalculationKindTranslate>
    {
        public override Action<IMappingExpression<CalculationKindTranslate, CalculationKindTranslateDlDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression<CalculationKindTranslateDlDto, CalculationKindTranslate>> AlterSaveMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
