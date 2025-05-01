using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public class  NeedChamberServiceTranslateDlDto : TranslateDto< NeedChamberServiceTranslateDlDto,  NeedChamberServiceTranslate, TranslateColumn>, ILinkToEntity<NeedChamberServiceTranslate>
    {
    }

    public class  NeedChamberServiceTranslateDlDtoConfig : PerDtoConfig< NeedChamberServiceTranslateDlDto,  NeedChamberServiceTranslate>
    {
        public override Action<IMappingExpression< NeedChamberServiceTranslate,  NeedChamberServiceTranslateDlDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ColumnName,x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression< NeedChamberServiceTranslateDlDto,  NeedChamberServiceTranslate>> AlterSaveMapping => 
            cfg => cfg
                .ForMember(x => x.ColumnName,x => x.MapFrom(dto => dto.ColumnName));
    }
}
