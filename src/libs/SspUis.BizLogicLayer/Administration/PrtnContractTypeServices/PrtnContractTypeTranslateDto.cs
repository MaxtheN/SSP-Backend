using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.PrtnContractTypeServices
{
    public class PrtnContractTypeTranslateDto : PrtnContractTypeTranslateDlDto, ILinkToEntity<PrtnContractTypeTranslate>
    {
        public string Language { get; set; }
    }
    public class PrtnContractTypeTranslateDtoConfig : PerDtoConfig<PrtnContractTypeTranslateDto, PrtnContractTypeTranslate>
    {
        public override Action<IMappingExpression<PrtnContractTypeTranslate, PrtnContractTypeTranslateDto>> AlterReadMapping => cfg => cfg
                .IncludeBase<PrtnContractTypeTranslate, PrtnContractTypeTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
