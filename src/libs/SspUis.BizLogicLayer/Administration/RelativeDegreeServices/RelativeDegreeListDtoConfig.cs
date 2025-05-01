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

namespace SspUis.BizLogicLayer.RelativeDegreeServices
{
    public class RelativeDegreeListDtoConfig : PerDtoConfig<RelativeDegreeListDto, RelativeDegree>
    {
        public override Action<IMappingExpression<RelativeDegree, RelativeDegreeListDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ShortName, x => x.MapFrom(ent => ent.Translates.AsQueryable()
                .FirstOrDefault(RelativeDegreeTranslate.GetExpr(TranslateColumn.short_name,ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ShortName)) 
            .ForMember(x => x.FullName, x => x.MapFrom(ent => ent.Translates.AsQueryable()
                .FirstOrDefault(RelativeDegreeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.FullName))
            .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
            .ForMember(x => x.Gender, x => x.MapFrom(ent => ent.Gender.Translates.AsQueryable()
                .FirstOrDefault(GenderTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Gender.FullName))
            ;
    }
}
