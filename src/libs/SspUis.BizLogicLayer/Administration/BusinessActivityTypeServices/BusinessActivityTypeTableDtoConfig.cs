using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.BusinessActivityTypeServices
{
    public class BusinessActivityTypeTableDtoConfig : PerDtoConfig<BusinessActivityTypeTableDto, BusinessActivityTypeTable>
    {
        public override Action<IMappingExpression<BusinessActivityTypeTable, BusinessActivityTypeTableDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Currency, x => x.MapFrom(ent => ent.Currency.Translates.AsQueryable()
                     .FirstOrDefault(CurrencyTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Currency.FullName));
    }
}
