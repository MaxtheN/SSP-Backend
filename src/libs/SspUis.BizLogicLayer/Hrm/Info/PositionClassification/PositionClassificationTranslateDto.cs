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

namespace SspUis.BizLogicLayer.Hrm.PositionClassificationServices
{
    public class PositionClassificationTranslateDto : PositionClassificationTranslateDlDto, ILinkToEntity<PositionClassificationTranslate>
    {
        public string Language { get; set; }
    }
    public class PositionClassificationTranslateDtoConfig : PerDtoConfig<PositionClassificationTranslateDto, PositionClassificationTranslate>
    {
        public override Action<IMappingExpression<PositionClassificationTranslate, PositionClassificationTranslateDto>> AlterReadMapping => cfg => cfg
                .IncludeBase<PositionClassificationTranslate, PositionClassificationTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
