using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer;

public class EmployeeListDtoConfig : PerDtoConfig<EmployeeListDto, Employee>
{
    public override Action<IMappingExpression<Employee, EmployeeListDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
            .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
            .ForMember(x => x.FullName, x => x.MapFrom(ent => ent.Person.FullName))
            .ForMember(x => x.Pinfl, x => x.MapFrom(ent => ent.Person.Pinfl))
            .ForMember(x => x.RegionId, x => x.MapFrom(ent => ent.Person.LivingRegionId))
            .ForMember(x => x.DistrictId, x => x.MapFrom(ent => ent.Person.LivingDistrictId))
            .ForMember(x => x.BirthDate, x => x.MapFrom(ent => ent.Person.BirthDate))
            .ForMember(x => x.TotalWorkedYear, x => x.MapFrom(ent => ent.MehnatWorkedYear /*+ ent.SspWorkedYear*/))
            .ForMember(x => x.TotalWorkedMonth, x => x.MapFrom(ent => ent.MehnatWorkedMonth /*+ ent.SspWorkedMonth*/))
            .ForMember(x => x.TotalWorkedDay, x => x.MapFrom(ent => ent.MehnatWorkedDay/* + ent.SspWorkedDay*/))
            .ForMember(x => x.PictureId, x => x.MapFrom(ent => ent.Person.PictureId))
            .ForMember(x => x.PassportInfo, x => x.MapFrom(ent => ent.Person.PassportSeria + " " + ent.Person.PassportNumber))
            .ForMember(x => x.Region, x => x.MapFrom(ent => ent.Person.LivingRegion.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Person.LivingRegion.FullName))
            .ForMember(x => x.District, x => x.MapFrom(ent => ent.Person.LivingDistrict.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Person.LivingDistrict.FullName));
}