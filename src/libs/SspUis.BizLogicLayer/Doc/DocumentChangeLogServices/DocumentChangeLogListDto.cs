using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.DocumentChangeLogServices
{
    public class DocumentChangeLogListDto :
        ILinkToEntity<DocumentChangeLog>, 
        IHaveIdProp<long>
    {
        public long Id { get; set; }
        public DateTime DateAt { get; set; }
        public string UserInfo { get; set; } = null!;
        public int TableId { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; }
        public string Table { get; set; }
        public string Status { get; set; }
        public string IpAddress { get; set; } = null!;
        public string UserAgent { get; set; } = null!;
        public string Message { get; set; }
		public long DocId { get; set; }
	}
}
