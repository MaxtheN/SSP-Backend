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
using SspUis.DataLayer.Interfaces;

namespace SspUis.BizLogicLayer.VideoLessonServices
{
    public class VideoLessonDto : UpdateVideoLessonDlDto, ILinkToEntity<VideoLesson>
    {
        public string Category { get; set; }
        public string State { get; set; }
    }
}
