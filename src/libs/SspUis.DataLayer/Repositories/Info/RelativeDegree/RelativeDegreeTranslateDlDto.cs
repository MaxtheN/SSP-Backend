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

namespace SspUis.DataLayer.Repositories
{
    public class RelativeDegreeTranslateDlDto : TranslateDto<RelativeDegreeTranslateDlDto, RelativeDegreeTranslate, TranslateColumn>, ILinkToEntity<RelativeDegreeTranslate>
    {

    }

    public class RelativeDegreeTranslateDlDtoConfig : PerDtoConfig<RelativeDegreeTranslateDlDto, RelativeDegreeTranslate>
    {
        public override Action<IMappingExpression<RelativeDegreeTranslate, RelativeDegreeTranslateDlDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression<RelativeDegreeTranslateDlDto, RelativeDegreeTranslate>> AlterSaveMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
