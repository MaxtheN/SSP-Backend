using System;
using System.Linq;
using GenericServices.Configuration;
using AutoMapper;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.DocumentHistoryServices
{
    public class DocumentJobHistoryDtoConfig : PerDtoConfig<DocumentJobHistoryDto, DocumentJobHistory>
    {
        public override Action<IMappingExpression<DocumentJobHistory, DocumentJobHistoryDto>> AlterReadMapping => 
            cfg => cfg
                .ForMember(x => x.Table, x => x.MapFrom(ent => ent.Table.FullName))
                .ForMember(x => x.FromStatus, x => x.MapFrom(ent => ent.FromStatus.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.FromStatus.FullName))
                .ForMember(x => x.ToStatus, x => x.MapFrom(ent => ent.ToStatus.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ToStatus.FullName))
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))

        ;
    }
}
