using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.Models;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.RoleServices
{
    public class RoleDto : UpdateRoleDlDto, ILinkToEntity<Role>
    {
        public string State { get; internal set; }
        new public List<RoleTranslateDto> Translates { get; set; } = new();
    }
}
