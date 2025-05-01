using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;

namespace SspUis.BizLogicLayer
{
    public class DocumentChatListDtoConfig : PerDtoConfig<DocumentChatListDto, DocumentChat>
    {
        public override Action<IMappingExpression<DocumentChat, DocumentChatListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Table, x => x.MapFrom(ent => ent.Table.FullName))
                .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
                .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.FullName))
                .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.FullName))
                .ForMember(x => x.App, x => x.MapFrom(ent => ent.App.FullName));
    }
}
