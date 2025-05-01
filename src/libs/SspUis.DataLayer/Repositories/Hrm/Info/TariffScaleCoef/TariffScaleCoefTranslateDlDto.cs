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
    public class TariffScaleCoefTranslateDlDto : TranslateDto<TariffScaleCoefTranslateDlDto, TariffScaleCoefTranslate, TranslateColumn>, ILinkToEntity<TariffScaleCoefTranslate>
    {

    }

    public class TariffScaleCoefTranslateDlDtoConfig : PerDtoConfig<TariffScaleCoefTranslateDlDto, TariffScaleCoefTranslate>
    {
        public override Action<IMappingExpression<TariffScaleCoefTranslate, TariffScaleCoefTranslateDlDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression<TariffScaleCoefTranslateDlDto, TariffScaleCoefTranslate>> AlterSaveMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
