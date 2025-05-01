using GenericServices;
using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using GenericServices.Configuration;
using AutoMapper;
using WEBASE.Utility;
using SspUis.DataLayer;
using WEBASE;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer;

public class EmployeeDtoConfig : PerDtoConfig<EmployeeDto, Employee>
{
    public override Action<IMappingExpression<Employee, EmployeeDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
            .ForMember(x => x.PlaceOfWorks, x => x.MapFrom(ent => ent.PlaceOfWorks.Where(p => p.IsImported == false)))
            .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
            //.ForMember(x => x.IsPassedAttestation, x => x.MapFrom(ent => ent.Attestations.OrderByDescending(a => a.CreatedAt).Any(a => a.ExpirationDate > DateTime.UtcNow && a.OrderTypeId == OrderTypeConst.PASSED)))
            .ForMember(x => x.CertificateNumber, x => x.Ignore())
            .ForMember(x => x.ExpirationDate, x => x.Ignore())
            .ForMember(x => x.OrderTypeId, x => x.Ignore())
            .ForMember(x => x.OrderType, x => x.Ignore())
            .ForMember(x => x.CreatedUser, x => x.MapFrom(a => a.CreatedUser.Person.FullName))
            .ForMember(x => x.CreatedUserName, x => x.MapFrom(a => a.CreatedUser.UserName))
            //.ForMember(x => x.User, x => x.Ignore())
            ;
}
