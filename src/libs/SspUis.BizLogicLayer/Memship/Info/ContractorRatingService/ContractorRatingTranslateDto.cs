using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.BizLogicLayer.Hrm.ContractorActivityTypeServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Memship;

public class ContractorRatingTranslateDto :
        ContractorRatingTranslateDlDto,
    ILinkToEntity<ContractorRatingTranslate>
{
    public string Language { get; set; }

    public class ContractorRatingTranslateDtoConfig : PerDtoConfig<ContractorRatingTranslateDto, ContractorRatingTranslate>
    {
        public override Action<IMappingExpression<ContractorRatingTranslate, ContractorRatingTranslateDto>> AlterReadMapping =>
            cfg => cfg
                .IncludeBase<ContractorRatingTranslate, ContractorRatingTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
