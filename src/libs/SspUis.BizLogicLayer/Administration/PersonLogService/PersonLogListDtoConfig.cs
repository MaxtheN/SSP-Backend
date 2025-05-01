using AutoMapper;
using GenericServices.Configuration;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.DataLayer.EfClasses.Public.Hl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Administration.PersonLogService
{
    public class PersonLogListDtoConfig : PerDtoConfig<PersonLogListDto, PersonLog>
    {
        public override Action<IMappingExpression<PersonLog, PersonLogListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Person, x => x.MapFrom(ent => ent.Person.FullName));
    }
}
