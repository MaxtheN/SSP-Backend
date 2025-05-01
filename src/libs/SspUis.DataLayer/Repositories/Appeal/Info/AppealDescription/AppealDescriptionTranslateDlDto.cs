using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Appeal;
using System;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public class  AppealDescriptionTranslateDlDto : TranslateDto< AppealDescriptionTranslateDlDto,  AppealDescriptionTranslate, TranslateColumn>, ILinkToEntity< AppealDescriptionTranslate>
    {
    }

    public class  AppealDescriptionTranslateDlDtoConfig : PerDtoConfig< AppealDescriptionTranslateDlDto,  AppealDescriptionTranslate>
    {
        public override Action<IMappingExpression< AppealDescriptionTranslate,  AppealDescriptionTranslateDlDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ColumnName,x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression< AppealDescriptionTranslateDlDto,  AppealDescriptionTranslate>> AlterSaveMapping => 
            cfg => cfg
                .ForMember(x => x.ColumnName,x => x.MapFrom(dto => dto.ColumnName));
    }
}
