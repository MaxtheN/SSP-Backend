using System;
using WEBASE;

namespace SspUis.BizLogicLayer
{
    public class DocumentSortFilterOptions : TableSortFilterPageOptions
    {
        public int[] StatusIds { get; set; }
        public DateOnly? FromDocDate { get; set; }
        public DateOnly? ToDocDate { get; set; }
    }
}
