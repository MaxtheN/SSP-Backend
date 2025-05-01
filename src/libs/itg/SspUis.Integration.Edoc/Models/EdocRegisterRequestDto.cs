namespace SspUis.Integration.Edoc.Models
{
    public class EdocRegisterRequestDto
    {
        public Document Document { get; set; }
        public bool IsGenerate { get; set; }
        public DateTime DocDate { get; set; }
        public string? SenderName { get; set; }
        public string? DocNumber { get; set; }
        public int ExternalDocumentTypeId { get; set; }
        public string SpecialNotes { get; set; }
        public List<FileForEdoc> Files { get; set; }
        public List<AttachmentForEdoc> Attachments { get; set; }
    }
    public class AttachmentForEdoc
    {
        public Guid Id { get; set; }
    }
    public class FileForEdoc
    {
        public Guid Id { get; set; }
    }
    public class Document
    {
        public long DocumentTypeId { get; set; }
        public string? Summary { get; set; }
        public DateTime? TermExecution { get; set; }
        public long? ActivityFieldId { get; set; }
        public string? RegNumber { get; set; }
        public DateTime RegDate { get; set; }
        public int? ImportanceId { get; set; }
        public long? ExternalDocumentId { get; set; }
        public long? CallCenterAppealId { get; set; }
        public long? EmployeeManageId { get; set; }
    }
}
