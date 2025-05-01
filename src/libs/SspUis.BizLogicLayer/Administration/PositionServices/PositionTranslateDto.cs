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

namespace SspUis.BizLogicLayer.PositionServices
{
    public class PositionTranslateDto : PositionTranslateDlDto, ILinkToEntity<PositionTranslate>
    {
        public string Language { get; set; }
    }

    public class PositionTranslateDtoConfig : PerDtoConfig<PositionTranslateDto, PositionTranslate>
    {
        public override Action<IMappingExpression<PositionTranslate, PositionTranslateDto>> AlterReadMapping =>
            cfg => cfg
                .IncludeBase<PositionTranslate, PositionTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
