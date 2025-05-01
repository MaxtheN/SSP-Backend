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

namespace SspUis.BizLogicLayer.OrganizationLegalFormServices
{
    public class OrganizationLegalFormListDtoConfig : PerDtoConfig<OrganizationLegalFormListDto, OrganizationLegalForm>
    {
        public override Action<IMappingExpression<OrganizationLegalForm, OrganizationLegalFormListDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ShortName, x => x.MapFrom(ent => ent.Translates.AsQueryable()
                .FirstOrDefault(OrganizationLegalFormTranslate.GetExpr(TranslateColumn.short_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ShortName))
            .ForMember(x => x.FullName, x => x.MapFrom(ent => ent.Translates.AsQueryable()
                .FirstOrDefault(OrganizationLegalFormTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.FullName))
            .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName));
    }
}
