using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class PrtnCertificateSignRepository : BaseEntityRepository<long, PrtnCertificateSign>, IPrtnCertificateSignRepository
    {

        public PrtnCertificateSignRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }
        public PrtnCertificateSign CreateSignHistory(PrtnCertificatesignDlDto dto)
        {
            PrtnCertificateSign entity = dto.CreateEntity();
            Context.Set<PrtnCertificateSign>().Add(entity);
            Context.Entry(entity).State = EntityState.Added;

            if (entity == null)
                return null;

            Context.SaveChanges();

            return entity;
        }
    }
}
