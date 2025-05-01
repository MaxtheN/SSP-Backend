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

namespace SspUis.BizLogicLayer.OkedServices
{
    public class OkedDto : UpdateOkedDlDto, ILinkToEntity<Oked>
    {
        public bool IsGroup { get; set; }
        public int? ParentId { get; set; }
        public int Level { get; set; }
        public string State { get; internal set; }
        public string Parent { get; set; }
        public string  OkedType { get; set; }
        
        new public List<OkedTranslateDto> Translates { get; set; } = new();
    }
}
