using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public class  ContractorActivityTypeTranslateDlDto :
        TranslateDto< ContractorActivityTypeTranslateDlDto, 
            ContractorActivityTypeTranslate, 
            TranslateColumn>, 
        ILinkToEntity< ContractorActivityTypeTranslate>
    {
    }

    public class  ContractorActivityTypeTranslateDlDtoConfig : PerDtoConfig< ContractorActivityTypeTranslateDlDto,  ContractorActivityTypeTranslate>
    {
        public override Action<IMappingExpression< ContractorActivityTypeTranslate,  ContractorActivityTypeTranslateDlDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ColumnName,x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression< ContractorActivityTypeTranslateDlDto,  ContractorActivityTypeTranslate>> AlterSaveMapping => 
            cfg => cfg
                .ForMember(x => x.ColumnName,x => x.MapFrom(dto => dto.ColumnName));
    }
}
