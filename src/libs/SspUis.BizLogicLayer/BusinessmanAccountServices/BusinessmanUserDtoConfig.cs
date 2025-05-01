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

namespace SspUis.BizLogicLayer.BusinessmanAccountServices
{
    public class BusinessmanUserDtoConfig : PerDtoConfig<BusinessmanAccountUserDto, BusinessmanUser>
    {
        public override Action<IMappingExpression<BusinessmanUser, BusinessmanAccountUserDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName))
                .ForMember(x => x.FullName, x => x.MapFrom(ent => ent.FullName))
                .ForMember(x => x.Inn, x => x.MapFrom(ent => ent.Inn))
                .ForMember(x => x.Pinfl, x => x.MapFrom(ent => ent.Pinfl))
                ;
    }
}
