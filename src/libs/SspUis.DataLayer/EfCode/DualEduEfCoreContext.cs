using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.DualEdu;
using WEBASE.EF;

namespace SspUis.DataLayer.EfCode;

public partial class EfCoreContext : BaseDbContext
{
    #region DOC
    public virtual DbSet<DualApplication> DualApplications { get; set; }
    public virtual DbSet<DualContract> DualContracts { get; set; }
    public virtual DbSet<DualApplicationTable> DualApplicationTables { get; set; }
    public virtual DbSet<SubsidyRequest> SubsidyRequests { get; set; }
    public virtual DbSet<SubsidyRequestTable> SubsidyRequestTables { get; set; }
    public virtual DbSet<SubsidyRequestFile> SubsidyRequestFiles { get; set; }
    #endregion

    #region ENUM
    #endregion

    #region INFO
    public virtual DbSet<DualEducationType> DualEducationTypes { get; set; }
    public virtual DbSet<Institute> Institutes { get; set; }
    public virtual DbSet<InstituteBilling> InstituteBillings { get; set; }
    public virtual DbSet<Specialty> Specialtys { get; set; }
    public virtual DbSet<SpecialtyBilling> SpecialtyBillings { get; set; }
    #endregion

    #region Translates
    public virtual DbSet<DualEducationTypeTranslate> DualEducationTypeTranslates { get; set; }
    public virtual DbSet<InstituteTranslate> InstituteTranslates { get; set; }
    public virtual DbSet<SpecialtyTranslate> SpecialtyTranslates { get; set; }
    #endregion
}