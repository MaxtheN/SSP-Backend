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

namespace SspUis.BizLogicLayer.Hrm.TariffScaleServices;

public class TariffScaleTranslateDto : TariffScaleTranslateDlDto, ILinkToEntity<TariffScaleTranslate>
{
    public string Language { get; set; }
}
public class TariffScaleTranslateDtoConfig : PerDtoConfig<TariffScaleTranslateDto, TariffScaleTranslate>
{
    public override Action<IMappingExpression<TariffScaleTranslate, TariffScaleTranslateDto>> AlterReadMapping => cfg => cfg
            .IncludeBase<TariffScaleTranslate, TariffScaleTranslateDlDto>()
            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
}
