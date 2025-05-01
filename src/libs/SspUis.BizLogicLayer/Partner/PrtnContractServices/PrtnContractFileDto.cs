using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.PrtnContractServices
{
    public class PrtnContractFileDto: PrtnContractFileDlDto
    {
        public string FileName { get; internal set; }
        public DateTime CreatedAt { get; set; }
    }
}
