using SspUis.BizLogicLayer.Doc;
using System;

namespace SspUis.BizLogicLayer
{
    public class DocumentListDto<T> : IDocumentListModel
    {
        public T Id { get; set; }
        public int StatusId { get; set; }
        public DateOnly DocOn { get; set; }
     
    }
}
