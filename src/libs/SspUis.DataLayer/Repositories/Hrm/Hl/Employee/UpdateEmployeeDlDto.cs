using System;
using AutoMapper;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateEmployeeDlDto : EmployeeDlDto<UpdateEmployeeDlDto>, IHaveIdProp<int>
    {
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int StateId { get; set; }
        public new int PersonId { get => base.PersonId; set => base.PersonId = value; }

        public UpdatePersonDlDto Person { get; set; }

        protected override Action<IMappingExpression<UpdateEmployeeDlDto, Employee>> AlterMapping =>
            cfg =>
            {
                base.AlterMapping(cfg);
                cfg
                .ForMember(x => x.Person, c => c.Ignore())
                ;
            };
    }
}
