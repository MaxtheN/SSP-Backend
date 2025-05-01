using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Appeal;
using System;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public class  AppealTypeArriveTranslateDlDto : TranslateDto< AppealTypeArriveTranslateDlDto,  AppealTypeArriveTranslate, TranslateColumn>, ILinkToEntity< AppealTypeArriveTranslate>
    {
    }

    public class  AppealTypeArriveTranslateDlDtoConfig : PerDtoConfig< AppealTypeArriveTranslateDlDto,  AppealTypeArriveTranslate>
    {
        public override Action<IMappingExpression< AppealTypeArriveTranslate,  AppealTypeArriveTranslateDlDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ColumnName,x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression< AppealTypeArriveTranslateDlDto,  AppealTypeArriveTranslate>> AlterSaveMapping => 
            cfg => cfg
                .ForMember(x => x.ColumnName,x => x.MapFrom(dto => dto.ColumnName));
    }
}
