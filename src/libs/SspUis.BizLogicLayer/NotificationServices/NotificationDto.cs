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

namespace SspUis.BizLogicLayer.NotificationServices
{
    public class NotificationDto : UpdateNotificationDlDto, ILinkToEntity<Notification>
    {
        public string Type { get; set; }
        public string DocStatus { get; set; }
        public string Table { get; set; }
    }
}
