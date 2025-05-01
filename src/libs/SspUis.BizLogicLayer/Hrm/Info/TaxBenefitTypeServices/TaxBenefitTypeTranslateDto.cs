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

namespace SspUis.BizLogicLayer.Hrm.TaxBenefitTypeServices
{
    public class TaxBenefitTypeTranslateDto : TaxBenefitTypeTranslateDlDto, ILinkToEntity<TaxBenefitTypeTranslate>
    {
        public string Language { get; set; }
    }
    public class TaxBenefitTypeTranslateDtoConfig : PerDtoConfig<TaxBenefitTypeTranslateDto, TaxBenefitTypeTranslate>
    {
        public override Action<IMappingExpression<TaxBenefitTypeTranslate, TaxBenefitTypeTranslateDto>> AlterReadMapping => cfg => cfg
                .IncludeBase<TaxBenefitTypeTranslate, TaxBenefitTypeTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
