using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.DocumentHistoryService
{
    public class DocumentHistoryListDtoConfig : PerDtoConfig<DocumentHistoryListDto, DocumentHistory>
    {
        public override Action<IMappingExpression<DocumentHistory, DocumentHistoryListDto>> AlterReadMapping =>
           cfg => cfg
                .ForMember(x => x.Id, x => x.MapFrom(ent => ent.Id))
                .ForMember(x => x.DateAt, x => x.MapFrom(ent => ent.DateAt))
                .ForMember(x => x.UserId, x => x.MapFrom(ent => ent.UserId))
                .ForMember(x => x.UserInfo, x => x.MapFrom(ent => ent.UserInfo))
                .ForMember(x => x.TableId, x => x.MapFrom(ent => ent.TableId))
                .ForMember(x => x.DocId, x => x.MapFrom(ent => ent.DocId))
                .ForMember(x => x.StatusId, x => x.MapFrom(ent => ent.StatusId))
                .ForMember(x => x.IpAddress, x => x.MapFrom(ent => ent.IpAddress))
                .ForMember(x => x.UserAgent, x => x.MapFrom(ent => ent.UserAgent))
                .ForMember(x => x.Message, x => x.MapFrom(ent => ent.Message))
                .ForMember(x => x.OrganizationId, x => x.MapFrom(ent => ent.OrganizationId))
                .ForMember(x => x.DocContent, x => x.MapFrom(ent => ent.DocContent))
               ;
    }
}
