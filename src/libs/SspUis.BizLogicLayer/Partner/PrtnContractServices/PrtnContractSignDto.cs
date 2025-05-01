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
    public class PrtnContractSignDto : ILinkToEntity<PrtnContractSign>
    {
        public long Id { get; set; }
        public int PrtnContractTypeTableId { get; set; }
        public int SignOrganizationTypeId { get; set; }
        public int OrganizationSignId { get; set; }
        public string OrganizationSignPinfl { get; set; }
        public int? StatusId { get; set; }
        public string Status { get; set; }
        public string SignedUserInfo { get; set; }
        public bool IsSigned { get; set; }
        public DateTime? SignedAt { get; set; }
        public int? OrganizationId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
