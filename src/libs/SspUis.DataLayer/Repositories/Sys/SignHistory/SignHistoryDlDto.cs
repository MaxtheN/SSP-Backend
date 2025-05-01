using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class SignHistoryDlDto<TDto> : EntityDto<TDto, SignHistory>
        where TDto : SignHistoryDlDto<TDto>
    {
        public int TableId { get; set; }
        public long DocumentId { get; set; }
        public string DataForSign { get; set; }
        [LocalizedStringLength(50)]
        public string DocNumber { get; set; }
        public DateTime DocDate { get; set; }
        public string UserInfo { get; set; }
        public int StatusId { get; set; }
        public byte[] Pkcs7info { get; set; }
        public byte[] Pc7key { get; set; }
    }
}
