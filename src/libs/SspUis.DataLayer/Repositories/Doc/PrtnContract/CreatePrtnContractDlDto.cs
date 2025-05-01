using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using WEBASE.EF;
using Newtonsoft.Json;

namespace SspUis.DataLayer.Repositories
{
    public class CreatePrtnContractDlDto : PrtnContractDlDto<CreatePrtnContractDlDto>
    {
        [JsonIgnore]
        public int OrganizationId { get; set; }
    }
}
