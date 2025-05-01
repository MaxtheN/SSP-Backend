using System;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public interface IDocumentPrintService
    : IBaseEntityService<
        Guid,
        DocumentTempleteFile,
        DocumentTempleteFileDto,
        DocumentTempleteFileDto,
        CreateDocumentTempleteFileDlDto,
        UpdateDocumentTempleteFileDlDto>
{
    SelectList<int> SelectList();
    ValueTask<byte[]> GenerateWord(int tableId);
    CreateDocumentTempleteFileDlDto UploadTemplateFile(StorageFile file);
    void SaveTemplate(CreateDocumentTempleteFileDlDto dto);
    ValueTask<byte[]> Print<TEntity, TId>(TEntity model)
        where TEntity : class, IHaveIdProp<TId>
        where TId : struct;
}
