using AutoMapper;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.OrganizationServices
{
    public class OrganizationSettlementAccountDtoConfig : PerDtoConfig<OrganizationSettlementAccountDto, OrganizationSettlementAccount>
    {
        public override Action<IMappingExpression<OrganizationSettlementAccount, OrganizationSettlementAccountDto>> AlterReadMapping =>
           cfg => cfg
               .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                    .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
               .ForMember(x => x.Bank, x => x.MapFrom(ent => ent.Bank.Translates.AsQueryable()
                    .FirstOrDefault(BankTranslate.GetExpr(BankTranslateColumn.bank_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Bank.BankName))
               .ForMember(x => x.BankCode, x => x.MapFrom(ent => ent.Bank.Code))
               ;
    }
}
