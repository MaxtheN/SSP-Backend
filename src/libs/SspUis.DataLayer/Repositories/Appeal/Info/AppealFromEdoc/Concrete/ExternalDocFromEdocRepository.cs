using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ExternalDocFromEdocRepository :
        BaseEntityRepository<int,
            ExternalDocumentFromEdoc,
            CreateExternalDocumentFromEdocDlDto,
            UpdateExternalDocumentFromEdocDlDto>,
            IExternalDocFromEdocRepository
    {
        public ExternalDocFromEdocRepository(ICrudServices crudServices) : base(crudServices)
        {

        }

        
    }
}
