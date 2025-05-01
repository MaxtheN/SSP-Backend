using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Corruption;
using System;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public class  ContractorUnionActivityTypeTranslateDlDto : TranslateDto< ContractorUnionActivityTypeTranslateDlDto,  ContractorUnionActivityTypeTranslate, TranslateColumn>, ILinkToEntity< ContractorUnionActivityTypeTranslate>
    {
    }

    public class  ContractorUnionActivityTypeTranslateDlDtoConfig : PerDtoConfig< ContractorUnionActivityTypeTranslateDlDto,  ContractorUnionActivityTypeTranslate>
    {
        public override Action<IMappingExpression< ContractorUnionActivityTypeTranslate,  ContractorUnionActivityTypeTranslateDlDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ColumnName,x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression< ContractorUnionActivityTypeTranslateDlDto,  ContractorUnionActivityTypeTranslate>> AlterSaveMapping => 
            cfg => cfg
                .ForMember(x => x.ColumnName,x => x.MapFrom(dto => dto.ColumnName));
    }
}
