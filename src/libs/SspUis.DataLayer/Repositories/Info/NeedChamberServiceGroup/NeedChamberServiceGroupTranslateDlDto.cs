using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public class NeedChamberServiceGroupTranslateDlDto
        : TranslateDto<NeedChamberServiceGroupTranslateDlDto, NeedChamberServiceGroupTranslate, TranslateColumn>,
        ILinkToEntity<NeedChamberServiceGroupTranslate>
    { }

    public class NeedChamberServiceGroupTranslateDlDtoConfig
        : PerDtoConfig<NeedChamberServiceGroupTranslateDlDto, NeedChamberServiceGroupTranslate>
    {
        public override Action<IMappingExpression<NeedChamberServiceGroupTranslate, NeedChamberServiceGroupTranslateDlDto>> AlterReadMapping =>
            cfg => cfg.ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression<NeedChamberServiceGroupTranslateDlDto, NeedChamberServiceGroupTranslate>> AlterSaveMapping =>
            cfg => cfg.ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
