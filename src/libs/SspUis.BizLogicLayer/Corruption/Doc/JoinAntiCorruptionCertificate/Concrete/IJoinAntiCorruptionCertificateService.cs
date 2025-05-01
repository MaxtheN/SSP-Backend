using System;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses.Corruption;
using SspUis.DataLayer.Repositories.Corruption;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Corruption
{

    public interface IJoinAntiCorruptionCertificateService : IBaseEntityService<long, JoinAntiCorruptionCertificate, JoinAntiCorruptionCertificateListDto, JoinAntiCorruptionCertificateDto, CreateJoinAntiCorruptionCertificateDlDto, UpdateJoinAntiCorruptionCertificateDlDto, JoinAntiCorruptionCertificateSortFilterOptions>
    {
        PagedResult<JoinAntiCorruptionCertificateListDto> GetList(JoinAntiCorruptionCertificateSortFilterOptions dto);
        JoinAntiCorruptionCertificateDto Get(long id);
        ValueTask<byte[]> DownloadPdf(Guid id2, string? lang);
    }
}
