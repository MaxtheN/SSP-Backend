using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Memship;

public interface IDebtService : IBaseEntityService<long, Debt, DebtListDto, DebtDto, CreateDebtDlDto, UpdateDebtDlDto, DebtSortFilterOption>
{
}
