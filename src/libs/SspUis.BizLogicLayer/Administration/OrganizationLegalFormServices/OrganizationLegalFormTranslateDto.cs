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

namespace SspUis.BizLogicLayer.OrganizationLegalFormServices
{
    public class OrganizationLegalFormTranslateDto : OrganizationLegalFormTranslateDlDto, ILinkToEntity<OrganizationLegalFormTranslate>
    {
        public string Language { get; set; }
    }
    public class OrganizationLegalFormTranslateDtoConfig : PerDtoConfig<OrganizationLegalFormTranslateDto, OrganizationLegalFormTranslate>
    {
        public override Action<IMappingExpression<OrganizationLegalFormTranslate, OrganizationLegalFormTranslateDto>> AlterReadMapping => cfg => cfg
               .IncludeBase<OrganizationLegalFormTranslate, OrganizationLegalFormTranslateDlDto>()
               .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
