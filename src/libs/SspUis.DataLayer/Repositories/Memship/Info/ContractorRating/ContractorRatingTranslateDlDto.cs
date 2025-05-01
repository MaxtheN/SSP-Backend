using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;

using WEBASE;

namespace SspUis.DataLayer.Repositories;

public class ContractorRatingTranslateDlDto : TranslateDto<ContractorRatingTranslateDlDto,
    ContractorRatingTranslate, TranslateColumn>,
    ILinkToEntity<ContractorRatingTranslate>
{
}
public class ContractorRatingTranslateDlDtoConfig : PerDtoConfig<ContractorRatingTranslateDlDto, ContractorRatingTranslate>
{
    public override Action<IMappingExpression<ContractorRatingTranslate, ContractorRatingTranslateDlDto>> AlterReadMapping => cfg => cfg
        .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

    public override Action<IMappingExpression<ContractorRatingTranslateDlDto, ContractorRatingTranslate>> AlterSaveMapping =>
        cfg => cfg
            .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
}