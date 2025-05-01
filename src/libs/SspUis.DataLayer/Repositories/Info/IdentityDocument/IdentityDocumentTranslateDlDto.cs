using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.Repositories
{
    public class IdentityDocumentTranslateDlDto : TranslateDto<IdentityDocumentTranslateDlDto, IdentityDocumentTranslate, TranslateColumn>, ILinkToEntity<IdentityDocumentTranslate>
    {

    }

    public class IdentityDocumentTranslateDlDtoConfig : PerDtoConfig<IdentityDocumentTranslateDlDto, IdentityDocumentTranslate>
    {
        public override Action<IMappingExpression<IdentityDocumentTranslate, IdentityDocumentTranslateDlDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression<IdentityDocumentTranslateDlDto, IdentityDocumentTranslate>> AlterSaveMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
