using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Doc.MonoApplicationServices
{
    public class MonoApplicationDocumentListDto<T> : IDocumentListModel, IMonoApplicationListModel
    {
        public T Id { get; set; }
        public int StatusId { get; set; }
        public DateOnly DocOn { get; set; }
        public int? BandlikResponseStatusId { get; set; }
        public string BandlikResponseStatus { get; set; }
    }
}
