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

namespace SspUis.BizLogicLayer.Hrm.TariffScaleCoefServices;

public class TariffScaleCoefTranslateDto : TariffScaleCoefTranslateDlDto, ILinkToEntity<TariffScaleCoefTranslate>
{
    public string Language { get; set; }
}
public class TariffScaleCoefTranslateDtoConfig : PerDtoConfig<TariffScaleCoefTranslateDto, TariffScaleCoefTranslate>
{
    public override Action<IMappingExpression<TariffScaleCoefTranslate, TariffScaleCoefTranslateDto>> AlterReadMapping => cfg => cfg
            .IncludeBase<TariffScaleCoefTranslate, TariffScaleCoefTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}
