using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.EfCode
{
    public partial class EfCoreContext
    {
        #region INFO
        public virtual DbSet<NeedChamberServiceGroup> NeedChamberServiceGroups { get; set; }

        #endregion

        #region TRANSLATES
        public virtual DbSet<NeedChamberServiceGroupTranslate> NeedChamberServiceGroupTranslate { get; set; }

        #endregion
    }
}
