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

namespace SspUis.BizLogicLayer.Hrm.InstituteServices
{
    public class InstituteTranslateDto : InstituteTranslateDlDto, ILinkToEntity<InstituteTranslate>
    {
        public string Language { get; set; }
    }
    public class InstituteTranslateDtoConfig : PerDtoConfig<InstituteTranslateDto, InstituteTranslate>
    {
        public override Action<IMappingExpression<InstituteTranslate, InstituteTranslateDto>> AlterReadMapping => cfg => cfg
                .IncludeBase<InstituteTranslate, InstituteTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
