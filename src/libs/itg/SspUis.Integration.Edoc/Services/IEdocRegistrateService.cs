using SspUis.Integration.Edoc.Models;
using StatusGeneric;
using WEBASE.Storage;

namespace SspUis.Integration.Edoc
{
    public interface IEdocRegistrateService : IStatusGeneric
    {
        Task<ExternalIncomingDocumentDto?> RegistrateEdoc(EdocRegisterRequestDto dto);
        Task UpdateEdoc(EdocRegisterRequestDto dto);
        Task<List<FileResultResponse>> UploadFile(byte[] file);
        Task<List<FileResultResponse>> UploadAttachment(StorageFile file);
        Task<ExternalIncomingDocumentDto> Get(long id);
        Task<ExternalIncomingDocumentDto> ForCallCenterGet(long id);
        Task<byte[]> DownloadAttachment(Guid fileId, bool isView);
    }
}
