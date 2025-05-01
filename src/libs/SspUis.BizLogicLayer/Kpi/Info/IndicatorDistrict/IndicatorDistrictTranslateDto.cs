using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer
{
    public class IndicatorDistrictTranslateDto :
        IndicatorDistrictTranslateDlDto,
        ILinkToEntity<IndicatorDistrictTranslate>
    {
        public string Language { get; set; }
    }

    public class IndicatorDistrictTranslateDtoConfig : PerDtoConfig<IndicatorDistrictTranslateDto, IndicatorDistrictTranslate>
    {
        public override Action<IMappingExpression<IndicatorDistrictTranslate, IndicatorDistrictTranslateDto>> AlterReadMapping =>
            cfg => cfg
                .IncludeBase<IndicatorDistrictTranslate, IndicatorDistrictTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
