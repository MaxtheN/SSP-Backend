using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.Hrm.QualificationCategoryServices
{
    public class QualificationCategoryListDtoConfig : PerDtoConfig<QualificationCategoryListDto, QualificationCategory>
    {
        public override Action<IMappingExpression<QualificationCategory, QualificationCategoryListDto>> AlterReadMapping => cfg => cfg
                .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable().FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
        ;
    }
}
