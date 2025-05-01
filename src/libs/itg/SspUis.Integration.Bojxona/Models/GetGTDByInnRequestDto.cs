using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Bojxona.Models
{
    public class GetGTDByInnRequestDto
    {
        [Required]
        [JsonProperty("stir")]
        public string Stir { get; set; }
        [Required]
        [JsonProperty("year")]
        public string Year { get; set; }
        [Range(1,int.MaxValue)]
        public int? CountryId { get; set; }
    }
}
