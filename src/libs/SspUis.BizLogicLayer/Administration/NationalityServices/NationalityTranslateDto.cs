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

namespace SspUis.BizLogicLayer.NationalityServices
{
    public class NationalityTranslateDto : NationalityTranslateDlDto, ILinkToEntity<NationalityTranslate>
    {
        public string Language { get; set; }
    }

    public class NationalityTranslateDtoConfig : PerDtoConfig<NationalityTranslateDto, NationalityTranslate>
    {
        public override Action<IMappingExpression<NationalityTranslate, NationalityTranslateDto>> AlterReadMapping => cfg => cfg
                .IncludeBase<NationalityTranslate, NationalityTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }

}
