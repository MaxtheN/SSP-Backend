using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public class PartisanshipTranslateDlDto : TranslateDto<PartisanshipTranslateDlDto, PartisanshipTranslate, TranslateColumn>, ILinkToEntity<PartisanshipTranslate>
    {
    }

    public class PartisanshipTranslateDlDtoConfig : PerDtoConfig<PartisanshipTranslateDlDto, PartisanshipTranslate>
    {
        public override Action<IMappingExpression<PartisanshipTranslate, PartisanshipTranslateDlDto>> AlterReadMapping => Partisanship => Partisanship
            .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression<PartisanshipTranslateDlDto, PartisanshipTranslate>> AlterSaveMapping =>
            Partisanship => Partisanship
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
