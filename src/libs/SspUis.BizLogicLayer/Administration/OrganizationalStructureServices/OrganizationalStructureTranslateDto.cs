using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer.OrganizationalStructureServices
{
    public class OrganizationalStructureTranslateDto : OrganizationalStructureTranslateDlDto, ILinkToEntity<OrganizationalStructureTranslate>
    {
        public string Language { get; set; }
    }

    public class OrganizationalStructureTranslateDtoConfig : PerDtoConfig<OrganizationalStructureTranslateDto, OrganizationalStructureTranslate>
    {
        public override Action<IMappingExpression<OrganizationalStructureTranslate, OrganizationalStructureTranslateDto>> AlterReadMapping =>
            cfg => cfg
                .IncludeBase<OrganizationalStructureTranslate, OrganizationalStructureTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
