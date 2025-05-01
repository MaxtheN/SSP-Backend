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
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.PrtnCertificateServices
{
    public class PrtnCertificateDto : UpdatePrtnCertificateDlDto, ILinkToEntity<PrtnCertificate>, IDocument
    {
        public Guid Id2 { get; set; }
        public string PrtnContractType { get; set; }
        public DateOnly? CancelOn { get; set; }
        public int TableId { get; set; } = TableIdConst.DOC_PRTN_CERTIFICATE;
        public string Contractor { get; set; }
        public int TotalPostCount { get; set; }
        public int SuccessPostCount { get; set; }
        public string Status { get; set; }
        public bool CanCancel { get; set; }
        public long? CurrentPrtnCertificateSignId { get; set; }
    }
}
