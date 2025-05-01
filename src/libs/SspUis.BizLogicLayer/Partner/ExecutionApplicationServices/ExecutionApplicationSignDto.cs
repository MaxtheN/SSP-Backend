using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.Models;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SspUis.BizLogicLayer.PrtnContractServices
{
    public class ExecutionApplicationSignDto : ILinkToEntity<ExecutionApplicationSign>
    {
        public long Id { get; set; }
        public DateTime? SignedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
