using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.NotificationServices
{
    public class NotificationListDto : ILinkToEntity<Notification>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int TypeId { get; set; }
        public int TableId { get; set; }
        public int DocStatusId { get; set; }
        public long DocId { get; set; }
        public string Type { get; set; }
        public string DocStatus { get; set; }
        public string Table { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
