using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
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

namespace SspUis.BizLogicLayer.AppointEmployeeServices
{
    public class AppointEmployeeDtoConfig : PerDtoConfig<AppointEmployeeSignDto, AppointEmployeeSign>
    {
        public override Action<IMappingExpression<AppointEmployeeSign, AppointEmployeeSignDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.OrganizationId, x => x.MapFrom(ent => ent.Owner.OrganizationId))
            ;

    }
}
