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
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.CalculationKindServices
{
    public class CalculationKindTranslateDto : CalculationKindTranslateDlDto, ILinkToEntity<CalculationKindTranslate>
    {
        public string Language { get; set; }
    }
    public class CalculationKindTranslateDtoConfig : PerDtoConfig<CalculationKindTranslateDto, CalculationKindTranslate>
    {
        public override Action<IMappingExpression<CalculationKindTranslate, CalculationKindTranslateDto>> AlterReadMapping => cfg => cfg
                .IncludeBase<CalculationKindTranslate, CalculationKindTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
