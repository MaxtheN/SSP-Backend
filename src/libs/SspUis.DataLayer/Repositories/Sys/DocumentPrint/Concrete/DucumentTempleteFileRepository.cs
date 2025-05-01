using System;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class DucumentTempleteFileRepository : BaseEntityRepository<Guid, DocumentTempleteFile, CreateDocumentTempleteFileDlDto, UpdateDocumentTempleteFileDlDto>, IDocumentTempleteFileRepository
{
    public DucumentTempleteFileRepository(ICrudServices crudServices) : base(crudServices)
    {
    }

    protected override void OnCreate(DocumentTempleteFile entity, CreateDocumentTempleteFileDlDto dto)
    {
        //entity.Id = Guid.NewGuid();
    }
}
