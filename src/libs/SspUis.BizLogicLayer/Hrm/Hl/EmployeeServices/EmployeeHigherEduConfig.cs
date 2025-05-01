using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeHigherEduDtoConfig : PerDtoConfig<EmployeeHigherEduDto, EmployeeHigherEdu>
{
    public override Action<IMappingExpression<EmployeeHigherEdu, EmployeeHigherEduDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Specialty, c => c.MapFrom(d => d.Specialty.FullName))
            .ForMember(d => d.Institute, c => c.MapFrom(d => d.Institute.FullName))
            .ForMember(x => x.EmployeeHigherEduDegree, x => x.MapFrom(ent => ent.EmployeeHigherEduDegrees.Translates.AsQueryable()
                .FirstOrDefault(EmployeeHigherEduDegreeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.EmployeeHigherEduDegrees.FullName))
        ;
}
