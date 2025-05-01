using GenericServices;
using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using GenericServices.Configuration;
using AutoMapper;
using WEBASE.Utility;
using SspUis.DataLayer;
using WEBASE;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.PrtnContractServices
{
    public class PrtnContractSignDtoConfig : PerDtoConfig<PrtnContractSignDto, PrtnContractSign>
    {
        public override Action<IMappingExpression<PrtnContractSign, PrtnContractSignDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
                .ForMember(x => x.SignOrganizationTypeId, x => x.MapFrom(ent => ent.PrtnContractTypeTable.SignOrganizationTypeId))
                .ForMember(x => x.SignOrganizationTypeId, x => x.MapFrom(ent => ent.PrtnContractTypeTable.SignOrganizationTypeId))
                .ForMember(x => x.OrganizationSignPinfl, x => x.MapFrom(ent => ent.OrganizationSign.Pinfl))
            ;

    }
}
