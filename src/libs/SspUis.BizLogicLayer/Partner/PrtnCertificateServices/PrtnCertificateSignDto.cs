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

namespace SspUis.BizLogicLayer.PrtnCertificateServices
{
    public class PrtnCertificateSignDto : ILinkToEntity<PrtnCertificateSign>
    {
        public long Id { get; set; }
        public long OwnerId { get; set; }
        public Guid SignFile { get; set; }
        public Guid DataFile { get; set; }
        public int? StatusId { get; set; }
        public string Status { get; set; }
        public string SignedUserInfo { get; set; }
        public DateTime? SignedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
