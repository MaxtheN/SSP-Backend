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
    public class PrtnRejectReasonTranslateDlDto :
        TranslateDto<PrtnRejectReasonTranslateDlDto, PrtnRejectReasonTranslate, TranslateColumn>,
        ILinkToEntity<PrtnRejectReasonTranslate>
    {

    }
    public class PrtnRejectReasonTranslateDlDtoConfig : PerDtoConfig<PrtnRejectReasonTranslateDlDto, PrtnRejectReasonTranslate>
    {
        public override Action<IMappingExpression<PrtnRejectReasonTranslate, PrtnRejectReasonTranslateDlDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression<PrtnRejectReasonTranslateDlDto, PrtnRejectReasonTranslate>> AlterSaveMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
