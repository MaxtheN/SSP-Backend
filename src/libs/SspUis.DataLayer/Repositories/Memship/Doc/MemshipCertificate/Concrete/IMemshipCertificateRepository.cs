using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IMemshipCertificateRepository 
	: IBaseEntityRepository<long, MemshipCertificate, 
		CreateMemshipCertificateDlDto, 
		UpdateMemshipCertificateDlDto,
		UpdateStatusMemshipCertificateDlDto>
{
}
