using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateEmployeeIsHrDlDto : EntityDto<UpdateEmployeeIsHrDlDto, Employee>
    {
        public int Id { get; set; }

        public override void UpdateEntity(Employee entity)
        {
            base.UpdateEntity(entity);
        }
    }
}
