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

namespace SspUis.BizLogicLayer.DistrictServices
{
    public class DistrictTranslateDto : DistrictTranslateDlDto, ILinkToEntity<DistrictTranslate>
    {
        public string Language { get; set; }
    }

    public class DistrictTranslateDtoConfig : PerDtoConfig<DistrictTranslateDto, DistrictTranslate>
    {
        public override Action<IMappingExpression<DistrictTranslate, DistrictTranslateDto>> AlterReadMapping =>
            cfg => cfg
                .IncludeBase<DistrictTranslate, DistrictTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
