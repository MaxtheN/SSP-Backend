using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using WEBASE;

namespace SspUis.DataLayer.Repositories;

public class IndicatorDistrictTranslateDlDto : TranslateDto<IndicatorDistrictTranslateDlDto,
    IndicatorDistrictTranslate, TranslateColumn>, ILinkToEntity<IndicatorDistrictTranslate>
{

}

public class IndicatorDistrictTranslateDlDtoConfig : PerDtoConfig<IndicatorDistrictTranslateDlDto, IndicatorDistrictTranslate>
{
    public override Action<IMappingExpression<IndicatorDistrictTranslate, IndicatorDistrictTranslateDlDto>> AlterReadMapping => cfg => cfg
        .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

    public override Action<IMappingExpression<IndicatorDistrictTranslateDlDto, IndicatorDistrictTranslate>> AlterSaveMapping =>
        cfg => cfg
            .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
}

