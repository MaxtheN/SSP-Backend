
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.EfCode;

public partial class EfCoreContext : BaseDbContext
{
   
    #region SYS
    public virtual DbSet<IntegrationApiAddress> IntegrationApiAddresses { get; set; }
    public virtual DbSet<IntegrationApiTestLog> IntegrationApiTestLogs { get;set; }
    #endregion
    public virtual DbSet<FinancePayDocsByAcc> FinancePayDocsByAccs { get; set; }
}
