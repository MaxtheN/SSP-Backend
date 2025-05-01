using AutoMapper;
using SspUis.DataLayer.EfClasses;
using GenericServices;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WEBASE.Attributes;
using WEBASE.Models;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class OrganizationSettlementAccountDlDto : EntityDto<OrganizationSettlementAccountDlDto, OrganizationSettlementAccount>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(250)]
        public string AccountName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(20)]
        [LocalizedMinLength(20)]
        public string AccountCode { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int BankId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int StateId { get; set; }
    }
}
