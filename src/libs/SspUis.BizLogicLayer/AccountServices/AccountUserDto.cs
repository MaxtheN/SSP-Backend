using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.AccountServices
{
    public class AccountUserDto : ILinkToEntity<User>, IHaveStateId
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Inn { get; set; }
        public string Pinfl { get; set; }
        public int? OrganizationId { get; set; }
        public int? OrganizationRegionId { get; set; }
        public int? OrganizationDistrictId { get; set; }
        public int? OrganizationGroupId { get; set; }
        public int? LanguageId { get; set; }
        public int StateId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Organization { get; set; }
        public int? PositionCategoryId { get; set; }
        public long? EmployeeManageId { get; set; }
        public string PositionCategory { get; set; }
        public string Language { get; set; }
        public Guid? PictureId { get; set; }
        public List<string> Modules { get; set; } = new();
        public List<string> Roles { get; set; } = new();
    }
}
