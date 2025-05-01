using System;
using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class UpdateDocumentTempleteFileDlDto : DocumentTempleteFileDlDto<UpdateDocumentTempleteFileDlDto>, IHaveIdProp<Guid>
{

    [LocalizedRange(1, int.MaxValue)]
    public int StatusId { get; set; }
}
