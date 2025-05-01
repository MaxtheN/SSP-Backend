using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Interfaces;
using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.VideoLessonServices
{
    public class VideoLessonListDto : ILinkToEntity<VideoLesson>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public string OrderCode { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public string Theme { get; set; }
        public string Tag { get; set; }
        public string Uri { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
    }
}
