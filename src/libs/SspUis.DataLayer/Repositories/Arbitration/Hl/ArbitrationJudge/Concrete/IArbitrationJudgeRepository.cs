using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IArbitrationJudgeRepository
        : IBaseEntityRepository<int,
            ArbitrationJudge,
            CreateArbitrationJudgeDlDto,
            UpdateArbitrationJudgeDlDto>
    {
    }
}
