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
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.VideoCategoryServices
{
    public class VideoCategoryDto : UpdateVideoCategoryDlDto, ILinkToEntity<VideoCategory>
    {
        public string State { get; set; }
        new public List<VideoCategoryTranslateDto> Translates { get; set; } = new();
    }
}
