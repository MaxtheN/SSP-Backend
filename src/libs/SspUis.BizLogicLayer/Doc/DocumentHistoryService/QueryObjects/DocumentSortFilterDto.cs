using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.DocumentHistoryService
{
    public class DocumentSortFilterDto : SortFilterPageOptions
    {
        public long DocId { get; set; }
        public int TableId { get; set; }
    }
}
