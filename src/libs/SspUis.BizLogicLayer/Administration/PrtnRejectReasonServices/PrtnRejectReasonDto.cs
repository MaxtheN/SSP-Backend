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

namespace SspUis.BizLogicLayer.PrtnRejectReasonServices
{
    public class PrtnRejectReasonDto : UpdatePrtnRejectReasonDlDto, ILinkToEntity<PrtnRejectReason>
    {
        public string State { get; internal set; }
        public string PrtnContractType { get; internal set; }
        public string PrtnContractTypeTable { get; internal set; }
        new public List<PrtnRejectReasonTranslateDto> Translates { get; set; } = new();
    }
}
