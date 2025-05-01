using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Administration.CountryServices
{
    public class AsSelectListForBojxonaDto
    {
        [Required]
        public string Inn { get; set; }
        [Required]
        public int Year { get; set; }   
    }
}
