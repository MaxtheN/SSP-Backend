using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using SspUis.Core.Security;
using AutoMapper;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdatePrtnCreditDemandDlDto : PrtnCreditDemandDlDto<UpdatePrtnCreditDemandDlDto>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public int StatusId { get; set; }
    }
}
