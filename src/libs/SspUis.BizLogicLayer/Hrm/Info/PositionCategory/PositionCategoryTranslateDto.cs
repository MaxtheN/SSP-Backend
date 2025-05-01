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

namespace SspUis.BizLogicLayer.Hrm.PositionCategoryServices
{
    public class PositionCategoryTranslateDto : PositionCategoryTranslateDlDto, ILinkToEntity<PositionCategoryTranslate>
    {
        public string Language { get; set; }
    }
    public class PositionCategoryTranslateDtoConfig : PerDtoConfig<PositionCategoryTranslateDto, PositionCategoryTranslate>
    {
        public override Action<IMappingExpression<PositionCategoryTranslate, PositionCategoryTranslateDto>> AlterReadMapping => cfg => cfg
                .IncludeBase<PositionCategoryTranslate, PositionCategoryTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
