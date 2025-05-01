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
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace SspUis.DataLayer.Repositories
{
    public class OrganizationSignDlDto : EntityDto<OrganizationSignDlDto, OrganizationSign>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int PrtnContractTypeTableId { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(14)]
        public string Pinfl { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string FirstName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string LastName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string MiddleName { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string PassportSeria { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string PassportNumber { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(50)]
        public string PhoneNumber { get; set; }
        public int? UserId { get; set; }
        [LocalizedRequired]
        public DateOnly BirthOn { get; set; }
        public DateOnly? ExpireOn { get; set; }
    }
}
