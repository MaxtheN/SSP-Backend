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
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class LanguageProficiencyListDtoConfig : PerDtoConfig<LanguageProficiencyListDto, LanguageProficiency>
{
    public override Action<IMappingExpression<LanguageProficiency, LanguageProficiencyListDto>> AlterReadMapping => cfg => cfg
        .ForMember(x => x.ShortName, x => x.MapFrom(ent => ent.Translates.AsQueryable()
            .FirstOrDefault(LanguageProficiencyTranslate.GetExpr(TranslateColumn.short_name,ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ShortName)) 
        .ForMember(x => x.FullName, x => x.MapFrom(ent => ent.Translates.AsQueryable()
            .FirstOrDefault(LanguageProficiencyTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.FullName))
        .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
            .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
        ;
}
