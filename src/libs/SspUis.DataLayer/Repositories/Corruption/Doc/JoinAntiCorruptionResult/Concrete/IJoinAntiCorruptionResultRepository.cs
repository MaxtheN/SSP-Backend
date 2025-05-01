using SspUis.DataLayer.EfClasses.Corruption;
using System;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Corruption;
public interface IJoinAntiCorruptionResultRepository : IBaseEntityRepository<long, JoinAntiCorruptionResult, CreateJoinAntiCorruptionResultDlDto, UpdateJoinAntiCorruptionResultDlDto, UpdateStatusJoinAntiCorruptionResultDlDto>
{
}
