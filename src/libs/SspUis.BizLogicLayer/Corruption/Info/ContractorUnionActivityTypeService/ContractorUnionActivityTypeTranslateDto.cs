using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Corruption;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer.Corruption.ContractorUnionActivityTypeServices
{
    public class  ContractorUnionActivityTypeTranslateDto :  ContractorUnionActivityTypeTranslateDlDto, ILinkToEntity< ContractorUnionActivityTypeTranslate>
    {
        public string Language { get; set; }
    }

    public class  ContractorUnionActivityTypeTranslateDtoConfig : PerDtoConfig< ContractorUnionActivityTypeTranslateDto,  ContractorUnionActivityTypeTranslate>
    {
        public override Action<IMappingExpression< ContractorUnionActivityTypeTranslate,  ContractorUnionActivityTypeTranslateDto>> AlterReadMapping =>
            cfg => cfg
                .IncludeBase< ContractorUnionActivityTypeTranslate,  ContractorUnionActivityTypeTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
