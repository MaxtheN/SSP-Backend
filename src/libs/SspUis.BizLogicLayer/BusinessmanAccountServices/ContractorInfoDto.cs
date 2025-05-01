using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.BusinessmanAccountServices
{
    public class ContractorInfoDto
    {
        public DateOnly RegistrationDate { get; set; }
        public int DifferenceInYears { get; set; }
        public int DifferenceInMonths { get; set; }
        public int DifferenceInDays { get; set; }
        public bool IsBudget { get; set; }
        public bool MoreThan2Years { get; set; }
        public bool CanCreate { get; set; }
    }

    public class ContractorInfoListDto : ILinkToEntity<Contractor>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Inn { get; set; }
        public string Pinfl { get; set; }
        public int CountryId { get; set; }
        public string Country { get; set; }
        public int RegionId { get; set; }
        public string Region { get; set; }
        public int DistrictId { get; set; }
        public string District { get; set; }
        public string Director { get; set; }
        public DateOnly RegistrationDate { get; set; }
        public string RegistrationNumber { get; set; }
    }
}
