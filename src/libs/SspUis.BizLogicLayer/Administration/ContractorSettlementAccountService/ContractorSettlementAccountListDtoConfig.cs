using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.BizLogicLayer.Administration.ContractorSettlementAccountService;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Administration.ContractorSettlementAccountService
{
    public class ContractorSettlementAccountListDtoConfig : PerDtoConfig<ContractorSettlementAccountListDto, ContractorSettlementAccount>
    {
        public override Action<IMappingExpression<ContractorSettlementAccount, ContractorSettlementAccountListDto>> AlterReadMapping =>
            cfg => cfg
            .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.FullName))
            .ForMember(x => x.Bank, x => x.MapFrom(ent => ent.Bank.BankName));
    }
}
