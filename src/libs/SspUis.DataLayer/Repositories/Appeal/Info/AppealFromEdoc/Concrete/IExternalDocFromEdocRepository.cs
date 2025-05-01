using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IExternalDocFromEdocRepository :
        IBaseEntityRepository<int,
            ExternalDocumentFromEdoc,
            CreateExternalDocumentFromEdocDlDto,
            UpdateExternalDocumentFromEdocDlDto>
    {

    }
}
