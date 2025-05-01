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

namespace SspUis.BizLogicLayer.Hrm.DualEducationTypeServices
{
    public class DualEducationTypeTranslateDto : DualEducationTypeTranslateDlDto, ILinkToEntity<DualEducationTypeTranslate>
    {
        public string Language { get; set; }
    }
    public class DualEducationTypeTranslateDtoConfig : PerDtoConfig<DualEducationTypeTranslateDto, DualEducationTypeTranslate>
    {
        public override Action<IMappingExpression<DualEducationTypeTranslate, DualEducationTypeTranslateDto>> AlterReadMapping => cfg => cfg
                .IncludeBase<DualEducationTypeTranslate, DualEducationTypeTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
