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

namespace SspUis.BizLogicLayer.IdentityDocumentServices
{
    public class IdentityDocumentTranslateDto : IdentityDocumentTranslateDlDto, ILinkToEntity<IdentityDocumentTranslate>
    {
        public string Language { get; set; }
    }
    public class IdentityDocumentTranslateDtoConfig : PerDtoConfig<IdentityDocumentTranslateDto, IdentityDocumentTranslate>
    {
        public override Action<IMappingExpression<IdentityDocumentTranslate, IdentityDocumentTranslateDto>> AlterReadMapping => cfg => cfg
                .IncludeBase<IdentityDocumentTranslate, IdentityDocumentTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
