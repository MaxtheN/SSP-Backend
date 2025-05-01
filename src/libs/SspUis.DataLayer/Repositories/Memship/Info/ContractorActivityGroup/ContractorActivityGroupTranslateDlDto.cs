using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public class  ContractorActivityGroupTranslateDlDto : 
        TranslateDto< ContractorActivityGroupTranslateDlDto, 
            ContractorActivityGroupTranslate, TranslateColumn>, 
        ILinkToEntity< ContractorActivityGroupTranslate>
    {
    }

    public class  ContractorActivityGroupTranslateDlDtoConfig : PerDtoConfig< ContractorActivityGroupTranslateDlDto,  ContractorActivityGroupTranslate>
    {
        public override Action<IMappingExpression< ContractorActivityGroupTranslate,  ContractorActivityGroupTranslateDlDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ColumnName,x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression< ContractorActivityGroupTranslateDlDto,  ContractorActivityGroupTranslate>> AlterSaveMapping => 
            cfg => cfg
                .ForMember(x => x.ColumnName,x => x.MapFrom(dto => dto.ColumnName));
    }
}
