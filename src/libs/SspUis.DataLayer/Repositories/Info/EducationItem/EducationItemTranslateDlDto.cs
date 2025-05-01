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
    public class EducationItemTranslateDlDto : TranslateDto<EducationItemTranslateDlDto, EducationItemTranslate, TranslateColumn>, ILinkToEntity<EducationItemTranslate>
    {

    }

    public class EducationItemTranslateDlDtoConfig : PerDtoConfig<EducationItemTranslateDlDto, EducationItemTranslate>
    {
        public override Action<IMappingExpression<EducationItemTranslate, EducationItemTranslateDlDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression<EducationItemTranslateDlDto, EducationItemTranslate>> AlterSaveMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
