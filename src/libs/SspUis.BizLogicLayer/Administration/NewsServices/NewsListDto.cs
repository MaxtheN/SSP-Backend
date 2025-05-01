using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.NewsServices
{
    public class NewsListDto : ILinkToEntity<News>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortContent { get; set; }
        public DateTime Date { get; set; }
        public int ViewCount { get; set; }
        public Guid? ImageId { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
    }
}
