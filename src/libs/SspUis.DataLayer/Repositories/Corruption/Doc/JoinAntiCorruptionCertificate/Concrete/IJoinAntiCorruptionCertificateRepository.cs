using SspUis.DataLayer.EfClasses.Corruption;
using System;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Corruption;
public interface IJoinAntiCorruptionCertificateRepository : IBaseEntityRepository<long, JoinAntiCorruptionCertificate, CreateJoinAntiCorruptionCertificateDlDto, UpdateJoinAntiCorruptionCertificateDlDto>
{
}
