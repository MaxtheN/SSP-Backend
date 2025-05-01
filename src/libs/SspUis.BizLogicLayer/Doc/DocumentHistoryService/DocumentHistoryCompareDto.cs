using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.DocumentHistoryService
{
    public class DocumentHistoryCompareDto
    {
        public long TableId { get; set; }
        public string? PreviousDocContent { get; set; } = null!;
        public string? CurrentDocContent { get; set; } = null!;
    }
}
