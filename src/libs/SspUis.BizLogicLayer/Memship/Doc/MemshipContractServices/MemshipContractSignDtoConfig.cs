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

namespace SspUis.BizLogicLayer.MemshipContractServices
{
    public class MemshipContractSignDtoConfig : PerDtoConfig<MemshipContractSignDto, MemshipContractSign>
    {
        public override Action<IMappingExpression<MemshipContractSign, MemshipContractSignDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.MemshipContractTypeId, x => x.MapFrom(ent => ent.Owner.MemshipContractTypeId))
                .ForMember(x => x.OrganizationId, x => x.MapFrom(ent => ent.Owner.OrganizationId));
    }
}
