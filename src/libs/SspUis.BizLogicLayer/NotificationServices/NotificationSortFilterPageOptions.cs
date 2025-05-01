using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.NotificationServices
{
    public class NotificationSortFilterPageOptions : SortFilterPageOptions
    {
        public int? NotificationStatusId { get; set; }
    }
}
