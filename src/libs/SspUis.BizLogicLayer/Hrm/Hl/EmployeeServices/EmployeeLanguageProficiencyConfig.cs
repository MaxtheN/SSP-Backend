using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeLanguageProficiencyDtoConfig : PerDtoConfig<EmployeeLanguageProficiencyDto, EmployeeLanguageProficiency>
{
    public override Action<IMappingExpression<EmployeeLanguageProficiency, EmployeeLanguageProficiencyDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Languagperoficiency, c => c.MapFrom(d => d.Languagperoficiency.FullName))
            .ForMember(x => x.LanguageDegrees, x => x.MapFrom(ent => ent.LanguageDegrees != null ? ent.LanguageDegrees.Translates.AsQueryable().FirstOrDefault(LanguageDegreeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.LanguageDegrees.FullName : ""))
        ;
}
