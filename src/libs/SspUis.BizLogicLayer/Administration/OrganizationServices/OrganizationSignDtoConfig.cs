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
    public class OrganizationSignDtoConfig : PerDtoConfig<OrganizationSignDto, OrganizationSign>
    {
        public override Action<IMappingExpression<OrganizationSign, OrganizationSignDto>> AlterReadMapping =>
           cfg => cfg
               .ForMember(x => x.PrtnContractTypeTablePosition, x => x.MapFrom(ent => ent.PrtnContractTypeTable.Position.Translates.AsQueryable()
                    .FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnContractTypeTable.Position.FullName))
                .ForMember(x => x.PhoneNumber, x => x.MapFrom(ent => ent.User.PhoneNumber))
               ;
    }
}
