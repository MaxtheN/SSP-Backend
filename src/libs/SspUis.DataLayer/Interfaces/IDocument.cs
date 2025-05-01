using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.DataLayer.Interfaces
{
    public interface IDocument
    {
        long Id { get; }
        string DocNumber { get; }
        DateTime DocOn { get; }
        int StatusId { get; }
    }
}
