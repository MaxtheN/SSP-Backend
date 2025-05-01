using System.Collections;

namespace SspUis.BizLogicLayer.ReportServices
{
    public interface IMergeable
    {

        object Row { get; }

        IEnumerable SubRow { get;  }

        int SubRowCount { get; }

    }
}