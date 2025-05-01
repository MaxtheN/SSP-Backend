using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.NewsTagServices
{
    public class NewsTagListDto : ILinkToEntity<NewsTag>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public int NewsId { get; set; }
        public string News { get; set; }
        public DateTime Date { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
    }
}
