using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    public interface IJobDocumentEntity : IHaveIdProp<long>
    {
        int TableId { get; set; }
        int? PrevStatusId { get; set; }
        int StatusId { get; set; }
        string Message { get; set; }
    }
}
