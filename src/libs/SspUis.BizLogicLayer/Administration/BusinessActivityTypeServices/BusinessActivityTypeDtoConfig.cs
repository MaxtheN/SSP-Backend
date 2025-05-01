using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.BusinessActivityTypeServices
{
    public class BusinessActivityTypeDtoConfig : PerDtoConfig<BusinessActivityTypeDto, BusinessActivityType>
    {
        public override Action<IMappingExpression<BusinessActivityType, BusinessActivityTypeDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
                .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn))
                .ForMember(x => x.BankCode, x => x.MapFrom(ent => ent.Bank.Code))
                .ForMember(x => x.BusinessCtorName, x => x.MapFrom(ent => ent.BusinessCtor.FullName))
                .ForMember(x => x.Bank, x => x.MapFrom(ent => ent.Bank.Translates.AsQueryable()
                    .FirstOrDefault(BankTranslate.GetExpr(BankTranslateColumn.bank_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Bank.BankName));
    }
}
