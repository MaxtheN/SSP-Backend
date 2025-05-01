using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.Models;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.EmployeeManageServices
{
    public class EmployeeCheckForSingDto
    {
        public List<long?> EmployeeLeaveOrderIds { get; set; }
        public List<long?> EmployeeBusinessTripIds { get; set; }
        public List<long?> EmployeeSickLeaveIds { get; set; }
    }
}
