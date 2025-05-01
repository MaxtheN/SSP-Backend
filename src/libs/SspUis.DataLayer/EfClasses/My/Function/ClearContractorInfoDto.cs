using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.DataLayer.EfClasses.My
{
    [Keyless]
    public class ClearContractorInfoDto
    {
        [Column("result_status")]
        public bool ResultStatus { get; set; }
    }
}
