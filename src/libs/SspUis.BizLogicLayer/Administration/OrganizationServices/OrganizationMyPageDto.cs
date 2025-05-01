using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.OrganizationServices
{
    public class OrganizationMyPageDto
    {
        public string Address { get; set; }
        public string Region { get; set; }
        public int RegionId { get; set; }
        public string? District { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public new List<OrganizationFileDto> Files { get; set; } = new();
    }
}
