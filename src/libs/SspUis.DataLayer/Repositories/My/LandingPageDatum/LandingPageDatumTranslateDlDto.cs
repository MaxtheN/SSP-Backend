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
    public class LandingPageDatumTranslateDlDto : TranslateDto<LandingPageDatumTranslateDlDto, LandingPageDatumTranslate, LandingPageDatumTranslateColumn>, ILinkToEntity<LandingPageDatumTranslate>
    {

    }

    public class LandingPageDatumTranslateDlDtoConfig :PerDtoConfig<LandingPageDatumTranslateDlDto, LandingPageDatumTranslate>
    {
        public override Action<IMappingExpression<LandingPageDatumTranslate, LandingPageDatumTranslateDlDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<LandingPageDatumTranslateColumn>()));

        public override Action<IMappingExpression<LandingPageDatumTranslateDlDto, LandingPageDatumTranslate>> AlterSaveMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
