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

namespace SspUis.BizLogicLayer.PrtnRejectReasonServices
{
    public class PrtnRejectReasonTranslateDto : PrtnRejectReasonTranslateDlDto, ILinkToEntity<PrtnRejectReasonTranslate>
    {
        public string Language { get; set; }
    }
    public class PrtnRejectReasonTranslateDtoConfig : PerDtoConfig<PrtnRejectReasonTranslateDto, PrtnRejectReasonTranslate>
    {
        public override Action<IMappingExpression<PrtnRejectReasonTranslate, PrtnRejectReasonTranslateDto>> AlterReadMapping => cfg => cfg
                .IncludeBase<PrtnRejectReasonTranslate, PrtnRejectReasonTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
