using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.DocumentHistoryServices
{
    public class DocumentJobHistoryListDtoConfig : PerDtoConfig<DocumentJobHistoryListDto, DocumentJobHistory>
    {
        public override Action<IMappingExpression<DocumentJobHistory, DocumentJobHistoryListDto>> AlterReadMapping =>
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
