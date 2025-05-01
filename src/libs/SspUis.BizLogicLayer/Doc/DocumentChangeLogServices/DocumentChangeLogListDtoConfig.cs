using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;
using WEBASE.DependencyInjection;

namespace SspUis.BizLogicLayer.DocumentChangeLogServices
{
    public class DocumentChangeLogListDtoConfig : PerDtoConfig<DocumentChangeLogListDto, DocumentChangeLog>
    {
        public override Action<IMappingExpression<DocumentChangeLog, DocumentChangeLogListDto>> AlterReadMapping =>
           cfg => cfg
                //.ForMember(x => x.Table, x => x.MapFrom(ent => ent.Table.Translates.AsQueryable().FirstOrDefault(TableTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Table.FullName))
                .ForMember(x => x.Table, x => x.MapFrom(ent => ent.Table.FullName))
                .ForMember(x => x.Message, x => x.MapFrom(ent => ent.Message ?? ent.Application.MemshipContract.Message))
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable().FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
               ;
    }
}
