using System;
using System.Linq;
using GenericServices.Configuration;
using AutoMapper;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.DocumentChangeLogServices
{
    public class DocumentChangeLogDtoConfig : PerDtoConfig<DocumentChangeLogDto, DocumentChangeLog>
    {
        public override Action<IMappingExpression<DocumentChangeLog, DocumentChangeLogDto>> AlterReadMapping => 
            cfg => cfg
                .ForMember(x => x.Table, x => x.MapFrom(ent => ent.Table.FullName))
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.FullName))
                ;
    }
}
