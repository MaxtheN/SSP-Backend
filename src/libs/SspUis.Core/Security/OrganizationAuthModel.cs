using System;

namespace SspUis.Core.Security
{
    public class OrganizationAuthModel
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int RegionId { get; set; }
        public int? DistrictId { get; set; }
        public int? SignOrganizationTypeId { get; set; }
        public int? OrganizationalStructureId { get; set; }
        public int OrganizationGroupId { get; set; }
        public int? ParentId { get; set; }
        public string Parent { get; set; }
        public string Inn { get; set; }
    }

    public class ContractorAuthModel
    {
        public long Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int CountryId { get; set; }
        public int RegionId { get; set; }
        public DateOnly RegistrationDate { get; set; }
        public string RegistrationNumber { get; set; }
        public int DistrictId { get; set; }
        public int? OkedId { get; set; }
        public string Pinfl { get; set; }
        public string Inn { get; set; }
        public string PhoneNumber { get; set; }
    }
}
