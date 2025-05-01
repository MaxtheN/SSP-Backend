using SspUis.DataLayer.EfClasses;
using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.OrganizationServices
{
    public class OrganizationListDto : ILinkToEntity<Organization>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int StateId { get; set; }
        public string Inn { get; set; }
        public int? ParentId { get; set; }
        public string Parent { get; set; }
        public int CountryId { get; set; }
        public int RegionId { get; set; }
        public int? DistrictId { get; set; }
        public string Address { get; set; }
        public int? OkedId { get; set; }
        public string Director { get; set; }
        public string Accounter { get; set; }
        public string VatCode { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string OrderCode { get; set; }
        public int? SignOrganizationTypeId { get; set; }
        public string SignOrganizationType { get; set; }
        public int? OrganizationLegalFormId { get; set; }
        public string OrganizationLegalForm { get; set; }
        public int? OrganizationalStructureId { get; set; }
        public string OrganizationalStructure { get; set; }
        public string Email { get; set; }
        public int OrganizationGroupId { get; set; }

        public string Country { get; set; }
        public string OrganizationGroup { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string Oked { get; set; }
        public string State { get; set; }
    }
}
