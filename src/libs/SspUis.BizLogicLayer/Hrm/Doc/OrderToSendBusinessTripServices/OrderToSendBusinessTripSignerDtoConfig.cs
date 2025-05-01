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

namespace SspUis.BizLogicLayer.Hrm
{
    public class OrderToSendBusinessTripSignerConfig : PerDtoConfig<OrderToSendBusinessTripSignerDto, OrderToSendBusinessTripSigner>
    {
        public override Action<IMappingExpression<OrderToSendBusinessTripSigner, OrderToSendBusinessTripSignerDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Department, x => x.MapFrom(ent => ent.Department.FullName))
                .ForMember(x => x.Position, x => x.MapFrom(ent => ent.Position.FullName))
                .ForMember(x => x.ForQrCodePosition, x => x.MapFrom(ent => ent.Position.FullName))
                .ForMember(x => x.IsDirector, x => x.MapFrom(ent => ent.IsDirector))
                .ForMember(x => x.IsHr, x => x.MapFrom(ent => ent.IsHr))
                .ForMember(x => x.DocNumber, x => x.MapFrom(ent => ent.Owner.DocNumber))
                .ForMember(x => x.DocDate, x => x.MapFrom(ent => ent.Owner.DocOn))
                .ForMember(x => x.Employee, x => x.MapFrom(ent => ent.EmployeeManage.Employee.Person.FullName))
            ;

    }
}
