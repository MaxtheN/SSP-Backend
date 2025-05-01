using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Hrm
{
    public class ClearTableDto
    {
        public long Id { get; set; }
        public List<long>? TableIds { get; set; } = null!;
        public bool? CreateLog { get; set; } = true;
    }
}
