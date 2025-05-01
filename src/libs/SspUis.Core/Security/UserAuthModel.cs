using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.Core.Security
{
    public class UserAuthModel
    {
        public int Id { get; set; }
        public string Inn { get; set; }
        public int PersonId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public IEnumerable<string> Modules { get; set; } = new List<string>();
        public bool IsAdmin { get; set; }
        public int? LanguageId { get; set; }
        public string LanguageCode { get; set; }
        public string Pinfl { get; set; }
        public int OrganizationId { get; set; }
        public int? PositionId { get; set; }
        public bool? IsHr { get; set; }
        public long? EmployeeManageId { get; set; }
        public string PhoneNumber { get; set; }
        //public void ResolveModules()
        //{
        //    if (IsAdmin)
        //        Modules = Enum.GetNames(typeof(ModuleCode)).ToHashSet();
        //}
        public void ResolveModules(bool? hasChastiment = null)
        {
            if (IsAdmin)
            {
                Modules = Enum.GetNames(typeof(ModuleCode)).ToHashSet();
            }
            else
            {
                if (hasChastiment == true)
                {
                    Modules = new List<string>
                    {
                            ModuleCode.ChastisementSign.ToString(),
                            ModuleCode.ChastisementSignerView.ToString()
                    };
                }
            }
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }

}
