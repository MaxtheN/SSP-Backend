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

namespace SspUis.BizLogicLayer.RegionServices
{
    public class RegionTranslateDto : RegionTranslateDlDto, ILinkToEntity<RegionTranslate>
    {
        public string Language { get; set; }
    }

    public class RegionTranslateDtoConfig : PerDtoConfig<RegionTranslateDto, RegionTranslate>
    {
        public override Action<IMappingExpression<RegionTranslate, RegionTranslateDto>> AlterReadMapping =>
            cfg => cfg
                .IncludeBase<RegionTranslate, RegionTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
