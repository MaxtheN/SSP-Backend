using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.DocxToPdf;
using WEBASE.Models;
using WEBASE.OfficeTools.Attributes;
using WEBASE.OfficeTools.Factory;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public class DocumentPrintService : BaseEntityService
    <Guid,
    DocumentTempleteFile,
    DocumentTempleteFileDto,
    DocumentTempleteFileDto,
    CreateDocumentTempleteFileDlDto,
    UpdateDocumentTempleteFileDlDto,
    IDocumentTempleteFileRepository>, IDocumentPrintService
{
    private readonly IStorageService _storageService;
    private readonly IConvertService _pdfConverter;

    public DocumentPrintService(
        IUnitOfWork unitOfWork,
        IConvertService pdfConverter,
        IStorageService storageService)
        : base(unitOfWork)
    {
        this._pdfConverter = pdfConverter;
        this._storageService = storageService;
    }
    public SelectList<int> SelectList()
    {
        var result = new SelectList<int>();
        if (true)
        {
            var types = typeof(DocumentPrintService).Assembly.GetTypes()
                .Where(a => a.GetCustomAttribute(typeof(PrintableModelAttribute)) != null)
                .Select(a => new
                {
                    Type = a,
                    Attr = a.GetCustomAttribute<PrintableModelAttribute>()
                })
                .ToArray();
            if (DocumentPrintingCache.PrintableModels == null)
            {
                DocumentPrintingCache.PrintableModels = new();
                foreach (var type in types)
                {
                    var model = new PrintableModelAttribute(type.Attr.Text, type.Attr.TableId);
                    model.ModelType = type.Type;
                    DocumentPrintingCache.PrintableModels.Add(model);
                }
            }
            //types[0].Attr.Text;
        }
        return DocumentPrintingCache.PrintableModels.AsSelectList();
    }

    public async ValueTask<byte[]> GenerateWord(int tableId)
    {
        var type = DocumentPrintingCache.PrintableModels
            .FirstOrDefault(x => x.TableId == tableId)
            .ModelType;
        var dto = Activator.CreateInstance(type);
        var file = WordFactory.GenerateWordTemplate(dto);
        return file;
    }
    public CreateDocumentTempleteFileDlDto UploadTemplateFile(StorageFile file)
    {
        var fileInfo = _storageService.SaveTemp(
                DocumentStorageConst.SYS_DOCUMENT_TEMPLETE, file).FirstOrDefault();
        var dto =
        new CreateDocumentTempleteFileDlDto()
        {
            Id = fileInfo.FileId,
            FileName = fileInfo.FileName,
            FileExtension = Path.GetExtension(fileInfo.FileName)
        };
        CombineStatuses(_storageService);
        if (HasErrors)
            return null;

        return dto;

    }
    public void SaveTemplate(CreateDocumentTempleteFileDlDto dto)
    {
        var entity = base.Create(dto);

        if (HasErrors)
            return;
        UnitOfWork.Save();
        _storageService.MoveToPersistent(DocumentStorageConst.SYS_DOCUMENT_TEMPLETE, dto.TableId.ToString(), dto.Id);
        CombineStatuses(_storageService);
    }

    //Print
    public async ValueTask<byte[]> Print<TEntity, TId>(TEntity model)
        where TEntity : class, IHaveIdProp<TId>
        where TId : struct
    {
        var template = Repository.DbSet.FirstOrDefault(f => f.TableId == 85);
        StorageFile file = null;
        if (template != null)
            file = _storageService.GetFile(DocumentStorageConst.SYS_DOCUMENT_TEMPLETE, template.TableId.ToString(), template.Id);
        //else 
        //var entity = model.GetType().GetInterface("ILinkToEntity").GenericTypeArguments.GetType();
        //var value = await UnitOfWork.Context.FindAsync(entity, model.Id);

        MemoryStream stream;
        if (file != null)
            stream = (MemoryStream)file.GetStream();
        else
            stream = new MemoryStream(WordFactory.GenerateWordTemplate(model));


        var result = WordFactory.PrintWordDocument(model, stream);

        //StorageFile[] temp = new StorageFile[1];
        //temp[0] = new(new StorageFileInfo() { FileName = "TempFile.docx" }, (Stream)result);
        //_storageService.SaveTemp("AutoSavedFile", temp);

        return await _pdfConverter.DocxToPdfAsync(result, new());
        //throw new NotImplementedException();
    }
    //
}
