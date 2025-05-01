using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.BizLogicLayer.Administration.ContractorContactService;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.ContractorContactService
{
    public class ContractorContactListDtoConfig : PerDtoConfig<ContractorContactListDto, ContractorContact>
    {
        public override Action<IMappingExpression<ContractorContact, ContractorContactListDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.ContactType, c => c.MapFrom(d => d.ContactType.Translates.AsQueryable()
                .FirstOrDefault(
                    ContactTypeTranslate.GetExpr(
                        TranslateColumn.full_name,
                        ServiceProvider.CultureHelper.CurrentCulture.Id))
                .TranslateText ?? d.ContactType.FullName))
        .ForMember(d => d.OwnerId, c => c.MapFrom(ent => ent.OwnerId));
    }
}
