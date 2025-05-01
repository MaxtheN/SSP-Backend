using AutoMapper;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using GenericServices;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.OrganizationServices
{
    public class OrganizationTranslateDto : OrganizationTranslateDlDto, ILinkToEntity<OrganizationTranslate>
    {
        public string Language { get; set; }
    }

    public class OrganizationTranslateDtoConfig : PerDtoConfig<OrganizationTranslateDto, OrganizationTranslate>
    {
        public override Action<IMappingExpression<OrganizationTranslate, OrganizationTranslateDto>> AlterReadMapping =>
            cfg => cfg
                .IncludeBase<OrganizationTranslate, OrganizationTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
