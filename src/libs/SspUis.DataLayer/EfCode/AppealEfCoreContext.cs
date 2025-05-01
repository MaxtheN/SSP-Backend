using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Appeal;
using WEBASE.EF;

namespace SspUis.DataLayer.EfCode;

public partial class EfCoreContext : BaseDbContext
{

    #region DOC
    public virtual DbSet<AppealApplication> AppealApplications { get; set; }
    public virtual DbSet<AppealApplicationSign> AppealApplicationSigns { get; set; }

    #endregion

    #region ENUM
    public virtual DbSet<AppealType> AppealTypes { get; set; }
    public virtual DbSet<AppealFormatType> AppealFormatTypes { get; set; }
    #endregion

    #region Translates
    //public virtual DbSet<AppealTypeTranslate> AppealTypeTranslates { get; set; }
    //public virtual DbSet<AppealFormatTypeTranslate> AppealFormatTypeTranslates { get; set; }

    #endregion

    #region I N F O
    public virtual DbSet<AppealTypeArrive> AppealTypeArrives { get; set; }
    public virtual DbSet<AppealTypeArriveTranslate> AppealTypeArriveTranslates { get; set; }
    public virtual DbSet<AppealDescription> AppealDescriptions { get; set; }
    public virtual DbSet<AppealDescriptionTranslate> AppealDescriptionTranslates { get; set; }
    public virtual DbSet<ExternalDocumentFromEdoc> ExternalDocuments { get; set; }

    #endregion



}
