using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.ContractorServices;

public class ContractorContactDtoConfig : PerDtoConfig<ContractorContactDto, ContractorContact>
{
    public override Action<IMappingExpression<ContractorContact, ContractorContactDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.ContactType, c => c.MapFrom(d => d.ContactType.Translates.AsQueryable()
                .FirstOrDefault(
                    ContactTypeTranslate.GetExpr(
                        TranslateColumn.full_name,
                        ServiceProvider.CultureHelper.CurrentCulture.Id))
                .TranslateText ?? d.ContactType.FullName))
        .ForMember(d=>d.OwnerId, c=>c.MapFrom(ent=>ent.OwnerId));
}
