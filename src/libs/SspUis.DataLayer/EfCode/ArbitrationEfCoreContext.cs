using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.EfCode;

public partial class EfCoreContext : BaseDbContext
{
    #region   Doc
    public virtual DbSet<ArbitrationCourtApplication> ArbArbitrationCourtApplications { get; set; }
    public virtual DbSet<ArbitrationCourtApplicationFile> ArbitrationCourtApplicationFiles { get; set; }
    public virtual DbSet<ArbitrationCourtApplicationSigner> ArbitrationCourtApplicationSigners { get; set; }
    public virtual DbSet<ArbitrationResult> ArbitrationResults { get; set; }
    public virtual DbSet<ArbitrationResultSign> ArbitrationResultSigns { get; set; }
    public virtual DbSet<ArbitrationResultFile> ArbitrationResultFiles { get; set; }
    public virtual DbSet<ArbitrationDiscussion> ArbitrationDiscussions { get; set; }
    public virtual DbSet<ArbitrationDiscussionSign> ArbitrationDiscussionSigns { get; set; }
	public virtual DbSet<ArbitrationDiscussionFile> ArbitrationDiscussionFiles { get; set; }
    public virtual DbSet<ArbitrationDelay> ArbitrationDelays { get; set; }
    public virtual DbSet<ArbitrationDelaySign> ArbitrationDelaySigns { get; set; }
	public virtual DbSet<ArbitrationDelayFile> ArbitrationDelayFiles { get; set; }
	#endregion

	#region  Info
	public virtual DbSet<ArbitrationJudge> ArbitrationJudge { get; set; }
    #endregion
}
