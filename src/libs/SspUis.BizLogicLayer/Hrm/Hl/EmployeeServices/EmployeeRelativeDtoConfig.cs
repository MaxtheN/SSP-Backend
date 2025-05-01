using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeRelativeDtoConfig : PerDtoConfig<EmployeeRelativeDto, EmployeeRelative>
{
    public override Action<IMappingExpression<EmployeeRelative, EmployeeRelativeDto>> AlterReadMapping => 
        cfg=>cfg
            .ForMember(d=>d.Nationality,c=>c.MapFrom(d=>d.Nationality.FullName))
            .ForMember(d=>d.Citizenship,c=>c.MapFrom(d=>d.Citizenship.FullName))
            .ForMember(d=>d.Country,c=>c.MapFrom(d=>d.Country.FullName))
            .ForMember(d=>d.Region,c=>c.MapFrom(d=>d.Region.FullName))
            .ForMember(d=>d.District,c=>c.MapFrom(d=>d.District.FullName))
        .ForMember(x => x.RelativeDegree, x => x.MapFrom(ent => ent.RelativeDegree.Translates.AsQueryable()
                .FirstOrDefault(RelativeDegreeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.RelativeDegree.FullName))
        ;
}
