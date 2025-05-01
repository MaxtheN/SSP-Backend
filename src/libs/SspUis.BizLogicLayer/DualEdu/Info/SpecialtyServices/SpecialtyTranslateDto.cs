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
using SspUis.DataLayer.EfClasses.DualEdu;

namespace SspUis.BizLogicLayer.Hrm.SpecialtyServices
{
    public class SpecialtyTranslateDto : SpecialtyTranslateDlDto, ILinkToEntity<SpecialtyTranslate>
    {
        public string Language { get; set; }
    }
    public class SpecialtyTranslateDtoConfig : PerDtoConfig<SpecialtyTranslateDto, SpecialtyTranslate>
    {
        public override Action<IMappingExpression<SpecialtyTranslate, SpecialtyTranslateDto>> AlterReadMapping => cfg => cfg
                .IncludeBase<SpecialtyTranslate, SpecialtyTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
