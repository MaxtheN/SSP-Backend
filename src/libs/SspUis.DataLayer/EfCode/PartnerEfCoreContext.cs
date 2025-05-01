using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.EfClasses.Integration.Bojxona;

namespace SspUis.DataLayer.EfCode
{
    public partial class EfCoreContext : BaseDbContext
    {
        public virtual DbSet<PrtnCertificate> PrtnCertificates { get; set; }
        public virtual DbSet<PrtnApplication> PrtnApplications { get; set; }
        public virtual DbSet<PrtnContract> PrtnContracts { get; set; }
        public virtual DbSet<PrtnContractType> PrtnContractTypes { get; set; }
        public virtual DbSet<PrtnRejectReason> PrtnRejectReasons { get; set; }
        public virtual DbSet<PrtnCreditDemand> PrtnCreditDemands { get; set; }
        public virtual DbSet<BojxonaImtiyoz> BojxonaImtiyozs { get; set; }
        public virtual DbSet<ExecutionApplication> ExecutionApplications { get; set; }
        public virtual DbSet<ExecutionApplicationTable> ExecutionApplicationTables { get; set; }

        #region Translates
        public virtual DbSet<PrtnRejectReasonTranslate> PrtnRejectReasonTranslates { get; set; }
        public virtual DbSet<PrtnContractTypeTranslate> PrtnContractTypeTranslates { get; set; }
        #endregion

    }
}
