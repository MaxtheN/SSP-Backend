using Microsoft.EntityFrameworkCore;
using Ssp.DataLayer.EFClasses.Edoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;

namespace SspUis.DataLayer.EfCode
{
    public partial class EfCoreContext : BaseDbContext
    {
        public virtual DbSet<User> EdocUsers { get; set; }
        public virtual DbSet<Organization> EdocOrganizations { get; set; }
        public virtual DbSet<Position> EdocPositions { get; set; }
        public virtual DbSet<Department> EdocDepartments { get; set; }
        public virtual DbSet<Bank> EdocBanks { get; set; }
        public virtual DbSet<Employee> EdocEmployees { get; set; }
        public virtual DbSet<EmployeePassport> EdocEmployeePassports { get; set; }
        public virtual DbSet<AppointmentOrder> EdocAppointmentOrders { get; set; }
        public virtual DbSet<Appointment> EdocAppointments { get; set; }
    }
}
