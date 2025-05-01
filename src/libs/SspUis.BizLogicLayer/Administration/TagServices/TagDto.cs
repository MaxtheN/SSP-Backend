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

namespace SspUis.BizLogicLayer.TagServices
{
    public class TagDto : UpdateTagDlDto, ILinkToEntity<Tag>
    {
        public string State { get; set; }
        //new public List<TagTranslateDto> Translates { get; set; } = new();
    }
}
