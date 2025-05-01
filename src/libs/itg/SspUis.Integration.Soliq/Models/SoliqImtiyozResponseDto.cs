using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Soliq.Models
{
    public class SoliqImtiyozResponseDto
    {
        public bool Success { get; set; }
        public string Reason { get; set; }
        public Data Data { get; set; }
    }

    public class Data
    {
        public long CompanyTin { get; set; }
        public int Count { get; set; }
        public decimal Summa { get; set; }
    }
}
