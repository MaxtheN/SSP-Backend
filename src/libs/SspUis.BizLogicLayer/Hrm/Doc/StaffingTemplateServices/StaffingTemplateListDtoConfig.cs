using System;
using System.Linq;
using GenericServices.Configuration;
using AutoMapper;
using System.Collections.Generic;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;

namespace SspUis.BizLayer.Hrm.StaffingTemplateServices
{
    public class StaffingTemplateListDtoConfig : PerDtoConfig<StaffingTemplateListDto, StaffingTemplate>
    {
        public override Action<IMappingExpression<StaffingTemplate, StaffingTemplateListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable().FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
            ;
    }
}
