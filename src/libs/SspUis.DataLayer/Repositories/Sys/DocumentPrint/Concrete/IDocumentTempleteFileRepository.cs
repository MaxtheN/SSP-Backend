using System;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IDocumentTempleteFileRepository : IBaseEntityRepository<Guid, DocumentTempleteFile, CreateDocumentTempleteFileDlDto, UpdateDocumentTempleteFileDlDto>
{
}
