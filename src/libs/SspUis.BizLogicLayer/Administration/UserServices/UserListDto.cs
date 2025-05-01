using System.Collections.Generic;
using GenericServices;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.UserServices
{
    public class UserListDto : ILinkToEntity<User>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public int? OrganizationId { get; set; }
        public int? EmployeeId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string State { get; set; }
        public int StateId { get; set; }
        public List<string> Roles { get; set; } = new();
        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public List<int> UserRoles { get; set; } = new();
        public List<int> UserModels { get; set; } = new();
        public string Organization { get; set; }
        public string OrganizationInn { get; set; }
    }

}
