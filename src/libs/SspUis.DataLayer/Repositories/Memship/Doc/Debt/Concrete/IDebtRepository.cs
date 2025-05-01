using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IDebtRepository : IBaseEntityRepository<long, Debt, CreateDebtDlDto, UpdateDebtDlDto, UpdateStatusDebtDlDto>
    {
    }
}
