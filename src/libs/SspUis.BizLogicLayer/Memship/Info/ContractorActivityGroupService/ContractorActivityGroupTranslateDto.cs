using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer.Hrm.ContractorActivityGroupServices
{
    public class  ContractorActivityGroupTranslateDto :  ContractorActivityGroupTranslateDlDto, 
        ILinkToEntity< ContractorActivityGroupTranslate>
    {
        public string Language { get; set; }
    }

    public class  ContractorActivityGroupTranslateDtoConfig : PerDtoConfig< ContractorActivityGroupTranslateDto,  ContractorActivityGroupTranslate>
    {
        public override Action<IMappingExpression< ContractorActivityGroupTranslate,  ContractorActivityGroupTranslateDto>> AlterReadMapping =>
            cfg => cfg
                .IncludeBase< ContractorActivityGroupTranslate,  ContractorActivityGroupTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
