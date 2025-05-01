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
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.LandingPageDatumServices
{
    public class LandingPageDatumDtoConfig : PerDtoConfig<LandingPageDatumDto, LandingPageDatum>
    {
        public override Action<IMappingExpression<LandingPageDatum, LandingPageDatumDto>> AlterReadMapping => cfg => cfg
        .ForMember(x => x.Label, x => x.MapFrom(ent => ent.Translates.AsQueryable()
                    .FirstOrDefault(LandingPageDatumTranslate.GetExpr(LandingPageDatumTranslateColumn.label, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Label))
            .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
            .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName));
    }
}
