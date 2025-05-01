using AutoMapper;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using System.Xml.Linq;

namespace SspUis.BizLogicLayer.Hrm
{
    public class EmployeeMissedDayDtoConfig : PerDtoConfig<EmployeeMissedDayDto, EmployeeMissedDay>
    {
        public override Action<IMappingExpression<EmployeeMissedDay, EmployeeMissedDayDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
                .ForMember(x => x.Department, x => x.MapFrom(ent => ent.Department.Translates.AsQueryable()
                    .FirstOrDefault(DepartmentTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
                .ForMember(x => x.Position, x => x.MapFrom(ent => ent.Position.Translates.AsQueryable()
                    .FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
            ;

    }
}
