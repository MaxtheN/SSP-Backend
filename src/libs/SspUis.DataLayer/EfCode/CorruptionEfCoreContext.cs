using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Corruption;

namespace SspUis.DataLayer.EfCode;

public partial class EfCoreContext
{
    #region ENUM
    public virtual DbSet<CorruptionReviewType> CorruptionReviewTypes { get; set; }
    public virtual DbSet<JoinAntiCorruptionResultType> JoinAntiCorruptionResultTypes { get; set; }
    #endregion

    #region INFO
    public virtual DbSet<ContractorUnionActivityType> ContractorUnionActivityTypes { get; set; }

    #endregion

    #region TRANSLATES
    public virtual DbSet<CorruptionReviewTypeTranslate> CorruptionReviewTypeTranslates { get; set; }
    public virtual DbSet<JoinAntiCorruptionResultTypeTranslate> JoinAntiCorruptionResultTypeTranslates { get; set; }
    public virtual DbSet<ContractorUnionActivityTypeTranslate> ContractorUnionActivityTypeTranslates { get; set; }
    #endregion

    #region DOC
    public virtual DbSet<JoinAntiCorruptionApplication> JoinAntiCorruptionApplications { get; set; }
    public virtual DbSet<JoinAntiCorruptionApplicationEmployee> JoinAntiCorruptionApplicationEmployees { get; set; }
    public virtual DbSet<JoinAntiCorruptionApplicationFile> JoinAntiCorruptionApplicationFiles { get; set; }
    public virtual DbSet<JoinAntiCorruptionApplicationParticipate> JoinAntiCorruptionApplicationParticipates { get; set; }
    public virtual DbSet<JoinAntiCorruptionApplicationTable> JoinAntiCorruptionApplicationTables { get; set; }
    public virtual DbSet<JoinAntiCorruptionCertificate> JoinAntiCorruptionCertificates { get; set; }
    public virtual DbSet<JoinAntiCorruptionResult> JoinAntiCorruptionResults { get; set; }
    public virtual DbSet<JoinAntiCorruptionResultFile> JoinAntiCorruptionResultFiles { get; set; }
    public virtual DbSet<JoinAntiCorruptionResultTable> JoinAntiCorruptionResultTables { get; set; }
    #endregion
}
